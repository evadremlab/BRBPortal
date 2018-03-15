Namespace Windows.Forms

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
            Me.txtCode = New System.Windows.Forms.TextBox
            Me.btnDD = New System.Windows.Forms.Button
            Me.TxtDescr = New System.Windows.Forms.TextBox
            Me.SuspendLayout()
            '
            'txtCode
            '
            Me.txtCode.Location = New System.Drawing.Point(0, 0)
            Me.txtCode.Name = "txtCode"
            Me.txtCode.Size = New System.Drawing.Size(50, 20)
            Me.txtCode.TabIndex = 0
            '
            'btnDD
            '
            Me.btnDD.Image = Global.QSILib.My.Resources.Resources.ARW01DN
            Me.btnDD.Location = New System.Drawing.Point(206, 0)
            Me.btnDD.Name = "btnDD"
            Me.btnDD.Size = New System.Drawing.Size(18, 20)
            Me.btnDD.TabIndex = 2
            Me.btnDD.UseVisualStyleBackColor = True
            '
            'TxtDescr
            '
            Me.TxtDescr.Location = New System.Drawing.Point(56, 0)
            Me.TxtDescr.Name = "TxtDescr"
            Me.TxtDescr.Size = New System.Drawing.Size(150, 20)
            Me.TxtDescr.TabIndex = 3
            '
            'qDD
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.TxtDescr)
            Me.Controls.Add(Me.btnDD)
            Me.Controls.Add(Me.txtCode)
            Me.Name = "qDD"
            Me.Size = New System.Drawing.Size(230, 24)
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Public WithEvents txtCode As System.Windows.Forms.TextBox
        Public WithEvents TxtDescr As System.Windows.Forms.TextBox
        Public WithEvents btnDD As System.Windows.Forms.Button

    End Class

End Namespace