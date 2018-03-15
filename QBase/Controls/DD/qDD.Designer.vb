

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class qDD
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.txtCode = New System.Windows.Forms.TextBox()
        Me.btnDD = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'txtCode
        '
        Me.txtCode.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtCode.Location = New System.Drawing.Point(0, 1)
        Me.txtCode.Name = "txtCode"
        Me.txtCode.Size = New System.Drawing.Size(151, 20)
        Me.txtCode.TabIndex = 0
        '
        'btnDD
        '
        Me.btnDD.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDD.Image = Global.QSILib.My.Resources.Resources.dwnarrow
        Me.btnDD.Location = New System.Drawing.Point(149, 1)
        Me.btnDD.Name = "btnDD"
        Me.btnDD.Size = New System.Drawing.Size(18, 20)
        Me.btnDD.TabIndex = 2
        Me.btnDD.TabStop = False
        Me.btnDD.UseVisualStyleBackColor = True
        '
        'qDD
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnDD)
        Me.Controls.Add(Me.txtCode)
        Me.Name = "qDD"
        Me.Size = New System.Drawing.Size(170, 21)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnDD As System.Windows.Forms.Button
    Friend WithEvents txtCode As System.Windows.Forms.TextBox

End Class

