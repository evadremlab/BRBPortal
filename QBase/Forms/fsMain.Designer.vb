Namespace Windows.Forms

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class fsMain
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
            Me.btnCancel = New System.Windows.Forms.Button
            Me.lblStatus1 = New QSILib.Windows.Forms.qLabel
            Me.lblStatus2 = New QSILib.Windows.Forms.qLabel
            CType(Me.iDS, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.iEP, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'btnCancel
            '
            Me.btnCancel.Image = Global.QSILib.My.Resources.Resources.DELETE
            Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnCancel.Location = New System.Drawing.Point(85, 88)
            Me.btnCancel.Margin = New System.Windows.Forms.Padding(2)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(67, 24)
            Me.btnCancel.TabIndex = 0
            Me.btnCancel.Text = "Cancel"
            Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.btnCancel.UseVisualStyleBackColor = True
            '
            'lblStatus1
            '
            Me.lblStatus1.AutoSize = True
            Me.lblStatus1.Location = New System.Drawing.Point(10, 11)
            Me.lblStatus1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
            Me.lblStatus1.Name = "lblStatus1"
            Me.lblStatus1.Size = New System.Drawing.Size(0, 13)
            Me.lblStatus1.TabIndex = 1
            Me.lblStatus1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'lblStatus2
            '
            Me.lblStatus2.AutoSize = True
            Me.lblStatus2.Location = New System.Drawing.Point(37, 35)
            Me.lblStatus2.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
            Me.lblStatus2.Name = "lblStatus2"
            Me.lblStatus2.Size = New System.Drawing.Size(0, 13)
            Me.lblStatus2.TabIndex = 2
            Me.lblStatus2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'fsMain
            '
            Me.AcceptButton = Me.btnCancel
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
            Me.ClientSize = New System.Drawing.Size(238, 119)
            Me.ControlBox = False
            Me.Controls.Add(Me.lblStatus2)
            Me.Controls.Add(Me.lblStatus1)
            Me.Controls.Add(Me.btnCancel)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.Margin = New System.Windows.Forms.Padding(2)
            Me.Name = "fsMain"
            Me.Text = "Report Status"
            CType(Me.iDS, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.iEP, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Protected WithEvents lblStatus1 As qLabel
        Public WithEvents btnCancel As System.Windows.Forms.Button
        Public WithEvents lblStatus2 As QSILib.Windows.Forms.qLabel
    End Class

End Namespace