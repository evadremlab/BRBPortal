Namespace Windows.Forms

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frMain
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
            Me.ReportViewer1 = New Microsoft.Reporting.WinForms.ReportViewer
            Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
            Me.closetimer = New System.Windows.Forms.Timer(Me.components)
            Me.SuspendLayout()
            '
            'ReportViewer1
            '
            Me.ReportViewer1.AutoSize = True
            Me.ReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.ReportViewer1.DocumentMapWidth = 45
            Me.ReportViewer1.Location = New System.Drawing.Point(0, 0)
            Me.ReportViewer1.Margin = New System.Windows.Forms.Padding(2)
            Me.ReportViewer1.Name = "ReportViewer1"
            Me.ReportViewer1.Size = New System.Drawing.Size(694, 402)
            Me.ReportViewer1.TabIndex = 0
            '
            'Timer1
            '
            Me.Timer1.Interval = 3000
            '
            'frMain
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(694, 402)
            Me.Controls.Add(Me.ReportViewer1)
            Me.Margin = New System.Windows.Forms.Padding(2)
            Me.Name = "frMain"
            Me.Text = "frMain"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Private WithEvents ReportViewer1 As Microsoft.Reporting.WinForms.ReportViewer
        Friend WithEvents Timer1 As System.Windows.Forms.Timer
        Friend WithEvents closetimer As System.Windows.Forms.Timer
    End Class

End Namespace