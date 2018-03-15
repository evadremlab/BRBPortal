Imports System.Diagnostics
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Odbc
Imports System.Windows.Forms
Imports QSILib.Windows.Forms
Imports System.Reflection
'Imports IBM.Data.Informix
Imports System.IO
Imports QSILib
'Imports GemBox.Spreadsheet
Imports System.Net.Mail
Imports System.Data.OleDb
Imports System.Text
'Imports Microsoft.Office.Interop

Public Module qFunctions

    Private iTableBeingUpdated As DataTable = Nothing   'BHS 4/28/09 Allow SetTableIdentity to update each row in a Detail Save

#Region "------------------------ Documentation -----------------------"
    'Changes
    '12/8/06 BHS Add Enumerators and Control Management Functions
    '2/16/07 BHS Add Appl.ConnType="IFX"
    '8/23/07 BHS Added GVToExcel export based on GemBox tool
    '11/26/07 BHS Extended CreateFormByName to look at all Assemblies in Appl.AssemblyNames (also see GetEditForm)
    '2/12/08 BHS Added DA.FillSchema to BuildDA to get schema information including column.MaxLength
    '            Added logic in Binding of Text fields to set qTextBox.MaxLength
    '6/17/08 BHS CreateConnection always uses the SQL Server connection string
    '5/4/09 BHS Release 3.101 HOT FIX - don't try to add qRowID column if it is already in a table.  Also, always check that a 
    '           connection is closed before opening it.

    'BHSCONV Modify database IO to work against SQL Server only if Appl.ConnType = "SQL"
    '   Modified functions are in a Region called "Conversion to SQL Server"

    '6/28/12 BHS Change all ShowError calls to Throw New ApplicationException
    '5/22/13 DJW - Removed IfxToSQLSyntax from SQLBuildDV.
    '1/16/2014 GBV - Changed default for keeping connection open to False in SQLBuildDV


#End Region

#Region "------------------------ Conversion to SQL Server -----------------------"
    'BHSCONV
    ''' <summary> Given Informix SQL syntax, returns SQL Server SQL syntax </summary>
    Function IfxToSQLSyntax(ByVal aSQL As String) As String
        'BHS 2/25/17 - never convert - no IFX
        Return aSQL

        If ConnType = "IFX" Then Return aSQL
        If aSQL Is Nothing OrElse aSQL = "" Then Return aSQL

        aSQL = Replace(aSQL, "OTT:", "OTT..")
        aSQL = Replace(aSQL, "BK:", "BK..")
        aSQL = Replace(aSQL, "SF:", "SF..")
        aSQL = Replace(aSQL, "SD:", "SD..")
        aSQL = Replace(aSQL, "UCTT:", "UCTT..")
        aSQL = Replace(aSQL, "SQLPTS:", "SQLPTS..")
        aSQL = Replace(aSQL, "STARTUP2:", "STARTUP2..")
        aSQL = Replace(aSQL, "CPI:", "CPI..") ' GBV 4/23/2013
        'If aSQL.ToUpper.IndexOf("CREATE UNIQUE INDEX") = -1 Then ' GBV 2/5/2013
        '    aSQL = Replace(aSQL, " UNIQUE ", " DISTINCT ")
        'End If

        'EXPECT TO NEED TO FIX TODAY() and DATE(TODAY) - wait until we have an example.
        'aSQL = Clean(aSQL)  Don't want to double-clean, leading to 4 apostrophes.  We need a smarter routine.
        'aSQL = CleanAmpersand(aSQL)  "
        aSQL = DropDateFunction(aSQL)
        aSQL = Replace(aSQL, "||", " + ") 'Ifx to SQL concatenation character
        'BHS 6/24/13 enclose SQL Server reserved word in brackets
        aSQL = Replace(aSQL, " lineno", " [lineno]")
        aSQL = Replace(aSQL, ",lineno", ",[lineno]")
        aSQL = Replace(aSQL, "(lineno", "([lineno]")

        'SRM 07/24/2013 -- Do not replace "function" in the table "functionmesssage"
        'SDC 07/26/2013 -- Instead, only replace "function" in table "contact_list_order"
        'If aSQL.IndexOf("FunctionMessage") = -1 Then
        If aSQL.ToLower.IndexOf("contact_list_order") >= 0 Then
            aSQL = Replace(aSQL, " function", " [function]")
            aSQL = Replace(aSQL, ",function", ",[function]")
            aSQL = Replace(aSQL, "(function", "([function]")
        End If

        aSQL = ReplaceIfxTrim(aSQL)
        aSQL = ReplaceIfxBracketsWithSQLSubString(aSQL)
        aSQL = DropAVOID_INDEX(aSQL)
        aSQL = RemoveMYUPPER(aSQL)
        Return aSQL
    End Function

    'BHSCONV
    'Assumes no spaces in the phrase TRIM(
    Function ReplaceIfxTrim(ByVal aSQL As String,
                            Optional ByVal aStartPos As Integer = 0) As String
        If aSQL.ToUpper.IndexOf("TRIM(", aStartPos) = -1 Then Return aSQL

        'BHS 6/24/13 This routine won't work if LTRIM(RTRIM is already in the string (could happen if ReplaceIfxTrim is called twice)
        If aSQL.ToUpper.IndexOf("LTRIM(RTRIM(", aStartPos) > -1 Then Return aSQL

        'BHS 9/21/12 Search for FirstPos past aStartPos
        Dim FirstPos As Integer = aSQL.ToUpper.IndexOf("TRIM(", aStartPos)

        Dim SecondPos As Integer = aSQL.IndexOf(")", FirstPos) + 2
        Dim SQL As String =
           Mid(aSQL, 1, FirstPos) &
           " LTRIM(RTRIM(" &
           Mid(aSQL, FirstPos + 6, SecondPos - 7 - FirstPos) &
               ")) " &
           Mid(aSQL, SecondPos)

        'BHS 9/21/12 Add 8 to SecondPos to reflect "TRIM(" changed to  " LTRIM(RTRIM(" 
        SecondPos += 8

        Return ReplaceIfxTrim(SQL, SecondPos)  'Recursive
    End Function

    'BHSCONV
    ''' <summary> Remove Date function call from SQL call </summary>
    Function DropDateFunction(ByVal aSQL As String) As String
        If aSQL.ToUpper.IndexOf("DATE(") = -1 Then Return aSQL
        If aSQL.ToUpper.IndexOf("GETDATE(") > -1 Then Return aSQL ' GBV 1/25/2013

        Dim FirstPos As Integer = aSQL.ToUpper.IndexOf("DATE(")
        Dim SecondPos As Integer = aSQL.IndexOf(")", FirstPos) + 2
        If SecondPos < FirstPos Then Return aSQL

        Dim Phrase1 As String = Mid(aSQL, 1, FirstPos) & " "
        Dim Phrase2 As String = Mid(aSQL, FirstPos + 6, SecondPos - 7 - FirstPos)
        Dim Phrase3 As String = Mid(aSQL, SecondPos)
        Dim SQL As String = Phrase1 & Phrase2 & Phrase3

        Return DropDateFunction(SQL)    'recursive
    End Function

    'BHSCONV
    Function DropAVOID_INDEX(ByVal aSQL As String) As String
        If aSQL.ToUpper.IndexOf("{+AVOID_INDEX") = -1 Then Return aSQL

        Dim FirstPos As Integer = aSQL.ToUpper.IndexOf("{+AVOID_INDEX")
        Dim SecondPos As Integer = aSQL.IndexOf("}", FirstPos) + 2
        If SecondPos < FirstPos Then Return aSQL

        Dim SQL As String =
           Mid(aSQL, 1, FirstPos) &
           Mid(aSQL, SecondPos)
        Return SQL

    End Function

    'BHSCONV
    ''' <summary>Recursive function to change Informix [] phrases to SQL Substring function calls </summary>
    Function ReplaceIfxBracketsWithSQLSubString(ByVal aSQL As String) As String
        Dim StartBracketPos As Integer = aSQL.IndexOf("[") + 1  'To make pos 1 based, for use with mid
        If StartBracketPos <= 1 Then Return aSQL 'Bad syntax
        Dim CommaPos As Integer = aSQL.IndexOf(",", StartBracketPos) + 1
        If CommaPos < StartBracketPos Then Return aSQL 'Bad syntax
        Dim EndBracketPos As Integer = aSQL.IndexOf("]", CommaPos) + 1
        If EndBracketPos < StartBracketPos Then Return aSQL 'Bad syntax


        'Parse ColumnName before Start Bracket
        Dim ColumnNameStart As Integer = 0
        For i As Integer = (StartBracketPos - 1) To 0 Step -1   'StartBracketPos is 0 based and mid is 1 based
            Dim letter As String = Mid(aSQL, i, 1)
            'BHS 8/16/12 Added "(" to not include it in the column name
            If letter = " " Or letter = "," Or letter = "=" Or letter = "(" Then
                ColumnNameStart = i + 1 'not including the comma or space
                Exit For
            End If
        Next
        If ColumnNameStart <= 1 Then Return aSQL 'Bad syntax
        Dim ColumnName As String = Mid(aSQL, ColumnNameStart, StartBracketPos - ColumnNameStart)

        Dim StartArg As String = Mid(aSQL, StartBracketPos + 1, (CommaPos - 1 - StartBracketPos))
        Dim EndArg As String = Mid(aSQL, CommaPos + 1, (EndBracketPos - 1 - CommaPos))
        If Not IsNumeric(StartArg) Then Return aSQL 'Bad syntax
        If Not IsNumeric(EndArg) Then Return aSQL 'Bad syntax
        Dim SubstringLength As Integer = 1 + CInt(EndArg) - CInt(StartArg)


        Dim NewSQL As String =
            Mid(aSQL, 1, ColumnNameStart - 1) &
            " SUBSTRING(" & ColumnName & ", " & StartArg & ", " & SubstringLength.ToString & ")" &
            Mid(aSQL, EndBracketPos + 1)

        Return ReplaceIfxBracketsWithSQLSubString(NewSQL)   'recursive


    End Function

    'BHSCONV - convert Ifx syntax to SQL
    ''' <summary> SQL-Server-specific DoSQL full logic </summary>
    Function SQLDoSQL(ByVal acn As SqlConnection,
                      ByVal aSQL As String,
                      ByRef aErrorMessage As String,
                      Optional ByVal aKeepConnectionOpen As Boolean = False,
                      Optional ByVal aForceWriteToLive As Boolean = False) As Object

        'BHS 5/22/09 Don't allow Update, Delete or Insert command against live DB from Test NUI
        If aForceWriteToLive = False Then
            Dim CapsCommand As String = Trim(aSQL).ToUpper
            If CapsCommand.IndexOf("UPDATE ") > -1 Or
               CapsCommand.IndexOf("DELETE ") > -1 Or
               CapsCommand.IndexOf("INSERT ") > -1 Then
                If OKToWriteToDB() = False Then Throw New Exception("You may not write to the Live Database from a Test Application")
            End If
        End If
        '   If there is an SQL Error, it is implicitly returned in the ByRef aErrorMessage
        '   The result of the SQL statement is explicitly returned in ob
        aSQL = CheckSQLLength(aSQL, False)  'BHS 4/7/11

        'BHSCONV
        aSQL = IfxToSQLSyntax(aSQL)

        Dim cmd As SqlCommand = New SqlCommand(aSQL, acn)
        'cmd.CommandTimeout = 120 ' 120 seconds - GBV - 7/22/2011
        cmd.CommandTimeout = 3600 ' 1 hour BHS 12/7/12
        Dim ob As Object = New Object

        aErrorMessage = ""

        If acn.State = ConnectionState.Closed Then acn.Open()

        Try
            ob = cmd.ExecuteScalar()
        Catch ex As Exception
            aErrorMessage = "SQL Error on: (" & aSQL & ")  -  " & ex.ToString
            Throw New Exception("DoSQL SQL: " & Chr(13) & WrapString(aSQL, 132), ex)
        Finally
            If Not aKeepConnectionOpen Then acn.Close()
        End Try

        If Not aKeepConnectionOpen Then acn.Close()

        Return ob

    End Function

    'BHSCONV - added aKeepConnectionOpen and aGetSchema on 7/10/12
    'GBV 1/16/2014 - Changed a KeepConnectionOpen to False to avoid connection pooling issues
    ''' <summary> Full SQLBuildDV.  SQL Server </summary>
    Function SQLBuildDV(ByVal aSQL As String,
                        ByVal aCn As SqlConnection,
                        Optional ByVal aEmptyRow As Boolean = False,
                        Optional ByVal aType As String = "str",
                        Optional ByVal aKeepConnectionOpen As Boolean = False,
                        Optional ByVal aGetSchema As Boolean = False) As DataView


        'Fill dvdisc for dropdown
        aSQL = CheckSQLLength(aSQL, False) '5/14/08
        'BHSCONV
        'aSQL = PrepareSQLForStaticTest(aSQL)  Static DB not used in SQL Server

        'DJW 5/22/13 - Removed the following line as it is not needed in this routine.
        '              We should already have a real SQL query string at this point.

        ' GBV 8/20/2013 if this function is called directly, translate. Otherwise, do not translate
        Dim ST As New StackTrace
        Dim CallingFunction As String = ST.GetFrames(1).GetMethod.Name

        If CallingFunction = "SQLBuildDV" Then
            aSQL = IfxToSQLSyntax(aSQL)
        End If


        If aSQL.Length = 0 Then Return Nothing

        Dim DAD As SqlDataAdapter = New SqlDataAdapter(aSQL, aCn)
        Dim DSD As New DataSet

        Try
            '--- BHSCONV 7/10/12 from IfxBuildDV
            'If aGetSchema = True Then
            '    Throw New Exception("Programmer Error - use BuildDA to create an SQL " & _
            '                        "Data Adapter, not IfxBuildDV")
            '    Return Nothing
            'End If
            'DAD.SelectCommand.CommandTimeout = 5400
            DAD.SelectCommand.CommandTimeout = 3600 ' 1 hour BHS 12/7/12

            '--- End
            If aGetSchema = True Then
                DAD.MissingSchemaAction = MissingSchemaAction.AddWithKey ' GBV 4/4/2013
            End If

            DAD.Fill(DSD)
            TrimTable(DSD.Tables(0))

            If aEmptyRow Then
                ''Assumes single column key in the first field
                'Dim DR As DataRow = DSD.Tables(0).NewRow
                'If Left(aType.ToLower, 3) = "str" Then
                '    DR.Item(0) = ""
                'Else
                '    DR.Item(0) = 0
                'End If
                'DSD.Tables(0).Rows.InsertAt(DR, 0)
                'Assumes single column key in the first field
                Dim DR As DataRow = DSD.Tables(0).NewRow
                If Left(aType.ToLower, 3) = "str" Then
                    DR.Item(0) = ""
                    'BHS 4/14/2010
                ElseIf Left(aType.ToLower, 3) = "dat" Then
                    DR.Item(0) = DBNull.Value
                Else
                    'DR.Item(0) = 0
                    'BHS 9/15/10
                    DR.Item(0) = DBNull.Value  ' SRM 9/1/10
                End If
                DSD.Tables(0).Rows.InsertAt(DR, 0)
            End If

            Return DSD.Tables(0).DefaultView

        Catch ex As Exception
            '    TryError("Problem building data view (Functions:SQLBuildDV(" & aSQL & " | " & aCn.ConnectionString + ")", ex)

            Throw New ApplicationException("SQLBuildDV SQL: " & WrapString(aSQL, 140), ex)

            '--- BHSCONV 7/10/12 From IfxBuildDV
        Finally
            If aKeepConnectionOpen = False And DAD.SelectCommand.Connection.State = ConnectionState.Open Then
                DAD.SelectCommand.Connection.Close()
            End If
            DAD.Dispose()  'OK if we're only returning a DV
            '--- End

        End Try

    End Function

    'BHSCONV
    'Note, this was changed before 7/21/09, since we knew we had to use ODBC to build DataAdapters with command builders
    '''' <summary>
    '''' Informix Build DA.  Fills Connection, Fills DA Update/Insert/Delete commands, Fills the Table Schema, and Loads data into aDS.
    '''' Note we use an ODBC connection, not IFX, so commands get built correctly.  You may want to use the same 
    '''' connection for all tables to be saved, to keep them in one transaction. 
    '''' You may want to use gODBCConnStr to specify the ODBC connection parameters, in which case
    '''' aDB (database) is reqd (OTT, BK, SD, SF, GLA)
    '''' Note, you may optionally specify a Command SQL that deals only with the table to be updated</summary>
    'Public Function IfxBuildDA(ByVal aSQL As String, _
    '                           ByRef aDS As DataSet, _
    '                           ByVal aTableName As String, _
    '                           Optional ByRef aODBCCn As OdbcConnection = Nothing, _
    '                           Optional ByVal aCnStr As String = "", _
    '                           Optional ByVal aDB As String = "", _
    '                           Optional ByVal aCommandSQL As String = "", _
    '                           Optional ByVal aFillSchema As Boolean = True, _
    '                           Optional ByRef aFrm As fBase = Nothing) As OdbcDataAdapter
    '    Dim DA As OdbcDataAdapter = Nothing

    '    Try
    '        aSQL = CheckSQLLength(aSQL) '5/14/08
    '        If aSQL.Length = 0 Then Return Nothing 'BHS 4/27/09

    '        'BHSCONV
    '        If ConnType <> "IFX" Then
    '            If aFrm IsNot Nothing Then
    '                aSQL = CheckSQLLength(aSQL, False) '5/14/08
    '                aSQL = IfxToSQLSyntax(aSQL)

    '                If aDB = "" Then
    '                    MsgBox("PROGRAMMER ERROR - Running against SQL Server requires " & _
    '                           "an aDB parameter in IfxBuildDA call, so we know which " & _
    '                           "database we're working against.")
    '                    Return Nothing
    '                End If

    '                'BHS 5/15/12, since iSQLCn is always set to SQLPTS in OnLoad, always change it here.
    '                Dim CnStr As String = gSQLConnStr
    '                If aDB.Length > 0 Then
    '                    CnStr = Replace(CnStr, "catalog=SQLPTS", "catalog=" & aDB)
    '                    CnStr = Replace(CnStr, "catalog=DMS", "catalog=" & aDB)
    '                End If

    '                aFrm.iSQLCn = New SqlConnection(CnStr)

    '                'BHSCONV
    '                'If aTableName not yet in iSQLDATableNames, add it
    '                If aFrm.GetSQLDANo(aTableName) = 0 Then
    '                    aFrm.iSQLDATableNames.Add(aTableName)
    '                End If

    '                Select Case aFrm.GetSQLDANo(aTableName)
    '                    Case 1
    '                        aFrm.iSQLDA1 = BuildDA(aSQL, aDS, aTableName, aFrm.iSQLCn, aCommandSQL, aFillSchema, CnStr, aDB)
    '                    Case 2
    '                        aFrm.iSQLDA2 = BuildDA(aSQL, aDS, aTableName, aFrm.iSQLCn, aCommandSQL, aFillSchema, CnStr, aDB)
    '                    Case 3
    '                        aFrm.iSQLDA3 = BuildDA(aSQL, aDS, aTableName, aFrm.iSQLCn, aCommandSQL, aFillSchema, CnStr, aDB)
    '                    Case 4
    '                        aFrm.iSQLDA4 = BuildDA(aSQL, aDS, aTableName, aFrm.iSQLCn, aCommandSQL, aFillSchema, CnStr, aDB)
    '                    Case 5
    '                        aFrm.iSQLDA5 = BuildDA(aSQL, aDS, aTableName, aFrm.iSQLCn, aCommandSQL, aFillSchema, CnStr, aDB)
    '                    Case 6
    '                        aFrm.iSQLDA6 = BuildDA(aSQL, aDS, aTableName, aFrm.iSQLCn, aCommandSQL, aFillSchema, CnStr, aDB)
    '                    Case 7
    '                        aFrm.iSQLDA7 = BuildDA(aSQL, aDS, aTableName, aFrm.iSQLCn, aCommandSQL, aFillSchema, CnStr, aDB)
    '                    Case 8
    '                        aFrm.iSQLDA8 = BuildDA(aSQL, aDS, aTableName, aFrm.iSQLCn, aCommandSQL, aFillSchema, CnStr, aDB)
    '                    Case 9
    '                        aFrm.iSQLDA9 = BuildDA(aSQL, aDS, aTableName, aFrm.iSQLCn, aCommandSQL, aFillSchema, CnStr, aDB)

    '                    Case Else
    '                        MsgBox("PROGRAMMER ERROR - iSQLDATableNames array error")
    '                End Select

    '            Else
    '                MsgBox("PROGRAMMER ERROR - SQLDA parameters missing aFrm")
    '            End If
    '            'aFrm.GetSQLDA(aTableName) = BuildDA(aSQL, aDS, aTableName, aFrm.iSQLCn, aCommandSQL, aFillSchema, aCnStr, aDB)
    '            Return New OdbcDataAdapter("SELECT 'junk'", gODBCConnStr)
    '        End If


    '        'BHSCONV - here is the old Informix-oriented code:

    '        'If optional aCommandSQL is specified, use it to build commands
    '        Dim SQL As String = aSQL
    '        If aCommandSQL.Length > 0 Then SQL = aCommandSQL




    '        If aODBCCn Is Nothing Then aODBCCn = New Odbc.OdbcConnection(SetODBCDatabase(aCnStr, aDB)) '...BHS 05/15/2009
    '        DA = New OdbcDataAdapter(SQL, aODBCCn)  'BHS 1/22/09 changed from aSQL

    '        'BHS 5/22/09 Only allow Update, Delete or Insert command against live DB if not in Test NUI
    '        If OKToWriteToDB() = True Then
    '            Dim CB As New Odbc.OdbcCommandBuilder(DA)
    '            CB.ConflictOption = ConflictOption.OverwriteChanges 'Update and Delete based on key value only.  GBV 8/9/2011
    '            'CB.ConflictOption = ConflictOption.CompareAllSearchableValues  BHS 9/28/11
    '            CB.SetAllValues = False                             'Update only fields that have been changed

    '            DA.UpdateCommand = CB.GetUpdateCommand
    '            DA.InsertCommand = CB.GetInsertCommand
    '            DA.DeleteCommand = CB.GetDeleteCommand
    '        End If

    '        DA.InsertCommand.UpdatedRowSource = UpdateRowSource.FirstReturnedRecord 'BHS 3/10/09

    '        'After commands are built, use the main SQL for filling data
    '        DA.SelectCommand.CommandText = aSQL

    '        If aODBCCn.State = ConnectionState.Closed Then aODBCCn.Open() 'BHS 5/4/09
    '        DA.MissingSchemaAction = MissingSchemaAction.AddWithKey
    '        'ODBCDoSQL(aODBCCn, "Set Isolation Dirty Read", "")  'BHS 4/22/09
    '        Dim cmdDirtyRead As New OdbcCommand("Set Isolation Dirty Read", aODBCCn) 'BHS 4/27/09
    '        cmdDirtyRead.ExecuteNonQuery()
    '        DA.Fill(aDS, aTableName)
    '        'BHS 8/19/10 Trim informix data coming in
    '        TrimTable(aDS.Tables(aTableName))

    '        'BHS 10/19/10
    '        If aFillSchema = True Then
    '            DA.FillSchema(aDS, SchemaType.Mapped, aTableName) 'BHS 2/12/08 Get Schema Data also, to set Textbox. MaxLength
    '            'BHS 9/13/10 Remove related table constraints
    '            If aCommandSQL > "" Then
    '                'If aCommandSQL.ToLower.IndexOf(" where ") > -1 Then  'Assumes aCommandSQL ends in Where Clause
    '                '    aCommandSQL &= " AND 1 = 2"
    '                'Else
    '                '    aCommandSQL &= " WHERE 1 = 2"
    '                'End If

    '                'BHS 2/2/11 tolerate Group By, Having, and Order By
    '                aCommandSQL = AddWhereClause(aCommandSQL, "1 = 2")

    '                RemoveRelatedTableConstraints(aDS.Tables(aTableName), IfxBuildDV(aCommandSQL, False, "str", SetODBCDatabase(aCnStr, aDB), True, True, False))
    '            End If
    '        End If


    '        ' GBV 9/9/10 changed strategy to deal with additional columns
    '        'If aCommandSQL <> "" Then
    '        '    Dim wrksql As String = aCommandSQL.ToLower
    '        '    wrksql = wrksql.Substring(0, wrksql.IndexOf("where") - 1) & " WHERE 1 = 2"
    '        '    Dim DV As DataView = IfxBuildDV(wrksql, False, "str", SetODBCDatabase(aCnStr, aDB), True, True, False)
    '        '    For Each C As DataColumn In aDS.Tables(aTableName).Columns
    '        '        If Not DV.Table.Columns.Contains(C.ColumnName) Then
    '        '            C.ReadOnly = False
    '        '            C.AllowDBNull = True
    '        '            C.Unique = False
    '        '            If C.DataType Is GetType(String) Then
    '        '                C.MaxLength = 500
    '        '            End If
    '        '        End If
    '        '    Next
    '        'End If


    '        If aDS.Tables(aTableName).Columns.Item("qRowID") Is Nothing Then    'BHS 5/4/09
    '            aDS.Tables(aTableName).Columns.Add("qRowID", GetType(System.Int32), "0")    'BHS 4/28/09 Add integer column named qRowID
    '        End If

    '    Catch ex As Exception
    '        'Ignore abandoned mutex
    '        If ex.Message.IndexOf("abandoned mutex") = -1 Then
    '            'BHS 6/28/12
    '            Throw New ApplicationException("IfxBuildDA SQL: " & WrapString(aSQL, 140), ex)
    '            'ShowError("IfxBuildDA", ex)
    '        End If

    '    Finally
    '        'BHSCONV
    '        If aODBCCn IsNot Nothing AndAlso aODBCCn.State = ConnectionState.Open Then
    '            aODBCCn.Close()
    '        End If
    '    End Try

    '    Return DA
    'End Function

    ''BHSCONV - Added aCnStr and aDB, to receive info from IfxBuildDA

    ''' <summary> Build Data Adapter for data entry. Note aCn should be iSQLCn, so all DAs share a connection for Transaction purposes.
    ''' Use aCommandSQL to specify a simpler SQL for Insert, Delete and Update statements than used in the original Select.  SQL Server </summary>
    Function BuildDA(ByVal aSQL As String,
                     ByRef aDS As DataSet,
                     ByVal aTableName As String,
                     Optional ByRef aCn As SqlConnection = Nothing,
                     Optional ByVal aCommandSQL As String = "",
                     Optional ByVal aFillSchema As Boolean = True,
                     Optional ByVal aCnStr As String = "",
                     Optional ByVal aDB As String = "") As SqlDataAdapter

        'BHS 6/28/12 Added Try
        Try


            'BHSCONV 5/29/12 changed aCN to Optional ByRef parameter
            aSQL = CheckSQLLength(aSQL, False) '5/14/08

            'If optional aCommandSQL is specified, use it to build commands
            Dim SQL As String = aSQL
            If aCommandSQL.Length > 0 Then SQL = aCommandSQL

            'BHSCONV 5/30/12 set aCommandSQL so it will be used to RemoveRelatedTableConstraints
            aCommandSQL = SQL

            'BHSCONV - set connection string, and apply it to aCn 
            If aCnStr = "" Then
                If aCn IsNot Nothing Then
                    aCnStr = aCn.ConnectionString
                Else
                    aCnStr = gSQLConnStr
                End If
            End If

            If aDB.Length > 0 Then
                'If SQLPTS and aDB is supplied, sustitute aDB for SQLPTS
                aCnStr = Replace(aCnStr, "catalog=SQLPTS", "catalog=" & aDB)
                aCnStr = Replace(aCnStr, "catalog=DMS", "catalog=" & aDB)
            End If

            If aCn Is Nothing Then
                aCn = New SqlConnection(aCnStr)
            Else
                If aCn IsNot Nothing AndAlso aCn.State = ConnectionState.Open Then aCn.Close()
                aCn.ConnectionString = aCnStr
            End If
            If aCn.State = ConnectionState.Closed Then aCn.Open()

            SQL = IfxToSQLSyntax(SQL)

            Dim DA As SqlDataAdapter = New SqlDataAdapter(SQL, aCn)

            'BHS 5/22/09 Only allow Update, Delete or Insert command against live DB if not in Test NUI
            If OKToWriteToDB() = True Then
                Dim CB As SqlCommandBuilder = New SqlCommandBuilder(DA)
                CB.ConflictOption = ConflictOption.OverwriteChanges 'BHS 3/24/08 Optimistic concurrency
                'CB.ConflictOption = ConflictOption.CompareAllSearchableValues ' GBV 8/12/2011  turned off BHS 9/28/11
                CB.SetAllValues = False
                DA.UpdateCommand = CB.GetUpdateCommand
                DA.InsertCommand = CB.GetInsertCommand
                DA.DeleteCommand = CB.GetDeleteCommand
            End If

            'After commands are built, use the main SQL for filling data
            DA.SelectCommand.CommandText = aSQL
            If aCn.State = ConnectionState.Closed Then aCn.Open() 'BHS 5/4/09
            DA.Fill(aDS, aTableName)
            'BHSCONV
            TrimTable(aDS.Tables(aTableName))

            'BHS 10/19/10
            If aFillSchema = True Then
                DA.FillSchema(aDS, SchemaType.Mapped, aTableName) 'BHS 2/12/08 Get Schema Data also, to set Textbox. MaxLength
                'BHS 9/13/10 Remove related table constraints
                If aCommandSQL > "" Then
                    'BHS 2/2/11 tolerate Group By, Having, and Order By
                    aCommandSQL = AddWhereClause(aCommandSQL, "1 = 2")

                    'If aCommandSQL.ToLower.IndexOf(" where ") > -1 Then  'Assumes aCommandSQL ends in Where Clause
                    '    aCommandSQL &= " AND 1 = 2"
                    'Else
                    '    aCommandSQL &= " WHERE 1 = 2"
                    'End If
                    'BHS 2/2/11 Changed BuildDV to SQLBuildDV

                    'BHSCONV change gSQLConnStr to aCn
                    'RemoveRelatedTableConstraints(aDS.Tables(aTableName), SQLBuildDV(aCommandSQL, gSQLConnStr))
                    'RemoveRelatedTableConstraints(aDS.Tables(aTableName), SQLBuildDV(aCommandSQL, aCn))
                End If
            End If

            If aDS.Tables(aTableName).Columns.Item("qRowId") Is Nothing Then    'BHS 5/4/09
                aDS.Tables(aTableName).Columns.Add("qRowID", GetType(System.Int32), "0")    'BHS 4/28/09 Add integer column named qRowID
            End If

            'aCn.Close()
            Return DA
        Catch ex As Exception
            Throw New ApplicationException("BuildDA SQL: " & WrapString(aSQL, 140), ex)
        Finally
            aCn.Close()
        End Try
    End Function

    'BHS 7/21/09 Change IfxConnection to ODBCConnection
    ' TODO aErrorMessage should be dropped in the future, since errors now bubble to client.  Try block is fine as it is
    ''' <summary> DoSQL.  IFX </summary>
    Function IfxDoSQL(ByVal aCn As OdbcConnection,
                      ByVal aSQL As String,
                      ByRef aErrorMessage As String,
                      Optional ByVal aForceWriteToLive As Boolean = False,
                      Optional ByVal aKeepConnectionOpen As Boolean = False) As Object

        'BHS 5/22/09 Don't allow Update, Delete or Insert command against live DB from Test NUI
        If aForceWriteToLive = False Then
            Dim CapsCommand As String = Trim(aSQL).ToUpper
            If CapsCommand.IndexOf("UPDATE ") > -1 Or
               CapsCommand.IndexOf("DELETE ") > -1 Or
               CapsCommand.IndexOf("INSERT ") > -1 Then
                If OKToWriteToDB() = False Then Throw New Exception("You may not write to the Live Database from a Test Application")
            End If
        End If
        aSQL = CheckSQLLength(aSQL) '5/14/08
        If aSQL.Length = 0 Then Return Nothing 'BHS 4/27/09

        If ConnType <> "IFX" Then
            aSQL = IfxToSQLSyntax(aSQL)
            Return SQLDoSQL(aSQL)
        End If


        aSQL = PrepareSQLForStaticTest(aSQL)
        '   If there is an SQL Error, it is implicitly returned in the ByRef aErrorMessage
        '   The result of the SQL statement is explicitly returned in ob

        Dim cmd As OdbcCommand = New OdbcCommand(aSQL, aCn)
        cmd.CommandTimeout = 5400    '90 minutes
        Dim ob As Object = New Object

        aErrorMessage = ""

        Try
            If aCn.State = ConnectionState.Closed Then aCn.Open()
            'Dim cmdDirtyRead As New IfxCommand("Set Isolation Dirty Read", aCn) 'BHS 4/27/09
            'cmdDirtyRead.ExecuteNonQuery()
            ob = cmd.ExecuteScalar()

        Catch ex As Exception When ex.Message.IndexOf("abandoned mutex") > -1   'Ignore abandoned mutex
            'Add SQL to the exception, save the original as an inner exception
        Catch ex As Exception
            aErrorMessage = "SQL Error on: (" & aSQL & ")  -  " & ex.Message
            If ex.InnerException IsNot Nothing Then     'BHS 11/12/09
                aErrorMessage &= Chr(13) & Chr(13) & ex.InnerException.ToString
            End If
            Throw New Exception("IFXDoSQL SQL: " & Chr(13) & WrapString(aSQL, 132), ex)

        Finally
            If aKeepConnectionOpen = False Then
                'BHSCONV
                If aCn IsNot Nothing AndAlso aCn.State = ConnectionState.Open Then
                    aCn.Close()
                End If
            End If
        End Try

        Return ob

    End Function


    'BHSCONV - branch to SQL Server based on Appl.ConnType
    'BHS 7/21/09 New Version doesn't allow Ifx connections
    'Drop aODBC parameter
    'WARNING - If you copy this to a hotfix, make sure to change the HotFix BackgroundWork call to IfxBuildDV
    'BHS 7/7/09 provide option to use ODBCDataAdapter
    ''' <summary> Create a dataview, including an possible empty row on top.  IFX </summary>
    Function IfxBuildDV(ByVal aSQL As String,
                        ByVal aEmptyRow As Boolean,
                        Optional ByVal aType As String = "str",
                        Optional ByVal aCnStr As String = "",
                        Optional ByVal aKeepConnectionOpen As Boolean = False,
                        Optional ByVal aGetSchema As Boolean = False,
                        Optional ByVal aUsegODBCCn As Boolean = True) As DataView

        'BHSCONV 7/10/12
        If Appl.ConnType <> "IFX" Then
            aCnStr = gSQLConnStr    'Can't use connection string designed for IFX
            Dim Cn As New SqlConnection(aCnStr)

            'DJW 5/22/13 Convert syntax prior to calling SQLBuildDV
            aSQL = IfxToSQLSyntax(aSQL)

            'BHSCONV Final two parameters require additions to SQLBuildDV
            Return SQLBuildDV(aSQL, Cn, aEmptyRow, aType, aKeepConnectionOpen, aGetSchema)
        End If


        aSQL = CheckSQLLength(aSQL) '5/14/08
        aSQL = PrepareSQLForStaticTest(aSQL)    'BHS 12/8/08 Change references from OTT and UCTT to OTS and UCS
        If aSQL.Length = 0 Then Return Nothing 'BHS 4/27/09
        Dim DV As DataView = Nothing
        'Dim DR As DataRow
        If aCnStr = "" Then aCnStr = gAppConnStr 'BHS 7/21/09 This is now an ODBC connection string
        Dim ODBCDA As OdbcDataAdapter = Nothing

        Try
            Dim DSD As New DataSet
            'BHS 12/7/09 allow explicit instruction to not use gODBCCn
            If aCnStr = gAppConnStr And aUsegODBCCn = True Then ' GBV 7/23/2009
                If gODBCCn Is Nothing Then gODBCCn = New OdbcConnection(gAppConnStr) ' GBV 7/23/2009
                ODBCDA = New OdbcDataAdapter(aSQL, gODBCCn)
            Else
                ODBCDA = New OdbcDataAdapter(aSQL, aCnStr)
            End If
            If aGetSchema Then
                ODBCDA.MissingSchemaAction = MissingSchemaAction.AddWithKey ' GBV 8/17/2009
            End If
            ODBCDA.SelectCommand.CommandTimeout = 5400
            If ODBCDA.SelectCommand.Connection.State = ConnectionState.Closed Then ODBCDA.SelectCommand.Connection.Open()
            ODBCDA.Fill(DSD)
            TrimTable(DSD.Tables(0))
            If aEmptyRow Then
                '    'Assumes single column key in the first field
                '    DR = DSD.Tables(0).NewRow
                '    DR.Item(0) = System.DBNull.Value    'BHS 11/26/07 Need to test populations with EmptyRow = True
                '    DSD.Tables(0).Rows.InsertAt(DR, 0)
                'End If
                'Assumes single column key in the first field
                Dim DR As DataRow = DSD.Tables(0).NewRow
                If Left(aType.ToLower, 3) = "str" Then
                    DR.Item(0) = ""
                    'BHS 4/14/2010
                ElseIf Left(aType.ToLower, 3) = "dat" Then
                    DR.Item(0) = DBNull.Value
                Else
                    'DR.Item(0) = 0
                    'BHS 9/15/10
                    DR.Item(0) = DBNull.Value  ' SRM 9/1/10
                End If
                DSD.Tables(0).Rows.InsertAt(DR, 0)
            End If


            DV = DSD.Tables(0).DefaultView

        Catch ex As Exception When ex.Message.IndexOf("abandoned mutex") > -1   'Ignore abandoned mutex
            'Add SQL to the exception, save the original as an inner exception
            'BHS 11/3/09 Ignore link failure:
        Catch ex As Exception When ex.Message.IndexOf("Communication link failure") > -1

        Catch ex As Exception When ex.Message.ToLower.IndexOf("the connection has been disabled") > -1

        Catch ex As Exception
            Throw New ApplicationException("IFXBuildDV SQL: " & WrapString(aSQL, 140), ex)

        Finally

            If aKeepConnectionOpen = False And ODBCDA.SelectCommand.Connection.State = ConnectionState.Open Then
                ODBCDA.SelectCommand.Connection.Close()
            End If
            ODBCDA.Dispose()

        End Try

        Return DV

    End Function

    ''' <summary>
    ''' GBV 12/13/2012. 
    ''' Do a transaction, given an array of SQL statements. SQL server.
    ''' The only difference with DoSQLTran is that this function supports 
    ''' DROP (table, index, etc.), or SP_RENAME (tables, etc), whereas
    ''' DoSQLTran does not.
    ''' </summary>
    ''' <param name="aSQL">A previously populated ArrayList, with a single SQL statement per item</param>
    ''' <param name="aErrorMessage">String variable used to return error messages</param>
    ''' <param name="aCN">An optional SQL Connection object. If "Nothing" it creates one</param>
    ''' <param name="aT">An optional SQL transaction object. If "Nothing" it creates one</param>
    ''' <returns>True or False</returns>
    ''' <remarks>There are situations where you need to call this function several times
    ''' before committing. In that case, pass a connection and a transaction, and when
    ''' done, commit the transaction in your code</remarks>
    Public Function DoSQLTranScalar(ByVal aSQL As ArrayList,
                              ByRef aErrorMessage As String,
                              Optional ByVal aCN As SqlConnection = Nothing,
                              Optional ByVal aT As SqlTransaction = Nothing) As Boolean
        'BHS 5/22/09 No activity with transactions is allowed for Live DB in a Test Version of NUI
        If OKToWriteToDB() = False Then Throw New Exception("You may not write to the Live Database from a Test Application")

        Dim T As SqlTransaction = Nothing   '=Nothing BHS 5/26/06
        If aT IsNot Nothing Then T = aT
        Dim cmd As SqlCommand = New SqlCommand
        cmd.CommandTimeout = 3600 ' 1 hour BHS 12/7/12

        Dim SQLResult As Object
        Dim cn As SqlConnection
        If aCN IsNot Nothing Then
            cn = aCN
        Else
            cn = New SqlConnection(Appl.gSQLConnStr)   'BHS 8/22/08
        End If

        Dim i As Integer

        If aT Is Nothing Then
            BeginTran(cn, T, cmd)
        Else    'BHS 10/6/08 Need to set up command without doing a begin transaction
            If cn.State = ConnectionState.Closed Then cn.Open()
            cmd = cn.CreateCommand()
            'cmd.CommandTimeout = 120 ' 120 seconds - GBV - 7/22/2011
            cmd.CommandTimeout = 3600 ' 1 hour BHS 12/7/12
            cmd.Transaction = aT
        End If


        For i = 0 To aSQL.Count() - 1
            'SRM -- Removed becuase this is an SQL only function
            'aSQL(i) = IfxToSQLSyntax(aSQL(i).ToString)


            cmd.CommandText = CheckSQLLength(aSQL(i).ToString, False)   'BHS 4/7/11 added CheckSQLLength

            SQLResult = TranSQLScalar(T, cmd, aErrorMessage)

            If IsDBNull(SQLResult) Then Return False 'Rollback happened in TranSQLScalar

        Next

        If aT Is Nothing Then T.Commit() 'BHS 9/29/08  Don't finish transaction if it was open when we started
        Return True
    End Function



    'BHSCONV
    'BHS 4/4/08
    ''' <summary> Do a Transaction, given array of SQL strings. SQL Server. </summary>
    Public Function DoSQLTran(ByVal aSQL As ArrayList,
                              ByRef aErrorMessage As String,
                              Optional ByVal aCN As SqlConnection = Nothing,
                              Optional ByVal aT As SqlTransaction = Nothing) As Boolean

        'BHS 5/22/09 No activity with transactions is allowed for Live DB in a Test Version of NUI
        If OKToWriteToDB() = False Then Throw New Exception("You may not write to the Live Database from a Test Application")

        Dim T As SqlTransaction = Nothing   '=Nothing BHS 5/26/06
        If aT IsNot Nothing Then T = aT
        Dim cmd As SqlCommand = New SqlCommand
        'cmd.CommandTimeout = 120 ' 120 seconds - GBV - 7/22/2011
        cmd.CommandTimeout = 3600 ' 1 hour BHS 12/7/12

        Dim SQLResult As Integer
        Dim cn As SqlConnection
        If aCN IsNot Nothing Then
            cn = aCN
        Else
            'cn = CreateConnection()
            cn = New SqlConnection(Appl.gSQLConnStr)   'BHS 8/22/08
        End If

        Dim i As Integer

        If aT Is Nothing Then
            BeginTran(cn, T, cmd)
        Else    'BHS 10/6/08 Need to set up command without doing a begin transaction
            If cn.State = ConnectionState.Closed Then cn.Open()
            cmd = cn.CreateCommand()
            'cmd.CommandTimeout = 120 ' 120 seconds - GBV - 7/22/2011
            cmd.CommandTimeout = 3600 ' 1 hour BHS 12/7/12
            cmd.Transaction = aT
        End If


        For i = 0 To aSQL.Count() - 1
            'BHSCONV
            aSQL(i) = IfxToSQLSyntax(aSQL(i).ToString)


            cmd.CommandText = CheckSQLLength(aSQL(i).ToString, False)   'BHS 4/7/11 added CheckSQLLength

            SQLResult = TranSQL(T, cmd, aErrorMessage)

            If SQLResult < 0 Then Return False 'Rollback happened in TranSQL

        Next

        If aT Is Nothing Then T.Commit() 'BHS 9/29/08  Don't finish transaction if it was open when we started
        Return True

    End Function

    'BHSCONV add Optional ByRef aFrm
    '''<summary> Informix SaveTable2 opens a tran if needed, and does deletes before adds.  Rollback if prob.  
    ''' Leave to calling program to commit so multiple tables can be updated in one transaction. 
    ''' This routine is called from IfxSaveTable in fBase </summary>
    Function IfxSaveTable2(ByVal aDA As OdbcDataAdapter,
                           ByVal aT As DataTable,
                           ByVal aTableType As String,
                           Optional ByRef aCn As OdbcConnection = Nothing,
                           Optional ByRef aTran As OdbcTransaction = Nothing,
                           Optional ByVal aIsCron As Boolean = False,
                           Optional ByRef aAcceptConcurrency As Boolean = True,
                           Optional ByRef aFrm As fBase = Nothing) As Boolean
        'BHS 9/30/11 Changed aAcceptConcurrency to True, to avoid non-key concurrency checking by default
        Dim TD, TM, TA As DataTable
        Dim Str, Err As String

        'BHSCONV
        If ConnType <> "IFX" Then
            If aFrm IsNot Nothing AndAlso aFrm.GetSQLDA(aT.TableName) IsNot Nothing Then
                Return SaveTable2(aFrm.GetSQLDA(aT.TableName), aT, aTableType, aFrm.iSQLCn, aFrm.iSQLTran, aIsCron, aAcceptConcurrency)
            Else
                MsgBox("Programmer Error - SaveTable2.aFrm parameter required for ConnType = SQL.", MsgBoxStyle.Exclamation, "Programmer Error")
                Return False
            End If
        End If


        If aDA.UpdateCommand Is Nothing Then
            Throw New Exception("You may not write to the Live Database from a Test Application")
        End If

        'Set up connection and Transaction if not open yet
        If aCn Is Nothing Then
            aCn = aDA.UpdateCommand.Connection
        End If
        If aCn.State = ConnectionState.Closed Then aCn.Open()
        If aTran Is Nothing Then aTran = aCn.BeginTransaction()

        'Assign iSQLTransaction to aDA commands
        aDA.UpdateCommand.Transaction = aTran
        aDA.InsertCommand.Transaction = aTran
        aDA.DeleteCommand.Transaction = aTran
        aDA.InsertCommand.UpdatedRowSource = UpdateRowSource.FirstReturnedRecord 'BHS 3/10/09

        Try

            If aTableType = "D" Then
                'iTableBeingUpdated = aT
                TD = aT.GetChanges(DataRowState.Deleted)
                TM = aT.GetChanges(DataRowState.Modified)
                TA = aT.GetChanges(DataRowState.Added)

                'Delete detail records
                If Not TD Is Nothing Then
                    aDA.Update(TD)
                    TD.Dispose()
                End If

                'Add detail records
                If Not TA Is Nothing Then
                    aDA.Update(TA)
                    TA.Dispose()
                End If

                'Modify detail records
                If Not TM Is Nothing Then
                    aDA.Update(TM)
                    TM.Dispose()
                End If

            Else

                'Update master record
                'iTableBeingUpdated = Nothing
                aDA.Update(aT)
                'iODBCTran.Rollback()    'BHS 7/9/08 DEBUG

            End If

        Catch dbcx As Data.DBConcurrencyException
            'BHS 6/28/12 Removed special concurrency logic - not used in current scheme
            Throw New ApplicationException("Database Concurrency Exception", dbcx)

            'BHS 6/28/12 - Save the following commented code for possible future use:
            'If Not aIsCron Then
            '    Dim MsgResult As MsgBoxResult
            '    If aAcceptConcurrency Then
            '        MsgResult = MsgBoxResult.Yes
            '    Else
            '        MsgResult = MsgBox("The record you are attempting to save has been changed by another user. " & vbCrLf & _
            '                           "Do you want to save any way?", MsgBoxStyle.YesNo)
            '    End If
            '    If MsgResult = MsgBoxResult.Yes Then
            '        aAcceptConcurrency = True
            '        'Get the record from database and "merge" it with the memory table, preserving changes.
            '        ' Then do the update.
            '        Dim Table As DataTable = aT.Clone
            '        aDA.SelectCommand.Transaction = aTran
            '        aDA.Fill(Table)
            '        If aTableType = "M" Then
            '            If Table.Rows.Count = aT.Rows.Count Then
            '                aT.Merge(Table, True)
            '                aDA.Update(aT)
            '                Return True
            '            Else
            '                'BHS 6/28/12
            '                Throw New ApplicationException("Unable to update: database record was deleted by another user", dbcx)
            '                Return False
            '            End If
            '        Else
            '            Dim TMod As DataTable = aT.GetChanges(DataRowState.Modified)
            '            If TMod IsNot Nothing Then
            '                If Table.Rows.Count = aT.Rows.Count Then
            '                    TMod.Merge(Table, True)
            '                    aDA.Update(TMod)
            '                    TMod.Dispose()
            '                    Return True
            '                Else
            '                    'BHS 6/28/12
            '                    Throw New ApplicationException("Unable to update: database record(s) either added or deleted by another user", dbcx)
            '                    Return False
            '                End If
            '            End If
            '        End If
            '    Else
            '        Str = RollBackTran(aTran)
            '        aTran.Dispose()
            '        Err = "Update Cancelled due to Concurrency"
            '        If Str <> "" Then Err &= " *** AND ROLLBACK EXCEPTION TOO. "

            '        'BHS 6/28/12
            '        Throw New ApplicationException(Err, dbcx)

            '        Return False
            '    End If
            'Else
            '    Throw
            'End If



        Catch ex As Exception
            Err = ex.ToString
            Str = RollBackTran(aTran)
            aTran.Dispose()
            If Str > " " Then Err += " *** AND ROLLBACK EXCEPTION TOO: " + Str

            If Err.ToLower.IndexOf("could not do a physical-order read to fetch next row") > -1 Then
                Err = "Database Save Failed: The database record you're trying to write to is locked.  " &
                          "Your changes have not been saved to the database.  Please try again later.  " & Chr(13) & Chr(13) & Err
            End If

            Throw New ApplicationException(Err, ex)

            Return False

        End Try

        'TrimTable(aT)

        Return True

    End Function

    'BHSCONV
    ''' <summary>Set SQL Database - Use this to change an SQL connection string 
    ''' from the default' Catalog=SQLPTS </summary>
    Public Function SetSQLDatabase(ByVal aConnString As String,
                                    ByVal aDB As String) As String
        Dim pointer As Integer = aConnString.ToLower.IndexOf("connection=sqlpts")
        Dim Str As String = Mid(aConnString, 1, pointer) & "connection=" & aDB
        If aConnString.Length > pointer + 17 Then
            Str &= Mid(aConnString, pointer + 17)
        End If
        Return Str
    End Function

    'BHSCONV Add parameters to handle aSQLCn and aSQLTran

    ''' <summary> Do a Transaction, given array of SQL strings. ODBC and SQL connections/transactions can be specified, to be run based on Appl.ConnType </summary>
    Public Function ODBCDoSQLTran(ByVal aSQL As ArrayList,
                                  ByRef aErrorMessage As String,
                                  Optional ByVal aCn As OdbcConnection = Nothing,
                                  Optional ByVal aT As OdbcTransaction = Nothing,
                                  Optional ByVal aNoRollBackIfLocked As Boolean = False,
                                  Optional ByVal aSQLCn As SqlConnection = Nothing,
                                  Optional ByVal aSQLTran As SqlTransaction = Nothing) As Boolean

        If OKToWriteToDB() = False Then Throw New Exception("You may not write to the Live Database from a Test Application")

        'BHSCONV
        If Appl.ConnType = "SQL" Then
            Return DoSQLTran(aSQL, aErrorMessage, aSQLCn, aSQLTran)
        End If

        Dim T As OdbcTransaction = Nothing   '=Nothing BHS 5/26/06
        If aT IsNot Nothing Then T = aT

        Dim cmd As OdbcCommand = New OdbcCommand
        Dim SQLResult As Integer

        'Dim cn As OdbcConnection = New OdbcConnection(My.Settings.appConnStr)
        Dim cn As OdbcConnection
        If aCn IsNot Nothing Then
            cn = aCn
        Else
            cn = New OdbcConnection(gODBCConnStr)
        End If

        Dim i As Integer

        'ODBCBeginTran(aCn, T, cmd)

        If aT Is Nothing Then
            ODBCBeginTran(cn, T, cmd)
        Else    'BHS 10/6/08 Need to set up command without doing a begin transaction
            If cn.State = ConnectionState.Closed Then cn.Open()
            cmd = cn.CreateCommand()
            cmd.Transaction = aT
        End If

        For i = 0 To aSQL.Count() - 1
            cmd.CommandText = aSQL(i).ToString

            SQLResult = ODBCTranSQL(T, cmd, aErrorMessage, aNoRollBackIfLocked) ' GBV 2/18/2011

            If SQLResult < 0 Then Return False 'Rollback happened in TranSQL

        Next

        If aT Is Nothing Then T.Commit() 'BHS 9/29/08  Don't finish transaction if it was open when we started
        'T.Commit()
        Return True

    End Function


    'BHSCONV Add parameters to allow aSQLCn and aSQLTran

    ''' <summary> Add a single SQL to an existing Transaction. ODBC connection and transaction are required. SQL connection/transaction can be specified, to be run based on Appl.ConnType </summary>
    ''' GBV - 8/18/2011 - Added aNoRollBackIfLocked to skip rollbacks if Informix record is locked
    Public Function ODBCDoSQLTran(ByVal aSQL As String,
                                  ByRef aErrorMessage As String,
                                  ByVal aCn As OdbcConnection,
                                  ByVal aT As OdbcTransaction,
                                  Optional ByVal aNoRollBackIfLocked As Boolean = False,
                                  Optional ByVal aSQLCn As SqlConnection = Nothing,
                                  Optional ByVal aSQLTran As SqlTransaction = Nothing) As Boolean

        If OKToWriteToDB() = False Then Throw New Exception("You may not write to the Live Database from a Test Application")


        Dim SQLs As New ArrayList()
        SQLs.Add(aSQL)

        'BHSCONV
        If Appl.ConnType = "SQL" Then
            Return DoSQLTran(SQLs, aErrorMessage, aSQLCn, aSQLTran)
        End If

        Return ODBCDoSQLTran(SQLs, aErrorMessage, aCn, aT, aNoRollBackIfLocked)


        'BHS 8/17/12 - remove this logic - just share the SQL Array logic above
        'Dim T As OdbcTransaction = Nothing
        'If aT IsNot Nothing Then T = aT

        'Dim cmd As OdbcCommand = New OdbcCommand
        'Dim SQLResult As Integer

        'Dim cn As OdbcConnection

        ''BHS 8/17/12
        'If aCn IsNot Nothing Then
        '    cn = aCn
        'Else
        '    cn = New OdbcConnection(gAppConnStr)
        'End If

        ''BHS 8/17/12
        'If aT Is Nothing Then
        '    ODBCBeginTran(cn, T, cmd)
        'Else
        '    If cn.State = ConnectionState.Closed Then cn.Open()
        '    cmd = cn.CreateCommand()
        '    cmd.Transaction = aT
        'End If

        ''BHS 8/17/12
        'cmd.CommandTimeout = 5400   '90 minutes

        'cmd.CommandText = aSQL
        'SQLResult = ODBCTranSQL(T, cmd, aErrorMessage, aNoRollBackIfLocked) ' GBV 2/18/2011

        'If SQLResult < 0 Then Return False 'Rollback happened in TranSQL
        'Return True
    End Function

    'BHSCONV Add aSQLCn and aSQLTran as parameters
    '''' <summary> Do a Transaction, given array of SQL strings. ODBC and SQL Connection/Transactions may be specified, to be used based on Appl.ConnType. </summary>
    'Public Function IfxDoSQLTran(ByVal aSQL As ArrayList, _
    '                             Optional ByVal aCN As OdbcConnection = Nothing, _
    '                             Optional ByVal aT As OdbcTransaction = Nothing, _
    '                             Optional ByVal aSQLCn As SqlConnection = Nothing, _
    '                             Optional ByVal aSQLTran As SqlTransaction = Nothing) As Boolean
    '    Dim err As String = ""
    '    Return IfxDoSQLTran(aSQL, err, aCN, aT, aSQLCn, aSQLTran)
    'End Function

    ''BHSCONV Add aSQLCn and aSQLTran as parameters
    '''' <summary> Do a Transaction, given array of SQL strings. ODBC and SQL Connection/Transactions may be specified, to be used based on Appl.ConnType. </summary>
    'Public Function IfxDoSQLTran(ByVal aSQL As ArrayList, ByRef aErrorMessage As String, _
    '                             Optional ByVal aCn As OdbcConnection = Nothing, _
    '                             Optional ByVal aT As OdbcTransaction = Nothing, _
    '                             Optional ByVal aSQLCn As SqlConnection = Nothing, _
    '                             Optional ByVal aSQLTran As SqlTransaction = Nothing) As Boolean

    '    'BHSCONV 10/17/12 
    '    If ConnType = "SQL" Then
    '        Return DoSQLTran(aSQL, aSQLCn, aSQLTran)
    '    End If

    '    Dim T As OdbcTransaction = Nothing   '=Nothing BHS 5/26/06
    '    If aT IsNot Nothing Then T = aT

    '    Dim cmd As OdbcCommand = New OdbcCommand
    '    Dim SQLResult As Integer
    '    Dim cn As OdbcConnection
    '    If aCn IsNot Nothing Then
    '        cn = aCn
    '    Else
    '        cn = New OdbcConnection(gAppConnStr)
    '    End If
    '    'Dim cn As IfxConnection = New IfxConnection(gAppConnStr)
    '    Dim i As Integer

    '    Try

    '        If aT Is Nothing Then
    '            ODBCBeginTran(cn, T, cmd)
    '        Else    'BHS 10/6/08 Need to set up command without doing a begin transaction
    '            If cn.State = ConnectionState.Closed Then cn.Open()
    '            cmd = cn.CreateCommand()
    '            cmd.Transaction = aT
    '        End If

    '        'IfxBeginTran(cn, T, cmd)
    '        cmd.CommandTimeout = 5400   '...90 minutes

    '        For i = 0 To aSQL.Count() - 1
    '            cmd.CommandText = CheckSQLLength(aSQL(i).ToString)  '5/14/08

    '            SQLResult = IfxTranSQL(T, cmd, aErrorMessage)

    '            If SQLResult < 0 Then Return False 'Rollback happened in IfxTranSQL

    '        Next

    '        If aT Is Nothing Then T.Commit() 'BHS 9/29/08  Don't finish transaction if it was open when we started
    '        'T.Commit()
    '        'BHS 6/24/08 Ifx Transaction doesn't have a dispose, so we dispose the command and connection objects instead...
    '    Finally
    '        If aCn Is Nothing Then cn.Dispose() 'GBV 5/20/2010
    '        cmd.Dispose()   'BHS 6/24/08
    '    End Try
    '    Return True

    'End Function

    'BHSCONV 1/18/13 Convert to All-SQL
    'BHS 7/20/09 ODBCBuildDV based on IfxBuildDV
    ''' <summary> Create a dataview, including an possible empty row on top.  IFX </summary>
    Function ODBCBuildDV(ByVal aSQL As String,
                        ByVal aEmptyRow As Boolean,
                        Optional ByVal aType As String = "str",
                        Optional ByRef aODBCCn As OdbcConnection = Nothing,
                        Optional ByVal aKeepConnectionOpen As Boolean = False,
                        Optional ByVal aGetSchema As Boolean = False) As DataView

        'BHS 1/18/13 Call IfxBuildDV for standard All-SQL logic
        Dim ConnStr As String = Nothing
        If aODBCCn IsNot Nothing Then
            If aODBCCn.ConnectionString IsNot Nothing AndAlso
                aODBCCn.ConnectionString.Length > 0 Then
                ConnStr = aODBCCn.ConnectionString
            End If
        End If
        Return IfxBuildDV(aSQL, aEmptyRow, aType, ConnStr, aKeepConnectionOpen, aGetSchema)

        'The following code replaced by above call, 1/18/13:

        'aSQL = CheckSQLLength(aSQL) '5/14/08
        'aSQL = PrepareSQLForStaticTest(aSQL)    'BHS 12/8/08 Change references from OTT and UCTT to OTS and UCS
        'If aSQL.Length = 0 Then Return Nothing 'BHS 4/27/09
        'Dim DV As DataView = Nothing
        'Dim DR As DataRow
        ''If aODBCCn Is Nothing Then aODBCCn = New OdbcConnection(gODBCConnStr) GBV 7/23/2009
        'If gODBCCn Is Nothing Then gODBCCn = New OdbcConnection(gAppConnStr)
        'If aODBCCn Is Nothing Then aODBCCn = gODBCCn
        'Dim DAD As IfxDataAdapter = Nothing
        'Dim ODBCDA As OdbcDataAdapter = Nothing

        'Try
        '    Dim DSD As New DataSet

        '    ODBCDA = New OdbcDataAdapter(aSQL, aODBCCn)
        '    If aGetSchema Then
        '        ODBCDA.MissingSchemaAction = MissingSchemaAction.AddWithKey
        '    End If
        '    ODBCDA.SelectCommand.CommandTimeout = 5400
        '    If ODBCDA.SelectCommand.Connection.State = ConnectionState.Closed Then ODBCDA.SelectCommand.Connection.Open()
        '    ODBCDA.Fill(DSD)

        '    TrimTable(DSD.Tables(0))
        '    'DAD.Dispose()
        '    If aEmptyRow Then
        '        'Assumes single column key in the first field
        '        DR = DSD.Tables(0).NewRow
        '        DR.Item(0) = System.DBNull.Value    'BHS 11/26/07 Need to test populations with EmptyRow = True
        '        DSD.Tables(0).Rows.InsertAt(DR, 0)
        '    End If

        '    'DAD.Dispose()   'BHS 3/8/07
        '    DV = DSD.Tables(0).DefaultView

        'Catch ex As Exception When ex.Message.IndexOf("abandoned mutex") > -1   'Ignore abandoned mutex
        '    'Add SQL to the exception, save the original as an inner exception
        'Catch ex As Exception
        '    Throw New ApplicationException("OBCBuildDV SQL: " & WrapString(aSQL, 140), ex)

        'Finally
        '    If aKeepConnectionOpen = False And ODBCDA.SelectCommand.Connection.State = ConnectionState.Open Then
        '        ODBCDA.SelectCommand.Connection.Close()
        '    End If
        '    ODBCDA.Dispose()

        'End Try

        'Return DV

    End Function

    'BHSCONV 1/21/13 Added aKeepConnectionOpen and aForceWriteToLive
    ''' <summary> Full SQLGetNumber, return 0 if null result.  SQL Server. </summary>
    Function SQLGetNumber(ByVal aCn As SqlConnection,
                          ByVal aSQL As String,
                          ByVal aErr As String,
                          Optional aKeepConnectionOpen As Boolean = False,
                          Optional aForceWriteToLive As Boolean = False) As Decimal
        Dim ob As Object

        ob = SQLDoSQL(aCn, aSQL, aErr, aKeepConnectionOpen, aForceWriteToLive)

        If aErr > "" Then
            ErrMsg("Problem executing SQL", aErr)
            Return 0
        End If

        If IsDBNull(ob) Or IsNothing(ob) Then Return 0
        If Not IsNumeric(ob) Then Return 0

        Return CType(ob, Decimal)

    End Function

    'BHSCONV 1/21/13 
    ''' <summary>Write to a temporary table, based on whether ConnType = "SQL" or not </summary>
    Public Function WriteTemp(aSQLWithPound As String,
                              Optional aSQLCn As SqlConnection = Nothing,
                              Optional aKeepConnectionOpen As Boolean = True,
                              Optional aForceWriteToLive As Boolean = True) As Boolean
        If aSQLCn Is Nothing Then aSQLCn = gSQLCn
        If ConnType = "SQL" Then
            aSQLWithPound = IfxToSQLSyntax(aSQLWithPound)
            SQLDoSQL(aSQLWithPound, aSQLCn, aKeepConnectionOpen, aForceWriteToLive)
        Else
            Dim SQL As String = Replace(aSQLWithPound, "#", "")
            'IfxDoSQL(SQL, aForceWriteToLive)
        End If
    End Function

    'BHSCONV 1/22/13
    ''' <summary>BuildDV with Temporary Table - handles Ifx and SQL syntax</summary>
    Public Function BuildDVTemp(aSQLWithPound As String,
                                 Optional aSQLCn As SqlConnection = Nothing,
                                 Optional aEmptyRow As Boolean = False,
                                 Optional aType As String = "str",
                                 Optional aKeepConnectionOpen As Boolean = True,
                                 Optional aGetSchema As Boolean = False,
                                 Optional aIfxCn As OdbcConnection = Nothing) As DataView
        If aSQLCn Is Nothing Then aSQLCn = gSQLCn
        If ConnType = "SQL" Then
            aSQLWithPound = IfxToSQLSyntax(aSQLWithPound)
            Return SQLBuildDV(aSQLWithPound, aSQLCn, aEmptyRow, aType, aKeepConnectionOpen, aGetSchema)
        Else
            Dim SQL As String = Replace(aSQLWithPound, "#", "")
            'Note IfxBuildDV call does not support an explicit connection, so we always use the default
            Return IfxBuildDV(SQL, aEmptyRow, aType, , aKeepConnectionOpen, aGetSchema)
        End If

    End Function


#End Region

#Region "------------------------ Binding Routines -----------------------"

    ''' <summary> Bind Controls from BS </summary>
    Sub BindControls(ByRef aC As Control, ByRef aBS As BindingSource)

        ' GBV 4/23/2015 - Ticket 1508
        If aC.DataBindings.Count > 0 Then
            If aC.DataBindings(0).BindingManagerBase.IsBindingSuspended Then
                aC.DataBindings(0).BindingManagerBase.ResumeBinding()
            End If
        Else
            BindControl(aC, aBS)
        End If


        'Recursive
        For Each C As Control In aC.Controls
            BindControls(C, aBS)
        Next

    End Sub

    'BHS 2/9/10 New from Oakland:
    ''' <summary> Bind Controls for a specified table </summary>
    Sub BindControls(ByRef aC As Control, ByVal aTableName As String, ByRef aBs As BindingSource)

        ' GBV 4/23/2015 - Ticket 1508
        If aC.DataBindings.Count > 0 Then
            If aC.DataBindings(0).BindingManagerBase.IsBindingSuspended Then
                aC.DataBindings(0).BindingManagerBase.ResumeBinding()
            End If
        Else
            If TypeOf (aC) Is qTextBox Then
                Dim BindDef As String = CType(aC, qTextBox)._BindDef
                If BindDef <> "" AndAlso BindDef.IndexOf(".") > -1 Then
                    If BindDef.Substring(0, BindDef.IndexOf(".")).ToLower = aTableName.ToLower Then
                        CType(aC, qTextBox)._BindDef = BindDef.Substring(aTableName.Length + 1)
                        BindControl(aC, aBs)
                    End If
                End If
            End If
            If TypeOf (aC) Is qMaskedTextBox Then
                Dim BindDef As String = CType(aC, qMaskedTextBox)._BindDef
                If BindDef <> "" AndAlso BindDef.IndexOf(".") > -1 Then
                    If BindDef.Substring(0, BindDef.IndexOf(".")).ToLower = aTableName.ToLower Then
                        CType(aC, qMaskedTextBox)._BindDef = BindDef.Substring(aTableName.Length + 1)
                        BindControl(aC, aBs)
                    End If
                End If
            End If
            If TypeOf (aC) Is qComboBox Then
                Dim BindDef As String = CType(aC, qComboBox)._BindDef
                If BindDef <> "" AndAlso BindDef.IndexOf(".") > -1 Then
                    If BindDef.Substring(0, BindDef.IndexOf(".")).ToLower = aTableName.ToLower Then
                        CType(aC, qComboBox)._BindDef = BindDef.Substring(aTableName.Length + 1)
                        BindControl(aC, aBs)
                    End If
                End If
            End If
            If TypeOf aC Is qRC Then
                Dim BindDef As String = CType(aC, qRC)._BindDef
                If BindDef <> "" AndAlso BindDef.IndexOf(".") > -1 Then
                    If BindDef.Substring(0, BindDef.IndexOf(".")).ToLower = aTableName.ToLower Then
                        CType(aC, qRC)._BindDef = BindDef.Substring(aTableName.Length + 1)
                        BindControl(aC, aBs)
                    End If
                End If
            End If
            If TypeOf (aC) Is qCheckBox Then
                Dim BindDef As String = CType(aC, qCheckBox)._BindDef
                If BindDef <> "" AndAlso BindDef.IndexOf(".") > -1 Then
                    If BindDef.Substring(0, BindDef.IndexOf(".")).ToLower = aTableName.ToLower Then
                        CType(aC, qCheckBox)._BindDef = BindDef.Substring(aTableName.Length + 1)
                        BindControl(aC, aBs)
                    End If
                End If
            End If
            If TypeOf (aC) Is qDD Then
                Dim BindDef As String = CType(aC, qDD)._BindDef
                If BindDef <> "" AndAlso BindDef.IndexOf(".") > -1 Then
                    If BindDef.Substring(0, BindDef.IndexOf(".")).ToLower = aTableName.ToLower Then
                        CType(aC, qDD)._BindDef = BindDef.Substring(aTableName.Length + 1)
                        BindControl(aC, aBs)
                    End If
                End If
            End If
        End If


        'Recursive
        For Each C As Control In aC.Controls
            BindControls(C, aTableName, aBs)
        Next
    End Sub

    'PROGRAMMER'S NOTE - IF YOU MAKE CHANGES HERE, CONSIDER MAKING THEM IN qDR.ItemValueNeeded or .ItemValuePushed as well
    ''' <summary> Called Recursively from BindControls, sets up default binding for q Controls 
    ''' including Formatting as data arrives from the datatable and Parsing as data is sent back to the datatable </summary>
    Sub BindControl(ByRef aC As Control,
                   ByRef aBS As BindingSource)
        'qTextBox
        If TypeOf (aC) Is qTextBox Then
            Dim qT As qTextBox = CType(aC, qTextBox)
            If qT._BindDef.Length > 0 Then
                If Not HasBindingAlready(qT.DataBindings, "text") Then
                    If qT._DataType = DataTypeEnum.Dat Then
                        Dim B As Binding = CreateBinding("Text", aBS, qT._BindDef)
                        B.FormatString = "MM/dd/yyyy" 'BHS 9/18/09
                        B.FormattingEnabled = True
                        AddHandler B.Format, AddressOf DBdate2Textbox
                        AddHandler B.Parse, AddressOf TextBox2DBdate
                        qT.DataBindings.Add(B)
                    ElseIf qT._DataType = DataTypeEnum.Num Then
                        Dim B As Binding = CreateBinding("Text", aBS, qT._BindDef)
                        AddHandler B.Format, AddressOf DBNum2Textbox
                        AddHandler B.Parse, AddressOf TextBox2DBNum
                        qT.DataBindings.Add(B)
                    Else
                        Dim B As Binding = CreateBinding("Text", aBS, qT._BindDef)
                        AddHandler B.Format, AddressOf DBChar2Textbox
                        qT.DataBindings.Add(B)


                        'qT.DataBindings.Add("text", aBS, qT._BindDef)
                        'BHS 2/12/08 Assign textbox.maxlength from schema
                        Dim T As DataTable = TryCast(aBS.DataSource, DataTable)
                        If T Is Nothing Then
                            Dim DS As DataSet = TryCast(aBS.DataSource, DataSet)
                            If DS IsNot Nothing Then
                                T = TryCast(DS.Tables(aBS.DataMember), DataTable)
                            End If
                        End If
                        If T IsNot Nothing Then
                            Dim ColName As String = qT._BindDef
                            Dim i As Integer = ColName.IndexOf(".")
                            ColName = Mid(ColName, i + 2)
                            Dim C As DataColumn = T.Columns(ColName)
                            If C.MaxLength > -1 And qT.MaxLength > 32000 Then
                                qT.MaxLength = C.MaxLength
                            End If
                        End If
                    End If
                End If
            End If
        End If

        'qRC
        If TypeOf (aC) Is qRC Then
            Dim RC As qRC = CType(aC, qRC)
            If RC._BindDef.Length > 0 Then
                If Not HasBindingAlready(RC.DataBindings, "_DBText") Then
                    If RC._DataType = DataTypeEnum.Dat Then
                        Dim B As Binding = CreateBinding("_DBText", aBS, RC._BindDef)
                        B.FormatString = "MM/dd/yyyy" 'BHS 9/18/09
                        B.FormattingEnabled = True
                        AddHandler B.Format, AddressOf DBdate2Textbox
                        AddHandler B.Parse, AddressOf TextBox2DBdate
                        RC.DataBindings.Add(B)
                    ElseIf RC._DataType = DataTypeEnum.Num Then
                        Dim B As Binding = CreateBinding("_DBText", aBS, RC._BindDef)
                        AddHandler B.Format, AddressOf DBNum2Textbox
                        AddHandler B.Parse, AddressOf TextBox2DBNum
                        RC.DataBindings.Add(B)
                    Else
                        Dim B As Binding = CreateBinding("_DBText", aBS, RC._BindDef)
                        AddHandler B.Format, AddressOf DBChar2Textbox
                        RC.DataBindings.Add(B)

                        'RC.DataBindings.Add("text", aBS, RC._BindDef)

                        'Maxlength doesn't apply to qRC
                        'BHS 2/12/08 Assign textbox.maxlength from schema
                        'Dim T As DataTable = TryCast(aBS.DataSource, DataTable)
                        'If T Is Nothing Then
                        '    Dim DS As DataSet = TryCast(aBS.DataSource, DataSet)
                        '    If DS IsNot Nothing Then
                        '        T = TryCast(DS.Tables(aBS.DataMember), DataTable)
                        '    End If
                        'End If
                        'If T IsNot Nothing Then
                        '    Dim ColName As String = RC._BindDef
                        '    Dim i As Integer = ColName.IndexOf(".")
                        '    ColName = Mid(ColName, i + 2)
                        '    Dim C As DataColumn = T.Columns(ColName)
                        '    If C.MaxLength > -1 And RC.MaxLength > 32000 Then
                        '        RC.MaxLength = C.MaxLength
                        '    End If
                        'End If
                    End If
                End If
            End If
        End If

        'qMaskedTextBox
        If TypeOf (aC) Is qMaskedTextBox Then
            Dim qMT As qMaskedTextBox = CType(aC, qMaskedTextBox)
            If qMT._BindDef.Length > 0 Then
                If Not HasBindingAlready(qMT.DataBindings, "text") Then
                    qMT.DataBindings.Add("text", aBS, qMT._BindDef)
                End If
            End If
        End If
        'qDateTimePicker
        If TypeOf (aC) Is qDateTimePicker Then
            Dim qDT As qDateTimePicker = CType(aC, qDateTimePicker)
            If qDT._BindDef.Length > 0 Then
                If Not HasBindingAlready(qDT.DataBindings, "text") Then
                    qDT.DataBindings.Add("text", aBS, qDT._BindDef)
                End If
            End If
        End If
        'qCheckBox
        If TypeOf (aC) Is qCheckBox Then
            Dim qXB As qCheckBox = CType(aC, qCheckBox)
            If qXB._BindDef.Length > 0 Then
                If Not HasBindingAlready(qXB.DataBindings, "checked") Then
                    Dim B As Binding = CreateBinding("checked", aBS, qXB._BindDef)
                    AddHandler B.Format, AddressOf DB2CheckBox
                    AddHandler B.Parse, AddressOf CheckBox2DB
                    qXB.DataBindings.Add(B)
                    'qXB.DataBindings.Add("checked", aBS, qXB._BindDef)     'Replaced BHS 8/15/08
                End If
            End If
        End If
        'qComboBox BHS 2/28/08
        'qCBMultiCol 2/23/09
        If TypeOf (aC) Is qComboBox Or TypeOf (aC) Is qCBMultiCol Then
            Dim qCB As qComboBox = CType(aC, qComboBox)
            If qCB._BindDef.Length > 0 Then
                If Not HasBindingAlready(qCB.DataBindings, "Text") And
                   Not HasBindingAlready(qCB.DataBindings, "SelectedValue") Then
                    If qCB._DataType = DataTypeEnum.Dat Then
                        If qCB.DropDownStyle = ComboBoxStyle.DropDownList Then
                            Dim B As Binding = CreateBinding("SelectedValue", aBS, qCB._BindDef)
                            qCB.DataBindings.Add(B)
                        Else
                            Dim B2 As Binding = CreateBinding("Text", aBS, qCB._BindDef)
                            AddHandler B2.Format, AddressOf DBdate2Textbox
                            AddHandler B2.Parse, AddressOf TextBox2DBdate
                            qCB.DataBindings.Add(B2)
                        End If
                    ElseIf qCB._DataType = DataTypeEnum.Num Then    'BHS 8/11/08
                        If qCB.DropDownStyle = ComboBoxStyle.DropDownList Then
                            Dim B As Binding = CreateBinding("SelectedValue", aBS, qCB._BindDef)
                            qCB.DataBindings.Add(B)
                        Else
                            Dim B2 As Binding = CreateBinding("Text", aBS, qCB._BindDef)
                            AddHandler B2.Format, AddressOf DBNum2Textbox
                            AddHandler B2.Parse, AddressOf TextBox2DBNum
                            qCB.DataBindings.Add(B2)
                        End If
                    Else
                        If qCB.DropDownStyle = ComboBoxStyle.DropDownList Then
                            qCB.DataBindings.Add("SelectedValue", aBS, qCB._BindDef)
                        Else
                            qCB.DataBindings.Add("Text", aBS, qCB._BindDef) 'BHS 8/11/08
                        End If

                        'SRM 8/30/10 Assign qCB.maxlength from schema
                        Dim T As DataTable = TryCast(aBS.DataSource, DataTable)
                        If T Is Nothing Then
                            Dim DS As DataSet = TryCast(aBS.DataSource, DataSet)
                            If DS IsNot Nothing Then
                                T = TryCast(DS.Tables(aBS.DataMember), DataTable)
                            End If
                        End If
                        If T IsNot Nothing Then
                            Dim ColName As String = qCB._BindDef
                            Dim i As Integer = ColName.IndexOf(".")
                            ColName = Mid(ColName, i + 2)
                            Dim C As DataColumn = T.Columns(ColName)
                            If C.MaxLength > -1 And qCB.MaxLength > 32000 Then
                                qCB.MaxLength = C.MaxLength
                            End If
                        End If
                    End If
                End If
            End If
        End If


        If TypeOf (aC) Is qDD Then
            Dim D As qDD = CType(aC, qDD)
            If D._BindDef.Length > 0 Then
                If Not HasBindingAlready(D.DataBindings, "TextInfo") And
                   Not HasBindingAlready(D.DataBindings, "SelectedValue") Then
                    If D._DataType = DataTypeEnum.Dat Then
                        If D._MustMatchList = True Then
                            Dim B As Binding = CreateBinding("SelectedValue", aBS, D._BindDef)
                            'BHS 4/20/10
                            AddHandler B.Format, AddressOf DBdate2Textbox
                            AddHandler B.Parse, AddressOf TextBox2DBdate
                            D.DataBindings.Add(B)
                        Else
                            Dim B2 As Binding = CreateBinding("TextInfo", aBS, D._BindDef)
                            AddHandler B2.Format, AddressOf DBdate2Textbox
                            AddHandler B2.Parse, AddressOf TextBox2DBdate
                            D.DataBindings.Add(B2)
                        End If
                    ElseIf D._DataType = DataTypeEnum.Num Then    'BHS 8/11/08
                        If D._MustMatchList = True Then
                            Dim B As Binding = CreateBinding("SelectedValue", aBS, D._BindDef)
                            'BHS 4/20/10
                            AddHandler B.Format, AddressOf DBNum2Textbox
                            AddHandler B.Parse, AddressOf TextBox2DBNum
                            D.DataBindings.Add(B)
                        Else
                            Dim B2 As Binding = CreateBinding("TextInfo", aBS, D._BindDef)
                            AddHandler B2.Format, AddressOf DBNum2Textbox
                            AddHandler B2.Parse, AddressOf TextBox2DBNum
                            D.DataBindings.Add(B2)
                        End If
                    Else
                        If D._MustMatchList = True Then
                            'D.DataBindings.Add("SelectedValue", aBS, D._BindDef)  'BHS 9/16/10
                            Dim B As Binding = CreateBinding("SelectedValue", aBS, D._BindDef)
                            AddHandler B.Format, AddressOf DBChar2Textbox
                            D.DataBindings.Add(B)
                        Else
                            'D.DataBindings.Add("TextInfo", aBS, D._BindDef) 'BHS 9/16/10
                            Dim B As Binding = CreateBinding("TextInfo", aBS, D._BindDef)
                            AddHandler B.Format, AddressOf DBChar2Textbox
                            D.DataBindings.Add(B)
                        End If


                        'SRM 8/30/10 Assign qDD.maxlength from schema
                        Dim T As DataTable = TryCast(aBS.DataSource, DataTable)
                        If T Is Nothing Then
                            Dim DS As DataSet = TryCast(aBS.DataSource, DataSet)
                            If DS IsNot Nothing Then
                                T = TryCast(DS.Tables(aBS.DataMember), DataTable)
                            End If
                        End If
                        If T IsNot Nothing Then
                            Dim ColName As String = D._BindDef
                            Dim i As Integer = ColName.IndexOf(".")
                            ColName = Mid(ColName, i + 2)
                            Dim C As DataColumn = T.Columns(ColName)
                            If C.MaxLength > -1 And D.MaxLength > 32000 Then
                                D.MaxLength = C.MaxLength
                            End If
                        End If

                    End If
                End If
            End If
        End If
        ''qComboBox
        'If TypeOf (aC) Is qComboBox Then
        '    Dim qCB As qComboBox = CType(aC, qComboBox)
        '    If qCB._BindDef.Length > 0 Then
        '        If Not HasBindingAlready(qCB.DataBindings, "selectedvalue") Then
        '            qCB.DataBindings.Add("selectedvalue", aBS, qCB._BindDef)
        '        End If
        '    End If
        'End If
    End Sub


    ''' <summary> Create a Binding Object </summary>
    Function CreateBinding(ByVal aType As String,
                           ByVal aBS As BindingSource,
                           ByVal aFieldName As String) As Binding
        Return New Binding(aType, aBS, aFieldName)
    End Function

    ''' <summary> Returns true if bindingcontrolcollection has the property being queried (e.g. Text) </summary>
    Function HasBindingAlready(ByVal aBindings As ControlBindingsCollection,
                               ByVal aPropertyName As String) As Boolean
        For Each B As Binding In aBindings
            If B.PropertyName = aPropertyName Then Return True
        Next
        Return False
    End Function

    ''' <summary> Manage moving date from datatable to textbox </summary>
    Private Sub DBdate2Textbox(ByVal sender As Object,
                               ByVal cevent As ConvertEventArgs)
        If cevent.Value Is DBNull.Value Then
            cevent.Value = ""
        Else
            Dim d As Date
            d = CDate(cevent.Value)
            Dim C As Control = CType(sender, Binding).Control

            'BHS 4/20/10
            'Use qTextBox._Format if available
            Dim qT As qTextBox = TryCast(C, qTextBox)
            If qT IsNot Nothing Then
                If qT._Format = "" Then
                    cevent.Value = d.ToString("MM/dd/yyyy") 'SRM 8/30/10 default format to MM/dd/yyyy
                Else
                    cevent.Value = d.ToString(qT._Format)
                End If

                Return
            End If

            'Use qDD._Format if available  
            Dim DD As qDD = TryCast(C, qDD)
            If DD IsNot Nothing Then
                cevent.Value = d.ToString(DD._Format)
                If DD._Format = "" Then
                    cevent.Value = d.ToString("MM/dd/yyyy") 'SRM 8/30/10 default format to MM/dd/yyyy
                Else
                    cevent.Value = d.ToString(DD._Format)
                End If

                Return
            End If

            'Otherwise use standard date format
            cevent.Value = d.ToString("MM/dd/yyyy")
        End If
    End Sub

    ''' <summary> Manage moving date from textbox to datatable </summary>
    Private Sub TextBox2DBdate(ByVal sender As Object,
                               ByVal cevent As ConvertEventArgs)
        If cevent.Value.ToString = "" Then
            cevent.Value = DBNull.Value
        Else
            Dim d As Date
            If IsDate(cevent.Value) Then    'BHS 9/3/08
                d = CDate(cevent.Value)
                cevent.Value = d
            Else
                '    By not setting cEvent, last value is left if Validation error catches invalid date
            End If
        End If
    End Sub

    ''' <summary> Manage moving number from datatable to textbox </summary>
    Private Sub DBNum2Textbox(ByVal sender As Object,
                              ByVal cevent As ConvertEventArgs)
        If cevent.Value Is DBNull.Value Then
            cevent.Value = ""
        Else
            If TypeOf cevent.Value Is Double Or TypeOf cevent.Value Is Decimal Then
                Dim N As Double = CType(cevent.Value, Double)
                Dim C As Control = CType(sender, Binding).Control
                Dim qT As qTextBox = TryCast(C, qTextBox)
                If qT IsNot Nothing AndAlso qT._Format.Length > 0 Then
                    cevent.Value = N.ToString(qT._Format)
                    '    'BHS 7/12/12  I'm thinking if I do nothing the value moves through
                    'Else
                    '    cevent.Value = N.ToString   'Format doesn't matter for qRC
                End If
            End If
        End If
    End Sub

    ''' <summary> Manage moving number from textbox to datatable </summary>
    Private Sub TextBox2DBNum(ByVal sender As Object,
                              ByVal cevent As ConvertEventArgs)
        Try
            If cevent.Value.ToString = "" Then
                cevent.Value = DBNull.Value
            Else

                If IsNumeric(cevent.Value) Then     'BHS 9/3/08
                    cevent.Value = CType(cevent.Value, Decimal)
                Else
                    'Do nothing, and expect _validatenumber to catch invalid number
                End If
            End If
        Catch ex As Exception
            'SRM 8/30/10 Make no change if the user typed in so many numbers it cause a numeric overflow
        End Try
    End Sub

    ''' <summary> Manage moving character from datatable to textbox </summary>
    Private Sub DBChar2Textbox(ByVal sender As Object,
                               ByVal cevent As ConvertEventArgs)
        If cevent.Value Is DBNull.Value Then
            cevent.Value = ""
        Else
            cevent.Value = cevent.Value.ToString.Trim
        End If
    End Sub

    'BHS 8/15/08 
    ''' <summary> Datatable 1 or 0 to Checkbox Checked </summary>
    Private Sub DB2CheckBox(ByVal sender As Object,
                            ByVal cevent As ConvertEventArgs)
        If cevent.Value Is DBNull.Value Then
            cevent.Value = False    'BHS 8/15/08 Checkbox.Checked must be true or false, not Nothing or DBNull
        Else
            'BHS 8/18/08 Add _CheckedTrueValue test
            'GBV 6/29/12 First cast sender to Binding object
            Dim B As Binding = TryCast(sender, Binding)
            If B Is Nothing Then Return
            'GBV 6/29/12 Now Cast the binding object's BindableComponent property to the checkbox control
            Dim qCH As qCheckBox = TryCast(B.BindableComponent, qCheckBox)
            If qCH IsNot Nothing Then
                If qCH._CheckedTrueValue > "" Then
                    cevent.Value = (cevent.Value.ToString.Trim = qCH._CheckedTrueValue.Trim)
                Else
                    If TypeOf cevent.Value Is Boolean Then Return

                    If TypeOf cevent.Value Is Integer Then cevent.Value = (CType(cevent.Value, Integer) = 1)
                End If
            End If


        End If
    End Sub

    'BHS 8/15/08 
    ''' <summary> Checkbox checked to Datatable 1 or 0 </summary>
    Private Sub CheckBox2DB(ByVal sender As Object,
                            ByVal cevent As ConvertEventArgs)
        If cevent.Value Is Nothing Then
            cevent.Value = DBNull.Value
        Else
            'GBV 6/29/12 First cast sender to Binding object
            Dim B As Binding = TryCast(sender, Binding)
            If B Is Nothing Then Return
            'GBV 6/29/12 Now Cast the binding object's BindableComponent property to the checkbox control
            Dim qCH As qCheckBox = TryCast(B.BindableComponent, qCheckBox)
            If qCH IsNot Nothing Then
                If qCH._CheckedTrueValue > "" Then  'BHS 9/3/08
                    If CType(cevent.Value, Boolean) = True Then
                        cevent.Value = qCH._CheckedTrueValue
                    Else
                        cevent.Value = qCH._CheckedFalseValue
                    End If
                Else
                    If CType(cevent.Value, Boolean) = True Then
                        cevent.Value = 1
                    Else
                        cevent.Value = 0
                    End If
                End If
            End If
        End If


    End Sub

#End Region

#Region "------------------------ CreateFormByName -----------------------"

    ''' <summary> Instantiate a Form given its Home Assembly Name and its Object Name. GBV added aObject 8/29/2012 </summary>
    Function CreateFormByName(ByVal aFormName As String,
                              Optional ByVal aSource As String = "",
                              Optional ByVal aParameter As String = "",
                              Optional ByVal aObject As Object = Nothing) As fBase
        Dim A As Assembly

        For Each A In My.Application.Info.LoadedAssemblies

            For Each AssemblyName As String In Appl.AssemblyNames
                If ParseStr(A.FullName, ",").ToLower = AssemblyName.ToLower Then

                    Dim FullName As String = AssemblyName & "." & aFormName
                    Dim T As Type

                    Try

                        'Create type based on case-insensitive object name
                        T = A.GetType(FullName, True, True)
                        Dim F As fBase = CType(Activator.CreateInstance(T), fBase)
                        ' GBV 8/18/2014 - Minimum size should be design size
                        If F IsNot Nothing Then
                            F.MinimumSize = F.Size
                        End If
                        'BHS 4/21/09
                        If aParameter.Length > 0 OrElse aSource.Length > 0 OrElse aObject IsNot Nothing Then ' GBV 9/10/2010
                            F.PassParameter(aSource, aParameter)
                        End If
                        If aObject IsNot Nothing Then
                            F.iGenericObject = aObject
                        End If
                        Return F

                    Catch ex2 As Exception
                        Try
                            'Try without Assembly Name before declaring error
                            T = A.GetType(aFormName, True, True)
                            'Create form based on type
                            Dim F As fBase = CType(Activator.CreateInstance(T), fBase)
                            ' GBV 8/18/2014 - Minimum size should be design size
                            If F IsNot Nothing Then
                                F.MinimumSize = F.Size
                            End If
                            If aParameter.Length > 0 OrElse aSource.Length > 0 Then ' GBV 9/10/2010
                                F.PassParameter(aSource, aParameter)
                            End If
                            If aObject IsNot Nothing Then
                                F.iGenericObject = aObject
                            End If
                            Return F

                        Catch ex As Exception
                            'BHS 8/27/12 Only show exception if it is not of the 
                            '   "could not load type" variety
                            If ex.Message.ToLower.IndexOf("could not load type") = -1 Then
                                ShowError("Unexpected error loading form", ex)
                            End If
                            'ShowError("Full Form Name Not Found: " & FullName & " - ", ex)  BHS 9/17 don't complain if can't find form to open
                            'Return Nothing  BHS 11/26/07 don't give up yet - try other assemblies
                        End Try

                    End Try

                End If
            Next
        Next

        MsgBox("Assembly Name Not Found")
        Return Nothing

        'If ParseStr(A.FullName, ",").ToLower = aAssemblyName.ToLower Then

        '    Dim FullName As String = aAssemblyName & "." & aFormName
        '    Dim T As Type

        '    Try

        '        'Create type based on case-insensitive object name
        '        T = A.GetType(FullName, True, True)

        '    Catch ex2 As Exception
        '        Try
        '            'Try without Assembly Name before declaring error
        '            T = A.GetType(aFormName, True, True)
        '        Catch ex As Exception
        '            'ShowError("Full Form Name Not Found: " & FullName & " - ", ex)  BHS 9/17 don't complain if can't find form to open
        '            Return Nothing
        '        End Try

        '    End Try

        '    'Create form based on type
        '    Dim F As fBase = CType(Activator.CreateInstance(T), fBase)
        '    Return F

        'End If

    End Function

    'BHS 11/26/07 
    ''' <summary> Allow any number of assemblies BHS 9/17/07 Allows multiple assemblies </summary>
    Function GetEditForm(ByVal aName As String) As fBase
        Return CreateFormByName(aName)
        ''For Each AssemblyName As String In Appl.AssemblyNames
        '    Dim F As fBase = CreateFormByName(aName)
        '    If F IsNot Nothing Then Return F
        'Next
        'Return Nothing
        'Dim F As fBase = CreateFormByName(Appl.AssemblyName, aName)
        'If F Is Nothing And Appl.AssemblyName2.Length > 0 Then
        '    F = CreateFormByName(Appl.AssemblyName2, aName)
        'End If
        'Return F
    End Function
#End Region

#Region "------------------------ System/Environment Status -----------------------"

    'BHS 9/11/08
    ''' <summary> Returns True if we're in the development environment, 
    ''' and false if we're in runtime or debugger </summary>
    Function InDevEnv() As Boolean
        Return Process.GetCurrentProcess().ProcessName.ToUpper().Equals("DEVENV")
    End Function
#End Region

#Region "------------------------ Fill Combo Box -----------------------"

    ''' <summary> Fill Combobox based on an SQL statement </summary>
    Function FillComboBoxSQL(ByVal aSQL As String,
                             ByVal aCB As ComboBox,
                             Optional ByVal aType As String = "str",
                             Optional ByVal aCnStr As String = "",
                             Optional ByVal aIncludeBlankRow As Boolean = True) As Boolean
        Dim DV As New DataView
        Try
            If aCnStr = "" Then aCnStr = Appl.gSQLConnStr 'BHS 7/17/08 - always SQL if using SQLConnection
            'SetConnectionString() 'BHS 4/21/08  revised BHS 7/17
            Dim Cn As New SqlConnection(aCnStr)
            aSQL = CheckSQLLength(aSQL, False) '5/14/08
            DV = SQLBuildDV(aSQL, Cn, aIncludeBlankRow, aType)

            aCB.DisplayMember = "name"
            aCB.ValueMember = "value"
            aCB.DataSource = DV

        Catch ex As Exception   'Allowed in case decendant program depends on False return
            '6/28/12
            Throw New ApplicationException("Problem building dropdown for " & aCB.Name, ex)
            'DV.Dispose()
            Return False
        End Try
        Return True

    End Function
    ''' <summary> Fill Combobox based on an SQL statement (Informix database) </summary>
    Function FillComboBoxIfx(ByVal aSQL As String,
                             ByVal aCB As ComboBox,
                             Optional ByVal aType As String = "str",
                             Optional ByVal aCnStr As String = "",
                             Optional ByVal aIncludeBlankRow As Boolean = True) As Boolean
        Dim DV As New DataView
        Try
            If aCnStr = "" Then aCnStr = Appl.gAppConnStr ' GBV 1/29/2009. This is always Informix BHS 7/21/09 this is always ODBC  
            aSQL = CheckSQLLength(aSQL) '5/14/08
            DV = IfxBuildDV(aSQL, aIncludeBlankRow, aType, aCnStr)

            aCB.DisplayMember = "name"
            aCB.ValueMember = "value"
            aCB.DataSource = DV

        Catch ex As Exception   'Allowed in case decendant program depends on False return
            'BHS 6/28/12
            Throw New ApplicationException("Problem building dropdown for " & aCB.Name, ex)
            Return False
        End Try
        Return True
    End Function

    ''' <summary> Example:  FillComboBoxMultiCol(cbOrg, "Select nk, org1, m_add2, stat From name_ad Where ven_yn = 'Y' 
    ''' And (lname is Null or lname = '') Order By org1", False, "org1=0,m_add2=250,stat=500", "org1", "org1") </summary>
    Function FillComboBoxMultiCol(ByVal aCB As qCBMultiCol,
                                  ByVal aSQL As String,
                                  ByVal aIncludeBlankRow As Boolean,
                                  ByVal aMultiColDef As String,
                                  ByVal aValueMember As String,
                                  ByVal aDisplayMember As String,
                                  Optional ByVal aDV As DataView = Nothing) As Boolean

        Dim DV As DataView = aDV    'BHS 3/11/09    'Allow passing a dataview to override the SQL string

        'Build DV
        If DV Is Nothing Then DV = BuildDV(aSQL, aIncludeBlankRow)

        Dim Def As String = ParseStr(aMultiColDef, ",")
        Dim ColName As String = ""
        Dim Offset As Integer = 0

        While Def > ""
            ColName = ParseStr(Def, "=")
            ColName = RemoveStr(ColName, " ")  'Remove any blanks

            If IsNumeric(Def) Then
                Offset = CType(Def, Integer)
                DV.Table.Columns(ColName).ExtendedProperties.Add("x", Offset)
            Else
                MsgBox("Programmer Error - bad MultiColDef for " & aCB.Name & ".  Format is Colname1=0, Colname2 = 100, etc. where 0 and 100 are tabstops in pixels.")
                Return False
            End If

            Def = ParseStr(aMultiColDef, ",")

        End While

        DV.Table.Columns(aValueMember).ExtendedProperties.Add("visible", 0)

        aCB.ValueMember = aValueMember
        aCB.DisplayMember = aDisplayMember
        aCB.DataSource = DV

        Return True

    End Function
    ''' <summary> Example:  SQLFillComboBoxMultiCol(cbOrg, "Select nk, org1, m_add2, stat From name_ad Where ven_yn = 'Y' 
    ''' And (lname is Null or lname = '') Order By org1", False, "org1=0,m_add2=250,stat=500", "org1", "org1") </summary>
    Function SQLFillComboBoxMultiCol(ByVal aCB As qCBMultiCol,
                                  ByVal aSQL As String,
                                  ByVal aIncludeBlankRow As Boolean,
                                  ByVal aMultiColDef As String,
                                  ByVal aValueMember As String,
                                  ByVal aDisplayMember As String,
                                  Optional ByVal aDV As DataView = Nothing) As Boolean
        Dim DV As DataView = aDV    'BHS 3/11/09    'Allow passing a dataview to override the SQL string

        'Build DV
        If DV Is Nothing Then DV = SQLBuildDV(aSQL, aIncludeBlankRow)

        Dim Def As String = ParseStr(aMultiColDef, ",")
        Dim ColName As String = ""
        Dim Offset As Integer = 0

        While Def > ""
            ColName = ParseStr(Def, "=")
            ColName = RemoveStr(ColName, " ")  'Remove any blanks

            If IsNumeric(Def) Then
                Offset = CType(Def, Integer)
                DV.Table.Columns(ColName).ExtendedProperties.Add("x", Offset)
            Else
                MsgBox("Programmer Error - bad MultiColDef for " & aCB.Name & ".  Format is Colname1=0, Colname2 = 100, etc. where 0 and 100 are tabstops in pixels.")
                Return False
            End If

            Def = ParseStr(aMultiColDef, ",")

        End While

        DV.Table.Columns(aValueMember).ExtendedProperties.Add("visible", 0)

        aCB.ValueMember = aValueMember
        aCB.DisplayMember = aDisplayMember
        aCB.DataSource = DV

        Return True
    End Function

    '---- The rest of these functions are about filling a combobox, qcombobox or GVComboBox from name/value pairs:

    ''' <summary> Fill ComboBox from Name/Value Pairs string E.g. "Yes=Y,No=N" </summary>
    Sub FillComboBox(ByVal aNVPairs As String,
                     ByRef aCB As ComboBox)
        FillComboBox(aNVPairs, gNameValueDelimiter, gNameValuePairDelimiter, aCB)
    End Sub

    ''' <summary> Fill qComboBox from Name/Value Pairs. E.g. "Yes=Y,No=N" </summary>
    Sub FillComboBox(ByVal aNVPairs As String,
                     ByRef aCB As qComboBox)
        FillComboBox(aNVPairs, gNameValueDelimiter, gNameValuePairDelimiter, CType(aCB, ComboBox))
    End Sub

    ''' <summary> Fill a toolstrip ComboBox from Name/Value Pairs. E.g. "Yes=Y,No=N" </summary>
    Sub FillComboBox(ByVal aNVPairs As String,
                     ByRef aCB As ToolStripComboBox)
        FillComboBox(aNVPairs, gNameValueDelimiter, gNameValuePairDelimiter, CType(aCB.ComboBox, ComboBox))
    End Sub

    ''' <summary> Fill Yes/No Combobox from Name/Value pairs. </summary>
    Sub FillComboBox(ByVal aNVPairs As String,
                     ByRef aCB As qCBYesNo)
        FillComboBox(aNVPairs, gNameValueDelimiter, gNameValuePairDelimiter, CType(aCB, ComboBox))
    End Sub

    ''' <summary> Fill qCombobox from Name/Value pairs where you specify the delimiters </summary>
    Sub FillComboBox(ByVal aNVPairs As String,
                     ByVal aNVDelim As String,
                     ByVal aNVPairDelim As String,
                     ByRef aCB As qComboBox)
        FillComboBox(aNVPairs, aNVDelim, aNVPairDelim, CType(aCB, ComboBox))
    End Sub

    ''' <summary> Fill ComboBox from Name/Value Pairs (full parameters) </summary>
    Sub FillComboBox(ByVal aNVPairs As String,
                     ByVal aNVDelim As String,
                     ByVal aNVPairDelim As String,
                     ByRef aCB As ComboBox)
        Dim DV As DataView = BuildNameValueDV(aNVPairs, aNVDelim, aNVPairDelim)
        'SRM 7/29/2013 -- Removed Gabriels change since it was causing issues in 201 and 205
        aCB.DataSource = DV 'GBV 7/26/2013 moved it down after setting value member
        aCB.DisplayMember = "Name"
        aCB.ValueMember = "Value"
        'aCB.DataSource = DV
    End Sub

    ''' <summary> Fill ComboBox from Name/Value Pairs (full parameters) </summary>
    Sub FillComboBox2(ByVal aNVPairs As String,
                     ByVal aNVDelim As String,
                     ByVal aNVPairDelim As String,
                     ByRef aCB As ComboBox)
        Dim DV As DataView = BuildNameValueDV(aNVPairs, aNVDelim, aNVPairDelim)
        'aCB.DataSource = DV GBV 7/26/2013 moved it down after setting value member
        aCB.DisplayMember = "Name"
        aCB.ValueMember = "Value"
        aCB.DataSource = DV
    End Sub

    ''' <summary> Fill GV ComboBox from Name/Value Pairs (minimal parameters) </summary>
    Sub FillComboBox(ByVal aNVPairs As String,
                     ByRef aCB As DataGridViewComboBoxColumn)
        FillComboBox(aNVPairs, gNameValueDelimiter, gNameValuePairDelimiter, aCB)
    End Sub

    ''' <summary> Fill GV ComboBox from Name/Value Pairs (full parameters) </summary>
    Sub FillComboBox(ByVal aNVPairs As String,
                     ByVal aNVDelim As String,
                     ByVal aNVPairDelim As String,
                     ByRef aCB As DataGridViewComboBoxColumn)
        Dim DV As DataView = BuildNameValueDV(aNVPairs, aNVDelim, aNVPairDelim)
        aCB.DataSource = DV
        aCB.DisplayMember = "Name"
        aCB.ValueMember = "Value"
    End Sub

    'BHS 4/29/10 From Oakland
    'SRM 7/19/10 Allow option to show value first in name/value pairs

    Sub FillDD(ByVal aNVPairs As String,
           ByRef aDD As qDD,
           Optional ByVal aNVDelim As String = "",
           Optional ByVal aNVPairDelim As String = "",
           Optional ByVal aShowNameOnly As Boolean = False,
           Optional ByVal aMustMatchOnValue As Boolean = False,
           Optional ByRef aShowValueFirst As Boolean = False)

        Dim NVDelim As String = aNVDelim
        Dim NVPairDelim As String = aNVPairDelim
        If NVDelim = "" Then NVDelim = gNameValueDelimiter
        If NVPairDelim = "" Then NVPairDelim = gNameValuePairDelimiter

        'SRM 10/31/12 only add columns if they have not already been added -- Chip did this at Alameda
        If aShowNameOnly = True And aDD.ColumnCount = 0 Then
            aDD.AddColumn("Name")
            aDD.AddColumn("Value", , , False)   'Add an invisible column Value, so qDD _TextColumn, _SelectedValueColumn, etc., can refer to it.
        End If

        aDD._TextColumn = "Name"
        aDD._SelectedValueColumn = "Value"

        Dim DV As DataView = BuildNameValueDV(aNVPairs, NVDelim, NVPairDelim, aShowValueFirst)
        'SRM From Oakland 7/19/10
        aDD.DataSource = DV
        aDD._TextColumn = "Name"
        aDD._SelectedValueColumn = "Value"
        If aMustMatchOnValue = True Then  '021911 - SRM - removed "Or aShowValueFirst"
            aDD._FindByColumn = "Value"
        Else
            aDD._FindByColumn = "Name"
        End If
    End Sub

    'OTT VERSION, ANTICIPATES IFX or SQL
    ''' <summary> Fill DD based on an SQL statement.   
    ''' Assumes column names are Name and Value </summary>
    Function FillDDSQL(ByVal aSQL As String,
                             ByVal aDD As qDD,
                             Optional ByVal aIncludeBlankRow As Boolean = True,
                             Optional ByVal aBlankRowType As String = "str",
                             Optional ByVal aShowNameOnly As Boolean = False,
                             Optional ByVal aCnStr As String = "",
                             Optional ByVal aIsIfx As Boolean = True) As Boolean
        Dim DV As New DataView
        Try
            If aIsIfx = True Then
                If aCnStr = "" Then aCnStr = Appl.gAppConnStr
                Dim Cn As New OdbcConnection(aCnStr)
                aSQL = CheckSQLLength(aSQL)
                DV = IfxBuildDV(aSQL, aIncludeBlankRow, aBlankRowType)
            Else
                If aCnStr = "" Then aCnStr = Appl.gSQLConnStr
                Dim Cn As New SqlConnection(aCnStr)
                aSQL = CheckSQLLength(aSQL, False) '5/14/08
                DV = SQLBuildDV(aSQL, Cn, aIncludeBlankRow, aBlankRowType)
            End If

            Dim HoldText = aDD.Text

            aDD._TextColumn = "name"
            aDD._SelectedValueColumn = "value"

            If aShowNameOnly = True Then
                If aDD.ColumnCount = 0 Then aDD.AddColumn("name")
                If aDD.ColumnCount = 1 Then aDD.AddColumn("value", , "", False) 'Suppress display of value column
            End If


            aDD.DataSource = DV

            'If HoldText IsNot Nothing AndAlso HoldText.Length > 0 Then
            '    aDD.Text = HoldText
            'Else
            '    aDD.Text = ""
            '    aDD.SelectedValue = ""
            'End If

        Catch ex As Exception   'Allowed in case decendant program depends on False return
            'BHS 6/28/12
            Throw New ApplicationException("Problem building dropdown for " & aDD.Name, ex)
            'DV.Dispose()
            Return False
        End Try
        Return True

    End Function


    ''' <summary> Fill DD based on a copy of a DataView (useful in DR.ItemClone).
    ''' Assumes DV columns are Name and Value </summary>
    Function FillDDDV(ByVal aDV As DataView,
                             ByVal aDD As qDD,
                             Optional ByVal aShowNameOnly As Boolean = False) As Boolean

        If aDD.RowCount = 0 Then
            aDD.DataSource = aDV.Table.Copy.DefaultView
        End If

        aDD._TextColumn = "name"
        aDD._SelectedValueColumn = "value"

        If aShowNameOnly = True Then
            If aDD.ColumnCount = 0 Then aDD.AddColumn("name")
            If aDD.ColumnCount = 1 Then aDD.AddColumn("value", , "", False) 'Suppress display of value column
        End If

        aDD.DataSource = aDV

        Return True

    End Function


    ''' <summary> Fill Auto Complete List based on SQL provided. </summary>
    Public Function FillACM(ByVal aSQL As String) As AutoCompleteStringCollection
        Dim acm As New AutoCompleteStringCollection
        Dim DV As DataView = SQLBuildDV(aSQL, False)
        For Each R As DataRowView In DV
            acm.Add(R.Item(0).ToString)
        Next
        Return acm
    End Function


    ''' <summary> Underlying function to take Name/Value Pair string and build datatable records from it </summary>
    Public Function BuildNameValueDV(ByVal NameValuePairs As String,
                                     ByVal NameValueDelimiter As String,
                                     ByVal NameValuePairDelimiter As String,
                                     Optional ByRef aShowValueFirst As Boolean = False) As DataView

        ' Declare and Instantiate Name/Value Variables
        Dim nameValues As String = NameValuePairs
        Dim nameValue As String = String.Empty
        Dim name As String = String.Empty
        Dim value As String = String.Empty

        ' Declare and Instantiate DataTable
        Dim dt As New DataTable
        ' Add Columns to DataTable
        ' SRM 7/19/2010 -- allow option to show Value first
        If aShowValueFirst Then
            dt.Columns.Add("Value", GetType(System.String))
            dt.Columns.Add("Name", GetType(System.String))
        Else
            dt.Columns.Add("Name", GetType(System.String))
            dt.Columns.Add("Value", GetType(System.String))
        End If
        ' Declare and Instantiate DataView Based on DataTable
        Dim dv As New DataView(dt)

        ' Loop Through Name/Value Pairs
        Do While nameValues <> String.Empty

            ' Check for Pair Delimiter in Full Name/Value List
            If nameValues.IndexOf(NameValuePairDelimiter) > -1 _
                    And NameValuePairDelimiter <> String.Empty Then
                ' Get Single Name/Value Pair From Beginning of Full Name/Value List
                nameValue = nameValues.Substring(0, nameValues.IndexOf(NameValuePairDelimiter))
                ' Remove Single Name/Value From Full Name/Value List
                nameValues = nameValues.Remove(0, nameValues.IndexOf(NameValuePairDelimiter) + 1)
            Else
                ' Get Only Name/Value Pair Remaining in Full Name/Value List
                nameValue = nameValues
                ' Clear Full Name/Value List
                nameValues = String.Empty
            End If

            ' Check for Name/Value Delimiter in Name/Value Pair
            If nameValue.IndexOf(NameValueDelimiter) > -1 _
                    And NameValueDelimiter <> String.Empty Then
                ' Get Name From Name/Value Pair
                name = nameValue.Substring(0, nameValue.IndexOf(NameValueDelimiter))
                ' Get Value From Name/Value Pair
                value = nameValue.Substring(nameValue.IndexOf(NameValueDelimiter) + 1,
                    nameValue.Length - nameValue.IndexOf(NameValueDelimiter) - 1)
            Else
                ' Get Name From Name/Value Pair
                name = nameValue
                ' Get Value From Name/Value Pair
                value = nameValue
            End If

            ' Declare and Instantiate DataRow Base on DataTable Schema
            Dim dr As DataRow = dt.NewRow
            ' Set DataRow Column Values
            dr!Name = name
            dr!Value = value
            ' Add DataRow to DataTable
            dt.Rows.Add(dr)
        Loop

        ' Return DataView
        Return dv

    End Function

    Public Function BuildNameValueDV(ByVal NameValuePairs As String) As DataView
        Return BuildNameValueDV(NameValuePairs, gNameValueDelimiter, gNameValuePairDelimiter)
    End Function

#End Region

#Region "------------------------ Message and Error Mgt Functions -----------------------"
    ''' <summary> Programmer Error </summary>
    Sub ProgrammerErr(ByVal aMsg As String)
        MsgBox(aMsg, MsgBoxStyle.Exclamation, "Programmer Error - call Quartet Systems, Inc. ")
    End Sub

    ''' <summary> Called by general error processing, logs the error and sends emails </summary>
    Function LogError(ByVal aMsg As String, ByVal aEx As System.Exception) As Boolean
        Dim ErrorMsg As String = BuildMsg(aMsg, aEx)

        Dim Subject As String = gReleaseContext & " Error " & gUserName & " " & FormatDate(Now, "yyyy/MM/dd") & " " & TimeString

        If gVersion <> "" And gVersion <> "qsi" And Mid(gUserName, 1, 3) <> "qsi" Then   'App being run in an official version
            'BHS 2/5/10 Don't show errors for flSQLSearch
            If gMDIForm.ActiveMdiChild Is Nothing Or
                gMDIForm.ActiveMdiChild IsNot Nothing AndAlso gMDIForm.ActiveMdiChild.Name <> "flSQLSearch" Then
                'Send Email to appropriate recipients
                Dim MC As New SmtpClient
                MC.Host = "ex.ucop.edu"
                Select Case gVersion

                    Case "alpha"
                        For Each Recipient As String In gErrorAlphaEmailRecipients
                            Dim Mail As New MailMessage(gEmailSender, Recipient, "PTSW Alpha " & Subject, ErrorMsg)
                            MC.Send(Mail)
                        Next

                    Case "acctg"
                        For Each Recipient As String In gErrorAcctgEmailRecipients
                            Dim Mail As New MailMessage(gEmailSender, Recipient, "PTSW Acctg " & Subject, ErrorMsg)
                            MC.Send(Mail)
                        Next

                    Case "hotfix"
                        For Each Recipient As String In gErrorProdEmailRecipients
                            Dim Mail As New MailMessage(gEmailSender, Recipient, "PTSW Hot Fix " & Subject, ErrorMsg)
                            MC.Send(Mail)
                        Next

                    Case "prod"
                        For Each Recipient As String In gErrorProdEmailRecipients
                            Dim Mail As New MailMessage(gEmailSender, Recipient, "PTSW PRODUCTION " & Subject, ErrorMsg)
                            MC.Send(Mail)
                        Next

                    Case "pre-release"
                        For Each Recipient As String In gErrorProdEmailRecipients
                            Dim Mail As New MailMessage(gEmailSender, Recipient, "PTSW Pre-Release " & Subject, ErrorMsg)
                            MC.Send(Mail)
                        Next

                End Select

            End If
        End If

        'Write error to log
        If Not WriteToFile(ErrorMsg, gErrorLogPath & "\errorlog", 8196) Then
            MsgBox("Error writing to log file")
        End If
        Return False    'Always return false so we can call from low-level routine, and still have error bubble up
    End Function

    ''' <summary> Construct full detail error message </summary>
    Function BuildMsg(ByVal aMsg As String,
                      ByVal aEx As Exception) As String
        Dim LogMsg As String = Chr(13) & Now.ToString & "  User: " & gUserName & " on " & Environment.MachineName & Chr(13) & Chr(10) & aMsg & Chr(13) & Chr(10) & aEx.ToString & Chr(13) & Chr(10)
        If aEx.InnerException IsNot Nothing Then LogMsg &= Chr(13) & Chr(13) & aEx.InnerException.ToString
        Return LogMsg
    End Function

    'BHS 9/15/08 Removed.  Use ShowError instead
    '''' <summary> Typically called in Error portion of a Try block, to set up standard handling of the error </summary>
    'Function TryError(ByVal aMsg As String, ByVal ex As Exception) As Boolean

    '    'Write error to log
    '    LogError(aMsg, ex)

    '    'Don't display QSI Log messages
    '    If aMsg.IndexOf("Log Error") > -1 Then Return True

    '    'Standard Error Messages
    '    Dim ExMsg As String = ex.ToString

    '    If ExMsg.ToLower.IndexOf("syntax error") > -1 And ExMsg.ToLower.IndexOf("full-text") > -1 Then
    '        ExMsg = Left(ExMsg, ExMsg.ToLower.IndexOf("full-text") + 17)
    '    End If
    '    If ExMsg.IndexOf("Timeout") > -1 Then ExMsg = "Your query exceeded the allowed search time of 20 minutes.  For assistance in redefining your query, please contact 858-847-9414, 510-587-6017, 510-587-6028, or email lisa.chartrand@ucop.edu."

    '    'Show Error to user
    '    aMsg = aMsg & "      Details Follow:" & Chr(13) & "-----------------------------------------------------------------------" & Chr(13) & Chr(13) & ExMsg

    '    MsgBox(aMsg, MsgBoxStyle.Critical, "Error!")

    '    Return True 'Always returns true

    'End Function

    ''' <summary> Use this routine to change error text of standard messages </summary>
    Function TranslateErrorMsg(ByRef aMsg As String,
                               ByRef aEx As Exception) As String
        'BHS 11/4/11 Change aMsg to aEx.ToString to fix bug
        If aEx.ToString.IndexOf("Timeout") > -1 Then
            aMsg = "Your query exceeded the allowed search time.  For assistance in redefining your query, please use the contacts below. "
        End If

        If aEx.ToString.IndexOf("quiescent") > -1 Then
            aMsg = "The database server is off line.  Please try later."
        End If

        If aEx.ToString.ToLower.IndexOf("could not do a physical-order read") > -1 Then
            aMsg = "Record locked by another user at save time.  Please try later."
        End If

        Return aMsg
    End Function

    Function TryError(ByVal aMsg As String, ByVal ex As Exception) As Boolean

        'Write error to log
        LogError(aMsg, ex)

        'Don't display QSI Log messages
        If aMsg.IndexOf("Log Error") > -1 Then Return True

        'Standard Error Messages
        Dim ExMsg As String = ex.ToString

        If ExMsg.ToLower.IndexOf("syntax error") > -1 And ExMsg.ToLower.IndexOf("full-text") > -1 Then
            ExMsg = Left(ExMsg, ExMsg.ToLower.IndexOf("full-text") + 17)
        End If
        If ExMsg.IndexOf("Timeout") > -1 Then ExMsg = "Your query exceeded the allowed search time of 20 minutes.  For assistance in redefining your query, please contact 858-847-9414, 510-587-6017, 510-587-6028, or email lisa.chartrand@ucop.edu."

        'Show Error to user
        aMsg = aMsg & "      Details Follow:" & Chr(13) & "-----------------------------------------------------------------------" & Chr(13) & Chr(13) & ExMsg

        MsgBox(aMsg, MsgBoxStyle.Critical, "Error!")

        Return True 'Always returns true

    End Function

    ''' <summary> Present error in a standard form </summary>
    Function ShowError(ByVal aMsg As String,
                       ByVal aEx As System.Exception,
                       Optional ByVal aSQLDescr As String = "") As Boolean

        ' GBV 7/24/2014 - When running a cron, do not show error box
        If gIsFromCron Then
            'Throw New Exception(aMsg, aEx)
            Dim MC As New SmtpClient
            MC.Host = "ex.ucop.edu"
            Dim MM As New MailMessage()
            MM.To.Add("gabriel@quartetsystems.com")
            Dim FromStr As String = SQLGetString("SELECT email_from FROM uc_system")
            If FromStr <> "" Then
                Dim MA As New MailAddress(FromStr)
                MM.From = MA
            End If
            MM.Subject = "CRON JOB ERROR"
            MM.Body = aMsg & ". " & aEx.ToString
            MC.Send(MM)
            Return True
        End If

        'Make message more specific in some circumstances
        Dim FriendlyMsg As String = TranslateErrorMsg(aMsg, aEx)


        If FriendlyMsg.ToLower.IndexOf("error executing database query") > -1 Or
           FriendlyMsg.ToLower.IndexOf("error processing query") > -1 Or
           aEx.Message.ToLower.IndexOf("syntax error") > -1 Or
           (aEx.InnerException IsNot Nothing AndAlso aEx.InnerException.ToString.ToLower.IndexOf("syntax error") > -1) Then
            Dim frm2 As New fSyntaxError(aSQLDescr, BuildMsg(FriendlyMsg, aEx))
            frm2.ShowDialog()
        Else
            'Log error in its original form
            LogError(aMsg, aEx)
            Dim frm As New fError(gUserName & " at " & Now.ToString, FriendlyMsg, gGenErrorMsg, BuildMsg(FriendlyMsg, aEx))
            frm.ShowDialog()
        End If


        'aMsg = aMsg & "           (" & My.User.Name & " at " & Now.ToString & ")" & Chr(13) & " -----------------------------------------------------------------------" + Chr(13) + Chr(13) & gGenErrorMsg

        'MsgBox(aMsg, MsgBoxStyle.Critical, "Error!")

        Return True 'Always returns true
    End Function

    ''' <summary> Present an Error Message without logging, translation, etc. </summary>
    Sub ErrMsg(ByVal aMsg As String,
               ByVal aDetail As String)
        aMsg = aMsg + Chr(13) + "-----------------------------------------------------------------------" + Chr(13) + Chr(13) + aDetail
        MsgBox(aMsg, MsgBoxStyle.Exclamation, "Error!")
    End Sub


#End Region

#Region "------------------------ Control Functions -----------------------"

    ''' <summary> Finds a control, given a name and context.  If found returns True, otherwise False. </summary>
    Function FindControl(ByVal aName As String,
                         ByVal aParent As Control,
                         ByRef aFoundControl As Control) As Boolean

        aFoundControl = FindControl(aName, aParent)
        If aFoundControl Is Nothing Then Return False
        Return True

    End Function

    ''' <summary> Returns a control whose name matches the input, or an empty control if nothing matches </summary>
    Function FindControl(ByVal aName As String,
                         ByVal aParent As Control) As Control
        Dim C, CFound As Control

        If aParent.Name.ToLower = aName.ToLower Then Return aParent

        For Each C In aParent.Controls
            If C.Name.ToLower = aName.ToLower Then Return C
            CFound = FindControl(aName, C)   'Recursive to find all children
            If CFound.Name.ToLower = aName.ToLower Then Return CFound
        Next

        'If not found, return an empty control
        Dim CEmpty As Control = New Control
        Return CEmpty

    End Function

    'BHS 5/6/11
    ''' <summary> Returns true if the controlname is found within controlparent, and 
    ''' the control's text value has changed </summary>
    Function ControlIsDirty(ByVal aControlName As String,
                            ByVal aControlParent As Control,
                            ByVal aOrigValue As String) As Boolean
        Dim C As Control = FindControl(aControlName, aControlParent)
        If C IsNot Nothing Then
            Return ControlIsDirty(C, aOrigValue)
        End If
        Return False
    End Function

    ''' <summary> Returns true if the control's text value has changed </summary>
    Function ControlIsDirty(ByVal aControl As Control, ByVal aOrigValue As String) As Boolean
        Dim ControlValue As String = aControl.Text

        'BHS 5/11/11
        Dim qCB As qCheckBox = TryCast(aControl, qCheckBox)
        If qCB IsNot Nothing Then
            If qCB.Checked = True Then
                If qCB._CheckedTrueValue.Length > 0 Then    'BHS 1/11/10
                    ControlValue = qCB._CheckedTrueValue
                Else
                    ControlValue = "1"
                End If
            Else
                If qCB._CheckedFalseValue.Length > 0 Then
                    ControlValue = qCB._CheckedFalseValue
                Else
                    ControlValue = "0"
                End If
            End If
        End If

        If StringCompare(ControlValue, aOrigValue) = False Then
            Return True
        End If
        Return False
    End Function

    ''' <summary> Is this an Entry Control?  Can the user write to it? </summary>
    Function isEntryControl(ByVal aC As Control,
                            Optional ByVal aIsWritable As Boolean = False) As Boolean

        If TypeOf (aC) Is TextBox Then
            If aIsWritable = False Then Return True
            Dim T As TextBox = CType(aC, TextBox)
            If T.Enabled And T.ReadOnly = False And T.CanFocus And T.CanSelect Then Return True
            Return False
        End If

        If TypeOf (aC) Is qDD Then
            Return aC.Enabled
        End If

        If TypeOf (aC) Is MaskedTextBox Then
            If aIsWritable = False Then Return True
            Dim MT As MaskedTextBox = CType(aC, MaskedTextBox)
            If MT.Enabled And MT.ReadOnly = False And MT.CanFocus And MT.CanSelect Then Return True
            Return False
        End If

        If TypeOf (aC) Is DateTimePicker Then
            If aIsWritable = False Then Return True
            Dim P As DateTimePicker = CType(aC, DateTimePicker)
            If P.Enabled And P.CanFocus And P.CanSelect Then Return True
            Return False
        End If

        If TypeOf (aC) Is CheckBox Then
            If aIsWritable = False Then Return True
            Dim xb As CheckBox = CType(aC, CheckBox)
            If xb.Enabled And xb.CanFocus And xb.CanSelect Then Return True
            Return False
        End If

        'BHS 7/12/12
        If TypeOf (aC) Is qRC Then
            If aIsWritable = False Then Return True
            Dim RC As qRC = CType(aC, qRC)
            If RC.Enabled And RC.CanFocus And RC.CanSelect Then Return True
            Return False
        End If

        If TypeOf (aC) Is ComboBox Then
            If aIsWritable = False Then Return True
            Dim cb As ComboBox = CType(aC, ComboBox)
            If cb.Enabled And cb.CanFocus And cb.CanSelect Then Return True
            Return False
        End If

        Return False

    End Function

    'BHS 8/12/10  Note, we could add logic to check the schema if the control is bound (see BuildDA)
    ''' <summary> Returns str, dat or num, based on control's _DataType, or _QueryDef </summary>
    Function GetControlType(ByVal aC As Control) As String
        Dim Type As String = ""

        Dim qT As qTextBox = TryCast(aC, qTextBox)
        If qT IsNot Nothing Then
            Type = ParseStr(qT._QueryDef, "|")
            If qT._DataType = DataTypeEnum.Dat Then Type = "dat"
            If qT._DataType = DataTypeEnum.Num Then Type = "num"
            If Type.Length = 0 Then Type = "str"
        End If

        Dim RC As qRC = TryCast(aC, qRC)
        If RC IsNot Nothing Then
            Type = ParseStr(RC._QueryDef, "|")
            If RC._DataType = DataTypeEnum.Dat Then Type = "dat"
            If RC._DataType = DataTypeEnum.Num Then Type = "num"
            If Type.Length = 0 Then Type = "str"
        End If

        Dim qMT As qMaskedTextBox = TryCast(aC, qMaskedTextBox)
        If qMT IsNot Nothing Then
            Type = ParseStr(qMT._QueryDef, "|")
            If qMT._DataType = DataTypeEnum.Dat Then Type = "dat"
            If qMT._DataType = DataTypeEnum.Num Then Type = "num"
            If Type.Length = 0 Then Type = "str"
        End If

        Dim qCB As qComboBox = TryCast(aC, qComboBox)
        If qCB IsNot Nothing Then
            Type = ParseStr(qCB._QueryDef, "|")
            If qCB._DataType = DataTypeEnum.Dat Then Type = "dat"
            If qCB._DataType = DataTypeEnum.Num Then Type = "num"
            If Type.Length = 0 Then Type = "str"
        End If

        Dim qDD As qDD = TryCast(aC, qDD)
        If qDD IsNot Nothing Then
            Type = ParseStr(qDD._QueryDef, "|")
            If qDD._DataType = DataTypeEnum.Dat Then Type = "dat"
            If qDD._DataType = DataTypeEnum.Num Then Type = "num"
            If Type.Length = 0 Then Type = "str"
        End If

        Dim qMC As qCBMultiCol = TryCast(aC, qCBMultiCol)
        If qMC IsNot Nothing Then
            Type = ParseStr(qMC._QueryDef, "|")
            If qMC._DataType = DataTypeEnum.Dat Then Type = "dat"
            If qMC._DataType = DataTypeEnum.Num Then Type = "num"
            If Type.Length = 0 Then Type = "str"
        End If

        Dim qCh As qCheckBox = TryCast(aC, qCheckBox)
        If qCh IsNot Nothing Then
            Type = ParseStr(qCh._QueryDef, "|")
            If qCh._DataType = DataTypeEnum.Dat Then Type = "dat"
            If qCh._DataType = DataTypeEnum.Num Then Type = "num"
            If Type.Length = 0 Then Type = "str"
        End If

        Return Type

    End Function

    ''' <summary> Copy To Clipboard </summary>
    Sub CopyToClipboard(ByVal aC As Control)
        Dim SelectionStart, SelectionLength As Integer
        Dim S As String = GetControlSelectedTextValues(aC, SelectionStart, SelectionLength)
        If S IsNot Nothing AndAlso S > "" Then
            Try
                My.Computer.Clipboard.SetText(S)
            Catch ex As Exception
                '6/28/12 Leave error message in this situation, since this can get called
                'from the menu
                ShowError("Unable to copy selection to clipboard:", ex)
            End Try
        End If
    End Sub

    ''' <summary> Return selected text, selection start, and selection length </summary>
    Function GetControlSelectedTextValues(ByVal aC As Control,
                                          ByRef aSelectionStart As Integer,
                                          ByRef aSelectionLength As Integer) As String

        If TypeOf (aC) Is TextBox Then
            Dim T As TextBox = CType(aC, TextBox)
            aSelectionStart = T.SelectionStart
            aSelectionLength = T.SelectionLength
            If aSelectionStart = 0 And aSelectionLength = 0 Then Return aC.Text
            Return Mid(aC.Text, aSelectionStart + 1, aSelectionLength)
        End If

        If TypeOf (aC) Is MaskedTextBox Then
            Dim MT As MaskedTextBox = CType(aC, MaskedTextBox)
            aSelectionStart = MT.SelectionStart
            aSelectionLength = MT.SelectionLength
            If aSelectionStart = 0 And aSelectionLength = 0 Then Return aC.Text
            Return Mid(aC.Text, aSelectionStart + 1, aSelectionLength)
        End If

        If TypeOf (aC) Is CheckBox Then
            Dim cb As CheckBox = CType(aC, CheckBox)
            If cb.Checked Then Return "1"
            Return "0"
        End If

        If TypeOf (aC) Is DateTimePicker Then
            Return aC.Text
        End If

        If TypeOf (aC) Is ComboBox Then
            Dim CB As ComboBox = CType(aC, ComboBox)
            aSelectionStart = CB.SelectionStart
            aSelectionLength = CB.SelectionLength
            If aSelectionStart = 0 And aSelectionLength = 0 Then Return aC.Text
            Return Mid(aC.Text, aSelectionStart + 1, aSelectionLength)
        End If

        ' GBV 7/18/2014 - include qDD (ticket 850)
        If TypeOf (aC) Is qDD Then
            Dim qdd As qDD = CType(aC, qDD)
            aSelectionStart = qdd.txtCode.SelectionStart
            aSelectionLength = qdd.txtCode.SelectionLength
            If aSelectionStart = 0 And aSelectionLength = 0 Then Return aC.Text
            Return Mid(aC.Text, aSelectionStart + 1, aSelectionLength)
        End If

        'Return full string for all other types of control
        Return aC.Text

    End Function


    ''' <summary> NO LONGER ACTIVELY USED - see ParseQueryDef and GetControlType(C).  Parses Tag field in Textbox or ComboBox to Column Name and Type.  </summary>
    Function GetTagColInfo(ByVal aTag As Object,
                           ByRef aColName As String,
                           ByRef aColType As String) As Boolean
        Dim S As String

        If TypeOf aTag Is String Then
            S = CType(aTag, String)
            aColName = ParseStr(S, "|")
            aColType = S

            If aColName > "" Then
                If aColType > "" Then
                    Return True
                Else
                    aColType = "str"
                    Return True
                End If
            Else
                ProgrammerErr("Missing ColName/ColType Tag in TextBox")
                Return False
            End If
        Else
            ProgrammerErr("Expected object of type String in Text Box Tag")
            Return False
        End If

    End Function

    ''' <summary> Derive ColumnName and ColumnType from QueryDef </summary>
    Function ParseQueryDef(ByVal aQueryDef As String,
                           ByRef aColName As String,
                           ByRef aColType As String) As Boolean

        aColName = ParseStr(aQueryDef, "|")
        aColType = aQueryDef

        If aColName > "" Then
            If aColType > "" Then
                Return True
            Else
                aColType = "str"
                Return True
            End If
        End If

        'No value in aQueryDef
        Return False

    End Function

    ''' <summary>  Set up the delays for the ToolTip. </summary>
    Function SetToolTipProperties(ByRef aTT As ToolTip) As Boolean

        aTT.AutoPopDelay = 5000
        aTT.InitialDelay = 1000
        aTT.ReshowDelay = 500
        aTT.ShowAlways = True
        Return True
    End Function

    ''' <summary> Set an absolute Form Postion to match a local Control position, but also within the parent form </summary>
    Function SetFormPosition(ByVal aC As Control,
                             ByVal aNewForm As Form,
                             ByVal aHost As Form,
                             ByVal aXCorr As Integer,
                             ByVal aYCorr As Integer) As Point
        Dim P As Point = aC.Location
        Dim ParentX As Integer = 0
        P.X += aXCorr
        P.Y += aYCorr

        'Shift point based on parent form
        If aHost.MdiParent IsNot Nothing Then
            P.X += aHost.MdiParent.Location.X
            P.Y += aHost.MdiParent.Location.Y
            ParentX = aHost.MdiParent.Location.X
        End If

        'Shift point based on container(s) around this control
        Dim C As Control = aC.Parent
        While C IsNot Nothing
            P.X += C.Location.X
            P.Y += C.Location.Y
            C = C.Parent
        End While

        'Make sure New Form will fit in MDI
        'Dim i As Integer = P.X + aNewForm.Width - My.Computer.Screen.Bounds.Width
        Dim i As Integer = P.X + aNewForm.Width - aHost.MdiParent.Width - ParentX
        If i > 0 Then P.X -= i


        Return P
        'iFrm.Location = Me.PointToScreen(New Point(Me.Location))

    End Function

    'BHS 10/17/08
    ''' <summary> Move a control to an X,Y location </summary>
    Sub SetControlLocation(ByVal aControl As Control,
                           ByVal aX As Integer,
                           ByVal aY As Integer)
        Dim P As Point = aControl.Location
        P.X = aX
        P.Y = aY
        aControl.Location = P
    End Sub

    '''<summary> Select the top row of a GV, deselecting all other rows </summary>
    Public Sub GVSelectTopRow(ByRef aGV As DataGridView)
        For Each R As DataGridViewRow In aGV.SelectedRows
            R.Selected = False
        Next
        If aGV.Rows.Count > 0 Then
            aGV.Rows(0).Selected = True
            If aGV.Columns.Count > 0 Then
                aGV.FirstDisplayedCell = aGV.Rows(0).Cells(0)
            End If
        End If

    End Sub

    '''<summary> Select the top row of a qGVList </summary>
    Public Sub GVSelectTopRow(ByRef aGV As qGVList)
        GVSelectTopRow(CType(aGV, DataGridView))
    End Sub

    ''BHS 9/4/08
    '''' <summary> Return a string with name value pairs of _QueryDescr and Text 
    '''' from qEntryControls within the control passed.  aCriteriaType may be "Report" or "Population"
    '''' to limit pairs based on whether they have _QueryDef filled in. Use aPairDelimiter = | 
    '''' for GVToExcel descriptions.</summary>
    'Function GetNameValuePairs(ByVal aC As Control, Optional ByVal aCriteriaType As String = "", _
    '                           Optional ByVal aPairDelimiter As String = ",") As String
    '    Dim NVStr As String = ""
    '    GetNameValuePairs(NVStr, aC, aCriteriaType, aPairDelimiter)
    '    Return Mid(NVStr, 1, NVStr.Length - 1)    'Trim off last comma
    'End Function

    ''BHS 9/4/08
    '''' <summary> Recursive routine that examines control and all child controls to extract Name/Value pairs </summary>
    'Sub GetNameValuePairs(ByRef aNVStr As String, ByVal aC As Control, ByVal aCriteriaType As String, _
    '                      ByVal aPairDelimiter As String)
    '    If TypeOf (aC) Is qTextBox Then
    '        Dim qT As qTextBox = CType(aC, qTextBox)
    '        If qT.Text.Length > 0 Then
    '            Select Case aCriteriaType
    '                Case "Report"
    '                    If qT._QueryDef.Length = 0 Then
    '                        aNVStr &= qT._QueryDescr & "=" & qT.Text & aPairDelimiter
    '                    End If
    '                Case "Population"
    '                    If qT._QueryDef.Length > 0 Then
    '                        aNVStr &= qT._QueryDescr & "=" & qT.Text & aPairDelimiter
    '                    End If
    '                Case Else
    '                    aNVStr &= qT._QueryDescr & "=" & qT.Text & aPairDelimiter
    '            End Select
    '        End If
    '    End If
    '    If TypeOf (aC) Is qCheckBox Then
    '        Dim qCh As qCheckBox = CType(aC, qCheckBox)
    '        If qCh.Checked = True Then
    '            Select Case aCriteriaType
    '                Case "Report"
    '                    If qCh._QueryDef.Length = 0 Then
    '                        aNVStr &= qCh._QueryDescr & "=True" & aPairDelimiter
    '                    End If
    '                Case "Population"
    '                    If qCh._QueryDef.Length > 0 Then
    '                        aNVStr &= qCh._QueryDescr & "=True" & aPairDelimiter
    '                    End If
    '                Case Else
    '                    aNVStr &= qCh._QueryDescr & "=True" & aPairDelimiter
    '            End Select

    '        End If
    '    End If
    '    If TypeOf (aC) Is qComboBox Then
    '        Dim qCB As qComboBox = CType(aC, qComboBox)
    '        If qCB.Text.Length > 0 Then
    '            Select Case aCriteriaType
    '                Case "Report"
    '                    If qCB._QueryDef.Length = 0 Then
    '                        aNVStr &= qCB._QueryDescr & "=" & qCB.Text & aPairDelimiter
    '                    End If
    '                Case "Population"
    '                    If qCB._QueryDef.Length > 0 Then
    '                        aNVStr &= qCB._QueryDescr & "=" & qCB.Text & aPairDelimiter
    '                    End If
    '                Case Else
    '                    aNVStr &= qCB._QueryDescr & "=" & qCB.Text & aPairDelimiter
    '            End Select
    '            aNVStr &= qCB._QueryDescr & "=" & qCB.Text & aPairDelimiter
    '        End If
    '    End If
    '    For Each C As Control In aC.Controls
    '        GetNameValuePairs(aNVStr, C, aCriteriaType, aPairDelimiter)
    '    Next

    'End Sub


    'BHS 9/11/08
    ''' <summary> Return a string with report parameter descriptions (_QueryDescr > "", _QueryDef = "" And 
    ''' some data in the control </summary>
    Function GetRptDescr(ByVal aFP As QSILib.Windows.Forms.fpMainVersion) As String
        Dim RDescr As String = "Partition = " & aFP.cbPartition.Text
        GetRptDescr2(RDescr, aFP)
        Return RDescr
    End Function

    Function GetRptDescrFPMain2(ByVal aFP As QSILib.Windows.Forms.fpMain2) As String
        Dim RDescr As String = "Partition = " & aFP.cbPartition.Text
        GetRptDescr2(RDescr, aFP)
        Return RDescr
    End Function

    'BHS 9/11/08
    ''' <summary> Recursive routine that examines control and all child controls to extract Report Descriptions 
    ''' (_QueryDescr > "" And _QueryDef = "" and some data in the control) </summary>
    Sub GetRptDescr2(ByRef RDescr As String, ByVal aC As Control)
        If aC.Name <> "cbVersion" And aC.Name <> "cbPartition" And aC.Name <> "cbDest" Then
            If TypeOf (aC) Is qTextBox Then
                Dim qT As qTextBox = CType(aC, qTextBox)
                If qT.Text.Length > 0 And qT._QueryDescr.Length > 0 And qT._QueryDef.Length = 0 Then
                    If RDescr.Length > 0 Then RDescr &= ",  "
                    RDescr &= qT._QueryDescr & " = " & qT.Text
                End If
            End If
            If TypeOf (aC) Is qCheckBox Then
                Dim qCh As qCheckBox = CType(aC, qCheckBox)
                If qCh.Checked = True And qCh._QueryDescr.Length > 0 And qCh._QueryDef.Length = 0 Then
                    If RDescr.Length > 0 Then RDescr &= ",  "
                    RDescr &= qCh._QueryDescr & " = True"
                End If
            End If

            If TypeOf (aC) Is qRC Then
                Dim RC As qRC = CType(aC, qRC)
                If RC._DBText.Length > 0 And RC._QueryDescr.Length > 0 And RC._QueryDef.Length = 0 Then
                    If RDescr.Length > 0 Then RDescr &= ",  "
                    RDescr &= RC._QueryDescr & " = " & RC._DBText
                End If
            End If

            'BHS 2/9/10 From Oakland
            If TypeOf (aC) Is qDD Then
                Dim DD As qDD = CType(aC, qDD)
                If DD.Text.Length > 0 And DD._QueryDescr.Length > 0 And DD._QueryDef.Length = 0 Then
                    If RDescr.Length > 0 Then RDescr &= ",  "
                    RDescr &= DD._QueryDescr & " = " & DD.Text
                End If
            End If

            If TypeOf (aC) Is qComboBox Then
                Dim qCB As qComboBox = CType(aC, qComboBox)
                If qCB.Text.Length > 0 And qCB._QueryDescr.Length > 0 And qCB._QueryDef.Length = 0 Then
                    If RDescr.Length > 0 Then RDescr &= ",  "
                    RDescr &= qCB._QueryDescr & " = " & qCB.Text
                End If
            End If
        End If
        For Each C As Control In aC.Controls
            GetRptDescr2(RDescr, C)
        Next

    End Sub


    'BHS 8/17/10
    ''' <summary>Save MDI size when it is resized </summary>
    Public Sub SaveWindowSize(ByVal aClassName As String, ByVal aWidth As Integer, ByVal aHeight As Integer)
        Dim SQL As String = ""

        'BHS 10/29/12 Move these into a single transaction, to make sure that the Delete completes before the Insert starts
        'SQLDoSQL("Delete From tRpt Where classname = '" & aClassName & "' " & _
        '         " And title = 'Window Size' And savedby = '" & gUserName & "'", True)
        'SQLDoSQL(" Insert Into tRpt " & _
        '        " (classname, title, savedby, sortno) Values " & _
        '        " ('" & aClassName & "', 'Window Size', '" & gUserName & "', 1)", True)
        'SQLDoSQL("Delete From tRptCol Where classname = '" & aClassName & "' " & _
        '         " And title = 'Window Size' And savedby = '" & gUserName & "'", True)
        'SQLDoSQL("Insert Into tRptCol (classname, title, savedby, columnname, columnvalue) " & _
        '          " Values ('" & aClassName & "', 'Window Size', '" & gUserName & "', " & _
        '          aWidth.ToString & ", " & aHeight.ToString & ")", True)

        Dim SQLs As New ArrayList
        SQLs.Add("Delete From tRpt Where classname = '" & aClassName & "' " &
                 " And title = 'Window Size' And savedby = '" & gUserName & "'")
        SQLs.Add(" Insert Into tRpt " &
                " (classname, title, savedby, sortno) Values " &
                " ('" & aClassName & "', 'Window Size', '" & gUserName & "', 1)")
        SQLs.Add("Delete From tRptCol Where classname = '" & aClassName & "' " &
                 " And title = 'Window Size' And savedby = '" & gUserName & "'")
        SQLs.Add("Insert Into tRptCol (classname, title, savedby, columnname, columnvalue) " &
                  " Values ('" & aClassName & "', 'Window Size', '" & gUserName & "', " &
                  aWidth.ToString & ", " & aHeight.ToString & ")")
        Try
            DoSQLTran(SQLs)
        Catch ex As Exception
            'Ignore error
        End Try

    End Sub

    'BHS 8/17/10
    ''' <summary> Set MDI size if it has been saved in tRptCol </summary>
    Public Function SetWindowSize(ByVal aClassName As String, ByRef aSize As Size) As Size
        If Not InDevEnv() Then
            Dim DV As DataView = SQLBuildDV("Select * From tRptCol Where classname = '" & aClassName & "'" &
              " And title = 'Window Size' And savedby = '" & gUserName & "'", gSQLConnStr)

            If DV.Table.Rows.Count > 0 Then
                Dim R As DataRow = DV.Table.Rows(0)
                Dim S As New Size(CInt(R.Item("columnname").ToString), CInt(R.Item("columnvalue").ToString))
                'BHS 9/10/10 don't reduce design-time size
                If S.Height >= aSize.Height And S.Width >= aSize.Width Then
                    aSize = S
                    Return S
                End If
            End If
        End If
        Return aSize

    End Function


    'BHS 8/18/10
    ''' <summary>Save Window location when it is moved </summary>
    Public Sub SaveWindowLocation(ByVal aClassName As String, ByVal aX As Integer, ByVal aY As Integer)
        Dim SQL As String = ""

        'BHS 10/29/12 Move these into a single transaction, to make sure that the Delete completes before the Insert starts
        'SQLDoSQL("Delete From tRpt Where classname = '" & aClassName & "' " & _
        '         " And title = 'Window Loc' And savedby = '" & gUserName & "'", True)
        'SQLDoSQL(" Insert Into tRpt " & _
        '        " (classname, title, savedby, sortno) Values " & _
        '        " ('" & aClassName & "', 'Window Loc', '" & gUserName & "', 1)", True)
        'SQLDoSQL("Delete From tRptCol Where classname = '" & aClassName & "' " & _
        '         " And title = 'Window Loc' And savedby = '" & gUserName & "'", True)
        'SQLDoSQL("Insert Into tRptCol (classname, title, savedby, columnname, columnvalue) " & _
        '          " Values ('" & aClassName & "', 'Window Loc', '" & gUserName & "', " & _
        '          aX.ToString & ", " & aY.ToString & ")", True)

        Dim SQLs As New ArrayList
        SQLs.Add("Delete From tRpt Where classname = '" & aClassName & "' " &
                 " And title = 'Window Loc' And savedby = '" & gUserName & "'")
        SQLs.Add(" Insert Into tRpt " &
                " (classname, title, savedby, sortno) Values " &
                " ('" & aClassName & "', 'Window Loc', '" & gUserName & "', 1)")
        SQLs.Add("Delete From tRptCol Where classname = '" & aClassName & "' " &
                 " And title = 'Window Loc' And savedby = '" & gUserName & "'")
        SQLs.Add("Insert Into tRptCol (classname, title, savedby, columnname, columnvalue) " &
                  " Values ('" & aClassName & "', 'Window Loc', '" & gUserName & "', " &
                  aX.ToString & ", " & aY.ToString & ")")
        Try
            DoSQLTran(SQLs)
        Catch ex As Exception
            'Ignore error
        End Try


    End Sub

    'BHS 8/17/10
    ''' <summary> Set Window Location if it has been saved in tRptCol </summary>
    Public Function SetWindowLocation(ByVal aClassName As String, ByRef aPoint As Point) As Point
        If Not InDevEnv() Then
            Dim DV As DataView = SQLBuildDV("Select * From tRptCol Where classname = '" & aClassName & "'" &
              " And title = 'Window Loc' And savedby = '" & gUserName & "'", gSQLConnStr)

            If DV.Table.Rows.Count > 0 Then
                Dim R As DataRow = DV.Table.Rows(0)
                Dim P As New Point(CInt(R.Item("columnname").ToString), CInt(R.Item("columnvalue").ToString))
                aPoint = P
                Return aPoint
            End If
        End If
        Return aPoint

    End Function

    'BHS 8/26/10
    ''' <summary> Save report zoom for this user and class </summary>
    Public Sub SaveZoom(ByVal aClassName As String, ByVal aZoom As Single)

        'BHS 10/29/12 Move these into a single transaction, to make sure that the Delete completes before the Insert starts
        'SQLDoSQL("Delete From tRpt Where classname = '" & aClassName & "' " & _
        '         " And title = 'Report Zoom' And savedby = '" & gUserName & "'", True)
        'SQLDoSQL(" Insert Into tRpt " & _
        '        " (classname, title, savedby, sortno) Values " & _
        '        " ('" & aClassName & "', 'Report Zoom', '" & gUserName & "', 1)", True)
        'SQLDoSQL("Delete From tRptCol Where classname = '" & aClassName & "' " & _
        '         " And title = 'Report Zoom' And savedby = '" & gUserName & "'", True)
        'SQLDoSQL("Insert Into tRptCol (classname, title, savedby, columnname, columnvalue) " & _
        '          " Values ('" & aClassName & "', 'Report Zoom', '" & gUserName & "', " & _
        '          aZoom.ToString & ", '')", True)

        Dim SQLs As New ArrayList

        SQLs.Add("Delete From tRpt Where classname = '" & aClassName & "' " &
                 " And title = 'Report Zoom' And savedby = '" & gUserName & "'")
        SQLs.Add(" Insert Into tRpt " &
                " (classname, title, savedby, sortno) Values " &
                " ('" & aClassName & "', 'Report Zoom', '" & gUserName & "', 1)")
        SQLs.Add("Delete From tRptCol Where classname = '" & aClassName & "' " &
                 " And title = 'Report Zoom' And savedby = '" & gUserName & "'")
        SQLs.Add("Insert Into tRptCol (classname, title, savedby, columnname, columnvalue) " &
                  " Values ('" & aClassName & "', 'Report Zoom', '" & gUserName & "', " &
                  aZoom.ToString & ", '')")
        Try
            DoSQLTran(SQLs)
        Catch ex As Exception
            'Ignore error
        End Try

    End Sub

    ''' <summary> Set report zoom if it has been saved in tRptCol </summary>
    Public Function SetZoom(ByVal aClassName As String, ByRef aZoom As Single) As Single
        If Not InDevEnv() Then
            Dim DV As DataView = SQLBuildDV("Select * From tRptCol Where classname = '" & aClassName & "'" &
              " And title = 'Report Zoom' And savedby = '" & gUserName & "'", gSQLConnStr)

            If DV.Table.Rows.Count > 0 Then
                Dim R As DataRow = DV.Table.Rows(0)
                Dim wstr As String = R.Item("columnname").ToString
                If IsNumeric(wstr) Then
                    aZoom = CType(wstr, Single)
                    Return aZoom
                End If
            End If
        End If
        Return aZoom

    End Function

    'BHS 9/11/08
    ''' <summary> Return a clause that describes the filter.  Optional ToolStripComboBox 
    ''' reference if filter is limited to a column </summary>
    Function GetFilterDescr(ByVal aFilter As String,
                            Optional ByVal aCol As ToolStripComboBox = Nothing) As String
        If aFilter.Length = 0 Then Return ""

        Dim FDescr As String = aFilter

        If aCol IsNot Nothing AndAlso aCol.ComboBox.SelectedValue IsNot Nothing Then
            If aCol.ComboBox.SelectedValue.ToString <> "" And
               aCol.ComboBox.SelectedValue.ToString <> "all" Then
                FDescr &= " on " & aCol.Text
            End If
        End If

        Return FDescr

    End Function

    ''' <summary> Set Gridview List properties </summary>
    Sub SetListGVProperties(ByRef aGV As DataGridView)
        If TypeOf (aGV) Is qGVBase Then
            aGV.RowTemplate.DefaultCellStyle.BackColor = Nothing
            aGV.RowTemplate.DefaultCellStyle.ForeColor = Nothing
            aGV.BackgroundColor = QListBackColor
            If CType(aGV, qGVBase)._ShowSelectionBar = True Then
                aGV.RowTemplate.DefaultCellStyle.SelectionBackColor = QSelectionBackColor
                aGV.RowTemplate.DefaultCellStyle.SelectionForeColor = QSelectionForeColor
                'BHS 1/11/11 - to make report lists selection work
                aGV.DefaultCellStyle.SelectionBackColor = QSelectionBackColor

            Else
                aGV.RowTemplate.DefaultCellStyle.SelectionBackColor = Nothing
                aGV.RowTemplate.DefaultCellStyle.SelectionForeColor = QForeColor
                aGV.DefaultCellStyle.SelectionBackColor = QDefaultRowBackColor  'BHS 8/8/8   Replace dark blue
                aGV.AlternatingRowsDefaultCellStyle.SelectionBackColor = QAltRowBackColor   'BHS 10/16/08
            End If

            aGV.BackgroundColor = QListBackColor
            aGV.ColumnHeadersDefaultCellStyle.BackColor = QColHeaderBackColor
            aGV.RowsDefaultCellStyle.BackColor = QDefaultRowBackColor
            aGV.RowsDefaultCellStyle.ForeColor = QForeColor
            aGV.AlternatingRowsDefaultCellStyle.BackColor = QAltRowBackColor
            aGV.AlternatingRowsDefaultCellStyle.ForeColor = QForeColor
            Return
        End If

        'BHS 10/16/08 I don't know when this would be used - I think we always use qGVList for lists
        If Not TypeOf (aGV) Is qGVList Then
            aGV.AllowUserToAddRows = False
            aGV.AllowUserToDeleteRows = False
            aGV.AllowUserToOrderColumns = True
            aGV.RowTemplate.DefaultCellStyle.BackColor = QListBackColor
            aGV.RowTemplate.DefaultCellStyle.ForeColor = Color.Black
            aGV.ColumnHeadersDefaultCellStyle.BackColor = QColHeaderBackColor
            aGV.BackgroundColor = QListBackColor
            aGV.RowTemplate.DefaultCellStyle.SelectionBackColor = QSelectionBackColor
            aGV.RowTemplate.DefaultCellStyle.SelectionForeColor = QSelectionForeColor
            aGV.MultiSelect = False
            aGV.RowHeadersVisible = False
            aGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            aGV.ColumnHeadersDefaultCellStyle.BackColor = QColHeaderBackColor

        End If
    End Sub

    ''' <summary> Set Gridview Edit properties </summary>
    Sub SetEditGVProperties(ByRef aGV As DataGridView)
        Dim Col As DataGridViewColumn
        If TypeOf (aGV) Is qGVList Then
            aGV.RowTemplate.DefaultCellStyle.BackColor = Nothing
            aGV.RowTemplate.DefaultCellStyle.ForeColor = Nothing
            aGV.BackgroundColor = QListBackColor
            If CType(aGV, qGVBase)._ShowSelectionBar = True Then
                aGV.RowTemplate.DefaultCellStyle.SelectionBackColor = QSelectionBackColor
                aGV.RowTemplate.DefaultCellStyle.SelectionForeColor = QSelectionForeColor
            Else
                aGV.RowTemplate.DefaultCellStyle.SelectionBackColor = Nothing
                aGV.RowsDefaultCellStyle.SelectionBackColor = QDefaultRowBackColor
                aGV.AlternatingRowsDefaultCellStyle.SelectionBackColor = QAltRowBackColor
                aGV.RowTemplate.DefaultCellStyle.SelectionForeColor = QForeColor
            End If

            aGV.BackgroundColor = QListBackColor
            aGV.ColumnHeadersDefaultCellStyle.BackColor = QColHeaderBackColor
            aGV.RowsDefaultCellStyle.BackColor = QDefaultRowBackColor
            aGV.RowsDefaultCellStyle.ForeColor = QForeColor
            aGV.AlternatingRowsDefaultCellStyle.BackColor = QAltRowBackColor
            aGV.AlternatingRowsDefaultCellStyle.ForeColor = QForeColor

        Else
            If aGV.AllowUserToAddRows = True Then
                MsgBox("Programmer Error - aGV.AllowUserToAddRows must be False in Designer")
            End If
            aGV.AllowUserToAddRows = False
            aGV.AllowUserToDeleteRows = False
            aGV.AllowUserToOrderColumns = True
            aGV.RowTemplate.DefaultCellStyle.BackColor = QListBackColor 'Color.White
            aGV.RowTemplate.DefaultCellStyle.ForeColor = QForeColor      'Color.Black
            aGV.ColumnHeadersDefaultCellStyle.BackColor = QBackColor
            'System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
            aGV.BackgroundColor = QBackColor
            'System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
            aGV.RowTemplate.DefaultCellStyle.SelectionBackColor = QSelectionBackColor 'Color.AliceBlue
            aGV.RowTemplate.DefaultCellStyle.SelectionForeColor = QSelectionForeColor 'Color.Black
            aGV.MultiSelect = False
            'aGV.ReadOnly = Nothing    'Set individually
            aGV.RowHeadersVisible = False
            aGV.SelectionMode = DataGridViewSelectionMode.CellSelect
            aGV.EditMode = DataGridViewEditMode.EditOnEnter
            aGV.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
            'aGV.ReadOnly = False
            For Each Col In aGV.Columns
                If Col.ReadOnly = True Then
                    Col.CellTemplate.Style.BackColor = QListBackColor
                Else
                    Col.CellTemplate.Style.BackColor = QEntryBackColor
                End If
                Col.SortMode = DataGridViewColumnSortMode.NotSortable
            Next

        End If

    End Sub

    ''' <summary> Change GV Cell ForeColor to the QEmphasisForeColor </summary>
    Sub EmphasizeGVCell(ByRef aR As DataGridViewRow, ByVal aColName As String)
        aR.Cells(aColName).Style.ForeColor = QEmphasisForeColor
        aR.Cells(aColName).Style.SelectionForeColor = QEmphasisForeColor
    End Sub

#End Region

#Region "------------------------ GVFooter Functions --------------------------"

    ''' <summary> Adjust height of GV footer based on whether a horizontal scrollbar is showing </summary>
    Sub AdjustForScrollBar(ByVal aGVMain As DataGridView,
                           ByVal aGVFoot As DataGridView)
        Dim i As Integer = 0
        For Each Col As DataGridViewColumn In aGVMain.Columns
            i += Col.Width
        Next
        If i > aGVMain.Width Then   'Scroll bar visible
            If aGVFoot.Height = 20 Then
                aGVFoot.Height = 36
                Dim Pos As New Point(aGVFoot.Location.X, aGVFoot.Location.Y - 16)
                aGVFoot.Location = Pos
                aGVMain.Height -= 16
            End If
        Else    'Scroll bar not visible
            If aGVFoot.Height = 36 Then
                aGVFoot.Height = 20
                Dim Pos As New Point(aGVFoot.Location.X, aGVFoot.Location.Y + 16)
                aGVFoot.Location = Pos
                aGVMain.Height += 16
            End If
        End If
    End Sub
#End Region

#Region "------------------------ File IO Command Functions -----------------------"
    ''' <summary> Return True if this user can write to the file </summary>
    Function FileIsWritable(ByVal aPath As String) As Boolean

        Try
            Dim fs As New System.IO.FileStream(aPath, FileMode.OpenOrCreate)
            Dim CanWrite As Boolean = fs.CanWrite
            fs.Close()
            Return CanWrite
        Catch ex As Exception
            Return False
        End Try

    End Function

    ''' <summary> Write a line to a file </summary>
    Function WriteToFile(ByVal aStr As String,
                         ByVal aPath As String,
                         Optional ByVal aBuf As Integer = 2048) As Boolean
        Try
            If File.Exists(aPath) = False Then
                File.Create(aPath)
            End If

            If FileIsWritable(aPath) Then
                Dim sw As New System.IO.StreamWriter(aPath, True, System.Text.Encoding.ASCII, aBuf)
                sw.WriteLine(aStr)
                sw.Close()
            Else
                Return False
            End If

        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    ''' <summary> Returns False if aPath is nothing or contains invalid characters </summary>
    Function IsValidFileNameOrPath(ByVal aPath As String) As Boolean
        If aPath Is Nothing Then Return False

        ' Determines if there are bad characters in the name.
        For Each badChar As Char In System.IO.Path.GetInvalidPathChars
            If InStr(aPath, badChar) > 0 Then
                Return False
            End If
        Next

        Return True
    End Function

#End Region

#Region "------------------------ SQL Command Functions -----------------------"

    ''' <summary> Set authConnStr when app opens </summary>
    Sub SetAuthConnStr(ByVal aValue As String)
        gAuthConnStr = aValue
    End Sub

    ''' <summary> Set Help Path when app opens </summary>
    Sub SetHelpPath(ByVal aValue As String)
        gHelpPath = aValue
    End Sub

    'Set ReportServer when app opens
    Sub SetReportServer(ByVal aValue As String)
        gReportServer = aValue
    End Sub

    ''' <summary> Create an SQL Connection Object.  SQL Server </summary>
    Function CreateConnection() As SqlConnection
        Dim cn As SqlConnection = New SqlConnection(Appl.gSQLConnStr)   'BHS 6/17/08
        Return cn
    End Function

    ''' <summary> Get connect from Appl Settings </summary>
    Function SetConnectionString() As String
        Return gAppConnStr
    End Function

    ''' <summary> Set Data Adapter connection from aConnStr.  SQL Server. </summary>
    Function SetDAConnection(ByVal aDA As SqlDataAdapter,
                             ByVal aConnStr As String) As SqlDataAdapter
        Dim cn As SqlConnection = New SqlConnection(aConnStr)

        aDA.SelectCommand.Connection = cn
        aDA.InsertCommand.Connection = cn
        aDA.UpdateCommand.Connection = cn
        aDA.DeleteCommand.Connection = cn

        Return aDA

    End Function

    ''' <summary> Substitues components of aKey in &lt;keyvalue&gt; markers in aSQL.  Returns true if Select returns one or more records </summary>
    Function TestMatch(ByVal aSQL As String,
                       ByVal aKey As String) As Boolean
        Dim KeyField As String = ""
        Dim SQL As String = aSQL

        While aKey > ""
            KeyField = ParseStr(aKey, "|")
            SQL = SubstituteStr(SQL.ToLower, "<keyvalue>", KeyField)
        End While

        'Try    'BHS 9/15/08 
        If SQLGetNumber(SQL) > 0 Then Return True
        Return False
        'Catch ex As Exception
        '    ShowError("Problem with Matching SQL", ex)
        'End Try

        'Return False

    End Function


    ''' <summary> Generic get count SQL.  Database depends on Appl.ConnType </summary>
    Function SQLGetCount(ByVal aTableName As String,
                         ByVal aFieldName As String,
                         ByVal aValue As String) As Integer
        Dim SQLString As String

        SQLString = "Select Count(*) From " + aTableName + " Where " +
            aFieldName + " = '" + aValue + "'"

        Return CInt(DoSQL(SQLString))

    End Function

    ''' <summary> Get a number from an SQL, null result returns 0.  SQL Server.  See also IfxGetNumber. </summary>
    Function SQLGetNumber(ByVal aSQL As String) As Decimal
        Dim cn As SqlConnection = CreateConnection()

        Return SQLGetNumber(cn, aSQL)

    End Function

    ''' <summary> Get a number, null result returns 0.  SQL Server. </summary>
    Function SQLGetNumber(ByVal aCn As SqlConnection, ByVal aSQL As String) As Decimal

        Return SQLGetNumber(aCn, aSQL, "")

    End Function

    ''' <summary> Get a number, returning any error. SQL Server. </summary>
    Function SQLGetNumber(ByVal aSQL As String, ByVal aErr As String) As Decimal
        Dim cn As SqlConnection = CreateConnection()

        Return SQLGetNumber(cn, aSQL, aErr)

    End Function

    ''' <summary> SQL returns a trimmed string.  Return "" if null result.  SQL Server. </summary>
    Function SQLGetString(ByVal aSQL As String,
                          Optional ByVal aCN As SqlConnection = Nothing,
                          Optional ByVal aTrim As Boolean = True) As String
        If aCN Is Nothing Then
            aCN = CreateConnection()
        End If

        Dim ob As Object = DoSQL(aCN, aSQL, "")
        If ob Is System.DBNull.Value Then Return ""
        If ob Is Nothing Then Return ""
        If aTrim Then
            Return ob.ToString.Trim
        Else
            Return ob.ToString
        End If

    End Function



    'BHS 8/22/08
    ''' <summary> Do a Transaction, given array of SQL strings. No error message string required. SQL Server. </summary>
    Public Function DoSQLTran(ByVal aSQL As ArrayList,
                              Optional ByVal aCN As SqlConnection = Nothing,
                              Optional ByVal aT As SqlTransaction = Nothing) As Boolean
        Dim err As String = ""
        Return DoSQLTran(aSQL, err, aCN, aT)

    End Function

    'BHSCONV Replaced
    ''BHS 4/4/08
    '''' <summary> Do a Transaction, given array of SQL strings. SQL Server. </summary>
    'Public Function DoSQLTran(ByVal aSQL As ArrayList, _
    '                          ByRef aErrorMessage As String, _
    '                          Optional ByVal aCN As SqlConnection = Nothing, _
    '                          Optional ByVal aT As SqlTransaction = Nothing) As Boolean

    '    'BHS 5/22/09 No activity with transactions is allowed for Live DB in a Test Version of NUI
    '    If OKToWriteToDB() = False Then Throw New Exception("You may not write to the Live Database from a Test Application")

    '    Dim T As SqlTransaction = Nothing   '=Nothing BHS 5/26/06
    '    If aT IsNot Nothing Then T = aT
    '    Dim cmd As SqlCommand = New SqlCommand
    '    cmd.CommandTimeout = 120 ' 120 seconds - GBV - 7/22/2011
    '    Dim SQLResult As Integer
    '    Dim cn As SqlConnection
    '    If aCN IsNot Nothing Then
    '        cn = aCN
    '    Else
    '        'cn = CreateConnection()
    '        cn = New SqlConnection(Appl.gSQLConnStr)   'BHS 8/22/08
    '    End If

    '    Dim i As Integer

    '    If aT Is Nothing Then
    '        BeginTran(cn, T, cmd)
    '    Else    'BHS 10/6/08 Need to set up command without doing a begin transaction
    '        If cn.State = ConnectionState.Closed Then cn.Open()
    '        cmd = cn.CreateCommand()
    '        cmd.CommandTimeout = 120 ' 120 seconds - GBV - 7/22/2011
    '        cmd.Transaction = aT
    '    End If


    '    For i = 0 To aSQL.Count() - 1
    '        cmd.CommandText = CheckSQLLength(aSQL(i).ToString, False)   'BHS 4/7/11 added CheckSQLLength

    '        SQLResult = TranSQL(T, cmd, aErrorMessage)

    '        If SQLResult < 0 Then Return False 'Rollback happened in TranSQL

    '    Next

    '    If aT Is Nothing Then T.Commit() 'BHS 9/29/08  Don't finish transaction if it was open when we started
    '    Return True

    'End Function


    '''<summary> Open a connection and assign a transaction to it </summary>
    Function SQLBeginTran(ByRef aSQLCn As SqlConnection, ByRef aSQLTran As SqlTransaction) As Boolean
        If OKToWriteToDB() = False Then Throw New Exception("You may not write to the Live Database from a Test Application")

        If aSQLCn Is Nothing Then aSQLCn = New SqlConnection(gSQLConnStr)
        If aSQLCn.State <> ConnectionState.Open Then aSQLCn.Open()

        If aSQLTran Is Nothing Then aSQLTran = aSQLCn.BeginTransaction
        Return True
    End Function

    ''' <summary> Given a connection, this function sets aT transaction and enrolls aCmd as
    '''   a command in that transaction.  SQL Server. </summary>
    Public Function BeginTran(ByVal acn As SqlConnection,
                              ByRef aT As SqlTransaction,
                              ByRef aCmd As SqlCommand) As Boolean

        'BHS 5/22/09 No activity with transactions is allowed for Live DB in a Test Version of NUI
        If OKToWriteToDB() = False Then Throw New Exception("You may not write to the Live Database from a Test Application")

        If acn.State = ConnectionState.Closed Then acn.Open()
        If aT Is Nothing Then aT = acn.BeginTransaction()

        aCmd = acn.CreateCommand()
        'aCmd.CommandTimeout = 120 ' 120 seconds - GBV - 7/22/2011
        aCmd.CommandTimeout = 3600 ' 1 hour BHS 12/7/12
        aCmd.Transaction = aT
        Return True
    End Function

    ''' <summary> Commit Tran, clean up, and return any error.  SQL Server.   </summary>
    Public Function CommitTran(ByVal aT As SqlTransaction) As String
        Dim ErrorMsg As String = ""

        Try
            aT.Commit()
        Catch ex As Exception
            ErrorMsg = "Commit Error: " + ex.ToString

            'BHS 6/28/12
            Throw New ApplicationException("Commit Error: ", ex)

        End Try

        If Not aT.Connection Is Nothing Then aT.Connection.Close()
        If Not aT Is Nothing Then aT.Dispose()

        Return ErrorMsg

    End Function


    ''' <summary> Rollback Tran, clean up, and return any error.  SQL Server.  </summary>
    Public Function RollBackTran(ByVal aT As SqlTransaction) As String
        Dim ErrorMsg As String = ""

        Try
            aT.Rollback()
        Catch ex As Exception
            ErrorMsg = "Rollback Error: " + ex.ToString

            'BHS 6/28/12
            Throw New ApplicationException("Rollback Error: ", ex)

            'Finally
            '    If Not aT.Connection Is Nothing Then aT.Connection.Close()
            '    If Not aT Is Nothing Then aT.Dispose()
        End Try


        Return ErrorMsg

    End Function

    ''' <summary> Rollback Tran, clean up, and return any error.  SQL Server.   </summary>
    Public Function RollBackTran(ByVal aT As OdbcTransaction) As String
        Dim ErrorMsg As String = ""

        Try
            aT.Rollback()
        Catch ex As Exception
            ErrorMsg = "Rollback Error: " + ex.ToString

            'BHS 6/28/12
            Throw New ApplicationException("Rollback Error: ", ex)
        End Try

        If Not aT.Connection Is Nothing Then aT.Connection.Close()
        If Not aT Is Nothing Then aT.Dispose()

        Return ErrorMsg

    End Function

    ''Do an SQL Statement, handle errors within this routine, and return Boolean as to whether all went well
    'BHS 7/8/8 Removed to confirm there are no references to this.  DoSQLBoolean should do an SQL that returns a guaranteed boolean
    'Public Function DoSQLBoolean(ByVal aSQL As String, ByVal aErrorSummary As String) As Boolean
    '    Dim err As String = ""
    '    Dim cn As SqlConnection = CreateConnection()

    '    SQLDoSQL(cn, aSQL, err)

    '    If err > "" Then
    '        ErrMsg(aErrorSummary, err)
    '        Return False
    '    End If

    '    Return True

    'End Function

    'Do an SQL Statement, handle errors within this routine, and return Boolean as to whether all went well
    Public Function DoSQLBoolean(ByVal aSQL As String, ByVal aErrorSummary As String) As Boolean
        Dim err As String = ""
        Dim cn As SqlConnection = CreateConnection()

        DoSQL(cn, aSQL, err)

        If err > "" Then
            ErrMsg(aErrorSummary, err)
            Return False
        End If

        Return True

    End Function

    ''' <summary> Do an SQL Statement, no connection or errormessage given.  Database determined by Appl.ConnType.   </summary>
    Function DoSQL(ByVal aSQL As String) As Object
        'If Appl.ConnType = "IFX" Then
        '    Return IfxDoSQL(aSQL)
        'End If

        Return SQLDoSQL(aSQL)

    End Function

    ''' <summary> DoSQL, passing an error message reference </summary>
    Function DoSQL(ByVal aSQL As String, ByRef aErrorMessage As String,
                   Optional ByVal aForceWriteToLive As Boolean = False) As Object
        'If Appl.ConnType = "IFX" Then
        '    Return IfxDoSQL(aSQL, aErrorMessage, aForceWriteToLive)
        'End If

        Dim cn As SqlConnection = CreateConnection()

        Return SQLDoSQL(cn, aSQL, aErrorMessage, , aForceWriteToLive)

    End Function

    ''' <summary> DoSQL explicitly for SQL Server </summary>
    Function SQLDoSQL(ByVal aSQL As String,
                      Optional ByVal aForceWriteToLive As Boolean = False) As Object
        Dim cn As SqlConnection = CreateConnection()
        Dim errormsg As String = ""

        Return SQLDoSQL(cn, aSQL, errormsg, , aForceWriteToLive)

    End Function

    ''' <summary> DoSQL explicitly for SQL Server, specifying a connection </summary>
    Function SQLDoSQL(ByVal aSQL As String,
                      ByVal aCn As SqlConnection,
                      Optional ByVal aKeepConnectionOpen As Boolean = False,
                      Optional ByVal aForceWriteToLive As Boolean = False) As Object
        Dim errormsg As String = ""
        Return SQLDoSQL(aCn, aSQL, errormsg, aKeepConnectionOpen, aForceWriteToLive)
    End Function


    ''' <summary> Full DoSQL with SQLConnection specified </summary>
    Function DoSQL(ByVal acn As SqlConnection,
                   ByVal aSQL As String,
                   ByRef aErrorMessage As String,
                   Optional ByVal aForceWriteToLive As Boolean = False) As Object
        Return SQLDoSQL(acn, aSQL, aErrorMessage, , aForceWriteToLive)
    End Function

    'BHSCONV Replaced
    '''' <summary> SQL-Server-specific DoSQL full logic </summary>
    'Function SQLDoSQL(ByVal acn As SqlConnection, _
    '                  ByVal aSQL As String, _
    '                  ByRef aErrorMessage As String, _
    '                  Optional ByVal aKeepConnectionOpen As Boolean = False, _
    '                  Optional ByVal aForceWriteToLive As Boolean = False) As Object

    '    'BHS 5/22/09 Don't allow Update, Delete or Insert command against live DB from Test NUI
    '    If aForceWriteToLive = False Then
    '        Dim CapsCommand As String = Trim(aSQL).ToUpper
    '        If CapsCommand.IndexOf("UPDATE ") > -1 Or _
    '           CapsCommand.IndexOf("DELETE ") > -1 Or _
    '           CapsCommand.IndexOf("INSERT ") > -1 Then
    '            If OKToWriteToDB() = False Then Throw New Exception("You may not write to the Live Database from a Test Application")
    '        End If
    '    End If
    '    '   If there is an SQL Error, it is implicitly returned in the ByRef aErrorMessage
    '    '   The result of the SQL statement is explicitly returned in ob
    '    aSQL = CheckSQLLength(aSQL, False)  'BHS 4/7/11


    '    Dim cmd As SqlCommand = New SqlCommand(aSQL, acn)
    '    cmd.CommandTimeout = 120 ' 120 seconds - GBV - 7/22/2011
    '    Dim ob As Object = New Object

    '    aErrorMessage = ""

    '    If acn.State = ConnectionState.Closed Then acn.Open()

    '    Try
    '        ob = cmd.ExecuteScalar()
    '    Catch ex As Exception
    '        Throw New Exception("DoSQL SQL: " & Chr(13) & WrapString(aSQL, 132), ex)
    '        aErrorMessage = "SQL Error on: (" & aSQL & ")  -  " & ex.ToString
    '    Finally
    '        If Not aKeepConnectionOpen Then acn.Close()
    '    End Try

    '    If Not aKeepConnectionOpen Then acn.Close()

    '    Return ob

    'End Function

    ''' <summary> Do a Stored Procedure command plus error message reference.  SQL Server. </summary>
    Function DoSP(ByVal aSPCommand As String,
                  ByRef aErrorMessage As String) As Object
        Dim cn As SqlConnection = CreateConnection()

        Return DoSP(cn, aSPCommand, aErrorMessage)

    End Function

    ''' <summary> Do a Stored Procedure from connection, command, and error reference.  SQL Server </summary>
    Function DoSP(ByVal cn As SqlConnection,
                  ByVal aSPCommand As String,
                  ByRef aErrorMessage As String) As Object

        'Create an empty parametercollection
        Dim Params As ArrayList = New ArrayList

        Return DoSP(cn, aSPCommand, Params, 3600, aErrorMessage)

    End Function

    ''' <summary> Full DoSP.  SQL Server </summary>
    Function DoSP(ByVal acn As SqlConnection,
                  ByVal aSPCommand As String,
                  ByVal aParams As ArrayList,
                  ByVal aTimeOut As Integer,
                  ByRef aErrorMessage As String) As Object

        '   If there is an SQL Error, it is implicitly returned in the ByRef aErrorMessage
        '   The result of the SQL statement is explicitly returned in ob

        Dim cmd As SqlCommand = New SqlCommand(aSPCommand, acn)
        Dim ob As Object = New Object
        Dim param As SqlParameter

        aErrorMessage = ""

        cmd.CommandType = CommandType.StoredProcedure
        For Each param In aParams
            cmd.Parameters.Add(param)
        Next
        cmd.CommandTimeout = aTimeOut

        If acn.State = ConnectionState.Closed Then acn.Open()

        Try
            ob = cmd.ExecuteScalar()
        Finally
            acn.Close()
        End Try

        Return ob

    End Function

    ''' <summary> Do Stored Procedure where sender prepares object, containing parameter(s), timeout, etc.  SQL Server.  </summary>
    Function DoSP(ByVal acn As SqlConnection,
                  ByVal aCmd As SqlCommand,
                  ByVal aErrorMessage As String) As Object

        Dim ob As Object = New Object

        aErrorMessage = ""

        If acn.State = ConnectionState.Closed Then acn.Open()

        Try
            ob = aCmd.ExecuteScalar()
        Catch ex As Exception
            aErrorMessage = ex.ToString

            'BHS 6/28/12
            Throw New ApplicationException("Stored Procedure Error: ", ex)
        End Try

        acn.Close()

        Return ob

    End Function

    ''' <summary> Build SQL Parameter.  SQL Server </summary>
    Function BuildSQLParam(ByVal aName As String,
                           ByVal aValue As String,
                           ByVal aDirection As ParameterDirection,
                           ByVal aSize As Integer) As SqlParameter
        Dim P As SqlParameter = New SqlParameter

        P.ParameterName = aName
        P.Value = aValue
        P.Direction = aDirection
        P.Size = aSize
        Return P

    End Function

    ''' <summary> Build SQLCommand.  SQL Server </summary>
    Function BuildCmd(ByVal aCn As SqlConnection,
                      ByVal aSPCommand As String,
                      ByVal aType As CommandType,
                      ByVal aTimeout As Integer) As SqlCommand
        Dim cmd As SqlCommand = New SqlCommand

        If aCn.State = ConnectionState.Closed Then aCn.Open()
        cmd.Connection = aCn

        cmd.CommandText = aSPCommand
        cmd.CommandType = aType
        cmd.CommandTimeout = aTimeout
        Return cmd

    End Function

    ''' <summary> Add an SQL to an existing transaction.  SQL Server.  </summary>
    Public Function TranSQL(ByVal aSQL As String,
                            ByVal aT As SqlTransaction,
                            ByVal aCn As SqlConnection) As Boolean

        aSQL = CheckSQLLength(aSQL, False) '5/14/08

        Dim cmd As SqlCommand = New SqlCommand(aSQL, aCn, aT)
        'cmd.CommandTimeout = 120 ' 120 seconds - GBV - 7/22/2011
        cmd.CommandTimeout = 3600 ' 1 hour BHS 12/7/12
        Dim err As String = ""

        'Notify user if there is a problem
        If TranSQL(aT, cmd, err) = -1 Then
            ErrMsg("SQL Error:", err)
            Return False
        End If

        Return True

    End Function

    ''' <summary>
    ''' GBV - 12/13/2012
    ''' Execute a Command.  If there is a problem, rollback the associated Transaction. Called by DoSQLTranScalar. 
    ''' Supports DROP and SP_RENAME statement. See DoSQLTranScalar.
    ''' </summary>
    ''' <param name="aT">A transaction object</param>
    ''' <param name="aCmd">A command object</param>
    ''' <param name="aErrorMessage">String variable to return error messages</param>
    ''' <returns>A data object</returns>
    ''' <remarks>Do not call this function directly. Use DoSQLTranScalar.</remarks>
    Public Function TranSQLScalar(ByVal aT As SqlTransaction,
                            ByVal aCmd As SqlCommand,
                            ByRef aErrorMessage As String) As Object
        If OKToWriteToDB() = False Then Throw New Exception("You may not write to the Live Database from a Test Application")

        '   If there is a problem, it does a rollback and returns the error message

        Dim ob As New Object
        Dim str As String

        Try
            ob = aCmd.ExecuteScalar
        Catch ex As Exception
            aErrorMessage = ex.ToString
            str = RollBackTran(aT)
            If str > " " Then aErrorMessage += "*** " + str
            aErrorMessage += "Command: " & aCmd.CommandText
            'BHS 3/27/08
            Throw New ApplicationException("TranSQLScalar SQL: " & WrapString(aErrorMessage, 140), ex)
            Return Convert.DBNull
        End Try

        Return ob
    End Function
    ''' <summary> Execute a Command.  If there is a problem, rollback the associated Transaction  </summary>
    Public Function TranSQL(ByVal aT As SqlTransaction,
                            ByVal aCmd As SqlCommand,
                            ByRef aErrorMessage As String) As Integer

        If OKToWriteToDB() = False Then Throw New Exception("You may not write to the Live Database from a Test Application")

        '   If there is a problem, it does a rollback and returns the error message

        Dim Rows As Integer
        Dim str As String

        Try
            Rows = aCmd.ExecuteNonQuery()
            ' GBV 6/24/2014 - need to handle legitimate statements that do not return row counts
            If aCmd.CommandText.ToLower.IndexOf("set identity_insert") > -1 Then
                Rows = 0
            End If
        Catch ex As Exception
            aErrorMessage = ex.ToString
            str = RollBackTran(aT)
            If str > " " Then aErrorMessage += "*** " + str
            aErrorMessage += "Command: " & aCmd.CommandText
            'BHS 3/27/08
            Throw New ApplicationException("TranSQL SQL: " & WrapString(aErrorMessage, 140), ex)
            Return -1
        End Try

        Return Rows

    End Function

    'BHS 5/14/08 
    ''' <summary> Make sure SQL is less than 65536 characters long.  Return a revised string if needed </summary>
    Function CheckSQLLength(ByVal aSQL As String,
                            Optional ByVal aIsIfx As Boolean = True) As String
        Try
            If aSQL.Length > 65536 Then
                Throw New ApplicationException("Search definition exceeds 65,536 characters!  Please simplify your search request.  " & WrapString(aSQL, 140)) 'BHS 5/14/08
            End If
            'BHS 4/7/11 Change MYUPPER to UPPER if SQL search
            If aIsIfx = False Then
                'This assumes that MYUPPER is capitalized in AddWhere, where it is created
                aSQL = Replace(aSQL, "MYUPPER(", "UPPER(")
            End If
        Catch ex As Exception
            'BHS 6/12/28  I leave this as a ShowError, since we want to do corrective action after
            ShowError("Your Search request exceed 65,000 characters!  Please simplify your search request and try again.", ex)
            Dim i As Integer = aSQL.IndexOf("Where")
            If i > 0 Then   'Attempt to return an empty dataview with the columns set up correctly
                'SRM 02/05/13 - change 0 to 1 for second argument since it is 1 based.
                aSQL = Mid(aSQL, 1, i) & " Where 1=2"
            Else    'If this is a massive SQL and there is no Where clause, its a problem.
                aSQL += aSQL & " Where 1=2"
            End If
        End Try
        Return aSQL

    End Function

    'BHS 4/4/11
    '''<summary> Remove MYUPPER() references </summary>
    Function RemoveMYUPPER(ByVal aSQL As String) As String
        Dim wstr As String = aSQL.ToUpper
        While wstr.ToUpper.IndexOf("MYUPPER(") > -1
            'Remove MYUPPER(
            Dim i As Integer = wstr.IndexOf("MYUPPER(")
            If i = 0 Then
                aSQL = ""
            Else
                aSQL = Mid(aSQL, 1, i)
            End If
            If wstr.Length > i + 8 Then aSQL &= Mid(wstr, i + 9)

            'Remove final )
            wstr = aSQL
            Dim j As Integer = wstr.IndexOf(")", i)
            aSQL = Mid(wstr, 1, j)
            If wstr.Length > j + 1 Then aSQL &= Mid(wstr, j + 2)
            '...refresh wstr (SDC 07/22/2013)
            wstr = aSQL.ToUpper
        End While
        Return aSQL
    End Function

    ''' <summary> BuildDVs with just SQL.  SQL Server </summary>
    Function SQLBuildDV(ByVal aSQL As String) As DataView
        Dim aCN As SqlConnection = New SqlConnection(gSQLConnStr)
        Return SQLBuildDV(aSQL, aCN, False, "str")
    End Function

    ''' <summary> BuildDV with SQL and aEmptyRow.  SQL Server </summary>
    Function SQLBuildDV(ByVal aSQL As String,
                        ByVal aEmptyRow As Boolean) As DataView
        Dim aCN As SqlConnection = New SqlConnection(gSQLConnStr)
        Return SQLBuildDV(aSQL, aCN, aEmptyRow, "str")
    End Function

    'BHSCONV replaced
    '''' <summary> Full SQLBuildDV.  SQL Server </summary>
    'Function SQLBuildDV(ByVal aSQL As String, _
    '                    ByVal aCn As SqlConnection, _
    '                    Optional ByVal aEmptyRow As Boolean = False, _
    '                    Optional ByVal aType As String = "str") As DataView

    '    ''BHS 11/10/09
    '    'If iBadSQL = True Then
    '    '    iBadSQL = False
    '    '    Dim DV As New DataView
    '    '    Return DV
    '    'End If

    '    'Fill dvdisc for dropdown
    '    aSQL = CheckSQLLength(aSQL, False) '5/14/08
    '    Dim DAD As SqlDataAdapter = New SqlDataAdapter(aSQL, aCn)
    '    Dim DSD As New DataSet

    '    Try
    '        DAD.Fill(DSD)
    '        TrimTable(DSD.Tables(0))
    '        'Catch ex As Exception
    '        '    TryError("Problem building data view (Functions:SQLBuildDV(" & aSQL & " | " & aCn.ConnectionString + ")", ex)
    '    Catch ex As Exception
    '        Throw New ApplicationException("SQLBuildDV SQL: " & WrapString(aSQL, 140), ex)
    '    End Try

    '    If aEmptyRow Then
    '        ''Assumes single column key in the first field
    '        'Dim DR As DataRow = DSD.Tables(0).NewRow
    '        'If Left(aType.ToLower, 3) = "str" Then
    '        '    DR.Item(0) = ""
    '        'Else
    '        '    DR.Item(0) = 0
    '        'End If
    '        'DSD.Tables(0).Rows.InsertAt(DR, 0)
    '        'Assumes single column key in the first field
    '        Dim DR As DataRow = DSD.Tables(0).NewRow
    '        If Left(aType.ToLower, 3) = "str" Then
    '            DR.Item(0) = ""
    '            'BHS 4/14/2010
    '        ElseIf Left(aType.ToLower, 3) = "dat" Then
    '            DR.Item(0) = DBNull.Value
    '        Else
    '            'DR.Item(0) = 0
    '            'BHS 9/15/10
    '            DR.Item(0) = DBNull.Value  ' SRM 9/1/10
    '        End If
    '        DSD.Tables(0).Rows.InsertAt(DR, 0)
    '    End If

    '    Return DSD.Tables(0).DefaultView

    'End Function

    ''' <summary> Create a dataview, including an possible empty row on top.  Database based on Appl.ConnType </summary>
    Function BuildDV(ByVal aSQL As String,
                     ByVal aEmptyRow As Boolean) As DataView


        If Appl.ConnType = "IFX" Then
            aSQL = CheckSQLLength(aSQL) '5/14/08
            Return IfxBuildDV(aSQL, aEmptyRow)
        End If

        aSQL = CheckSQLLength(aSQL, False) '5/14/08

        'DJW 5/23/13 Need to convert syntax to SQL Server prior to calling SQLBuildDV
        aSQL = IfxToSQLSyntax(aSQL)

        'Dim DR As DataRow
        Dim cn As SqlConnection = CreateConnection()

        Return SQLBuildDV(aSQL, cn, aEmptyRow, "str")  'BHS 3/9/07

        ''Fill dvdisc for dropdown
        'Dim DAD As SqlDataAdapter = New SqlDataAdapter(aSQL, cn)
        'Dim DSD As New DataSet
        'DAD.Fill(DSD)
        'TrimTable(DSD.Tables(0))

        'If aEmptyRow Then
        '    'Assumes single column key in the first field
        '    DR = DSD.Tables(0).NewRow
        '    DR.Item(0) = ""
        '    DSD.Tables(0).Rows.InsertAt(DR, 0)
        'End If

        'Return DSD.Tables(0).DefaultView

    End Function

    ''' <summary> Create a dataview, including an possible empty row on top, with non-string type.  Database based on Appl.ConnType.  </summary>
    Function BuildDV(ByVal aSQL As String,
                     ByVal aEmptyRow As Boolean,
                     ByVal aType As String) As DataView


        If Appl.ConnType = "IFX" Then
            aSQL = CheckSQLLength(aSQL) '5/14/08
            Return IfxBuildDV(aSQL, aEmptyRow, aType)
        End If
        aSQL = CheckSQLLength(aSQL, False) '5/14/08
        'Dim DR As DataRow
        Dim cn As SqlConnection = CreateConnection()

        Return SQLBuildDV(aSQL, cn, aEmptyRow, aType)  'BHS 3/9/07

        ''Fill dvdisc for dropdown
        'Dim DAD As SqlDataAdapter = New SqlDataAdapter(aSQL, cn)
        'Dim DSD As New DataSet
        'DAD.Fill(DSD)
        'TrimTable(DSD.Tables(0))

        'If aEmptyRow Then
        '    'Assumes single column key in the first field
        '    DR = DSD.Tables(0).NewRow
        '    If Left(aType.ToLower, 3) = "str" Then
        '        DR.Item(0) = ""
        '    Else
        '        DR.Item(0) = 0
        '    End If
        '    DSD.Tables(0).Rows.InsertAt(DR, 0)
        'End If

        'Return DSD.Tables(0).DefaultView

    End Function


    ''' <summary> Create a dataview from an SQL.  Database determined by Appl.ConnType </summary>
    Function BuildDV(ByVal aSQL As String,
                     ByVal cnString As String) As DataView
        If Appl.ConnType = "IFX" Then
            Return IfxBuildDV(aSQL, False, "str", cnString)
        End If

        aSQL = CheckSQLLength(aSQL, False) '5/14/08

        Dim cn As SqlConnection = New SqlConnection(cnString)

        Return SQLBuildDV(aSQL, cn, False, "str")  'BHS 3/9/07

    End Function


    ''' <summary> Create a dataview from an SQL.  SQL Server </summary>
    Function SQLBuildDV(ByVal aSQL As String,
                        ByVal cnString As String) As DataView

        aSQL = CheckSQLLength(aSQL, False) '5/14/08

        Dim cn As SqlConnection = New SqlConnection(cnString)

        Return SQLBuildDV(aSQL, cn, False, "str")  'BHS 3/9/07

    End Function

    'BHSCONV Replaced
    ''Build Data Adapter
    ''   BHS 2/7/08 added optional aCommandSQL to allow building Insert/Update/Delete commands based on a simpler
    ''      SQL than the one we use to fill iDS.
    '''' <summary> Build Data Adapter for data entry. Note aCn should be iSQLCn, so all DAs share a connection for Transaction purposes.
    '''' Use aCommandSQL to specify a simpler SQL for Insert, Delete and Update statements than used in the original Select.  SQL Server </summary>
    'Function BuildDA(ByVal aSQL As String, _
    '                 ByRef aDS As DataSet, _
    '                 ByVal aTableName As String, _
    '                 ByVal aCn As SqlConnection, _
    '                 Optional ByVal aCommandSQL As String = "", _
    '                 Optional ByVal aFillSchema As Boolean = True) As SqlDataAdapter

    '    aSQL = CheckSQLLength(aSQL, False) '5/14/08

    '    'If optional aCommandSQL is specified, use it to build commands
    '    Dim SQL As String = aSQL
    '    If aCommandSQL.Length > 0 Then SQL = aCommandSQL

    '    Dim DA As SqlDataAdapter = New SqlDataAdapter(SQL, aCn)

    '    'BHS 5/22/09 Only allow Update, Delete or Insert command against live DB if not in Test NUI
    '    If OKToWriteToDB() = True Then
    '        Dim CB As SqlCommandBuilder = New SqlCommandBuilder(DA)
    '        CB.ConflictOption = ConflictOption.OverwriteChanges 'BHS 3/24/08 Optimistic concurrency
    '        'CB.ConflictOption = ConflictOption.CompareAllSearchableValues ' GBV 8/12/2011  turned off BHS 9/28/11
    '        CB.SetAllValues = False
    '        DA.UpdateCommand = CB.GetUpdateCommand
    '        DA.InsertCommand = CB.GetInsertCommand
    '        DA.DeleteCommand = CB.GetDeleteCommand
    '    End If

    '    'After commands are built, use the main SQL for filling data
    '    DA.SelectCommand.CommandText = aSQL
    '    If aCn.State = ConnectionState.Closed Then aCn.Open() 'BHS 5/4/09
    '    DA.Fill(aDS, aTableName)

    '    'BHS 10/19/10
    '    If aFillSchema = True Then
    '        DA.FillSchema(aDS, SchemaType.Mapped, aTableName) 'BHS 2/12/08 Get Schema Data also, to set Textbox. MaxLength
    '        'BHS 9/13/10 Remove related table constraints
    '        If aCommandSQL > "" Then
    '            'BHS 2/2/11 tolerate Group By, Having, and Order By
    '            aCommandSQL = AddWhereClause(aCommandSQL, "1 = 2")

    '            'If aCommandSQL.ToLower.IndexOf(" where ") > -1 Then  'Assumes aCommandSQL ends in Where Clause
    '            '    aCommandSQL &= " AND 1 = 2"
    '            'Else
    '            '    aCommandSQL &= " WHERE 1 = 2"
    '            'End If
    '            'BHS 2/2/11 Changed BuildDV to SQLBuildDV
    '            RemoveRelatedTableConstraints(aDS.Tables(aTableName), SQLBuildDV(aCommandSQL, gSQLConnStr))
    '        End If
    '    End If

    '    If aDS.Tables(aTableName).Columns.Item("qRowId") Is Nothing Then    'BHS 5/4/09
    '        aDS.Tables(aTableName).Columns.Add("qRowID", GetType(System.Int32), "0")    'BHS 4/28/09 Add integer column named qRowID
    '    End If

    '    aCn.Close()
    '    Return DA
    'End Function

    'BHS 2/2/11 add routine to add a Where clause to an SQL
    ''' <summary> Returns aSQL with aWhereClause appropriately inserted </summary>
    Function AddWhereClause(ByVal aSQL As String, ByVal aWhereClause As String) As String
        '   Find insertion point for where clause
        Dim ip As Integer = aSQL.ToLower.IndexOf("group by")
        If ip < 0 Then ip = aSQL.ToLower.IndexOf("having")
        If ip < 0 Then ip = aSQL.ToLower.IndexOf("order by")
        If ip < 0 Then ip = aSQL.Length

        Dim Remainder As String = ""

        If ip < aSQL.Length Then
            Remainder = Mid(aSQL, ip + 1)
        Else
            Remainder = " "
        End If

        If aSQL.ToLower.IndexOf(" where ") > -1 Then
            Return Mid(aSQL, 1, ip) & " AND " & aWhereClause & " " & Remainder
        Else
            Return Mid(aSQL, 1, ip) & " WHERE " & aWhereClause & " " & Remainder
        End If
    End Function

    '' Used to pass grid data to Report Server; probably obsolete now. 
    ' ''' <summary> Create an array of SQL statements from a GridView to insert data into the database.  </summary>
    'GBV 4/20/2015 - moved entire function to flExpReb2.vb
    'Function BuildInsertArrayFromGV(ByVal aTableName As String, _
    '                                ByVal aSQLs As ArrayList, _
    '                                ByVal aGV As DataGridView) As Boolean

    '    Dim FieldList As String = ""
    '    Dim ValueList As String = ""
    '    Dim S As String = ""

    '    Dim i As Integer
    '    'NOTE: If SQL Server gets a truncation error, try to find the column that's causing it by reducing maxi
    '    '       to limit the number of columns inserted.
    '    Dim maxi As Integer = 1000

    '    For Each R As DataGridViewRow In aGV.Rows

    '        i = 0

    '        For Each Col As DataGridViewColumn In aGV.Columns 'GBV 4/20/2015 - changed datatype of col to ancestor

    '            i += 1
    '            If i > maxi Then Continue For

    '            'Only work with qGVTextBoxColumn Type
    '            If TypeOf (Col) Is qGVTextBoxColumn Then
    '                Dim C As qGVTextBoxColumn = CType(Col, qGVTextBoxColumn)
    '                If C._DataType <> DataTypeEnum.DontSave Then    'BHS 4/2/07
    '                    FieldList += C.DataPropertyName.ToString.Trim & ","

    '                    If R.Cells(C.Name).Value IsNot Nothing AndAlso R.Cells(C.Name).Value.ToString.Length > 0 Then
    '                        Select Case C._DataType
    '                            Case DataTypeEnum.Str
    '                                ValueList += "'" & PrepareSQLSearchString(R.Cells(C.Name).Value.ToString) & "',"

    '                            Case DataTypeEnum.Dat
    '                                ValueList += "'" & FormatDateTime(CType(R.Cells(C.Name).Value.ToString, DateTime), DateFormat.ShortDate) & "',"

    '                            Case DataTypeEnum.Num
    '                                ValueList += StripCommas(R.Cells(C.Name).Value.ToString) & ","
    '                        End Select
    '                    Else
    '                        ValueList += "NULL,"
    '                    End If
    '                End If
    '            ElseIf TypeOf (Col) Is DataGridViewLinkColumn Then ' GBV 4/20/2015 - to support link columns
    '                Dim C As DataGridViewLinkColumn = CType(Col, DataGridViewLinkColumn)
    '                FieldList += C.DataPropertyName.ToString.Trim & ","
    '                If R.Cells(C.Name).Value IsNot Nothing AndAlso R.Cells(C.Name).Value.ToString.Length > 0 Then
    '                    Dim aValue As String = R.Cells(C.Name).Value.ToString
    '                    If IsDate(aValue) Then
    '                        ValueList += "'" & Date.Parse(aValue).ToString("MM/dd/yyyy") & "',"
    '                    ElseIf IsNumeric(aValue) Then
    '                        ValueList += StripCommas(aValue) & ","
    '                    Else
    '                        ValueList += "'" & PrepareSQLSearchString(R.Cells(C.Name).Value.ToString) & "',"
    '                    End If
    '                Else
    '                    ValueList += "NULL,"
    '                End If

    '            End If

    '        Next

    '        'Remove final comma
    '        If FieldList.Length > 2 Then
    '            FieldList = Mid(FieldList, 1, FieldList.Length - 1)
    '            ValueList = Mid(ValueList, 1, ValueList.Length - 1)

    '            aSQLs.Add("Insert Into " & aTableName & "(" & FieldList & ") Values (" & ValueList & ")")
    '        End If
    '        FieldList = ""
    '        ValueList = ""
    '    Next

    '    Return True
    'End Function

    'Create an Insert statement of all controls on a form  BHS 8/31/07
    '   Used to create report temp tables of current form contents
    '   Depends on BuildFieldValueStringsFromControl and BuildSQLFromControl
    ''' <summary> Create an Insert Statement that includes values from all fields on a form </summary>
    Function BuildSQLInsertFromForm(ByVal aTableName As String,
                                    ByVal aC As Control) As String
        Dim FieldList As String = ""
        Dim ValueList As String = ""

        'Get all controls recursively, and build up FieldList and ValueList
        BuildFieldValueStringsFromControl(aC, FieldList, ValueList)

        'Remove final comma and return Insert string
        If FieldList.Length > 2 Then
            FieldList = Mid(FieldList, 1, FieldList.Length - 1)
            ValueList = Mid(ValueList, 1, ValueList.Length - 1)
            Return "Insert Into " & aTableName & "(" & FieldList & ") Values (" & ValueList & ")"
        Else
            Return ""
        End If

    End Function

    ''' <summary> Build Insert Array From controls on a form (excluding datagridviews) </summary>
    Sub BuildFieldValueStringsFromControl(ByVal aC As Control,
                                          ByRef aFieldList As String,
                                          ByRef aValueList As String)
        Dim C2 As Control
        Dim txt As qTextBox
        Dim cb As qComboBox
        Dim dd As qDD
        Dim dt As qDateTimePicker
        Dim mt As qMaskedTextBox
        Dim td As qTextDisplay
        Dim xb As qCheckBox
        Dim rc As qRC

        If TypeOf (aC) Is qTextBox Then
            txt = CType(aC, qTextBox)
            BuildSQLFromControl(aFieldList, aValueList, txt.Name, txt.Text.TrimEnd, txt._DataType)
        End If

        If TypeOf (aC) Is qRC Then
            rc = CType(aC, qRC)
            BuildSQLFromControl(aFieldList, aValueList, rc.Name, rc._DBText.TrimEnd, rc._DataType)
        End If

        'BHS 2/9/10 From Oakland

        If TypeOf (aC) Is qDD Then
            dd = CType(aC, qDD)
            BuildSQLFromControl(aFieldList, aValueList, dd.Name, dd.Text.TrimEnd, dd._DataType)
        End If

        If TypeOf (aC) Is qComboBox Then
            cb = CType(aC, qComboBox)
            BuildSQLFromControl(aFieldList, aValueList, cb.Name, cb.Text.TrimEnd, cb._DataType)
        End If

        If TypeOf (aC) Is qDateTimePicker Then
            dt = CType(aC, qDateTimePicker)
            BuildSQLFromControl(aFieldList, aValueList, dt.Name, dt.Text.TrimEnd, dt._DataType)
        End If

        If TypeOf (aC) Is qMaskedTextBox Then
            mt = CType(aC, qMaskedTextBox)
            BuildSQLFromControl(aFieldList, aValueList, mt.Name, mt.Text.TrimEnd, mt._DataType)
        End If

        If TypeOf (aC) Is qTextDisplay Then
            td = CType(aC, qTextDisplay)
            BuildSQLFromControl(aFieldList, aValueList, td.Name, td.Text.TrimEnd, td._DataType)
        End If

        If TypeOf (aC) Is qCheckBox Then
            xb = CType(aC, qCheckBox)
            Dim i As String = "0"
            If xb.Checked Then i = "1"
            BuildSQLFromControl(aFieldList, aValueList, xb.Name, i, xb._DataType)
        End If

        For Each C2 In aC.Controls   'Call this routine recursively
            BuildFieldValueStringsFromControl(C2, aFieldList, aValueList)
        Next

    End Sub

    ''' <summary> Build SQL phrase from a control </summary>
    Sub BuildSQLFromControl(ByRef aFieldList As String,
                            ByRef aValueList As String,
                            ByVal aName As String,
                            ByVal aValue As String,
                            ByVal aType As DataTypeEnum)
        If aType = DataTypeEnum.DontSave Then Return

        aFieldList += aName & ","

        If aValue IsNot Nothing AndAlso aValue.ToString.Length > 0 Then
            Select Case aType
                Case DataTypeEnum.Str
                    aValueList += "'" & PrepareSQLSearchString(aValue & "',")

                Case DataTypeEnum.Dat
                    aValueList += "'" & FormatDateTime(CType(aValue, DateTime), DateFormat.ShortDate) & "',"

                Case DataTypeEnum.Num
                    aValueList += StripCommas(aValue) & ","
            End Select
        Else
            aValueList += "NULL,"
        End If

    End Sub

    'BHS 1/25/12
    ''' <summary> Creates a transaction, calls SaveTable2, and either commits or 
    ''' rollbacks the result </summary>
    Function SaveTableAndCommit(ByVal aDA As SqlDataAdapter,
                        ByVal aT As DataTable,
                        ByVal aTableType As String,
                        Optional ByRef aCn As SqlConnection = Nothing,
                        Optional ByRef aTran As SqlTransaction = Nothing,
                        Optional ByVal aIsCron As Boolean = False,
                        Optional ByRef aAcceptConcurrency As Boolean = True) As Boolean
        If aCn Is Nothing Then
            aCn = aDA.UpdateCommand.Connection
        End If

        If aCn.State = ConnectionState.Closed Then aCn.Open()
        If aTran Is Nothing Then aTran = aCn.BeginTransaction

        Dim SaveOK As Boolean = False

        SaveOK = SaveTable2(aDA, aT, aTableType, aCn, aTran)

        'If SaveTable2 fails, the rollback happens in SaveTable2
        If SaveOK Then
            If aTran IsNot Nothing Then
                aTran.Commit()
                aTran.Dispose()
                aCn.Close()
            End If
        End If

        Return SaveOK

    End Function

    'BHS 10/16/12 Mark up SaveTableAndCommit to create an Informix version - not tested as of 10/16/12x
    ''' <summary> Creates a transaction, calls SaveTable2, and either commits or 
    ''' rollbacks the result </summary>
    Function IfxSaveTableAndCommit(ByVal aDA As OdbcDataAdapter,
                        ByVal aT As DataTable,
                        ByVal aTableType As String,
                        Optional ByRef aCn As OdbcConnection = Nothing,
                        Optional ByRef aTran As OdbcTransaction = Nothing,
                        Optional ByVal aIsCron As Boolean = False,
                        Optional ByRef aAcceptConcurrency As Boolean = True) As Boolean
        If aCn Is Nothing Then
            aCn = aDA.UpdateCommand.Connection
        End If

        If aCn.State = ConnectionState.Closed Then aCn.Open()
        If aTran Is Nothing Then aTran = aCn.BeginTransaction

        Dim SaveOK As Boolean = False

        SaveOK = IfxSaveTable2(aDA, aT, aTableType, aCn, aTran)

        'If SaveTable2 fails, the rollback happens in SaveTable2
        If SaveOK Then
            If aTran IsNot Nothing Then
                aTran.Commit()
                aTran.Dispose()
                aCn.Close()
            End If
        End If

        Return SaveOK

    End Function

    '''<summary> SQL Server SaveTable2 opens a tran if needed, and does deletes before adds.  Rollback if prob.  
    ''' Leave to calling program to commit so multiple tables can be updated in one transaction.
    ''' This routine is called from SaveTable in fBase </summary>
    Function SaveTable2(ByVal aDA As SqlDataAdapter,
                        ByVal aT As DataTable,
                        ByVal aTableType As String,
                        Optional ByRef aCn As SqlConnection = Nothing,
                        Optional ByRef aTran As SqlTransaction = Nothing,
                        Optional ByVal aIsCron As Boolean = False,
                        Optional ByRef aAcceptConcurrency As Boolean = True) As Boolean
        'BHS 9/30/11 Changed aAcceptConcurrency to True, to avoid non-key concurrency checking by default
        Dim TD, TM, TA As DataTable
        Dim Str, Err As String

        If aDA.UpdateCommand Is Nothing Then
            Throw New Exception("You may not write to the Live Database from a Test Application")
        End If

        If aDA.UpdateCommand Is Nothing Then
            Throw New Exception("You may not write to the Live Database from a Test Application")
        End If

        'Set up connection and Transaction if not open yet
        If aCn Is Nothing Then
            aCn = aDA.UpdateCommand.Connection
        End If

        If aCn.State = ConnectionState.Closed Then aCn.Open()
        If aTran Is Nothing Then aTran = aCn.BeginTransaction

        'Assign aCN and aTran to aDA commands
        'BHS 7/12/12
        aDA.UpdateCommand.Connection = aCn
        aDA.InsertCommand.Connection = aCn
        aDA.DeleteCommand.Connection = aCn

        aDA.UpdateCommand.Transaction = aTran
        aDA.InsertCommand.Transaction = aTran
        aDA.DeleteCommand.Transaction = aTran

        Try

            If aTableType = "D" Then
                'iTableBeingUpdated = aT
                TD = aT.GetChanges(DataRowState.Deleted)
                TM = aT.GetChanges(DataRowState.Modified)
                TA = aT.GetChanges(DataRowState.Added)


                'Delete detail records
                If Not TD Is Nothing Then
                    aDA.Update(TD)
                    TD.Dispose()
                End If

                'Add detail records
                If Not TA Is Nothing Then
                    'ShowTable(TA)
                    aDA.Update(TA)
                    TA.Dispose()
                End If

                'Modify detail records
                If Not TM Is Nothing Then
                    aDA.Update(TM)
                    TM.Dispose()
                End If

            Else

                'Update master record
                'iTableBeingUpdated = Nothing
                aDA.Update(aT)

            End If

        Catch dbcx As Data.DBConcurrencyException
            'BHS 6/28/12 Removed special concurrency logic - not used in current scheme
            Throw New ApplicationException("Database Concurrency Exception", dbcx)

            'If Not aIsCron Then
            '    Dim MsgResult As MsgBoxResult
            '    If aAcceptConcurrency Then
            '        MsgResult = MsgBoxResult.Yes
            '    Else
            '        MsgResult = MsgBox("The record you are attempting to save has been changed by another user. " & vbCrLf & _
            '                           "Do you want to save any way?", MsgBoxStyle.YesNo)
            '    End If
            '    If MsgResult = MsgBoxResult.Yes Then
            '        aAcceptConcurrency = True
            '        'Get the record from database and "merge" it with the memory table, preserving changes.
            '        ' Then do the update.
            '        Dim Table As DataTable = aT.Clone
            '        aDA.SelectCommand.Transaction = aTran
            '        aDA.Fill(Table)
            '        If aTableType = "M" Then
            '            If Table.Rows.Count = aT.Rows.Count Then
            '                aT.Merge(Table, True)
            '                aDA.Update(aT)
            '                Return True
            '            Else
            '                ShowError("Unable to update: database record was deleted by another user", dbcx)
            '                Return False
            '            End If
            '        Else
            '            Dim TMod As DataTable = aT.GetChanges(DataRowState.Modified)
            '            If TMod IsNot Nothing Then
            '                If Table.Rows.Count = aT.Rows.Count Then
            '                    TMod.Merge(Table, True)
            '                    aDA.Update(TMod)
            '                    TMod.Dispose()
            '                    Return True
            '                Else
            '                    ShowError("Unable to update: database record(s) deleted by another user", dbcx)
            '                    Return False
            '                End If
            '            End If
            '        End If
            '    Else
            '        Str = RollBackTran(aTran)
            '        aTran.Dispose()
            '        Err = "Update Cancelled due to Concurrency"
            '        If Str <> "" Then Err &= " *** AND ROLLBACK EXCEPTION TOO. "
            '        ShowError(Err, dbcx)
            '        Return False
            '    End If
            'Else
            '    'BHS 6/28/12
            '    Throw New ApplicationException("Database Concurrency Exception", dbcx)
            'End If

        Catch ex As Exception
            Err = "SaveTable Error"
            Str = RollBackTran(aTran)
            aTran.Dispose()
            If Str > " " Then Err &= " *** AND ROLLBACK EXCEPTION TOO."
            'If Not aIsCron Then  ' GBV 9/3/2010
            Throw New ApplicationException(Err, ex)
            'Else
            '    Throw
            'End If
            Return False

        End Try

        'TrimTable(aT)

        Return True

    End Function

    ''' <summary>
    ''' Identical to SaveTable2 except that the concurrency logic is enabled
    ''' </summary>
    ''' <remarks>GBV 5/28/2015</remarks>
    Function SaveTable3(ByVal aDA As SqlDataAdapter,
                        ByVal aT As DataTable,
                        ByVal aTableType As String,
                        Optional ByRef aCn As SqlConnection = Nothing,
                        Optional ByRef aTran As SqlTransaction = Nothing,
                        Optional ByVal aIsCron As Boolean = False,
                        Optional ByRef aAcceptConcurrency As Boolean = True) As Boolean
        'BHS 9/30/11 Changed aAcceptConcurrency to True, to avoid non-key concurrency checking by default
        Dim TD, TM, TA As DataTable
        Dim Str, Err As String

        If aDA.UpdateCommand Is Nothing Then
            Throw New Exception("You may not write to the Live Database from a Test Application")
        End If

        If aDA.UpdateCommand Is Nothing Then
            Throw New Exception("You may not write to the Live Database from a Test Application")
        End If

        'Set up connection and Transaction if not open yet
        If aCn Is Nothing Then
            aCn = aDA.UpdateCommand.Connection
        End If

        If aCn.State = ConnectionState.Closed Then aCn.Open()
        If aTran Is Nothing Then aTran = aCn.BeginTransaction

        'Assign aCN and aTran to aDA commands
        'BHS 7/12/12
        aDA.UpdateCommand.Connection = aCn
        aDA.InsertCommand.Connection = aCn
        aDA.DeleteCommand.Connection = aCn

        aDA.UpdateCommand.Transaction = aTran
        aDA.InsertCommand.Transaction = aTran
        aDA.DeleteCommand.Transaction = aTran

        Try

            If aTableType = "D" Then
                'iTableBeingUpdated = aT
                TD = aT.GetChanges(DataRowState.Deleted)
                TM = aT.GetChanges(DataRowState.Modified)
                TA = aT.GetChanges(DataRowState.Added)


                'Delete detail records
                If Not TD Is Nothing Then
                    aDA.Update(TD)
                    TD.Dispose()
                End If

                'Add detail records
                If Not TA Is Nothing Then
                    'ShowTable(TA)
                    aDA.Update(TA)
                    TA.Dispose()
                End If

                'Modify detail records
                If Not TM Is Nothing Then
                    aDA.Update(TM)
                    TM.Dispose()
                End If

            Else

                'Update master record
                'iTableBeingUpdated = Nothing
                aDA.Update(aT)

            End If

        Catch dbcx As Data.DBConcurrencyException
            'BHS 6/28/12 Removed special concurrency logic - not used in current scheme
            'Throw New ApplicationException("Database Concurrency Exception", dbcx)

            If Not aIsCron Then
                Dim MsgResult As MsgBoxResult
                If aAcceptConcurrency Then
                    MsgResult = MsgBoxResult.Yes
                Else
                    MsgResult = MsgBox("The record you are attempting to save has been changed by another user. " & vbCrLf &
                                       "Do you want to save any way?", MsgBoxStyle.YesNo)
                End If
                If MsgResult = MsgBoxResult.Yes Then
                    aAcceptConcurrency = True
                    'Get the record from database and "merge" it with the memory table, preserving changes.
                    ' Then do the update.
                    Dim Table As DataTable = aT.Clone
                    aDA.SelectCommand.Transaction = aTran
                    aDA.Fill(Table)
                    If aTableType = "M" Then
                        If Table.Rows.Count = aT.Rows.Count Then
                            aT.Merge(Table, True)
                            aDA.Update(aT)
                            Return True
                        Else
                            ShowError("Unable to update: database record was deleted by another user", dbcx)
                            Return False
                        End If
                    Else
                        Dim TMod As DataTable = aT.GetChanges(DataRowState.Modified)
                        If TMod IsNot Nothing Then
                            If Table.Rows.Count = aT.Rows.Count Then
                                TMod.Merge(Table, True)
                                aDA.Update(TMod)
                                TMod.Dispose()
                                Return True
                            Else
                                ShowError("Unable to update: database record(s) deleted by another user", dbcx)
                                Return False
                            End If
                        End If
                    End If
                Else
                    Str = RollBackTran(aTran)
                    aTran.Dispose()
                    Err = "Update Cancelled due to Concurrency"
                    If Str <> "" Then Err &= " *** AND ROLLBACK EXCEPTION TOO. "
                    ShowError(Err, dbcx)
                    Return False
                End If
            Else
                'BHS 6/28/12
                Throw New ApplicationException("Database Concurrency Exception", dbcx)
            End If

        Catch ex As Exception
            Err = "SaveTable Error"
            Str = RollBackTran(aTran)
            aTran.Dispose()
            If Str > " " Then Err &= " *** AND ROLLBACK EXCEPTION TOO."
            'If Not aIsCron Then  ' GBV 9/3/2010
            Throw New ApplicationException(Err, ex)
            'Else
            '    Throw
            'End If
            Return False

        End Try

        'TrimTable(aT)

        Return True

    End Function


    '''<summary> When an SQLDataAdapter is used to save a table, this function can be called from the DA.SQLRowUpdated event
    '''  to set the iDS table's identity value while still in the transaction. Requires an SQL Tran reference. </summary>
    Sub SetTableIdentity(ByVal aTableName As String, ByVal aIdentityColName As String, ByVal e As System.Data.SqlClient.SqlRowUpdatedEventArgs, ByVal aTran As SqlTransaction)
        If e.Errors Is Nothing Then
            If e.StatementType = StatementType.Insert Then
                Dim SQL As String = "Select " & aIdentityColName & " From " & aTableName & " Where " &
                      aIdentityColName & " = @@Identity"
                Dim cmd As New SqlCommand(SQL, e.Command.Connection)
                If aTran IsNot Nothing Then cmd.Transaction = aTran
                Dim dr As SqlDataReader = cmd.ExecuteReader()
                If dr.Read Then
                    Dim RO As Boolean = e.Row.Table.Columns(aIdentityColName).ReadOnly  'BHS 2/19/08
                    e.Row.Table.Columns(aIdentityColName).ReadOnly = False
                    e.Row.Item(aIdentityColName) = dr.Item(aIdentityColName)
                    e.Row.Table.Columns(aIdentityColName).ReadOnly = RO
                    'BHS 4/28/09
                    'If iTableBeingUpdated.Columns("qRowID") IsNot Nothing Then

                    'End If


                End If

                dr.Close()
            End If
        End If
    End Sub

    '''
    '''<summary> Call after Inserting a row using ODBCDoSQLTran </summary>
    Function GetIfxTableIdentity(ByVal aPartn As String, ByVal aTableName As String,
                            ByVal aColName As String, ByVal aTrans As OdbcTransaction) As Integer

        Dim cmd As New OdbcCommand("SELECT dbinfo('sqlca.sqlerrd1') FROM " &
                   aPartn & ":systables where tabname = '" & aTableName & "'", aTrans.Connection)
        cmd.Transaction = aTrans
        Return CInt(cmd.ExecuteScalar)

    End Function

    '''
    '''<summary> Call after Inserting a row using ODBCDoSQLTran </summary>
    Function GetSQLTableIdentity(ByVal aPartn As String, ByVal aTableName As String,
                            ByVal aColName As String, ByVal aCn As SqlConnection, ByVal aTrans As SqlTransaction) As Integer

        Dim ReturnVal As Integer = 0

        Dim SQL As String = "SELECT " & aColName & " FROM " & aPartn & ".." & aTableName & " WHERE " & aColName & " = @@Identity"
        Dim cmd As New SqlCommand(SQL, aCn)
        cmd.Transaction = aTrans
        Dim dr As SqlDataReader = cmd.ExecuteReader()
        If dr.Read Then ReturnVal = CInt(dr.Item(aColName))
        dr.Close()

        Return ReturnVal
    End Function


#End Region

#Region "------------------------ ODBC Command Functions -----------------------"
    ''' <summary>
    ''' Set ODBC Database - Live and Test ODBC Drivers start with a DATABASE clause that can be modified
    ''' by this function.  Use this to avoid putting the database in the FROM clause.
    ''' </summary>
    Public Function SetODBCDatabase(ByVal aConnString As String,
                                    ByVal aDB As String) As String
        Dim Str As String = "DATABASE=" & aDB & ";" & Mid(aConnString, 14)
        Return Str
    End Function

    'Public Function ODBCDoSQLTran(ByVal aSQL As ArrayList, _
    '                              aErrMsg As String, 
    '                              Optional ByVal aCN As OdbcConnection = Nothing, _
    '                              Optional ByVal aT As OdbcTransaction = Nothing) As Boolean
    '    Return ODBCDoSQLTran(aSQL, aCN, aErrMsg, aT)
    'End Function

    '''' <summary> Do a Transaction, given array of SQL strings. ODBC </summary>
    'Public Function ODBCDoSQLTran(ByVal aSQL As ArrayList, ByRef aErrorMessage As String) As Boolean
    '    Return ODBCDoSQLTran(aSQL, New OdbcConnection(gAppConnStr), aErrorMessage)
    'End Function



    ''' <summary> Begin Transaction.  ODBC </summary>
    Public Function ODBCBeginTran(ByVal acn As OdbcConnection,
                                  ByRef aT As OdbcTransaction,
                                  ByRef aCmd As OdbcCommand) As Boolean
        '  Given a connection, this function sets aT transaction and enrolls aCmd as
        '   a command in that transaction

        'BHS 5/22/09 No activity with transactions is allowed for Live DB in a Test Version of NUI
        If OKToWriteToDB() = False Then Throw New Exception("You may not write to the Live Database from a Test Application")
        If acn.State = ConnectionState.Closed Then acn.Open()

        aT = acn.BeginTransaction()
        aCmd = acn.CreateCommand()
        aCmd.Transaction = aT
        Return True
    End Function

    ''' <summary> Transaction SQL executes an SQL Statement within a transaction.  ODBC </summary>
    ''' GBV - 8/18/2011 - Added aNoRollBackIfLocked to skip rollbacks if Informix record is locked
    Public Function ODBCTranSQL(ByVal aT As OdbcTransaction,
                                ByVal aCmd As OdbcCommand,
                                ByRef aErrorMessage As String,
                                ByVal aNoRollBackIfLocked As Boolean) As Integer

        '   If there is a problem, it does a rollback and returns the error message

        Dim Rows As Integer
        Dim str As String
        Dim Locked As Boolean = False

        Try
            'ODBCDoSQL(aCmd.Connection, "Set Isolation Dirty Read", "")  'Running within a Transaction, not reading
            Dim cmdDirtyRead As New OdbcCommand("Set Isolation Dirty Read", aCmd.Connection) 'BHS 4/27/09
            cmdDirtyRead.Transaction = aT   'BHS 4/28/09
            cmdDirtyRead.ExecuteNonQuery()
            Rows = aCmd.ExecuteNonQuery()
        Catch ex As Exception
            ' GBV 8/12/2011 - Friendlier message
            If ex.ToString.ToLower.IndexOf("could not do a physical-order read to fetch next row") > -1 Then
                aErrorMessage = "Database Save Failed: The database record you're trying to write to is locked.  " &
                                "Your changes have not been saved to the database.  Please try again later."
                Locked = True
            Else
                aErrorMessage = "Command: " & aCmd.CommandText & "     " & ex.ToString
            End If
            If Locked AndAlso aNoRollBackIfLocked Then ' GBV 2/18/2011
                Return 0
            Else
                str = ODBCRollBackTran(aT)
                If str > " " Then aErrorMessage += "*** " + str

                'BHS 4/19/12  [NO ROLLOUT FOR NUI SCHEDULED YET]
                '   Avoid silent database errors
                Throw New Exception("ODBCTranSQL: " & WrapString(aErrorMessage, 140), ex)

                Return -1
            End If

        End Try

        Return Rows

    End Function

    ''' <summary> Rollback Tran, clean up, and return any error.  ODBC </summary>
    Public Function ODBCRollBackTran(ByVal aT As OdbcTransaction) As String
        Dim ErrorMsg As String = ""

        Try
            aT.Rollback()
        Catch ex As Exception
            ErrorMsg = "Rollback Error: " + ex.ToString

            'BHS 6/28/12
            Throw New ApplicationException("Rollback Error: ", ex)
        End Try

        If Not aT.Connection Is Nothing Then aT.Connection.Close()
        If Not aT Is Nothing Then aT.Dispose()

        Return ErrorMsg

    End Function

    'BHS 7/20/09
    '''' <summary> Create a dataview, including an possible empty row on top.  ODBC </summary>
    'Function ODBCBuildDV(ByVal aSQL As String, ByVal aEmptyRow As Boolean) As DataView

    '    Dim DR As DataRow
    '    'Dim cn As OdbcConnection = New OdbcConnection(My.Settings.AppConnStr)

    '    'Fill dvdisc for dropdown
    '    Dim DAD As OdbcDataAdapter = New OdbcDataAdapter(aSQL, gAppConnStr)
    '    Dim DSD As New DataSet
    '    'ODBCDoSQL(DAD.SelectCommand.Connection, "Set Isolation Dirty Read", "")  'BHS 4/22/09
    '    If DAD.SelectCommand.Connection.State = ConnectionState.Closed Then DAD.SelectCommand.Connection.Open()
    '    Dim cmdDirtyRead As New OdbcCommand("Set Isolation Dirty Read", DAD.SelectCommand.Connection) 'BHS 4/27/09
    '    cmdDirtyRead.ExecuteNonQuery()
    '    DAD.Fill(DSD)
    '    TrimTable(DSD.Tables(0))

    '    If aEmptyRow Then
    '        'Assumes single column key in the first field
    '        DR = DSD.Tables(0).NewRow
    '        DR.Item(0) = ""
    '        DSD.Tables(0).Rows.InsertAt(DR, 0)
    '    End If

    '    Return DSD.Tables(0).DefaultView

    'End Function

    '''' <summary> Do an SQL Statement, no connection or errormessage given.  ODBC </summary>
    'Function ODBCDoSQL(ByVal aSQL As String) As Object

    '    'Dim cn As OdbcConnection = New OdbcConnection(gAppConnStr)
    '    If gODBCCn Is Nothing Then gODBCCn = New OdbcConnection(gAppConnStr) ' GBV 7/23/2009
    '    Dim errormsg As String = ""

    '    Return ODBCDoSQL(gODBCCn, aSQL, errormsg)

    'End Function

    '''' <summary> Do an SQL Statement, no connection given.  ODBC </summary>
    'Function ODBCDoSQL(ByVal aSQL As String, _
    '                   ByRef aErrorMessage As String) As Object


    '    'Dim cn As OdbcConnection = New OdbcConnection(gAppConnStr)
    '    If gODBCCn Is Nothing Then gODBCCn = New OdbcConnection(gAppConnStr) ' GBV 7/23/2009

    '    Return ODBCDoSQL(gODBCCn, aSQL, aErrorMessage)

    'End Function

    'BHS 7/20/09 Replaced by new ODBDoSQL
    '''' <summary> Do an SQL Statement.  ODBC </summary>
    'Function ODBCDoSQL(ByVal aCn As OdbcConnection, _
    '                   ByVal aSQL As String, _
    '                   ByRef aErrorMessage As String) As Object

    '    'BHS 5/22/09 Don't allow Update, Delete or Insert command against live DB from Test NUI
    '    Dim CapsCommand As String = Trim(aSQL).ToUpper
    '    If CapsCommand.IndexOf("UPDATE ") > -1 Or _
    '       CapsCommand.IndexOf("DELETE ") > -1 Or _
    '       CapsCommand.IndexOf("INSERT ") > -1 Then
    '        If OKToWriteToDB() = False Then Throw New Exception("You may not write to the Live Database from a Test Application")
    '    End If

    '    '   If there is an SQL Error, it is implicitly returned in the ByRef aErrorMessage
    '    '   The result of the SQL statement is explicitly returned in ob

    '    Dim cmd As OdbcCommand = New OdbcCommand(aSQL, aCn)
    '    Dim ob As Object = New Object

    '    aErrorMessage = ""

    '    If aCn.State = ConnectionState.Closed Then aCn.Open()

    '    Try
    '        'ODBCDoSQL(aCn, "Set Isolation Dirty Read", "")  'BHS 4/22/09
    '        Dim cmdDirtyRead As New OdbcCommand("Set Isolation Dirty Read", aCn) 'BHS 4/27/09
    '        cmdDirtyRead.ExecuteNonQuery()
    '        ob = cmd.ExecuteScalar()
    '    Catch ex As Exception
    '        aErrorMessage = ex.ToString
    '    End Try

    '    aCn.Close()

    '    Return ob

    'End Function

    ''' <summary> Build Data Adapter. Note aCn should be iODBCCn, so all DAs share a connection for Transaction purposes.  ODBC </summary>
    Function ODBCBuildDA(ByVal aSQL As String,
                         ByRef aDS As DataSet,
                         ByVal aTableName As String,
                         ByVal aCn As OdbcConnection) As OdbcDataAdapter
        Dim DA As New OdbcDataAdapter(aSQL, aCn)
        Try

            'BHS 5/22/09 Only allow Update, Delete or Insert command against live DB if not in Test NUI
            If OKToWriteToDB() = True Then
                Dim CB As New OdbcCommandBuilder(DA)
                DA.UpdateCommand = CB.GetUpdateCommand
                DA.InsertCommand = CB.GetInsertCommand
                DA.DeleteCommand = CB.GetDeleteCommand
                DA.InsertCommand.UpdatedRowSource = UpdateRowSource.FirstReturnedRecord 'BHS 3/10/09
            End If

            If aCn.State = ConnectionState.Closed Then aCn.Open() 'BHS 5/4/09
            'ODBCDoSQL(aCn, "Set Isolation Dirty Read", "")  'BHS 4/22/09
            Dim cmdDirtyRead As New OdbcCommand("Set Isolation Dirty Read", aCn) 'BHS 4/27/09
            cmdDirtyRead.ExecuteNonQuery()
            DA.Fill(aDS, aTableName)
            DA.FillSchema(aDS, SchemaType.Mapped, aTableName) 'BHS 2/12/08 Get Schema Data also, to set Textbox. MaxLength

            'BHS 9/13/10 Because there is no aCommandSQL in this routine, there is no need 
            '  to RemoveRelatedTableConstraints

            ''BHS 9/8/10 'Except for autoincrement columns, make all   columns writable
            'For Each C As DataColumn In aDS.Tables(aTableName).Columns
            '    If C.AutoIncrement = False And C.ColumnName <> "qRowID" Then
            '        C.ReadOnly = False
            '    End If
            'Next

            If aDS.Tables(aTableName).Columns.Item("qRowID") Is Nothing Then    'BHS 5/4/09
                aDS.Tables(aTableName).Columns.Add("qRowID", GetType(System.Int32), "0")    'BHS 4/28/09 Add integer column named qRowID
            End If
            'aCn.Close()    BHS 5/10/10
        Catch ex As Exception
            If ex.Message.IndexOf("abandoned mutex") = -1 Then
                Throw New ApplicationException("ODBCBuildDA", ex)
            End If

        Finally
            aCn.Close() ' GBV 3/4/2010 moved from before catch block
        End Try

        Return DA
    End Function

    '''<summary> When an ODBCDataAdapter is used to save a table, this function can be called from the DA.ODBCRowUpdated event
    '''  to set the iDS table's identity value. </summary>
    Sub SetTableIdentity(ByVal aPartn As String, ByVal aTableName As String,
                            ByVal aColName As String, ByVal aTrans As OdbcTransaction,
                            ByRef e As System.Data.Odbc.OdbcRowUpdatedEventArgs)
        If e.Errors Is Nothing Then
            If e.StatementType = StatementType.Insert Then
                Dim cmd As New OdbcCommand("SELECT dbinfo('sqlca.sqlerrd1') FROM " &
                           aPartn & ":systables where tabname = '" & aTableName & "'",
                           aTrans.Connection)
                cmd.Transaction = aTrans
                Dim RO As Boolean = e.Row.Table.Columns(aColName).ReadOnly  'GBV 3/28/2009
                e.Row.Table.Columns(aColName).ReadOnly = False
                e.Row.Item(aColName) = CInt(cmd.ExecuteScalar)
                e.Row.Table.Columns(aColName).ReadOnly = RO ' GBV 3/28/2009
            End If
        End If
    End Sub

#End Region



#Region "------------------------ SQL Clause Functions -----------------------"
    ''' <summary> Add to SQL Where Clause </summary>
    Public Function AddWhere(ByVal aSQL As String,
                             ByVal aColName As String,
                             ByVal aColValue As String,
                             ByVal aColType As String,
                             Optional ByRef aSyntaxError As Boolean = False,
                             Optional ByVal aTrimStartingWithWhere As Boolean = False) As String

        'Revs - BHS 8/27/07 Add Between logic, <> to Not Like when wildcards are involved
        '       BHS 10/11/07 Add Null logic
        '       BHS 1/19/10 gQBESyntaxError
        'Symbols:
        '   * Wildcard
        '   > >= < <= = Relationships
        '  , Or
        '  + And
        'UC Symbols: {AND}, {-} for range

        Dim ip As Integer       'insert point
        Dim pp As Integer       'part pointer
        Dim NewSQL As String    'string that holds the growing SQL statement
        Dim Remainder As String 'remainder of SQL statement
        Dim searchtime1 As Boolean = False
        Dim searchtime2 As Boolean = False
        Dim comparison As String = " = "
        Dim UsesBuiltIn As Boolean = False  '...to track whether QBE field contains a built-in function
        Dim wstr, wstr2, wstr9 As String

        Dim whereclause As String = ""

        Dim partvalue As String = "" 'part of value before the next comma
        Dim secondpartvalue As String = ""  'BHS 8/27/07
        Dim Part As Boolean = False
        Dim OrClause As Boolean = False
        Dim FirstPass As Boolean = True

        'Try casting any datetime to get rid of time portion (SDC 08/27/2014)
        aColName = aColName.Trim
        If Left(aColType.ToLower, 1) = "d" Then
            aColName = "Cast(Floor(Cast(" & aColName & " As Float)) AS Datetime)"
        End If

        'NOTE: This is too early to clean the string.  We clean each part of the string below.  (SDC 04/03/2014)
        'aColValue = PrepareSQLSearchString(aColValue)
        '   Find insertion point for where clause
        ip = aSQL.ToLower.IndexOf("group by")
        If ip < 0 Then ip = aSQL.ToLower.IndexOf("having")
        If ip < 0 Then ip = aSQL.ToLower.IndexOf("order by")
        If ip < 0 Then ip = aSQL.Length

        If ip < aSQL.Length Then
            Remainder = Mid(aSQL, ip + 1)
        Else
            Remainder = " "
        End If

        NewSQL = Mid(aSQL, 1, ip)

        'Loop to process more than one comparison if there are commas
        Do
            pp = aColValue.ToUpper.IndexOf(gOrSymbol)
            If pp < 1 Then pp = aColValue.ToUpper.IndexOf(gOrSymbol2)
            If pp > 0 Then
                Part = True
                OrClause = True
                partvalue = Mid(aColValue, 1, pp)
                If aColValue.ToUpper.IndexOf(gOrSymbol) > 0 Then
                    aColValue = Mid(aColValue, pp + 1 + gOrSymbol.Length)
                Else
                    aColValue = Mid(aColValue, pp + 1 + gOrSymbol2.Length)
                End If
            Else
                pp = aColValue.ToUpper.IndexOf(gAndSymbol)
                If pp > 0 Then
                    Part = True
                    OrClause = False
                    partvalue = Mid(aColValue, 1, pp)
                    aColValue = Mid(aColValue, pp + 1 + gAndSymbol.Length)
                Else
                    partvalue = aColValue
                End If
            End If
            '...determine if this part contains a built-in function (SDC 04/03/2014)
            UsesBuiltIn = (partvalue.ToUpper.IndexOf("DATEADD") >= 0)
            '...replace "TODAY" in a date field with today's date, "NOW" with the current time
            'NOTE: if we're using a built-in function, we need the single quotes (SDC 04/03/2014)
            If Left(aColType.ToLower, 1) = "d" Then
                '...keep track if we're searching on time instead of date
                searchtime1 = (partvalue.IndexOf("NOW") >= 0)
                If UsesBuiltIn = True Then
                    partvalue = Replace(partvalue, "{TODAY}", "'" & Today.ToString("MM/dd/yyyy") & "'")
                    partvalue = Replace(partvalue, "TODAY", "'" & Today.ToString("MM/dd/yyyy") & "'")
                    partvalue = Replace(partvalue, "{NOW}", "'" & Now.ToString & "'")
                    partvalue = Replace(partvalue, "NOW", "'" & Now.ToString & "'")
                Else
                    partvalue = Replace(partvalue, "{TODAY}", Today.ToString("MM/dd/yyyy"))
                    partvalue = Replace(partvalue, "TODAY", Today.ToString("MM/dd/yyyy"))
                    partvalue = Replace(partvalue, "{NOW}", Now.ToString)
                    partvalue = Replace(partvalue, "NOW", Now.ToString)
                End If
            End If
            '...if there's no built-in function, let's clean the string (SDC 04/03/2014)
            'NOTE: If a built-in function is used, leave the single quotes alone.
            If UsesBuiltIn = False Then partvalue = PrepareSQLSearchString(partvalue)

            secondpartvalue = ""
            '   Set comparison operator
            'If partvalue.IndexOf("?") > -1 Then     '...just use MATCHES if any question mark is included
            '    comparison = " MATCHES "
            'Else
            If partvalue.IndexOf("*") > -1 Or partvalue.IndexOf("?") > -1 Then
                comparison = " LIKE "
                partvalue = Replace(partvalue, "*", "%")
                partvalue = Replace(partvalue, "?", "_")
            End If
            'End If
            If partvalue.ToUpper.IndexOf(gBetweenSymbol) > -1 Then  'BHS 8/27/07
                comparison = " BETWEEN "
                secondpartvalue = Mid(partvalue, partvalue.IndexOf(gBetweenSymbol) + gBetweenSymbol.Length + 1)
                partvalue = Mid(partvalue, 1, partvalue.IndexOf(gBetweenSymbol))
                '...since this has a second part, redefine searchtime1 and searchtime2
                searchtime1 = (partvalue.IndexOf(":") > 8)
                searchtime2 = (secondpartvalue.IndexOf(":") > 8)
            End If
            If partvalue.IndexOf(">=") > -1 Then
                comparison = " >= "
                partvalue = Mid(partvalue, partvalue.IndexOf(">=") + 3)
            End If
            If partvalue.IndexOf("<=") > -1 Then
                comparison = " <= "
                partvalue = Mid(partvalue, partvalue.IndexOf("<=") + 3)
            End If
            If partvalue.IndexOf("<>") > -1 Then
                comparison = " <> "
                If partvalue.IndexOf("%") > -1 Then comparison = " Not Like " 'BHS 8/27/07
                partvalue = Mid(partvalue, partvalue.IndexOf("<>") + 3)
            End If
            If partvalue.IndexOf("<") > -1 Then
                comparison = " < "
                partvalue = Mid(partvalue, partvalue.IndexOf("<") + 2)
            End If
            If partvalue.IndexOf(">") > -1 Then
                comparison = " > "
                partvalue = Mid(partvalue, partvalue.IndexOf(">") + 2)
            End If
            If partvalue.ToUpper.IndexOf(gNullSymbol) > -1 Or partvalue.ToUpper.IndexOf(gNullSymbol2) > -1 Then
                comparison = " IS NULL "
                partvalue = " Empty "
            End If
            If partvalue.ToUpper.IndexOf(gNotNullSymbol) > -1 Or
               partvalue.ToUpper.IndexOf(gNotNullSymbol2) > -1 Or
               partvalue.ToUpper.IndexOf(gNotNullSymbol3) > -1 Or
               partvalue.ToUpper.IndexOf(gNotNullSymbol4) > -1 Then
                comparison = " IS NOT NULL "
                partvalue = " Not Empty "
            End If

            '   Set addition to the where clause
            If aSQL.ToLower.IndexOf(" where ") > 0 Then
                whereclause = " AND "
            Else
                If aSQL.Length > 0 Or aTrimStartingWithWhere = False Then
                    whereclause = " WHERE "
                Else
                    whereclause = ""
                End If
            End If

            If Part Then  'Multipart value (commas or pluses)
                If FirstPass Then
                    whereclause += "("
                    FirstPass = False
                Else
                    If OrClause Then
                        whereclause = " OR " 'Replace AND with OR
                    Else
                        whereclause = " AND "
                    End If
                End If
            End If

            'Add column name and value to NewSQL
            Select Case Left(aColType.ToLower, 1)
                Case "d"
                    If comparison = " IS NULL " Then
                        NewSQL &= whereclause & aColName & " IS NULL "
                    ElseIf comparison = " IS NOT NULL " Then
                        NewSQL &= whereclause & aColName & " IS NOT NULL "
                    Else
                        '...handle differently depending on whether we're using a built-in function (SDC 04/03/2014)
                        Dim dt As String = ""
                        If UsesBuiltIn = False Then
                            'BHS 11/21/07 force dates into a standard MM/dd/yyyy format
                            If searchtime1 = True Then
                                If IsDate(partvalue) Then dt = CDate(partvalue).ToString("MM/dd/yyyy hh:mm:ss")
                            Else
                                dt = FormatDateString(partvalue.Trim, "MM/dd/yyyy")
                            End If
                        Else
                            '...check that all parts of DATEADD are correct
                            If partvalue.ToUpper.IndexOf("DATEADD") >= 0 Then
                                wstr = partvalue.Trim
                                wstr9 = ParseStr(wstr, "(") & "("
                                wstr2 = ParseStr(wstr, ",").ToUpper
                                If wstr2 = "DAY" Or wstr2 = "MONTH" Or wstr2 = "YEAR" Then
                                    wstr9 = wstr9 & wstr2 & ","
                                    wstr2 = ParseStr(wstr, ",")
                                    If IsNumeric(wstr2) Then
                                        wstr9 = wstr9 & wstr2 & ","
                                        wstr = Replace(Replace(wstr, "'", ""), ")", "") '...strip out ")" and quotes
                                        If IsDate(wstr) Then dt = wstr9 & "'" & wstr & "')"
                                    End If
                                End If
                            Else
                                '...strip quotes out to check if date, and then add quotes back in
                                wstr = Replace(partvalue.Trim, "'", "").Trim
                                If IsDate(wstr) Then dt = "'" & wstr & "'"
                            End If
                        End If

                        '...handle differently depending on whether we're using a built-in function (SDC 04/03/2014)
                        Dim dt2 As String = ""
                        If UsesBuiltIn = False Then
                            'BHS 11/21/07 force dates into a standard MM/dd/yyyy format
                            If searchtime2 = True Then
                                If IsDate(secondpartvalue) Then dt2 = CDate(secondpartvalue).ToString("MM/dd/yyyy hh:mm:ss")
                            Else
                                dt2 = FormatDateString(secondpartvalue.Trim, "MM/dd/yyyy")
                            End If
                        Else
                            '...check that all parts of DATEADD are correct
                            If secondpartvalue.ToUpper.IndexOf("DATEADD") >= 0 Then
                                wstr = secondpartvalue.Trim
                                wstr9 = ParseStr(wstr, "(") & "("
                                wstr2 = ParseStr(wstr, ",").ToUpper
                                If wstr2 = "DAY" Or wstr2 = "MONTH" Or wstr2 = "YEAR" Then
                                    wstr9 = wstr9 & wstr2 & ","
                                    wstr2 = ParseStr(wstr, ",")
                                    If IsNumeric(wstr2) Then
                                        wstr9 = wstr9 & wstr2 & ","
                                        wstr = Replace(Replace(wstr, "'", ""), ")", "") '...strip out ")" and quotes
                                        If IsDate(wstr) Then dt2 = wstr9 & "'" & wstr & "')"
                                    End If
                                End If
                            Else
                                '...strip quotes out to check if date, and then add quotes back in
                                If secondpartvalue > "" Then
                                    wstr = Replace(secondpartvalue.Trim, "'", "").Trim
                                    If IsDate(wstr) Then dt2 = "'" & wstr & "'"
                                End If
                            End If
                        End If

                        Dim DatesOK As Boolean = True
                        If dt.Length = 0 Then DatesOK = False

                        If UsesBuiltIn = False Then
                            If dt.Length > 0 Then
                                If IsDate(dt) Then
                                    If CDate(dt) < CDate("1/1/1900") Then DatesOK = False
                                    If CDate(dt) > CDate("12/31/2099") Then DatesOK = False
                                Else
                                    DatesOK = False
                                End If
                            End If

                            If secondpartvalue.Length > 0 And dt2.Length > 0 Then
                                If IsDate(dt2) Then
                                    If CDate(dt2) < CDate("1/1/1900") Then DatesOK = False
                                    If CDate(dt2) > CDate("12/31/2099") Then DatesOK = False
                                Else
                                    DatesOK = False
                                End If
                            End If
                        End If

                        If DatesOK Then
                            '...handle differently depending on whether we're using a built-in function (SDC 04/03/2014)
                            If UsesBuiltIn = False Then
                                NewSQL &= whereclause + aColName + comparison + "'" & dt & "'"
                                If secondpartvalue.Length > 0 Then
                                    NewSQL &= " And " & "'" & dt2 & "'"
                                End If
                            Else
                                '...dt and dt2 contain a built-in function
                                NewSQL &= whereclause + aColName + comparison + dt
                                If secondpartvalue.Length > 0 Then
                                    NewSQL &= " And " & dt2
                                End If
                            End If
                        Else 'Invalid date forces zero population
                            Dim frmDate As New fSyntaxError("Invalid date: " & partvalue.Trim & "   " & secondpartvalue.Trim, "")
                            frmDate.ShowDialog()
                            gQBESyntaxError = True
                            NewSQL += whereclause + aColName + " > '1/1/1999' and " + aColName + " < '1/1/1999'"
                        End If
                    End If

                        '---BHS 4/7/11  replace handling of strings to incorporate MYUPPER
                        '   MYUPPER gets changed to UPPER in CheckSQLLength() 
                        '   when called from SQL Server routines

                        'Case "s"
                        'If comparison = " IS NULL " Then
                        '    NewSQL &= whereclause & aColName.Trim & " IS NULL "
                        'ElseIf comparison = " IS NOT NULL " Then
                        '    NewSQL &= whereclause & aColName.Trim & " IS NOT NULL "
                        'Else
                        '    'BHS 8/10/10 force upper case compare to make Informix not case sensitive.  SQL Server
                        '    'tolerates this syntax as well, although the VS Server Explorer doesn't.
                        '    'BHS 2/3/11 Take out UPPER phrase, because it will be ruin performance when users search on indexed fields.  Instead, the programmer must explicitly program UPPER when case insensitivity is required in certain fields.
                        '    'NewSQL += whereclause + "UPPER(" + aColName.Trim + ")" + _
                        '    '   comparison + "'" + partvalue.Trim.ToUpper + "'"
                        '    'If secondpartvalue.Length > 0 Then
                        '    '    NewSQL &= " And " & "'" & secondpartvalue.Trim.ToUpper & "'"
                        '    'End If
                        '    NewSQL += whereclause + aColName.Trim + _
                        '       comparison + "'" + partvalue.Trim + "'"
                        '    If secondpartvalue.Length > 0 Then
                        '        NewSQL &= " And " & "'" & secondpartvalue.Trim & "'"
                        '    End If
                        'End If


                Case "s"
                    If comparison = " IS NULL " Then
                        NewSQL &= whereclause & aColName & " IS NULL "
                    ElseIf comparison = " IS NOT NULL " Then
                        NewSQL &= whereclause & aColName & " IS NOT NULL "
                    Else
                        ' --- BHS 4/7/11 Do MYUPPER() logic if  
                        '   aColName in IfxCaseInsensitiveColumns ---

                        'Strip possible table name from aColName
                        wstr = aColName
                        Dim ColNameOnly As String = wstr.ToUpper
                        While ParseStr(wstr, ".").Length > 0
                            If wstr.Length > 0 Then ColNameOnly = wstr.ToUpper
                        End While

                        'Build gIfxCaseInsensitiveColumns if not filled in
                        '10/07/2013 SRM -- no need to add anything for case insensive fiedls if conntype is "SQL"
                        If gSQLConnStr.ToUpper().IndexOf("SQLPTS") > -1 And gIfxCaseInsensitiveColumns.Length = 0 And ConnType <> "SQL" Then
                            Dim DV As DataView =
                               SQLBuildDV("Select * FROM IfxCaseInsensitiveColumns", False)
                            If DV.Count = 0 Then
                                gIfxCaseInsensitiveColumns = "||"
                            Else
                                For Each R As DataRow In DV.Table.Rows
                                    gIfxCaseInsensitiveColumns &= "|" &
                                       GetItemString(R, "ColName").ToUpper & "|"
                                Next
                            End If
                        End If

                        'Use MYUPPER syntax if Ifx and aColName in gIfxCaseInsensitiveColumns
                        If gIfxCaseInsensitiveColumns.IndexOf("|" & ColNameOnly & "|") > -1 Then
                            NewSQL += whereclause + "MYUPPER(" + aColName + ")" +
                               comparison + "'" + partvalue.Trim.ToUpper + "'"
                            If secondpartvalue.Length > 0 Then
                                NewSQL &= " And " & "'" & secondpartvalue.Trim.ToUpper & "'"
                            End If
                        Else    'Standard SQL 
                            NewSQL += whereclause + aColName +
                         comparison + "'" + partvalue.Trim + "'"
                            If secondpartvalue.Length > 0 Then
                                NewSQL &= " And " & "'" & secondpartvalue.Trim & "'"
                            End If
                        End If
                    End If

                    ' --- End of BHS 4/7/11 change

                Case Else
                    If comparison = " IS NULL " Then
                        NewSQL &= whereclause & aColName & " IS NULL "
                    ElseIf comparison = " IS NOT NULL " Then
                        NewSQL &= whereclause & aColName & " IS NOT NULL "
                    ElseIf IsNumeric(partvalue.Trim) Then
                        NewSQL += whereclause + aColName + comparison + partvalue.Trim
                        If secondpartvalue.Length > 0 Then NewSQL &= " And " & secondpartvalue.Trim
                    Else 'invalid number forces zero population
                        Dim frmNum As New fSyntaxError("Invalid number: " & partvalue.Trim & "   " & secondpartvalue.Trim, "")
                        frmNum.ShowDialog()
                        gQBESyntaxError = True
                        NewSQL += whereclause + aColName + " > 0 and " + aColName + " < 0"
                    End If
            End Select

            'Add final ) if the end of a multi-part Value (with commas)
            If Part And pp = -1 Then
                NewSQL += ")"
            End If

        Loop While pp > 0   'Get another part

        'Add remainder of original SQL (e.g. Group By, Having, Order By)
        NewSQL += " " + Remainder

        'BHS 12/21/10 Remove beginning WHERE or AND if aSQL is ""
        If aSQL = "" Then

        End If


        Return NewSQL

    End Function

    ''' <summary> Add to SQL From Clause </summary>
    Function AddFrom(ByVal aSQL As String,
                     ByVal aTableName As String,
                     ByVal aWhereClause As String) As String
        Dim ip As Integer
        Dim Remainder As String

        'Add aTableName to From Clause
        ip = aSQL.ToLower.IndexOf("where")
        If ip < 0 Then ip = aSQL.ToLower.IndexOf("group by")
        If ip < 0 Then ip = aSQL.ToLower.IndexOf("having")
        If ip < 0 Then ip = aSQL.ToLower.IndexOf("order by")
        If ip < 0 Then
            aSQL += " , " + aTableName
        Else
            Remainder = Mid(aSQL, ip + 1)
            aSQL = Mid(aSQL, 1, ip) + " , " + aTableName + " " + Remainder
        End If

        'Add aWhereClause to Where Clause
        ip = aSQL.ToLower.IndexOf("group by")
        If ip < 0 Then ip = aSQL.ToLower.IndexOf("having")
        If ip < 0 Then ip = aSQL.ToLower.IndexOf("order by")
        If ip < 0 Then
            ip = Len(aSQL)
            Remainder = ""
        Else
            Remainder = Mid(aSQL, ip + 1)
        End If

        aSQL = Mid(aSQL, 1, ip)
        If aSQL.ToLower.IndexOf(" where ") < 0 Then
            aSQL += " Where "
        Else
            aSQL += " And "
        End If
        aSQL += " " + aWhereClause + " " + Remainder

        Return aSQL

    End Function

#End Region

#Region "------------------------ Data Table and Data Row Manipulation -----------------------"
    ''' <summary> Return True if row is not deleted or detached from the table </summary>
    Public Function RowIsDeleted(ByRef aRow As DataRow) As Boolean
        If aRow.RowState = DataRowState.Deleted Then Return True
        If aRow.RowState = DataRowState.Detached Then Return True
        Return False
    End Function

#End Region

#Region "------------------------ String Manipulation Functions -----------------------"

    ''Create Time String
    'Public Function TimeString(ByVal aHr As Integer, ByVal aMin As Integer, ByVal aSec As Integer) As String
    '    Dim S, S2 As String
    '    S2 = "0" & aHr.ToString
    '    If S2.Length > 2 Then S2 = S2(aHr, 2)
    '    S = S2
    '    S2 = "0" & 



    'End Function

    ''' <summary> Insert carriage returns in the first space after every aWidth characters </summary>
    Public Function WrapString(ByVal aStr As String,
                               ByVal aWidth As Integer) As String
        If aWidth < 1 Or aWidth > 500 Then Return aStr
        Dim i As Integer = aWidth
        While i < aStr.Length
            aStr = Mid(aStr, 1, i) & Chr(13) & Mid(aStr, i + 1)
            i = i + aWidth
        End While
        Return aStr

    End Function

    ''' <summary> Encode a string to hex </summary>
    Public Function HexEncode(ByVal Value As String) As String

        'Used to Hexencode a string to be passed in the URL, 
        'to avoid problems with special characters like %

        Dim X As Integer
        Dim ReturnStr As String = String.Empty

        ' Convert to Hex
        For X = 1 To Len(Value)
            ReturnStr = ReturnStr & Uri.HexEscape(CChar(Mid(Value, X, 1)))
        Next

        Return ReturnStr

    End Function

    ''' <summary> Right Trim character columns in a Table </summary>
    Function TrimTable(ByRef aT As DataTable) As Boolean

        Dim Row As Integer
        Dim Col As Integer
        Dim RowCount As Integer = aT.Rows.Count - 1
        Dim ColCount As Integer = aT.Columns.Count - 1

        For Row = 0 To RowCount
            If Not aT.Rows(Row).RowState = DataRowState.Deleted Then
                For Col = 0 To ColCount
                    If aT.Rows(Row)(Col).GetType().Name.ToLower = "string" Then
                        aT.Rows(Row)(Col) = aT.Rows(Row)(Col).ToString.Trim
                    End If
                Next
            End If
        Next

        Return True

    End Function

    'TODO BHS Test this with debugger
    ''' <summary> Find last occurance of a character in a string </summary>
    Function IndexOfLast(ByVal aChar As String,
                         ByVal aStr As String) As Integer
        Dim i, j As Integer

        i = aStr.IndexOf(aChar)
        If i = -1 Then Return i

        Do
            j = aStr.IndexOf(aChar, i + 1)
            If j = -1 Then Return i
            i = j
        Loop

    End Function

    ''' <summary> Remove the last character from a string, sent by reference </summary>
    Function RemoveLastChar(ByRef aStr As String) As Boolean
        Dim pos As Integer

        pos = Len(aStr)

        If pos > 0 Then aStr = Left(aStr, pos - 1)

        Return True

    End Function

    ''' <summary> Remove the last characters from a string, sent by reference </summary>
    Function RemoveLastChar(ByRef aStr As String,
                            ByVal aRemoveAmt As Integer) As Boolean
        Dim pos As Integer

        pos = Len(aStr)

        If pos > (aRemoveAmt - 1) Then aStr = Left(aStr, pos - aRemoveAmt)

        Return True

    End Function

    'BHS 2/27/08 Replaced with code below, to replace string ONLY ONCE.  Use SubstituteAllStr to replace all occurances
    ''Substitute aToStr for aFromStr once in aStr
    'Function SubstituteStr(ByRef aStr As String, ByVal aFromStr As String, ByVal aToStr As String) As String
    '    Dim pos, size As Integer
    '    Dim str As String = aStr

    '    Do While True

    '        pos = str.IndexOf(aFromStr)
    '        size = Len(aFromStr)

    '        If pos < 0 Then Return str
    '        If pos = 1 Then
    '            str = aToStr + Mid(str, size + 1)
    '        Else
    '            str = Mid(str, 1, pos) + aToStr + Mid(str, pos + size + 1)
    '        End If

    '    Loop

    '    aStr = str  'Set aStr in case programmer expects to use that
    '    Return str

    'End Function

    ''' <summary> Substitute aToStr for aFromStr in aStr </summary>
    Function SubstituteStr(ByRef aStr As String,
                           ByVal aFromStr As String,
                           ByVal aToStr As String) As String
        Dim pos, size As Integer

        pos = aStr.IndexOf(aFromStr)
        size = Len(aFromStr)

        If pos < 0 Then Return aStr
        If pos = 1 Then
            aStr = aToStr + Mid(aStr, size + 1)
        Else
            aStr = Mid(aStr, 1, pos) + aToStr + Mid(aStr, pos + size + 1)
        End If

        Return aStr

    End Function

    ''' <summary> Substitute aToStr for aFromStr for all occurances in aStr </summary>
    Function SubstituteAllStr(ByRef aStr As String,
                              ByVal aFromStr As String,
                              ByVal aToStr As String) As String
        Dim pos, size As Integer
        Dim str As String = aStr

        Do While True

            pos = str.IndexOf(aFromStr)
            size = Len(aFromStr)

            If pos < 0 Then
                aStr = str      'Set reference parameter
                Return str
            End If

            If pos = 1 Then
                str = aToStr + Mid(str, size + 1)
            Else
                str = Mid(str, 1, pos) + aToStr + Mid(str, pos + size + 1)
            End If

        Loop

        aStr = str  'BHS 8/27/08
        Return str

    End Function


    ''' <summary> Removes multiple instances of a string (not tested yet) </summary>
    Function RemoveStr(ByVal aFullStr As String,
                       ByVal aRemoveStr As String) As String
        Dim size As Integer = 10000

        While Len(aFullStr) < size
            size = Len(aFullStr)
            aFullStr = SubstituteStr(aFullStr, aRemoveStr, "")
        End While

        Return aFullStr

    End Function

    ''' <summary> Returns value up to aDivider, and left truncates aStr through aDivider.  aStartIndex starts searching at that position</summary>
    Function ParseStr(ByRef aStr As String, ByVal aDivider As String, Optional ByVal aStartIndex As Integer = 0) As String
        Dim divlen, pos As Integer
        Dim RetStr As String

        If aStartIndex > aStr.Length Then
            RetStr = aStr
            aStr = ""
            Return RetStr
        End If
        divlen = aDivider.Length

        pos = aStr.IndexOf(aDivider, aStartIndex)
        If pos = 0 Then     'BHS 7/10/08     from pos > 0, to allow for divider as first character
            'aStr = Mid(aStr, 2)    'SDC 4/9/09
            aStr = Mid(aStr, divlen + 1)
            Return ""
        End If
        If pos > 0 Then
            RetStr = Left(aStr, pos)
            'aStr = Mid(aStr, pos + 2)  'SDC 4/9/09
            aStr = Mid(aStr, pos + divlen + 1)
        Else
            RetStr = aStr
            aStr = ""
        End If

        Return RetStr
    End Function

    ''' <summary> Returns value after aDivider, and right truncates aStr starting at aDivider </summary>
    Function RightParseStr(ByRef aStr As String,
                           ByVal aDivider As String) As String
        Dim pos, lastpos As Integer
        Dim RetStr As String = aStr

        lastpos = 0

        If aStr.Length < lastpos + 2 Then
            aStr = ""
            Return RetStr
        End If

        While aStr.Length >= lastpos + 2
            pos = aStr.IndexOf(aDivider, lastpos + 2)
            If pos > 0 Then
                lastpos = pos
                Continue While
            Else
                If lastpos > 0 Then
                    RetStr = Mid(aStr, lastpos + 2)
                    aStr = Left(aStr, lastpos)
                Else
                    RetStr = aStr
                    aStr = ""
                End If
            End If
        End While

        Return RetStr
    End Function

    'BHS 8/18/08
    ''' <summary> Synonym for PrepareSQLSearchString, doubles up apostrophes for inclusion in SQL statements </summary>
    Function Clean(ByVal aStr As String) As String
        Return PrepareSQLSearchString(aStr)
    End Function

    'We may want to add other reserved characters here such as colon, comma
    ''' <summary> Prepare SQL Search String by removing single apostrophes, ... </summary>
    Function PrepareSQLSearchString(ByVal aStr As String) As String
        Dim pos1, pos2 As Integer

        If aStr = "" Then Return aStr
        'Change all apostrophes to double apostrophes
        pos1 = aStr.IndexOf("'")
        pos2 = -1   'position in string to start search 'BHS changed from 0 5/29/07

        Do While pos1 > pos2
            aStr = Left(aStr, pos1) + "''" + Mid(aStr, pos1 + 2)
            pos2 = pos1
            pos1 = aStr.IndexOf("'", pos2 + 2)
        Loop

        'Return prepared string
        Return aStr

    End Function

    ''' <summary> When displaying a string, insert double ampersands so ampersands don't appear as underlines ''' </summary>
    Function CleanAmpersand(ByVal aStr As String) As String
        Return Replace(aStr, "&", "&&")
    End Function

    ''' <summary> Convert a string to a decimal.  If the string is not numeric, return 0 </summary>
    Function ToDec(ByVal aStr As String) As Decimal
        Dim amt As Decimal

        If IsNumeric(aStr) Then
            amt = CType(aStr, Decimal)
        Else
            amt = 0
        End If

        Return amt

    End Function

    ''' <summary> Return True if strings from two datafields of the same type are functionally the same </summary>
    Function StringCompare(ByVal aStr1 As String,
                           ByVal aStr2 As String,
                           ByVal aColumnType As String,
                           ByVal aControlType As String) As Boolean

        ' If strings are different...
        If (aStr1 Is Nothing And Not aStr2 Is Nothing And aStr2 <> "") Or
                      (Not aStr1 Is Nothing And aStr2 Is Nothing And aStr1 <> "") Or
                      (aStr1 <> aStr2) Then

            'But numbers are functionally the same, compare is True
            If Left(aColumnType.ToLower, 3) = "num" Then
                If NumberCompare(aStr1, aStr2) Then Return True
            End If

            'But dates are functionally the same, compare is True
            If Left(aColumnType.ToLower, 3) = "dat" Then
                If DateCompare(aStr1, aStr2) Then Return True
            End If

            'If checkbox, allow 0 = False, 1 = True
            If Left(aControlType.ToLower, 3) = "che" Then
                If Not IsNothing(aStr1) And Not IsNothing(aStr2) Then
                    If aStr1.ToLower = "false" And aStr2 = "0" Then Return True
                    If aStr1.ToLower = "true" And aStr2 = "1" Then Return True
                    If aStr2.ToLower = "false" And aStr1 = "0" Then Return True
                    If aStr2.ToLower = "true" And aStr1 = "1" Then Return True
                End If
            End If

            'Otherwise strings are different, compare fails
            Return False

        End If

        'Strings are identical
        Return True

    End Function

    'This may remove somc confusion about the aControlType parameter - checkbox, qcheckbox, etc.
    ''' <summary> Return True if strings from two datafields of the same type are functionally the same </summary>
    Function StringCompare(ByVal aStr1 As String,
                           ByVal aStr2 As String,
                           ByVal aColumnType As String,
                           ByVal aIsCheckBox As Boolean) As Boolean
        If aIsCheckBox = True Then
            Return StringCompare(aStr1, aStr2, aColumnType, "che")
        Else
            Return StringCompare(aStr1, aStr2, aColumnType, "not")
        End If


    End Function

    'This may remove somc confusion about the aControlType parameter - checkbox, qcheckbox, etc.
    ''' <summary> Return True if strings from two datafields of the same type are functionally the same </summary>
    Function StringCompare(ByVal aStr1 As String,
                           ByVal aStr2 As String,
                           ByVal aColumnType As DataTypeEnum,
                           ByVal aIsCheckBox As Boolean) As Boolean
        Dim ColType As String = "str"
        If aColumnType = DataTypeEnum.Dat Then ColType = "dat"
        If aColumnType = DataTypeEnum.Num Then ColType = "num"

        Return StringCompare(aStr1, aStr2, ColType, aIsCheckBox)

    End Function


    ''' <summary> Return True if strings are functionally the same (e.g. Trimmed strings, equivalent numbers, equivalent dates) </summary>
    Function StringCompare(ByVal aStr1 As String,
                           ByVal aStr2 As String) As Boolean

        If Trim(aStr1) = Trim(aStr2) Then Return True

        If IsNumeric(aStr1) And IsNumeric(aStr2) Then
            Dim a As Decimal = CType(aStr1, Decimal)
            Dim b As Decimal = CType(aStr2, Decimal)
            If a = b Then Return True
        End If

        If IsDate(aStr1) And IsDate(aStr2) Then
            Dim a As Date = CType(aStr1, Date)
            Dim b As Date = CType(aStr2, Date)
            If a = b Then Return True
        End If

        Return False
    End Function

    ''' <summary> Compare dates.  Return true if they are functionally the same </summary>
    Function DateCompare(ByVal aStr1 As String,
                         ByVal aStr2 As String) As Boolean

        If IsDate(aStr1) And IsDate(aStr2) Then
            If CType(aStr1, Date) = CType(aStr2, Date) Then Return True
        End If

        Return False
    End Function

    ''' <summary> Compare numbers.  Return True only if both strings are equivalent valid numbers </summary>
    Function NumberCompare(ByVal aStr1 As String,
                           ByVal aStr2 As String) As Boolean

        If aStr1 <= " " Then aStr1 = "0"
        If aStr2 <= " " Then aStr2 = "0"

        If IsNumeric(aStr1) And IsNumeric(aStr2) Then
            If CType(aStr1, Single) = CType(aStr2, Single) Then Return True
        End If

        Return False

    End Function

    ''' <summary> Convert an aDS value to string (handles DRVersion 'original' and 'current' </summary>
    Function DBColToString(ByVal aDS As System.Data.DataSet,
                           ByVal aTab As String,
                           ByVal aCol As String,
                           ByVal aRow As Integer,
                           ByVal aDRVersion As String) As String
        Dim O As Object

        If aDRVersion.ToLower = "original" Then
            If aDS.Tables(aTab).Rows(aRow).RowState = DataRowState.Added Then Return ""
            O = aDS.Tables(aTab).Rows(aRow)(aCol, DataRowVersion.Original)
        Else
            O = aDS.Tables(aTab).Rows(aRow)(aCol)
        End If

        If IsDBNull(O) Then Return ""
        Return CType(O, String).Trim

    End Function

    ''' <summary> Convert a aDS value to number.  Return 0 if value is dbnull or doesn't convert to a number. </summary>
    Function DBColToNum(ByVal aDS As System.Data.DataSet,
                        ByVal aTab As String,
                        ByVal aCol As String,
                        ByVal aRow As Integer,
                        ByVal aDRVersion As String) As Decimal
        Dim ws As String

        ws = DBColToString(aDS, aTab, aCol, aRow, aDRVersion)
        If ws > " " And IsNumeric(ws) Then Return CDec(ws)
        Return 0

    End Function

    ''' <summary> Accept a DB Value into a number.  Return 0 if DBNull or not a number.  </summary>
    Function DBColToNum(ByVal aItem As Object) As Object
        If IsDBNull(aItem) Then Return Nothing
        Return aItem
    End Function

    ''' <summary> Return 0 if we can't derive a number from this object </summary>
    Function NumberOrZero(ByVal aObject As Object) As Decimal
        If aObject Is Nothing Then Return 0
        If aObject Is DBNull.Value Then Return 0
        If IsNumeric(aObject) Then Return CType(aObject, Decimal)
        Return 0
    End Function

    ''' <summary> Left pad number with spaces so that sorting will be numeric in an alpha column.  Not sure this works with negatives. </summary>
    Function LeftPadNumber(ByVal aNum As Decimal,
                           Optional ByVal aFormat As String = "N2",
                           Optional ByVal aLength As Integer = 14) As String
        Dim S As String = aNum.ToString
        If aFormat <> "" Then S = Format(aNum, aFormat)
        While aLength > S.Length
            S = " " & S
        End While
        Return S
    End Function

    ''' <summary> Remove commas (often from a formatted number) </summary>
    Function StripCommas(ByRef aStr As String) As String
        Dim pos1, pos2 As Integer

        'Remove all commas
        pos1 = aStr.IndexOf(",")
        pos2 = 0    'position in string to start search

        Do While pos1 > pos2
            aStr = Left(aStr, pos1) + Mid(aStr, pos1 + 2)
            pos2 = pos1
            pos1 = aStr.IndexOf(",", pos2 + 2)
        Loop

        'Return prepared string
        Return aStr
    End Function

    ''' <summary> Return a number from a datarow item.  Return 0 if can't calculate </summary>
    Function GetItemNumber(ByVal aRow As DataRow,
                           ByVal aItem As String) As Decimal

        If aRow IsNot Nothing Then
            If aRow.Item(aItem) IsNot Nothing AndAlso aRow.Item(aItem) IsNot DBNull.Value Then
                If IsNumeric(aRow.Item(aItem)) Then Return CType(aRow.Item(aItem), Decimal)
            End If
        End If
        Return 0

    End Function

    ''' <summary> Return a number from a datagridviewrow item.  Return 0 if can't calculate </summary>
    Function GetItemNumber(ByVal aRow As DataGridViewRow,
                           ByVal aItem As String) As Decimal

        If aRow IsNot Nothing Then
            If aRow.Cells(aItem) IsNot Nothing AndAlso aRow.Cells(aItem).Value IsNot DBNull.Value Then
                If IsNumeric(aRow.Cells(aItem).Value) Then Return CType(aRow.Cells(aItem).Value, Decimal)
            End If
        End If
        Return 0

    End Function


    'Return a number from a datarow item.  Return 0 if can't calculate
    Function GetItemString(ByVal aRow As DataRow, ByVal aItem As String, Optional ByVal aOrig As Boolean = False) As String

        If aRow IsNot Nothing Then
            If aRow.Item(aItem) IsNot Nothing AndAlso aRow.Item(aItem) IsNot DBNull.Value Then
                If aOrig = True Then
                    Return CType(aRow.Item(aItem, DataRowVersion.Original), String)
                Else
                    Return CType(aRow.Item(aItem), String)
                End If

            End If
        End If
        Return ""

    End Function

    'BHS 2/26/17 Removed OTT version, added QSIServer version above.
    ''BHS 4/21/09 Added optional aVersion
    ''BHS 7/7/10 Assume user wants to trim, but allow not trimming
    '''' <summary> Return a string from a datarow item.  Return "" if Nothing or DBNull </summary>
    'Function GetItemString(ByVal aRow As DataRow,
    '                       ByVal aItem As String,
    '                       Optional ByVal aVersion As DataRowVersion = DataRowVersion.Current,
    '                       Optional ByVal aTrim As Boolean = True) As String

    '    If aRow IsNot Nothing Then
    '        If aRow.HasVersion(aVersion) = True Then ' GBV 3/17/2011
    '            If aTrim = True Then
    '                Return aRow.Item(aItem, aVersion).ToString.Trim
    '            Else
    '                Return aRow.Item(aItem, aVersion).ToString
    '            End If
    '        Else
    '            If aTrim = True Then
    '                Return aRow.Item(aItem).ToString.Trim
    '            Else
    '                Return aRow.Item(aItem).ToString
    '            End If
    '        End If
    '    End If
    '    Return ""

    'End Function


    'Return a number from a datagridviewrow item.  Return 0 if can't calculate
    'Function GetItemString(ByVal aRow As DataGridViewRow, ByVal aItem As String) As String

    '    If aRow IsNot Nothing Then
    '        If RowHasNamedColumn(aRow, aItem) Then
    '            If aRow.Cells(aItem) IsNot Nothing AndAlso aRow.Cells(aItem) IsNot DBNull.Value Then
    '                Return aRow.Cells(aItem).Value.ToString
    '            End If
    '        End If
    '    Else
    '        MsgBox("Programmer Error - Column Name not found in row")
    '        Return ""
    '    End If
    '    Return ""

    'End Function




    'BHS 7/7/10 Assume user wants to trim, but allow not trimming
    ''' <summary> Return a string from a datarow item.  Return "" if Nothing or DBNull </summary>
    Function GetItemString(ByVal aRow As DataGridViewRow,
                           ByVal aItem As String,
                           Optional ByVal aTrim As Boolean = True) As String

        If aRow IsNot Nothing Then
            If RowHasNamedColumn(aRow, aItem) Then
                If aRow.Cells(aItem) IsNot Nothing AndAlso aRow.Cells(aItem).Value IsNot Nothing AndAlso
                   aRow.Cells(aItem).Value IsNot DBNull.Value Then
                    If aTrim = True Then
                        Return aRow.Cells(aItem).Value.ToString.Trim
                    Else
                        Return aRow.Cells(aItem).Value.ToString
                    End If

                End If
            Else
                MsgBox("Programmer Error - Column Name not found in row")
                Return ""
            End If
        End If
        Return ""

    End Function

    ''' <summary> Row Has Named Column - returns true if named column exists in the row </summary>
    Function RowHasNamedColumn(ByVal aR As DataGridViewRow,
                               ByVal aColName As String) As Boolean
        For Each C As DataGridViewCell In aR.Cells
            If C.OwningColumn.Name = aColName Then Return True
        Next
        Return False
    End Function

    ''' <summary> Return a date from a datarow item.  Return 1/1/1900 is can't calculate </summary>
    Function GetItemDate(ByVal aRow As DataRow, ByVal aItem As String) As Date
        Return GetItemDate(aRow, aItem, DataRowVersion.Current)

    End Function

    'Handles versions: original current, default, and proposed
    ''' <summary> Return a date from a datarow item.  Return 1/1/1900 if we can't calculate </summary>
    Function GetItemDate(ByVal aRow As DataRow,
                         ByVal aItem As String,
                         ByVal aVersion As DataRowVersion) As Date

        If aRow IsNot Nothing Then
            ' GBV 3/17/2011
            If aRow.HasVersion(aVersion) = True Then
                If Not IsDBNull(aRow.Item(aItem, aVersion)) Then
                    If IsDate(aRow.Item(aItem, aVersion).ToString) Then
                        Return Date.Parse(aRow.Item(aItem, aVersion).ToString)
                    End If
                End If
            Else
                If Not IsDBNull(aRow.Item(aItem)) Then
                    If IsDate(aRow.Item(aItem).ToString) Then
                        Return Date.Parse(aRow.Item(aItem).ToString)
                    End If
                End If
            End If
        End If

        Return Date.Parse("1/1/1900")

    End Function

    ''' <summary> Return a date from a datagridviewrow item.  Return 1/1/1900 is can't calculate </summary>
    Function GetItemDate(ByVal aRow As DataGridViewRow,
                         ByVal aItem As String) As Date

        If aRow IsNot Nothing Then
            If aRow.Cells(aItem) IsNot Nothing AndAlso aRow.Cells(aItem).Value IsNot DBNull.Value Then
                If IsDate(aRow.Cells(aItem).Value) Then Return CType(aRow.Cells(aItem).Value, Date)
            End If
        End If
        Return CType("1/1/1900", Date)

    End Function

    ' Used in building SQL Where Clause 
    ''' <summary> Return a date in MM/dd/yyyy format </summary>
    Function FormatDateString(ByVal aDate As String,
                              Optional ByVal aFormat As String = "MM/dd/yyyy") As String
        Dim str As String = ""
        If Not IsDate(aDate) Then Return ""
        Dim dt As Date = Date.Parse(aDate, gDateFormatInterface)
        Return Format(dt, aFormat)
    End Function

    ''' <summary> Format date (default MM/dd/yyyy) - note this returns an object, whereas DateToStr returns a string </summary>
    Function FormatDate(ByRef aOb As Object) As Object
        Return FormatDate(aOb, "MM/dd/yyyy")
    End Function

    ''' <summary> Format date where caller specifies the format </summary>
    Function FormatDate(ByRef aOb As Object,
                        ByVal aFormat As String) As Object
        'Return blank if input string is DENull, empty, "", or not a date
        If IsDBNull(aOb) Then Return aOb
        If aOb Is Nothing Then Return aOb

        Dim D As Date
        If IsDate(aOb) Then
            D = CType(aOb, Date)
        Else
            Return aOb
        End If
        'Otherwise, return in aFormat
        Return Format(D, aFormat)

    End Function

    ''' <summary> Format date where caller specifies the format </summary>
    Function FormatDate(ByRef aDt As Date,
                        Optional ByVal aFormat As String = "MM/dd/yyyy") As String
        'Return blank if input string is DENull, empty, "", or not a date
        If IsDBNull(aDt) Then Return ""
        'If aDt Is Nothing Then Return aDt

        'Otherwise, return in aFormat
        Return Format(aDt, aFormat)

    End Function

    ''' <summary> True if we can search on this keyvalue </summary>
    Function KeyValueSearchable(ByVal aKeyValue As Integer) As Boolean
        Dim OK As Boolean = True
        If aKeyValue < 41 Then OK = False
        If aKeyValue = 8 Then OK = True
        Return OK
    End Function

    ''' <summary> Add a Name/Value pair to the end of a string </summary>
    Sub AddPairToList(ByRef aStr As String,
                      ByVal aFirst As String,
                      ByVal aSecond As String,
                      Optional ByVal aCompare As String = "=",
                      Optional ByVal aSeparator As String = ", ")
        If aStr.Length > 0 Then aStr &= aSeparator
        aStr &= aFirst & aCompare & aSecond
    End Sub

    Function NowToFileNamePhrase() As String
        Return Format(Now(), "yyyy-MM-dd-HH-mm-ssff")
    End Function


    ''' <summary> Translates OEM keycode values to regular strings </summary>
    Function TranslateKeyCode(ByVal e As System.Windows.Forms.KeyEventArgs) As String

        If e.KeyValue = 186 Then
            If e.Shift = True Then Return ":"
            Return ";"
        End If

        If e.KeyValue = 187 Then
            If e.Shift = True Then Return "+"
            Return "="
        End If

        If e.KeyValue = 188 Then
            If e.Shift = True Then Return "<"
            Return ","
        End If

        If e.KeyValue = 189 Then
            If e.Shift = True Then Return "_"
            Return "-"
        End If

        If e.KeyValue = 190 Then
            If e.Shift = True Then Return ">"
            Return "."
        End If

        If e.KeyValue = 191 Then
            If e.Shift = True Then Return "?"
            Return "/"
        End If

        If e.KeyValue = 192 Then
            If e.Shift = True Then Return "~"
            Return "`"
        End If

        If e.KeyValue = 219 Then
            If e.Shift = True Then Return "{"
            Return "["
        End If

        If e.KeyValue = 220 Then
            If e.Shift = True Then Return "?"
            Return "\"
        End If

        If e.KeyValue = 221 Then
            If e.Shift = True Then Return "}"
            Return "]"
        End If

        If e.KeyValue = 222 Then
            If e.Shift = True Then Return """"
            Return "'"
        End If

        Return Chr(e.KeyValue)
    End Function

#End Region

#Region "------------------------ Miscellaneous Functions -----------------------"

    ''' <summary> Stub for AddWatch, which can eventually be copied over from Oakland </summary>
    Public Sub AddWatch(ByVal aEvent As String, Optional ByVal aDescr As String = "", Optional ByVal aType As String = "")
        If gWatchOn = True Then
            SQLDoSQL("Insert Into Watch Values ('" & gUserName & "', GetDate(), '" & aEvent & "', '" & aDescr & "', '" & aType & "')")
        End If
    End Sub

    ''' <summary> This function returns an end-of-month date for the given date </summary>
    Public Function GetEOM(ByVal aDate As Date) As Date
        Dim mo, yr As Integer
        Dim retdate As Date

        mo = aDate.Month + 1
        yr = aDate.Year
        If mo > 12 Then
            mo = 1
            yr = yr + 1
        End If
        retdate = CType(mo.ToString & "/01/" & yr.ToString, Date).AddDays(-1)
        Return retdate
    End Function

    ''' <summary> Returns current ProcessID/Machine Name </summary>
    Function GetProcessID() As String
        Dim ID As Integer = Process.GetCurrentProcess.Id
        Dim MachName As String = My.Computer.Name
        Return ID.ToString & "/" & MachName
    End Function

    ''' <summary> Let other applications run for a while if physical memory is more than half used. </summary>
    Sub CedeCPUIfMemoryLow(Optional ByVal aRepeats As Integer = 1000)
        Dim d As Double = 0
        d = My.Computer.Info.AvailablePhysicalMemory / My.Computer.Info.TotalPhysicalMemory
        If d < 0.5 Then
            For i As Integer = 1 To aRepeats
                Application.DoEvents()
            Next
        End If
    End Sub

    ''' <summary> Make a control visible or not visble </summary>
    Sub ShowControl(ByVal aC As Control,
                    ByVal aVisible As Boolean)
        aC.Visible = aVisible
        aC.Refresh()
    End Sub

    ''' <summary> Warn the user if wildcards are entered in combination with wildcards, etc. </summary>
    Sub NoWildCards(ByVal aText As String)
        If aText.IndexOf("*") > -1 Or aText.IndexOf(">") > -1 Or aText.IndexOf("<") > -1 Or aText.IndexOf(gOrSymbol) > -1 Or aText.IndexOf(gAndSymbol) > -1 Then
            MsgBox("Warning - Wildcards won't work in this field")
        End If
    End Sub

    ''' <summary> Build a filter based on a gridview </summary>
    Public Function BuildGVFilter(ByVal aGV As DataGridView,
                                  ByVal aSearchStr As String) As String
        'Dim Str As String = PrepareSQLSearchString(aSearchStr.ToUpper)
        aSearchStr = PrepareSQLSearchString(aSearchStr.ToUpper)     'BHS 1/15/09
        Dim C As DataGridViewColumn
        Dim Filter As String = ""
        If aSearchStr = "" Then Return "" 'BHS 4/4/07

        For Each C In aGV.Columns
            Filter = BuildGVColFilter(Filter, aGV, C, aSearchStr)
        Next

        Return Filter


    End Function

    'BHS 9/3/08
    ''' <summary> BuildGVColFilter takes a starting filter string and adds a filter to an given GV Column </summary>
    Public Function BuildGVColFilter(ByVal aFilter As String,
                                     ByVal aGV As DataGridView,
                                     ByVal aC As DataGridViewColumn,
                                     ByVal aSearchStr As String) As String
        If Not IsNothing(aC.ValueType) Then
            Select Case Mid(aC.ValueType.Name.ToLower, 1, 3)
                Case "str", "obj"   'BHS 11/11/09 added "obj"
                    If aC.Visible = True Then    'BHS 3/12/07 Changed from aC.Displayed
                        If aFilter > " " Then aFilter &= " Or "
                        aFilter &= aC.DataPropertyName + " Like '%" + aSearchStr + "%'"
                    End If
                Case "dat"
                    If aC.Visible Then
                        If aSearchStr.Length > 5 AndAlso IsDate(aSearchStr) AndAlso
                           CType((aSearchStr), DateTime) > CType("1/1/1980", DateTime) Then
                            Dim D As Date = CType((aSearchStr), Date)
                            Dim DateStr As String = CType(D, String)
                            If D.Day > 0 Then
                                If aFilter > " " Then aFilter &= " Or "
                                aFilter &= "(" & aC.DataPropertyName & " >= #" + DateStr + " 12:00am# AND "
                                aFilter &= aC.DataPropertyName & " <= #" & DateStr & " 11:59pm#)"
                            End If
                        End If
                    End If
                    'BHS Add logic for boolean column here
                Case Else
                    If aC.Visible Then
                        If aSearchStr.Length > 0 AndAlso IsNumeric(aSearchStr) Then
                            Dim D As Decimal
                            Try
                                D = Convert.ToDecimal(aSearchStr)
                            Catch ex As Exception
                                Return aFilter
                            End Try
                            If aFilter > " " Then aFilter &= " Or "
                            aFilter &= aC.DataPropertyName & " = " & D.ToString
                        End If
                    End If
            End Select
        End If
        Return aFilter

    End Function

    'TODO RefreshGVProp logic appears in fBase, and possibly other places.  This should be consolidated
    ''' <summary> Refresh default DataGridView properties </summary>
    Sub RefreshGVProp(ByVal GV As DataGridView)
        GV.AllowUserToAddRows = False
        GV.AllowUserToDeleteRows = False
        GV.AllowUserToOrderColumns = True
        GV.RowTemplate.DefaultCellStyle.BackColor = Nothing
        GV.RowTemplate.DefaultCellStyle.ForeColor = Nothing
        GV.BackgroundColor = QListBackColor
        If CType(GV, qGVBase)._ShowSelectionBar = True Then
            GV.RowTemplate.DefaultCellStyle.SelectionBackColor = QSelectionBackColor
            GV.RowTemplate.DefaultCellStyle.SelectionForeColor = QSelectionForeColor
        Else
            GV.RowTemplate.DefaultCellStyle.SelectionBackColor = Nothing
            GV.RowsDefaultCellStyle.SelectionBackColor = QDefaultRowBackColor
            GV.AlternatingRowsDefaultCellStyle.SelectionBackColor = QAltRowBackColor
            GV.RowTemplate.DefaultCellStyle.SelectionForeColor = QForeColor
        End If

        GV.BackgroundColor = QListBackColor
        GV.ColumnHeadersDefaultCellStyle.BackColor = QColHeaderBackColor
        GV.RowsDefaultCellStyle.BackColor = QDefaultRowBackColor
        GV.RowsDefaultCellStyle.ForeColor = QForeColor
        GV.AlternatingRowsDefaultCellStyle.BackColor = QAltRowBackColor
        GV.AlternatingRowsDefaultCellStyle.ForeColor = QForeColor

    End Sub

    ''' <summary> Return True if this control is for entering a single value  </summary>
    Function isSingleEntryControl(ByVal aC As Control) As Boolean
        If IsNothing(aC) Then Return False
        If TypeOf (aC) Is TextBox Then Return True
        If TypeOf (aC) Is CheckBox Then Return True
        If TypeOf (aC) Is ComboBox Then Return True
        If TypeOf (aC) Is DateTimePicker Then Return True
        If TypeOf (aC) Is MaskedTextBox Then Return True
        If TypeOf (aC) Is RadioButton Then Return True
        If TypeOf (aC) Is qDD Then Return True
        If TypeOf (aC) Is qRC Then Return True
        Return False
    End Function

    ''' <summary> Return True is this is a q control and is used for entering a single value </summary>
    Function isSingleQuartetEntryControl(ByVal aC As Control) As Boolean
        If aC Is Nothing Then Return False
        If TypeOf aC Is qTextBox Then Return True
        If TypeOf (aC) Is qCheckBox Then Return True
        If TypeOf (aC) Is qRC Then Return True
        If TypeOf (aC) Is qComboBox Then Return True
        If TypeOf (aC) Is qDD Then Return True
        If TypeOf (aC) Is qDateTimePicker Then Return True
        If TypeOf (aC) Is qMaskedTextBox Then Return True
        Return False
    End Function

    '''<summary> ShowTable opens a window and shows the rows in a table.  Useful debugging tool to show the state of rows in a table. </summary>
    Sub ShowTable(ByVal aTable As DataTable)
        Dim F As New fShowTable
        F.iTable = aTable
        F.ShowDialog()
        F.Close()
    End Sub

    '''<summary> Show each Table in a dataset.  Usefule debugging tool for showing the state of rows in each table in a dataset </summary>
    Sub ShowTable(ByVal aDS As DataSet)
        Dim F As New fShowTable
        Dim D As New Dictionary(Of String, DataTable)
        Dim Values As String = ""
        F.iDS = aDS
        F.ShowDialog()
        F.Close()
    End Sub

    ''' <summary> Display status in bottom of MDI window ''' </summary> 
    Sub ShowMDIStatus(ByVal aMessage As String, ByVal aForm As Form)
        gMDIStatusLabel.Text = aMessage
    End Sub

    ''' <summary> Round a decimal number to "n" places </summary>
    Public Function QSIRound(ByVal aDecNum As Decimal,
                             ByVal aNDecPlaces As Integer) As Decimal
        Dim decno As Decimal
        Dim decconstant As Decimal = CType(0.00000001, Decimal)

        '...add a very small constant to avoid "even number" rounding
        decno = Decimal.Round(aDecNum + decconstant, aNDecPlaces)

        Return decno
    End Function

    ''' <summary>Upload a text file to ftp server</summary>
    Public Sub ftpUploadText(ByVal aHost As String, ByVal aUser As String, ByVal aPassword As String,
                             ByVal aRemotePath As String, ByVal aLocalPath As String)

        Dim Host As String = aHost
        Dim URI As String = ""
        Dim FileContents As Byte()
        Dim ftp As System.Net.FtpWebRequest
        Dim SourceStream As New StreamReader(aLocalPath)
        Dim RequestStream As Stream

        Try
            If Host.ToLower.IndexOf("ftp://") = -1 Then
                Host = "ftp://" & Host
            End If
            If Host.LastIndexOf("/") <> Host.Length - 1 Then
                Host &= "/"
            End If

            URI = Host & aRemotePath
            ftp = CType(System.Net.FtpWebRequest.Create(URI), System.Net.FtpWebRequest)
            ftp.Credentials = New System.Net.NetworkCredential(aUser, aPassword)
            ftp.KeepAlive = False
            ftp.UseBinary = False
            ftp.Method = System.Net.WebRequestMethods.Ftp.UploadFile
            FileContents = Encoding.UTF8.GetBytes(SourceStream.ReadToEnd())
            SourceStream.Close()
            ftp.ContentLength = FileContents.Length
            RequestStream = ftp.GetRequestStream()
            RequestStream.Write(FileContents, 0, FileContents.Length)
            RequestStream.Close()

        Catch ex As Exception
            'BHS 6/28/12
            Throw New ApplicationException("Error uploading FTP text to " & Host, ex)

        End Try
    End Sub

    ''' <summary>Download a text file to ftp server</summary>
    Public Sub ftpDownloadText(ByVal aHost As String, ByVal aUser As String, ByVal aPassword As String,
                               ByVal aRemotePath As String, ByVal aLocalPath As String)
        Dim Host As String = aHost
        Dim URI As String = ""
        Dim ftp As System.Net.FtpWebRequest
        Dim ftpResponse As System.Net.FtpWebResponse
        Dim ResponseStream As Stream
        Dim SR As StreamReader
        Dim SW As New StreamWriter(aLocalPath)

        Try
            If Host.ToLower.IndexOf("ftp://") = -1 Then
                Host = "ftp://" & Host
            End If
            If Host.LastIndexOf("/") <> Host.Length - 1 Then
                Host &= "/"
            End If

            URI = Host & aRemotePath
            ftp = CType(System.Net.FtpWebRequest.Create(URI), System.Net.FtpWebRequest)
            ftp.Credentials = New System.Net.NetworkCredential(aUser, aPassword)
            ftp.KeepAlive = False
            ftp.UseBinary = False
            ftp.Method = System.Net.WebRequestMethods.Ftp.DownloadFile
            ftpResponse = CType(ftp.GetResponse(), System.Net.FtpWebResponse)
            ResponseStream = ftpResponse.GetResponseStream()
            SR = New StreamReader(ResponseStream, Encoding.UTF8)
            'SW = New StreamWriter(aLocalPath)
            SW.Write(SR.ReadToEnd())

        Catch ex As Exception
            'BHS 6/28/12
            Throw New ApplicationException("Error downloading FTP text to " & Host, ex)
        Finally
            SW.Close()
        End Try
    End Sub

    ''' <summary>List files from a folder in an ftp server''' </summary>
    Public Function ftpGetDirectoryDetails(ByVal aHost As String,
                                               ByVal aUser As String,
                                               ByVal aPassword As String,
                                               ByVal aRemotePath As String) As String

        Dim ResultStr As String = ""
        Dim Host As String = aHost
        Dim URI As String = ""
        Dim ftp As System.Net.FtpWebRequest
        Dim ftpResponse As System.Net.FtpWebResponse
        Dim SR As StreamReader

        If Host.ToLower.IndexOf("ftp://") = -1 Then
            Host = "ftp://" & Host
        End If
        If Host.LastIndexOf("/") <> Host.Length - 1 Then
            Host &= "/"
        End If

        URI = Host & aRemotePath
        ftp = CType(System.Net.FtpWebRequest.Create(URI), System.Net.FtpWebRequest)
        ftp.Credentials = New System.Net.NetworkCredential(aUser, aPassword)
        ftp.KeepAlive = False
        ftp.UseBinary = False
        ftp.Method = System.Net.WebRequestMethods.Ftp.ListDirectoryDetails
        ftpResponse = CType(ftp.GetResponse(), System.Net.FtpWebResponse)
        SR = New StreamReader(ftpResponse.GetResponseStream(), Encoding.UTF8)
        ResultStr = SR.ReadToEnd()

        Return ResultStr
    End Function

    ''' <summary>Get Time Stamp of file in an ftp server''' </summary>
    Public Function ftpGetTimeStamp(ByVal aHost As String,
                                    ByVal aUser As String,
                                    ByVal aPassword As String,
                                    ByVal aRemotePath As String) As DateTime
        Dim Host As String = aHost
        Dim URI As String = ""
        Dim ftp As System.Net.FtpWebRequest
        Dim ftpResponse As System.Net.FtpWebResponse

        If Host.ToLower.IndexOf("ftp://") = -1 Then
            Host = "ftp://" & Host
        End If
        If Host.LastIndexOf("/") <> Host.Length - 1 Then
            Host &= "/"
        End If

        URI = Host & aRemotePath
        ftp = CType(System.Net.FtpWebRequest.Create(URI), System.Net.FtpWebRequest)
        ftp.Credentials = New System.Net.NetworkCredential(aUser, aPassword)
        ftp.Method = System.Net.WebRequestMethods.Ftp.GetDateTimestamp
        ftpResponse = CType(ftp.GetResponse(), System.Net.FtpWebResponse)
        Return ftpResponse.LastModified
    End Function
    ''' <summary>Delete a file from ftp server</summary>
    Public Function ftpDeleteFile(ByVal aHost As String, ByVal aUser As String, ByVal aPassword As String,
                                  ByVal aRemotePath As String) As Boolean
        Dim Host As String = aHost
        Dim URI As String = ""
        Dim ftp As System.Net.FtpWebRequest
        Dim ftpResponse As System.Net.FtpWebResponse

        If Host.ToLower.IndexOf("ftp://") = -1 Then
            Host = "ftp://" & Host
        End If
        If Host.LastIndexOf("/") <> Host.Length - 1 Then
            Host &= "/"
        End If

        URI = Host & aRemotePath
        ftp = CType(System.Net.FtpWebRequest.Create(URI), System.Net.FtpWebRequest)
        ftp.Credentials = New System.Net.NetworkCredential(aUser, aPassword)
        ftp.Method = System.Net.WebRequestMethods.Ftp.DeleteFile
        ftpResponse = CType(ftp.GetResponse(), System.Net.FtpWebResponse)

        Return ftpResponse.StatusCode = Net.FtpStatusCode.FileActionOK

    End Function

#End Region

#Region "------------------------ Enumerators -----------------------"

    ''' <summary> Work Flow Activity Types</summary>
    Public Enum WFActType
        Request
        Deadline
        Milestone
        Notification
    End Enum

    ''' <summary>
    ''' Specifies the column formats used with custom DataGridViewColumns.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum ColumnFormatEnum
        None
        PhoneNumber
        SocialSecurityNumber
        Status
    End Enum

    ''' <summary>
    ''' Specifies the data types used with custom controls.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum DataTypeEnum
        Str
        Dat
        Num
        DontSave
    End Enum

    ''' <summary>
    ''' Specifies the form types used with the custom forms.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum FormTypeEnum
        List
        Edit
    End Enum

    ''' <summary>
    ''' Specifies the data providers used with data access.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum DataProviderEnum
        SQL = 1
        ODBC = 2
        IFX = 3
    End Enum

    ''' <summary>
    ''' Specifies the mask types used with custom controls.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum MaskTypeEnum
        None
        PhoneNumber
        SocialSecurityNumber
        ZipCode
        Custom
    End Enum

    ''' <summary> Specifies the prompt response types. </summary>
    Public Enum PromptResponseTypeEnum
        Character
        Dat
        Int
        Dec
        YesNo
    End Enum

    ''' <summary>
    ''' Specifies the text casing used with custom controls.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum TextCaseEnum
        Normal
        Cap1stLetter
        Upper
        Lower
    End Enum


#End Region

#Region "------------------------ Keith's Control Management Functions -----------------------"

    Public Function GetMask(ByVal MaskType As MaskTypeEnum) As String

        Dim mask As String = String.Empty

        Select Case MaskType
            Case MaskTypeEnum.None
            Case MaskTypeEnum.PhoneNumber
                mask = "(999) 000-0000"
            Case MaskTypeEnum.SocialSecurityNumber
                mask = "000-00-0000"
            Case MaskTypeEnum.ZipCode
                mask = "00000-9999"
        End Select

        Return mask

    End Function

    Public Function GetMaskType(ByVal Mask As String) As MaskTypeEnum

        Dim maskType As MaskTypeEnum = MaskTypeEnum.None

        Select Case Mask
            Case "(999) 000-0000"
                maskType = MaskTypeEnum.PhoneNumber
            Case "000-00-0000"
                maskType = MaskTypeEnum.SocialSecurityNumber
            Case "00000-9999"
                maskType = MaskTypeEnum.ZipCode
            Case String.Empty
                maskType = MaskTypeEnum.None
            Case Else
                maskType = MaskTypeEnum.Custom
        End Select

        Return maskType

    End Function

    Public Function FormatPhoneNumber(ByVal Phone As String) As String

        ' Strip Non Numeric Characters
        Phone = NumericOnly(Phone)
        ' Return Formatted Phone
        Select Case Phone.Length
            Case 7
                Return "     " & Phone.Substring(0, 3) & "-" & Phone.Substring(3, 4)
            Case 10
                Return "(" & Phone.Substring(0, 3) & ") " & Phone.Substring(3, 3) & "-" & Phone.Substring(6, 4)
            Case Is > 10
                Return "(" & Phone.Substring(0, 3) & ") " & Phone.Substring(3, 3) & "-" & Phone.Substring(6, 4) & " x" & Phone.Substring(10)
            Case Else
                Return Phone
        End Select

    End Function

    Public Function FormatSocialSecurityNumber(ByVal SocialSecurityNumber As String) As String

        ' Strip Non Numeric Characters
        SocialSecurityNumber = NumericOnly(SocialSecurityNumber)
        ' Return Formatted Social Security Number
        Return SocialSecurityNumber.Substring(0, 3) & "-" & SocialSecurityNumber.Substring(3, 2) & "-" & SocialSecurityNumber.Substring(5, 4)

    End Function

    Public Function FormatStatus(ByVal Status As Integer) As String

        ' Return Formatted Phone
        Select Case Status
            Case 0
                Return "Inactive"
            Case 1
                Return "Active"
            Case Else
                Return String.Empty
        End Select

    End Function

    Public Function NumericOnly(ByVal Value As String) As String
        ' Declare and Instantiate ReturnVal Variable
        Dim ReturnVal As String = ""
        ' Loop Through String Removing All Non Numeric Characters
        Dim X As Integer = 0
        Do While (X < Value.Length)
            If (Char.IsNumber(Value, X) = True) Then
                ' Append Numeric Character to ReturnVal
                ReturnVal = (ReturnVal + Value.Substring(X, 1))
            End If
            X = (X + 1)
        Loop
        ' Return ReturnVal
        Return ReturnVal.Trim
    End Function



    Public Function Cap1stLetter(ByVal Value As String) As String

        ' Declare and Instantiate ReturnVal Variable
        Dim ReturnVal As String = ""
        ' Declare and Instantiate CapLetter Flag
        Dim CapLetter As Boolean = True
        ' Declare and Instantiate Buffer Variable
        Dim Buffer As String = Value
        ' Loop Through Buffer and Capitalize Each Character After a Space
        Do While (Buffer <> "")
            If (Buffer.Substring(0, 1).IndexOf(" ") > -1) Then
                ' Flag Next Character to be Capitalized
                CapLetter = True
                ' Do Nothing to Character
                ReturnVal = (ReturnVal + Buffer.Substring(0, 1))
            Else
                If (CapLetter = True) Then
                    ' Flag Next Character to NOT be Capitalized
                    CapLetter = False
                    ' Capitalized Character
                    ReturnVal = (ReturnVal + Buffer.Substring(0, 1).ToUpper)
                Else
                    ' Do Nothing to Character
                    ReturnVal = (ReturnVal + Buffer.Substring(0, 1))
                End If
            End If
            ' Strip First Character From Buffer
            Buffer = Buffer.Substring(1, (Buffer.Length - 1))
        Loop
        ' Return ReturnVal
        Return ReturnVal

    End Function


#End Region

#Region "------------------------ Keith's GetMySettingsValue -----------------------"

    ''' Generic function that returns object variable from My.Settings
    Public Function GetMySettingsValue(ByVal PropertyName As String) As Object
        ' Declare and Instantiate Object
        Dim o As Object = Nothing
        Try
            ' Get Object From My.Settings
            o = My.Settings.Item(PropertyName)
        Catch ex As System.Configuration.SettingsPropertyNotFoundException
            ' Do Nothing
        End Try
        ' Return Object
        Return o
    End Function


    ''' Specific procedure that sets input variable to type converted value from My.Settings
    Public Sub GetMySettingsValue(ByVal PropertyName As String, ByRef Value As Integer, ByVal EnumType As Type)
        ' Declare and Instantiate Object From My.Settings
        Dim o As Object = GetMySettingsValue(PropertyName)
        ' Check for Found PropertyName in My.Settings
        If o IsNot Nothing Then
            Try
                ' Convert String to Enum
                Value = CType(System.Enum.Parse(EnumType, o.ToString), Integer)
            Catch ex As Exception
                ' Do Nothing
            End Try
        End If
    End Sub


    ''' Specific procedure that sets input variable to type converted value from My.Settings
    Public Sub GetMySettingsValue(ByVal PropertyName As String, ByRef Value As Boolean)
        ' Declare and Instantiate Object From My.Settings
        Dim o As Object = GetMySettingsValue(PropertyName)
        ' Check for Found PropertyName in My.Settings
        If o IsNot Nothing Then
            ' Set Value to Object
            Value = CType(o, Boolean)
        End If
    End Sub

    ''' Specific procedure that sets input variable to type converted value from My.Settings
    Public Sub GetMySettingsValue(ByVal PropertyName As String, ByRef Value As Byte)
        ' Declare and Instantiate Object From My.Settings
        Dim o As Object = GetMySettingsValue(PropertyName)
        ' Check for Found PropertyName in My.Settings
        If o IsNot Nothing Then
            ' Set Value to Object
            Value = CType(o, Byte)
        End If
    End Sub

    ''' <summary>
    ''' Specific procedure that sets input variable to type converted value from My.Settings
    ''' </summary>
    ''' <param name="PropertyName"></param>
    ''' <param name="Value"></param>
    ''' <remarks></remarks>
    Public Sub GetMySettingsValue(ByVal PropertyName As String, ByRef Value As Char)
        ' Declare and Instantiate Object From My.Settings
        Dim o As Object = GetMySettingsValue(PropertyName)
        ' Check for Found PropertyName in My.Settings
        If o IsNot Nothing Then
            ' Set Value to Object
            Value = CType(o, Char)
        End If
    End Sub

    ''' <summary>
    ''' Specific procedure that sets input variable to type converted value from My.Settings
    ''' </summary>
    ''' <param name="PropertyName"></param>
    ''' <param name="Value"></param>
    ''' <remarks></remarks>
    Public Sub GetMySettingsValue(ByVal PropertyName As String, ByRef Value As Date)
        ' Declare and Instantiate Object From My.Settings
        Dim o As Object = GetMySettingsValue(PropertyName)
        ' Check for Found PropertyName in My.Settings
        If o IsNot Nothing Then
            ' Set Value to Object
            Value = CType(o, Date)
        End If
    End Sub

    ''' <summary>
    ''' Specific procedure that sets input variable to type converted value from My.Settings
    ''' </summary>
    ''' <param name="PropertyName"></param>
    ''' <param name="Value"></param>
    ''' <remarks></remarks>
    Public Sub GetMySettingsValue(ByVal PropertyName As String, ByRef Value As Decimal)
        ' Declare and Instantiate Object From My.Settings
        Dim o As Object = GetMySettingsValue(PropertyName)
        ' Check for Found PropertyName in My.Settings
        If o IsNot Nothing Then
            ' Set Value to Object
            Value = CType(o, Decimal)
        End If
    End Sub

    ''' <summary>
    ''' Specific procedure that sets input variable to type converted value from My.Settings
    ''' </summary>
    ''' <param name="PropertyName"></param>
    ''' <param name="Value"></param>
    ''' <remarks></remarks>
    Public Sub GetMySettingsValue(ByVal PropertyName As String, ByRef Value As Double)
        ' Declare and Instantiate Object From My.Settings
        Dim o As Object = GetMySettingsValue(PropertyName)
        ' Check for Found PropertyName in My.Settings
        If o IsNot Nothing Then
            ' Set Value to Object
            Value = CType(o, Double)
        End If
    End Sub

    ''' <summary>
    ''' Specific procedure that sets input variable to type converted value from My.Settings
    ''' </summary>
    ''' <param name="PropertyName"></param>
    ''' <param name="Value"></param>
    ''' <remarks></remarks>
    Public Sub GetMySettingsValue(ByVal PropertyName As String, ByRef Value As Integer)
        ' Declare and Instantiate Object From My.Settings
        Dim o As Object = GetMySettingsValue(PropertyName)
        ' Check for Found PropertyName in My.Settings
        If o IsNot Nothing Then
            ' Set Value to Object
            Value = CType(o, Integer)
        End If
    End Sub

    ''' <summary>
    ''' Specific procedure that sets input variable to type converted value from My.Settings
    ''' </summary>
    ''' <param name="PropertyName"></param>
    ''' <param name="Value"></param>
    ''' <remarks></remarks>
    Public Sub GetMySettingsValue(ByVal PropertyName As String, ByRef Value As Long)
        ' Declare and Instantiate Object From My.Settings
        Dim o As Object = GetMySettingsValue(PropertyName)
        ' Check for Found PropertyName in My.Settings
        If o IsNot Nothing Then
            ' Set Value to Object
            Value = CType(o, Long)
        End If
    End Sub

    ''' <summary>
    ''' Specific procedure that sets input variable to type converted value from My.Settings
    ''' </summary>
    ''' <param name="PropertyName"></param>
    ''' <param name="Value"></param>
    ''' <remarks></remarks>
    Public Sub GetMySettingsValue(ByVal PropertyName As String, ByRef Value As SByte)
        ' Declare and Instantiate Object From My.Settings
        Dim o As Object = GetMySettingsValue(PropertyName)
        ' Check for Found PropertyName in My.Settings
        If o IsNot Nothing Then
            ' Set Value to Object
            Value = CType(o, SByte)
        End If
    End Sub

    ''' <summary>
    ''' Specific procedure that sets input variable to type converted value from My.Settings
    ''' </summary>
    ''' <param name="PropertyName"></param>
    ''' <param name="Value"></param>
    ''' <remarks></remarks>
    Public Sub GetMySettingsValue(ByVal PropertyName As String, ByRef Value As Short)
        ' Declare and Instantiate Object From My.Settings
        Dim o As Object = GetMySettingsValue(PropertyName)
        ' Check for Found PropertyName in My.Settings
        If o IsNot Nothing Then
            ' Set Value to Object
            Value = CType(o, Short)
        End If
    End Sub

    ''' <summary>
    ''' Specific procedure that sets input variable to type converted value from My.Settings
    ''' </summary>
    ''' <param name="PropertyName"></param>
    ''' <param name="Value"></param>
    ''' <remarks></remarks>
    Public Sub GetMySettingsValue(ByVal PropertyName As String, ByRef Value As Single)
        ' Declare and Instantiate Object From My.Settings
        Dim o As Object = GetMySettingsValue(PropertyName)
        ' Check for Found PropertyName in My.Settings
        If o IsNot Nothing Then
            ' Set Value to Object
            Value = CType(o, Single)
        End If
    End Sub

    ''' <summary>
    ''' Specific procedure that sets input variable to type converted value from My.Settings
    ''' </summary>
    ''' <param name="PropertyName"></param>
    ''' <param name="Value"></param>
    ''' <remarks></remarks>
    Public Sub GetMySettingsValue(ByVal PropertyName As String, ByRef Value As System.Collections.Specialized.StringCollection)
        ' Declare and Instantiate Object From My.Settings
        Dim o As Object = GetMySettingsValue(PropertyName)
        ' Check for Found PropertyName in My.Settings
        If o IsNot Nothing Then
            ' Set Value to Object
            Value = CType(o, System.Collections.Specialized.StringCollection)
        End If
    End Sub

    ''' <summary>
    ''' Specific procedure that sets input variable to type converted value from My.Settings
    ''' </summary>
    ''' <param name="PropertyName"></param>
    ''' <param name="Value"></param>
    ''' <remarks></remarks>
    Public Sub GetMySettingsValue(ByVal PropertyName As String, ByRef Value As System.Drawing.Color)
        ' Declare and Instantiate Object From My.Settings
        Dim o As Object = GetMySettingsValue(PropertyName)
        ' Check for Found PropertyName in My.Settings
        If o IsNot Nothing Then
            ' Set Value to Object
            Value = CType(o, System.Drawing.Color)
        End If
    End Sub

    ''' <summary>
    ''' Specific procedure that sets input variable to type converted value from My.Settings
    ''' </summary>
    ''' <param name="PropertyName"></param>
    ''' <param name="Value"></param>
    ''' <remarks></remarks>
    Public Sub GetMySettingsValue(ByVal PropertyName As String, ByRef Value As System.Drawing.Font)
        ' Declare and Instantiate Object From My.Settings
        Dim o As Object = GetMySettingsValue(PropertyName)
        ' Check for Found PropertyName in My.Settings
        If o IsNot Nothing Then
            ' Set Value to Object
            Value = CType(o, System.Drawing.Font)
        End If
    End Sub

    ''' <summary>
    ''' Specific procedure that sets input variable to type converted value from My.Settings
    ''' </summary>
    ''' <param name="PropertyName"></param>
    ''' <param name="Value"></param>
    ''' <remarks></remarks>
    Public Sub GetMySettingsValue(ByVal PropertyName As String, ByRef Value As System.Drawing.Size)
        ' Declare and Instantiate Object From My.Settings
        Dim o As Object = GetMySettingsValue(PropertyName)
        ' Check for Found PropertyName in My.Settings
        If o IsNot Nothing Then
            ' Set Value to Object
            Value = CType(o, System.Drawing.Size)
        End If
    End Sub

    ''' <summary>
    ''' Specific procedure that sets input variable to type converted value from My.Settings
    ''' </summary>
    ''' <param name="PropertyName"></param>
    ''' <param name="Value"></param>
    ''' <remarks></remarks>
    Public Sub GetMySettingsValue(ByVal PropertyName As String, ByRef Value As System.Drawing.Point)
        ' Declare and Instantiate Object From My.Settings
        Dim o As Object = GetMySettingsValue(PropertyName)
        ' Check for Found PropertyName in My.Settings
        If o IsNot Nothing Then
            ' Set Value to Object
            Value = CType(o, System.Drawing.Point)
        End If
    End Sub

    ''' <summary>
    ''' Specific procedure that sets input variable to type converted value from My.Settings
    ''' </summary>
    ''' <param name="PropertyName"></param>
    ''' <param name="Value"></param>
    ''' <remarks></remarks>
    Public Sub GetMySettingsValue(ByVal PropertyName As String, ByRef Value As System.Guid)
        ' Declare and Instantiate Object From My.Settings
        Dim o As Object = GetMySettingsValue(PropertyName)
        ' Check for Found PropertyName in My.Settings
        If o IsNot Nothing Then
            ' Set Value to Object
            Value = CType(o, System.Guid)
        End If
    End Sub

    ''' <summary>
    ''' Specific procedure that sets input variable to type converted value from My.Settings
    ''' </summary>
    ''' <param name="PropertyName"></param>
    ''' <param name="Value"></param>
    ''' <remarks></remarks>
    Public Sub GetMySettingsValue(ByVal PropertyName As String, ByRef Value As System.TimeSpan)
        ' Declare and Instantiate Object From My.Settings
        Dim o As Object = GetMySettingsValue(PropertyName)
        ' Check for Found PropertyName in My.Settings
        If o IsNot Nothing Then
            ' Set Value to Object
            Value = CType(o, System.TimeSpan)
        End If
    End Sub

    ''' <summary>
    ''' Specific procedure that sets input variable to type converted value from My.Settings
    ''' </summary>
    ''' <param name="PropertyName"></param>
    ''' <param name="Value"></param>
    ''' <remarks></remarks>
    Public Sub GetMySettingsValue(ByVal PropertyName As String, ByRef Value As String)
        ' Declare and Instantiate Object From My.Settings
        Dim o As Object = GetMySettingsValue(PropertyName)
        ' Check for Found PropertyName in My.Settings
        If o IsNot Nothing Then
            ' Set Value to Object
            Value = CType(o, String)
        End If
    End Sub

    ''' <summary>
    ''' Specific procedure that sets input variable to type converted value from My.Settings
    ''' </summary>
    ''' <param name="PropertyName"></param>
    ''' <param name="Value"></param>
    ''' <remarks></remarks>
    Public Sub GetMySettingsValue(ByVal PropertyName As String, ByRef Value As UInteger)
        ' Declare and Instantiate Object From My.Settings
        Dim o As Object = GetMySettingsValue(PropertyName)
        ' Check for Found PropertyName in My.Settings
        If o IsNot Nothing Then
            ' Set Value to Object
            Value = CType(o, UInteger)
        End If
    End Sub

    ''' <summary>
    ''' Specific procedure that sets input variable to type converted value from My.Settings
    ''' </summary>
    ''' <param name="PropertyName"></param>
    ''' <param name="Value"></param>
    ''' <remarks></remarks>
    Public Sub GetMySettingsValue(ByVal PropertyName As String, ByRef Value As ULong)
        ' Declare and Instantiate Object From My.Settings
        Dim o As Object = GetMySettingsValue(PropertyName)
        ' Check for Found PropertyName in My.Settings
        If o IsNot Nothing Then
            ' Set Value to Object
            Value = CType(o, ULong)
        End If
    End Sub

    ''' <summary>
    ''' Specific procedure that sets input variable to type converted value from My.Settings
    ''' </summary>
    ''' <param name="PropertyName"></param>
    ''' <param name="Value"></param>
    ''' <remarks></remarks>
    Public Sub GetMySettingsValue(ByVal PropertyName As String, ByRef Value As UShort)
        ' Declare and Instantiate Object From My.Settings
        Dim o As Object = GetMySettingsValue(PropertyName)
        ' Check for Found PropertyName in My.Settings
        If o IsNot Nothing Then
            ' Set Value to Object
            Value = CType(o, UShort)
        End If
    End Sub

#End Region

#Region "------------------------ Generic Export to Excel -----------------------"
    'Based on GemBox
    '   BHS 12/14/07 allow specifying a second Gridview to add to the bottom of the first
    '''' <summary> Move contents of a GV to  Excel.  Optional Excel File to add this worksheet to an existing spreadsheet.  
    '''' Optional SaveAndFinish to indicate whether to leave spreadsheet open for additional GVToExcel, 
    '''' Optional aGV2 to show footer in same spreadsheet </summary>
    'Function GVToExcel(ByVal aGV As DataGridView,
    '                   ByVal aWorkBookName As String,
    '                   ByVal aSourceFunctionName As String,
    '                   Optional ByVal aSQLDescr As String = "",
    '                   Optional ByRef aExcelFile As ExcelFile = Nothing,
    '                   Optional ByVal aSaveAndFinish As Boolean = True,
    '                   Optional ByVal aShowHeader As Boolean = True,
    '                   Optional ByVal aGV2 As DataGridView = Nothing,
    '                   Optional ByVal aRptDescr As String = "",
    '                   Optional ByVal aTitle As String = "",
    '                   Optional ByVal aAnnotation As String = "",
    '                   Optional ByVal aOldExcelXLS As Boolean = False,
    '                   Optional ByVal aFreezeRowsCount As Integer = 0,
    '                   Optional ByVal aFreezeColsCount As Integer = 0) As Boolean

    '    ' GBV - 11/16/2012 add xls extention if aOldExcelXLS is true
    '    Dim Path As String
    '    If aOldExcelXLS Then
    '        Path = ExportPath(aSourceFunctionName, ".xls")
    '    Else
    '        Path = ExportPath(aSourceFunctionName)
    '    End If


    '    Dim OneMoment As New OneMoment
    '    OneMoment.lblMessage.Text = "Building Spreadsheet"
    '    OneMoment.MdiParent = gMDIForm
    '    OneMoment.Show()
    '    OneMoment.Refresh()

    '    Try
    '        'Excel variables
    '        GemBox.Spreadsheet.SpreadsheetInfo.SetLicense("EH1J-LVMC-TL23-0GRX")
    '        'GemBox.Spreadsheet.SpreadsheetInfo.SetLicense("EY49-C7IJ-RRGP-0881")  32 bit
    '        Dim EF As ExcelFile
    '        Dim WCell As GemBox.Spreadsheet.ExcelCell
    '        Dim WCol As GemBox.Spreadsheet.ExcelColumn
    '        Dim WRow As GemBox.Spreadsheet.ExcelRow

    '        'GV variables
    '        Dim C As DataGridViewCell

    '        Dim Col As DataGridViewColumn
    '        Dim CellStyle As New DataGridViewCellStyle

    '        'Count variables
    '        Dim ColCount As Integer
    '        Dim RowCount As Integer
    '        Dim ColumnsSkipped As Integer = 0
    '        Dim i As Integer

    '        'Pick up EF from parameters, if provided
    '        If aExcelFile Is Nothing Then
    '            EF = New ExcelFile
    '        Else
    '            EF = aExcelFile
    '        End If

    '        '--- BEGIN 10/31/12 HOTFIX
    '        'BHS 10/31/12 Remove the 65000 row check, now that we're in 64 bits
    '        'BHS 1/7/11  Return False if aGV exceeds 65,000 lines
    '        'If aGV.Rows.Count > 65000 Then
    '        '    aExcelFile = EF 'Pass back a valid EF in case calling code needs it for additional worksheet
    '        '    MsgBox("Sorry - your list of " & aGV.Rows.Count.ToString("#,##0") & _
    '        '           " lines exceeds what Excel can handle.", MsgBoxStyle.Exclamation, _
    '        '           "Problem exporting to Excel")
    '        '    Return False
    '        'End If
    '        '--- END 10/31/12 HOTFIX

    '        ''SRM 11/19/2012 only use new excel file format if we have more than 65000 lines
    '        'If aGV.Rows.Count < 65000 Then
    '        '    aOldExcelXLS = True
    '        '    Path = ExportPath(aSourceFunctionName, ".xls")
    '        'End If


    '        Dim W As GemBox.Spreadsheet.ExcelWorksheet = EF.Worksheets.Add(aWorkBookName)
    '        EF.DefaultFontName = "MS Sans Serif"
    '        EF.DefaultFontSize = 220

    '        ' GBV 7/30/2014 - Ticket 3167 - Freeze rows and/or columns if requested
    '        Dim Columns As String = "BCDEFGHIJKLMNOPQRSTUVWXYZ"
    '        Dim CellName As String = ""
    '        If aFreezeColsCount > 0 AndAlso aFreezeRowsCount > 0 Then
    '            If aFreezeColsCount > 25 Then aFreezeColsCount = 25
    '            CellName = Columns(aFreezeColsCount - 1) & (aFreezeRowsCount + 1).ToString
    '            W.Panes = New WorksheetPanes(PanesState.Frozen, aFreezeColsCount, aFreezeRowsCount, CellName, PanePosition.BottomRight)
    '        ElseIf aFreezeColsCount > 0 Then
    '            If aFreezeColsCount > 25 Then aFreezeColsCount = 25
    '            CellName = Columns(aFreezeColsCount - 1) & "1"
    '            W.Panes = New WorksheetPanes(PanesState.Frozen, aFreezeColsCount, aFreezeRowsCount, CellName, PanePosition.BottomRight)
    '        ElseIf aFreezeRowsCount > 0 Then
    '            CellName = "A" & (aFreezeRowsCount + 1).ToString
    '            W.Panes = New WorksheetPanes(PanesState.Frozen, aFreezeColsCount, aFreezeRowsCount, CellName, PanePosition.BottomRight)
    '        End If

    '        RowCount = -1

    '        '==== Insert a Spreadsheet Title if there is one ==== (GBV 11/14/2008)
    '        If aTitle.Length > 0 Then
    '            WCell = W.Cells(0, 0)
    '            WCell.Style.Font.Weight = 700  ' Bold
    '            WCell.Style.Font.UnderlineStyle = UnderlineStyle.Single
    '            WCell.Style.Font.Size = 300
    '            WCell.Value = aTitle
    '            RowCount += 2
    '        End If

    '        '==== Fill Spreadsheet Column Headers ====
    '        If aShowHeader = True Then

    '            RowCount += 1

    '            WRow = W.Rows(RowCount)
    '            WRow.Height = aGV.ColumnHeadersHeight * 19

    '            For ColCount = 0 To aGV.Columns.Count - 1
    '                Col = GetColByDisplayIndex(aGV, ColCount) 'Forces columns in current visual (index) order

    '                If Col.Visible = False Or Mid(Col.Name.ToLower, 1, 7) = "workcol" Then
    '                    ColumnsSkipped += 1
    '                Else
    '                    WCell = W.Cells(RowCount, ColCount - ColumnsSkipped)   'Don't leave blank columns for skipped columns

    '                    'HeaderText
    '                    WCell.Value = Col.HeaderText

    '                    'Background Color
    '                    WCell.Style.FillPattern.SetSolid(aGV.ColumnHeadersDefaultCellStyle.BackColor)
    '                    WCell.Style.WrapText = True

    '                    'Column Width
    '                    WCol = W.Columns(ColCount - ColumnsSkipped)
    '                    WCol.Width = Col.Width * 40

    '                    'Header text alignment
    '                    If aGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter Or
    '                    aGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter Or
    '                    aGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.TopCenter Then
    '                        WCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
    '                    End If

    '                    If Col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter Or
    '                                    Col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter Or
    '                                    Col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter Then
    '                        WCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
    '                    End If

    '                    'Cell Borders
    '                    WCell.SetBorders(MultipleBorders.Outside, Color.Black, LineStyle.Thin)
    '                End If

    '            Next

    '        Else
    '            For ColCount = 0 To aGV.Columns.Count - 1
    '                Col = GetColByDisplayIndex(aGV, ColCount) 'Forces columns in current visual (index) order

    '                If Col.Visible = False Or Mid(Col.Name.ToLower, 1, 7) = "workcol" Then
    '                    ColumnsSkipped += 1
    '                Else
    '                    'Column Width
    '                    WCol = W.Columns(ColCount - ColumnsSkipped)
    '                    WCol.Width = Col.Width * 40
    '                End If

    '            Next

    '        End If

    '        '==== Fill Spreadsheet Row Data ====
    '        'If you make changes in the For Each, consider making it below also

    '        For Each R As DataGridViewRow In aGV.Rows
    '            RowCount += 1

    '            'Row Height
    '            WRow = W.Rows(RowCount)
    '            WRow.Height = R.Height * 14

    '            ColumnsSkipped = 0
    '            'For Each Column in DisplayIndex order
    '            For ColCount = 0 To aGV.Columns.Count - 1
    '                Col = GetColByDisplayIndex(aGV, ColCount) 'Forces columns in current visual (index) order
    '                'BHS 4/11/08 - set isNumberColumn = True if qBVTextBoxColumn._DataType = Num
    '                Dim isNumberColumn As Boolean = False
    '                'Dim qCol As QSILib.qGVTextBoxColumn = TryCast(Col, qGVTextBoxColumn)
    '                'If qCol IsNot Nothing Then
    '                '    If qCol._DataType = DataTypeEnum.Num Then
    '                '        isNumberColumn = True
    '                '    End If
    '                'End If

    '                If Col.Visible = False Or Mid(Col.Name.ToLower, 1, 7) = "workcol" Then
    '                    ColumnsSkipped += 1
    '                Else
    '                    WCell = W.Cells(RowCount, ColCount - ColumnsSkipped)
    '                    C = R.Cells(Col.Name)

    '                    'Cell Text
    '                    WCell.Value = C.Value ' GBV 12/17/2013 changed it back to Value to handle dates and numbers
    '                    'WCell.Value = C.FormattedValue  '...SDC 11/30/2007
    '                    'For checkboxes, show Y or blank
    '                    If TypeOf (C.OwningColumn) Is DataGridViewCheckBoxColumn Then
    '                        If WCell.Value.ToString = "1" OrElse WCell.Value.ToString = "True" Then
    '                            WCell.Value = "Y"
    '                        Else
    '                            WCell.Value = ""
    '                        End If
    '                    End If

    '                    'Cell Background Color
    '                    WCell.Style.FillPattern.SetSolid(C.InheritedStyle.BackColor)

    '                    'Font bold (400 = regular)
    '                    If C.InheritedStyle.Font.Bold Then
    '                        WCell.Style.Font.Weight = 700
    '                    Else
    '                        WCell.Style.Font.Weight = 400
    '                    End If

    '                    'Cell Format
    '                    If aTitle.Length > 0 AndAlso aShowHeader = True Then  ' GBV 11/14/2008
    '                        i = RowCount - 2
    '                    ElseIf aTitle.Length > 0 OrElse aShowHeader = True Then
    '                        i = RowCount - 1
    '                    Else
    '                        i = RowCount
    '                    End If
    '                    If i >= aGV.Rows.Count Then i = aGV.Rows.Count - 1 ' GBV 11/14/2008
    '                    'If aShowHeader = False Then i = RowCount GBV 11/14/2008
    '                    C.GetInheritedStyle(CellStyle, i, False)
    '                    WCell.Style.NumberFormat = CellStyle.Format

    '                    '   Translate N forms that Excel doesn't recognize
    '                    If isNumberColumn Or (((Mid(CellStyle.Format, 1, 1) = "N") Or (Mid(CellStyle.Format, 1, 1) = "C") Or Mid(CellStyle.Format, 1, 3) = "#,#") And CellStyle.Format.Length > 1) Then  'BHS 3/12/08 Added "C"
    '                        Dim ws As String = Mid(CellStyle.Format, 2)
    '                        If IsNumeric(ws) Then
    '                            If CInt(ws) = 0 Then
    '                                WCell.Style.NumberFormat = "#,##0"
    '                            Else
    '                                WCell.Style.NumberFormat = "#,##0." & Mid("00000000000000000000", 1, CInt(ws))
    '                            End If
    '                        End If
    '                        'BHS 12/14/07
    '                        If IsNumeric(WCell.Value) Then
    '                            Dim D As Decimal = CType(WCell.Value, Decimal)
    '                            WCell.Value = D
    '                        Else
    '                            If IsNumeric(C.Value) Then  'BHS 4/14/08 To handle percentage columns
    '                                Dim D As Decimal = CType(C.Value, Decimal)
    '                                WCell.Value = D
    '                            End If
    '                        End If
    '                    End If

    '                    'Cell text alignment
    '                    If Col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter Or
    '                       Col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter Or
    '                       Col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopCenter Then
    '                        WCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
    '                    End If
    '                    If Col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft Or
    '                       Col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft Or
    '                       Col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft Then
    '                        WCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left
    '                    End If
    '                    If Col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight Or
    '                       Col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight Or
    '                       Col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight Then
    '                        WCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right
    '                    End If

    '                    If C.Style.Alignment = DataGridViewContentAlignment.BottomCenter Or
    '                                    C.Style.Alignment = DataGridViewContentAlignment.MiddleCenter Or
    '                                    C.Style.Alignment = DataGridViewContentAlignment.TopCenter Then
    '                        WCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
    '                    End If
    '                    If C.Style.Alignment = DataGridViewContentAlignment.BottomLeft Or
    '                                    C.Style.Alignment = DataGridViewContentAlignment.MiddleLeft Or
    '                                    C.Style.Alignment = DataGridViewContentAlignment.TopLeft Then
    '                        WCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left
    '                    End If
    '                    If C.Style.Alignment = DataGridViewContentAlignment.BottomRight Or
    '                                    C.Style.Alignment = DataGridViewContentAlignment.MiddleRight Or
    '                                    C.Style.Alignment = DataGridViewContentAlignment.TopRight Then
    '                        WCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right
    '                    End If

    '                    'Cell Borders
    '                    WCell.SetBorders(MultipleBorders.Outside, Color.Black, LineStyle.Thin)
    '                End If
    '            Next
    '        Next

    '        '====== Add second GV if appropriate
    '        If aGV2 IsNot Nothing Then

    '            For Each R As DataGridViewRow In aGV2.Rows
    '                RowCount += 1

    '                'Row Height
    '                WRow = W.Rows(RowCount)
    '                WRow.Height = R.Height * 14

    '                ColumnsSkipped = 0
    '                'For Each Column in DisplayIndex order
    '                For ColCount = 0 To aGV2.Columns.Count - 1
    '                    Col = GetColByDisplayIndex(aGV2, ColCount) 'Forces columns in current visual (index) order

    '                    'BHS 4/11/08 - set isNumberColumn = True if qBVTextBoxColumn._DataType = Num
    '                    Dim isNumberColumn As Boolean = False
    '                    'Dim qCol As QSILib.qGVTextBoxColumn = TryCast(Col, qGVTextBoxColumn)
    '                    'If qCol IsNot Nothing Then
    '                    '    If qCol._DataType = DataTypeEnum.Num Then
    '                    '        isNumberColumn = True
    '                    '    End If
    '                    'End If

    '                    If Col.Visible = False Or Mid(Col.Name.ToLower, 1, 7) = "workcol" Then
    '                        ColumnsSkipped += 1
    '                    Else
    '                        WCell = W.Cells(RowCount, ColCount - ColumnsSkipped)
    '                        C = R.Cells(Col.Name)

    '                        'Cell Text
    '                        WCell.Value = C.Value ' GBV 12/17/2013 changed it back to Value to handle dates and numbers
    '                        'WCell.Value = C.FormattedValue  '...SDC 11/30/2007
    '                        'For checkboxes, show Y or blank
    '                        If TypeOf (C.OwningColumn) Is DataGridViewCheckBoxColumn Then
    '                            If WCell.Value.ToString = "1" Then
    '                                WCell.Value = "Y"
    '                            Else
    '                                WCell.Value = ""
    '                            End If
    '                        End If

    '                        'Cell Background Color
    '                        WCell.Style.FillPattern.SetSolid(C.InheritedStyle.BackColor)

    '                        'Font bold (400 = regular)
    '                        If C.InheritedStyle.Font.Bold Then
    '                            WCell.Style.Font.Weight = 700
    '                        Else
    '                            WCell.Style.Font.Weight = 400
    '                        End If

    '                        'Cell Format
    '                        'i = C.OwningRow.Index
    '                        'i = RowCount - 1
    '                        'If aShowHeader = False Then i = RowCount
    '                        C.GetInheritedStyle(CellStyle, C.OwningRow.Index, False)
    '                        WCell.Style.NumberFormat = CellStyle.Format

    '                        '   Translate N forms that Excel doesn't recognize
    '                        If isNumberColumn Or (((Mid(CellStyle.Format, 1, 1) = "N") Or (Mid(CellStyle.Format, 1, 1) = "C") Or Mid(CellStyle.Format, 1, 3) = "#,#") And CellStyle.Format.Length > 1) Then  'BHS 3/12/08 Added "C"
    '                            Dim ws As String = Mid(CellStyle.Format, 2)
    '                            If IsNumeric(ws) Then
    '                                If CInt(ws) = 0 Then
    '                                    WCell.Style.NumberFormat = "#,##0"
    '                                Else
    '                                    WCell.Style.NumberFormat = "#,##0." & Mid("00000000000000000000", 1, CInt(ws))
    '                                End If
    '                            End If
    '                            'BHS 12/14/07
    '                            If IsNumeric(WCell.Value) Then
    '                                Dim D As Decimal = CType(WCell.Value, Decimal)
    '                                WCell.Value = D
    '                            Else
    '                                If IsNumeric(C.Value) Then  'BHS 4/14/08    To handle percentage columns
    '                                    Dim D As Decimal = CType(C.Value, Decimal)
    '                                    WCell.Value = D
    '                                End If
    '                            End If
    '                        End If

    '                        'Cell text alignment
    '                        If Col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter Or
    '                           Col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter Or
    '                           Col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopCenter Then
    '                            WCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
    '                        End If
    '                        If Col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft Or
    '                           Col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft Or
    '                           Col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft Then
    '                            WCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left
    '                        End If
    '                        If Col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight Or
    '                           Col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight Or
    '                           Col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight Then
    '                            WCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right
    '                        End If

    '                        If C.Style.Alignment = DataGridViewContentAlignment.BottomCenter Or
    '                                        C.Style.Alignment = DataGridViewContentAlignment.MiddleCenter Or
    '                                        C.Style.Alignment = DataGridViewContentAlignment.TopCenter Then
    '                            WCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
    '                        End If
    '                        If C.Style.Alignment = DataGridViewContentAlignment.BottomLeft Or
    '                                        C.Style.Alignment = DataGridViewContentAlignment.MiddleLeft Or
    '                                        C.Style.Alignment = DataGridViewContentAlignment.TopLeft Then
    '                            WCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left
    '                        End If
    '                        If C.Style.Alignment = DataGridViewContentAlignment.BottomRight Or
    '                                        C.Style.Alignment = DataGridViewContentAlignment.MiddleRight Or
    '                                        C.Style.Alignment = DataGridViewContentAlignment.TopRight Then
    '                            WCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right
    '                        End If

    '                        'Cell Borders
    '                        WCell.SetBorders(MultipleBorders.Outside, Color.Black, LineStyle.Thin)
    '                    End If
    '                Next
    '            Next
    '        End If

    '        'Population Description
    '        If aRptDescr.Length > 0 Then
    '            RowCount += 1
    '            Dim Descr As String = "Report Criteria: " & aRptDescr
    '            While Descr.Length > 0
    '                Dim wstr As String = ParseStr(Descr, ",", 80)    'Break up desription into strings of 80 or more.
    '                If Descr.Length > 0 Then wstr &= "," 'Add a comma if we're going to have another line below it
    '                RowCount += 1
    '                WCell = W.Cells(RowCount, 0)
    '                WCell.Value = wstr
    '            End While
    '        End If

    '        If aSQLDescr.Length > 0 Then
    '            RowCount += 2
    '            WCell = W.Cells(RowCount, 0)
    '            If Mid(aSQLDescr, 1, 11).ToLower = "filtered by" Then
    '                WCell.Value = aSQLDescr
    '            Else
    '                WCell.Value = "Population Criteria: " & aSQLDescr
    '            End If

    '        End If

    '        If aAnnotation.Length > 0 Then
    '            RowCount += 2
    '            WCell = W.Cells(RowCount, 0)
    '            WCell.Value = aAnnotation
    '        End If


    '        ''Population Description 
    '        ''  Assumes ~ between Report Critiera (first) and Population Criteria (second part of string)
    '        ''  Assumes | between name/value pairs.  If string comes in without these symbols, then whole
    '        ''  string appears after "Populatino Criteria: "
    '        'Dim Descr As String = ParseStr(aSQLDescr, "~")  'Get first part of the string
    '        'While Descr.Length > 0
    '        '    If aSQLDescr.Length = 0 Then
    '        '        Descr = "Population Criteria: " & Descr
    '        '    Else
    '        '        Descr = "Report Criteria: " & Descr
    '        '    End If
    '        '    RowCount += 1
    '        '    While Descr.Length > 0
    '        '        Dim wstr As String = ParseStr(Descr, "|", 80)   'Break up desription into strings of 80 or more.
    '        '        wstr = Replace(wstr, "|", ",")
    '        '        RowCount += 1
    '        '        WCell = W.Cells(RowCount, 0)
    '        '        WCell.Value = wstr
    '        '    End While
    '        '    Descr = ParseStr(aSQLDescr, "~")    'Get next part of the string
    '        'End While

    '        'Either save and show spreadsheet, or store it in aExcelFile for later use
    '        If aSaveAndFinish Then
    '            'EF.SaveXls(Path)
    '            'System.Diagnostics.Process.Start("excel.exe", Path) -GBV - separate function 11/14/2008
    '            SaveExcel(EF, Path, aOldExcelXLS)
    '        Else
    '            aExcelFile = EF
    '        End If

    '        Return True
    '    Finally
    '        OneMoment.Close()

    '    End Try
    'End Function

    'Sub SaveExcel(ByVal aEF As ExcelFile,
    '              ByVal aPath As String,
    '              Optional aOldExcelXLS As Boolean = False)

    '    Try
    '        ' GBV 7/21/2014
    '        If gIsFromCron Then
    '            aPath = aPath.Replace("H:\", gFileServer & "\home\" & gUserName & "\")
    '            gCronRPTPath = aPath
    '        End If

    '        If aOldExcelXLS = True Then
    '            aEF.SaveXls(aPath)
    '        Else
    '            'BHS 11/14/12 Change SaveXls to SaveXlsx
    '            aEF.SaveXlsx(aPath)
    '        End If

    '        If gIsFromCron Then Return ' GBV 7/21/2014 - if from cron we are done.

    '        'BHS 4/3/09 Allow user to use Browse Directory window to save export before saving
    '        Dim MBR As MsgBoxResult = MsgBox("Save this spreadsheet before viewing?",
    '                                         MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2,
    '                                         "Excel Export")
    '        Dim SavePath As String = ""
    '        If MBR = MsgBoxResult.Yes Then
    '            Dim viewlocal As Boolean = False
    '            If SQLGetString("Select HCamp From UserMain Where UserID = '" & gUserName & "'") <> "OTT" Then viewlocal = True

    '            'BHS 11/19/12 manage possibility of different extensions
    '            Dim Ext As String = "xls"
    '            If aOldExcelXLS = False Then Ext = "xlsx"

    '            'Fill in previous values if any
    '            Dim tDrive As String = ""
    '            Dim tPath As String = ""
    '            If gLastSaveDrive <> "" Then tDrive = gLastSaveDrive.Trim
    '            If gLastSaveLocation <> "" Then tPath = gLastSaveLocation.Trim
    '            viewlocal = gLastSaveLocal

    '            Dim SavePathForm As New fSaveFile(tDrive, tPath, viewlocal, SavePath, Ext)

    '            'SavePathForm.MdiParent = gMDIForm  Can't show a Modal dialog box within an MDI
    '            SavePathForm.ShowDialog()
    '            SavePath = SavePathForm.iSavePath
    '            If SavePath IsNot Nothing AndAlso SavePath.Length > 0 Then
    '                Try
    '                    If aOldExcelXLS = True Then
    '                        'aEF.SaveXls(aPath)
    '                        aEF.SaveXls(SavePath)
    '                    Else
    '                        'BHS 11/14/12 Change SaveXls to SaveXlsx
    '                        'aEF.SaveXlsx(aPath)
    '                        aEF.SaveXlsx(SavePath)
    '                    End If
    '                Catch ex As Exception
    '                    If ex.Message.ToLower.IndexOf("part of the path") > -1 Then
    '                        MsgBox("Invalid Path - try again", MsgBoxStyle.Exclamation, "Can't Save File")
    '                        Return
    '                    Else
    '                        'Throw New ApplicationException("Trouble writing to specified file", ex)
    '                        ' Make error message user friendly and not fatal - GBV 4/11/2014 (ticket 3064)
    '                        MsgBox("Trouble writing to specified file", MsgBoxStyle.Exclamation, "Can't Save File")
    '                        Return
    '                    End If
    '                End Try
    '                'System.Diagnostics.Process.Start("excel.exe", """" & SavePath & """") 'BHS 4/21/09 open Excel on NewPath, if valid
    '                Try
    '                    'BHS 11/20/12 - Need to explicitly use Excel, for successful read of user's local files
    '                    System.Diagnostics.Process.Start("excel.exe", """" & SavePath & """")

    '                Catch ex As Exception
    '                    ' Make error message user friendly and not fatal - GBV 4/11/2014 (ticket 3064)
    '                    'Throw New ApplicationException("Trouble reading saved Excel file", ex)
    '                    MsgBox("Trouble reading saved Excel file", MsgBoxStyle.Exclamation, "Can't Read File")
    '                    Return
    '                End Try

    '                'Set global gLastSaveLocation if the user saved the file - DJW 01/21/2015
    '                If SavePath <> "" Then
    '                    gLastSaveLocation = SavePathForm.txtDocPath.Text.Trim
    '                    gLastSaveDrive = SavePathForm.cbDrive.Text.Trim
    '                    If SavePathForm.chViewLocalNetwork.Checked = True Then
    '                        gLastSaveLocal = True
    '                    Else
    '                        gLastSaveLocal = False
    '                    End If
    '                End If
    '            Else
    '                'System.Diagnostics.Process.Start("excel.exe", """" & aPath & """")
    '                'BHS 11/19/2012 Let OS decide which program to invoke
    '                System.Diagnostics.Process.Start(aPath)
    '            End If
    '        Else
    '            'System.Diagnostics.Process.Start("excel.exe", """" & aPath & """")
    '            'BHS 11/19/2012 Let OS decide which program to invoke
    '            System.Diagnostics.Process.Start(aPath)
    '        End If

    '    Catch ex As Exception
    '        Throw New ApplicationException("Unexpected error opening excel file: ", ex)
    '    End Try

    'End Sub

    ''' <summary> Get reference to column by knowing its index </summary>
    Function GetColByDisplayIndex(ByVal aGV As DataGridView,
                                  ByVal aIndex As Integer) As DataGridViewColumn
        For Each C As DataGridViewColumn In aGV.Columns
            If C.DisplayIndex = aIndex Then Return C
        Next
        Return aGV.Columns(0)
    End Function


    ''' <summary> Create a unique filename in H:\PTSW\Reports to send an export to </summary>
    Function ExportPath(Optional ByVal aFunctionName As String = "PTSW",
                        Optional ByVal aExtension As String = ".xlsx") As String
        'BHS 11/14/12 changed extension to .xlsx
        Dim DateTime As String = Format(Now(), "yyyy-MM-dd-HH-mm-ssff")
        Dim Path As String = "H:\PTSW\Reports\tmp-" & Trim(Mid(gUserName, 4, 14)) & "-" & aFunctionName & "-" & DateTime & aExtension
        Return Path
    End Function
#End Region

#Region "------------------------ Active Reports Functions -----------------------"
    ''' <summary> Used for greenbar reports </summary>
    Function GetBackColor(ByRef aRow As Integer) As Color
        aRow += 1
        If aRow Mod (2) = 0 Then
            Return QAltRowBackColor
        Else
            '01/24/2011 SDC Changed after discussion with Bruce--affects 251, 253, 451 reports
            'Return QDefaultRowBackColor
            Return System.Drawing.Color.Transparent
        End If
    End Function

    ' '''<summary>Add security parameters to aExp, an ActiveReports PDFExport, passed by Reference</summary>
    'Sub SecurePDFExport(ByRef aExp As DataDynamics.ActiveReports.Export.Pdf.PdfExport)
    '    ' ... Secure the document - GBV 6/21/2010
    '    aExp.Security.Encrypt = True
    '    aExp.Security.OwnerPassword = "A5103255307"
    '    aExp.Security.Permissions = DataDynamics.ActiveReports.Export.Pdf.PdfPermissions.AllowPrint
    '    aExp.Security.Use128Bit = True
    'End Sub

    ''''<summary>Add security parameters to aExp, an ActiveReports PDFExport, passed by Reference</summary>
    'Sub SecurePDFExport(ByRef aExp As GrapeCity.ActiveReports.Export.Pdf.Section.PdfExport)
    '    ' ... Secure the document - GBV 6/21/2010
    '    aExp.Security.Encrypt = True
    '    aExp.Security.OwnerPassword = "A5103255307"
    '    aExp.Security.Permissions = GrapeCity.ActiveReports.Export.Pdf.Section.PdfPermissions.AllowPrint
    '    aExp.Security.Use128Bit = True
    'End Sub

#End Region

    'BHS 10/17/08
    ''' <summary> Run a function given its Function Number.  Returns true if function is found and run </summary>
    Function RunFunction(ByVal aFunctNo As String) As Boolean
        Dim QMI, QMI2 As qMenuItem

        For Each MI In Appl.gMenu.Items
            For Each TSI In CType(MI, ToolStripDropDownItem).DropDownItems
                If TypeOf TSI Is qMenuItem Then
                    QMI = CType(TSI, qMenuItem)
                    If QMI._FunctNo = aFunctNo Then
                        QMI.PerformClick()
                        Return True
                    End If
                    'BHS 11/15/10 Run from possible submenu
                    For Each TSI2 In CType(QMI, ToolStripDropDownItem).DropDownItems
                        If TypeOf TSI2 Is qMenuItem Then
                            QMI2 = CType(TSI2, qMenuItem)
                            If QMI2._FunctNo = aFunctNo Then
                                QMI2.PerformClick()
                                Return True
                            End If
                        End If
                    Next
                End If
            Next
        Next

        Return False

    End Function

    'GBV 2/2/2009
    ''' <summary> Get Fiscal Year from a Date object. Function returns integer</summary>
    Function GetFiscalYear(ByVal aDate As Date) As Integer
        Dim aMonth As Integer = aDate.Month
        Dim aYear As Integer = aDate.Year
        If aMonth >= 7 Then Return aYear + 1
        Return aYear
    End Function

    'GBV 12/28/2010
    ''' <summary>Strip all characters that are not letters or numbers. Respect QBE symbols</summary>
    Public Function StripJunk(ByVal aString As String, Optional ByVal aIsQBE As Boolean = False) As String
        Dim CleanString As String = aString
        Dim WrkStr As String = ""
        Dim QBESymbols As String = "<>=|+?{*"

        ' if this is not a QBE string, get rid of everything that is not a number or a letter
        If Not aIsQBE Then
            For i As Integer = 0 To CleanString.Length - 1
                If IsNumeric(CleanString(i)) OrElse
                   (Asc(CleanString(i)) >= 65 AndAlso
                    Asc(CleanString(i)) <= 90) OrElse
                   (Asc(CleanString(i)) >= 97 AndAlso
                    Asc(CleanString(i)) <= 122) Then
                    WrkStr &= CleanString(i)
                End If
            Next
            CleanString = WrkStr
        Else ' If this is a QBE string, we must watch for QBE symbols
            Dim j As Integer = 0
            While j < CleanString.Length
                If QBESymbols.IndexOf(CleanString(j)) > -1 OrElse
                   IsNumeric(CleanString(j)) OrElse
                   (Asc(CleanString(j)) >= 65 AndAlso
                    Asc(CleanString(j)) <= 90) OrElse
                   (Asc(CleanString(j)) >= 97 AndAlso
                    Asc(CleanString(j)) <= 122) Then
                    WrkStr &= CleanString(j)
                    If CleanString(j) = "{" Then
                        j += 1
                        While j < CleanString.Length AndAlso CleanString(j) <> "}"
                            WrkStr &= CleanString(j)
                            j += 1
                        End While
                        If j < CleanString.Length AndAlso CleanString(j) = "}" Then
                            WrkStr &= CleanString(j)
                        End If
                    End If
                End If
                j += 1
            End While
            CleanString = WrkStr
        End If
        Return CleanString
    End Function

    'SRM 03/20/2013
    ''' <summary>Strip all characters that are not numbers. </summary>
    Public Function StripToNumbersOnly(ByVal aString As String) As String
        Dim CleanString As String = ""

        For i As Integer = 0 To aString.Length - 1
            If IsNumeric(aString(i)) Then CleanString &= aString(i)
        Next

        Return CleanString
    End Function

    'For PlantID

    'Returns String handling NULLS
    Function GetString(ByVal aStr As Object) As String
        If aStr Is Nothing Then Return ""
        If IsDBNull(aStr) Then Return ""
        Return aStr.ToString
    End Function

    '''<summary>Abbreviated call to PrepareSQLSearchString</summary>
    Function PSS(ByVal aStr As String) As String
        Return PrepareSQLSearchString(aStr)
    End Function

    'This seems to be equivalent to ToDec above
    Function GetNumber(ByVal aNum As Object) As Decimal
        If aNum Is Nothing Then Return 0
        If aNum Is DBNull.Value Then Return 0
        If IsNumeric(aNum) Then Return CType(aNum, Decimal)
        Return 0
    End Function

    'Fill ASP.Net DropDownList from Name/Value Pairs string (minimal parameters)
    Sub FillDropDownList(ByVal aNVPairs As String, ByRef aCB As System.Web.UI.WebControls.DropDownList)
        Dim DV As DataView = BuildNameValueDV(aNVPairs, "=", ",")
        aCB.DataSource = DV
        aCB.DataTextField = "name"
        aCB.DataValueField = "value"

    End Sub
End Module


