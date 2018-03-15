Imports QSILib

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class farMain3
    Inherits QSILib.Windows.Forms.fBase

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
        Me.Viewer1 = New GrapeCity.ActiveReports.Viewer.Win.Viewer()
        CType(Me.iDS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.iEP, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Viewer1
        '
        Me.Viewer1.BackColor = System.Drawing.SystemColors.Control
        Me.Viewer1.CurrentPage = 0
        Me.Viewer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Viewer1.Location = New System.Drawing.Point(0, 0)
        Me.Viewer1.Margin = New System.Windows.Forms.Padding(2)
        Me.Viewer1.Name = "Viewer1"
        Me.Viewer1.PreviewPages = 0
        '
        '
        '
        '
        '
        '
        Me.Viewer1.Sidebar.ParametersPanel.ContextMenu = Nothing
        Me.Viewer1.Sidebar.ParametersPanel.Width = 200
        '
        '
        '
        Me.Viewer1.Sidebar.SearchPanel.ContextMenu = Nothing
        Me.Viewer1.Sidebar.SearchPanel.Width = 200
        '
        '
        '
        Me.Viewer1.Sidebar.ThumbnailsPanel.ContextMenu = Nothing
        Me.Viewer1.Sidebar.ThumbnailsPanel.Width = 200
        '
        '
        '
        Me.Viewer1.Sidebar.TocPanel.ContextMenu = Nothing
        Me.Viewer1.Sidebar.TocPanel.Text = "Table Of Contents"
        Me.Viewer1.Sidebar.TocPanel.Width = 200
        Me.Viewer1.Sidebar.Width = 200
        Me.Viewer1.Size = New System.Drawing.Size(912, 573)
        Me.Viewer1.TabIndex = 0
        '
        'farMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(912, 573)
        Me.Controls.Add(Me.Viewer1)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "farMain"
        Me.Text = "Report Viewer"
        CType(Me.iDS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.iEP, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Viewer1 As GrapeCity.ActiveReports.Viewer.Win.Viewer
End Class
