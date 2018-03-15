Imports Infragistics.Win.UltraWinGrid
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

    Public Class flMaintest

        Public iLoaded As Boolean = True
        Public iCallingFP As fpMainVersion = Nothing  'SDC 3/18/09
        Public icallingFPNovers As fpMain = Nothing  'SDC 3/18/09 (probably not used)
        Public iCallingFS As fsMain = Nothing       'BHS 2/9/09
        Protected iKeepFSOpen As Boolean = False
        'Allow programmer to distinguish when user hit button, and when Query is programatically called from another object
        Protected iSearchButtonHit As Boolean = False
        Public Event OnSetEditForm(ByRef aFrm As feMain)
        Protected Event OnFind(ByRef aActedOn As Boolean)
        Protected Event OnAfterFind()
        Protected Event OnLinkToEditForm(ByRef aActedOn As Boolean, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
        'Friend WithEvents BindingNavigatorCountItem As New System.Windows.Forms.ToolStripLabel
        'Friend WithEvents BindingNavigatorSeparator As New System.Windows.Forms.ToolStripSeparator
        'Friend WithEvents BindingNavigatorPositionItem As New System.Windows.Forms.ToolStripTextBox
        'Friend WithEvents BindingNavigatorSeparator1 As New System.Windows.Forms.ToolStripSeparator
        'Friend WithEvents BindingNavigatorSeparator2 As New System.Windows.Forms.ToolStripSeparator
        'Friend WithEvents BindingNavigatorDeleteItem As New System.Windows.Forms.ToolStripButton
        'Public WithEvents txtFind As New System.Windows.Forms.ToolStripTextBox
        'Protected WithEvents bnPrev As New System.Windows.Forms.ToolStripButton
        'Protected WithEvents bnNext As New System.Windows.Forms.ToolStripButton
        'Protected WithEvents bnLast As New System.Windows.Forms.ToolStripButton
        'Protected WithEvents bnFirst As New System.Windows.Forms.ToolStripButton
        'Public WithEvents PrintPreviewToolStripButton As New System.Windows.Forms.ToolStripButton
        'Public WithEvents T_utilBindingNavigatorSaveItem As New System.Windows.Forms.ToolStripButton
        'Public WithEvents lblFind As New System.Windows.Forms.ToolStripLabel
        'Public WithEvents StatusMsg As New System.Windows.Forms.ToolStripLabel
        'Public WithEvents ProgressMsg As New System.Windows.Forms.ToolStripLabel
        'Public WithEvents bnList As System.Windows.Forms.BindingNavigator
        'Public WithEvents lblFind2 As New System.Windows.Forms.ToolStripLabel
        'Public WithEvents txtFind2 As New System.Windows.Forms.ToolStripTextBox
        'Friend WithEvents Find2Separator As New System.Windows.Forms.ToolStripSeparator
        'Public WithEvents lblFind3 As New System.Windows.Forms.ToolStripLabel
        'Public WithEvents txtFind3 As New System.Windows.Forms.ToolStripTextBox
        'Friend WithEvents Find3Separator As New System.Windows.Forms.ToolStripSeparator
        'Protected WithEvents tsSaveQuery As New System.Windows.Forms.ToolStripButton
        'Public WithEvents lblOnColumn As New System.Windows.Forms.ToolStripLabel
        'Public WithEvents cbColumns As New System.Windows.Forms.ToolStripComboBox
        'Protected WithEvents btnDefault As New System.Windows.Forms.ToolStripButton
        'Protected WithEvents tsSaveLayout As New System.Windows.Forms.ToolStripButton

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
                'bnList2.Visible = False

                ''BindingNavigatorCountItem
                ''
                'Me.BindingNavigatorCountItem.Name = "BindingNavigatorCountItem"
                'Me.BindingNavigatorCountItem.Size = New System.Drawing.Size(36, 23)
                'Me.BindingNavigatorCountItem.Text = "of {0}"
                'Me.BindingNavigatorCountItem.ToolTipText = "Total number of items"
                ''
                ''PrintPreviewToolStripButton
                ''
                'Me.PrintPreviewToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
                'Me.PrintPreviewToolStripButton.Enabled = False
                ''Me.PrintPreviewToolStripButton.Image = CType(Resources.GetObject("PrintPreviewToolStripButton.Image"), System.Drawing.Image)
                'Me.PrintPreviewToolStripButton.Image = QSILib.My.Resources.PrintPreviewToolStripButton
                'Me.PrintPreviewToolStripButton.ImageTransparentColor = System.Drawing.Color.Black
                'Me.PrintPreviewToolStripButton.Name = "PrintPreviewToolStripButton"
                'Me.PrintPreviewToolStripButton.Size = New System.Drawing.Size(23, 23)
                'Me.PrintPreviewToolStripButton.Text = "Run Query"
                'Me.PrintPreviewToolStripButton.Visible = False
                ''
                ''bnFirst
                ''
                'Me.bnFirst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
                ''Me.bnFirst.Image = CType(Resources.GetObject("bnFirst.Image"), System.Drawing.Image)
                'Me.bnFirst.Image = QSILib.My.Resources.bnFirst_Image
                'Me.bnFirst.Name = "bnFirst"
                'Me.bnFirst.RightToLeftAutoMirrorImage = True
                'Me.bnFirst.Size = New System.Drawing.Size(23, 23)
                'Me.bnFirst.Text = "Move first"
                'Me.bnFirst.Visible = True
                ''
                ''bnPrev
                ''
                'Me.bnPrev.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
                ''Me.bnPrev.Image = CType(Resources.GetObject("bnPrev.Image"), System.Drawing.Image)
                'Me.bnPrev.Image = QSILib.My.Resources.bnPrev
                'Me.bnPrev.Name = "bnPrev"
                'Me.bnPrev.RightToLeftAutoMirrorImage = True
                'Me.bnPrev.Size = New System.Drawing.Size(23, 23)
                'Me.bnPrev.Text = "Move previous"
                'Me.bnPrev.Visible = True
                ''
                ''BindingNavigatorSeparator
                ''
                'Me.BindingNavigatorSeparator.Name = "BindingNavigatorSeparator"
                'Me.BindingNavigatorSeparator.Size = New System.Drawing.Size(6, 26)
                ''
                ''BindingNavigatorPositionItem
                ''
                'Me.BindingNavigatorPositionItem.AccessibleName = "Position"
                'Me.BindingNavigatorPositionItem.AutoSize = False
                'Me.BindingNavigatorPositionItem.Name = "BindingNavigatorPositionItem"
                'Me.BindingNavigatorPositionItem.Size = New System.Drawing.Size(38, 21)
                'Me.BindingNavigatorPositionItem.Text = "0"
                'Me.BindingNavigatorPositionItem.ToolTipText = "Current position"
                ''
                ''BindingNavigatorSeparator1
                ''
                'Me.BindingNavigatorSeparator1.Name = "BindingNavigatorSeparator1"
                'Me.BindingNavigatorSeparator1.Size = New System.Drawing.Size(6, 26)
                ''
                ''bnNext
                ''
                'Me.bnNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
                ''Me.bnNext.Image = CType(Resources.GetObject("bnNext.Image"), System.Drawing.Image)
                'Me.bnNext.Image = QSILib.My.Resources.bnNext
                'Me.bnNext.Name = "bnNext"
                'Me.bnNext.RightToLeftAutoMirrorImage = True
                'Me.bnNext.Size = New System.Drawing.Size(23, 23)
                'Me.bnNext.Text = "Move next"
                ''
                ''bnLast
                ''
                'Me.bnLast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
                ''Me.bnLast.Image = CType(Resources.GetObject("bnLast.Image"), System.Drawing.Image)
                'Me.bnLast.Image = QSILib.My.Resources.bnLast
                'Me.bnLast.Name = "bnLast"
                'Me.bnLast.RightToLeftAutoMirrorImage = True
                'Me.bnLast.Size = New System.Drawing.Size(23, 23)
                'Me.bnLast.Text = "Move last"
                ''
                ''BindingNavigatorSeparator2
                ''
                'Me.BindingNavigatorSeparator2.Name = "BindingNavigatorSeparator2"
                'Me.BindingNavigatorSeparator2.Size = New System.Drawing.Size(6, 26)
                ''
                ''BindingNavigatorDeleteItem
                ''
                'Me.BindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
                'Me.BindingNavigatorDeleteItem.Enabled = False
                'Me.BindingNavigatorDeleteItem.Image = CType(Resources.GetObject("BindingNavigatorDeleteItem.Image"), System.Drawing.Image)
                'Me.BindingNavigatorDeleteItem.Name = "BindingNavigatorDeleteItem"
                'Me.BindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = True
                'Me.BindingNavigatorDeleteItem.Size = New System.Drawing.Size(23, 23)
                'Me.BindingNavigatorDeleteItem.Text = "Delete"
                'Me.BindingNavigatorDeleteItem.Visible = False
                ''
                ''T_utilBindingNavigatorSaveItem
                ''
                'Me.T_utilBindingNavigatorSaveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
                'Me.T_utilBindingNavigatorSaveItem.Enabled = False
                ''Me.T_utilBindingNavigatorSaveItem.Image = CType(Resources.GetObject("T_utilBindingNavigatorSaveItem.Image"), System.Drawing.Image)
                'Me.T_utilBindingNavigatorSaveItem.Image = QSILib.My.Resources.T_utilBindingNavigatorSaveItem
                'Me.T_utilBindingNavigatorSaveItem.Name = "T_utilBindingNavigatorSaveItem"
                'Me.T_utilBindingNavigatorSaveItem.Size = New System.Drawing.Size(23, 23)
                'Me.T_utilBindingNavigatorSaveItem.Text = "Save Data"
                'Me.T_utilBindingNavigatorSaveItem.Visible = False
                ''
                ''lblFind
                ''
                'Me.lblFind.Name = "lblFind"
                'Me.lblFind.Size = New System.Drawing.Size(31, 23)
                'Me.lblFind.Text = "Filter"
                ''
                ''txtFind
                ''
                'Me.txtFind.Name = "txtFind"
                'Me.txtFind.Size = New System.Drawing.Size(76, 26)
                ''
                ''ProgressMsg
                ''
                'Me.ProgressMsg.Font = New System.Drawing.Font("Tahoma", 8.400001!)
                'Me.ProgressMsg.ForeColor = System.Drawing.Color.Red
                'Me.ProgressMsg.Name = "ProgressMsg"
                'Me.ProgressMsg.Size = New System.Drawing.Size(0, 23)
                ''
                ''StatusMsg
                ''
                'Me.StatusMsg.ForeColor = System.Drawing.Color.Red
                'Me.StatusMsg.Name = "StatusMsg"
                'Me.StatusMsg.Size = New System.Drawing.Size(0, 23)
                ''
                ''lblOnColumn
                ''
                'Me.lblOnColumn.Name = "lblOnColumn"
                'Me.lblOnColumn.Size = New System.Drawing.Size(71, 23)
                'Me.lblOnColumn.Text = "    On Column"
                'Me.lblOnColumn.Visible = False
                ''
                ''cbColumns
                ''
                'Me.cbColumns.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
                'Me.cbColumns.Name = "cbColumns"
                'Me.cbColumns.Size = New System.Drawing.Size(121, 26)
                'Me.cbColumns.Visible = False
                ''
                ''Find2Separator
                ''
                'Me.Find2Separator.Name = "Find2Separator"
                'Me.Find2Separator.Size = New System.Drawing.Size(6, 26)
                'Me.Find2Separator.Visible = False
                ''
                ''lblFind2
                ''
                'Me.lblFind2.Name = "lblFind2"
                'Me.lblFind2.Size = New System.Drawing.Size(36, 23)
                'Me.lblFind2.Text = "Find 2"
                'Me.lblFind2.Visible = False
                ''
                ''txtFind2
                ''
                'Me.txtFind2.Name = "txtFind2"
                'Me.txtFind2.Size = New System.Drawing.Size(76, 26)
                'Me.txtFind2.Visible = False
                ''
                ''Find3Separator
                ''
                'Me.Find3Separator.Name = "Find3Separator"
                'Me.Find3Separator.Size = New System.Drawing.Size(6, 26)
                ''
                ''lblFind3
                ''
                'Me.lblFind3.Name = "lblFind3"
                'Me.lblFind3.Size = New System.Drawing.Size(36, 23)
                'Me.lblFind3.Text = "Find 3"
                'Me.lblFind3.Visible = False
                ''
                ''txtFind3
                ''
                'Me.txtFind3.Name = "txtFind3"
                'Me.txtFind3.Size = New System.Drawing.Size(76, 21)
                'Me.txtFind3.Visible = False
                ''
                ''tsSaveQuery
                ''
                'Me.tsSaveQuery.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
                ''Me.tsSaveQuery.Image = CType(Resources.GetObject("tsSaveQuery.Image"), System.Drawing.Image)
                'Me.tsSaveQuery.Image = QSILib.My.Resources.tsSaveQuery
                'Me.tsSaveQuery.ImageTransparentColor = System.Drawing.Color.Magenta
                'Me.tsSaveQuery.Name = "tsSaveQuery"
                'Me.tsSaveQuery.Size = New System.Drawing.Size(23, 20)
                'Me.tsSaveQuery.Text = "Save Query"
                'Me.tsSaveQuery.ToolTipText = "Save Query"
                ''
                ''tsSaveLayout
                ''
                'Me.tsSaveLayout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
                'Me.tsSaveLayout.Image = QSILib.My.Resources.ViewNormal16x16
                'Me.tsSaveLayout.ImageTransparentColor = System.Drawing.Color.Magenta
                'Me.tsSaveLayout.Name = "tsSaveLayout"
                'Me.tsSaveLayout.Size = New System.Drawing.Size(23, 20)
                'Me.tsSaveLayout.Text = "Save List Layout(column order and widths)"
                ''
                ''btnDefault
                ''
                'Me.btnDefault.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
                'Me.btnDefault.Image = QSILib.My.Resources.pen
                'Me.btnDefault.ImageTransparentColor = System.Drawing.Color.Magenta
                'Me.btnDefault.Name = "btnDefault"
                'Me.btnDefault.Size = New System.Drawing.Size(23, 20)
                'Me.btnDefault.Text = "Make this my default function"
                'Me.btnDefault.Visible = False

                ''bnList
                'Me.bnList = New System.Windows.Forms.BindingNavigator(Me.components)
                'Me.bnList.AddNewItem = Nothing
                'Me.bnList.AutoSize = False
                'Me.bnList.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
                'Me.bnList.CountItem = Me.BindingNavigatorCountItem
                'Me.bnList.DeleteItem = Nothing
                'Me.bnList.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PrintPreviewToolStripButton, Me.bnFirst, Me.bnPrev, Me.BindingNavigatorSeparator, Me.BindingNavigatorPositionItem, Me.BindingNavigatorCountItem, Me.BindingNavigatorSeparator1, Me.bnNext, Me.bnLast, Me.BindingNavigatorSeparator2, Me.BindingNavigatorDeleteItem, Me.T_utilBindingNavigatorSaveItem, Me.lblFind, Me.txtFind, Me.ProgressMsg, Me.StatusMsg, Me.lblOnColumn, Me.cbColumns, Me.Find2Separator, Me.lblFind2, Me.txtFind2, Me.Find3Separator, Me.lblFind3, Me.txtFind3, Me.tsSaveQuery, Me.tsSaveLayout, Me.btnDefault})
                'Me.bnList.Location = New System.Drawing.Point(0, 0)
                'Me.bnList.MoveFirstItem = Me.bnFirst
                'Me.bnList.MoveLastItem = Me.bnLast
                'Me.bnList.MoveNextItem = Me.bnNext
                'Me.bnList.MovePreviousItem = Me.bnPrev
                'Me.bnList.Name = "bnList"
                'Me.bnList.PositionItem = Me.BindingNavigatorPositionItem
                'Me.bnList.Size = New System.Drawing.Size(776, 26)
                'Me.bnList.TabIndex = 10
                'Me.bnList.Text = "BindingNavigator1"

                'Me.Controls.Add(Me.bnList)

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
                For Each UG As UltraGrid In iUGs    'Only do UGs marked by the programmer for event handling
                    AddHandler UG.DoubleClickCell, AddressOf UGDoubleClick
                    AddHandler UG.AfterCellActivate, AddressOf UGContentClick
                Next
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
            RaiseEvent OnLinkToEditForm(ActedOn, e)
            If ActedOn Then Return
            If Me._EditFormName.ToLower = "none" Then Return
            If iListGV.SelectedRows.Count > 0 Then
                If iListGV.SelectedRows(0) IsNot Nothing Then   'BHS 3/12/08
                    iKey = GetKey(iListGV, iListGV.SelectedRows(0).Index)
                    OpenEditForm(iKey)
                End If
            End If
        End Sub

        '''<summary> GV Click on link column links to Edit Form, passing iKey </summary>
        Sub GVContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
            Dim ActedOn As Boolean = False
            If e.ColumnIndex < 0 OrElse e.ColumnIndex > iListGV.Columns.Count - 1 Then Return ' GBV 11/10/09
            If iListGV.Columns(e.ColumnIndex).CellType.ToString.ToLower.IndexOf("datagridviewlinkcell") < 0 Then Return
            If e.RowIndex < 0 Then Return 'BHS 10/7/08
            RaiseEvent OnLinkToEditForm(ActedOn, e)
            If ActedOn Then Return
            If Me._EditFormName.ToLower = "none" Then Return
            If iListGV.Columns(e.ColumnIndex).CellType.ToString.ToLower.IndexOf("datagridviewlinkcell") > 0 Then
                If iListGV.SelectedRows(0) IsNot Nothing Then   'BHS 3/12/08
                    iKey = GetKey(iListGV, iListGV.SelectedRows(0).Index)
                    OpenEditForm(iKey)
                End If
            End If
        End Sub

        '''<summary> UG double click links to Edit Form, passing iKey </summary>
        Private Sub UGDoubleClick(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.DoubleClickCellEventArgs)
            Dim UG As qUG = TryCast(sender, qUG)
            If UG IsNot Nothing Then
                If e.Cell.Row.Index > -1 Then
                    If UG._ListEditFormName.Length = 0 Or UG._ListEditFormName = "none" Then Return
                    iKey = GetKey(UG, e.Cell.Row.Index)
                    OpenEditFormFromList(UG._ListEditFormName, iKey, Nothing, UG)
                End If

            End If
        End Sub

        '''<summary> UG URL column click links to Edit Form, passing iKey </summary>
        Private Sub UGContentClick(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim UG As qUG = TryCast(sender, qUG)
            If UG IsNot Nothing Then
                If UG.ActiveCell.Row.Index > -1 Then
                    If UG.ActiveCell.Column.Style = ColumnStyle.URL Then
                        If UG._ListEditFormName.Length = 0 Or UG._ListEditFormName = "none" Then Return
                        iKey = GetKey(UG, UG.ActiveCell.Row.Index)
                        OpenEditFormFromList(UG._ListEditFormName, iKey, Nothing, UG)
                    End If
                End If
            End If
        End Sub

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
            Query()
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
                                        R.Cells(C.Name).Value = DV(0)(C.DataPropertyName)
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
                If iListUG IsNot Nothing Then
                    For Each UR As UltraGridRow In iListUG.Rows
                        If aKey = GetKey(iListUG, UR.Index) Then
                            For Each UC As UltraGridCell In UR.Cells
                                If DV.Count >= 1 Then
                                    If UC.Column.Key > "" Then
                                        'Skip Chaptered column types which describe relationships
                                        If UC.Column.DataType.ToString.IndexOf("Chaptered") = -1 Then
                                            UC.Value = DV(0)(UC.Column.Key)
                                        End If
                                    End If
                                Else
                                    'Changes in the record may exclude it from the records listed in the Query, so we need to be silent here
                                    'ProgrammerErr("Need a single row return in SelectRowValues")
                                    'Return False
                                End If
                            Next
                        End If
                    Next
                End If
            End If

            Return True

        End Function

        '''<summary> Create an SQL that will collect current DB values for the changed row
        ''' Aassumes all key fields are strings
        ''' Override in child if these assumptions don't work </summary>
        Public Overrides Function SelectRowValues(ByVal aKey As String) As String
            Dim KeyFields As String = ""
            If iListGV IsNot Nothing Then
                If iListGV.Tag IsNot Nothing Then KeyFields = iListGV.Tag.ToString()
                If TypeOf (iListGV) Is qGVBase Then
                    Dim qGV As qGVBase = CType(iListGV, qGVBase)
                    If qGV._KeyFields IsNot Nothing Then KeyFields = qGV._KeyFields
                End If
            Else
                If iListUG IsNot Nothing Then
                    KeyFields = iListUG._KeyFields
                End If
            End If

            Dim FieldName As String = ParseStr(KeyFields, "|")
            Dim KeyValue As String = ParseStr(aKey, "|")
            Dim C As DataGridViewColumn
            Dim SQL As String = iListSQL

            If iListSQL <= " " Then
                ProgrammerErr("iListSQL not set in descendant")
                Return "bad"
            End If

            Do While FieldName > " "
                'Get Field Type from iListGV Column
                If iListGV IsNot Nothing Then
                    For Each C In iListGV.Columns
                        If C.Name = FieldName Then

                            'Need to test for date or number here - don't know how yet (assume string)
                            'iListGV.DataBindings.BindableComponent.BindingContext.Item(
                            SQL = AddWhere(SQL, C.DataPropertyName, KeyValue, "S")
                            GoTo FoundIt
                        End If
                    Next
                Else
                    If iListUG IsNot Nothing Then
                        For Each UC As UltraGridColumn In iListUG.DisplayLayout.Bands(0).Columns
                            If UC.Key = FieldName Then
                                SQL = AddWhere(SQL, UC.Key, KeyValue, "S")
                                GoTo FoundIt
                            End If
                        Next
                    End If
                End If
                ProgrammerErr("Key Field doesn't match a Column Name or Column Type not tested for")

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


        '''<summary> Also execute filter every time user changes the columns to be searched </summary>
        Private Sub cbColumns_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
            Try
                Find1()
            Catch ex As Exception
                ShowError("Error filtering list data", ex)
                Return  'Not fatal
            End Try
        End Sub

        'Find/columns logic
        Sub Find1()
            Dim ActedOn As Boolean = False

            ShowProgress("", True)  'BHS 9/29/09 Show hourglass

            RaiseEvent OnFind(ActedOn)  'Let child have chance to handle event

            If ActedOn = False Then
                'BHS 9/3/08 Column-specific filtering
                If cbColumns.Visible = True Then
                    If cbColumns.ComboBox.SelectedValue.ToString = "" Or _
                        cbColumns.ComboBox.SelectedValue.ToString = "all" Then
                        iListNavigator.BindingSource.Filter = BuildGVFilter(iListGV, txtFind.Text)
                    Else
                        iListNavigator.BindingSource.Filter = _
                           BuildGVColFilter("", iListGV, iListGV.Columns(cbColumns.ComboBox.SelectedValue.ToString), txtFind.Text)
                    End If
                Else
                    iListNavigator.BindingSource.Filter = BuildGVFilter(iListGV, txtFind.Text)
                End If

                ShowProgress("", False)  'BHS 9/29/09 Turn off hourglass

                RaiseEvent OnAfterFind()
            End If
        End Sub

        '''<summary> Execute filter every time user changes the Find text </summary>
        Private Sub txtFind2_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
            Try
                Dim ActedOn As Boolean = False
                RaiseEvent OnFind(ActedOn)  'Let child have chance to handle event

                If ActedOn = False Then
                    iListNavigator.BindingSource.Filter = BuildGVFilter(iListGV, txtFind.Text)
                    RaiseEvent OnAfterFind()
                End If
            Catch ex As Exception
                ShowError("Error filtering list data", ex)
                Return  'Not fatal
            End Try
        End Sub

        '''<summary> Execute filter every time user changes the Find text </summary>
        Private Sub txtFind3_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
            Try
                Dim ActedOn As Boolean = False
                RaiseEvent OnFind(ActedOn)  'Let child have chance to handle event

                If ActedOn = False Then
                    iListNavigator.BindingSource.Filter = BuildGVFilter(iListGV, txtFind.Text)
                    RaiseEvent OnAfterFind()
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
            Post("EndOfClear")
        End Sub


    End Class

End Namespace