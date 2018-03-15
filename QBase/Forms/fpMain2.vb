Namespace Windows.Forms
    'FIX - RUN BUTTON SHOULD RUN VALIDATE, AND THEN RUN ONRUN TO KICK OFF CLIENT LOGIC
    'Ancestor for Report and Update Parameters form.

    Public Class fpMain2
        Inherits fBase
        Private iSQLConn As New SqlClient.SqlConnection(Appl.gSQLConnStr)
        Private iFillVersionSQL As String
        Public iLastRunRecno As Integer = 0
        Public iVersionRecno As Integer = 0
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

                'iFillVersionSQL = "Select title Name, title Value, sortno From tRpt Where classname = '" & _
                '      GetObName() & "' And SavedBy = '" & gUserName & _
                '      "' AND title <> 'Window Size' Union Select '<default>', '<default>', 2 " & _
                '      " Order By sortno"
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
                    'FillComboBoxSQL(iFillVersionSQL, cbVersion, "str", Appl.gSQLConnStr, False)
                    'If cbVersion.Items.Count < 2 Then
                    '    FillComboBox("LastRun=LastRun,<Default>=<Default>", cbVersion)  'BHS 8/7/08
                    'End If
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
            '...pass recno of default version, which is the "Last Run" version
            ' GBV 4/21/2015 - Ticket 3666 - moved all tQBE_xxx tables to IASCommon database
            Dim DefaultRecno As Integer = CInt(SQLGetNumber("Select QBERecno From IASCommon..tQBE_Version Where ClassName = '" & _
                GetObjName() & "' And SavedBy = '" & gUserName & "' And DefaultQuery = 'True'"))
            If Not IsNothing(DefaultRecno) AndAlso DefaultRecno > 0 Then
                FillQBEDefaults_ByRecno(DefaultRecno)
                '...save currently-loaded version and Last Run version
                iVersionRecno = DefaultRecno
                iLastRunRecno = DefaultRecno
                'FillControlsFromtRptColDV(DV)   'BHS 8/8/8
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

        '''<summary> Move current controls to Last Run version in tRpt and tRptCol </summary>
        Function UpdateRptVersion() As Boolean
            Dim SQL, wstr As String

            'Add a tQBE_Version record if one doesn't exist
            ' GBV 4/21/2015 - Ticket 3666 - moved all tQBE_xxx tables to IASCommon database
            SQL = "Select count(*) From IASCommon..tQBE_Version Where QBERecno = " & iLastRunRecno.ToString
            If SQLGetNumber(SQL) = 0 Then
                SQL = "Insert Into IASCommon..tQBE_Version " & _
                    "(ClassName, Title, SavedBy, SortNo, Descr, Shared, EmailNotify, DefaultQuery, CreatedBy, CreatedDt) " & _
                    "Values ('" & GetObName() & "', 'Last Run', '" & gUserName & "', 1, 'Last Run', 'N', 'N', " & _
                    "'True', '" & gUserName & "', '" & Now.ToString & "')"
                SQLDoSQL(SQL, True)
                '...because we can't duplicate "Last Run" as a title, we can explicitly get the record number without
                '   using a data adaptor
                wstr = "Select QBERecno From IASCommon..tQBE_Version Where ClassName = '" & GetObjName() & _
                    "' And Title = 'Last Run' And SavedBy = '" & gUserName & "'"
                iLastRunRecno = CInt(SQLGetNumber(wstr))
            Else
                SQLDoSQL("Delete from IASCommon..tQBE_Columns Where QBERecno = " & iLastRunRecno.ToString, True)    'Force write to Live
            End If

            'Add a record for each control
            If iLastRunRecno > 0 Then ControlToQBEColumns(Me, iLastRunRecno)

        End Function

#End Region

#Region "---------------------------- Button Events ------------------------------"

        '''<summary> Save QBE fields to database </summary>
        Private Sub tsSaveQuery_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsSaveQuery.Click
            Try
                Dim objname As String = GetObjName()
                '...strip off first two characters to get resource name
                Dim resname As String = ""
                If objname.Length > 2 Then resname = Mid(objname, 3)

                '...try to get object's menu name
                Dim classnamedescr As String = SQLGetString("Select menuname From gmenu Where resname = '" & resname & "'")
                If classnamedescr.IndexOf("(Old") > 0 Then classnamedescr = Mid(classnamedescr, 1, classnamedescr.IndexOf("(Old"))
                classnamedescr = "NUI " & classnamedescr.Trim

                '...pull up Saved Query screen
                'Dim frm As New feSaveQuery
                'frm.iClassname = GetObjName()
                'frm.iClassnamedescr = classnamedescr
                'frm.iQBEControl = Me
                'frm.iOriginalRecno = iVersionRecno
                'frm.iVersionType = "R"
                'frm.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
                'frm.ShowDialog()
                ''...this version may have changed!
                'iVersionRecno = frm.iOriginalRecno
            Catch ex As Exception
                ShowError("Problem saving Query", ex)
            End Try
        End Sub

        ''' <summary> Load QBE fields from database </summary>
        Private Sub tsLoadQuery_Click(sender As System.Object, e As System.EventArgs) Handles tsLoadQuery.Click
            Try
                Dim frm As New feLoadQuery
                frm.iClassname = GetObjName()
                frm.iQBEControl = Me
                frm.iVersionRecno = iVersionRecno
                frm.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
                frm.ShowDialog()
                '...load another query version, if appropriate
                If frm.iVersionRecno <> iVersionRecno Then
                    iVersionRecno = frm.iVersionRecno
                    FillQBEDefaults_ByRecno(iVersionRecno)
                End If
                '...switch focus to <Search> button
                SetFocusControl("btnQuery")
            Catch ex As Exception
                ShowError("Problem loading Query", ex)
            End Try
        End Sub

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

                If Frm IsNot Nothing Then Frm.iCallingFP2 = Me
            End If
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
            iVersionRecno = 0
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