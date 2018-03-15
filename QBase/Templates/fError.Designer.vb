<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fError
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(fError))
        Me.btnOK = New System.Windows.Forms.Button
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.btnDetails = New System.Windows.Forms.Button
        Me.txtMsg = New QSILib.Windows.Forms.qTextBox
        Me.txtGeneral = New QSILib.Windows.Forms.qTextBox
        Me.txtUserTime = New QSILib.Windows.Forms.qTextBox
        Me.SuspendLayout()
        '
        'btnOK
        '
        Me.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnOK.Location = New System.Drawing.Point(181, 168)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 2
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnDetails
        '
        Me.btnDetails.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnDetails.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnDetails.Location = New System.Drawing.Point(272, 168)
        Me.btnDetails.Name = "btnDetails"
        Me.btnDetails.Size = New System.Drawing.Size(89, 23)
        Me.btnDetails.TabIndex = 6
        Me.btnDetails.Text = "Show Details"
        Me.btnDetails.UseVisualStyleBackColor = True
        '
        'txtMsg
        '
        Me.txtMsg._Format = ""
        Me.txtMsg._FormatNumber = ""
        Me.txtMsg._IsKeyField = False
        Me.txtMsg._ValidateMaxValue = ""
        Me.txtMsg._ValidateMinValue = ""
        Me.txtMsg.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMsg.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.txtMsg.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtMsg.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMsg.Location = New System.Drawing.Point(6, 10)
        Me.txtMsg.MaximumSize = New System.Drawing.Size(526, 50)
        Me.txtMsg.MinimumSize = New System.Drawing.Size(526, 20)
        Me.txtMsg.Multiline = True
        Me.txtMsg.Name = "txtMsg"
        Me.txtMsg.Size = New System.Drawing.Size(526, 33)
        Me.txtMsg.TabIndex = 3
        Me.txtMsg.TabStop = False
        Me.txtMsg.Text = "Error Msg"
        '
        'txtGeneral
        '
        Me.txtGeneral._Format = ""
        Me.txtGeneral._FormatNumber = ""
        Me.txtGeneral._IsKeyField = False
        Me.txtGeneral._ValidateMaxValue = ""
        Me.txtGeneral._ValidateMinValue = ""
        Me.txtGeneral.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtGeneral.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.txtGeneral.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtGeneral.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGeneral.Location = New System.Drawing.Point(6, 63)
        Me.txtGeneral.Multiline = True
        Me.txtGeneral.Name = "txtGeneral"
        Me.txtGeneral.Size = New System.Drawing.Size(526, 56)
        Me.txtGeneral.TabIndex = 5
        Me.txtGeneral.TabStop = False
        Me.txtGeneral.Text = "General Message"
        '
        'txtUserTime
        '
        Me.txtUserTime._Format = ""
        Me.txtUserTime._FormatNumber = ""
        Me.txtUserTime._IsKeyField = False
        Me.txtUserTime._ValidateMaxValue = ""
        Me.txtUserTime._ValidateMinValue = ""
        Me.txtUserTime.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.txtUserTime.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtUserTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUserTime.Location = New System.Drawing.Point(6, 134)
        Me.txtUserTime.Multiline = True
        Me.txtUserTime.Name = "txtUserTime"
        Me.txtUserTime.Size = New System.Drawing.Size(468, 20)
        Me.txtUserTime.TabIndex = 7
        Me.txtUserTime.TabStop = False
        Me.txtUserTime.Text = "User and Time"
        '
        'fError
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(547, 193)
        Me.Controls.Add(Me.txtMsg)
        Me.Controls.Add(Me.btnDetails)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.txtGeneral)
        Me.Controls.Add(Me.txtUserTime)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "fError"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Unexpected Error"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents txtMsg As QSILib.Windows.Forms.qTextBox
    Friend WithEvents txtGeneral As QSILib.Windows.Forms.qTextBox
    Friend WithEvents btnDetails As System.Windows.Forms.Button
    Friend WithEvents txtUserTime As QSILib.Windows.Forms.qTextBox
End Class
