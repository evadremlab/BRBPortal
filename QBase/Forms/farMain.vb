Imports DataDynamics.ActiveReports

Public Class farMain
    Public irpt As DataDynamics.ActiveReports.ActiveReport3

    Sub New(ByVal arpt As DataDynamics.ActiveReports.ActiveReport3)
        InitializeComponent()
        irpt = arpt
    End Sub

    'Private Sub farMain_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
    '    Me.WindowState = FormWindowState.Maximized
    'End Sub

    'Private Sub farMain_Deactivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Deactivate
    '    Me.WindowState = FormWindowState.Normal
    'End Sub

    Private Sub fARViewer_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Viewer1.Document = irpt.Document
        Viewer1.AutoSize = True
        'Viewer1.Toolbar.Tools.Item(6). = True
        Viewer1.ReportViewer.ViewType = Viewer.ViewType.ContinuousScroll

        'Save To PDF button
        'Dim btn As New DataDynamics.ActiveReports.Toolbar.Button()
        'btn.Caption = "PDF"
        'btn.ToolTip = "Save report to PDF"
        ''btn.Image = Global.QSILib.My.Resources.Resources.SAVE
        ''btn.ImageIndex = 2
        'btn.ButtonStyle = DataDynamics.ActiveReports.Toolbar.ButtonStyle.Text
        'btn.Id = 333
        'Me.Viewer1.Toolbar.Tools.Insert(24, btn)
        Dim Sep As New DataDynamics.ActiveReports.Toolbar.Separator
        Viewer1.Toolbar.Tools.Add(Sep)

        'make a Save image object
        Dim img As System.Drawing.Image = My.Resources.PDF
        'add it to the Viewer's image collection
        Viewer1.Toolbar.Images.Images.Add(img)
        'get the index of the image (store the index in case we need it later)
        Dim myImageIndex As Integer = Viewer1.Toolbar.Images.Images.Count - 1

        'make PDF button object
        Dim btn As New DataDynamics.ActiveReports.Toolbar.Button()
        btn.Caption = "PDF"
        btn.ToolTip = "Save report to PDF"
        btn.ImageIndex = myImageIndex
        btn.Id = 333
        btn.ButtonStyle = DataDynamics.ActiveReports.Toolbar.ButtonStyle.TextAndIcon
        Viewer1.Toolbar.Tools.Add(btn)

        Dim Sep2 As New DataDynamics.ActiveReports.Toolbar.Separator
        Viewer1.Toolbar.Tools.Add(Sep2)

        'make a Close image object
        Dim img2 As System.Drawing.Image = My.Resources.DELETE
        Viewer1.Toolbar.Images.Images.Add(img2)
        myImageIndex = Viewer1.Toolbar.Images.Images.Count - 1

        'make PDF button object
        Dim btn2 As New DataDynamics.ActiveReports.Toolbar.Button()
        btn2.Caption = "Close"
        btn2.ToolTip = "Close this report viewer"
        btn2.ImageIndex = myImageIndex
        btn2.Id = 334
        btn2.ButtonStyle = DataDynamics.ActiveReports.Toolbar.ButtonStyle.TextAndIcon
        Viewer1.Toolbar.Tools.Add(btn2)

        ''Close Viewer Button
        'Dim btn2 As New DataDynamics.ActiveReports.Toolbar.Button()
        'btn2.Caption = "Close"
        'btn2.ToolTip = "Close this report viewer"
        ''btn.Image = Global.QSILib.My.Resources.Resources.DELETE
        'btn2.ButtonStyle = DataDynamics.ActiveReports.Toolbar.ButtonStyle.Text
        'btn2.Id = 334
        'Me.Viewer1.Toolbar.Tools.Insert(25, btn2)

        'Run the report
        irpt.Run()
    End Sub

    Private Sub Viewer1_ToolClick(ByVal sender As Object, ByVal e As DataDynamics.ActiveReports.Toolbar.ToolClickEventArgs) Handles Viewer1.ToolClick
        If e.Tool.Id = 333 Then

            Dim Path As String = ExportPath("PDFReport", ".pdf")
            Dim Exp As New DataDynamics.ActiveReports.Export.Pdf.PdfExport
            Exp.Export(Viewer1.Document, Path)

            System.Diagnostics.Process.Start(Path)

        End If

        If e.Tool.Id = 334 Then Me.Close()

    End Sub
End Class