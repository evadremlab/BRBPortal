Namespace Windows.Forms

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class fBase
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
            Me.components = New System.ComponentModel.Container()
            Me.CloseTimer = New System.Windows.Forms.Timer(Me.components)
            Me.PostTimer = New QSILib.qTimer()
            Me.SuspendLayout()
            '
            'CloseTimer
            '
            '
            'PostTimer
            '
            Me.PostTimer._EventName = Nothing
            Me.PostTimer._Param = Nothing
            Me.PostTimer.Interval = 10
            '
            'fBase
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(292, 266)
            Me.Name = "fBase"
            Me.Text = "fBase"
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents PostTimer As qTimer
        Public WithEvents CloseTimer As System.Windows.Forms.Timer

    End Class

End Namespace