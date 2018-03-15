Namespace Windows.Forms

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class flMain2
        Inherits fBase


        'Form overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()> _
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(flMain2))
            Me.btnQuery = New System.Windows.Forms.Button()
            Me.BtnAdd = New System.Windows.Forms.Button()
            Me.bnList = New System.Windows.Forms.BindingNavigator(Me.components)
            Me.BindingNavigatorCountItem = New System.Windows.Forms.ToolStripLabel()
            Me.PrintPreviewToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.bnFirst = New System.Windows.Forms.ToolStripButton()
            Me.bnPrev = New System.Windows.Forms.ToolStripButton()
            Me.BindingNavigatorSeparator = New System.Windows.Forms.ToolStripSeparator()
            Me.BindingNavigatorPositionItem = New System.Windows.Forms.ToolStripTextBox()
            Me.BindingNavigatorSeparator1 = New System.Windows.Forms.ToolStripSeparator()
            Me.bnNext = New System.Windows.Forms.ToolStripButton()
            Me.bnLast = New System.Windows.Forms.ToolStripButton()
            Me.BindingNavigatorSeparator2 = New System.Windows.Forms.ToolStripSeparator()
            Me.BindingNavigatorDeleteItem = New System.Windows.Forms.ToolStripButton()
            Me.T_utilBindingNavigatorSaveItem = New System.Windows.Forms.ToolStripButton()
            Me.lblFind = New System.Windows.Forms.ToolStripLabel()
            Me.txtFind = New System.Windows.Forms.ToolStripTextBox()
            Me.ProgressMsg = New System.Windows.Forms.ToolStripLabel()
            Me.StatusMsg = New System.Windows.Forms.ToolStripLabel()
            Me.lblOnColumn = New System.Windows.Forms.ToolStripLabel()
            Me.cbColumns = New System.Windows.Forms.ToolStripComboBox()
            Me.Find2Separator = New System.Windows.Forms.ToolStripSeparator()
            Me.lblFind2 = New System.Windows.Forms.ToolStripLabel()
            Me.txtFind2 = New System.Windows.Forms.ToolStripTextBox()
            Me.Find3Separator = New System.Windows.Forms.ToolStripSeparator()
            Me.lblFind3 = New System.Windows.Forms.ToolStripLabel()
            Me.txtFind3 = New System.Windows.Forms.ToolStripTextBox()
            Me.tsSaveQuery = New System.Windows.Forms.ToolStripButton()
            Me.tsLoadQuery = New System.Windows.Forms.ToolStripButton()
            Me.tsSaveLayout = New System.Windows.Forms.ToolStripButton()
            Me.btnDefault = New System.Windows.Forms.ToolStripButton()
            Me.btnSQL = New System.Windows.Forms.ToolStripButton()
            Me.tsFieldChooser = New System.Windows.Forms.ToolStripButton()
            Me.btnReport = New System.Windows.Forms.Button()
            Me.lblFTitle = New System.Windows.Forms.Label()
            Me.btnSearchHelp = New System.Windows.Forms.Button()
            Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
            Me.HelpMsg = New System.Windows.Forms.ToolStripStatusLabel()
            Me.btnClear1 = New System.Windows.Forms.Button()
            Me.btnExport1 = New System.Windows.Forms.Button()
            Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
            Me.btnExport2 = New System.Windows.Forms.Button()
            Me.btnExport3 = New System.Windows.Forms.Button()
            Me.btnExport4 = New System.Windows.Forms.Button()
            Me.btnReport2 = New System.Windows.Forms.Button()
            Me.btnReport3 = New System.Windows.Forms.Button()
            Me.btnReport4 = New System.Windows.Forms.Button()
            CType(Me.iDS, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.iEP, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.bnList, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.bnList.SuspendLayout()
            Me.StatusStrip1.SuspendLayout()
            Me.SuspendLayout()
            '
            'btnQuery
            '
            Me.btnQuery.Image = Global.QSILib.My.Resources.Resources.FIND
            Me.btnQuery.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnQuery.Location = New System.Drawing.Point(0, 28)
            Me.btnQuery.Name = "btnQuery"
            Me.btnQuery.Size = New System.Drawing.Size(65, 23)
            Me.btnQuery.TabIndex = 8
            Me.btnQuery.Text = "Search"
            Me.btnQuery.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.ToolTip1.SetToolTip(Me.btnQuery, "Search and redisplay list based on what you've entered")
            Me.btnQuery.UseVisualStyleBackColor = True
            '
            'BtnAdd
            '
            Me.BtnAdd.Image = Global.QSILib.My.Resources.Resources._NEW
            Me.BtnAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.BtnAdd.Location = New System.Drawing.Point(0, 80)
            Me.BtnAdd.Name = "BtnAdd"
            Me.BtnAdd.Size = New System.Drawing.Size(65, 23)
            Me.BtnAdd.TabIndex = 9
            Me.BtnAdd.Text = "Add"
            Me.BtnAdd.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.ToolTip1.SetToolTip(Me.BtnAdd, "Add a new record")
            Me.BtnAdd.UseVisualStyleBackColor = True
            '
            'bnList
            '
            Me.bnList.AddNewItem = Nothing
            Me.bnList.AutoSize = False
            Me.bnList.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
            Me.bnList.CountItem = Me.BindingNavigatorCountItem
            Me.bnList.DeleteItem = Nothing
            Me.bnList.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PrintPreviewToolStripButton, Me.bnFirst, Me.bnPrev, Me.BindingNavigatorSeparator, Me.BindingNavigatorPositionItem, Me.BindingNavigatorCountItem, Me.BindingNavigatorSeparator1, Me.bnNext, Me.bnLast, Me.BindingNavigatorSeparator2, Me.BindingNavigatorDeleteItem, Me.T_utilBindingNavigatorSaveItem, Me.lblFind, Me.txtFind, Me.ProgressMsg, Me.StatusMsg, Me.lblOnColumn, Me.cbColumns, Me.Find2Separator, Me.lblFind2, Me.txtFind2, Me.Find3Separator, Me.lblFind3, Me.txtFind3, Me.tsSaveQuery, Me.tsLoadQuery, Me.tsSaveLayout, Me.btnDefault, Me.btnSQL, Me.tsFieldChooser})
            Me.bnList.Location = New System.Drawing.Point(0, 0)
            Me.bnList.MoveFirstItem = Me.bnFirst
            Me.bnList.MoveLastItem = Me.bnLast
            Me.bnList.MoveNextItem = Me.bnNext
            Me.bnList.MovePreviousItem = Me.bnPrev
            Me.bnList.Name = "bnList"
            Me.bnList.PositionItem = Me.BindingNavigatorPositionItem
            Me.bnList.Size = New System.Drawing.Size(776, 26)
            Me.bnList.TabIndex = 10
            Me.bnList.Text = "BindingNavigator1"
            '
            'BindingNavigatorCountItem
            '
            Me.BindingNavigatorCountItem.Name = "BindingNavigatorCountItem"
            Me.BindingNavigatorCountItem.Size = New System.Drawing.Size(36, 23)
            Me.BindingNavigatorCountItem.Text = "of {0}"
            Me.BindingNavigatorCountItem.ToolTipText = "Total number of items"
            '
            'PrintPreviewToolStripButton
            '
            Me.PrintPreviewToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.PrintPreviewToolStripButton.Enabled = False
            Me.PrintPreviewToolStripButton.ImageTransparentColor = System.Drawing.Color.Black
            Me.PrintPreviewToolStripButton.Name = "PrintPreviewToolStripButton"
            Me.PrintPreviewToolStripButton.Size = New System.Drawing.Size(23, 23)
            Me.PrintPreviewToolStripButton.Text = "Run Query"
            Me.PrintPreviewToolStripButton.Visible = False
            '
            'bnFirst
            '
            Me.bnFirst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.bnFirst.Image = Global.QSILib.My.Resources.Resources.bnFirst_Image
            Me.bnFirst.Name = "bnFirst"
            Me.bnFirst.RightToLeftAutoMirrorImage = True
            Me.bnFirst.Size = New System.Drawing.Size(23, 23)
            Me.bnFirst.Text = "Move first"
            '
            'bnPrev
            '
            Me.bnPrev.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.bnPrev.Image = Global.QSILib.My.Resources.Resources.bnPrev
            Me.bnPrev.Name = "bnPrev"
            Me.bnPrev.RightToLeftAutoMirrorImage = True
            Me.bnPrev.Size = New System.Drawing.Size(23, 23)
            Me.bnPrev.Text = "Move previous"
            '
            'BindingNavigatorSeparator
            '
            Me.BindingNavigatorSeparator.Name = "BindingNavigatorSeparator"
            Me.BindingNavigatorSeparator.Size = New System.Drawing.Size(6, 26)
            '
            'BindingNavigatorPositionItem
            '
            Me.BindingNavigatorPositionItem.AccessibleName = "Position"
            Me.BindingNavigatorPositionItem.AutoSize = False
            Me.BindingNavigatorPositionItem.Name = "BindingNavigatorPositionItem"
            Me.BindingNavigatorPositionItem.Size = New System.Drawing.Size(38, 21)
            Me.BindingNavigatorPositionItem.Text = "0"
            Me.BindingNavigatorPositionItem.ToolTipText = "Current position"
            '
            'BindingNavigatorSeparator1
            '
            Me.BindingNavigatorSeparator1.Name = "BindingNavigatorSeparator1"
            Me.BindingNavigatorSeparator1.Size = New System.Drawing.Size(6, 26)
            '
            'bnNext
            '
            Me.bnNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.bnNext.Image = Global.QSILib.My.Resources.Resources.bnNext
            Me.bnNext.Name = "bnNext"
            Me.bnNext.RightToLeftAutoMirrorImage = True
            Me.bnNext.Size = New System.Drawing.Size(23, 23)
            Me.bnNext.Text = "Move next"
            '
            'bnLast
            '
            Me.bnLast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.bnLast.Image = Global.QSILib.My.Resources.Resources.bnLast
            Me.bnLast.Name = "bnLast"
            Me.bnLast.RightToLeftAutoMirrorImage = True
            Me.bnLast.Size = New System.Drawing.Size(23, 23)
            Me.bnLast.Text = "Move last"
            '
            'BindingNavigatorSeparator2
            '
            Me.BindingNavigatorSeparator2.Name = "BindingNavigatorSeparator2"
            Me.BindingNavigatorSeparator2.Size = New System.Drawing.Size(6, 26)
            '
            'BindingNavigatorDeleteItem
            '
            Me.BindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.BindingNavigatorDeleteItem.Enabled = False
            Me.BindingNavigatorDeleteItem.Name = "BindingNavigatorDeleteItem"
            Me.BindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = True
            Me.BindingNavigatorDeleteItem.Size = New System.Drawing.Size(23, 23)
            Me.BindingNavigatorDeleteItem.Text = "Delete"
            Me.BindingNavigatorDeleteItem.Visible = False
            '
            'T_utilBindingNavigatorSaveItem
            '
            Me.T_utilBindingNavigatorSaveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.T_utilBindingNavigatorSaveItem.Enabled = False
            Me.T_utilBindingNavigatorSaveItem.Name = "T_utilBindingNavigatorSaveItem"
            Me.T_utilBindingNavigatorSaveItem.Size = New System.Drawing.Size(23, 23)
            Me.T_utilBindingNavigatorSaveItem.Text = "Save Data"
            Me.T_utilBindingNavigatorSaveItem.Visible = False
            '
            'lblFind
            '
            Me.lblFind.Name = "lblFind"
            Me.lblFind.Size = New System.Drawing.Size(31, 23)
            Me.lblFind.Text = "Filter"
            '
            'txtFind
            '
            Me.txtFind.Name = "txtFind"
            Me.txtFind.Size = New System.Drawing.Size(76, 26)
            '
            'ProgressMsg
            '
            Me.ProgressMsg.Font = New System.Drawing.Font("Tahoma", 8.400001!)
            Me.ProgressMsg.ForeColor = System.Drawing.Color.Red
            Me.ProgressMsg.Name = "ProgressMsg"
            Me.ProgressMsg.Size = New System.Drawing.Size(0, 23)
            '
            'StatusMsg
            '
            Me.StatusMsg.ForeColor = System.Drawing.Color.Red
            Me.StatusMsg.Name = "StatusMsg"
            Me.StatusMsg.Size = New System.Drawing.Size(0, 23)
            '
            'lblOnColumn
            '
            Me.lblOnColumn.Name = "lblOnColumn"
            Me.lblOnColumn.Size = New System.Drawing.Size(71, 23)
            Me.lblOnColumn.Text = "    On Column"
            Me.lblOnColumn.Visible = False
            '
            'cbColumns
            '
            Me.cbColumns.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cbColumns.Name = "cbColumns"
            Me.cbColumns.Size = New System.Drawing.Size(121, 26)
            Me.cbColumns.Visible = False
            '
            'Find2Separator
            '
            Me.Find2Separator.Name = "Find2Separator"
            Me.Find2Separator.Size = New System.Drawing.Size(6, 26)
            Me.Find2Separator.Visible = False
            '
            'lblFind2
            '
            Me.lblFind2.Name = "lblFind2"
            Me.lblFind2.Size = New System.Drawing.Size(36, 23)
            Me.lblFind2.Text = "Find 2"
            Me.lblFind2.Visible = False
            '
            'txtFind2
            '
            Me.txtFind2.Name = "txtFind2"
            Me.txtFind2.Size = New System.Drawing.Size(76, 26)
            Me.txtFind2.Visible = False
            '
            'Find3Separator
            '
            Me.Find3Separator.Name = "Find3Separator"
            Me.Find3Separator.Size = New System.Drawing.Size(6, 26)
            '
            'lblFind3
            '
            Me.lblFind3.Name = "lblFind3"
            Me.lblFind3.Size = New System.Drawing.Size(36, 23)
            Me.lblFind3.Text = "Find 3"
            Me.lblFind3.Visible = False
            '
            'txtFind3
            '
            Me.txtFind3.Name = "txtFind3"
            Me.txtFind3.Size = New System.Drawing.Size(76, 26)
            Me.txtFind3.Visible = False
            '
            'tsSaveQuery
            '
            Me.tsSaveQuery.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.tsSaveQuery.Image = Global.QSILib.My.Resources.Resources.SAVEBlue
            Me.tsSaveQuery.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.tsSaveQuery.Name = "tsSaveQuery"
            Me.tsSaveQuery.Size = New System.Drawing.Size(23, 23)
            Me.tsSaveQuery.Tag = "tsSaveQuery"
            Me.tsSaveQuery.Text = "Save &Query"
            Me.tsSaveQuery.ToolTipText = "Save Query / Manage Queries (Ctrl-Q)"
            '
            'tsLoadQuery
            '
            Me.tsLoadQuery.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.tsLoadQuery.Image = Global.QSILib.My.Resources.Resources.Open
            Me.tsLoadQuery.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.tsLoadQuery.Name = "tsLoadQuery"
            Me.tsLoadQuery.Size = New System.Drawing.Size(23, 23)
            Me.tsLoadQuery.Tag = "tsloadquery"
            Me.tsLoadQuery.Text = "L&oad Query"
            Me.tsLoadQuery.ToolTipText = "Load Query (Ctrl-O)"
            '
            'tsSaveLayout
            '
            Me.tsSaveLayout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.tsSaveLayout.Image = Global.QSILib.My.Resources.Resources.ViewNormal16x16
            Me.tsSaveLayout.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.tsSaveLayout.Name = "tsSaveLayout"
            Me.tsSaveLayout.Size = New System.Drawing.Size(23, 23)
            Me.tsSaveLayout.Text = "Save List Layout(column order and widths)"
            '
            'btnDefault
            '
            Me.btnDefault.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.btnDefault.Image = Global.QSILib.My.Resources.Resources.pen
            Me.btnDefault.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.btnDefault.Name = "btnDefault"
            Me.btnDefault.Size = New System.Drawing.Size(23, 23)
            Me.btnDefault.Text = "Make this my default function"
            Me.btnDefault.Visible = False
            '
            'btnSQL
            '
            Me.btnSQL.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.btnSQL.Image = Global.QSILib.My.Resources.Resources.SQL
            Me.btnSQL.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.btnSQL.Name = "btnSQL"
            Me.btnSQL.Size = New System.Drawing.Size(23, 23)
            Me.btnSQL.Text = "Show the SQL used in query."
            Me.btnSQL.Visible = False
            '
            'tsFieldChooser
            '
            Me.tsFieldChooser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.tsFieldChooser.Image = CType(resources.GetObject("tsFieldChooser.Image"), System.Drawing.Image)
            Me.tsFieldChooser.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.tsFieldChooser.Name = "tsFieldChooser"
            Me.tsFieldChooser.Size = New System.Drawing.Size(23, 23)
            Me.tsFieldChooser.Text = "Field Chooser (select fields to view in grid)"
            '
            'btnReport
            '
            Me.btnReport.Image = Global.QSILib.My.Resources.Resources.PRINT
            Me.btnReport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnReport.Location = New System.Drawing.Point(0, 106)
            Me.btnReport.Name = "btnReport"
            Me.btnReport.Size = New System.Drawing.Size(65, 23)
            Me.btnReport.TabIndex = 11
            Me.btnReport.Tag = "inactive"
            Me.btnReport.Text = "Report"
            Me.btnReport.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.ToolTip1.SetToolTip(Me.btnReport, "Generate a report for the contents of the list")
            Me.btnReport.UseVisualStyleBackColor = True
            '
            'lblFTitle
            '
            Me.lblFTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblFTitle.AutoSize = True
            Me.lblFTitle.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold)
            Me.lblFTitle.ForeColor = System.Drawing.Color.Blue
            Me.lblFTitle.Location = New System.Drawing.Point(665, 0)
            Me.lblFTitle.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
            Me.lblFTitle.Name = "lblFTitle"
            Me.lblFTitle.Size = New System.Drawing.Size(113, 19)
            Me.lblFTitle.TabIndex = 20
            Me.lblFTitle.Text = "Function Title"
            '
            'btnSearchHelp
            '
            Me.btnSearchHelp.FlatAppearance.BorderSize = 0
            Me.btnSearchHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnSearchHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 5.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnSearchHelp.Image = Global.QSILib.My.Resources.Resources.help8
            Me.btnSearchHelp.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.btnSearchHelp.Location = New System.Drawing.Point(64, 28)
            Me.btnSearchHelp.Margin = New System.Windows.Forms.Padding(2)
            Me.btnSearchHelp.Name = "btnSearchHelp"
            Me.btnSearchHelp.Size = New System.Drawing.Size(19, 23)
            Me.btnSearchHelp.TabIndex = 22
            Me.btnSearchHelp.TabStop = False
            Me.btnSearchHelp.Text = " "
            Me.btnSearchHelp.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.btnSearchHelp.UseVisualStyleBackColor = True
            '
            'StatusStrip1
            '
            Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HelpMsg})
            Me.StatusStrip1.Location = New System.Drawing.Point(0, 291)
            Me.StatusStrip1.Name = "StatusStrip1"
            Me.StatusStrip1.Padding = New System.Windows.Forms.Padding(1, 0, 10, 0)
            Me.StatusStrip1.Size = New System.Drawing.Size(776, 22)
            Me.StatusStrip1.TabIndex = 23
            Me.StatusStrip1.Text = "StatusStrip1"
            '
            'HelpMsg
            '
            Me.HelpMsg.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(231, Byte), Integer), CType(CType(246, Byte), Integer))
            Me.HelpMsg.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
            Me.HelpMsg.Name = "HelpMsg"
            Me.HelpMsg.Overflow = System.Windows.Forms.ToolStripItemOverflow.Always
            Me.HelpMsg.Size = New System.Drawing.Size(0, 17)
            '
            'btnClear1
            '
            Me.btnClear1.Image = Global.QSILib.My.Resources.Resources.Clear
            Me.btnClear1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnClear1.Location = New System.Drawing.Point(0, 54)
            Me.btnClear1.Name = "btnClear1"
            Me.btnClear1.Size = New System.Drawing.Size(65, 23)
            Me.btnClear1.TabIndex = 24
            Me.btnClear1.Text = "Clear"
            Me.btnClear1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.ToolTip1.SetToolTip(Me.btnClear1, "Clear contents of the list and all query-by-example criteria")
            Me.btnClear1.UseVisualStyleBackColor = True
            '
            'btnExport1
            '
            Me.btnExport1.Image = Global.QSILib.My.Resources.Resources.excel21
            Me.btnExport1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnExport1.Location = New System.Drawing.Point(0, 132)
            Me.btnExport1.Name = "btnExport1"
            Me.btnExport1.Size = New System.Drawing.Size(65, 23)
            Me.btnExport1.TabIndex = 25
            Me.btnExport1.Tag = "active"
            Me.btnExport1.Text = "Export"
            Me.btnExport1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.ToolTip1.SetToolTip(Me.btnExport1, "Put list contents in a spreadsheet")
            Me.btnExport1.UseVisualStyleBackColor = True
            '
            'btnExport2
            '
            Me.btnExport2.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
            Me.btnExport2.Image = Global.QSILib.My.Resources.Resources.excel21
            Me.btnExport2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnExport2.Location = New System.Drawing.Point(0, 157)
            Me.btnExport2.Name = "btnExport2"
            Me.btnExport2.Size = New System.Drawing.Size(65, 23)
            Me.btnExport2.TabIndex = 26
            Me.btnExport2.Tag = "Inactive"
            Me.btnExport2.Text = "Export"
            Me.btnExport2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.ToolTip1.SetToolTip(Me.btnExport2, "Put list contents in a spreadsheet")
            Me.btnExport2.UseVisualStyleBackColor = False
            Me.btnExport2.Visible = False
            '
            'btnExport3
            '
            Me.btnExport3.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
            Me.btnExport3.Image = Global.QSILib.My.Resources.Resources.excel21
            Me.btnExport3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnExport3.Location = New System.Drawing.Point(0, 174)
            Me.btnExport3.Name = "btnExport3"
            Me.btnExport3.Size = New System.Drawing.Size(65, 23)
            Me.btnExport3.TabIndex = 27
            Me.btnExport3.Tag = "Inactive"
            Me.btnExport3.Text = "Export"
            Me.btnExport3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.ToolTip1.SetToolTip(Me.btnExport3, "Put list contents in a spreadsheet")
            Me.btnExport3.UseVisualStyleBackColor = False
            Me.btnExport3.Visible = False
            '
            'btnExport4
            '
            Me.btnExport4.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
            Me.btnExport4.Image = Global.QSILib.My.Resources.Resources.excel21
            Me.btnExport4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnExport4.Location = New System.Drawing.Point(0, 189)
            Me.btnExport4.Name = "btnExport4"
            Me.btnExport4.Size = New System.Drawing.Size(65, 23)
            Me.btnExport4.TabIndex = 28
            Me.btnExport4.Tag = "Inactive"
            Me.btnExport4.Text = "Export"
            Me.btnExport4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.ToolTip1.SetToolTip(Me.btnExport4, "Put list contents in a spreadsheet")
            Me.btnExport4.UseVisualStyleBackColor = False
            Me.btnExport4.Visible = False
            '
            'btnReport2
            '
            Me.btnReport2.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
            Me.btnReport2.Image = Global.QSILib.My.Resources.Resources.PRINT
            Me.btnReport2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnReport2.Location = New System.Drawing.Point(0, 203)
            Me.btnReport2.Name = "btnReport2"
            Me.btnReport2.Size = New System.Drawing.Size(65, 23)
            Me.btnReport2.TabIndex = 29
            Me.btnReport2.Tag = "Inactive"
            Me.btnReport2.Text = "Report"
            Me.btnReport2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.ToolTip1.SetToolTip(Me.btnReport2, "Generate a report for the contents of the list")
            Me.btnReport2.UseVisualStyleBackColor = False
            Me.btnReport2.Visible = False
            '
            'btnReport3
            '
            Me.btnReport3.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
            Me.btnReport3.Image = Global.QSILib.My.Resources.Resources.PRINT
            Me.btnReport3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnReport3.Location = New System.Drawing.Point(0, 218)
            Me.btnReport3.Name = "btnReport3"
            Me.btnReport3.Size = New System.Drawing.Size(65, 23)
            Me.btnReport3.TabIndex = 30
            Me.btnReport3.Tag = "Inactive"
            Me.btnReport3.Text = "Report"
            Me.btnReport3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.ToolTip1.SetToolTip(Me.btnReport3, "Generate a report for the contents of the list")
            Me.btnReport3.UseVisualStyleBackColor = False
            Me.btnReport3.Visible = False
            '
            'btnReport4
            '
            Me.btnReport4.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
            Me.btnReport4.Image = Global.QSILib.My.Resources.Resources.PRINT
            Me.btnReport4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnReport4.Location = New System.Drawing.Point(0, 232)
            Me.btnReport4.Name = "btnReport4"
            Me.btnReport4.Size = New System.Drawing.Size(65, 23)
            Me.btnReport4.TabIndex = 31
            Me.btnReport4.Tag = "Inactive"
            Me.btnReport4.Text = "Report"
            Me.btnReport4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.ToolTip1.SetToolTip(Me.btnReport4, "Generate a report for the contents of the list")
            Me.btnReport4.UseVisualStyleBackColor = False
            Me.btnReport4.Visible = False
            '
            'flMain2
            '
            Me.AcceptButton = Me.btnQuery
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
            Me.ClientSize = New System.Drawing.Size(776, 313)
            Me.Controls.Add(Me.btnReport4)
            Me.Controls.Add(Me.btnReport3)
            Me.Controls.Add(Me.btnReport2)
            Me.Controls.Add(Me.btnExport4)
            Me.Controls.Add(Me.btnExport3)
            Me.Controls.Add(Me.btnExport2)
            Me.Controls.Add(Me.btnExport1)
            Me.Controls.Add(Me.btnClear1)
            Me.Controls.Add(Me.StatusStrip1)
            Me.Controls.Add(Me.btnSearchHelp)
            Me.Controls.Add(Me.lblFTitle)
            Me.Controls.Add(Me.btnReport)
            Me.Controls.Add(Me.bnList)
            Me.Controls.Add(Me.BtnAdd)
            Me.Controls.Add(Me.btnQuery)
            Me.Margin = New System.Windows.Forms.Padding(2)
            Me.Name = "flMain2"
            Me.Text = "flMain"
            CType(Me.iDS, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.iEP, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.bnList, System.ComponentModel.ISupportInitialize).EndInit()
            Me.bnList.ResumeLayout(False)
            Me.bnList.PerformLayout()
            Me.StatusStrip1.ResumeLayout(False)
            Me.StatusStrip1.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents BindingNavigatorCountItem As System.Windows.Forms.ToolStripLabel
        Friend WithEvents BindingNavigatorSeparator As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents BindingNavigatorPositionItem As System.Windows.Forms.ToolStripTextBox
        Friend WithEvents BindingNavigatorSeparator1 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents BindingNavigatorSeparator2 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents BindingNavigatorDeleteItem As System.Windows.Forms.ToolStripButton
        Public WithEvents txtFind As System.Windows.Forms.ToolStripTextBox
        Protected WithEvents btnQuery As System.Windows.Forms.Button
        Protected WithEvents BtnAdd As System.Windows.Forms.Button
        Protected WithEvents bnPrev As System.Windows.Forms.ToolStripButton
        Protected WithEvents bnNext As System.Windows.Forms.ToolStripButton
        Protected WithEvents bnLast As System.Windows.Forms.ToolStripButton
        Protected WithEvents bnFirst As System.Windows.Forms.ToolStripButton
        Protected WithEvents btnReport As System.Windows.Forms.Button
        Protected WithEvents lblFTitle As System.Windows.Forms.Label
        Protected WithEvents btnSearchHelp As System.Windows.Forms.Button
        Friend WithEvents HelpMsg As System.Windows.Forms.ToolStripStatusLabel
        Public WithEvents PrintPreviewToolStripButton As System.Windows.Forms.ToolStripButton
        Public WithEvents T_utilBindingNavigatorSaveItem As System.Windows.Forms.ToolStripButton
        Public WithEvents lblFind As System.Windows.Forms.ToolStripLabel
        Public WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
        Public WithEvents StatusMsg As System.Windows.Forms.ToolStripLabel
        Public WithEvents ProgressMsg As System.Windows.Forms.ToolStripLabel
        Public WithEvents bnList As System.Windows.Forms.BindingNavigator
        Public WithEvents lblFind2 As System.Windows.Forms.ToolStripLabel
        Public WithEvents txtFind2 As System.Windows.Forms.ToolStripTextBox
        Friend WithEvents Find2Separator As System.Windows.Forms.ToolStripSeparator
        Public WithEvents lblFind3 As System.Windows.Forms.ToolStripLabel
        Public WithEvents txtFind3 As System.Windows.Forms.ToolStripTextBox
        Friend WithEvents Find3Separator As System.Windows.Forms.ToolStripSeparator
        Public WithEvents lblOnColumn As System.Windows.Forms.ToolStripLabel
        Public WithEvents cbColumns As System.Windows.Forms.ToolStripComboBox
        Protected WithEvents btnClear1 As System.Windows.Forms.Button
        Protected WithEvents btnDefault As System.Windows.Forms.ToolStripButton
        Protected WithEvents ToolTip1 As System.Windows.Forms.ToolTip
        Protected WithEvents tsSaveLayout As System.Windows.Forms.ToolStripButton
        Protected Friend WithEvents btnExport1 As System.Windows.Forms.Button
        Protected Friend WithEvents btnExport2 As System.Windows.Forms.Button
        Protected Friend WithEvents btnExport3 As System.Windows.Forms.Button
        Protected Friend WithEvents btnExport4 As System.Windows.Forms.Button
        Protected WithEvents btnReport2 As System.Windows.Forms.Button
        Protected WithEvents btnReport3 As System.Windows.Forms.Button
        Protected WithEvents btnReport4 As System.Windows.Forms.Button
        Protected WithEvents btnSQL As System.Windows.Forms.ToolStripButton
        Protected WithEvents tsSaveQuery As System.Windows.Forms.ToolStripButton
        Protected WithEvents tsLoadQuery As System.Windows.Forms.ToolStripButton
        Friend WithEvents tsFieldChooser As System.Windows.Forms.ToolStripButton
    End Class

End Namespace