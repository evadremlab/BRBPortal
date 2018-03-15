Imports QSILib
Imports System.Data.SqlClient
Imports QSILib.Windows.Forms

Public Class RecordLock

#Region "-------------------- Documentation ----------------------"

    'PRODUCTION INSTALLATION TO DO
    'Update tLocks to include prim_key as an identity field, at the end of the 
    'file design.  Putting prim_key at the end will accomodate earlier INSERT 
    'statements that did not list
    'fields before the VALUES section.  This has been tested and works in Test database,
    'but we should wait to update Production until this code goes in in Release 6.

    'RECORDLOCK Class
    'BHS 6/29/12
    '
    'Manages the setting and clearing of record locks to allow programs to manage
    'logical pessimistic locking, both at the record level and at the whole-function level.  
    '
    'Dependencies - 
    '   QSILib provides Appl.gSQLConnStr and standard SQL functions
    '   SQLPTS Database provides tlocks table
    '
    'Behavior -
    '   IsLocked returns False until we've got a locked record
    '   LockTable, Partn, LockKey, LockedBy and LockedDt are empty until we've
    '     attempted a lock.  iPrim_Key = 0 if we don't have a lock.
    '   Once iLockHasBeenTested = True, we don't recheck Lock table until Clear()
    '     or CheckLock() is called
    '   iSuppressMessageBoxes and iForceLock can be set at instantiation from crons
    '
    'Typical use -
    '   New()
    '   AttemptLock() when lock is needed
    '   -or- AttemptFunctionLock() to lock a whole function from other users
    '   Inspect RecordLock properties if useful to your program
    '   CheckLock() to confirm lock hasn't been knocked out by another program.  This is
    '     called from feMain just before Saving
    '   Clear() when lock is no longer needed




#End Region

#Region "-------------------- Instance Variables and Properties ----------------------"


    Private iIsLocked As Boolean = False    'True if record is locked
    Private iLockHasBeenTested As Boolean = False     'True if lock has been attempted
    Private iHoursBeforeLockIgnored As Integer = 12  'Replace locks older than 12 hours

    'tLocks values
    Private iLockTable As String = ""   'Name of the table being locked
    Private iPartn As String = ""       'Name of the Partition the table is in
    Private iLockKey As String          '| Concatenated string reflecting the unique key
    Private iLockedBy As String = ""    'gUserName of person who locked the record
    Private iLockedByName As String = ""    'Full user name of person who locked the record
    Private iLockedDt As DateTime = Nothing 'Date Time this record was locked
    Private iPrim_Key As Integer = 0    'Primary key used to reaccess the lock record

    'New parameters:
    Private iSuppressMessageBoxes As Boolean = False    'True if called from a cron
    Private iForceLock As Boolean = False               'True if we're going to replace any locks that get in the way

    Private iCn As SqlConnection = Nothing  'Connection to tlocks in SQLPTS
    Private iFunctNo As String = "" ' GBV 1/29/2014 - Ticket 2439
    Private iCallingForm As feMain2 = Nothing

    Public ReadOnly Property IsLocked() As Boolean
        Get
            Return iIsLocked

        End Get
    End Property

    Public ReadOnly Property LockHasBeenTested() As Boolean
        Get
            Return iLockHasBeenTested

        End Get
    End Property

    Public ReadOnly Property LockTable() As String
        Get
            Return iLockTable
        End Get
    End Property

    Public ReadOnly Property Partn() As String
        Get
            Return iPartn
        End Get
    End Property

    Public ReadOnly Property LockKey() As String
        Get
            Return iLockKey
        End Get
    End Property

    Public ReadOnly Property LockedBy() As String
        Get
            Return iLockedBy
        End Get
    End Property

    Public ReadOnly Property LockedDt() As DateTime
        Get
            Return iLockedDt
        End Get
    End Property

    ' GBV - Ticket 2439 - 1/23/2015
    Public ReadOnly Property FunctionNumber() As String
        Get
            Return iFunctNo
        End Get
    End Property

#End Region

#Region "-------------------- New ----------------------"

    'New
    ''' <summary>Set whether this is a non-visual cron, and whether we are to Force a Lock 
    ''' GBV - 10/23/2014 - Added aCallingForm Optional parameter - Ticket 2439</summary>
    Public Sub New(Optional ByVal aSuppressMessageBoxes As Boolean = False, _
                   Optional ByVal aForceLock As Boolean = False, _
                   Optional ByRef aCallingForm As feMain2 = Nothing)
        iSuppressMessageBoxes = aSuppressMessageBoxes
        iForceLock = aForceLock
        iCallingForm = aCallingForm
        If iCallingForm IsNot Nothing Then
            iSuppressMessageBoxes = True
        End If

    End Sub

#End Region

#Region "-------------------- Attempt Lock ----------------------"

    'AttemptLock - returns True if we can add a new lock record, or False with a message
    '   if the lock is already taken.
    ''' <summary> Try to lock aTable in aPartn, using aKey with | delimeters </summary>
    Function AttemptLock(ByRef aTable As String, ByRef aPartn As String, ByRef aKey As String, Optional ByVal aFunctionNo As String = "") As Boolean
        Dim SQL As String = ""
        Dim wstr As String
        Dim DV As DataView

        Try
            'Only do the test once, until RecordLock is cleared
            If iLockHasBeenTested = True Then Return iIsLocked
            iLockTable = aTable
            iPartn = aPartn
            iLockKey = aKey
            iFunctNo = aFunctionNo

            '12/3/12 DJW - Do not attempt to lock a key which is blank (new record), just return TRUE
            If iLockKey = "" Then
                'iIsLocked = True
                iLockHasBeenTested = True
                FillInstanceVariables(aTable, aPartn, aKey)
                Return True
            End If

            'Starting values in case we're interrupted:
            iIsLocked = False
            iLockHasBeenTested = False

            If iCn Is Nothing Then iCn = New SqlConnection(Appl.gSQLConnStr)
            If iCn.State = ConnectionState.Closed Then iCn.Open()

            SQL = "SELECT * FROM tLocks WHERE lock_table = '" & aTable & "' AND partn = '" & aPartn & "' AND lock_key = '" & Clean(aKey) & "'"
            DV = SQLBuildDV(SQL, False)

            ' GBV 7/20/2015 - Ticket 2439
            SQL = "SELECT count(*) FROM tLocks WHERE lock_table = '" & aTable & "' AND partn = '" & aPartn & "' AND lock_key = '" & Clean(aKey) & "'"
            If DV.Count = 0 Then ' ******* LOCK NOT FOUND *********
                SQL = "INSERT INTO tLocks (lock_table, partn, lock_key, lock_user, lock_dtm, funct_no) " &
                      " VALUES ('" & aTable & "', '" & aPartn & "', '" & Clean(aKey) & "', '" & gUserName & "', '" & Now.ToString & "', '" & aFunctionNo & "')"
                SQLDoSQL(SQL)
                iIsLocked = True
                iLockHasBeenTested = True
                FillInstanceVariables(aTable, aPartn, aKey)
                ' GBV - 10/24/2014 - Ticket 2439
                ' Per phone conversation with Sajeel on 1/26/2015,
                ' do not show anything in this case.
                If iCallingForm IsNot Nothing Then
                    iCallingForm.PanelLock.Visible = False
                End If
                ' ****** End of GBV 10/24/2014 **********
                Return True
            Else ' ********* LOCK FOUND *************
                ' ... Chek if this function and this user are the owners of the lock
                SQL = "SELECT count(*) FROM tLocks WHERE lock_table = '" & aTable & "' " &
                      " AND partn = '" & aPartn & "' AND lock_key = '" & Clean(aKey) & "' " &
                      " AND funct_no = '" & aFunctionNo & "' " &
                      " AND lock_user = '" & gUserName & "'"
                If SQLGetNumber(SQL) > 0 Then ' ********** They are the owner *********
                    ' set up a few things and get out
                    iIsLocked = True
                    iLockHasBeenTested = True
                    FillInstanceVariables(aTable, aPartn, aKey)
                    If iCallingForm IsNot Nothing Then
                        iCallingForm.PanelLock.Visible = False
                    End If
                    Return True
                Else ' ************* They are not the owner *********************
                    FillInstanceVariables(aTable, aPartn, aKey)
                    'If lock is older than iHoursBeforeLockIgnored hours, delete it and add new lock record
                    If DateDiff(DateInterval.Hour, iLockedDt, Now) >= iHoursBeforeLockIgnored Then
                        ' GBV 10/24/2014
                        If ReplaceLock() Then
                            iIsLocked = True
                            Return True
                        Else
                            iIsLocked = False
                            If iCallingForm IsNot Nothing Then
                                iCallingForm.PanelLock.Visible = True
                                iCallingForm.lblLock.Text = "Locked by " & DV.Table.Rows(0).Item("lock_user").ToString &
                                                            " in NUI " & DV.Table.Rows(0).Item("funct_no").ToString
                                iCallingForm.lblLock.Visible = True
                                If gUserName = DV.Table.Rows(0).Item("lock_user").ToString.Trim AndAlso
                                   SQLGetString("SELECT LockGroup FROM gMenu WHERE funct_no = '" & aFunctionNo & "'") =
                                   SQLGetString("SELECT LockGroup FROM gMenu WHERE funct_no = '" & DV.Table.Rows(0).Item("funct_no").ToString.Trim & "'") Then
                                    iCallingForm.btnTxLock.Visible = True
                                Else
                                    iCallingForm.btnTxLock.Visible = False
                                End If
                                If Auth.GetPermLevel("UnlockRecords") = 3 Then
                                    iCallingForm.btnUnlock.Visible = True
                                Else
                                    iCallingForm.btnUnlock.Visible = False
                                End If
                            End If
                            Return False
                        End If
                        'Return ReplaceLock()
                    End If

                    'Otherwise, shall we Force the lock, replacing the existing one?
                    If iForceLock = True Then
                        ' GBV 10/24/2014
                        If ReplaceLock() Then
                            iIsLocked = True
                            Return True
                        Else
                            iIsLocked = False
                            If iCallingForm IsNot Nothing Then
                                iCallingForm.PanelLock.Visible = True
                                iCallingForm.lblLock.Text = "Locked by " & DV.Table.Rows(0).Item("lock_user").ToString &
                                                            " in NUI " & DV.Table.Rows(0).Item("funct_no").ToString
                                iCallingForm.lblLock.Visible = True
                                If gUserName = DV.Table.Rows(0).Item("lock_user").ToString.Trim AndAlso
                                   SQLGetString("SELECT LockGroup FROM gMenu WHERE funct_no = '" & aFunctionNo & "'") =
                                   SQLGetString("SELECT LockGroup FROM gMenu WHERE funct_no = '" & DV.Table.Rows(0).Item("funct_no").ToString.Trim & "'") Then
                                    iCallingForm.btnTxLock.Visible = True
                                Else
                                    iCallingForm.btnTxLock.Visible = False
                                End If
                                If Auth.GetPermLevel("UnlockRecords") = 3 Then
                                    iCallingForm.btnUnlock.Visible = True
                                Else
                                    iCallingForm.btnUnlock.Visible = False
                                End If
                            End If
                            Return False
                        End If
                        'Return ReplaceLock()
                    End If

                    ' GBV - 10/24/2014 - Ticket 2439
                    If iSuppressMessageBoxes = True AndAlso iCallingForm IsNot Nothing Then
                        iCallingForm.PanelLock.Visible = True
                        iCallingForm.lblLock.Text = "Locked by " & DV.Table.Rows(0).Item("lock_user").ToString & " in NUI " & DV.Table.Rows(0).Item("funct_no").ToString
                        iCallingForm.lblLock.Visible = True
                        If DV.Table.Rows(0).Item("lock_user").ToString.Trim = gUserName AndAlso
                           SQLGetString("SELECT LockGroup FROM gMenu WHERE funct_no = '" & aFunctionNo & "'") =
                           SQLGetString("SELECT LockGroup FROM gMenu WHERE funct_no = '" & DV.Table.Rows(0).Item("funct_no").ToString.Trim & "'") Then
                            iCallingForm.btnTxLock.Visible = True
                        Else
                            iCallingForm.btnTxLock.Visible = False
                        End If
                        If Auth.GetPermLevel("UnlockRecords") = 3 Then
                            iCallingForm.btnUnlock.Visible = True
                        Else
                            iCallingForm.btnUnlock.Visible = False
                        End If
                        iIsLocked = True
                        iLockHasBeenTested = False
                        Return False
                    End If
                    ' ******** End of GBV 10/24/2014 *************
                End If
            End If



            '   If user has sufficient authority in a visual environment, we can replace
            If iSuppressMessageBoxes = False AndAlso Auth.GetPermLevel("UnlockRecords") = 3 Then
                wstr = "Record locked by " & UserPhrase() & " at " & iLockedDt.ToString & ".  Lock ID = " & DV.Item(0).Item("lock_key").ToString
                wstr = wstr + Chr(13) + "-----------------------------------------------------------------------" _
                            + Chr(13) + Chr(13) + "Would you like to unlock the record?"
                If MsgBox(wstr, MsgBoxStyle.YesNo, "Record Locked") = MsgBoxResult.Yes Then
                    Return ReplaceLock()
                End If
            End If

            'Otherwise, set instance variables with the existing lock and report failure
            '   to lock for this user
            Dim R As DataRow = DV.Table.Rows(0)

            FillInstanceVariables(GetItemString(R, "lock_table"),
                                  GetItemString(R, "partn"),
                                  GetItemString(R, "lock_key"))

            If iSuppressMessageBoxes = False Then
                wstr = "Record locked by " & UserPhrase() &
                   " at " & iLockedDt.ToString &
                   "  Key ID = " & iLockKey
                MsgBox(wstr, MsgBoxStyle.Information, "Record Locked")
            End If

            iIsLocked = False
            iLockHasBeenTested = True
            Return False

        Catch ex As Exception
            'BHS 1/26/17 suppress error
            'If iSuppressMessageBoxes = False Then
            '    ShowError("Error checking for record lock - " & SQL, ex)
            'Else
            '    Throw New Exception("Error checking for record lock.", ex)
            'End If

            iIsLocked = False
            Return False
        Finally
            'GBV 1/16/2014 - close connection to avoid connection pool issues
            If iCn IsNot Nothing AndAlso iCn.State = ConnectionState.Open Then iCn.Close()
        End Try

    End Function

    'BHS Return full user name and gUserName
    Function UserPhrase() As String
        If iLockedByName > "" Then
            Return iLockedByName & " [" & iLockedBy & "]"
        Else
            Return iLockedBy
        End If

    End Function

    'AttemptFunctionLock - returns True if we can add a new lock record, 
    'or False with a message if the lock is already taken.  
    'Function number is written in tLocks.lock_table, and partn and lock_key are blank.

    ''' <summary> Try to lock a Function (stored in lock_table) </summary>
    Function AttemptFunctionLock(ByRef aFunct As String, Optional ByVal aFunctionNo As String = "") As Boolean
        Dim SQL As String = ""
        Dim wstr As String
        Dim DV As DataView

        Try
            'Only do the test once, until RecordLock is cleared
            If iLockHasBeenTested = True Then Return iIsLocked
            iLockTable = aFunct
            iPartn = ""
            iLockKey = ""
            iFunctNo = aFunctionNo

            'Starting values in case we're interrupted:
            iIsLocked = False
            iLockHasBeenTested = False

            If iCn Is Nothing Then iCn = New SqlConnection(Appl.gSQLConnStr)
            If iCn.State = ConnectionState.Closed Then iCn.Open()

            SQL = "SELECT * FROM tLocks WHERE lock_table = '" & iLockTable &
            "' AND partn = '' AND lock_key = ''"
            DV = SQLBuildDV(SQL, False)

            '--- LOCK NOT FOUND --- so add lock record and return
            If DV.Count = 0 Then
                SQL = "INSERT INTO tLocks (lock_table, partn, lock_key, lock_user, lock_dtm, funct_no) " &
            " VALUES ('" & iLockTable & "', '', '', '" & gUserName & "', '" & Now.ToString & "', '" & aFunctionNo & "')"
                SQLDoSQL(SQL)
                iIsLocked = True
                iLockHasBeenTested = True
                FillInstanceVariables(iLockTable, iPartn, iLockKey)
                Return True
                'Need to discuss the below change with Sajeel before moving forward
                'ElseIf DV.Item(0).Item("lock_user").ToString = gUserName Then
                '    wstr = "You currently have this function locked.  Continue anyway?"
                '    If MsgBox(wstr, MsgBoxStyle.YesNo, "Record Locked") = MsgBoxResult.Yes Then
                '        iIsLocked = True
                '        iLockHasBeenTested = True
                '        FillInstanceVariables(iLockTable, iPartn, iLockKey)
                '        Return True
                '    End If
            End If

            '--- LOCK RECORD FOUND ---
            FillInstanceVariables(iLockTable, iPartn, iLockKey)
            'If lock is older than iHoursBeforeLockIgnored hours, delete it and add new lock record
            If DateDiff(DateInterval.Hour, iLockedDt, Now) >= iHoursBeforeLockIgnored Then
                Return ReplaceLock()
            End If

            'Otherwise, shall we Force the lock, replacing the existing one?
            If iForceLock = True Then Return ReplaceLock()

            '   If user has sufficient authority in a visual environment, we can replace
            If iSuppressMessageBoxes = False AndAlso Auth.GetPermLevel("UnlockRecords") = 3 Then
                wstr = "This function was locked by " & UserPhrase() & " at " & iLockedDt.ToString & "."
                wstr = wstr + Chr(13) + "-----------------------------------------------------------------------" _
                            + Chr(13) + Chr(13) + "Would you like to override the function lock?"
                If MsgBox(wstr, MsgBoxStyle.YesNo, "Function Locked") = MsgBoxResult.Yes Then
                    Return ReplaceLock()
                End If
            End If

            'Otherwise, set instance variables with the existing lock and report failure
            '   to lock for this user
            Dim R As DataRow = DV.Table.Rows(0)

            FillInstanceVariables(GetItemString(R, "lock_table"),
                                  GetItemString(R, "partn"),
                                  GetItemString(R, "lock_key"))

            If iSuppressMessageBoxes = False Then
                wstr = "This function was locked by " & UserPhrase() &
                   " at " & iLockedDt.ToString
                MsgBox(wstr, MsgBoxStyle.Information, "Function Locked")
            End If

            iIsLocked = False
            iLockHasBeenTested = True
            Return False

        Catch ex As Exception
            'BHS 1/26/17 Suppress error message
            'If iSuppressMessageBoxes = False Then
            '    ShowError("Error checking for function lock - " & SQL, ex)
            'Else
            '    Throw New Exception("Error checking for function lock.", ex)
            'End If

            iIsLocked = False
            Return False
        Finally
            'GBV 1/16/2014 - close connection to avoid connection pool issues
            If iCn.State = ConnectionState.Open Then iCn.Close()
        End Try

    End Function

    'FillInstanceVariables - called whenever the lock record is changed
    ''' <summary>Fill instance variables based on current tlock record. </summary>
    Private Sub FillInstanceVariables(ByVal aTable As String, _
                                      ByVal aPartn As String, _
                                      ByVal aKey As String)

        Dim Sql As String = "SELECT * FROM tLocks WHERE lock_table = '" & aTable & "' AND partn = '" & aPartn & "' AND lock_key = '" & Clean(aKey) & "'"
        Dim DV As DataView = SQLBuildDV(Sql, False)

        If DV.Table.Rows.Count = 0 Then
            Clear()
            Return
        End If

        Dim R As DataRow = DV.Table.Rows(0)

        iLockTable = GetItemString(R, "lock_table")
        iPartn = GetItemString(R, "partn")
        iLockKey = GetItemString(R, "lock_key")
        iLockedBy = GetItemString(R, "lock_user")
        If iLockedBy IsNot Nothing AndAlso iLockedBy > "" Then
            iLockedByName = SQLGetString("SELECT fullname FROM SQLPTS..UserMain " & _
                                     "  WHERE UserID = '" & iLockedBy & "'")

        Else
            iLockedByName = ""
        End If
        
        iLockedDt = Nothing
        Dim ob As Object = DV.Table.Rows(0).Item("lock_dtm")
        If ob IsNot Nothing AndAlso ob IsNot DBNull.Value Then
            iLockedDt = CType(ob, DateTime)
        End If

        iPrim_Key = CInt(GetItemNumber(DV.Table.Rows(0), "prim_key"))

        iFunctNo = GetItemString(R, "funct_no")
    End Sub

    'ReplaceLock, called from AttemptLock
    ''' <summary>Replace existing lock with new one </summary>
    Private Function ReplaceLock() As Boolean
        Dim SQL As String = ""
        Try

            If iPrim_Key = 0 Then
                Throw New Exception("Programmer Error - attempted to Replace Lock with no primary key reference")
            End If

            SQL = "DELETE FROM tLocks WHERE prim_key = " & iPrim_Key.ToString
            SQLDoSQL(SQL)

            SQL = "INSERT INTO tLocks (lock_table, partn, lock_key, lock_user, lock_dtm, funct_no) " & _
            " VALUES ('" & iLockTable & "', '" & iPartn & "', '" & Clean(iLockKey) & "', '" & gUserName & "', '" & Now.ToString & "', '" & iFunctNo & "')"
            SQLDoSQL(SQL)
            iIsLocked = True
            FillInstanceVariables(iLockTable, iPartn, iLockKey)
            Return True

        Catch ex As Exception
            If iSuppressMessageBoxes = False Then
                ShowError("Error attempting to replace lock - " & SQL, ex)
            Else
                Throw New Exception("Error attempting to replace lock.", ex)
            End If

            iIsLocked = False
            Return False
        Finally
            iLockHasBeenTested = True
        End Try
    End Function
#End Region

#Region "-------------------- Check Lock ----------------------"
    '''<Summary> Check to see if we still have a record lock</Summary>
    Public Function CheckLock() As Boolean
        Dim SQL As String = ""
        Dim DV As DataView = Nothing

        SQL = "SELECT * FROM tLocks WHERE prim_key = " & iPrim_Key.ToString
        DV = SQLBuildDV(SQL, False)

        If DV.Count = 0 Then
            If iSuppressMessageBoxes = False Then
                Dim wstr As String = "You have lost the lock on this record.  " & _
                "This may be due to a timeout if you left the record open for a long time, or it may be due to another program taking precedence.  " & _
                Chr(13) & "Please close this form and bring it up again to make your changes."
                MsgBox(wstr, MsgBoxStyle.Information, "Record No Longer Locked")
            End If

            iIsLocked = False
            Return False

        End If

        iIsLocked = True
        Return True

    End Function

#End Region

#Region "-------------------- Clear ----------------------"
    'Clear lock in database and local instance variables
    ''' <summary> Clear lock object, including removing tLock records </summary>
    Public Sub Clear()

        Try
            ' GBV 7/21/2015 - if not the same user or not enough authority, do not delete lock
            Dim Lock_user As String = SQLGetString("SELECT lock_user FROM tLocks " & _
                                                   "WHERE lock_table = '" & iLockTable & "' " & _
                                                   "  AND partn = '" & iPartn & "' " & _
                                                   "  AND lock_key = '" & iLockKey & "'")
            If Lock_user = gUserName OrElse Auth.GetPermLevel("UnlockRecords") = 3 Then
                If iIsLocked = True Then UnlockRecord(iLockTable, iPartn, iLockKey)
            Else
                Return
            End If
            iPrim_Key = 0
            iLockTable = ""
            iPartn = ""
            iLockKey = ""
            iLockedBy = ""
            iLockedDt = Nothing
            iIsLocked = False
            iLockHasBeenTested = False
            iFunctNo = ""

        Catch ex As Exception
            If iSuppressMessageBoxes = False Then
                ShowError("Unexpected error clearing lock", ex)
            Else
                Throw New Exception("Unexpected error clearing lock.", ex)
            End If
        End Try

    End Sub

    'UnlockRecord, called as part of Clear()
    ''' <summary> Remove lock record from lock_table  </summary>
    Private Function UnlockRecord(ByRef aTable As String, ByRef aPartn As String, ByRef aKey As String) As Boolean
        Dim SQL As String
        Try
            If iPrim_Key = 0 Then
                Throw New Exception("Programmer Error - attempted to Unlock Record with no primary key reference")
            End If

            SQL = "DELETE FROM tLocks WHERE prim_key = " & iPrim_Key
            SQLDoSQL(SQL)

        Catch ex As Exception
            If iSuppressMessageBoxes = False Then
                ShowError("Error unlocking record.", ex)
            Else
                Throw New Exception("Error unlocking record.", ex)
            End If

        End Try

        Return True

    End Function

    Public Sub ResetLockFlags(aFlag As Boolean)
        iIsLocked = aFlag
        iLockHasBeenTested = aFlag
    End Sub

#End Region

End Class
