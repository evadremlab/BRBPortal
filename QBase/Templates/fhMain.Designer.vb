Namespace Windows.Forms

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class fhMain
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
            Me.WebBrowser1 = New System.Windows.Forms.WebBrowser
            Me.SuspendLayout()
            '
            'WebBrowser1
            '
            Me.WebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.WebBrowser1.Location = New System.Drawing.Point(0, 0)
            Me.WebBrowser1.MinimumSize = New System.Drawing.Size(20, 20)
            Me.WebBrowser1.Name = "WebBrowser1"
            Me.WebBrowser1.Size = New System.Drawing.Size(703, 402)
            Me.WebBrowser1.TabIndex = 0
            Me.WebBrowser1.Url = New System.Uri("D:\WorkWin\DBAdmin\DBAdmin\Resources\DBAdmin.htm", System.UriKind.Absolute)
            '
            'fhMain
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.AutoSize = True
            Me.ClientSize = New System.Drawing.Size(703, 402)
            Me.Controls.Add(Me.WebBrowser1)
            Me.Name = "fhMain"
            Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
            Me.Text = "fhMain"
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents WebBrowser1 As System.Windows.Forms.WebBrowser
    End Class
End Namespace