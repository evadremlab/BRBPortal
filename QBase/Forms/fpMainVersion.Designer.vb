Namespace Windows.Forms

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class fpMainVersion
        Inherits fBase

        'comment
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(fpMainVersion))
            Me.btnRun = New System.Windows.Forms.Button()
            Me.lblFTitle = New System.Windows.Forms.Label()
            Me.lblOneMoment = New System.Windows.Forms.Label()
            Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
            Me.HelpMsg = New System.Windows.Forms.ToolStripStatusLabel()
            Me.StatusMsg = New System.Windows.Forms.Label()
            Me.ProgressMsg = New System.Windows.Forms.Label()
            Me.btnSave = New System.Windows.Forms.Button()
            Me.btnDelete = New System.Windows.Forms.Button()
            Me.cbVersion = New QSILib.Windows.Forms.qComboBox()
            Me.fpMainVersionLabel = New QSILib.Windows.Forms.qLabel()
            Me.QTimer1 = New QSILib.qTimer()
            Me.btnClear = New System.Windows.Forms.Button()
            Me.Label11 = New System.Windows.Forms.Label()
            Me.cbPartition = New QSILib.Windows.Forms.qComboBox()
            Me.bnList = New System.Windows.Forms.BindingNavigator(Me.components)
            Me.PrintPreviewToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.BindingNavigatorDeleteItem = New System.Windows.Forms.ToolStripButton()
            Me.T_utilBindingNavigatorSaveItem = New System.Windows.Forms.ToolStripButton()
            Me.ToolStripLabel2 = New System.Windows.Forms.ToolStripLabel()
            Me.btnDefault = New System.Windows.Forms.ToolStripButton()
            Me.btnSearchHelp = New System.Windows.Forms.Button()
            CType(Me.iDS, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.iEP, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.StatusStrip1.SuspendLayout()
            CType(Me.bnList, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.bnList.SuspendLayout()
            Me.SuspendLayout()
            '
            'btnRun
            '
            Me.btnRun.Image = Global.QSILib.My.Resources.Resources.Run
            Me.btnRun.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnRun.Location = New System.Drawing.Point(2, 40)
            Me.btnRun.Margin = New System.Windows.Forms.Padding(2)
            Me.btnRun.Name = "btnRun"
            Me.btnRun.Size = New System.Drawing.Size(65, 23)
            Me.btnRun.TabIndex = 13
            Me.btnRun.TabStop = False
            Me.btnRun.Text = "Run"
            Me.btnRun.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.btnRun.UseVisualStyleBackColor = True
            '
            'lblFTitle
            '
            Me.lblFTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblFTitle.AutoSize = True
            Me.lblFTitle.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblFTitle.ForeColor = System.Drawing.Color.Blue
            Me.lblFTitle.Location = New System.Drawing.Point(309, 1)
            Me.lblFTitle.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
            Me.lblFTitle.Name = "lblFTitle"
            Me.lblFTitle.Size = New System.Drawing.Size(176, 19)
            Me.lblFTitle.TabIndex = 19
            Me.lblFTitle.Text = "Report or Update Title"
            Me.lblFTitle.TextAlign = System.Drawing.ContentAlignment.TopRight
            '
            'lblOneMoment
            '
            Me.lblOneMoment.AutoSize = True
            Me.lblOneMoment.ForeColor = System.Drawing.Color.Red
            Me.lblOneMoment.Location = New System.Drawing.Point(2, 4)
            Me.lblOneMoment.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
            Me.lblOneMoment.Name = "lblOneMoment"
            Me.lblOneMoment.Size = New System.Drawing.Size(77, 13)
            Me.lblOneMoment.TabIndex = 20
            Me.lblOneMoment.Text = "One Moment..."
            Me.lblOneMoment.Visible = False
            '
            'StatusStrip1
            '
            Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HelpMsg})
            Me.StatusStrip1.Location = New System.Drawing.Point(0, 404)
            Me.StatusStrip1.Name = "StatusStrip1"
            Me.StatusStrip1.Padding = New System.Windows.Forms.Padding(1, 0, 10, 0)
            Me.StatusStrip1.Size = New System.Drawing.Size(488, 22)
            Me.StatusStrip1.TabIndex = 24
            Me.StatusStrip1.Text = "StatusStrip1"
            '
            'HelpMsg
            '
            Me.HelpMsg.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(231, Byte), Integer), CType(CType(246, Byte), Integer))
            Me.HelpMsg.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
            Me.HelpMsg.Name = "HelpMsg"
            Me.HelpMsg.Size = New System.Drawing.Size(0, 17)
            '
            'StatusMsg
            '
            Me.StatusMsg.AutoSize = True
            Me.StatusMsg.ForeColor = System.Drawing.Color.Red
            Me.StatusMsg.Location = New System.Drawing.Point(82, 4)
            Me.StatusMsg.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
            Me.StatusMsg.Name = "StatusMsg"
            Me.StatusMsg.Size = New System.Drawing.Size(0, 13)
            Me.StatusMsg.TabIndex = 25
            '
            'ProgressMsg
            '
            Me.ProgressMsg.AutoSize = True
            Me.ProgressMsg.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.ProgressMsg.ForeColor = System.Drawing.Color.Red
            Me.ProgressMsg.Location = New System.Drawing.Point(-1, 5)
            Me.ProgressMsg.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
            Me.ProgressMsg.Name = "ProgressMsg"
            Me.ProgressMsg.Size = New System.Drawing.Size(0, 13)
            Me.ProgressMsg.TabIndex = 26
            '
            'btnSave
            '
            Me.btnSave.Image = Global.QSILib.My.Resources.Resources.SAVE
            Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnSave.Location = New System.Drawing.Point(133, 367)
            Me.btnSave.Margin = New System.Windows.Forms.Padding(2)
            Me.btnSave.Name = "btnSave"
            Me.btnSave.Size = New System.Drawing.Size(101, 23)
            Me.btnSave.TabIndex = 27
            Me.btnSave.Text = "Save Version"
            Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.btnSave.UseVisualStyleBackColor = True
            '
            'btnDelete
            '
            Me.btnDelete.Image = Global.QSILib.My.Resources.Resources.trash2b
            Me.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnDelete.Location = New System.Drawing.Point(240, 367)
            Me.btnDelete.Margin = New System.Windows.Forms.Padding(2)
            Me.btnDelete.Name = "btnDelete"
            Me.btnDelete.Size = New System.Drawing.Size(101, 23)
            Me.btnDelete.TabIndex = 28
            Me.btnDelete.Text = "Delete Version"
            Me.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.btnDelete.UseVisualStyleBackColor = True
            '
            'cbVersion
            '
            Me.cbVersion._IsBrowseDD = False
            Me.cbVersion._IsKeyField = False
            Me.cbVersion._ToolTip = "Choose a report version (allows you to choose a prior report specification, if an" & _
        "y have been saved.)"
            Me.cbVersion._ValidateMaxValue = ""
            Me.cbVersion._ValidateMinValue = ""
            Me.cbVersion.FormattingEnabled = True
            Me.cbVersion.Location = New System.Drawing.Point(232, 40)
            Me.cbVersion.Margin = New System.Windows.Forms.Padding(2)
            Me.cbVersion.Name = "cbVersion"
            Me.cbVersion.Size = New System.Drawing.Size(242, 21)
            Me.cbVersion.TabIndex = 29
            '
            'fpMainVersionLabel
            '
            Me.fpMainVersionLabel.Location = New System.Drawing.Point(182, 41)
            Me.fpMainVersionLabel.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
            Me.fpMainVersionLabel.Name = "fpMainVersionLabel"
            Me.fpMainVersionLabel.Size = New System.Drawing.Size(46, 20)
            Me.fpMainVersionLabel.TabIndex = 30
            Me.fpMainVersionLabel.Text = "Version*"
            Me.fpMainVersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'QTimer1
            '
            Me.QTimer1._EventName = Nothing
            Me.QTimer1._Param = Nothing
            '
            'btnClear
            '
            Me.btnClear.Image = Global.QSILib.My.Resources.Resources.Clear
            Me.btnClear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnClear.Location = New System.Drawing.Point(2, 67)
            Me.btnClear.Name = "btnClear"
            Me.btnClear.Size = New System.Drawing.Size(65, 23)
            Me.btnClear.TabIndex = 31
            Me.btnClear.Text = "Clear"
            Me.btnClear.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.btnClear.UseVisualStyleBackColor = True
            '
            'Label11
            '
            Me.Label11.Location = New System.Drawing.Point(159, 64)
            Me.Label11.Name = "Label11"
            Me.Label11.Size = New System.Drawing.Size(69, 21)
            Me.Label11.TabIndex = 65
            Me.Label11.Text = "Partition*"
            Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'cbPartition
            '
            Me.cbPartition._IsBrowseDD = False
            Me.cbPartition._IsKeyField = False
            Me.cbPartition._ToolTip = "Choose the partition you want to search."
            Me.cbPartition._ValidateMaxValue = ""
            Me.cbPartition._ValidateMinValue = ""
            Me.cbPartition._ValidateRequired = True
            Me.cbPartition.FormattingEnabled = True
            Me.cbPartition.Location = New System.Drawing.Point(232, 64)
            Me.cbPartition.Name = "cbPartition"
            Me.cbPartition.Size = New System.Drawing.Size(95, 21)
            Me.cbPartition.TabIndex = 64
            '
            'bnList
            '
            Me.bnList.AddNewItem = Nothing
            Me.bnList.AutoSize = False
            Me.bnList.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
            Me.bnList.CountItem = Nothing
            Me.bnList.DeleteItem = Nothing
            Me.bnList.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PrintPreviewToolStripButton, Me.BindingNavigatorDeleteItem, Me.T_utilBindingNavigatorSaveItem, Me.ToolStripLabel2, Me.btnDefault})
            Me.bnList.Location = New System.Drawing.Point(0, 0)
            Me.bnList.MoveFirstItem = Nothing
            Me.bnList.MoveLastItem = Nothing
            Me.bnList.MoveNextItem = Nothing
            Me.bnList.MovePreviousItem = Nothing
            Me.bnList.Name = "bnList"
            Me.bnList.PositionItem = Nothing
            Me.bnList.Size = New System.Drawing.Size(488, 26)
            Me.bnList.TabIndex = 66
            Me.bnList.Text = "BindingNavigator1"
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
            'ToolStripLabel2
            '
            Me.ToolStripLabel2.ForeColor = System.Drawing.Color.Red
            Me.ToolStripLabel2.Name = "ToolStripLabel2"
            Me.ToolStripLabel2.Size = New System.Drawing.Size(0, 23)
            '
            'btnDefault
            '
            Me.btnDefault.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.btnDefault.Image = Global.QSILib.My.Resources.Resources.pen
            Me.btnDefault.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.btnDefault.Name = "btnDefault"
            Me.btnDefault.Size = New System.Drawing.Size(23, 23)
            Me.btnDefault.Text = "Make this my default function"
            '
            'btnSearchHelp
            '
            Me.btnSearchHelp.FlatAppearance.BorderSize = 0
            Me.btnSearchHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnSearchHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 5.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnSearchHelp.Image = Global.QSILib.My.Resources.Resources.help8
            Me.btnSearchHelp.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.btnSearchHelp.Location = New System.Drawing.Point(69, 40)
            Me.btnSearchHelp.Margin = New System.Windows.Forms.Padding(2)
            Me.btnSearchHelp.Name = "btnSearchHelp"
            Me.btnSearchHelp.Size = New System.Drawing.Size(19, 23)
            Me.btnSearchHelp.TabIndex = 67
            Me.btnSearchHelp.TabStop = False
            Me.btnSearchHelp.Text = " "
            Me.btnSearchHelp.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.btnSearchHelp.UseVisualStyleBackColor = True
            '
            'fpMainVersion
            '
            Me.AcceptButton = Me.btnRun
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
            Me.ClientSize = New System.Drawing.Size(488, 426)
            Me.Controls.Add(Me.btnSearchHelp)
            Me.Controls.Add(Me.bnList)
            Me.Controls.Add(Me.Label11)
            Me.Controls.Add(Me.cbPartition)
            Me.Controls.Add(Me.btnClear)
            Me.Controls.Add(Me.fpMainVersionLabel)
            Me.Controls.Add(Me.cbVersion)
            Me.Controls.Add(Me.btnDelete)
            Me.Controls.Add(Me.btnSave)
            Me.Controls.Add(Me.ProgressMsg)
            Me.Controls.Add(Me.StatusMsg)
            Me.Controls.Add(Me.StatusStrip1)
            Me.Controls.Add(Me.lblOneMoment)
            Me.Controls.Add(Me.lblFTitle)
            Me.Controls.Add(Me.btnRun)
            Me.Margin = New System.Windows.Forms.Padding(2)
            Me.Name = "fpMainVersion"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "Report Options ()"
            CType(Me.iDS, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.iEP, System.ComponentModel.ISupportInitialize).EndInit()
            Me.StatusStrip1.ResumeLayout(False)
            Me.StatusStrip1.PerformLayout()
            CType(Me.bnList, System.ComponentModel.ISupportInitialize).EndInit()
            Me.bnList.ResumeLayout(False)
            Me.bnList.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Protected WithEvents lblFTitle As System.Windows.Forms.Label
        Protected WithEvents lblOneMoment As System.Windows.Forms.Label
        Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
        Friend WithEvents HelpMsg As System.Windows.Forms.ToolStripStatusLabel
        Protected WithEvents StatusMsg As System.Windows.Forms.Label
        Protected WithEvents ProgressMsg As System.Windows.Forms.Label
        Protected WithEvents btnSave As System.Windows.Forms.Button
        Protected WithEvents btnDelete As System.Windows.Forms.Button
        Protected WithEvents fpMainVersionLabel As qLabel
        Friend WithEvents QTimer1 As qTimer
        Public WithEvents cbVersion As qComboBox
        Public WithEvents cbPartition As QSILib.Windows.Forms.qComboBox
        Protected WithEvents btnClear As System.Windows.Forms.Button
        Public WithEvents bnList As System.Windows.Forms.BindingNavigator
        Public WithEvents PrintPreviewToolStripButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents BindingNavigatorDeleteItem As System.Windows.Forms.ToolStripButton
        Public WithEvents T_utilBindingNavigatorSaveItem As System.Windows.Forms.ToolStripButton
        Public WithEvents ToolStripLabel2 As System.Windows.Forms.ToolStripLabel
        Protected WithEvents btnDefault As System.Windows.Forms.ToolStripButton
        Public WithEvents btnRun As System.Windows.Forms.Button
        Public WithEvents Label11 As System.Windows.Forms.Label
        Protected WithEvents btnSearchHelp As System.Windows.Forms.Button
    End Class

End Namespace