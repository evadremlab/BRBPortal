Namespace Windows.Forms

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class feMain2
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(feMain2))
            Me.btnNew = New System.Windows.Forms.Button()
            Me.btnDelete = New System.Windows.Forms.Button()
            Me.btnSave = New System.Windows.Forms.Button()
            Me.bnUtility = New System.Windows.Forms.BindingNavigator(Me.components)
            Me.BindingNavigatorAddNewItem = New System.Windows.Forms.ToolStripButton()
            Me.bnFirst = New System.Windows.Forms.ToolStripButton()
            Me.bnPrev = New System.Windows.Forms.ToolStripButton()
            Me.BindingNavigatorSeparator = New System.Windows.Forms.ToolStripSeparator()
            Me.bnNext = New System.Windows.Forms.ToolStripButton()
            Me.bnLast = New System.Windows.Forms.ToolStripButton()
            Me.BindingNavigatorSeparator2 = New System.Windows.Forms.ToolStripSeparator()
            Me.btnHelp = New System.Windows.Forms.ToolStripButton()
            Me.T_utilBindingNavigatorSaveItem = New System.Windows.Forms.ToolStripButton()
            Me.BindingNavigatorDeleteItem = New System.Windows.Forms.ToolStripButton()
            Me.StatusMsg = New System.Windows.Forms.ToolStripLabel()
            Me.btnDefault = New System.Windows.Forms.ToolStripButton()
            Me.ProgressMsg = New System.Windows.Forms.ToolStripLabel()
            Me.btnSQL = New System.Windows.Forms.ToolStripButton()
            Me.lblFTitle = New System.Windows.Forms.Label()
            Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
            Me.HelpMsg = New System.Windows.Forms.ToolStripStatusLabel()
            Me.txtDirtyDisplay = New QSILib.Windows.Forms.qTextBox()
            Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
            Me.btnSaveAndNew = New System.Windows.Forms.Button()
            Me.btnSaveAndNext = New System.Windows.Forms.Button()
            Me.btnSaveAndClose = New System.Windows.Forms.Button()
            Me.PanelLock = New System.Windows.Forms.Panel()
            Me.btnUnlock = New System.Windows.Forms.Button()
            Me.btnTxLock = New System.Windows.Forms.Button()
            Me.lblLock = New QSILib.Windows.Forms.qLabel()
            CType(Me.iDS, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.iEP, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.bnUtility, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.bnUtility.SuspendLayout()
            Me.StatusStrip1.SuspendLayout()
            Me.PanelLock.SuspendLayout()
            Me.SuspendLayout()
            '
            'btnNew
            '
            Me.btnNew.Image = Global.QSILib.My.Resources.Resources._NEW
            Me.btnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnNew.Location = New System.Drawing.Point(6, 58)
            Me.btnNew.Margin = New System.Windows.Forms.Padding(2)
            Me.btnNew.Name = "btnNew"
            Me.btnNew.Size = New System.Drawing.Size(25, 23)
            Me.btnNew.TabIndex = 900
            Me.btnNew.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.ToolTip1.SetToolTip(Me.btnNew, "Clear this form so I can add new information (<Ctrl> E)")
            Me.btnNew.UseVisualStyleBackColor = True
            '
            'btnDelete
            '
            Me.btnDelete.Image = Global.QSILib.My.Resources.Resources.trash2b
            Me.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnDelete.Location = New System.Drawing.Point(6, 82)
            Me.btnDelete.Margin = New System.Windows.Forms.Padding(2)
            Me.btnDelete.Name = "btnDelete"
            Me.btnDelete.Size = New System.Drawing.Size(25, 23)
            Me.btnDelete.TabIndex = 901
            Me.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.ToolTip1.SetToolTip(Me.btnDelete, "Permanently delete everything on this form (<Ctrl> D)")
            Me.btnDelete.UseVisualStyleBackColor = True
            '
            'btnSave
            '
            Me.btnSave.Image = Global.QSILib.My.Resources.Resources.SAVE
            Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnSave.Location = New System.Drawing.Point(6, 33)
            Me.btnSave.Margin = New System.Windows.Forms.Padding(2)
            Me.btnSave.Name = "btnSave"
            Me.btnSave.Size = New System.Drawing.Size(25, 23)
            Me.btnSave.TabIndex = 902
            Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.ToolTip1.SetToolTip(Me.btnSave, "Save everything on this form (<Ctrl> S)")
            Me.btnSave.UseVisualStyleBackColor = True
            '
            'bnUtility
            '
            Me.bnUtility.AddNewItem = Me.BindingNavigatorAddNewItem
            Me.bnUtility.BackColor = System.Drawing.Color.Transparent
            Me.bnUtility.CountItem = Nothing
            Me.bnUtility.DeleteItem = Nothing
            Me.bnUtility.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.bnFirst, Me.bnPrev, Me.BindingNavigatorSeparator, Me.bnNext, Me.bnLast, Me.BindingNavigatorSeparator2, Me.btnHelp, Me.T_utilBindingNavigatorSaveItem, Me.BindingNavigatorAddNewItem, Me.BindingNavigatorDeleteItem, Me.StatusMsg, Me.btnDefault, Me.ProgressMsg, Me.btnSQL})
            Me.bnUtility.Location = New System.Drawing.Point(0, 0)
            Me.bnUtility.MoveFirstItem = Nothing
            Me.bnUtility.MoveLastItem = Nothing
            Me.bnUtility.MoveNextItem = Nothing
            Me.bnUtility.MovePreviousItem = Nothing
            Me.bnUtility.Name = "bnUtility"
            Me.bnUtility.PositionItem = Nothing
            Me.bnUtility.Size = New System.Drawing.Size(399, 25)
            Me.bnUtility.TabIndex = 7
            Me.bnUtility.Text = "BindingNavigator1"
            '
            'BindingNavigatorAddNewItem
            '
            Me.BindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.BindingNavigatorAddNewItem.Image = CType(resources.GetObject("BindingNavigatorAddNewItem.Image"), System.Drawing.Image)
            Me.BindingNavigatorAddNewItem.Name = "BindingNavigatorAddNewItem"
            Me.BindingNavigatorAddNewItem.RightToLeftAutoMirrorImage = True
            Me.BindingNavigatorAddNewItem.Size = New System.Drawing.Size(23, 22)
            Me.BindingNavigatorAddNewItem.Text = "Add new"
            Me.BindingNavigatorAddNewItem.Visible = False
            '
            'bnFirst
            '
            Me.bnFirst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.bnFirst.Image = CType(resources.GetObject("bnFirst.Image"), System.Drawing.Image)
            Me.bnFirst.Name = "bnFirst"
            Me.bnFirst.RightToLeftAutoMirrorImage = True
            Me.bnFirst.Size = New System.Drawing.Size(23, 22)
            Me.bnFirst.Text = "Move first"
            '
            'bnPrev
            '
            Me.bnPrev.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.bnPrev.Image = CType(resources.GetObject("bnPrev.Image"), System.Drawing.Image)
            Me.bnPrev.Name = "bnPrev"
            Me.bnPrev.RightToLeftAutoMirrorImage = True
            Me.bnPrev.Size = New System.Drawing.Size(23, 22)
            Me.bnPrev.Text = "Move previous"
            '
            'BindingNavigatorSeparator
            '
            Me.BindingNavigatorSeparator.Name = "BindingNavigatorSeparator"
            Me.BindingNavigatorSeparator.Size = New System.Drawing.Size(6, 25)
            '
            'bnNext
            '
            Me.bnNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.bnNext.Image = CType(resources.GetObject("bnNext.Image"), System.Drawing.Image)
            Me.bnNext.Name = "bnNext"
            Me.bnNext.RightToLeftAutoMirrorImage = True
            Me.bnNext.Size = New System.Drawing.Size(23, 22)
            Me.bnNext.Text = "Move next"
            '
            'bnLast
            '
            Me.bnLast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.bnLast.Image = CType(resources.GetObject("bnLast.Image"), System.Drawing.Image)
            Me.bnLast.Name = "bnLast"
            Me.bnLast.RightToLeftAutoMirrorImage = True
            Me.bnLast.Size = New System.Drawing.Size(23, 22)
            Me.bnLast.Text = "Move last"
            '
            'BindingNavigatorSeparator2
            '
            Me.BindingNavigatorSeparator2.Name = "BindingNavigatorSeparator2"
            Me.BindingNavigatorSeparator2.Size = New System.Drawing.Size(6, 25)
            '
            'btnHelp
            '
            Me.btnHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.btnHelp.Enabled = False
            Me.btnHelp.Image = Global.QSILib.My.Resources.Resources.help8
            Me.btnHelp.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.btnHelp.Name = "btnHelp"
            Me.btnHelp.Size = New System.Drawing.Size(23, 22)
            Me.btnHelp.Text = "ToolStripButton1"
            Me.btnHelp.Visible = False
            '
            'T_utilBindingNavigatorSaveItem
            '
            Me.T_utilBindingNavigatorSaveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.T_utilBindingNavigatorSaveItem.Image = CType(resources.GetObject("T_utilBindingNavigatorSaveItem.Image"), System.Drawing.Image)
            Me.T_utilBindingNavigatorSaveItem.Name = "T_utilBindingNavigatorSaveItem"
            Me.T_utilBindingNavigatorSaveItem.Size = New System.Drawing.Size(23, 22)
            Me.T_utilBindingNavigatorSaveItem.Text = "Save Data"
            Me.T_utilBindingNavigatorSaveItem.Visible = False
            '
            'BindingNavigatorDeleteItem
            '
            Me.BindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.BindingNavigatorDeleteItem.Image = CType(resources.GetObject("BindingNavigatorDeleteItem.Image"), System.Drawing.Image)
            Me.BindingNavigatorDeleteItem.Name = "BindingNavigatorDeleteItem"
            Me.BindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = True
            Me.BindingNavigatorDeleteItem.Size = New System.Drawing.Size(23, 22)
            Me.BindingNavigatorDeleteItem.Text = "Delete"
            Me.BindingNavigatorDeleteItem.Visible = False
            '
            'StatusMsg
            '
            Me.StatusMsg.ForeColor = System.Drawing.Color.Red
            Me.StatusMsg.Name = "StatusMsg"
            Me.StatusMsg.Size = New System.Drawing.Size(0, 22)
            '
            'btnDefault
            '
            Me.btnDefault.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.btnDefault.Image = Global.QSILib.My.Resources.Resources.pen
            Me.btnDefault.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.btnDefault.Name = "btnDefault"
            Me.btnDefault.Size = New System.Drawing.Size(23, 22)
            Me.btnDefault.Text = "Make this my default function"
            Me.btnDefault.Visible = False
            '
            'ProgressMsg
            '
            Me.ProgressMsg.Font = New System.Drawing.Font("Tahoma", 8.400001!)
            Me.ProgressMsg.ForeColor = System.Drawing.Color.Red
            Me.ProgressMsg.Name = "ProgressMsg"
            Me.ProgressMsg.Size = New System.Drawing.Size(0, 22)
            '
            'btnSQL
            '
            Me.btnSQL.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.btnSQL.Image = Global.QSILib.My.Resources.Resources.SQL
            Me.btnSQL.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.btnSQL.Name = "btnSQL"
            Me.btnSQL.Size = New System.Drawing.Size(23, 22)
            Me.btnSQL.Text = "Show the SQL used in query."
            Me.btnSQL.Visible = False
            '
            'lblFTitle
            '
            Me.lblFTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblFTitle.AutoSize = True
            Me.lblFTitle.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblFTitle.ForeColor = System.Drawing.Color.Blue
            Me.lblFTitle.Location = New System.Drawing.Point(288, 0)
            Me.lblFTitle.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
            Me.lblFTitle.Name = "lblFTitle"
            Me.lblFTitle.Size = New System.Drawing.Size(113, 19)
            Me.lblFTitle.TabIndex = 20
            Me.lblFTitle.Text = "Function Title"
            '
            'StatusStrip1
            '
            Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HelpMsg})
            Me.StatusStrip1.Location = New System.Drawing.Point(0, 196)
            Me.StatusStrip1.Name = "StatusStrip1"
            Me.StatusStrip1.Padding = New System.Windows.Forms.Padding(1, 0, 10, 0)
            Me.StatusStrip1.Size = New System.Drawing.Size(399, 22)
            Me.StatusStrip1.TabIndex = 23
            Me.StatusStrip1.Text = "StatusStrip1"
            '
            'HelpMsg
            '
            Me.HelpMsg.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
            Me.HelpMsg.Name = "HelpMsg"
            Me.HelpMsg.Size = New System.Drawing.Size(0, 17)
            '
            'txtDirtyDisplay
            '
            Me.txtDirtyDisplay._Format = ""
            Me.txtDirtyDisplay._FormatNumber = ""
            Me.txtDirtyDisplay._IsKeyField = False
            Me.txtDirtyDisplay._ValidateMaxValue = ""
            Me.txtDirtyDisplay._ValidateMinValue = ""
            Me.txtDirtyDisplay.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
            Me.txtDirtyDisplay.BorderStyle = System.Windows.Forms.BorderStyle.None
            Me.txtDirtyDisplay.Enabled = False
            Me.txtDirtyDisplay.Location = New System.Drawing.Point(0, 33)
            Me.txtDirtyDisplay.Margin = New System.Windows.Forms.Padding(0)
            Me.txtDirtyDisplay.Name = "txtDirtyDisplay"
            Me.txtDirtyDisplay.ReadOnly = True
            Me.txtDirtyDisplay.Size = New System.Drawing.Size(4, 23)
            Me.txtDirtyDisplay.TabIndex = 980
            Me.txtDirtyDisplay.TabStop = False
            Me.txtDirtyDisplay.Visible = False
            '
            'ToolTip1
            '
            Me.ToolTip1.AutoPopDelay = 5000
            Me.ToolTip1.InitialDelay = 1000
            Me.ToolTip1.ReshowDelay = 500
            Me.ToolTip1.ShowAlways = True
            '
            'btnSaveAndNew
            '
            Me.btnSaveAndNew.Image = CType(resources.GetObject("btnSaveAndNew.Image"), System.Drawing.Image)
            Me.btnSaveAndNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnSaveAndNew.Location = New System.Drawing.Point(35, 58)
            Me.btnSaveAndNew.Margin = New System.Windows.Forms.Padding(2)
            Me.btnSaveAndNew.Name = "btnSaveAndNew"
            Me.btnSaveAndNew.Size = New System.Drawing.Size(40, 23)
            Me.btnSaveAndNew.TabIndex = 900
            Me.btnSaveAndNew.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.ToolTip1.SetToolTip(Me.btnSaveAndNew, "Save this record and create a new record Alt+Ctrl+S")
            Me.btnSaveAndNew.UseVisualStyleBackColor = True
            '
            'btnSaveAndNext
            '
            Me.btnSaveAndNext.Image = CType(resources.GetObject("btnSaveAndNext.Image"), System.Drawing.Image)
            Me.btnSaveAndNext.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnSaveAndNext.Location = New System.Drawing.Point(35, 33)
            Me.btnSaveAndNext.Margin = New System.Windows.Forms.Padding(2)
            Me.btnSaveAndNext.Name = "btnSaveAndNext"
            Me.btnSaveAndNext.Size = New System.Drawing.Size(40, 23)
            Me.btnSaveAndNext.TabIndex = 900
            Me.btnSaveAndNext.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.ToolTip1.SetToolTip(Me.btnSaveAndNext, "Save this record and move to the next record in the result list Shift+Ctrl+S")
            Me.btnSaveAndNext.UseVisualStyleBackColor = True
            '
            'btnSaveAndClose
            '
            Me.btnSaveAndClose.Image = CType(resources.GetObject("btnSaveAndClose.Image"), System.Drawing.Image)
            Me.btnSaveAndClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnSaveAndClose.Location = New System.Drawing.Point(35, 82)
            Me.btnSaveAndClose.Margin = New System.Windows.Forms.Padding(2)
            Me.btnSaveAndClose.Name = "btnSaveAndClose"
            Me.btnSaveAndClose.Size = New System.Drawing.Size(40, 23)
            Me.btnSaveAndClose.TabIndex = 900
            Me.btnSaveAndClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.ToolTip1.SetToolTip(Me.btnSaveAndClose, "Save this record and close the form Alt+Ctrl+C")
            Me.btnSaveAndClose.UseVisualStyleBackColor = True
            '
            'PanelLock
            '
            Me.PanelLock.Controls.Add(Me.btnUnlock)
            Me.PanelLock.Controls.Add(Me.btnTxLock)
            Me.PanelLock.Controls.Add(Me.lblLock)
            Me.PanelLock.Location = New System.Drawing.Point(146, 166)
            Me.PanelLock.Name = "PanelLock"
            Me.PanelLock.Size = New System.Drawing.Size(253, 28)
            Me.PanelLock.TabIndex = 984
            Me.PanelLock.Visible = False
            '
            'btnUnlock
            '
            Me.btnUnlock.Location = New System.Drawing.Point(197, 3)
            Me.btnUnlock.Name = "btnUnlock"
            Me.btnUnlock.Size = New System.Drawing.Size(54, 22)
            Me.btnUnlock.TabIndex = 983
            Me.btnUnlock.Text = "Unlock"
            Me.btnUnlock.UseVisualStyleBackColor = True
            Me.btnUnlock.Visible = False
            '
            'btnTxLock
            '
            Me.btnTxLock.Location = New System.Drawing.Point(139, 3)
            Me.btnTxLock.Name = "btnTxLock"
            Me.btnTxLock.Size = New System.Drawing.Size(54, 22)
            Me.btnTxLock.TabIndex = 981
            Me.btnTxLock.Text = "Tx Lock"
            Me.btnTxLock.UseVisualStyleBackColor = True
            Me.btnTxLock.Visible = False
            '
            'lblLock
            '
            Me.lblLock.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblLock.ForeColor = System.Drawing.Color.Blue
            Me.lblLock.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.lblLock.Location = New System.Drawing.Point(8, 7)
            Me.lblLock.Name = "lblLock"
            Me.lblLock.Size = New System.Drawing.Size(126, 14)
            Me.lblLock.TabIndex = 982
            Me.lblLock.Text = "Locked by smalani in NUI 204"
            Me.lblLock.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.lblLock.Visible = False
            '
            'feMain2
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
            Me.ClientSize = New System.Drawing.Size(399, 218)
            Me.Controls.Add(Me.PanelLock)
            Me.Controls.Add(Me.txtDirtyDisplay)
            Me.Controls.Add(Me.StatusStrip1)
            Me.Controls.Add(Me.lblFTitle)
            Me.Controls.Add(Me.btnSaveAndNext)
            Me.Controls.Add(Me.btnSaveAndClose)
            Me.Controls.Add(Me.btnSaveAndNew)
            Me.Controls.Add(Me.btnNew)
            Me.Controls.Add(Me.btnDelete)
            Me.Controls.Add(Me.btnSave)
            Me.Controls.Add(Me.bnUtility)
            Me.Margin = New System.Windows.Forms.Padding(2)
            Me.Name = "feMain2"
            Me.Text = "feMain"
            CType(Me.iDS, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.iEP, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.bnUtility, System.ComponentModel.ISupportInitialize).EndInit()
            Me.bnUtility.ResumeLayout(False)
            Me.bnUtility.PerformLayout()
            Me.StatusStrip1.ResumeLayout(False)
            Me.StatusStrip1.PerformLayout()
            Me.PanelLock.ResumeLayout(False)
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents BindingNavigatorSeparator As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents BindingNavigatorSeparator2 As System.Windows.Forms.ToolStripSeparator
        Protected WithEvents btnNew As System.Windows.Forms.Button
        Protected WithEvents BindingNavigatorAddNewItem As System.Windows.Forms.ToolStripButton
        Protected WithEvents BindingNavigatorDeleteItem As System.Windows.Forms.ToolStripButton
        Protected WithEvents btnDelete As System.Windows.Forms.Button
        Protected WithEvents btnSave As System.Windows.Forms.Button
        Protected WithEvents bnFirst As System.Windows.Forms.ToolStripButton
        Protected WithEvents bnPrev As System.Windows.Forms.ToolStripButton
        Protected WithEvents bnNext As System.Windows.Forms.ToolStripButton
        Protected WithEvents bnLast As System.Windows.Forms.ToolStripButton
        Protected WithEvents T_utilBindingNavigatorSaveItem As System.Windows.Forms.ToolStripButton
        Protected WithEvents lblFTitle As System.Windows.Forms.Label
        Protected WithEvents StatusMsg As System.Windows.Forms.ToolStripLabel
        Friend WithEvents btnHelp As System.Windows.Forms.ToolStripButton
        Protected Friend WithEvents bnUtility As System.Windows.Forms.BindingNavigator
        Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
        Friend WithEvents HelpMsg As System.Windows.Forms.ToolStripStatusLabel
        Protected WithEvents ProgressMsg As System.Windows.Forms.ToolStripLabel
        Protected WithEvents txtDirtyDisplay As QSILib.Windows.Forms.qTextBox
        Protected WithEvents ToolTip1 As System.Windows.Forms.ToolTip
        Protected WithEvents btnDefault As System.Windows.Forms.ToolStripButton
        Protected WithEvents btnSQL As System.Windows.Forms.ToolStripButton
        Protected WithEvents btnSaveAndNew As System.Windows.Forms.Button
        Protected WithEvents btnSaveAndNext As System.Windows.Forms.Button
        Protected WithEvents btnSaveAndClose As System.Windows.Forms.Button
        Public WithEvents PanelLock As System.Windows.Forms.Panel
        Public WithEvents btnUnlock As System.Windows.Forms.Button
        Public WithEvents btnTxLock As System.Windows.Forms.Button
        Public WithEvents lblLock As QSILib.Windows.Forms.qLabel
    End Class

End Namespace