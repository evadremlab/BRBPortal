Imports QSILib

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class farMain4
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
        Me.Viewer1 = New GrapeCity.ActiveReports.Viewer.Win.Viewer
        Me.btnToList = New System.Windows.Forms.Button
        Me.btnCustom1 = New System.Windows.Forms.Button
        Me.btnCustom2 = New System.Windows.Forms.Button
        CType(Me.iDS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.iEP, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Viewer1
        '
        Me.Viewer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Viewer1.AutoSize = True
        Me.Viewer1.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.Viewer1.Document = New GrapeCity.ActiveReports.Document.SectionDocument("ARNet Document")
        Me.Viewer1.Location = New System.Drawing.Point(0, 0)
        Me.Viewer1.Margin = New System.Windows.Forms.Padding(2)
        Me.Viewer1.Name = "Viewer1"
        Me.Viewer1.ReportViewer.CurrentPage = 0
        Me.Viewer1.ReportViewer.MultiplePageCols = 3
        Me.Viewer1.ReportViewer.MultiplePageRows = 2
        Me.Viewer1.ReportViewer.ViewType = GrapeCity.Viewer.Common.Model.ViewType.SinglePage
        Me.Viewer1.Size = New System.Drawing.Size(912, 548)
        Me.Viewer1.TabIndex = 0
        Me.Viewer1.TableOfContents.Text = "Table Of Contents"
        Me.Viewer1.TableOfContents.Width = 150
        'Me.Viewer1.TabTitleLength = 35
        Me.Viewer1.Toolbar.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        '
        'btnToList
        '
        Me.btnToList.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnToList.Image = Global.QSILib.My.Resources.Resources.ViewNormal16x16
        Me.btnToList.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnToList.Location = New System.Drawing.Point(19, 551)
        Me.btnToList.Name = "btnToList"
        Me.btnToList.Size = New System.Drawing.Size(75, 23)
        Me.btnToList.TabIndex = 1
        Me.btnToList.Text = "To List"
        Me.btnToList.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnToList.UseVisualStyleBackColor = True
        '
        'btnCustom1
        '
        Me.btnCustom1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCustom1.Location = New System.Drawing.Point(138, 551)
        Me.btnCustom1.Name = "btnCustom1"
        Me.btnCustom1.Size = New System.Drawing.Size(75, 23)
        Me.btnCustom1.TabIndex = 2
        Me.btnCustom1.Text = "Button1"
        Me.btnCustom1.UseVisualStyleBackColor = True
        Me.btnCustom1.Visible = False
        '
        'btnCustom2
        '
        Me.btnCustom2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCustom2.Location = New System.Drawing.Point(258, 551)
        Me.btnCustom2.Name = "btnCustom2"
        Me.btnCustom2.Size = New System.Drawing.Size(75, 23)
        Me.btnCustom2.TabIndex = 3
        Me.btnCustom2.Text = "Button2"
        Me.btnCustom2.UseVisualStyleBackColor = True
        Me.btnCustom2.Visible = False
        '
        'farMain2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(912, 573)
        Me.Controls.Add(Me.btnCustom2)
        Me.Controls.Add(Me.btnCustom1)
        Me.Controls.Add(Me.btnToList)
        Me.Controls.Add(Me.Viewer1)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "farMain2"
        Me.Text = "Report Viewer"
        CType(Me.iDS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.iEP, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Viewer1 As GrapeCity.ActiveReports.Viewer.Win.Viewer
    Public WithEvents btnToList As System.Windows.Forms.Button
    Public WithEvents btnCustom1 As System.Windows.Forms.Button
    Public WithEvents btnCustom2 As System.Windows.Forms.Button
End Class
