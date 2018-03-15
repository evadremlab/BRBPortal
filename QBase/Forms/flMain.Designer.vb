Namespace Windows.Forms

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class flMain
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(flMain))
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
            Me.tsSaveLayout = New System.Windows.Forms.ToolStripButton()
            Me.btnDefault = New System.Windows.Forms.ToolStripButton()
            Me.btnReport = New System.Windows.Forms.Button()
            Me.lblFTitle = New System.Windows.Forms.Label()
            Me.btnSearchHelp = New System.Windows.Forms.Button()
            Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
            Me.HelpMsg = New System.Windows.Forms.ToolStripStatusLabel()
            Me.btnClear1 = New System.Windows.Forms.Button()
            Me.btnExport1 = New System.Windows.Forms.Button()
            Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
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
            Me.bnList.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PrintPreviewToolStripButton, Me.bnFirst, Me.bnPrev, Me.BindingNavigatorSeparator, Me.BindingNavigatorPositionItem, Me.BindingNavigatorCountItem, Me.BindingNavigatorSeparator1, Me.bnNext, Me.bnLast, Me.BindingNavigatorSeparator2, Me.BindingNavigatorDeleteItem, Me.T_utilBindingNavigatorSaveItem, Me.lblFind, Me.txtFind, Me.ProgressMsg, Me.StatusMsg, Me.lblOnColumn, Me.cbColumns, Me.Find2Separator, Me.lblFind2, Me.txtFind2, Me.Find3Separator, Me.lblFind3, Me.txtFind3, Me.tsSaveQuery, Me.tsSaveLayout, Me.btnDefault})
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
            Me.PrintPreviewToolStripButton.Image = CType(resources.GetObject("PrintPreviewToolStripButton.Image"), System.Drawing.Image)
            Me.PrintPreviewToolStripButton.ImageTransparentColor = System.Drawing.Color.Black
            Me.PrintPreviewToolStripButton.Name = "PrintPreviewToolStripButton"
            Me.PrintPreviewToolStripButton.Size = New System.Drawing.Size(23, 23)
            Me.PrintPreviewToolStripButton.Text = "Run Query"
            Me.PrintPreviewToolStripButton.Visible = False
            '
            'bnFirst
            '
            Me.bnFirst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.bnFirst.Image = CType(resources.GetObject("bnFirst.Image"), System.Drawing.Image)
            Me.bnFirst.Name = "bnFirst"
            Me.bnFirst.RightToLeftAutoMirrorImage = True
            Me.bnFirst.Size = New System.Drawing.Size(23, 23)
            Me.bnFirst.Text = "Move first"
            '
            'bnPrev
            '
            Me.bnPrev.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.bnPrev.Image = CType(resources.GetObject("bnPrev.Image"), System.Drawing.Image)
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
            Me.bnNext.Image = CType(resources.GetObject("bnNext.Image"), System.Drawing.Image)
            Me.bnNext.Name = "bnNext"
            Me.bnNext.RightToLeftAutoMirrorImage = True
            Me.bnNext.Size = New System.Drawing.Size(23, 23)
            Me.bnNext.Text = "Move next"
            '
            'bnLast
            '
            Me.bnLast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.bnLast.Image = CType(resources.GetObject("bnLast.Image"), System.Drawing.Image)
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
            Me.BindingNavigatorDeleteItem.Image = CType(resources.GetObject("BindingNavigatorDeleteItem.Image"), System.Drawing.Image)
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
            Me.T_utilBindingNavigatorSaveItem.Image = CType(resources.GetObject("T_utilBindingNavigatorSaveItem.Image"), System.Drawing.Image)
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
            Me.tsSaveQuery.Image = CType(resources.GetObject("tsSaveQuery.Image"), System.Drawing.Image)
            Me.tsSaveQuery.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.tsSaveQuery.Name = "tsSaveQuery"
            Me.tsSaveQuery.Size = New System.Drawing.Size(23, 23)
            Me.tsSaveQuery.Text = "Save Query"
            Me.tsSaveQuery.ToolTipText = "Save Query"
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
            'btnReport
            '
            Me.btnReport.Image = Global.QSILib.My.Resources.Resources.PRINT
            Me.btnReport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnReport.Location = New System.Drawing.Point(0, 106)
            Me.btnReport.Name = "btnReport"
            Me.btnReport.Size = New System.Drawing.Size(65, 23)
            Me.btnReport.TabIndex = 11
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
            Me.btnExport1.Text = "Export"
            Me.btnExport1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.ToolTip1.SetToolTip(Me.btnExport1, "Put list contents in a spreadsheet")
            Me.btnExport1.UseVisualStyleBackColor = True
            '
            'flMain
            '
            Me.AcceptButton = Me.btnQuery
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
            Me.ClientSize = New System.Drawing.Size(776, 313)
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
            Me.Name = "flMain"
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
        Protected WithEvents tsSaveQuery As System.Windows.Forms.ToolStripButton
        Public WithEvents lblOnColumn As System.Windows.Forms.ToolStripLabel
        Public WithEvents cbColumns As System.Windows.Forms.ToolStripComboBox
        Protected WithEvents btnClear1 As System.Windows.Forms.Button
        Protected WithEvents btnExport1 As System.Windows.Forms.Button
        Protected WithEvents btnDefault As System.Windows.Forms.ToolStripButton
        Protected WithEvents ToolTip1 As System.Windows.Forms.ToolTip
        Protected WithEvents tsSaveLayout As System.Windows.Forms.ToolStripButton
    End Class

End Namespace