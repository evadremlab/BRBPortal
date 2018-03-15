<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OneMoment
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.lblMessage = New QSILib.Windows.Forms.qLabel
        Me.SuspendLayout()
        '
        'lblMessage
        '
        Me.lblMessage.Location = New System.Drawing.Point(12, 18)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(268, 13)
        Me.lblMessage.TabIndex = 0
        Me.lblMessage.Text = "lblMessage"
        Me.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'OneMoment
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(292, 56)
        Me.ControlBox = False
        Me.Controls.Add(Me.lblMessage)
        Me.Name = "OneMoment"
        Me.Text = "One Moment..."
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents lblMessage As QSILib.Windows.Forms.qLabel
End Class
