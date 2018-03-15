Namespace Windows.Forms

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class feMain
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(feMain))
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
            CType(Me.iDS, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.iEP, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.bnUtility, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.bnUtility.SuspendLayout()
            Me.StatusStrip1.SuspendLayout()
            Me.SuspendLayout()
            '
            'btnNew
            '
            Me.btnNew.Image = Global.QSILib.My.Resources.Resources._NEW
            Me.btnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnNew.Location = New System.Drawing.Point(9, 51)
            Me.btnNew.Name = "btnNew"
            Me.btnNew.Size = New System.Drawing.Size(98, 35)
            Me.btnNew.TabIndex = 900
            Me.btnNew.Text = "N&ew"
            Me.btnNew.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.ToolTip1.SetToolTip(Me.btnNew, "Clear this form so I can add new information (<Ctrl> E)")
            Me.btnNew.UseVisualStyleBackColor = True
            '
            'btnDelete
            '
            Me.btnDelete.Image = Global.QSILib.My.Resources.Resources.trash2b
            Me.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnDelete.Location = New System.Drawing.Point(9, 89)
            Me.btnDelete.Name = "btnDelete"
            Me.btnDelete.Size = New System.Drawing.Size(98, 35)
            Me.btnDelete.TabIndex = 901
            Me.btnDelete.Text = "&Delete"
            Me.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.ToolTip1.SetToolTip(Me.btnDelete, "Permanently delete everything on this form (<Ctrl> D)")
            Me.btnDelete.UseVisualStyleBackColor = True
            '
            'btnSave
            '
            Me.btnSave.Image = Global.QSILib.My.Resources.Resources.SAVE
            Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnSave.Location = New System.Drawing.Point(9, 126)
            Me.btnSave.Name = "btnSave"
            Me.btnSave.Size = New System.Drawing.Size(98, 35)
            Me.btnSave.TabIndex = 902
            Me.btnSave.Text = "&Save"
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
            Me.bnUtility.ImageScalingSize = New System.Drawing.Size(32, 32)
            Me.bnUtility.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.bnFirst, Me.bnPrev, Me.BindingNavigatorSeparator, Me.bnNext, Me.bnLast, Me.BindingNavigatorSeparator2, Me.btnHelp, Me.T_utilBindingNavigatorSaveItem, Me.BindingNavigatorAddNewItem, Me.BindingNavigatorDeleteItem, Me.StatusMsg, Me.btnDefault, Me.ProgressMsg, Me.btnSQL})
            Me.bnUtility.Location = New System.Drawing.Point(0, 0)
            Me.bnUtility.MoveFirstItem = Nothing
            Me.bnUtility.MoveLastItem = Nothing
            Me.bnUtility.MoveNextItem = Nothing
            Me.bnUtility.MovePreviousItem = Nothing
            Me.bnUtility.Name = "bnUtility"
            Me.bnUtility.Padding = New System.Windows.Forms.Padding(0, 0, 2, 0)
            Me.bnUtility.PositionItem = Nothing
            Me.bnUtility.Size = New System.Drawing.Size(598, 39)
            Me.bnUtility.TabIndex = 7
            Me.bnUtility.Text = "BindingNavigator1"
            '
            'BindingNavigatorAddNewItem
            '
            Me.BindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.BindingNavigatorAddNewItem.Image = CType(resources.GetObject("BindingNavigatorAddNewItem.Image"), System.Drawing.Image)
            Me.BindingNavigatorAddNewItem.Name = "BindingNavigatorAddNewItem"
            Me.BindingNavigatorAddNewItem.RightToLeftAutoMirrorImage = True
            Me.BindingNavigatorAddNewItem.Size = New System.Drawing.Size(36, 36)
            Me.BindingNavigatorAddNewItem.Text = "Add new"
            Me.BindingNavigatorAddNewItem.Visible = False
            '
            'bnFirst
            '
            Me.bnFirst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.bnFirst.Image = CType(resources.GetObject("bnFirst.Image"), System.Drawing.Image)
            Me.bnFirst.Name = "bnFirst"
            Me.bnFirst.RightToLeftAutoMirrorImage = True
            Me.bnFirst.Size = New System.Drawing.Size(36, 36)
            Me.bnFirst.Text = "Move first"
            '
            'bnPrev
            '
            Me.bnPrev.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.bnPrev.Image = CType(resources.GetObject("bnPrev.Image"), System.Drawing.Image)
            Me.bnPrev.Name = "bnPrev"
            Me.bnPrev.RightToLeftAutoMirrorImage = True
            Me.bnPrev.Size = New System.Drawing.Size(36, 36)
            Me.bnPrev.Text = "Move previous"
            '
            'BindingNavigatorSeparator
            '
            Me.BindingNavigatorSeparator.Name = "BindingNavigatorSeparator"
            Me.BindingNavigatorSeparator.Size = New System.Drawing.Size(6, 39)
            '
            'bnNext
            '
            Me.bnNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.bnNext.Image = CType(resources.GetObject("bnNext.Image"), System.Drawing.Image)
            Me.bnNext.Name = "bnNext"
            Me.bnNext.RightToLeftAutoMirrorImage = True
            Me.bnNext.Size = New System.Drawing.Size(36, 36)
            Me.bnNext.Text = "Move next"
            '
            'bnLast
            '
            Me.bnLast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.bnLast.Image = CType(resources.GetObject("bnLast.Image"), System.Drawing.Image)
            Me.bnLast.Name = "bnLast"
            Me.bnLast.RightToLeftAutoMirrorImage = True
            Me.bnLast.Size = New System.Drawing.Size(36, 36)
            Me.bnLast.Text = "Move last"
            '
            'BindingNavigatorSeparator2
            '
            Me.BindingNavigatorSeparator2.Name = "BindingNavigatorSeparator2"
            Me.BindingNavigatorSeparator2.Size = New System.Drawing.Size(6, 39)
            '
            'btnHelp
            '
            Me.btnHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.btnHelp.Enabled = False
            Me.btnHelp.Image = Global.QSILib.My.Resources.Resources.help8
            Me.btnHelp.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.btnHelp.Name = "btnHelp"
            Me.btnHelp.Size = New System.Drawing.Size(36, 36)
            Me.btnHelp.Text = "ToolStripButton1"
            Me.btnHelp.Visible = False
            '
            'T_utilBindingNavigatorSaveItem
            '
            Me.T_utilBindingNavigatorSaveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.T_utilBindingNavigatorSaveItem.Image = CType(resources.GetObject("T_utilBindingNavigatorSaveItem.Image"), System.Drawing.Image)
            Me.T_utilBindingNavigatorSaveItem.Name = "T_utilBindingNavigatorSaveItem"
            Me.T_utilBindingNavigatorSaveItem.Size = New System.Drawing.Size(36, 36)
            Me.T_utilBindingNavigatorSaveItem.Text = "Save Data"
            Me.T_utilBindingNavigatorSaveItem.Visible = False
            '
            'BindingNavigatorDeleteItem
            '
            Me.BindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.BindingNavigatorDeleteItem.Image = CType(resources.GetObject("BindingNavigatorDeleteItem.Image"), System.Drawing.Image)
            Me.BindingNavigatorDeleteItem.Name = "BindingNavigatorDeleteItem"
            Me.BindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = True
            Me.BindingNavigatorDeleteItem.Size = New System.Drawing.Size(36, 36)
            Me.BindingNavigatorDeleteItem.Text = "Delete"
            Me.BindingNavigatorDeleteItem.Visible = False
            '
            'StatusMsg
            '
            Me.StatusMsg.ForeColor = System.Drawing.Color.Red
            Me.StatusMsg.Name = "StatusMsg"
            Me.StatusMsg.Size = New System.Drawing.Size(0, 36)
            '
            'btnDefault
            '
            Me.btnDefault.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.btnDefault.Image = Global.QSILib.My.Resources.Resources.pen
            Me.btnDefault.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.btnDefault.Name = "btnDefault"
            Me.btnDefault.Size = New System.Drawing.Size(36, 36)
            Me.btnDefault.Text = "Make this my default function"
            Me.btnDefault.Visible = False
            '
            'ProgressMsg
            '
            Me.ProgressMsg.Font = New System.Drawing.Font("Tahoma", 8.400001!)
            Me.ProgressMsg.ForeColor = System.Drawing.Color.Red
            Me.ProgressMsg.Name = "ProgressMsg"
            Me.ProgressMsg.Size = New System.Drawing.Size(0, 36)
            '
            'btnSQL
            '
            Me.btnSQL.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.btnSQL.Image = Global.QSILib.My.Resources.Resources.SQL
            Me.btnSQL.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.btnSQL.Name = "btnSQL"
            Me.btnSQL.Size = New System.Drawing.Size(36, 36)
            Me.btnSQL.Text = "Show the SQL used in query."
            Me.btnSQL.Visible = False
            '
            'lblFTitle
            '
            Me.lblFTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblFTitle.AutoSize = True
            Me.lblFTitle.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblFTitle.ForeColor = System.Drawing.Color.Blue
            Me.lblFTitle.Location = New System.Drawing.Point(432, 0)
            Me.lblFTitle.Name = "lblFTitle"
            Me.lblFTitle.Size = New System.Drawing.Size(229, 37)
            Me.lblFTitle.TabIndex = 20
            Me.lblFTitle.Text = "Function Title"
            '
            'StatusStrip1
            '
            Me.StatusStrip1.ImageScalingSize = New System.Drawing.Size(32, 32)
            Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HelpMsg})
            Me.StatusStrip1.Location = New System.Drawing.Point(0, 313)
            Me.StatusStrip1.Name = "StatusStrip1"
            Me.StatusStrip1.Padding = New System.Windows.Forms.Padding(2, 0, 15, 0)
            Me.StatusStrip1.Size = New System.Drawing.Size(598, 22)
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
            Me.txtDirtyDisplay.Location = New System.Drawing.Point(0, 51)
            Me.txtDirtyDisplay.Margin = New System.Windows.Forms.Padding(0)
            Me.txtDirtyDisplay.Name = "txtDirtyDisplay"
            Me.txtDirtyDisplay.ReadOnly = True
            Me.txtDirtyDisplay.Size = New System.Drawing.Size(6, 35)
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
            'feMain
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.AutoSize = True
            Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
            Me.ClientSize = New System.Drawing.Size(598, 335)
            Me.Controls.Add(Me.txtDirtyDisplay)
            Me.Controls.Add(Me.StatusStrip1)
            Me.Controls.Add(Me.lblFTitle)
            Me.Controls.Add(Me.btnNew)
            Me.Controls.Add(Me.btnDelete)
            Me.Controls.Add(Me.btnSave)
            Me.Controls.Add(Me.bnUtility)
            Me.Margin = New System.Windows.Forms.Padding(3, 3, 3, 3)
            Me.Name = "feMain"
            Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
            Me.Text = "feMain"
            CType(Me.iDS, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.iEP, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.bnUtility, System.ComponentModel.ISupportInitialize).EndInit()
            Me.bnUtility.ResumeLayout(False)
            Me.bnUtility.PerformLayout()
            Me.StatusStrip1.ResumeLayout(False)
            Me.StatusStrip1.PerformLayout()
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
    End Class

End Namespace