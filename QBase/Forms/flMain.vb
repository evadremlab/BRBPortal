'Imports Infragistics.Win.UltraWinGrid
Imports System.Data.SQLClient

Namespace Windows.Forms

#Region "---------------------------- Documentation ------------------------------"

    'flMain is the primary list Class.  Application list forms will be derived from it, or from intermediate classes derived from flMain.  flMain itself derives from fBase, which defines instance variables, events, and global functions used by all forms.

    'Load
    '   Me.SetupForm() sets up buttons
    '   fBase.SetContext() collects info from child
    '      OnAfterSetContext() confirms that required info has been defined
    '                       also adds list event handlers
    '   fBase.Query() calls OnQuery in the child

    'Query
    '   fBase.Query() rasies OnQuery
    '   child.OnQuery sets up SQL, and may call flMain.BuildQuery to populate Where Clause
    '      from controls in pnlQuery
    '   DEBUG - need to figure out how to apply SQL to iListGV

    'Link To Edit Form
    '   Can be started with Add button, or with click on List link cell or double-click on List row
    '   fBase.GetKey returns key information for this list row  (event can override in child)
    '   flMain.OpenEditForm Opens Edit Form with references back to this list object

    'Find fires every time the user enters or removes a letter from the Find textbox.  It filters out
    '   all rows that don't have a displayed cell that matches the find text.  It doesn't change the
    '   list select population - only which rows display.  

#End Region

    Public Class flMain

        Public iLoaded As Boolean = True
        Public iCallingFP As fpMainVersion = Nothing  'SDC 3/18/09
        Public iCallingFP2 As fpMain2 = Nothing     'SDC 7/22/14
        Public icallingFPNovers As fpMain = Nothing  'SDC 3/18/09 (probably not used)
        Public iCallingFS As fsMain = Nothing       'BHS 2/9/09
        Protected iKeepFSOpen As Boolean = False
        'Allow programmer to distinguish when user hit button, and when Query is programatically called from another object
        Protected iSearchButtonHit As Boolean = False
        Public Event OnSetEditForm(ByRef aFrm As feMain)
        Protected Event OnFind(ByRef aActedOn As Boolean)
        Protected Event OnAfterFind()
        Protected Event OnClearControl()
        Protected Event OnLinkToEditForm(ByRef aActedOn As Boolean, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)


#Region "_EditFormName Property points to Edit Form to instantiate when user drills down in list"
        Private _EditFormNameVar As String = String.Empty
        <System.ComponentModel.Category("Data"), _
                System.ComponentModel.Description("The name of the Edit Form object to open when the user double-clicks a list line."), _
                System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
                System.ComponentModel.DefaultValue("")> _
                Public Overridable Property _EditFormName() As String
            Get
                Return _EditFormNameVar
            End Get
            Set(ByVal value As String)
                _EditFormNameVar = value
            End Set
        End Property
#End Region

#Region "---------------------------- Load ------------------------------"

        '''<summary> Load form  (errors handled in descendant program </summary>
        Private Sub flMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                iMode = "List"
                iLoaded = False
                '...disable the <Cancel> button in the fs, if appropriate
                If iCallingFS IsNot Nothing Then
                    iCallingFS.btnCancel.Enabled = False
                End If


                SetupForm()
                If Not SetContext() Then CloseTimer.Start() 'Prob loading; user already notified
                FillQBEDefaults()

                'BHS 4/9/10
                SetGVLayout()

                Me.Refresh()
            Catch ex As Exception
                ShowError("Error setting up form", ex)
                Close() 'Close form
            End Try
        End Sub

        '''<summary> Form gets focus </summary>
        Private Sub flMain_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
            If Not iLoaded Then
                Try
                    LoadForm()
                    SetGVProperties(iGVs, "list")
                    Query()
                    iLoaded = True
                Catch ex As Exception
                    ShowError("Error loading form", ex)
                    Return  'Not fatal
                End Try
            End If

            SetMenu()
        End Sub

        '''<summary> Setup buttons, etc. </summary>
        Sub SetupForm()

            Dim toolTip1 As New ToolTip()
            SetToolTipProperties(toolTip1)

            '08/24/2009 SDC  Commented out so we can use tooltip1 for each control
            ' Set up the ToolTip text for the Button and Checkbox.
            'toolTip1.SetToolTip(Me.btnQuery, "Search and redisplay list based on what you've entered")
            'toolTip1.SetToolTip(Me.BtnAdd, "Add a new record")

            SetColorScheme()    'Default fore and backcolors
            StatusStrip1.BackColor = QBackColor

        End Sub


        '''<summary> Add handlers for standard control events </summary>
        Private Sub flMain_OnAfterSetContext(ByRef aOK As Boolean) Handles Me.OnAfterSetContext
            Dim GV As DataGridView
            Dim C As Control

            'Handle List events in this base class
            For Each C In Me.Controls
                HandleListControl(C)
            Next

            'If fBase.ActiveForm IsNot Nothing Then  'Do this only at runtime
            If Not InDevEnv() Then
                For Each GV In iGVs 'Only do GVs marked by the programmer for event handling
                    AddHandler GV.CellDoubleClick, AddressOf GVDoubleClick
                    AddHandler GV.CellContentClick, AddressOf GVContentClick
                Next
                'For Each UG As UltraGrid In iUGs    'Only do UGs marked by the programmer for event handling
                '    AddHandler UG.DoubleClickCell, AddressOf UGDoubleClick
                '    AddHandler UG.AfterCellActivate, AddressOf UGContentClick
                'Next
            End If

            aOK = True
        End Sub

        '''<summary> Add handlers for standard single-entry control events </summary>
        Sub HandleListControl(ByVal aC As Control)

            If aC.GetType.Name.ToLower = "textbox" Or aC.GetType.Name.ToLower = "combobox" Or aC.GetType.Name.ToLower = "qtextbox" Or aC.GetType.Name.ToLower = "qmaskedtextbox" Or aC.GetType.Name.ToLower = "qcombobox" Or aC.GetType.Name.ToLower = "qdd" Then  'All textboxes
                'BHS 8/14/09 - these handlers are set up in fBase.  Invoking them here makes the logic run twice (which slows repainting)
                'AddHandler aC.Validating, AddressOf EntryControl_Validating
                'AddHandler aC.Validated, AddressOf EntryControl_Validated
                'AddHandler aC.Enter, AddressOf EntryControl_Enter
                'AddHandler aC.Leave, AddressOf EntryControl_Leave
            End If

            'Recursive
            Dim C As Control
            For Each C In aC.Controls
                HandleListControl(C)
            Next

        End Sub

#End Region

#Region "---------------------------- Command Buttons and Menu  ------------------------------"

        '''<summary> Add Button - opens edit form (flMain.OpenEditForm) </summary>
        Private Sub BtnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAdd.Click
            Try
                iKey = ""
                OpenEditForm("")
            Catch ex As Exception
                ShowError("Error in edit form", ex)
                Return  'Not fatal
            End Try
        End Sub

        '''<summary> Query/Search button Calls fBase.Query() which raises onQuery event in derived class </summary>
        Private Sub btnQuery_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuery.Click
            Try
                iSearchButtonHit = True
                Query()
                iSearchButtonHit = False
            Catch ex As Exception
                ShowError("Error running query", ex)
                Return  'Not fatal
            End Try
        End Sub

        '''<summary> Save QBE fields to database </summary>
        Private Sub tsSaveQuery_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsSaveQuery.Click
            Try
                UpdateQueryVersion()
            Catch ex As Exception
                ShowError("Problem saving Query", ex)
            End Try
        End Sub

        ''' <summary> Save Gridview Column order and width to database </summary>
        Private Sub tsSaveLayout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsSaveLayout.Click
            Try
                UpdateLayoutVersion()
            Catch ex As Exception
                ShowError("Problem saving Layout", ex)
            End Try
        End Sub

        '''<summary> User clicks something in the menu </summary>
        Overrides Function ReceiveMenuClick(ByVal aMenuItemName As String, ByVal aMenuItemTag As String) As Boolean
            If aMenuItemTag.ToLower = "btnquery" Then btnQuery.PerformClick()
            If aMenuItemTag.ToLower = "btnadd" Then BtnAdd.PerformClick()
            If aMenuItemTag.ToLower = "btnreport" Then btnReport.PerformClick()
            If aMenuItemTag.ToLower = "bnfirst" Then bnFirst.PerformClick()
            If aMenuItemTag.ToLower = "bnprev" Then bnPrev.PerformClick()
            If aMenuItemTag.ToLower = "bnnext" Then bnNext.PerformClick()
            If aMenuItemTag.ToLower = "bnlast" Then bnLast.PerformClick()
            SetMenu()
        End Function

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
            'BHS 8/17/09 Only change message if there is a change - this avoids unneeded screen paints
            If StringCompare(ProgressMsg.Text, aMessage) = False Then
                ProgressMsg.Text = aMessage
                Refresh()
            End If
            Me.Cursor = Cursors.Arrow
            If aShowHourGlass Then Me.Cursor = Cursors.WaitCursor
        End Function

        '''<summary> Show Help message </summary>
        Overrides Function ShowHelp(ByVal aMessage As String) As Boolean
            'BHS 8/17/09 Only change message if there is a change - this avoids unneeded screen paints
            If StringCompare(HelpMsg.Text, aMessage) = False Then
                HelpMsg.Text = Mid(aMessage, 1, 180)
                Refresh()
            End If

            Return True
        End Function

        '''<summary> Show or hide Query Save button in toolbar </summary>
        Sub ShowSave(ByVal aOK As Boolean)
            tsSaveQuery.Visible = aOK
        End Sub

#End Region

#Region "---------------------------- Link To Edit Form  ------------------------------"
        'These routines assume that we're connecting to Me._EditFormName from iListGV
        'See feMain parallel routines for a more general solution, allowing linking from multiple lists
        '''<summary> GV double click links to Edit Form, passing iKey </summary>
        Sub GVDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
            Dim ActedOn As Boolean = False
            Try
                RaiseEvent OnLinkToEditForm(ActedOn, e)
            Catch ex As Exception
                ShowError("Unexpected error opening edit form (flMain OnLinkToEditForm)", ex)
            End Try

            If ActedOn Then Return
            If Me._EditFormName.ToLower = "none" Then Return
            If iListGV.SelectedRows.Count > 0 Then
                If iListGV.SelectedRows.Count > 0 AndAlso iListGV.SelectedRows(0) IsNot Nothing Then   'BHS 3/12/08
                    iKey = GetKey(iListGV, iListGV.SelectedRows(0).Index)
                    OpenEditForm(iKey)
                End If
            End If
        End Sub

        '''<summary> GV Click on link column links to Edit Form, passing iKey </summary>
        Sub GVContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
            Dim ActedOn As Boolean = False
            Try
                RaiseEvent OnLinkToEditForm(ActedOn, e) ' GBV 1/11/2011 moved lines here
            Catch ex As Exception
                ShowError("Unexpected error opening edit form (flMain OnLinkToEditForm)", ex)
            End Try

            If ActedOn Then Return
            If e.ColumnIndex < 0 OrElse e.ColumnIndex > iListGV.Columns.Count - 1 Then Return ' GBV 11/10/09
            If iListGV.Columns(e.ColumnIndex).CellType.ToString.ToLower.IndexOf("datagridviewlinkcell") < 0 Then Return
            If e.RowIndex < 0 Then Return 'BHS 10/7/08
            'RaiseEvent OnLinkToEditForm(ActedOn, e) ' GBV 1/11/2011 moved lines up 
            'If ActedOn Then Return
            If Me._EditFormName.ToLower = "none" Then Return
            If iListGV.Columns(e.ColumnIndex).CellType.ToString.ToLower.IndexOf("datagridviewlinkcell") > 0 Then
                'DJW 1/16/13 Confirm there is a row before examining it
                If iListGV.SelectedRows.Count > 0 AndAlso iListGV.SelectedRows(0) IsNot Nothing Then   'BHS 3/12/08
                    iKey = GetKey(iListGV, iListGV.SelectedRows(0).Index)
                    OpenEditForm(iKey)
                End If
            End If
        End Sub

        ''<summary> UG double click links to Edit Form, passing iKey </summary>
        'Private Sub UGDoubleClick(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.DoubleClickCellEventArgs)
        '    Dim UG As qUG = TryCast(sender, qUG)
        '    If UG IsNot Nothing Then
        '        If e.Cell.Row.Index > -1 Then
        '            If UG._ListEditFormName.Length = 0 Or UG._ListEditFormName = "none" Then Return
        '            iKey = GetKey(UG, e.Cell.Row.Index)
        '            OpenEditFormFromList(UG._ListEditFormName, iKey, Nothing, UG)
        '        End If

        '    End If
        'End Sub

        ''<summary> UG URL column click links to Edit Form, passing iKey </summary>
        'Private Sub UGContentClick(ByVal sender As Object, ByVal e As System.EventArgs)
        '    Dim UG As qUG = TryCast(sender, qUG)
        '    If UG IsNot Nothing Then
        '        If UG.ActiveCell.Row.Index > -1 Then
        '            If UG.ActiveCell.Column.Style = ColumnStyle.URL Then
        '                If UG._ListEditFormName.Length = 0 Or UG._ListEditFormName = "none" Then Return
        '                iKey = GetKey(UG, UG.ActiveCell.Row.Index)
        '                OpenEditFormFromList(UG._ListEditFormName, iKey, Nothing, UG)
        '            End If
        '        End If
        '    End If
        'End Sub

        '''<summary> Open Edit Form, assumes iListGV has a selected row. </summary>
        Sub OpenEditForm()
            iKey = GetKey(iListGV, iListGV.SelectedRows(0).Index)
            If Me._EditFormName > "" Then
                'BHS 9/17/07 Allow multiple app assemblies
                Dim F As fBase = GetEditForm(Me._EditFormName)
                'CreateFormByName(Appl.AssemblyName, Me._EditFormName)
                If F IsNot Nothing Then
                    F.MdiParent = Me.MdiParent
                    F.Show()
                Else
                    MsgBoxErr("Programmer Error", "Bad _EditFormName (flMain::ListCellDoubleClick)")
                End If
            Else
                OpenEditForm(iKey)
            End If
        End Sub

        '''<summary> Stub for child function to set the edit form if Me._EditFormName is not specified </summary>
        Overridable Function SetEditForm() As feMain
            Return New feMain   'BHS 11/14/06
        End Function


        '''<summary> Open Edit Form (special case for List form - see general case in fBase) </summary>
        Overridable Function OpenEditForm(ByVal aKey As String) As Boolean
            Dim frm As fBase
            If Me._EditFormName > "" Then
                frm = GetEditForm(Me._EditFormName)
                'BHS 9/17/07 CreateFormByName(Appl.AssemblyName, Me._EditFormName)
                If frm Is Nothing Then
                    MsgBoxErr("Programmer Error", "Bad _EditFormName (flMain::ListCellDoubleClick)")
                    Return False
                End If
            Else
                frm = SetEditForm()
            End If

            'Make sure edit form is defined in derived class
            If frm.GetType.ToString.ToLower = "qsilib.windows.forms.femain" Then
                '...let's not display message, because that interferes when there IS not edit form!
                'MsgBoxErr("Programmer Error", "List Window _EditFormName is missing")
                Return False
            End If

            frm.iKey = aKey
            frm.iListForm = Me
            frm.MdiParent = Me.MdiParent()
            Try
                frm.Show()
            Catch ex As Exception
                ShowError("Error in Edit Window", ex)
                Return False
            End Try


            'frm.LoadForm()
            Return True

        End Function

        ''' <summary> 'Default behavior is to run a query after a Save of a New record.  
        ''' If this takes too long, override this
        '''function in the descendant and provide different behavior </summary>
        Overrides Function QueryAfterNewRecordSave() As Boolean

            'BHS 4/17/13 Don't run Query if there are no rows in the iListGV
            If iListGV IsNot Nothing Then
                If iListGV.RowCount < 1 Then Return False
            End If

            ' GBV 5/28/2015 - Give time to all commits to complete
            Threading.Thread.Sleep(500)

            Query()
            'SRM 10/5/11 -- apply filter after adding records
            Find1()
        End Function

        '''<summary> Synch up List when user enters a New key on Edit form that matches an existing record
        '''This function only gets called if there is no matching function in the child </summary>
        Overrides Function SelectListRow(ByVal aKey As String) As Boolean
            Dim R As DataGridViewRow
            Dim C As DataGridViewColumn
            Dim DV As New DataView
            Dim SQL As String = SelectRowValues(aKey)   'Collect DB values for changed row
            If SQL = "bad" Then Return False
            If SQL.Length > 0 Then

                ' GBV 5/28/2015 - Give time to all commits to complete
                Threading.Thread.Sleep(500)

                'BHS 5/2/08 Allow calls to SQLBuildDV if iConnType has been set
                'iConnType can be set in iSetContext in the client, and if it is set it overrides Appl.ConnType.
                If iConnType = "SQL" Then
                    Dim cn As New SqlConnection(gSQLConnStr)
                    DV = SQLBuildDV(SQL, cn, False, "str")
                Else
                    DV = BuildDV(SQL, False)
                End If
            Else
                ProgrammerErr("Need Function SelectRowValues in child list form")
            End If

            If iListGV IsNot Nothing Then
                For Each R In iListGV.Rows
                    If aKey = GetKey(iListGV, R.Index) Then
                        ' BHS Try taking out 12/11/06 iListGV.Rows(R.Index).Cells(1).Selected = True

                        For Each C In R.DataGridView.Columns
                            If DV.Count >= 1 Then
                                'XXX Do some testing about whether the DataPropertyName exists in the DV?
                                Try
                                    If C.DataPropertyName > "" Then
                                        'BHS 7/6/10
                                        If R.DataGridView IsNot Nothing Then
                                            R.Cells(C.Name).Value = DV(0)(C.DataPropertyName)
                                        End If
                                    End If
                                Catch ex As Exception
                                    ShowError("Error writing to List after Save (flMain.SelectListRow)", ex)
                                End Try

                            Else
                                'BHS 7/8/08
                                'Changes in the record may exclude it from the records listed in the Query, so we need to be silent here
                                'ProgrammerErr("Need a single row return in SelectRowValues: " & iListGV.Name & "; Key = " & aKey)
                                'Return False
                            End If
                        Next
                    End If

                Next
            Else
                'If iListUG IsNot Nothing Then
                '    For Each UR As UltraGridRow In iListUG.Rows
                '        If aKey = GetKey(iListUG, UR.Index) Then
                '            For Each UC As UltraGridCell In UR.Cells
                '                If DV.Count >= 1 Then
                '                    If UC.Column.Key > "" Then
                '                        'Skip Chaptered column types which describe relationships
                '                        If UC.Column.DataType.ToString.IndexOf("Chaptered") = -1 Then
                '                            UC.Value = DV(0)(UC.Column.Key)
                '                        End If
                '                    End If
                '                Else
                '                    'Changes in the record may exclude it from the records listed in the Query, so we need to be silent here
                '                    'ProgrammerErr("Need a single row return in SelectRowValues")
                '                    'Return False
                '                End If
                '            Next
                '        End If
                '    Next
                'End If
            End If

            UpdateListRows()

            Return True

        End Function


        '''<summary> Allows programmer to update the grid list with full names or descriptions </summary>
        Overridable Sub UpdateListRows()

        End Sub


        '''<summary> Create an SQL that will collect current DB values for the changed row
        ''' Aassumes all key fields are strings
        ''' Override in child if these assumptions don't work </summary>
        Public Overrides Function SelectRowValues(ByVal aKey As String) As String
            Dim KeyFields As String = ""
            Dim KeyFieldsTable As String = ""

            If iListGV IsNot Nothing Then
                If iListGV.Tag IsNot Nothing Then KeyFields = iListGV.Tag.ToString()
                If TypeOf (iListGV) Is qGVBase Then
                    Dim qGV As qGVBase = CType(iListGV, qGVBase)
                    If qGV._KeyFields IsNot Nothing Then KeyFields = qGV._KeyFields
                    If qGV._KeyFieldsTable IsNot Nothing AndAlso qGV._KeyFieldsTable <> "" Then
                        KeyFieldsTable = qGV._KeyFieldsTable & "."
                    End If
                End If
            Else
                'If iListUG IsNot Nothing Then
                '    KeyFields = iListUG._KeyFields
                'End If
            End If

            Dim FieldName As String = ParseStr(KeyFields, "|")
            Dim KeyValue As String = ParseStr(aKey, "|")
            Dim C As DataGridViewColumn
            Dim SQL As String = iListSQL

            If iListSQL <= " " Then
                'SRM 11/22/10 --don't show error because user may never have done a query (Oakland already had this)
                'ProgrammerErr("iListSQL not set in descendant")
                Return "bad"
            End If

            Do While FieldName > " "
                'Get Field Type from iListGV Column
                If iListGV IsNot Nothing Then
                    For Each C In iListGV.Columns
                        If C.Name = FieldName Then

                            'BHS 8/12/10 added check for type
                            Dim Type As String = "str"
                            Dim T As qGVTextBoxColumn = TryCast(C, qGVTextBoxColumn)
                            If T IsNot Nothing Then
                                If T._DataType = DataTypeEnum.Dat Then Type = "dat"
                                If T._DataType = DataTypeEnum.Num Then Type = "num"
                            End If
                            SQL = AddWhere(SQL, KeyFieldsTable & C.DataPropertyName, KeyValue, Type)
                            GoTo FoundIt
                        End If
                    Next
                Else
                    'If iListUG IsNot Nothing Then
                    '    For Each UC As UltraGridColumn In iListUG.DisplayLayout.Bands(0).Columns
                    '        If UC.Key = FieldName Then
                    '            SQL = AddWhere(SQL, UC.Key, KeyValue, "S")
                    '            GoTo FoundIt
                    '        End If
                    '    Next
                    'End If
                End If
                If iListGV.CurrentRow Is Nothing Then
                    Return "bad"
                Else
                    ProgrammerErr("Key Field doesn't match a Column Name or Column Type not tested for")
                End If

FoundIt:
                'Get next name/value pair in key
                FieldName = ParseStr(KeyFields, "|")
                KeyValue = ParseStr(aKey, "|")
            Loop

            Return SQL
        End Function

#End Region

#Region "---------------------------- Find ------------------------------------"
        '''<summary> Allow descendant to turn off find area </summary>
        Public Function SetFindVisible(ByVal aIsVisible As Boolean) As Boolean
            lblFind.Visible = aIsVisible
            txtFind.Visible = aIsVisible
            Return True
        End Function

        '''<summary> Allow descendant to turn on find2 area </summary>
        Public Function SetFind2Visible(ByVal aIsVisible As Boolean) As Boolean
            lblFind2.Visible = aIsVisible
            txtFind2.Visible = aIsVisible
            Find2Separator.Visible = aIsVisible
            Return True
        End Function

        '''<summary> Allow descendant to turn on find3 area </summary>
        Public Function SetFind3Visible(ByVal aIsVisible As Boolean) As Boolean
            lblFind3.Visible = aIsVisible
            txtFind3.Visible = aIsVisible
            Find3Separator.Visible = aIsVisible
            Return True
        End Function

        '''<summary> Allow descendant to set Find Prompt </summary>
        Public Function SetFindPrompt(ByVal aPrompt As String) As Boolean
            lblFind.Text = aPrompt
        End Function

        '''<summary> Allow descendant to set Find Prompt 2 </summary>
        Public Function SetFind2Prompt(ByVal aPrompt As String) As Boolean
            lblFind2.Text = aPrompt
        End Function

        '''<summary> Allow descendant to set Find Prompt 3 </summary>
        Public Function SetFind3Prompt(ByVal aPrompt As String) As Boolean
            lblFind3.Text = aPrompt
        End Function

        '''<summary> Execute filter every time user changes the Find text </summary>
        Private Sub txtFind_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtFind.KeyUp
            Try
                'BHS 4/3/09 remove asterisks to avoid find syntax problems
                Dim start As Integer = txtFind.SelectionStart
                If txtFind.Text.IndexOf("*") > -1 Then
                    SubstituteAllStr(txtFind.Text, "*", "")
                    If start > 1 Then start = start - 1 'Move cursor back one since asterisk was removed
                    txtFind.SelectionStart = start
                End If

                'BHS 9/14/12 remove [ to avoid find syntax problems
                start = txtFind.SelectionStart
                If txtFind.Text.IndexOf("[") > -1 Then
                    SubstituteAllStr(txtFind.Text, "[", "")
                    If start > 1 Then start = start - 1 'Move cursor back one since asterisk was removed
                    txtFind.SelectionStart = start
                End If

                'BHS 9/14/12 remove [ to avoid find syntax problems
                start = txtFind.SelectionStart
                If txtFind.Text.IndexOf("]") > -1 Then
                    SubstituteAllStr(txtFind.Text, "]", "")
                    If start > 1 Then start = start - 1 'Move cursor back one since asterisk was removed
                    txtFind.SelectionStart = start
                End If


                Find1()
            Catch ex As Exception
                ShowError("Error filtering list data", ex)
                Return  'Not fatal
            End Try
        End Sub

        '''<summary> Also execute filter every time user changes the columns to be searched </summary>
        Private Sub cbColumns_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbColumns.SelectedIndexChanged
            Try
                Find1()
            Catch ex As Exception
                ShowError("Error filtering list data", ex)
                Return  'Not fatal
            End Try
        End Sub

        'Find/columns logic
        Overrides Sub Find1()
            Dim ActedOn As Boolean = False

            ShowProgress("", True)  'BHS 9/29/09 Show hourglass

            Try
                RaiseEvent OnFind(ActedOn)  'Let child have chance to handle event
            Catch ex As Exception
                ShowError("Unexpected error filtering list (OnFind)", ex)
            End Try


            If ActedOn = False Then
                'BHS 9/3/08 Column-specific filtering
                If cbColumns.Visible = True Then
                    If cbColumns.ComboBox.SelectedValue.ToString = "" Or _
                        cbColumns.ComboBox.SelectedValue.ToString = "all" Then
                        iListNavigator.BindingSource.Filter = BuildGVFilter(iListGV, txtFind.Text)
                    Else
                        iListNavigator.BindingSource.Filter = _
                           BuildGVColFilter("", iListGV, iListGV.Columns(cbColumns.ComboBox.SelectedValue.ToString), Clean(txtFind.Text))
                    End If
                Else
                    'BHS 6/26/12 Test that there is an iListGV
                    If iListGV IsNot Nothing AndAlso _
                       Not iListGV.IsDisposed Then iListNavigator.BindingSource.Filter = BuildGVFilter(iListGV, txtFind.Text)
                End If

                ShowProgress("", False)  'BHS 9/29/09 Turn off hourglass

                Try
                    RaiseEvent OnAfterFind()
                Catch ex As Exception
                    ShowError("Unexpected error after list filtering (OnAfterFind)", ex)
                End Try

            End If
        End Sub

        '''<summary> Execute filter every time user changes the Find text </summary>
        Private Sub txtFind2_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtFind2.KeyUp
            Try
                Dim ActedOn As Boolean = False
                Try
                    RaiseEvent OnFind(ActedOn)  'Let child have chance to handle event
                Catch ex As Exception
                    ShowError("Unexpected error filtering list (OnFind)", ex)
                End Try

                If ActedOn = False Then
                    If Not iListGV.IsDisposed Then iListNavigator.BindingSource.Filter = BuildGVFilter(iListGV, txtFind.Text)
                    Try
                        RaiseEvent OnAfterFind()
                    Catch ex As Exception
                        ShowError("Unexpected error after list filtering (OnAfterFind)", ex)
                    End Try

                End If
            Catch ex As Exception
                ShowError("Error filtering list data", ex)
                Return  'Not fatal
            End Try
        End Sub

        '''<summary> Execute filter every time user changes the Find text </summary>
        Private Sub txtFind3_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtFind3.KeyUp
            Try
                Dim ActedOn As Boolean = False
                Try
                    RaiseEvent OnFind(ActedOn)  'Let child have chance to handle event
                Catch ex As Exception
                    ShowError("Unexpected error filtering list (OnFind)", ex)
                End Try


                If ActedOn = False Then
                    If Not iListGV.IsDisposed Then iListNavigator.BindingSource.Filter = BuildGVFilter(iListGV, txtFind.Text)
                    Try
                        RaiseEvent OnAfterFind()
                    Catch ex As Exception
                        ShowError("Unexpected error after list filtering (OnAfterFind)", ex)
                    End Try
                End If
            Catch ex As Exception
                ShowError("Error filtering list data", ex)
                Return  'Not fatal
            End Try
        End Sub

        ''' <summary> Fill flMain cbColumns combobox based on the visible columns in aGV </summary>
        Sub FillcbColumnsFromGVColumns(ByVal aGV As DataGridView)
            Dim wstr As String = "<All Columns>=all"

            For Each C As DataGridViewColumn In aGV.Columns
                If C.Visible = True And C.DataPropertyName.Length > 0 And Mid(C.Name.ToLower, 1, 7) <> "workcol" Then
                    wstr &= "," & C.HeaderText & "=" & C.DataPropertyName
                End If
            Next
            '...remove linefeed characters
            wstr = Replace(wstr, Chr(10), " ")

            FillComboBox(wstr, cbColumns)

            lblOnColumn.Visible = True
            cbColumns.Visible = True

        End Sub

#End Region

#Region "---------------------------- Obsolete ------------------------------"

        'Function SetFocusControl(ByVal ControlName As String)
        '    Dim C As Control = FindControl(ControlName, Me)

        '    If C.Name = ControlName Then
        '        C.Focus()
        '        Return True
        '    Else
        '        Return False
        '    End If

        'End Function


        ''Ignore validation problems if user is closing form
        'Private Sub flMain_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        '    iEP.Dispose()
        '    e.Cancel = False
        'End Sub

        ''Create aFrm object based on child
        'Sub SetEditForm(ByVal aFrm As feMain)
        '    RaiseEvent OnSetEditForm(aFrm)
        'End Sub


        'Private Sub txtFind_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtFind.KeyUp
        '    GVFind(iListGV, txtFind.Text)

        'End Sub

        'Public Sub GVFind(ByVal aGV As DataGridView, ByVal aSearchStr As String)
        '    Dim Str As String = aSearchStr.ToUpper
        '    Dim C As DataGridViewColumn
        '    Dim Filter As String = ""

        '    For Each C In aGV.Columns
        '        'Someday it would be nice to be able to filter on number and date columns, reduced to strings
        '        If Not IsNothing(C.ValueType) Then
        '            If C.Displayed And C.ValueType.Name.ToLower = "string" Then
        '                If Filter > " " Then Filter += " Or "
        '                Filter += C.DataPropertyName + " Like '%" + Str + "%'"
        '            End If
        '        End If
        '    Next

        '    iListNavigator.BindingSource.Filter = Filter
        'End Sub
#End Region

#Region "-------------------- Buttons and Form Close ----------------------------"

        Private Sub btnDefault_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDefault.Click
            SQLDoSQL("Update UserMain Set DefaultObject = '" & Me.Name & "' Where UserID = '" & gUserName & "'", True)
        End Sub

        Private Sub flMain_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
            If iCallingFS IsNot Nothing Then
                '...close the FS if appropriate
                'NOTE: Closing the FS will trigger the reinstatement of the fs Controlbox and <Run> button!
                If iKeepFSOpen = False Then iCallingFS.Close()
            End If
        End Sub

        Private Sub btnClear1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear1.Click
            'Call fbase function to clear all Query type fields 
            ClearQueryControls(Me, True)
            txtFind.Text = ""
            iListSQL = ""
            'Raise event to allow program to clear list and set defauilts
            Try
                RaiseEvent OnClearControl()
            Catch ex As Exception
                ShowError("Unexpected error clearing controls (OnClearControl)", ex)
            End Try

            Post("EndOfClear")
        End Sub

#End Region

    End Class

End Namespace