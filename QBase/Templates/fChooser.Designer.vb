Namespace Windows.Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class fChooser
        Inherits System.Windows.Forms.Form



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
            Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Me.btnCancel = New System.Windows.Forms.Button()
            Me.btnAccept = New System.Windows.Forms.Button()
            Me.FilterTimer = New System.Windows.Forms.Timer(Me.components)
            Me.btnAdd = New System.Windows.Forms.Button()
            Me.gvNonActive = New QSILib.qGVList()
            Me.gvActive = New QSILib.qGVList()
            Me.btnDeActivate = New System.Windows.Forms.Button()
            Me.btnActivate = New System.Windows.Forms.Button()
            Me.btnMoveDown = New System.Windows.Forms.Button()
            Me.btnMoveUp = New System.Windows.Forms.Button()
            Me.availfield = New QSILib.qGVTextBoxColumn()
            Me.selfields = New QSILib.qGVTextBoxColumn()
            CType(Me.gvNonActive, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.gvActive, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'btnCancel
            '
            Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnCancel.Image = Global.QSILib.My.Resources.Resources.DELETE
            Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnCancel.Location = New System.Drawing.Point(275, 339)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(79, 26)
            Me.btnCancel.TabIndex = 22
            Me.btnCancel.Text = "Cancel"
            Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.btnCancel.UseVisualStyleBackColor = True
            '
            'btnAccept
            '
            Me.btnAccept.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnAccept.Image = Global.QSILib.My.Resources.Resources.PASTE
            Me.btnAccept.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnAccept.Location = New System.Drawing.Point(190, 339)
            Me.btnAccept.Name = "btnAccept"
            Me.btnAccept.Size = New System.Drawing.Size(79, 26)
            Me.btnAccept.TabIndex = 21
            Me.btnAccept.Text = "Accept"
            Me.btnAccept.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.btnAccept.UseVisualStyleBackColor = True
            '
            'FilterTimer
            '
            '
            'btnAdd
            '
            Me.btnAdd.Image = Global.QSILib.My.Resources.Resources._NEW
            Me.btnAdd.Location = New System.Drawing.Point(551, 0)
            Me.btnAdd.Name = "btnAdd"
            Me.btnAdd.Size = New System.Drawing.Size(18, 23)
            Me.btnAdd.TabIndex = 29
            Me.btnAdd.UseVisualStyleBackColor = True
            Me.btnAdd.Visible = False
            '
            'gvNonActive
            '
            Me.gvNonActive._GVFoot = Nothing
            Me.gvNonActive._ShowSelectionBar = True
            Me.gvNonActive.AllowUserToAddRows = False
            Me.gvNonActive.AllowUserToDeleteRows = False
            Me.gvNonActive.AllowUserToOrderColumns = True
            DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
            DataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black
            Me.gvNonActive.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
            Me.gvNonActive.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None
            Me.gvNonActive.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.None
            Me.gvNonActive.BackgroundColor = System.Drawing.Color.White
            DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
            DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
            DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.gvNonActive.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
            Me.gvNonActive.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.gvNonActive.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.availfield})
            Me.gvNonActive.Location = New System.Drawing.Point(12, 12)
            Me.gvNonActive.MultiSelect = False
            Me.gvNonActive.Name = "gvNonActive"
            Me.gvNonActive.ReadOnly = True
            Me.gvNonActive.RowHeadersVisible = False
            DataGridViewCellStyle3.BackColor = System.Drawing.Color.White
            DataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black
            Me.gvNonActive.RowsDefaultCellStyle = DataGridViewCellStyle3
            Me.gvNonActive.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(180, Byte), Integer))
            Me.gvNonActive.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black
            Me.gvNonActive.RowTemplate.Height = 21
            Me.gvNonActive.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.gvNonActive.Size = New System.Drawing.Size(214, 262)
            Me.gvNonActive.TabIndex = 30
            '
            'gvActive
            '
            Me.gvActive._GVFoot = Nothing
            Me.gvActive._ShowSelectionBar = True
            Me.gvActive.AllowUserToAddRows = False
            Me.gvActive.AllowUserToDeleteRows = False
            Me.gvActive.AllowUserToOrderColumns = True
            DataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
            DataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black
            Me.gvActive.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle4
            Me.gvActive.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.gvActive.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None
            Me.gvActive.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.None
            Me.gvActive.BackgroundColor = System.Drawing.Color.White
            DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
            DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText
            DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.gvActive.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle5
            Me.gvActive.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.gvActive.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.selfields})
            Me.gvActive.Location = New System.Drawing.Point(321, 12)
            Me.gvActive.MultiSelect = False
            Me.gvActive.Name = "gvActive"
            Me.gvActive.ReadOnly = True
            Me.gvActive.RowHeadersVisible = False
            DataGridViewCellStyle6.BackColor = System.Drawing.Color.White
            DataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black
            Me.gvActive.RowsDefaultCellStyle = DataGridViewCellStyle6
            Me.gvActive.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(180, Byte), Integer))
            Me.gvActive.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black
            Me.gvActive.RowTemplate.Height = 21
            Me.gvActive.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.gvActive.Size = New System.Drawing.Size(210, 262)
            Me.gvActive.TabIndex = 31
            '
            'btnDeActivate
            '
            Me.btnDeActivate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnDeActivate.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnDeActivate.Image = Global.QSILib.My.Resources.Resources.ltarrow
            Me.btnDeActivate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnDeActivate.Location = New System.Drawing.Point(232, 98)
            Me.btnDeActivate.Name = "btnDeActivate"
            Me.btnDeActivate.Size = New System.Drawing.Size(79, 26)
            Me.btnDeActivate.TabIndex = 32
            Me.btnDeActivate.Text = "Delete"
            Me.btnDeActivate.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.btnDeActivate.UseVisualStyleBackColor = True
            '
            'btnActivate
            '
            Me.btnActivate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnActivate.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnActivate.Image = Global.QSILib.My.Resources.Resources.rtarrow
            Me.btnActivate.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.btnActivate.Location = New System.Drawing.Point(232, 44)
            Me.btnActivate.Name = "btnActivate"
            Me.btnActivate.Size = New System.Drawing.Size(79, 26)
            Me.btnActivate.TabIndex = 33
            Me.btnActivate.Text = "Add"
            Me.btnActivate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnActivate.UseVisualStyleBackColor = True
            '
            'btnMoveDown
            '
            Me.btnMoveDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnMoveDown.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnMoveDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnMoveDown.Location = New System.Drawing.Point(386, 312)
            Me.btnMoveDown.Name = "btnMoveDown"
            Me.btnMoveDown.Size = New System.Drawing.Size(88, 26)
            Me.btnMoveDown.TabIndex = 34
            Me.btnMoveDown.Text = "Move Down"
            Me.btnMoveDown.UseVisualStyleBackColor = True
            '
            'btnMoveUp
            '
            Me.btnMoveUp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnMoveUp.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnMoveUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnMoveUp.Location = New System.Drawing.Point(386, 280)
            Me.btnMoveUp.Name = "btnMoveUp"
            Me.btnMoveUp.Size = New System.Drawing.Size(88, 26)
            Me.btnMoveUp.TabIndex = 35
            Me.btnMoveUp.Text = "Move Up"
            Me.btnMoveUp.UseVisualStyleBackColor = True
            '
            'availfield
            '
            Me.availfield.HeaderText = "Available Fields"
            Me.availfield.Name = "availfield"
            Me.availfield.ReadOnly = True
            Me.availfield.Width = 200
            '
            'selfields
            '
            Me.selfields.HeaderText = "Selected Fields"
            Me.selfields.Name = "selfields"
            Me.selfields.ReadOnly = True
            Me.selfields.Width = 200
            '
            'fChooser
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
            Me.ClientSize = New System.Drawing.Size(543, 377)
            Me.Controls.Add(Me.btnMoveUp)
            Me.Controls.Add(Me.btnMoveDown)
            Me.Controls.Add(Me.btnActivate)
            Me.Controls.Add(Me.btnDeActivate)
            Me.Controls.Add(Me.gvActive)
            Me.Controls.Add(Me.gvNonActive)
            Me.Controls.Add(Me.btnAdd)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnAccept)
            Me.Name = "fChooser"
            Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "Field Chooser"
            CType(Me.gvNonActive, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.gvActive, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents FilterTimer As System.Windows.Forms.Timer
        Protected WithEvents btnCancel As System.Windows.Forms.Button
        Public WithEvents btnAccept As System.Windows.Forms.Button
        Public WithEvents btnAdd As System.Windows.Forms.Button
        Friend WithEvents gvNonActive As QSILib.qGVList
        Friend WithEvents gvActive As QSILib.qGVList
        Public WithEvents btnDeActivate As System.Windows.Forms.Button
        Public WithEvents btnActivate As System.Windows.Forms.Button
        Public WithEvents btnMoveDown As System.Windows.Forms.Button
        Public WithEvents btnMoveUp As System.Windows.Forms.Button
        Friend WithEvents availfield As QSILib.qGVTextBoxColumn
        Friend WithEvents selfields As QSILib.qGVTextBoxColumn
    End Class
End Namespace

