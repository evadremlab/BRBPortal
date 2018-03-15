'Imports Infragistics.Win.UltraWinGrid
Namespace Windows.Forms

    'ToDo - derive tool tip from metadata and automatically load (!)
    'Child Entry Forms inherit from here.
    Public Class feMain2

        Private iSaveOrigValue As Boolean
        Private iDeleteOrigValue As Boolean
        Private iNewOrigValue As Boolean
        Protected Event OnAfterSetControlAttributes(ByRef aC As Control)
        Protected Event OnShowSQL() 'SDC 04/02/2014

        Protected Event OnLeaveRecord()
        Private iAlreadyLoaded As Boolean = False ' GBV 1/26/2015 - Ticket 2439
        ' Private iLockTransferred As Boolean = False ' GBV 7/20/2015 - Ticket 2439

#Region "---------------------------- Documentation ------------------------------"
        'Load
        '   SetupForm sets up buttons, all event handlers
        '   SetContext collects info from child
        '   LoadForm fills the form with data

        'BHS 2/21/08 Change SetControlAttributes to only consider Q controls

#End Region

#Region "---------------------------- Properties ---------------------------------"

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

#End Region

#Region "---------------------------- Load ---------------------------------------"

        '''<summary> Fires when form first opens </summary>
        Private Sub feMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                iIsResizable = False    'Default, may be set to True if descendant form is master/detail
                'iIsResizable = True ' GBV 8/18/2014 - Ticket 3130 - commented out on 9/2/2014 - IAS staff did not like it.

                iSaveOrigValue = btnSave.Visible
                iDeleteOrigValue = btnDelete.Visible
                iNewOrigValue = btnNew.Visible

                ' GBV 10/23/2014 - Ticket 2439 - Suppress Message Boxes and passes this form to the RecordLock object
                ''iRecordLock = New RecordLock(True, False, Me)

                SetupForm()     'Visual setup of form

                If SetContext() Then   'Child-specific values required by Parents (e.g. Key Field Names)
                    LoadForm()      'Load data, setup validation, setup entry field array
                End If

                iAlreadyLoaded = True

            Catch ex As Exception
                ShowError("Error setting up form. ", ex)
                Post("close") 'Close form
            End Try
        End Sub

        '''<summary> Local setcontext values </summary>
        Private Sub feMain_OnSetContext() Handles Me.OnSetContext
            iMode = "Edit"
            btnSQL.Visible = False  '...let descendant turn this button on, based on feature set
            ShowStatus("")      'BHS 7/14/08
        End Sub

        '''<summary> After Set Context </summary>
        Private Sub feMain_OnAfterSetContext(ByRef aOK As Boolean) Handles Me.OnAfterSetContext
            'Turn off naviagation if list references not entered
            If iListForm Is Nothing And iListNavigator Is Nothing And iListGV Is Nothing Then
                bnFirst.Visible = False
                bnPrev.Visible = False
                bnNext.Visible = False
                bnLast.Visible = False
            End If
            'Set up drill-down events from lists
            If Not InDevEnv() Then
                For Each GV As DataGridView In iGVs 'Only do GVs marked by the programmer for event handling
                    AddHandler GV.CellDoubleClick, AddressOf gvDoubleClick
                    AddHandler GV.CellContentClick, AddressOf GVContentClick
                Next
                'For Each UG As UltraGrid In iUGs    'Only do UGs marked by the programmer for event handling
                '    AddHandler UG.DoubleClickCell, AddressOf UGDoubleClick
                '    AddHandler UG.AfterCellActivate, AddressOf UGContentClick
                'Next
            End If

            ' GBV 6/12/2015 - Ticket 1245 - Save this form in the List form
            If iListForm IsNot Nothing Then
                iListForm.iEditForm = Me
            End If

        End Sub

        '''<summary> After Form Load logic - called from fBase.LoadForm </summary>
        Private Sub feMain_OnAfterLoadForm() Handles Me.OnAfterLoadForm
            Dim GV As DataGridView

            'Each GV gets at least one empty row
            For Each GV In iGVs
                If GV.Rows.Count = 0 Then
                    If TypeOf (GV) Is qGVEdit Then
                        Dim qGV As qGVEdit = CType(GV, qGVEdit)
                        If qGV._BlankRowOnEmpty = False Then Continue For
                    End If
                    If Not TypeOf (GV) Is qGVList Then InsertRow(GV) 'BHS 7/17/7 Allow list GVs in edit env
                End If
            Next

        End Sub

        '''<summary> Set up visual characteristics controls that get inherited </summary>
        Function SetupForm() As Boolean

            StatusStrip1.BackColor = QBackColor
            Return True
        End Function

        'BHS 2/25/08 Along with SetControlAttributes, below, rewritten
        '''<summary> Set protected/unprotected attributes based on iIsNew </summary>
        Private Sub feMain_OnSetFormAttributes() Handles Me.OnSetFormAttributes

            'Set Focus to appropriate entry control
            If iIsNew = True Then
                SetFocusControl(iFirstKeyField)
            Else
                SetFocusControl(iFirstNonKeyField)
            End If

            'If runtime, make command buttons visible or not, and set control attributes, based on iIsWriter
            'If fBase.ActiveForm IsNot Nothing Then
            If Not InDevEnv() Then
                If My.Application.Info.Title <> "DMS" Then
                    If iIsCampusWriter Then 'GBV 7/15/2008
                        iIsWriter = True
                    ElseIf iCampusWriterChecked Then 'Not campus writer AND IsCampusWriter was run
                        iIsWriter = False
                    ElseIf Auth.GetPermLevel(iFName) = 2 AndAlso Not iCampusWriterChecked Then
                        iIsWriter = True
                    End If
                End If

                'BHS 5/14/10
                iIsWriter = DescendantOverrideIsWriter(iIsWriter)

                If ClientSetCmdButtonsVisible() = False Then    'BHS 6/27/08 allow client program to set command buttons visible
                    If Not iIsWriter Then ' GBV 6/21/2008
                        btnNew.Visible = False
                        btnDelete.Visible = False
                        btnSave.Visible = False
                        btnSaveAndNew.Visible = False
                        btnSaveAndNext.Visible = False
                        btnSaveAndClose.Visible = False
                    Else                              ' GBV 6/21/2008
                        btnNew.Visible = iNewOrigValue
                        If iIsNew = True Then
                            btnDelete.Visible = False   'BHS 10/10/08 never offer Delete button in New mode
                        Else
                            btnDelete.Visible = iDeleteOrigValue
                        End If

                        btnSave.Visible = iSaveOrigValue
                        btnSaveAndNew.Visible = iSaveOrigValue
                        btnSaveAndNext.Visible = iSaveOrigValue
                        btnSaveAndClose.Visible = iSaveOrigValue
                    End If

                    For Each C As Control In Me.Controls
                        SetControlAttributes(C)
                    Next
                End If
            End If

            'Additional child Attributes can be set in child OnSetFormAttributes event
            '(but be careful - no predictable order of processing between this level and child)

        End Sub

        '''<summary> Set Control Attributes based on user permission, and whether in New or Edit mode </summary>
        Sub SetControlAttributes(ByVal aC As Control)
            Dim T As qTextBox = TryCast(aC, qTextBox)
            Dim MT As qMaskedTextBox = TryCast(aC, qMaskedTextBox)
            Dim CB As qComboBox = TryCast(aC, qComboBox)
            Dim GV As qGVBase = TryCast(aC, qGVBase)
            Dim DD As QSILib.qDD = TryCast(aC, QSILib.qDD)
            Dim TAB As TabPage = TryCast(aC, TabPage)
            Dim QCHBX As qCheckBox = TryCast(aC, qCheckBox)
            Dim CHBX As CheckBox = TryCast(aC, CheckBox) ' GBV 7/13/2008
            Dim LB As ListBox = TryCast(aC, ListBox) ' GBV 7/13/2008
            Dim RC As qRC = TryCast(aC, qRC)

            Dim localwriter As Boolean = iIsWriter  'BHS 6/22/10 Localwriter is local to this control
            Dim iResourceName As String = aC.FindForm().Name & "." & aC.Name
            Dim PermLevel As Integer = 1    'BHS 6/22/10 PermLevel is for this control only

            ' Query permissions table
            If My.Application.Info.Title <> "DMS" Then
                PermLevel = Auth.GetPermLevel(iResourceName) ' GBV 7/11/2008
            Else
                PermLevel = 1
                If iIsWriter = True Then PermLevel = 3
            End If


            'Check permission level returned, if any
            If iIsCampusWriter OrElse Not iCampusWriterChecked Then 'GBV 7/15/2008
                If PermLevel > -1 Then  'BHS 6/22/10 update localwrter if Control Permissions have been set
                    localwriter = PermLevel > 1
                End If
            End If

            'BHS 6/22/10 Below, only PermLevel = 0 is checked for.  If ClientAuthority doesn't explicitly set
            'PermLevel = 0 then PermLevel has no effect on the following logic.
            If Not localwriter AndAlso PermLevel = -1 Then 'GBV 7/15/2008
                PermLevel = 1
            End If

            'If user is writer on this control, set iIsWriter

            'BHS 6/22/10 the following is commented out because we don't want to set iIsWriter, which affects the entire
            'form, based on this control's permission
            'If localwriter Then iIsWriter = True

            ' If user is writer on this control, make sure save button is visible
            ' GBV 8/3/2015 - moved this to the bottom
            'If localwriter AndAlso iSaveOrigValue = True Then
            '    btnSave.Visible = True
            '    btnSaveAndNew.Visible = True
            '    btnSaveAndNext.Visible = True
            '    btnSaveAndClose.Visible = True
            'End If

            'Set qTextbox attributes
            If T IsNot Nothing Then
                If T._ReadAlways Then localwriter = False ' GBV 7/14/2008
                If T._TransparentDisplay = True Then
                    T.SetTransparentDisplay()
                Else
                    If T._IsKeyField = True Then
                        If iIsNew = True Then
                            If Not localwriter Then
                                'If iIsWriter = False Or T._ReadAlways = True Then GBV 7/14/2008
                                T.SetWriter(False)
                            Else
                                T.SetWriter(True)
                            End If
                        Else    'key field in Edit Mode is always readonly
                            T.SetWriter(False)
                        End If
                    Else    'not a key field
                        If Not T._ReadAlways AndAlso T._ValidateRequired AndAlso iIsNew Then 'GBV 7/14/2008
                            T.SetWriter(True)
                        ElseIf Not localwriter Then
                            'If localwriter = False Or T._ReadAlways = True Then GBV 7/14/2008
                            'If iIsWriter = False Or T._ReadAlways = True Then  BHS 6/20/08
                            If PermLevel = 0 Then
                                T.SetTopSecret() 'GBV 7/14/2008
                            Else
                                T.SetWriter(False)
                            End If
                        Else
                            T.SetWriter(True)
                        End If
                    End If
                End If
            End If

            'Set qMasked Textbox attributes
            If MT IsNot Nothing Then
                If MT._ReadAlways Then localwriter = False 'GBV 7/14/2008
                If MT._IsKeyField = True Then
                    If iIsNew = True Then
                        If Not localwriter Then
                            'If iIsWriter = False Or MT._ReadAlways = True Then GBV 7/14/2008
                            MT.SetWriter(False)
                        Else
                            MT.SetWriter(True)
                        End If
                    Else    'key field in Edit Mode is always readonly
                        MT.SetWriter(False)
                    End If
                Else    'not a key field
                    If Not MT._ReadAlways AndAlso MT._ValidateRequired AndAlso iIsNew Then 'GBV 7/14/2008
                        MT.SetWriter(True)
                    ElseIf Not localwriter Then
                        'If localwriter = False Or T._ReadAlways = True Then GBV 7/14/2008
                        'If iIsWriter = False Or T._ReadAlways = True Then  BHS 6/20/08
                        If PermLevel = 0 Then
                            MT.SetTopSecret()
                        Else
                            MT.SetWriter(False)
                        End If
                    Else
                        MT.SetWriter(True)
                    End If
                End If
            End If


            'BHS 2/9/10 From Oakland
            'Set qDD attributes
            If DD IsNot Nothing Then
                If DD._ReadAlways Then localwriter = False
                If DD._IsKeyField = True Then
                    If iIsNew = True Then
                        If Not localwriter Then
                            'If iIsWriter = False Or DD._ReadAlways = True Then GBV 7/14/2008
                            DD.SetWriter(False)
                        Else
                            DD.SetWriter(True)
                        End If
                    Else    'key field in Edit Mode is always readonly
                        DD.SetWriter(False)
                    End If
                Else    'not a key field
                    If Not DD._ReadAlways AndAlso DD._ValidateRequired AndAlso iIsNew Then 'GBV 7/14/2008
                        DD.SetWriter(True)
                    ElseIf Not localwriter Then
                        'If localwriter = False Or T._ReadAlways = True Then GBV 7/14/2008
                        'If iIsWriter = False Or T._ReadAlways = True Then  BHS 6/20/08
                        If PermLevel = 0 Then
                            DD.SetTopSecret()
                        Else
                            DD.SetWriter(False)
                        End If
                    Else
                        DD.SetWriter(True)
                    End If
                End If
            End If

            'Set qComboBox attributes
            If CB IsNot Nothing Then
                If CB._ReadAlways Then localwriter = False
                If CB._IsKeyField = True Then
                    If iIsNew = True Then
                        If Not localwriter Then
                            'If iIsWriter = False Or CB._ReadAlways = True Then GBV 7/14/2008
                            CB.SetWriter(False)
                        Else
                            CB.SetWriter(True)
                        End If
                    Else    'key field in Edit Mode is always readonly
                        CB.SetWriter(False)
                    End If
                Else    'not a key field
                    If Not CB._ReadAlways AndAlso CB._ValidateRequired AndAlso iIsNew Then 'GBV 7/14/2008
                        CB.SetWriter(True)
                    ElseIf Not localwriter Then
                        'If localwriter = False Or T._ReadAlways = True Then GBV 7/14/2008
                        'If iIsWriter = False Or T._ReadAlways = True Then  BHS 6/20/08
                        If PermLevel = 0 Then
                            CB.SetTopSecret()
                        Else
                            CB.SetWriter(False)
                        End If
                    Else
                        CB.SetWriter(True)
                    End If
                End If
            End If

            If GV IsNot Nothing Then
                'If Not iIsWriter Then GBV 6/24/2008
                If Not localwriter AndAlso PermLevel = 0 Then ' GBV 7/10/2008
                    GV.Visible = False
                    Return
                ElseIf Not localwriter Then
                    GV.ReadOnly = True
                    For Each Col As DataGridViewColumn In GV.Columns ' GBV 7/13/2008
                        Col.ReadOnly = True
                    Next
                    Return
                End If
            End If

            'Set CheckBoxes  GBV 7/13/2008
            If CHBX IsNot Nothing Then
                '...logic for qCheckbox (BHS 7/29/2010)
                If QCHBX IsNot Nothing Then
                    If QCHBX._IsKeyField = True Then
                        If iIsNew = True Then
                            If Not localwriter Then
                                QCHBX.SetWriter(False)
                            Else
                                QCHBX.SetWriter(True)
                            End If
                        Else
                            QCHBX.SetWriter(False)
                        End If

                    Else
                        If iIsNew = True Then
                            QCHBX.SetWriter(True)
                        Else
                            If Not localwriter Then
                                QCHBX.SetWriter(False)
                            Else
                                QCHBX.SetWriter(True)
                            End If
                        End If
                    End If
                Else    'regular checkbox, not qChBx
                    If Not localwriter AndAlso PermLevel = 0 Then
                        CHBX.Visible = False
                    ElseIf Not localwriter Then
                        CHBX.Enabled = False
                    End If
                End If
            End If


            'Set Radio Control BHS 7/12/12

            If RC IsNot Nothing Then
                If RC._IsKeyField = True Then
                    If iIsNew = True Then
                        If Not localwriter Then
                            RC.SetWriter(False)
                        Else
                            RC.SetWriter(True)
                        End If
                    Else
                        RC.SetWriter(False)
                    End If

                Else
                    If iIsNew = True Then
                        RC.SetWriter(True)
                    Else
                        If Not localwriter Then
                            RC.SetWriter(False)
                        Else
                            RC.SetWriter(True)
                        End If
                    End If
                End If
            End If

            ' Set ListBoxes   GBV 7/13/2008
            If LB IsNot Nothing Then
                If Not localwriter AndAlso PermLevel = 0 Then
                    LB.Visible = False
                    Return
                ElseIf Not localwriter AndAlso PermLevel = 1 Then
                    LB.Enabled = False
                    Return
                End If
            End If

            'Set TabPages               GBV 7/9/2008
            If TAB IsNot Nothing Then
                If Not localwriter AndAlso PermLevel = 0 Then
                    Dim TAbC = CType(TAB.Parent, TabControl)

                    ' GBV 2/24/2015 - Need this otherwise when tab is removed, it skips the next tab
                    Dim NextTab As TabPage = Nothing
                    If TAbC.TabPages.Count > TAbC.TabPages.IndexOf(TAB) + 1 Then
                        NextTab = TAbC.TabPages(TAbC.TabPages.IndexOf(TAB) + 1)
                    End If

                    TAbC.TabPages.Remove(TAB)

                    If NextTab IsNot Nothing Then SetControlAttributes(NextTab) ' GBV 2/24/2015
                    Return  'avoid recursion
                End If
            End If

            ' If user is writer on this control, make sure save button is visible
            If localwriter AndAlso iSaveOrigValue = True Then
                btnSave.Visible = True
                btnSaveAndNew.Visible = True
                btnSaveAndNext.Visible = True
                btnSaveAndClose.Visible = True
            End If

            'BHS 12/6/10
            Try
                RaiseEvent OnAfterSetControlAttributes(aC)
            Catch ex As Exception
                ShowError("Unexpected error after setting control attributes (feMain OnAfterSetControlAttributes)", ex)
            End Try


            'Recursively set all other controls within this control
            For Each C As Control In aC.Controls
                SetControlAttributes(C)
            Next

        End Sub


#End Region

#Region "---------------------------- Navigation ---------------------------------"

        '''<summary> False if no iListNavigator or iListGV, or if Dirty and user wants to save changes </summary>
        Function NavigationOK() As Boolean
            Dim Answer As MsgBoxResult

            'BHS 1/30/08 a parent iListNavigators is available from lists, but probably won't be available from
            '   parent edit forms.  However, if iListGV or iListUG are set in the child form, and if the parent
            '   grid uses a binding source (like iBS), then we can navigate based on the list in the parent.
            If iListNavigator Is Nothing Then
                If iListGV IsNot Nothing Then
                    If Not TypeOf (iListGV.DataSource) Is BindingSource Then Return False
                Else
                    'If iListUG IsNot Nothing AndAlso TypeOf (iListUG.DataSource) Is BindingSource Then
                    'Else
                    '    Return False
                    'End If
                End If
            End If

            If CheckDirty() Then
                Answer = MsgBoxQuestion("OK to leave record without saving?")
                If Answer = MsgBoxResult.No Then
                    Return False 'Cancel
                Else
                    iEP.Clear() 'We're not saving and we're moving to a new record, so clear errors
                End If
            End If

            Return True

        End Function

        '''<summary> Load form with iKey from first row in list </summary>
        Private Sub BindingNavigatorMoveFirstItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bnFirst.Click
            Try
                'BHS 5/6/11 force isdirty check 
                If ControlIsDirty(iCurrentControlName, Me, iOrigValue) = True Then
                    iIsDirty = True
                End If

                SetFocusControl(iFirstNonKeyField)
                If NavigationOK() Then
                    iIsDirty() = False

                    Try
                        '--- HOT FIX 5.1.10
                        RaiseEvent OnLeaveRecord()
                        '--- END HOT FIX 5.1.10
                    Catch ex As Exception
                        ShowError("Unexpected error leaving record (feMain OnLeaveRecord)", ex)
                    End Try



                    Try
                        If iListGV IsNot Nothing Then
                            Dim BS As BindingSource = TryCast(iListGV.DataSource, BindingSource)
                            If BS IsNot Nothing Then BS.MoveFirst()
                            iKey = iListForm.GetKey(iListGV, iListGV.SelectedRows(0).Index)
                        Else
                            'If iListUG IsNot Nothing Then
                            '    Dim BS As BindingSource = TryCast(iListUG.DataSource, BindingSource)
                            '    If BS IsNot Nothing Then BS.MoveFirst()
                            '    iListUG.Refresh()
                            '    iKey = iListForm.GetKey(iListUG, iListUG.ActiveRow.Index)
                            'End If
                        End If

                        Me.LoadForm()
                        'BHS 5/17/10 SetFormAttributes() is done as part of Me.LoadForm, and calling
                        'it again overwrites form attributes written in OnAfterLoad
                        'SetFormAttributes() 'Set permissions based on this data

                    Catch ex As Exception
                        'Don't load form if can't find (THIS IS AN EXPENSIVE TECHNIQUE - IMPROVE SOME DAY
                    End Try
                End If
            Catch ex As Exception
                ShowError("Error using navigation keys", ex)
                Close() 'Close form
            End Try
        End Sub

        '''<summary> Load Form with iKey from previous row in list </summary>
        Private Sub BindingNavigatorMovePreviousItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bnPrev.Click
            Try
                'BHS 5/6/11 force isdirty check 
                If ControlIsDirty(iCurrentControlName, Me, iOrigValue) = True Then
                    iIsDirty = True
                End If

                SetFocusControl(iFirstNonKeyField)
                If NavigationOK() Then
                    iIsDirty() = False

                    Try
                        '--- HOT FIX 5.1.10
                        RaiseEvent OnLeaveRecord()
                        '--- END HOT FIX 5.1.10
                    Catch ex As Exception
                        ShowError("Unexpected error leaving record (feMain OnLeaveRecord)", ex)
                    End Try


                    Try
                        If iListGV IsNot Nothing Then
                            Dim BS As BindingSource = TryCast(iListGV.DataSource, BindingSource)

                            If iKey <> iListForm.GetKey(iListGV, iListGV.SelectedRows(0).Index) And _
                                     iListGV.SelectedRows(0).Index = iListGV.Rows.Count - 1 Then
                                'SRM 06/30/11 Don't move record if key is different and on last record -- this means the user 
                                'changed the record and the old record is no longer in the list
                            Else
                                If BS IsNot Nothing Then BS.MovePrevious()
                            End If
                            iKey = iListForm.GetKey(iListGV, iListGV.SelectedRows(0).Index)
                        Else
                            'If iListUG IsNot Nothing Then
                            '    Dim BS As BindingSource = TryCast(iListUG.DataSource, BindingSource)
                            '    If BS IsNot Nothing Then BS.MovePrevious()
                            '    iListUG.Refresh()
                            '    iKey = iListForm.GetKey(iListUG, iListUG.ActiveRow.Index)
                            'End If
                        End If

                        Me.LoadForm()
                        'BHS 5/17/10 SetFormAttributes() is done as part of Me.LoadForm, and calling
                        'it again overwrites form attributes written in OnAfterLoad
                        'SetFormAttributes() 'Set permissions based on this data
                    Catch ex As Exception
                        'Don't load form if can't find (THIS IS AN EXPENSIVE TECHNIQUE - IMPROVE SOME DAY
                    End Try
                End If
            Catch ex As Exception
                ShowError("Error using navigation keys", ex)
                Close() 'Close form
            End Try
        End Sub

        '''<summary> Load Form with iKey from next row in list </summary>
        Private Sub BindingNavigatorMoveNextItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bnNext.Click
            Try
                'BHS 5/6/11 force isdirty check 
                If ControlIsDirty(iCurrentControlName, Me, iOrigValue) = True Then
                    iIsDirty = True
                End If

                SetFocusControl(iFirstNonKeyField)

                If NavigationOK() Then

                    iIsDirty() = False

                    Try
                        '--- HOT FIX 5.1.10
                        RaiseEvent OnLeaveRecord()
                        '--- END HOT FIX 5.1.10
                    Catch ex As Exception
                        ShowError("Unexpected error leaving record (feMain OnLeaveRecord)", ex)
                    End Try


                    Try
                        If iListGV IsNot Nothing Then
                            Dim BS As BindingSource = TryCast(iListGV.DataSource, BindingSource)
                            'SRM 06/30/11 Don't move record if key is different -- this means the user 
                            'changed the record and the old record is no longer in the list
                            If iKey = iListForm.GetKey(iListGV, iListGV.SelectedRows(0).Index) Then
                                If BS IsNot Nothing Then BS.MoveNext()
                            End If
                            iKey = iListForm.GetKey(iListGV, iListGV.SelectedRows(0).Index)
                        Else
                            'If iListUG IsNot Nothing Then
                            '    Dim BS As BindingSource = TryCast(iListUG.DataSource, BindingSource)
                            '    If BS IsNot Nothing Then BS.MoveNext()
                            '    iListUG.Refresh()
                            '    iKey = iListForm.GetKey(iListUG, iListUG.ActiveRow.Index)
                            'End If
                        End If

                        Me.LoadForm()
                        'BHS 5/17/10 SetFormAttributes() is done as part of Me.LoadForm, and calling
                        'it again overwrites form attributes written in OnAfterLoad
                        'SetFormAttributes() 'Set permissions based on this data
                    Catch ex As Exception
                        'Don't load form if can't find (THIS IS AN EXPENSIVE TECHNIQUE - IMPROVE SOME DAY
                    End Try
                End If
            Catch ex As Exception
                ShowError("Error using navigation keys", ex)
                Close() 'Close form
            End Try
        End Sub

        '''<summary> Load Form with iKey from last row in list </summary>
        Private Sub BindingNavigatorMoveLastItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bnLast.Click
            Try
                'BHS 5/6/11 force isdirty check 
                If ControlIsDirty(iCurrentControlName, Me, iOrigValue) = True Then
                    iIsDirty = True
                End If

                SetFocusControl(iFirstNonKeyField)
                If NavigationOK() Then
                    iIsDirty() = False

                    Try
                        '--- HOT FIX 5.1.10
                        RaiseEvent OnLeaveRecord()
                        '--- END HOT FIX 5.1.10
                    Catch ex As Exception
                        ShowError("Unexpected error leaving record (feMain OnLeaveRecord)", ex)
                    End Try

                    Try
                        If iListGV IsNot Nothing Then
                            Dim BS As BindingSource = TryCast(iListGV.DataSource, BindingSource)
                            If BS IsNot Nothing Then BS.MoveLast()
                            iKey = iListForm.GetKey(iListGV, iListGV.SelectedRows(0).Index)
                        Else
                            'If iListUG IsNot Nothing Then
                            '    Dim BS As BindingSource = TryCast(iListUG.DataSource, BindingSource)
                            '    If BS IsNot Nothing Then BS.MoveLast()
                            '    iListUG.Refresh()
                            '    iKey = iListForm.GetKey(iListUG, iListUG.ActiveRow.Index)
                            'End If
                        End If

                        Me.LoadForm()
                        'BHS 5/17/10 SetFormAttributes() is done as part of Me.LoadForm, and calling
                        'it again overwrites form attributes written in OnAfterLoad
                        'SetFormAttributes() 'Set permissions based on this data
                    Catch ex As Exception
                        'Don't load form if can't find (THIS IS AN EXPENSIVE TECHNIQUE - IMPROVE SOME DAY
                    End Try

                End If
            Catch ex As Exception
                ShowError("Error using navigation keys", ex)
                Close() 'Close form
            End Try
        End Sub



#End Region

#Region "---------------------------- Command Buttons, ToolStrip and Menu --------"
        ''' <summary> New Button Clicked </summary>
        Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
            Try
                ShowStatus("")
                NewRecord(True) 'True does dirty checking
            Catch ex As Exception
                ShowError("Error creating a new record", ex)
                Return 'Not fatal
            End Try

        End Sub

        ''' <summary> Delete button clicked </summary>
        Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
            Try
                DeleteClicked()
            Catch ex As Exception
                ShowError("Error deleting record", ex)
                Return 'Not fatal
            End Try

        End Sub

        ''' <summary> Save button clicked </summary>
        Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
            Try
                btnSave.Enabled = False
                SaveClicked()
                btnSave.Enabled = True
            Catch ex As Exception
                ShowError("Error saving record", ex)
                btnSave.Enabled = True
                Return 'Not fatal
            End Try

        End Sub

        Private Sub btnSaveAndNext_Click(sender As System.Object, e As System.EventArgs) Handles btnSaveAndNext.Click
            Try
                If SaveClicked() Then
                    bnNext.PerformClick()
                End If

            Catch ex As Exception
                ShowError("Unexpected error creating a new record", ex)
            End Try

        End Sub

        Private Sub btnSaveAndNew_Click(sender As System.Object, e As System.EventArgs) Handles btnSaveAndNew.Click
            Try
                If SaveClicked() Then
                    If NewRecord(True) Then
                        'Do we need to set focus on first field?
                    End If
                End If

            Catch ex As Exception
                ShowError("Unexpected error creating a new record", ex)
            End Try
        End Sub

        Private Sub btnSaveAndClose_Click(sender As System.Object, e As System.EventArgs) Handles btnSaveAndClose.Click
            Try
                If SaveClicked() Then
                    Me.Close()
                End If

            Catch ex As Exception
                ShowError("Unexpected error creating a new record", ex)
            End Try
        End Sub

        ''' <summary> User clicked something in the menu </summary>
        Overrides Function ReceiveMenuClick(ByVal aMenuItemName As String, ByVal aMenuItemTag As String) As Boolean
            Try
                If aMenuItemTag.ToLower = "btnnew" Then btnNew.PerformClick()
                If aMenuItemTag.ToLower = "btnsave" Then btnSave.PerformClick()
                If aMenuItemTag.ToLower = "btnsaveandnext" Then btnSaveAndNext.PerformClick()
                If aMenuItemTag.ToLower = "btnsaveandnew" Then btnSaveAndNew.PerformClick()
                If aMenuItemTag.ToLower = "btnsaveandclose" Then btnSaveAndClose.PerformClick()
                If aMenuItemTag.ToLower = "btndelete" Then btnDelete.PerformClick()
                If aMenuItemTag.ToLower = "bnfirst" Then bnFirst.PerformClick()
                If aMenuItemTag.ToLower = "bnprev" Then bnPrev.PerformClick()
                If aMenuItemTag.ToLower = "bnnext" Then bnNext.PerformClick()
                If aMenuItemTag.ToLower = "bnlast" Then bnLast.PerformClick()
                SetMenu()
            Catch ex As Exception
                ShowError("Error running menu option", ex)
                Return False 'Not fatal
            End Try

        End Function

        ''' <summary> When form gets focus, refresh the menu </summary>
        ''' <remarks>GBV 1/26/2015 - Ticket 2439 - reset the form's attributes when the user comes back to the
        ''' form in case the lock was transferred to another form</remarks>
        Private Sub feMain_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
            SetMenu()

            ' GBV 1/26/2015 - Ticket 2439
            If iAlreadyLoaded Then
                ''If iRecordLock IsNot Nothing Then
                'iLockTransferred = False
                'iRecordLock.Clear()
                ''iRecordLock.ResetLockFlags(False)
                SetFormAttributes()
                ''End If
            End If
        End Sub

        ''' <summary> Apply filter after deleting a record </summary>
        Private Sub feMain_OnAfterDelete() Handles Me.OnAfterDelete
            If Not iListForm Is Nothing Then
                iListForm.find1()
            End If
        End Sub

        ''' <summary> Help button clicked </summary>
        Private Sub btnHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHelp.Click
            Try
                Dim frm As New fhMain(iFName)
                frm.Show()
            Catch ex As Exception
                ShowError("Error showing help", ex)
                Return 'Not fatal
            End Try
        End Sub

        ''' <summary> Show status message </summary>
        Overrides Function ShowStatus(ByVal aMessage As String) As Boolean
            'BHS 8/17/09 Only change message if there is a change - this avoids unneeded screen paints
            If StringCompare(StatusMsg.Text, aMessage) = False Then
                StatusMsg.Text = aMessage
                Refresh()
            End If
            Return True
        End Function

        ''' <summary> Show Progress message </summary>
        Overrides Function ShowProgress(ByVal aMessage As String) As Boolean
            'BHS 8/17/09 Only change message if there is a change - this avoids unneeded screen paints
            If StringCompare(ProgressMsg.Text, aMessage) = False Then
                ProgressMsg.Text = aMessage
                Refresh()
            End If

            Return True
        End Function

        ''' <summary> Show Progress and control cursor image </summary>
        Overrides Function ShowProgress(ByVal aMessage As String, ByVal aShowHourGlass As Boolean) As Boolean
            'BHS 8/17/09 Only change message if there is a change - this avoids unneeded screen paints
            If StringCompare(ProgressMsg.Text, aMessage) = False Then
                ProgressMsg.Text = aMessage
                Refresh()
            End If

            Me.Cursor = Cursors.Arrow
            If aShowHourGlass Then Me.Cursor = Cursors.WaitCursor
        End Function

        ''' <summary> Show Help message </summary>
        Overrides Function ShowHelp(ByVal aMessage As String) As Boolean
            'BHS 8/17/09 Only change message if there is a change - this avoids unneeded screen paints
            If StringCompare(HelpMsg.Text, aMessage) = False Then
                HelpMsg.Text = aMessage
                Refresh()
            End If

            Return True
        End Function

        ''' <summary> Show Yellow rectangle to the left of the New Button if aIsDirty = True </summary>
        Overrides Sub SetDirtyIndicator(ByVal aIsDirty As Boolean)
            'Put bar to left of New button, if it is visible
            If btnNew.Visible = True Then
                Dim Loc As Point = New Point(btnNew.Location.X - 5, btnNew.Location.Y)
                txtDirtyDisplay.Location = Loc
                txtDirtyDisplay.Visible = True  'Set to not visible in designer
                If aIsDirty = True Then
                    txtDirtyDisplay.BackColor = Color.Yellow
                Else
                    txtDirtyDisplay.BackColor = Color.White
                End If
            End If
        End Sub

        ''' <summary> User wants to close form </summary>
        Private Sub feMain_FormClosing1(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
            If CheckDirty() Then
                Dim Answer As MsgBoxResult = MsgBoxQuestion("OK to leave form without saving?")
                If Answer = MsgBoxResult.No Then
                    e.Cancel = True
                Else
                    e.Cancel = False
                    ShowStatus("")
                    iIsDirty = False

                    'DJW 2/12/13
                    ''iRecordLock.Clear()
                End If
            End If
            If e.Cancel = False Then
                'DJW 2/12/13
                ''iRecordLock.Clear()

                iEP.Dispose()

                Try
                    '--- HOT FIX 5.1.10
                    RaiseEvent OnLeaveRecord()
                    '--- END HOT FIX 5.1.10
                Catch ex As Exception
                    ShowError("Unexpected error leaving record (feMain OnLeaveRecord)", ex)
                End Try

            End If

        End Sub

        ' GBV 8/7/2015
        Private Sub feMain2_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
            ' GBV 8/7/2015 - Ticket 2439 - check all siblings and attempt to tranfer lock
            If Me.MdiParent IsNot Nothing Then
                For Each F As Form In Me.MdiParent.MdiChildren
                    If TypeOf (F) Is feMain2 AndAlso Not CType(F, fBase).iIsClosed Then
                        CType(F, feMain2).SetFormAttributes()
                    End If
                Next
            End If
        End Sub
#End Region

#Region "---------------------------- Link To Other Forms ------------------------"
        ''' <summary> User double-clicks a line in the GV.  Try to open appropriate form. </summary>
        Private Sub gvDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
            Dim GV As qGVBase = TryCast(sender, qGVBase)
            If GV IsNot Nothing Then
                If GV._ListEditFormName = "none" Or GV._ListEditFormName = "" Then Return
                If GV.SelectedRows.Count > 0 Then
                    iKey = GetKey(CType(GV, DataGridView), GV.SelectedRows(0).Index)
                    OpenEditFormFromList(GV._ListEditFormName, iKey, CType(GV, DataGridView))
                End If
            End If
        End Sub

        ''' <summary> User clicks in a GV cell.  If it is a linkcell, try to open appropriate form </summary>
        Sub GVContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
            Dim GV As qGVBase = TryCast(sender, qGVBase)
            If GV IsNot Nothing Then
                If GV._ListEditFormName = "none" Or GV._ListEditFormName = "" Then Return
                If GV.Columns(e.ColumnIndex).CellType.ToString.ToLower.IndexOf("datagridviewlinkcell") > -1 Then
                    If GV.SelectedRows.Count > 0 Then
                        iKey = GetKey(CType(GV, DataGridView), GV.SelectedRows(0).Index)
                        OpenEditFormFromList(GV._ListEditFormName, iKey, CType(GV, DataGridView))
                    End If
                End If
            End If

        End Sub

        '' <summary> User double-clicks UG row.  Try to open appropriate form </summary>
        'Private Sub UGDoubleClick(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.DoubleClickCellEventArgs)
        '    Dim UG As qUG = TryCast(sender, qUG)
        '    If UG IsNot Nothing Then
        '        If UG._ListEditFormName.Length = 0 Or UG._ListEditFormName = "none" Then Return
        '        If e.Cell.Row.Index > -1 Then
        '            iKey = GetKey(UG, e.Cell.Row.Index)
        '            OpenEditFormFromList(UG._ListEditFormName, iKey, Nothing, UG)
        '        End If
        '    End If
        'End Sub

        '' <summary> User clicks in a UG cell.  If it is a URL cell, try to open appropriate form </summary>
        'Private Sub UGContentClick(ByVal sender As Object, ByVal e As System.EventArgs)
        '    Dim UG As qUG = TryCast(sender, qUG)
        '    If UG IsNot Nothing Then
        '        If UG._ListEditFormName = "none" Or UG._ListEditFormName = "" Then Return
        '        If UG.ActiveCell.Row.Index > -1 Then
        '            If UG.ActiveCell.Column.Style = ColumnStyle.URL Then
        '                iKey = GetKey(UG, UG.ActiveCell.Row.Index)
        '                OpenEditFormFromList(UG._ListEditFormName, iKey, Nothing, UG)

        '                'BHS 2/29/08 point to next cell so we can activate this one again
        '                Dim i As Integer = UG.ActiveCell.Column.Index + 1
        '                If i < UG.DisplayLayout.Bands(0).Columns.Count Then
        '                    UG.ActiveCell = UG.ActiveCell.Row.Cells(i)
        '                End If

        '            End If
        '        End If
        '    End If
        'End Sub

#End Region

#Region "---------------------------- Misc Other Functions -----------------------"

        Private Sub btnDefault_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDefault.Click
            SQLDoSQL("Update UserMain Set DefaultObject = '" & Me.Name & "' Where UserID = '" & gUserName & "'", True)
        End Sub

        Private Sub btnSQL_Click(sender As System.Object, e As System.EventArgs) Handles btnSQL.Click
            Try
                RaiseEvent OnShowSQL()
            Catch ex As Exception
                '...do nothing
            End Try
        End Sub

        ' GBV - 10/22/2014 - Ticket 2439. Transfers a lock to this object/record
        Private Sub btnTxLock_Click(sender As System.Object, e As System.EventArgs) Handles btnTxLock.Click
            If MsgBoxQuestion("Do you wish to transfer the lock to this function?") <> MsgBoxResult.Yes Then Return
            ''If iRecordLock IsNot Nothing Then
            ''iRecordLock.Clear()
            'iLockTransferred = True
            SetFormAttributes()
                ' GBV 8/7/2015 - Ticket 2439 - check all siblings 
                For Each F As Form In Me.MdiParent.MdiChildren
                    If TypeOf (F) Is feMain2 AndAlso _
                       Not CType(F, fBase).iIsClosed AndAlso _
                       CType(F, fBase).FormID <> Me.FormID Then
                    '' CType(F, fBase).iRecordLock.ResetLockFlags(False)
                    CType(F, feMain2).SetFormAttributes()
                    End If
                Next
            ''End If

        End Sub
        ' GBV - 10/22/2014 - Ticket 2439 - Unlocks record
        Private Sub btnUnlock_Click(sender As System.Object, e As System.EventArgs) Handles btnUnlock.Click
            ''If iRecordLock IsNot Nothing Then
            If MsgBox("Please confirm that you wish to unlock this record.", MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                '' iRecordLock.Clear()
                SetFormAttributes()
                    ' GBV 8/7/2015 - Ticket 2439 - check all siblings 
                    For Each F As Form In Me.MdiParent.MdiChildren
                        If TypeOf (F) Is feMain2 AndAlso _
                           Not CType(F, fBase).iIsClosed AndAlso _
                           CType(F, fBase).FormID <> Me.FormID Then
                        '' CType(F, fBase).iRecordLock.ResetLockFlags(False)
                        CType(F, feMain2).SetFormAttributes()
                        End If
                    Next
                End If
            ''End If
        End Sub

#End Region

    End Class

End Namespace