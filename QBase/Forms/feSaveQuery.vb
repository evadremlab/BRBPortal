Imports System.Data
Imports System.Data.SqlClient
Imports System.Transactions
Imports System.Windows.Forms
Imports QSILib

Public Class feSaveQuery
    Private ReadyForResultsGrid As Boolean = True

    Private iAllDays As String
    Public iAllowEmails As Boolean = True
    Public iClassname As String
    Public iClassnamedescr As String = ""
    Private iCurrentRecno As Integer = 0
    Private iDayCheckbox As Boolean = False
    Private iDVUsers As DataView
    Private iHomeOffice As String
    Private iIteration As Integer = 1
    Public iQBEControl As Control
    Public iOriginalRecno As Integer = 0
    Public iVersionType As String = ""      '..."R" = report option screen; "S" = screen QBE

    Private iBStQBEcc As New BindingSource
    Private WithEvents itQBEcc As New DataTable
    Private WithEvents itQBEresults As New DataTable
    Private WithEvents itQBEversion As New DataTable
    Private WithEvents iDAtQBEcc As New SqlDataAdapter
    Private WithEvents iDAtQBEversion As SqlDataAdapter

#Region "---------------------------- Notes To Programmers ---------------------------"
    'An original QBE recno (iOriginalRecno) is passed into this function, and the user can choose to work with that version or 
    'a different version, which is called iCurrentRecno.  The user may also choose to add a new version, in which case the
    'iCurrentRecno is zero.  If iCurrentRecno is equal to iOriginalRecno, or if it is zero, the user may change the Title
    'field.  Otherwise, the Title field is protected.  We do this because we don't want the user to change the Title of a
    'query version that has not been loaded.  In other words, the user must <Load> a query in order to change its Title and 
    'QBE fields.  
    '
    'We uphold the following logic for changing the detail QBE records, when the user hits <Save>:
    '
    ''          If iOriginalTitle =      ABC     ABC     ABC     blank       blank      ***OLD***
    ''           If iCurrentTitle =      ABC     GHI     <new>   GHI         <new>      ***OLD***
    ''             If Title Field =      DEF     GHI*    GHI     GHI*        GHI        ***OLD***
    ''Delete Original QBE On <Save>      Yes     No      No      No          No         ***OLD***
    ''        Add New QBE On <Save>      Yes     No      Yes     No          Yes        ***OLD***

    '                                   (A)     (B)     (C)     (D)     (E)
    '          If iOriginalRecno =      123     123     123     0       0
    '           If iCurrentRecno =      123     789     0       789     0
    '   Title Field Can Be Changed      Yes     No      Yes     No      Yes
    'Delete Original QBE On <Save>      Yes     No      No      No      No
    '        Add New QBE On <Save>      Yes     No      Yes     No      Yes
    '
    '(A) Calling screen has version loaded, and we're saving changes to that version
    '(B) Calling screen has version loaded, and we're trying to save changes to another version
    '(C) Calling screen has version loaded, and we're saving a <NEW> version
    '(D) Calling screen does not have a version loaded, and we're trying to save changes to an existing version
    '(E) Calling screen does not have a version loaded, and we're saving a <NEW> version

#End Region

#Region "---------------------------- Load -------------------------------------------"

    'Set Context to allow ancestors to see key objects
    Private Sub feSaveQuery_OnSetContext() Handles Me.OnSetContext
        Dim SQL As String

        Try
            iSQLCn = New SqlConnection(gSQLConnStr)
            iConnType = "SQL"    'Notify library that this form should work with SQL Server rather than Informix

            iHomeOffice = SQLGetString("Select HCamp From UserMain Where UserID = '" & gUserName & "'")

            iFirstKeyField = "txtTitle"
            iFirstNonKeyField = "txtTitle"

            ' GBV 11/13/2014 - Changed query statement to include only active users.
            ' I also made ddCCUsers _MustMatchList = True so only valid and active users can be selected (ticket 3208)
            SQL = "SELECT UserID AS value, Fullname AS name FROM UserMain WHERE status = 'True' ORDER BY value"
            iDVUsers = SQLBuildDV(SQL, True)

            'Fill comboboxes
            ddQueries._TextColumn = "name"
            ddQueries._SelectedValueColumn = "value"
            ddQueries.AddColumn("name")
            ddQueries.AddColumn("value", , "", False)
            populate_queries()
            'FillDDSQL(wstr, ddQueries, False, , True)
            FillDD("Don't Share=N,Share With Campus=C,Share With All=A", ddShared, , , True)

            iAllDays = "=,1=1,2=2,3=3,4=4,5=5,6=6,7=7,8=8,9=9,10=10,11=11,12=12,13=13,14=14,15=15,16=16,17=17,18=18,19=19,20=20," & _
                   "21=21,22=22,23=23,24=24,25=25,26=26,27=27,28=28,29=29,30=30,31=31"
            FillDD(iAllDays, ddDOM1, , , True)
            FillDD("=,January=1,February=2,March=3,April=4,May=5,June=6,July=7,August=8,September=9,October=10,November=11,December=12", ddMonth, , , True)
            FillDD(iAllDays, ddDOM2, , , True)

            'Show menu name
            txtMenuName.Text = iClassnamedescr

            'Default field only valid with screen functions
            chkDefaultQuery.SetWriter(iVersionType = "S")

            btnShowFiles.Visible = Mid(gUserName, 1, 3).ToLower = "qsi"

        Catch ex As Exception
            ShowError("Unable to open Save Query screen.", ex)
            Post("Close")
        End Try

    End Sub

    'Load form from database
    Private Sub feSaveQuery_OnLoadForm() Handles Me.OnLoadForm
        Dim DV As DataView
        Dim newrow As DataRow
        Dim okaytoretrieveversion As Boolean = True
        Dim btnname, wstr, wstr2 As String
        Dim ii As Integer

        Try
            '...first, we see if the passed version belongs to this user or another owner
            'NOTE: Also, if there is no passed version, then we won't try to retrieve one!
            If iOriginalRecno > 0 Then
                ' GBV 4/21/2015 - Ticket 3666 - moved all tQBE_xxx tables to IASCommon database
                If CInt(SQLGetNumber("Select Count(*) From IASCommon..tQBE_Version Where QBERecno = " & iOriginalRecno.ToString & _
                                     " And Savedby = '" & gUserName & "'")) = 0 Then
                    iOriginalRecno = 0
                End If
            End If
            If iOriginalRecno = 0 Then okaytoretrieveversion = False
            '...now retrieve version
            If okaytoretrieveversion = True Then
                wstr = "Select * From tQBE_Version Where QBERecno = " & iOriginalRecno.ToString
            Else
                wstr = "Select * From tQBE_Version Where 1=2"
            End If
            ' GBV 4/21/2015 - Ticket 3666 - moved all tQBE_xxx tables to IASCommon database
            iDAtQBEversion = BuildDA(wstr, iDS, "tQBE_Version", iSQLCn, , , , "IASCommon")

            StartDirtyTracking(iDS.Tables("tQBE_Version"), itQBEversion)

            '...if we found the default, display fields
            fill_query_fields()

            '...retrieve CC users
            ' GBV 4/21/2015 - Ticket 3666 - moved all tQBE_xxx tables to IASCommon database
            wstr = "Select tcc.*, UM.Fullname From tQBE_CCUser tcc Left Outer Join SQLPTS..UserMain UM On UM.UserID = tcc.CCUser " & _
                "Where tcc.QBERecno = " & iOriginalRecno.ToString & " Order By tcc.CCUser"
            wstr2 = "Select tcc.* From tQBE_CCUser tcc Where tcc.QBERecno = " & iOriginalRecno.ToString & " Order By tcc.CCUser"
            iDAtQBEcc = BuildDA(wstr, iDS, "tCCUser", iSQLCn, wstr2, , , "IASCommon")
            iBStQBEcc.DataSource = iDS
            iBStQBEcc.DataMember = "tCCUser"

            StartDirtyTracking(iDS.Tables("tCCUser"), itQBEcc)

            '...do setup of DataRepeaters
            drCCUsers.Setup(itQBEcc, Me, False, False)

            '...build table of available results
            If iVersionType = "S" Then
                '...add fields to table
                itQBEresults.Columns.Add("sel", GetType(Boolean))
                itQBEresults.Columns.Add("btnName", GetType(String))
                itQBEresults.Columns.Add("title", GetType(String))
                itQBEresults.Columns.Add("origsel", GetType(Boolean))
                itQBEresults.Rows.Clear()
                '...grab possible available buttons from screen controls
                For Each C As Control In iQBEControl.Controls
                    If C.Name = "btnExport1" Or C.Name = "btnExport2" Or C.Name = "btnExport3" Or C.Name = "btnExport4" Or _
                        C.Name = "btnReport" Or C.Name = "btnReport2" Or C.Name = "btnReport3" Or C.Name = "btnReport4" Then
                        If C.Tag.ToString.ToLower.Trim = "active" Then
                            newrow = itQBEresults.NewRow
                            newrow.Item("btnname") = C.Name
                            newrow.Item("title") = C.Text
                            If C.Name = "btnExport1" Or C.Name = "btnExport2" Or C.Name = "btnExport3" Or C.Name = "btnExport4" Then
                                newrow.Item("title") = C.Text & " Grid"
                            End If
                            If newrow.Item("title").ToString = "Export Grid" Then newrow.Item("title") = "Result Grid"
                            itQBEresults.Rows.Add(newrow)
                        End If
                    End If
                Next
                '...initialize grid rows
                init_availgrid()
                '...now pull in records from table to mark selected buttons
                ii = 0
                ' GBV 4/21/2015 - Ticket 3666 - moved all tQBE_xxx tables to IASCommon database
                DV = BuildDV("Select btnname From IASCommon..tQBE_AvailResults Where QBERecno = " & iOriginalRecno.ToString, False)
                If DV.Count > 0 Then
                    For Each R As DataRow In DV.Table.Rows
                        btnname = R.Item("btnname").ToString
                        For Each R2 As DataRow In itQBEresults.Rows
                            If R2.Item("btnname").ToString = btnname Then
                                R2.Item("sel") = True
                                R2.Item("origsel") = True
                                ii = ii + 1
                            End If
                        Next
                    Next
                End If
                '...assign to gridview
                gvResults.AutoGenerateColumns = False
                gvResults.DataSource = itQBEresults.DefaultView
            End If

            ''...can't delete "Last Run" version
            'btnDelete.Visible = (itQBEversion.Rows.Count = 1 AndAlso itQBEversion.Rows(0).Item("Title").ToString <> "Last Run")
            '...initialize current recno as original recno
            iCurrentRecno = iOriginalRecno

            SendKeys.Send("%") 'Need this to get the underlines to show up on the buttons for some reason

        Catch ex As Exception
            ShowError("Unable to load form from database.", ex)
            Post("Close")
        End Try

    End Sub

    Private Sub feSaveQuery_OnEndOfLoad() Handles Me.OnEndOfLoad
        Try
            protect_fields()
            ddQueries.SetWriter(itQBEversion.Rows(0).Item("Descr").ToString > "")
        Catch ex As Exception
            ddQueries.SetWriter(False)
        End Try
        '...<Save> button is always visible
        btnSave.Visible = True
        '...email fields may not be visible
        cbEmailResults.Visible = iAllowEmails
        If iAllowEmails = False Then cbEmailResults.Checked = False
        '...make sure fields are available if entering "New" query
        If rbNew.Checked = True Then
            txtTitle.SetWriter(True)
            txtDescr.SetWriter(True)
            ddShared.SetWriter(True)
            cbEmailResults.SetWriter(iAllowEmails = True)
            chkDefaultQuery.SetWriter(iVersionType = "S")
            btnDelete.Visible = False
        End If
    End Sub

    Private Sub ddQueries_SelectedIndexChanged(sender As Object, ByRef e As DataGridViewRowEventArgs) Handles ddQueries.SelectedIndexChanged
        Dim wstr, wstr2 As String

        iIsLoading = True
        'If query is selected, retrieve it
        If Not IsDBNull(ddQueries.SelectedValue) AndAlso ddQueries.SelectedValue IsNot Nothing AndAlso _
            ddQueries.SelectedValue.ToString > "" Then
            '...clear table
            itQBEversion.Clear()
            '...retrieve record from file
            ' GBV 4/21/2015 - Ticket 3666 - moved all tQBE_xxx tables to IASCommon database
            wstr = "Select * From tQBE_Version Where QBERecno = " & ddQueries.SelectedValue.ToString
            iDAtQBEversion = BuildDA(wstr, iDS, "tQBE_Version", iSQLCn, , , , "IASCommon")
            '...start dirty tracking
            StartDirtyTracking(iDS.Tables("tQBE_Version"), itQBEversion)
            '...display fields
            fill_query_fields()
            '...change current query recno to what the user selected
            iCurrentRecno = CInt(ddQueries.SelectedValue.ToString)
            '...can't delete "Last Run" version
            btnDelete.Visible = (itQBEversion.Rows.Count = 1 AndAlso itQBEversion.Rows(0).Item("Title").ToString <> "Last Run")

            '...retrieve CC users
            ' GBV 4/21/2015 - Ticket 3666 - moved all tQBE_xxx tables to IASCommon database
            wstr = "Select tcc.*, UM.Fullname From tQBE_CCUser tcc Left Outer Join SQLPTS..UserMain UM On UM.UserID = tcc.CCUser " & _
                "Where tcc.QBERecno = " & ddQueries.SelectedValue.ToString & " Order By tcc.CCUser"
            wstr2 = "Select tcc.* From tQBE_CCUser tcc Where tcc.QBERecno = " & ddQueries.SelectedValue.ToString & " Order By tcc.CCUser"
            iDAtQBEcc = BuildDA(wstr, iDS, "tCCUser", iSQLCn, wstr2, , , "IASCommon")
            iBStQBEcc.DataSource = iDS
            iBStQBEcc.DataMember = "tCCUser"

            StartDirtyTracking(iDS.Tables("tCCUser"), itQBEcc)

            '...do setup of DataRepeaters
            drCCUsers.Setup(itQBEcc, Me, False, False)
        Else
            txtTitle.Text = ""
            txtDescr.Text = ""
            ddShared.SelectedValue = "N"
            cbEmailResults.Checked = False
            chkDefaultQuery.Checked = False
            '...change current query version
            iCurrentRecno = 0
        End If
        '...protect other fields if no version is chosen
        txtTitle.SetWriter(ddQueries.Text > "" AndAlso (iCurrentRecno = 0 OrElse iOriginalRecno = iCurrentRecno))
        txtDescr.SetWriter(ddQueries.Text > "")
        ddShared.SetWriter(ddQueries.Text > "")
        cbEmailResults.SetWriter(ddQueries.Text > "" AndAlso iAllowEmails = True)
        chkDefaultQuery.SetWriter(iVersionType = "S" AndAlso ddQueries.Text > "")
        iIsLoading = False
    End Sub

    Private Sub fill_query_fields()

        '...start with fresh screen
        pDaily.Visible = False : pWeekly.Visible = False : pMonthly.Visible = False : pYearly.Visible = False
        refresh_fields()
        cbEmailResults.Checked = False
        rbDaily.Checked = False
        rbWeekly.Checked = False
        rbMonthly.Checked = False
        rbYearly.Checked = False
        gbRecurrence.Visible = False
        lblccusers.Visible = False
        lblCol1.Visible = False
        lblCol2.Visible = False
        btnInsertCC.Visible = False
        btnRemoveCC.Visible = False
        drCCUsers.Visible = False
        lblScreenResults.Visible = False
        gvResults.Visible = False
        lblReportResults.Visible = False
        chkDefaultQuery.Checked = False

        '...start new query record if user is trying to save "Last Run" version
        If itQBEversion.Rows.Count = 1 AndAlso itQBEversion.Rows(0).Item("Title").ToString <> "Last Run" Then
            ddQueries.SelectedValue = itQBEversion.Rows(0).Item("QBERecno").ToString
            txtTitle.Text = itQBEversion.Rows(0).Item("title").ToString
            txtDescr.Text = itQBEversion.Rows(0).Item("descr").ToString
            If Not IsDBNull(itQBEversion.Rows(0).Item("shared")) Then
                ddShared.SelectedValue = itQBEversion.Rows(0).Item("shared").ToString
            Else
                ddShared.SelectedValue = "N"
            End If
            '...Recurrence pattern
            If IsDBNull(itQBEversion.Rows(0).Item("emailnotify")) OrElse itQBEversion.Rows(0).Item("emailnotify").ToString = "N" OrElse _
                iAllowEmails = False Then
                '...nothing to be done
            Else
                cbEmailResults.Checked = True
                '...show Recurrence and CC Users sections
                gbRecurrence.Visible = True
                lblccusers.Visible = True
                lblCol1.Visible = True
                lblCol2.Visible = True
                btnInsertCC.Visible = True
                btnRemoveCC.Visible = True
                drCCUsers.Visible = True
                Select Case itQBEversion.Rows(0).Item("emailnotify").ToString
                    Case "D"
                        rbDaily.Checked = True
                        pDaily.Visible = True
                    Case "W"
                        rbWeekly.Checked = True
                        pWeekly.Visible = True
                        cbSunday.Checked = (Mid(itQBEversion.Rows(0).Item("DaysOfWeek").ToString, 1, 1) = "Y")
                        cbMonday.Checked = (Mid(itQBEversion.Rows(0).Item("DaysOfWeek").ToString, 2, 1) = "Y")
                        cbTuesday.Checked = (Mid(itQBEversion.Rows(0).Item("DaysOfWeek").ToString, 3, 1) = "Y")
                        cbWednesday.Checked = (Mid(itQBEversion.Rows(0).Item("DaysOfWeek").ToString, 4, 1) = "Y")
                        cbThursday.Checked = (Mid(itQBEversion.Rows(0).Item("DaysOfWeek").ToString, 5, 1) = "Y")
                        cbFriday.Checked = (Mid(itQBEversion.Rows(0).Item("DaysOfWeek").ToString, 6, 1) = "Y")
                        cbSaturday.Checked = (Mid(itQBEversion.Rows(0).Item("DaysOfWeek").ToString, 7, 1) = "Y")
                        If cbSunday.Checked = True And cbMonday.Checked = True And cbTuesday.Checked = True And cbWednesday.Checked = True And _
                            cbThursday.Checked = True And cbFriday.Checked = True And cbSaturday.Checked = True Then
                            cbAllDays.Checked = True
                        End If
                    Case "M"
                        rbMonthly.Checked = True
                        pMonthly.Visible = True
                        ddDOM1.SelectedValue = CInt(itQBEversion.Rows(0).Item("DayOfMonth"))
                    Case "Y"
                        rbYearly.Checked = True
                        pYearly.Visible = True
                        ddMonth.SelectedValue = CInt(itQBEversion.Rows(0).Item("Mth"))
                        ddDOM2.SelectedValue = CInt(itQBEversion.Rows(0).Item("DayOfMonth"))
                End Select
                '...show Available Results only for screens
                lblScreenResults.Visible = (iVersionType = "S" And ReadyForResultsGrid)
                gvResults.Visible = (iVersionType = "S" And ReadyForResultsGrid)
                lblReportResults.Visible = (iVersionType = "R" And ReadyForResultsGrid)
            End If
            If Not IsDBNull(itQBEversion.Rows(0).Item("defaultquery")) AndAlso CBool(itQBEversion.Rows(0).Item("defaultquery")) = True Then
                chkDefaultQuery.Checked = True
            End If
            rbUpdate.Checked = True
        Else
            rbNew.Checked = True
        End If
    End Sub

#End Region

#Region "---------------------------- Field Handling ---------------------------------"

    'Default fields for a a new Query
    Private Sub rbNew_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rbNew.CheckedChanged
        If rbNew.Checked = True Then
            ddQueries.Text = ""
            txtTitle.Text = ""
            txtDescr.Text = ""
            ddShared.SelectedValue = "N"
            cbEmailResults.Checked = False
            chkDefaultQuery.Checked = False
            '...add record to table
            itQBEversion.Clear()
            Dim R As DataRow = itQBEversion.NewRow
            '...fields will be added in OnValidateForm
            itQBEversion.Rows.Add(R)
            '...reset current version title
            iCurrentRecno = 0
            txtTitle.SetWriter(True)
            '...protect fields
            protect_fields()
            '...focus on Title
            txtTitle.Focus()
        End If
    End Sub

    Private Sub rbUpdate_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rbUpdate.CheckedChanged
        protect_fields()
    End Sub

    Private Sub cbEmailResults_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles cbEmailResults.CheckedChanged
        Dim i As Integer = 0

        If iIsLoading = True Then Return
        '...only process if first time through, in case this event changes the cbEmailResults checkbox again
        If iIteration = 2 Then
            iIteration = 1
            Return
        End If
        '...if unchecking box and we have any records in tQBE_CCUser, then give warning
        If cbEmailResults.Checked = False AndAlso itQBEcc.Rows.Count > 0 Then
            '...only delete existing records if not starting new version
            If rbNew.Checked = False Then
                '...see if any have been saved
                For Each R As DataRow In itQBEcc.Rows
                    If R.RowState <> DataRowState.Added Then i = i + 1
                Next
                '...prompt user if we need to delete records
                If i > 0 AndAlso _
                    MsgBoxQuestion("The existing CC User records will be deleted immediately for this saved query.  Continue?", , "Saved Query") <> MsgBoxResult.Yes Then
                    iIteration = 2
                    cbEmailResults.Checked = True
                    Return
                End If
                '...remove tQBE_CCUser records
                DoSQL("Delete From IASCommon..tQBE_CCUser Where QBERecno = " & iCurrentRecno.ToString)
            End If
            '...clear memory table
            itQBEcc.Clear()
        End If
        gbRecurrence.Visible = cbEmailResults.Checked
        '...start fresh
        rbDaily.Checked = False
        rbWeekly.Checked = False
        rbMonthly.Checked = False
        rbYearly.Checked = False
        lblccusers.Visible = cbEmailResults.Checked
        lblCol1.Visible = cbEmailResults.Checked
        lblCol2.Visible = cbEmailResults.Checked
        btnInsertCC.Visible = cbEmailResults.Checked
        btnRemoveCC.Visible = cbEmailResults.Checked
        drCCUsers.Visible = cbEmailResults.Checked
        '...initialize Available Results grid rows
        init_availgrid()
        '...show Available Results only for screens
        lblScreenResults.Visible = (iVersionType = "S" AndAlso cbEmailResults.Checked = True And ReadyForResultsGrid)
        gvResults.Visible = (iVersionType = "S" AndAlso cbEmailResults.Checked = True And ReadyForResultsGrid)
        lblReportResults.Visible = (iVersionType = "R" AndAlso cbEmailResults.Checked = True And ReadyForResultsGrid)
    End Sub

    Private Sub rbDaily_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rbDaily.CheckedChanged
        If iIsLoading = True Then Return
        pDaily.Visible = rbDaily.Checked
        If rbDaily.Checked = True Then pWeekly.Visible = False
        If rbDaily.Checked = True Then pMonthly.Visible = False
        If rbDaily.Checked = True Then pYearly.Visible = False
        refresh_fields()
    End Sub

    Private Sub rbWeekly_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rbWeekly.CheckedChanged
        If iIsLoading = True Then Return
        pWeekly.Visible = rbWeekly.Checked
        If rbWeekly.Checked = True Then pDaily.Visible = False
        If rbWeekly.Checked = True Then pMonthly.Visible = False
        If rbWeekly.Checked = True Then pYearly.Visible = False
        refresh_fields()
    End Sub

    Private Sub rbMonthly_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rbMonthly.CheckedChanged
        If iIsLoading = True Then Return
        pMonthly.Visible = rbMonthly.Checked
        If rbMonthly.Checked = True Then pDaily.Visible = False
        If rbMonthly.Checked = True Then pWeekly.Visible = False
        If rbMonthly.Checked = True Then pYearly.Visible = False
        refresh_fields()
    End Sub

    Private Sub rbYearly_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rbYearly.CheckedChanged
        If iIsLoading = True Then Return
        pYearly.Visible = rbYearly.Checked
        If rbYearly.Checked = True Then pDaily.Visible = False
        If rbYearly.Checked = True Then pWeekly.Visible = False
        If rbYearly.Checked = True Then pMonthly.Visible = False
        refresh_fields()
    End Sub

    'Refresh all fields
    Private Sub refresh_fields()
        cbSunday.Checked = False
        cbMonday.Checked = False
        cbTuesday.Checked = False
        cbWednesday.Checked = False
        cbThursday.Checked = False
        cbFriday.Checked = False
        cbSaturday.Checked = False
        cbAllDays.Checked = False
        ddDOM1.SelectedValue = ""
        ddMonth.SelectedValue = ""
        ddDOM2.SelectedValue = ""
    End Sub

    Private Sub cbSunday_CheckedChanged(sender As Object, e As System.EventArgs) Handles cbSunday.CheckedChanged
        If iIsLoading = True Then Return
        If cbSunday.Checked = False Then
            iDayCheckbox = True
            cbAllDays.Checked = False
            iDayCheckbox = False
        End If
    End Sub

    Private Sub cbMonday_CheckedChanged(sender As Object, e As System.EventArgs) Handles cbMonday.CheckedChanged
        If iIsLoading = True Then Return
        If cbMonday.Checked = False Then
            iDayCheckbox = True
            cbAllDays.Checked = False
            iDayCheckbox = False
        End If
    End Sub

    Private Sub cbTuesday_CheckedChanged(sender As Object, e As System.EventArgs) Handles cbTuesday.CheckedChanged
        If iIsLoading = True Then Return
        If cbTuesday.Checked = False Then
            iDayCheckbox = True
            cbAllDays.Checked = False
            iDayCheckbox = False
        End If
    End Sub

    Private Sub cbWednesday_CheckedChanged(sender As Object, e As System.EventArgs) Handles cbWednesday.CheckedChanged
        If iIsLoading = True Then Return
        If cbWednesday.Checked = False Then
            iDayCheckbox = True
            cbAllDays.Checked = False
            iDayCheckbox = False
        End If
    End Sub

    Private Sub cbThursday_CheckedChanged(sender As Object, e As System.EventArgs) Handles cbThursday.CheckedChanged
        If iIsLoading = True Then Return
        If cbThursday.Checked = False Then
            iDayCheckbox = True
            cbAllDays.Checked = False
            iDayCheckbox = False
        End If
    End Sub

    Private Sub cbFriday_CheckedChanged(sender As Object, e As System.EventArgs) Handles cbFriday.CheckedChanged
        If iIsLoading = True Then Return
        If cbFriday.Checked = False Then
            iDayCheckbox = True
            cbAllDays.Checked = False
            iDayCheckbox = False
        End If
    End Sub

    Private Sub cbSaturday_CheckedChanged(sender As Object, e As System.EventArgs) Handles cbSaturday.CheckedChanged
        If iIsLoading = True Then Return
        If cbSaturday.Checked = False Then
            iDayCheckbox = True
            cbAllDays.Checked = False
            iDayCheckbox = False
        End If
    End Sub

    Private Sub cbAllDays_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles cbAllDays.CheckedChanged
        If iIsLoading = True Then Return
        If iDayCheckbox = False Then
            cbSunday.Checked = cbAllDays.Checked
            cbMonday.Checked = cbAllDays.Checked
            cbTuesday.Checked = cbAllDays.Checked
            cbWednesday.Checked = cbAllDays.Checked
            cbThursday.Checked = cbAllDays.Checked
            cbFriday.Checked = cbAllDays.Checked
            cbSaturday.Checked = cbAllDays.Checked
        End If
    End Sub

    Private Sub ddMonth_onDropDownClosed() Handles ddMonth.onDropDownClosed
        If ddMonth.Text = "" Then
            ddDOM2.SelectedValue = ""
        Else
            '...adjust day of month if user chose a different month
            If CInt(ddMonth.SelectedValue) = 2 AndAlso ddDOM2.Text > "" AndAlso CInt(ddDOM2.SelectedValue) > 28 Then
                ddDOM2.SelectedValue = 28
            End If
            If (CInt(ddMonth.SelectedValue) = 4 Or CInt(ddMonth.SelectedValue) = 6 Or CInt(ddMonth.SelectedValue) = 9 Or _
                CInt(ddMonth.SelectedValue) = 11) AndAlso ddDOM2.Text = "31" Then
                ddDOM2.SelectedValue = 30
            End If
        End If
    End Sub

    Private Sub ddDOM2_onDropDown(Sender As Object, ByRef aOK As Boolean) Handles ddDOM2.onDropDown
        Dim thismonthdays As String = iAllDays

        If ddMonth.Text = "" Then
            MsgBoxInfo("Month must be entered before selecting day of month.", , "Save Query")
            thismonthdays = "="
        Else
            '...28 days in February
            If CInt(ddMonth.SelectedValue) = 2 Then
                thismonthdays = "=,1=1,2=2,3=3,4=4,5=5,6=6,7=7,8=8,9=9,10=10,11=11,12=12,13=13,14=14,15=15,16=16,17=17,18=18,19=19,20=20," & _
                   "21=21,22=22,23=23,24=24,25=25,26=26,27=27,28=28"
            End If
            '...30 days in April, June, September, November
            If CInt(ddMonth.SelectedValue) = 4 Or CInt(ddMonth.SelectedValue) = 6 Or CInt(ddMonth.SelectedValue) = 9 Or CInt(ddMonth.SelectedValue) = 11 Then
                thismonthdays = "=,1=1,2=2,3=3,4=4,5=5,6=6,7=7,8=8,9=9,10=10,11=11,12=12,13=13,14=14,15=15,16=16,17=17,18=18,19=19,20=20," & _
                   "21=21,22=22,23=23,24=24,25=25,26=26,27=27,28=28,29=29,30=30"
            End If
        End If
        FillDD(thismonthdays, ddDOM2, , , True)
    End Sub

    Private Sub protect_fields()
        If rbNew.Checked = True Then
            ddQueries.SetWriter(False)
            txtTitle.SetWriter(True)
            txtDescr.SetWriter(True)
            ddShared.SetWriter(True)
            cbEmailResults.SetWriter(iAllowEmails)
            chkDefaultQuery.SetWriter(iVersionType = "S")
            '...can't delete this version until saved
            btnDelete.Visible = False
        End If
        If rbUpdate.Checked = True Then
            ddQueries.SetWriter(True)
            '...protect other fields if no version is chosen
            txtTitle.SetWriter(ddQueries.Text > "" AndAlso (iCurrentRecno = 0 OrElse iOriginalRecno = iCurrentRecno))
            txtDescr.SetWriter(ddQueries.Text > "")
            ddShared.SetWriter(ddQueries.Text > "")
            cbEmailResults.SetWriter(ddQueries.Text > "" AndAlso iAllowEmails)
            chkDefaultQuery.SetWriter(iVersionType = "S" AndAlso ddQueries.Text > "")
            '...make <Delete> button visible if we're displaying a version
            btnDelete.Visible = (ddQueries.Text > "" AndAlso ddQueries.Text <> "Last Run")
        End If
    End Sub

    'Data Entry Edit Checks
    Private Sub feSaveQuery_OnValidateControl(ByVal aColName As String, ByRef aValue As String, ByVal aRow As Integer, ByRef aErrorText As String) Handles Me.OnValidateControl
        Try
            Select Case aColName.ToLower
                Case "txttitle"
                    If aValue = "Last Run" Then
                        aErrorText = "Please use something other than 'Last Run' for a title."
                        Return
                    End If
                Case "dddom1"
                    If aValue = "31" AndAlso aValue <> iOrigValue Then
                        If MsgBox("Some months have fewer than 31 days.  For these months, the occurrence will fall on the last day of the month.", _
                                    MsgBoxStyle.OkCancel, "Save Query") <> MsgBoxResult.Ok Then
                            ddDOM1.SelectedValue = iOrigValue
                            aValue = iOrigValue
                        End If
                    End If
                Case "ddccuser"
                    If aValue <> "" Then
                        If drCCUsers.CurrentItemIndex >= 0 Then
                            Dim UserName As qTextBox = CType(drCCUsers.CurrentItem.Controls("txtUserName"), qTextBox)
                            UserName.Text = GetFullName(aValue)
                        End If
                    End If
            End Select
        Catch ex As Exception
            ShowError("Validation failed for " & aColName, ex)
        End Try

    End Sub

    'Project checks just before saving
    Private Sub feSaveQuery_OnValidateForm(ByRef aSender As Object, ByRef aErrorText As String) Handles Me.OnValidateForm
        Dim BS As BindingSource
        Dim wstr As String
        Dim ii As Integer

        'Commit changes
        For Each BS In iBSs
            BS.EndEdit()
        Next

        'Field relationship edits
        If iOriginalRecno = 0 AndAlso iCurrentRecno > 0 Then
            If iVersionType = "R" Then
                aErrorText = "To change an existing version, you must first load it on the report screen, using <Load Query>.  " & _
                   "You may check New Saved Query to save a new version now."
            Else
                aErrorText = "To change an existing version, you must first load it on the list screen, using <Load Query>.  " & _
                    "You may check New Saved Query to save a new version now."
            End If
            aSender = rbNew
            Return
        End If

        Dim cleanstr As String = PrepareSQLSearchString(txtTitle.Text)
        If cleanstr.Length > 60 Then
            aErrorText = "Title can't exceed 60 characters--version not saved."
            aSender = txtTitle
            Return
        End If
        cleanstr = PrepareSQLSearchString(txtDescr.Text)
        If cleanstr.Length > 2000 Then
            aErrorText = "Description can't exceed 2000 characters--version not saved."
            aSender = txtDescr
            Return
        End If

        If rbWeekly.Checked = True Then
            If cbSunday.Checked = True OrElse cbMonday.Checked = True OrElse cbTuesday.Checked = True OrElse cbWednesday.Checked = True OrElse _
                cbThursday.Checked = True OrElse cbFriday.Checked = True OrElse cbSaturday.Checked = True Then
            Else
                aErrorText = "For a weekly recurrence schedule, at least one day must be checked."
                aSender = cbSunday
                Return
            End If
        End If
        If rbMonthly.Checked = True AndAlso ddDOM1.Text = "" Then
            aErrorText = "For a monthly recurrence schedule, you must select the day of the month."
            aSender = ddDOM1
            Return
        End If
        If rbYearly.Checked = True Then
            If ddMonth.Text = "" Then
                aErrorText = "For a yearly recurrence schedule, you must select a month."
                aSender = ddMonth
                Return
            End If
            If ddDOM2.Text = "" Then
                aErrorText = "For a yearly recurrence schedule, you must select the day of the month."
                aSender = ddDOM2
                Return
            End If
        End If

        If cbEmailResults.Checked = True AndAlso iVersionType = "S" Then
            ii = 0
            For Each R As DataRow In itQBEresults.Rows
                If CBool(R.Item("sel")) = True Then ii = ii + 1
            Next
            If ii = 0 Then
                aErrorText = "At least one Available Result must be checked to be emailed."
                aSender = gvResults
                Return
            End If
        End If

        'Windows control edit checks

        'Clean up blank users in qDR and set key fields
Do_Over2:
        Dim counter As Integer = 0
        If itQBEcc.Rows.Count > 0 Then
            For Each r As DataRow In itQBEcc.Rows
                counter = counter - 1
                If RowIsDeleted(r) Then Continue For
                If (IsDBNull(r.Item("CCUser")) OrElse r.Item("CCUser").ToString = "") Then
                    r.Delete()
                    GoTo Do_Over2
                Else
                    If r.Item("CCRecno").ToString = "0" Then r.Item("CCRecno") = counter
                End If
            Next
        End If

        '++CON ddCCUser Cannot add 2 records with the same user.
        If drCCUsers.ItemCount > 1 Then
            For i As Integer = 0 To drCCUsers.ItemCount - 1
                drCCUsers.CurrentItemIndex = i
                If drCCUsers.CurrentItem Is Nothing Then Continue For

                Dim CCUser As qDD = CType(drCCUsers.CurrentItem.Controls("ddCCUser"), qDD)

                Dim UserName As String = GetFullName(CCUser.Text)
                If UserName = "" Then
                    aErrorText = "Invalid user ID."
                    aSender = CCUser
                    Return
                End If

                If i > drCCUsers.ItemCount - 2 Then Exit For

                For j As Integer = i + 1 To drCCUsers.ItemCount - 1
                    drCCUsers.CurrentItemIndex = j
                    If drCCUsers.CurrentItem Is Nothing Then Continue For

                    Dim CCUser2 As qDD = CType(drCCUsers.CurrentItem.Controls("ddCCUser"), qDD)

                    '...check for duplicate now
                    If CCUser.Text = CCUser2.Text Then
                        aErrorText = "Cannot add 2 records with the same user."
                        ShowControlError(CCUser2, aErrorText)
                        aSender = CType(CCUser2, qDD)
                        drCCUsers.Visible = True
                        Return
                    End If
                Next
            Next
        End If

        'Commit changes
        For Each BS In iBSs
            BS.EndEdit()
        Next

        'Fill itQBEversion fields from screen
        With itQBEversion.Rows(0)
            .Item("ClassName") = iClassname
            .Item("Title") = txtTitle.Text
            .Item("SavedBy") = gUserName
            .Item("SortNo") = 1
            .Item("Descr") = txtDescr.Text
            .Item("Shared") = ddShared.SelectedValue.ToString
            .Item("DefaultQuery") = "False"
            If chkDefaultQuery.Checked = True Then .Item("DefaultQuery") = "True"
            .Item("DaysOfWeek") = Convert.DBNull
            .Item("Mth") = Convert.DBNull
            .Item("DayOfMonth") = Convert.DBNull
            If cbEmailResults.Checked = True Then
                If rbDaily.Checked = True Then
                    .Item("EmailNotify") = "D"
                    .Item("DaysOfWeek") = "-YYYYY-"
                End If
                If rbWeekly.Checked = True Then
                    .Item("EmailNotify") = "W"
                    wstr = ""
                    If cbSunday.Checked = True Then wstr &= "Y" Else wstr &= "-"
                    If cbMonday.Checked = True Then wstr &= "Y" Else wstr &= "-"
                    If cbTuesday.Checked = True Then wstr &= "Y" Else wstr &= "-"
                    If cbWednesday.Checked = True Then wstr &= "Y" Else wstr &= "-"
                    If cbThursday.Checked = True Then wstr &= "Y" Else wstr &= "-"
                    If cbFriday.Checked = True Then wstr &= "Y" Else wstr &= "-"
                    If cbSaturday.Checked = True Then wstr &= "Y" Else wstr &= "-"
                    .Item("DaysOfWeek") = wstr
                End If
                If rbMonthly.Checked = True Then
                    .Item("EmailNotify") = "M"
                    .Item("DayOfMonth") = CInt(ddDOM1.SelectedValue)
                End If
                If rbYearly.Checked = True Then
                    .Item("EmailNotify") = "Y"
                    .Item("Mth") = CInt(ddMonth.SelectedValue)
                    .Item("DayOfMonth") = CInt(ddDOM2.SelectedValue)
                End If
            Else
                .Item("EmailNotify") = "N"
            End If
            If rbNew.Checked = True Then
                .Item("CreatedBy") = gUserName
                .Item("CreatedDt") = Now
            Else
                .Item("ModifiedDt") = Now
            End If
        End With

        'Initialize CCUser record field so we don't throw an error before reaching the OnSaveForm event
        For Each R As DataRow In itQBEcc.Rows
            If RowIsDeleted(R) Then Continue For
            R.Item("QBERecno") = itQBEversion.Rows(0).Item("QBERecno")
        Next

        'ShowTable(iDS)

    End Sub

#End Region

#Region "---------------------------- OnSave, OnDelete -------------------------------"

    Private Sub Me_OnSaveForm(ByRef aOK As Boolean) Handles Me.OnSaveForm
        Dim currentrecno As Integer = iCurrentRecno
        Dim SQLs As New ArrayList
        Dim wstr As String
        Dim i As Integer = 0

        aOK = False
        Try
            'Save tQBE_Version table (while creating connection and transaction)
            If SaveTable(iDAtQBEversion, iDS.Tables("tQBE_Version"), "M") Then
                '...remove old QBE criteria for original title if it's the same as current title
                If iOriginalRecno > 0 AndAlso iOriginalRecno = iCurrentRecno Then
                    SQLDoSQL("Delete from IASCommon..tQBE_Columns Where QBERecno = " & iOriginalRecno.ToString, True)    'Force write to Live
                End If
                '...save QBE criteria in tQBE_Columns if new query or same query as original
                'NOTE: iCurrentRecno gets set with call to SaveTable2, so we use the local currentrecno value
                If currentrecno = 0 OrElse (currentrecno > 0 AndAlso iOriginalRecno = currentrecno) Then
                    ControlToQBEColumns(iQBEControl, iCurrentRecno)
                End If
                '...fill in QBERecno on tQBE_CCUser records
                For Each R As DataRow In itQBEcc.Rows
                    If RowIsDeleted(R) Then Continue For
                    R.Item("QBERecno") = iCurrentRecno
                Next
                '...save tQBE_CCUser records
                If SaveTable(iDAtQBEcc, iDS.Tables("tCCUser"), "D") = False Then
                    Return
                End If
                '...for a screen, remove available results and add any new ones
                If iVersionType = "S" Then
                    SQLs.Clear()
                    For Each R As DataRow In itQBEresults.Rows
                        If IsDBNull(R.Item("sel")) OrElse CBool(R.Item("sel")) = False Then
                            '...should we delete from file?
                            If Not IsDBNull(R.Item("origsel")) AndAlso CBool(R.Item("origsel")) = True Then
                                wstr = "Delete From IASCommon..tQBE_AvailResults Where QBERecno = " & iCurrentRecno.ToString & _
                                    " And BtnName = '" & R.Item("btnname").ToString & "'"
                                SQLs.Add(wstr)
                                i = i + 1
                            End If
                        Else
                            '...should we add to file?
                            If IsDBNull(R.Item("origsel")) OrElse CBool(R.Item("origsel")) = False Then
                                wstr = "Insert Into IASCommon..tQBE_AvailResults (QBERecno, BtnName) Values (" & iCurrentRecno.ToString & _
                                    ",'" & R.Item("btnname").ToString & "')"
                                SQLs.Add(wstr)
                                i = i + 1
                            End If
                        End If
                    Next
                    If i > 0 Then
                        DoSQLTran(SQLs, iSQLCn, iSQLTran)
                        '...reset original select flags to be same as current select flags now, in case user saves again
                        For Each R As DataRow In itQBEresults.Rows
                            If IsDBNull(R.Item("sel")) OrElse CBool(R.Item("sel")) = False Then
                                R.Item("origsel") = False
                            Else
                                R.Item("origsel") = True
                            End If
                        Next
                    End If
                End If
                
            End If
            '...save version title
            If iOriginalRecno = 0 Then iOriginalRecno = iCurrentRecno
            aOK = True
        Catch ex As Exception
            ShowError("Changes to database record were not saved.", ex)
        End Try

    End Sub

    Private Sub iDATQBEVersion_RowUpdated(sender As Object, e As System.Data.SqlClient.SqlRowUpdatedEventArgs) Handles iDAtQBEversion.RowUpdated
        SetTableIdentity("tQBE_Version", "QBERecno", e, iSQLTran)
        '...save version record number
        iCurrentRecno = CInt(e.Row.Item("QBERecno"))
    End Sub

    Private Sub feSaveQuery_OnAfterSave() Handles Me.OnAfterSave
        Dim DV As DataView
        Dim wstr, wstr2 As String

        '...if this record is the default, clear default flag from other applicable records
        ' GBV 4/21/2015 - Ticket 3666 - moved all tQBE_xxx tables to IASCommon database
        If CBool(itQBEversion.Rows(0).Item("DefaultQuery")) = True Then
            wstr = "Update IASCommon..tQBE_Version Set DefaultQuery = 'False' Where Classname = '" & iClassname & "' And Savedby = '" & _
                gUserName & "' And QBERecno <> " & iCurrentRecno.ToString
            DoSQL(wstr)
        End If
        '...if New radio button is checked, switch to Update radio button and retrieve records again for dropdown
        If rbNew.Checked = True Then
            '...retrieve and fill query drop-down again
            ' GBV 4/21/2015 - Ticket 3666 - moved all tQBE_xxx tables to IASCommon database
            wstr = "Select Title name, QBERecno value, DefaultQuery From IASCommon..tQBE_Version Where Classname = '" & iClassname & _
             "' And SavedBy = '" & gUserName & "' And Title Not In ('Window Size','Report Zoom','Layout','Window Loc') Order By Title"
            DV = BuildDV(wstr, False)
            If DV.Count > 0 Then
                For Each R As DataRow In DV.Table.Rows
                    If Not IsDBNull(R.Item("defaultquery")) AndAlso CBool(R.Item("defaultquery")) = True Then
                        R.Item("name") = R.Item("name").ToString & " (Default)"
                    End If
                Next
            End If

            'Refill cc list because of issue with DMS 3666
            itQBEcc.Clear()
            wstr = "Select tcc.*, UM.Fullname From tQBE_CCUser tcc Left Outer Join SQLPTS..UserMain UM On UM.UserID = tcc.CCUser " & _
                "Where tcc.QBERecno = " & iOriginalRecno.ToString & " Order By tcc.CCUser"
            wstr2 = "Select tcc.* From tQBE_CCUser tcc Where tcc.QBERecno = " & iOriginalRecno.ToString & " Order By tcc.CCUser"
            iDAtQBEcc = BuildDA(wstr, iDS, "tCCUser", iSQLCn, wstr2, , , "IASCommon")
            iBStQBEcc.DataSource = iDS
            iBStQBEcc.DataMember = "tCCUser"

            ddQueries.DataSource = DV
            '...select correct record
            ddQueries.SelectedValue = iCurrentRecno
            '...switch radio buttons
            rbUpdate.Checked = True
            '...focus on first field
            txtTitle.Focus()
        End If

    End Sub

    'Set the deletion prompt
    Private Sub feSaveQuery_OKToDeletePrompt(ByRef aPromptText As String) Handles Me.OnOKToDeletePrompt
        aPromptText = "Are you SURE you want to delete this saved Query"
    End Sub

    'Delete
    Private Sub Me_OnDeleteForm(ByRef aOK As Boolean) Handles Me.OnDeleteForm
        Dim wstr As String

        'Delete Edit Rule Check
        aOK = False

        'Delete tQBE_Version record
        ' GBV 4/21/2015 - Ticket 3666 - moved all tQBE_xxx tables to IASCommon database
        Try
            wstr = "Delete From IASCommon..tQBE_Columns Where QBERecno = " & iCurrentRecno.ToString
            DoSQL(wstr)
            wstr = "Delete From IASCommon..tQBE_Version Where QBERecno = " & iCurrentRecno.ToString
            DoSQL(wstr)
            wstr = "Delete From IASCommon..tQBE_CCUser Where QBERecno = " & iCurrentRecno.ToString
            DoSQL(wstr)
            '...warn user if none of the existing versions are marked as defaults
            If CInt(SQLGetNumber("Select Count(*) From IASCommon..tQBE_Version Where Classname = '" & iClassname & "' And Savedby = '" & _
                        gUserName & "'")) > 0 Then
                If CInt(SQLGetNumber("Select Count(*) From IASCommon..tQBE_Version Where Classname = '" & iClassname & "' And Savedby = '" & _
                            gUserName & "' And DefaultQuery = 'True'")) = 0 Then
                    MsgBoxInfo("None of your remaining queries for this function are marked as the default query.", , "Save Query")
                End If
            End If
            '...switch to a new query
            rbNew.Checked = True
            '...re-populate queries dropdown
            populate_queries()
            '...get rid of <Delete> button now
            btnDelete.Visible = False
            '...initialize email fields
            cbEmailResults.Checked = False
            '...all done
            'NOTE: Don't return aOK = True unless the screen should be closed!
            'aOK = True
            ShowStatus("Deleted")
        Catch ex As Exception
            MsgBoxErr("Unable to delete Report Version record.")
            aOK = False
        End Try
    End Sub

#End Region

#Region "---------------------------- DataRepeater -----------------------------------"

    'Private Sub drCCUsers_ItemCloned(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.PowerPacks.DataRepeaterItemEventArgs) Handles drCCUsers.ItemCloned
    '    Dim holdtext As String
    '    Dim DV As DataView
    '    DV = iDVUsers.Table.Copy.DefaultView
    '    'Fill ddEscalationUser dropdown items
    '    Dim UserID As qDD = CType(e.DataRepeaterItem.Controls.Item("ddCCuser"), qDD)
    '    If UserID.RowCount = 0 Then    'Needs dropdown population filled in
    '        holdtext = UserID.Text  'Current .Text value
    '        If UserID.RowCount = 0 Then UserID.DataSource = DV
    '        If holdtext <> "" Then UserID.SelectedValue = holdtext
    '    End If
    'End Sub

#End Region

#Region "---------------------------- Buttons ----------------------------------------"

    Private Sub btnInsertCC_Click(sender As System.Object, e As System.EventArgs) Handles btnInsertCC.Click
        Try

            '...fill in user name because onvalidatecontrol doesn't get called from here
            If itQBEcc.Rows.Count > 0 Then
                For Each r As DataRow In itQBEcc.Rows
                    If RowIsDeleted(r) Then Continue For
                    If (IsDBNull(r.Item("CCUser")) OrElse r.Item("CCUser").ToString = "") Then Continue For
                    r.Item("fullname") = GetFullName(r.Item("CCUser").ToString)
                Next
            End If

            drCCUsers.InsertRow(False)
            For i = 1 To 100000
                Application.DoEvents()
            Next

            Dim CCUser As qDD = CType(drCCUsers.CurrentItem.Controls("ddCCUser"), qDD)
            CCUser.Focus()

        Catch ex As Exception
            ShowError("Error inserting CC User row.", ex)
        End Try
    End Sub

    Private Sub btnRemoveCC_Click(sender As System.Object, e As System.EventArgs) Handles btnRemoveCC.Click
        Try
            drCCUsers.RemoveRow()
        Catch ex As Exception
            ShowError("Error removing CC User row.", ex)
        End Try
    End Sub

    Private Sub btnShowFiles_Click(sender As System.Object, e As System.EventArgs) Handles btnShowFiles.Click
        ShowTable(iDS)
    End Sub

#End Region

#Region "---------------------------- Miscellaneous Routines -------------------------"

    'Retrieve a user's full name
    Private Function GetFullName(ByVal aUserID As String) As String
        Dim fullname As String = ""

        If aUserID > "" And iDVUsers.Count > 0 Then
            For Each R As DataRowView In iDVUsers
                If R.Item("Value").ToString = aUserID Then
                    fullname = R.Item("Name").ToString
                    Exit For
                End If
            Next
        End If
        Return fullname
    End Function

    Private Sub populate_queries()
        Dim DV As DataView
        Dim wstr As String
        ' GBV 4/21/2015 - Ticket 3666 - moved all tQBE_xxx tables to IASCommon database
        wstr = "Select Title name, QBERecno value, DefaultQuery From IASCommon..tQBE_Version Where Classname = '" & iClassname & _
            "' And SavedBy = '" & gUserName & "' And Title Not In ('Window Size','Report Zoom','Layout','Window Loc') Order By Title"
        DV = BuildDV(wstr, False)
        If DV.Count > 0 Then
            For Each R As DataRow In DV.Table.Rows
                If Not IsDBNull(R.Item("defaultquery")) AndAlso CBool(R.Item("defaultquery")) = True Then
                    R.Item("name") = R.Item("name").ToString & " (Default)"
                End If
            Next
        End If
        ddQueries.DataSource = DV

    End Sub

    Private Sub init_availgrid()
        For Each R As DataRow In itQBEresults.Rows
            R.Item("sel") = False
            R.Item("origsel") = False
        Next
        '...if there's only one row, mark it
        If itQBEresults.Rows.Count = 1 Then itQBEresults.Rows(0).Item("sel") = True
    End Sub

#End Region

End Class
