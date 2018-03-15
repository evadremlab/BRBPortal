<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Prompt
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
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.txtPrompt = New QSILib.Windows.Forms.qTextBox
        Me.lblPrompt = New QSILib.Windows.Forms.qLabel
        Me.SuspendLayout()
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(56, 36)
        Me.btnOK.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(56, 19)
        Me.btnOK.TabIndex = 1
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(130, 36)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(56, 19)
        Me.btnCancel.TabIndex = 2
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'txtPrompt
        '
        Me.txtPrompt._Format = ""
        Me.txtPrompt._FormatNumber = ""
        Me.txtPrompt._IsKeyField = False
        Me.txtPrompt._ValidateMaxValue = ""
        Me.txtPrompt._ValidateMinValue = ""
        Me.txtPrompt.Location = New System.Drawing.Point(56, 5)
        Me.txtPrompt.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.txtPrompt.Name = "txtPrompt"
        Me.txtPrompt.Size = New System.Drawing.Size(208, 19)
        Me.txtPrompt.TabIndex = 0
        '
        'lblPrompt
        '
        Me.lblPrompt.AutoSize = True
        Me.lblPrompt.Location = New System.Drawing.Point(4, 7)
        Me.lblPrompt.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblPrompt.Name = "lblPrompt"
        Me.lblPrompt.Size = New System.Drawing.Size(47, 13)
        Me.lblPrompt.TabIndex = 2
        Me.lblPrompt.Text = "QLabel1"
        Me.lblPrompt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Prompt
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(270, 64)
        Me.ControlBox = False
        Me.Controls.Add(Me.txtPrompt)
        Me.Controls.Add(Me.lblPrompt)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.Name = "Prompt"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.Text = "Prompt"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents btnOK As System.Windows.Forms.Button
    Public WithEvents btnCancel As System.Windows.Forms.Button
    Public WithEvents lblPrompt As QSILib.Windows.Forms.qLabel
    Public WithEvents txtPrompt As QSILib.Windows.Forms.qTextBox
End Class
