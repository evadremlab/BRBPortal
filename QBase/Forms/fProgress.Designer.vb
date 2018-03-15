<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fProgress
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
        Me.btnCancel = New System.Windows.Forms.Button
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.txtCount = New QSILib.Windows.Forms.qTextBox
        Me.txtDescr = New QSILib.Windows.Forms.qTextBox
        Me.txtElapsed = New QSILib.Windows.Forms.qTextBox
        Me.QLabel1 = New QSILib.Windows.Forms.qLabel
        Me.SuspendLayout()
        '
        'btnCancel
        '
        Me.btnCancel.Image = Global.QSILib.My.Resources.Resources.DELETE
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(83, 57)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 2
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'Timer1
        '
        '
        'txtCount
        '
        Me.txtCount._Format = ""
        Me.txtCount._FormatNumber = ""
        Me.txtCount._IsKeyField = False
        Me.txtCount._ValidateMaxValue = ""
        Me.txtCount._ValidateMinValue = ""
        Me.txtCount.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.txtCount.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtCount.Location = New System.Drawing.Point(6, 62)
        Me.txtCount.Name = "txtCount"
        Me.txtCount.Size = New System.Drawing.Size(71, 13)
        Me.txtCount.TabIndex = 4
        Me.txtCount.TabStop = False
        '
        'txtDescr
        '
        Me.txtDescr._Format = ""
        Me.txtDescr._FormatNumber = ""
        Me.txtDescr._IsKeyField = False
        Me.txtDescr._ValidateMaxValue = ""
        Me.txtDescr._ValidateMinValue = ""
        Me.txtDescr.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.txtDescr.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtDescr.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescr.Location = New System.Drawing.Point(6, 10)
        Me.txtDescr.Multiline = True
        Me.txtDescr.Name = "txtDescr"
        Me.txtDescr.Size = New System.Drawing.Size(177, 20)
        Me.txtDescr.TabIndex = 3
        Me.txtDescr.TabStop = False
        Me.txtDescr.Text = "Name and Address Search"
        '
        'txtElapsed
        '
        Me.txtElapsed._Format = ""
        Me.txtElapsed._FormatNumber = ""
        Me.txtElapsed._IsKeyField = False
        Me.txtElapsed._ValidateMaxValue = ""
        Me.txtElapsed._ValidateMinValue = ""
        Me.txtElapsed.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.txtElapsed.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtElapsed.Location = New System.Drawing.Point(83, 34)
        Me.txtElapsed.Name = "txtElapsed"
        Me.txtElapsed.Size = New System.Drawing.Size(100, 13)
        Me.txtElapsed.TabIndex = 1
        Me.txtElapsed.TabStop = False
        '
        'QLabel1
        '
        Me.QLabel1.AutoSize = True
        Me.QLabel1.Location = New System.Drawing.Point(3, 34)
        Me.QLabel1.Name = "QLabel1"
        Me.QLabel1.Size = New System.Drawing.Size(74, 13)
        Me.QLabel1.TabIndex = 0
        Me.QLabel1.Text = "Elapsed Time:"
        Me.QLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'fProgress
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(200, 87)
        Me.ControlBox = False
        Me.Controls.Add(Me.txtCount)
        Me.Controls.Add(Me.txtDescr)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.txtElapsed)
        Me.Controls.Add(Me.QLabel1)
        Me.Name = "fProgress"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Working"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents QLabel1 As QSILib.Windows.Forms.qLabel
    Friend WithEvents txtElapsed As QSILib.Windows.Forms.qTextBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents txtDescr As QSILib.Windows.Forms.qTextBox
    Friend WithEvents txtCount As QSILib.Windows.Forms.qTextBox
End Class
