Imports System.Data
Imports System.Data.SqlClient
Imports System.Transactions
Imports System.Windows.Forms
Imports QSILib

Public Class feLoadQuery
    Public iClassname As String
    Private iHomeOffice As String
    Private iLastQueryType As String = ""
    Public iQBEControl As Control
    Private iSkipRecords As String = "'Window Size','Report Zoom','Layout','Window Loc'"
    Public iVersionRecno As Integer = 0

#Region "---------------------------- Load --------------------------------------"

    'Set Context to allow ancestors to see key objects
    Private Sub feLoadQuery_OnSetContext() Handles Me.OnSetContext

        Try
            iSQLCn = New SqlConnection(gSQLConnStr)
            iConnType = "SQL"    'Notify library that this form should work with SQL Server rather than Informix
            'iListNavigator = iListForm.iListNavigator   'Points back to List form navigator
            'iListGV = iListForm.iListGV                 'Points back to List gridview
            'iBSs.Add(ibs)                               'Load Binding Source List
            'iDS = New DataSet                           'Initialize dataset

            'Get user's home office
            iHomeOffice = SQLGetString("Select HCamp From SQLPTS..UserMain Where UserID = '" & gUserName & "'")

            iFirstKeyField = "txtTitle"
            iFirstNonKeyField = "txtTitle"
            'iKeyFields.Add(txtProject)                'Load Key Fields List

            'Fill comboboxes
            '...prepare ddQueries dropdown
            ddQueries._TextColumn = "name"
            ddQueries._SelectedValueColumn = "value"
            ddQueries.AddColumn("name")
            ddQueries.AddColumn("value", , "", False)
            '...fill Query Type dropdown
            FillDD("My Saved Queries=M,Campus Shared Queries=C,System-wide Shared Queries=A", ddQueryType, , , True)

        Catch ex As Exception
            ShowError("Unable to open Load Query screen.", ex)
            Post("Close")
        End Try

    End Sub

    'Load form from database
    Private Sub feLoadQuery_OnLoadForm() Handles Me.OnLoadForm

        'Try

        'Catch ex As Exception
        '    ShowError("Unable to load form from database.", ex)
        '    Post("Close")
        'End Try

    End Sub

    Private Sub feLoadQuery_OnEndOfLoad() Handles Me.OnEndOfLoad
        Try
            ddQueryType.SetWriter(True)
            '...default to My Saved Queries
            ddQueryType.SelectedValue = "M"
            '...always make the queries dropdown available
            ddQueries.SetWriter(True)
            'btnLoad.Visible = False
            '...set focus
            ddQueryType.Focus()
            SendKeys.Send("%") 'Need this to get the underlines to show up on the buttons for some reason
        Catch ex As Exception
        End Try
    End Sub

    Private Sub ddQueryType_onDropDown(ByVal Sender As Object, ByRef aOK As Boolean) Handles ddQueryType.onDropDown
        '...keep track of last query type
        iLastQueryType = ddQueryType.Text
    End Sub

    Private Sub ddQueryType_SelectedIndexChanged(sender As Object, ByRef e As System.Windows.Forms.DataGridViewRowEventArgs) Handles ddQueryType.SelectedIndexChanged
        populate_queries()
    End Sub

    Private Sub populate_queries()
        Dim DV As DataView = Nothing
        Dim nonefound As Boolean = False
        Dim title, wstr As String
        Dim QBERecno As Integer

        '...fill query dropdown, based on type
        If ddQueryType.Text > "" Then
            '...my queries
            If ddQueryType.SelectedValue.ToString = "M" Then
                ' GBV 4/21/2015 - Ticket 3666 - moved all tQBE_xxx tables to IASCommon database
                wstr = "Select Title name, QBERecno value, DefaultQuery From IASCommon..tQBE_Version Where Classname = '" & iClassname & _
                    "' And SavedBy = '" & gUserName & "' And Title Not In (" & iSkipRecords & ") Order By 1"
                DV = BuildDV(wstr, False)
            End If
            '...campus shared queries (shared = "A" or "C", same home campus as mine)
            If ddQueryType.SelectedValue.ToString = "C" Then
                ' GBV 4/21/2015 - Ticket 3666 - moved all tQBE_xxx tables to IASCommon database
                wstr = "Select Title name, QBERecno value, SavedBy, DefaultQuery From IASCommon..tQBE_Version tQBE_Version Where Classname = '" & iClassname & _
                    "' And SavedBy <> '" & gUserName & "' And Title Not In (" & iSkipRecords & ") And " & _
                    "((Shared = 'A' Or Shared = 'C') And (Select HCamp From SQLPTS..UserMain Where UserID = tQBE_Version.SavedBy) = '" & _
                    iHomeOffice & "') Order By 1"
                DV = BuildDV(wstr, False)
                If DV.Count > 0 Then
                    For Each R As DataRow In DV.Table.Rows
                        'NOTE: If use of square brackets changes, then change logic in fill_query_fields
                        R.Item("name") = "[" & R.Item("savedby").ToString.Trim & "] - " & R.Item("name").ToString
                    Next
                End If
            End If
            '...system-wide shared queries (shared = "A", not same home campus as mine)
            If ddQueryType.SelectedValue.ToString = "A" Then
                ' GBV 4/21/2015 - Ticket 3666 - moved all tQBE_xxx tables to IASCommon database
                wstr = "Select Title name, QBERecno value, SavedBy, HCamp, DefaultQuery From IASCommon..tQBE_Version tQBE_Version, " & _
                    "SQLPTS..UserMain UserMain Where Classname = '" & _
                    iClassname & "' And SavedBy <> '" & gUserName & "' And Title Not In (" & iSkipRecords & ") And " & _
                    "(Shared = 'A' And (Select HCamp From SQLPTS..UserMain UserMain Where UserID = tQBE_Version.SavedBy) <> '" & iHomeOffice & _
                    "') And " & "UserMain.UserID = tQBE_Version.SavedBy Order By 1"
                DV = BuildDV(wstr, False)
                If DV.Count > 0 Then
                    For Each R As DataRow In DV.Table.Rows
                        'NOTE: If use of square brackets changes, then change logic in fill_query_fields
                        R.Item("name") = "[" & R.Item("HCamp").ToString & "][" & R.Item("savedby").ToString.Trim & "] - " & _
                            R.Item("name").ToString
                    Next
                End If
            End If
        End If
        '...if no queries of that type found, fill with dummy record
        QBERecno = 0
        title = ""
        If DV.Count = 0 Then
            ' GBV 4/21/2015 - Ticket 3666 - moved all tQBE_xxx tables to IASCommon database
            wstr = "Select '' name, 0 value From IASCommon..tQBE_Version Where 1=2"
            DV = BuildDV(wstr, True)
            DV.Item(0).Item("name") = "[No Queries Found]"
            DV.Item(0).Item("value") = 0
            nonefound = True
        Else
            '...if display my queries, look for default
            If ddQueryType.SelectedValue.ToString = "M" Then
                For Each R As DataRow In DV.Table.Rows
                    If CBool(R.Item("DefaultQuery")) = True Then
                        title = R.Item("name").ToString
                        QBERecno = CInt(R.Item("value"))
                        Exit For
                    End If
                Next
            End If
        End If
        '...fill Queries dropdown
        ddQueries.DataSource = DV
        '...clear selected query
        If ddQueryType.Text <> iLastQueryType Then
            ddQueries.Text = ""
            fill_query_fields()
            iLastQueryType = ddQueryType.Text
        End If
        '...if no queries found, make sure dummy displays in dropdown field
        If nonefound = True Then
            ddQueries.SelectedValue = 0
        Else
            '...see if the user's default query is of this type; otherwise, get first record
            If QBERecno = 0 AndAlso DV.Count > 0 Then
                title = DV.Item(0).Item("name").ToString
                QBERecno = CInt(DV.Item(0).Item("value"))
            End If
            If QBERecno > 0 Then
                ddQueries.Text = title
                ddQueries.SelectedValue = QBERecno
                fill_query_fields()
            End If
        End If
    End Sub

    Private Sub ddQueries_SelectedIndexChanged(sender As Object, ByRef e As DataGridViewRowEventArgs) Handles ddQueries.SelectedIndexChanged
        fill_query_fields()
    End Sub

    Private Sub btnLoad_Click(sender As System.Object, e As System.EventArgs) Handles btnLoad.Click
        iVersionRecno = 0
        If ddQueries.Text > "" Then iVersionRecno = CInt(ddQueries.SelectedValue.ToString)
        Close()
    End Sub

    Private Sub fill_query_fields()
        Dim DV As DataView
        Dim wstr As String

        '...locate query and fill in the fields
        If ddQueries.Text > "" Then
            ' GBV 4/21/2015 - Ticket 3666 - moved all tQBE_xxx tables to IASCommon database
            wstr = "Select * From IASCommon..tQBE_Version Where QBERecno = " & ddQueries.SelectedValue.ToString
            DV = BuildDV(wstr, False)
            If DV.Count = 1 AndAlso CInt(ddQueries.SelectedValue.ToString) > 0 Then
                With DV.Item(0)
                    txtTitle.Text = .Item("title").ToString
                    txtDescr.Text = .Item("descr").ToString
                    txtSavedBy.Text = .Item("savedby").ToString
                    txtCreatedDt.Text = ""
                    If Not IsDBNull(.Item("createddt")) Then txtCreatedDt.Text = CDate(.Item("createddt")).ToString("MM/dd/yyyy")
                    txtModifiedDt.Text = ""
                    If Not IsDBNull(.Item("modifieddt")) Then txtModifiedDt.Text = CDate(.Item("modifieddt")).ToString("MM/dd/yyyy")
                End With
            Else
                If CInt(ddQueries.SelectedValue.ToString) = 0 Then
                    txtTitle.Text = ""
                    txtDescr.Text = ""
                    txtSavedBy.Text = ""
                    txtCreatedDt.Text = ""
                    txtModifiedDt.Text = ""
                    btnLoad.Visible = False
                Else
                    MsgBoxErr("Can't find saved Query.", , "Load Query")
                    btnLoad.Visible = False
                End If
                Return
            End If
        Else
            txtTitle.Text = ""
            txtDescr.Text = ""
            txtSavedBy.Text = ""
            txtCreatedDt.Text = ""
            txtModifiedDt.Text = ""
        End If
        '...manage the Load button
        btnLoad.Visible = (ddQueries.Text > "")
    End Sub

#End Region

End Class
