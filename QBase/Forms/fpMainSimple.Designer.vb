Namespace Windows.Forms

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class fpMain
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
            Me.btnRun = New System.Windows.Forms.Button
            Me.lblFTitle = New System.Windows.Forms.Label
            Me.lblOneMoment = New System.Windows.Forms.Label
            Me.StatusStrip1 = New System.Windows.Forms.StatusStrip
            Me.HelpMsg = New System.Windows.Forms.ToolStripStatusLabel
            Me.StatusMsg = New System.Windows.Forms.Label
            Me.ProgressMsg = New System.Windows.Forms.Label
            CType(Me.iDS, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.iEP, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.StatusStrip1.SuspendLayout()
            Me.SuspendLayout()
            '
            'btnRun
            '
            Me.btnRun.Image = Global.QSILib.My.Resources.Resources.PRINT
            Me.btnRun.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnRun.Location = New System.Drawing.Point(2, 28)
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
            Me.lblFTitle.Location = New System.Drawing.Point(94, 1)
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
            Me.StatusStrip1.Location = New System.Drawing.Point(0, 224)
            Me.StatusStrip1.Name = "StatusStrip1"
            Me.StatusStrip1.Padding = New System.Windows.Forms.Padding(1, 0, 10, 0)
            Me.StatusStrip1.Size = New System.Drawing.Size(272, 22)
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
            Me.StatusMsg.Location = New System.Drawing.Point(4, 4)
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
            'fpMain
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
            Me.ClientSize = New System.Drawing.Size(272, 246)
            Me.Controls.Add(Me.ProgressMsg)
            Me.Controls.Add(Me.StatusMsg)
            Me.Controls.Add(Me.StatusStrip1)
            Me.Controls.Add(Me.lblOneMoment)
            Me.Controls.Add(Me.lblFTitle)
            Me.Controls.Add(Me.btnRun)
            Me.Margin = New System.Windows.Forms.Padding(2)
            Me.Name = "fpMain"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "fpMain"
            CType(Me.iDS, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.iEP, System.ComponentModel.ISupportInitialize).EndInit()
            Me.StatusStrip1.ResumeLayout(False)
            Me.StatusStrip1.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Protected WithEvents btnRun As System.Windows.Forms.Button
        Protected WithEvents lblFTitle As System.Windows.Forms.Label
        Protected WithEvents lblOneMoment As System.Windows.Forms.Label
        Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
        Friend WithEvents HelpMsg As System.Windows.Forms.ToolStripStatusLabel
        Protected WithEvents StatusMsg As System.Windows.Forms.Label
        Protected WithEvents ProgressMsg As System.Windows.Forms.Label
    End Class

End Namespace