Namespace Windows.Forms
    'FIX - RUN BUTTON SHOULD RUN VALIDATE, AND THEN RUN ONRUN TO KICK OFF CLIENT LOGIC
    'Ancestor for Report and Update Parameters form.

    Public Class fpMainVersion
        Inherits fBase
        Private iSQLConn As New SqlClient.SqlConnection(Appl.gSQLConnStr)
        Private iFillVersionSQL As String
        Protected Event OnFillControlsFromVersion()      'New button clicked
        Protected Event OnRun(ByRef aFS As fsMain)     'Run button clicked
        Protected Event OnClearControl()

        'Is Resizable - default is false, but may be set to true for Master/Detail edit forms.
        Private _iIsResizable As Boolean
        Public Property iIsResizable() As Boolean
            Get
                Return _iIsResizable
            End Get
            Set(ByVal value As Boolean)
                _iIsResizable = value
                If _iIsResizable = True Then
                    FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
                    StatusStrip1.SizingGrip = True
                Else
                    FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
                    StatusStrip1.SizingGrip = False
                End If
            End Set
        End Property

#Region "---------------------------- Documentation ------------------------------"
        'Parameter Ancestor WinForm
        'BHS 11/20/07 Added Versions

#End Region

#Region "---------------------------- Load ------------------------------"

        Private Sub fpMainVersion_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
            If Me.WindowState = FormWindowState.Maximized Then
                Me.WindowState = FormWindowState.Normal
            End If
        End Sub
        'BHS 8/28/09 Recursively set all textbox.maxlength to maximum 200
        Sub LimitTextLength(ByVal aC As Control)
            'BHS 8/28/09
            If aC.Name.ToLower = "tabpage2" Or aC.Name.ToLower = "txtinvc" Then
                Dim j As Integer = 1
            End If
            Dim qT As TextBox = TryCast(aC, TextBox)
            If qT IsNot Nothing Then
                If qT.MaxLength > 200 Then qT.MaxLength = 200
            End If
            For Each C As Control In aC.Controls
                LimitTextLength(C)
            Next
        End Sub

        '''<summary> Set Color Scheme, Set Validation Handles, Load Version dropdown and control defaults
        ''' Note, descendant runs a Load event also - order of events is not dependable </summary>
        Private Sub fpMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                iIsLoading = True

                iIsResizable = False

                iFillVersionSQL = "Select title Name, title Value, sortno From tRpt Where classname = '" & _
                      GetObName() & "' And SavedBy = '" & gUserName & _
                      "' AND title <> 'Window Size' Union Select '<default>', '<default>', 2 " & _
                      " Order By sortno"
                Dim toolTip1 As New ToolTip()
                SetToolTipProperties(toolTip1)
                toolTip1.SetToolTip(Me.btnRun, "Run the report, based on what you've entered")

                For Each C As Control In Me.Controls
                    AssignToolTips(toolTip1, C)    'Call recursive routine
                    LimitTextLength(C)
                Next

                SetHandles()

                SetColorScheme()    'Default fore and backcolors
                StatusStrip1.BackColor = QBackColor

                'Load cbVersion
                'If fBase.ActiveForm IsNot Nothing Then
                If Not InDevEnv() Then
                    FillComboBoxSQL(iFillVersionSQL, cbVersion, "str", Appl.gSQLConnStr, False)
                    If cbVersion.Items.Count < 2 Then
                        FillComboBox("LastRun=LastRun,<Default>=<Default>", cbVersion)  'BHS 8/7/08
                    End If
                    'Always show a <Default> option, which should appear second.

                    'Load starting control values based on Version after descendant load is finished
                    QTimer1.Start()

                End If

                LoadForm()

                iIsLoading = False

            Catch ex As Exception
                ShowError("Error setting up form", ex)
                Close() 'Close form
            End Try
        End Sub

        '''<summary> Runs at the end of the load </summary>
        Private Sub QTimer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles QTimer1.Tick
            QTimer1.Stop()
            TightenCascade()
            FillControlsFromVersion()
        End Sub

        '''<summary> Fill all controls based on cbVersion.SelectedValue </summary>
        Sub FillControlsFromVersion()
            If Not IsNothing(cbVersion) AndAlso Not IsNothing(cbVersion.SelectedValue) Then
                Dim DV As DataView = SQLBuildDV("Select * From tRptCol Where classname = '" & _
                  GetObName() & "'" & " And title = '" & PrepareSQLSearchString(cbVersion.SelectedValue.ToString) & _
                  "' And savedby = '" & gUserName & "'", iSQLConn, False, "str")
                FillControlsFromtRptColDV(DV)   'BHS 8/8/8
            End If
            '...always allow user to do special processing, even if a version hasn't been used before!
            Try
                RaiseEvent OnFillControlsFromVersion()
            Catch ex As Exception
                ShowError("Unexpected error loading data into form controls (OnFillControlsFromVersion)", ex)
            End Try

        End Sub

#End Region

#Region "---------------------------- General Report Functions ------------------------------"

        '''<summary> Move parameters to iParams </summary>
        Public Sub SetParams(ByRef aParams As ArrayList)
            iParams = aParams
        End Sub

        '''<summary> Add a Parameter to the parameter array (used in Report Server, probably not in Active Reports) </summary>
        Public Sub SetParams(ByVal aName As String, ByVal aValue As String)
            Dim P As strParam = New strParam

            P.Name = aName
            P.Value = aValue
            iParams.Add(P)

        End Sub

        '''<summary> Show status message </summary>
        Overrides Function ShowStatus(ByVal aMessage As String) As Boolean
            'BHS 8/17/09 Only change message if there is a change - this avoids unneeded screen paints
            If StringCompare(StatusMsg.Text, aMessage) = False Then
                StatusMsg.Text = aMessage
                Refresh()
            End If

            Return True
        End Function

        '''<summary> Show Progress message </summary>
        Overrides Function ShowProgress(ByVal aMessage As String) As Boolean
            'BHS 8/17/09 Only change message if there is a change - this avoids unneeded screen paints
            If StringCompare(ProgressMsg.Text, aMessage) = False Then
                ProgressMsg.Text = aMessage
                Refresh()
            End If

            Return True
        End Function

        '''<summary> Show Progress and control cursor image </summary>
        Overrides Function ShowProgress(ByVal aMessage As String, ByVal aShowHourGlass As Boolean) As Boolean
            ProgressMsg.Text = aMessage
            Refresh()
            Me.Cursor = Cursors.Arrow
            If aShowHourGlass Then Me.Cursor = Cursors.WaitCursor
        End Function

        '''<summary> Show Help message </summary>
        Overrides Function ShowHelp(ByVal aMessage As String) As Boolean
            'BHS 8/17/09 Only change message if there is a change - this avoids unneeded screen paints
            If StringCompare(HelpMsg.Text, aMessage) = False Then
                HelpMsg.Text = aMessage
                Refresh()
            End If

            Return True
        End Function
#End Region
        ''' <summary>
        ''' Builds a string if Report Critera Name/Values separated by aPairDelimiter and a second string of 
        ''' Population Criteria separated by aPairDelimiter.  The two strings are concatenated, separated by ~.
        ''' </summary>
        Public Function BuildRptDescrStr(ByVal aControl As Control, Optional ByVal aPairDelimiter As String = ",") As String
            Dim wstr As String = ""
            'Report Criteria
            wstr = "Partition=" & cbPartition.Text


            Return wstr

        End Function

#Region "---------------------------- Version Functions ------------------------------"

        '''<summary> Return the name of this form object </summary>
        Function GetObName() As String
            Return Mid(Me.Name, 1, 24)
        End Function

        '''<summary> cbVersion is Changed </summary>
        Private Sub cbVersion_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbVersion.SelectedValueChanged
            If Not IsNothing(cbVersion) AndAlso Not IsNothing(cbVersion.SelectedValue) Then

                If cbVersion.SelectedValue.ToString = "<default>" Then
                    btnClear.PerformClick()
                    btnDelete.Visible = False
                    Return
                End If

                If iIsLoading = False Then FillControlsFromVersion()
                If cbVersion.SelectedValue.ToString = "Last Run" Then
                    btnDelete.Visible = False
                Else
                    btnDelete.Visible = True
                End If
            End If
        End Sub

        '''<summary> Move current controls to Last Run version in tRpt and tRptCol </summary>
        Function UpdateRptVersion() As Boolean
            Dim SQL As String = ""
            Dim WhereClause As String = " Where classname = '" & GetObName() & "'" & _
               " And title = 'Last Run' And savedby = '" & gUserName & "'"

            'Add a tRpt record if one doesn't exist
            SQL = "Select count(*) From tRpt " & WhereClause
            If SQLGetNumber(SQL) = 0 Then
                SQL = " Insert Into tRpt " & _
                    " (classname, title, savedby, sortno) Values " & _
                    " ('" & GetObName() & "', 'Last Run', '" & gUserName & "', 1)"
            End If
            'BHS 2/19/2010 Allow writing to report version in live database
            SQLDoSQL(SQL, True)

            'Add a record for each control
            SQLDoSQL("Delete from tRptCol " & WhereClause, True)
            ControlToRptCol(Me, "Last Run")

        End Function

        
#End Region

#Region "---------------------------- Button Events ------------------------------"
        '''<summary> Run Button Clicked (there will be descendant logic for the Run button also, 
        '''but logic order doesn't matter </summary>
        Private Sub btnRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRun.Click
            If ValidateForm() Then  'Make sure parameters validate OK
                UpdateRptVersion()  'Update tRpt and tRptCol with parameters
                Me.ControlBox = False   'Turn off control box
                Me.btnRun.Enabled = False   'Turn off <Run> button
                Dim Frm As fsMain = Nothing
                Try
                    RaiseEvent OnRun(Frm)  'Descendant takes over from here
                Catch ex As Exception
                    ShowError("Unexpected error running report (fpMainVersion OnRun)", ex)
                End Try

                If Frm IsNot Nothing Then Frm.iCallingFP = Me
            End If
        End Sub


        '''<summary> User clicks Save button to save a version </summary>
        Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

PromptForName:
            Dim frm As New Prompt("Save Version", "Enter a name to describe this report version")
            frm.ShowDialog()
            Dim Title As String = frm.iAnswer
            Title = PrepareSQLSearchString(Title)
            If Title.Length > 0 Then
                '...check for maximum length of 40 characters
                If Title.Length > 40 Then
                    MsgBoxInfo("Name can't exceed 40 characters--version not saved.", , "Save Version")
                    GoTo PromptForName
                End If
                '...can't be "<default>"
                If Title.ToLower.Trim = "<default>" Then
                    MsgBoxInfo("Version must be named something else.", , "Save Version")
                    GoTo promptforname
                End If

                Dim SQL As String = ""
                Dim WhereClause As String = " Where classname = '" & GetObName() & "'" & _
                   " And title = '" & Title & "' And savedby = '" & gUserName & "'"

                'Add a tRpt record if one doesn't exist
                SQL = "Select count(*) From tRpt " & WhereClause
                If SQLGetNumber(SQL) = 0 Then
                    SQL = " Insert Into tRpt " & _
                        " (classname, title, savedby, sortno) Values " & _
                        " ('" & GetObName() & "', '" & Title & "', '" & gUserName & "', 4)"
                    SQLDoSQL(SQL, True)
                End If

                'Add a record for each control
                SQLDoSQL("Delete from tRptCol " & WhereClause, True)
                ControlToRptCol(Me, Title)

                'Refresh cbVersion
                FillComboBoxSQL(iFillVersionSQL, cbVersion, "str", Appl.gSQLConnStr, False)
                'Point to new title after the combobox population is refreshed
                Dim i As Integer = cbVersion.FindStringExact(Title)
                If i > -1 Then cbVersion.SelectedIndex = i
            End If

        End Sub

        '''<summary> After checking with user, delete current version if it isn't 'Last Run' </summary>
        Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
            Dim Title As String = PrepareSQLSearchString(cbVersion.SelectedValue.ToString)
            If Title = "Last Run" Then Return

            If MsgBoxQuestion("OK to delete version named " & Title & "?", , "Delete " & Title) _
              = MsgBoxResult.Yes Then
                Dim WhereClause As String = " Where classname = '" & GetObName() & "'" & _
                   " And title = '" & Title & "' And savedby = '" & gUserName & "'"

                DoSQL(iSQLConn, "Delete from tRptCol " & WhereClause, "", True)
                DoSQL(iSQLConn, "Delete from tRpt " & WhereClause, "", True)
            End If

            'Refresh cbVersion after deleting the current version
            'If fBase.ActiveForm IsNot Nothing Then
            If Not InDevEnv() Then
                FillComboBoxSQL(iFillVersionSQL, cbVersion, "str", Appl.gSQLConnStr, False)
            End If
            FillControlsFromVersion()

        End Sub

#End Region

#Region "---------------------------- Obsolete ------------------------------"

        ''Recursively move Single-Entry Control values to tRptCol  'In fBase
        'Sub ControlToRptCol(ByVal aC As Control, ByVal aTitle As String)
        '    Dim Val As String = ""
        '    For Each C As Control In aC.Controls
        '        If isSingleEntryControl(C) Then
        '            Val = C.Text
        '            If C.Name = "cbVersion" Then Val = aTitle
        '            DoSQL("Insert Into tRptCol (classname, title, savedby, columnname, columnvalue) " & _
        '               " Values ('" & GetObName() & "', '" & aTitle & "', '" & gUserName & "', '" & _
        '               C.Name & "', '" & Val & "')")
        '        End If
        '        ControlToRptCol(C, aTitle)
        '    Next
        'End Sub

        ''Return True if this control is for entering a single value  '***VERSION
        'Function isSingleEntryControl(ByVal aC As Control) As Boolean
        '    If IsNothing(aC) Then Return False
        '    If TypeOf (aC) Is TextBox Then Return True
        '    If TypeOf (aC) Is CheckBox Then Return True
        '    If TypeOf (aC) Is ComboBox Then Return True
        '    If TypeOf (aC) Is DateTimePicker Then Return True
        '    If TypeOf (aC) Is MaskedTextBox Then Return True
        '    Return False
        'End Function

#End Region

        Private Sub btnDefault_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDefault.Click
            SQLDoSQL("Update UserMain Set DefaultObject = '" & Me.Name & "' Where UserID = '" & gUserName & "'", True)
        End Sub

        Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click
            'Call fbase function to clear all fields 
            ClearQueryControls(Me, False)
            'Raise event to allow decendent to clear list and set defaults
            Try
                RaiseEvent OnClearControl()
            Catch ex As Exception
                ShowError("Unexpected error clearing controls (OnClearControl)", ex)
            End Try

            Post("EndOfClear")
        End Sub

        '''<summary> Help button clicked </summary>
        Private Sub btnSearchHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearchHelp.Click
            Try
                Dim frm As New fhMain("Search")
                frm.Show()
            Catch ex As Exception
                ShowError("Error showing help", ex)
                Return  'Not fatal
            End Try
        End Sub
    End Class

End Namespace