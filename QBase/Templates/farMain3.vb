'Imports DataDynamics.ActiveReports
Imports GrapeCity.ActiveReports
Imports QSILib.Windows.Forms

Public Class farMain3
    Public irpt As GrapeCity.ActiveReports.SectionReport
    Public iAlreadyRun As Boolean = False
    Public iCallingFP As fpMainVersion = Nothing
    Public iCallingFS As fsMain = Nothing   'BHS 2/9/09
    Private iKeepFSOpen As Boolean = False

    '''<summary> Pass Report reference, and whether you've already executed rpt.run() </summary>
    Sub New(ByVal arpt As GrapeCity.ActiveReports.SectionReport, Optional ByVal aAlreadyRun As Boolean = False)
        InitializeComponent()
        irpt = arpt
        iAlreadyRun = aAlreadyRun
    End Sub

    'Private Sub farMain_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
    '    Me.WindowState = FormWindowState.Maximized
    'End Sub

    'Private Sub farMain_Deactivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Deactivate
    '    Me.WindowState = FormWindowState.Normal
    'End Sub

    '''<summary> Show report in viewer, adding a PDF button to the regular toolbar </summary>
    Private Sub fARViewer_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' GBV 11/21/2014 - if this was called from a cron job, export to pdf and close form
        If gIsFromCron Then
            Try
                If iAlreadyRun = False Then irpt.Run()
                Dim Path As String = ExportPath("PDFReport", ".pdf")
                Dim Exp As New GrapeCity.ActiveReports.Export.Pdf.Section.PdfExport
                Exp.Export(irpt.Document, Path.Replace("H:\", gFileServer & "\home\" & gUserName & "\"))
                gCronRPTPath = Path.Replace("H:\", gFileServer & "\home\" & gUserName & "\")
                If Exp IsNot Nothing Then Exp.Dispose()
                If irpt IsNot Nothing Then irpt.Dispose()
                Close()
                Me.Dispose()
                Return
            Catch ex As Exception
                Throw New Exception("Error exporting report (farMain2)", ex)
            End Try

        End If

        Try
            iIsLoading = True

            Viewer1.Document = irpt.Document
            Viewer1.AutoSize = True
            Viewer1.ReportViewer.ViewType = GrapeCity.Viewer.Common.Model.ViewType.Continuous

            Dim Sep As New ToolStripSeparator
            Viewer1.Toolbar.ToolStrip.Items.Add(Sep)

            'make a Save image object
            Dim img As System.Drawing.Image = My.Resources.PDF

            ''add it to the Viewer's image collection
            'Viewer1.Toolbar.Images.Images.Add(img)
            ''get the index of the image (store the index in case we need it later)
            'Dim myImageIndex As Integer = Viewer1.Toolbar.Images.Images.Count - 1

            'make PDF button object
            Dim btn As New ToolStripButton("PDF", img)
            btn.ToolTipText = "Save report to PDF"
            btn.Name = "btnPDF"
            'btn.ImageIndex = myImageIndex
            'btn.Id = 333
            btn.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText
            Viewer1.Toolbar.ToolStrip.Items.Add(btn)
            AddHandler btn.Click, AddressOf btn_Click

            Dim Sep2 As New ToolStripSeparator
            Viewer1.Toolbar.ToolStrip.Items.Add(Sep2)

            'make a Close image object
            Dim img2 As System.Drawing.Image = My.Resources.DELETE
            'Viewer1.Toolbar.Images.Images.Add(img2)
            'myImageIndex = Viewer1.Toolbar.Images.Images.Count - 1

            'make PDF button object
            Dim btn2 As New ToolStripButton("Close", img2)
            btn2.ToolTipText = "Close this report viewer"
            btn2.Name = "btnClose"
            'btn2.ImageIndex = myImageIndex
            'btn2.Id = 334
            btn2.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText
            Viewer1.Toolbar.ToolStrip.Items.Add(btn2)
            AddHandler btn2.Click, AddressOf btn2_Click

            ''Close Viewer Button
            'Dim btn2 As New DataDynamics.ActiveReports.Toolbar.Button()
            'btn2.Caption = "Close"
            'btn2.ToolTip = "Close this report viewer"
            ''btn.Image = Global.QSILib.My.Resources.Resources.DELETE
            'btn2.ButtonStyle = DataDynamics.ActiveReports.Toolbar.ButtonStyle.Text
            'btn2.Id = 334
            'Me.Viewer1.Toolbar.Tools.Insert(25, btn2)

            'Run the report
            If iAlreadyRun = False Then irpt.Run()

            'BHS 8/26/10
            If irpt IsNot Nothing Then
                SetWindowSize(irpt.Name, Me.Size)
                SetZoom(irpt.Name, Viewer1.ReportViewer.Zoom)
            End If

            iIsLoading = False

        Catch ex As Exception
            ShowError("Trouble loading report into viewer", ex)
        End Try
    End Sub

    'BHS 4/5/10
    Private Sub Viewer1_HyperLink(ByVal sender As Object, ByVal e As GrapeCity.ActiveReports.Viewer.Win.HyperLinkEventArgs) Handles Viewer1.HyperLink
        If e.HyperLink.ToLower.IndexOf(".pdf") > -1 Then
            System.Diagnostics.Process.Start("AcroRd32.exe", """" & e.HyperLink & """")
            e.Handled = True
        End If
    End Sub

    Private Sub btn2_Click(sender As Object, e As EventArgs)
        Try
            Me.Close()
        Catch ex As Exception

        End Try

    End Sub

    Private Sub btn_Click(sender As Object, e As EventArgs)
        Try
            'BHS 6/1/09 Allow a special PDF version of the document (for instance to not use Javascript references that would work in the report viewer)
            'Dim Doc As Document.Document = iCallingFS.AdobeReportVersion()
            Dim Doc As GrapeCity.ActiveReports.Document.SectionDocument = Nothing
            If iCallingFS IsNot Nothing Then Doc = iCallingFS.AdobeReportVersion2()
            If Doc Is Nothing Then Doc = Viewer1.Document

            Dim Path As String = ExportPath("PDFReport", ".pdf")
            Dim Exp As New GrapeCity.ActiveReports.Export.Pdf.Section.PdfExport
            Exp.Export(Doc, Path)

            'BHS 4/3/09  Allow user to save PDF before viewing.
            Dim MBR As MsgBoxResult = MsgBox("Save this PDF before viewing?", _
                                             MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2, _
                                             "View PDF")
            If MBR <> MsgBoxResult.Yes Then
                ' GBV 3/30/2010 added AcroRd32
                System.Diagnostics.Process.Start("AcroRd32.exe", Path)  'Doesn't need """" around path because we built the path to not have spaces
            Else
                Dim SavePath As String = ""
                Dim viewlocal As Boolean = False
                If SQLGetString("Select HCamp From UserMain Where UserID = '" & gUserName & "'") <> "OTT" _
                   Then viewlocal = True

                'Fill in previous values if any
                Dim tDrive As String = ""
                Dim tPath As String = ""
                If gLastSaveDrive <> "" Then tDrive = gLastSaveDrive.Trim
                If gLastSaveLocation <> "" Then tPath = gLastSaveLocation.Trim
                viewlocal = gLastSaveLocal

                Dim SavePathForm As New fSaveFile(tDrive, tPath, viewlocal, SavePath, "pdf")

                SavePathForm.ShowDialog()
                SavePath = SavePathForm.iSavePath
                If SavePath IsNot Nothing AndAlso SavePath.Length > 0 Then
                    Exp.Export(Doc, SavePath)
                    System.Diagnostics.Process.Start("AcroRd32.exe", """" & SavePath & """")  'BHS 4/21/09 Open newly saved path, if available
                    'Return

                    'Set global gLastSaveLocation if the user saved the file - DJW 01/21/2015
                    If SavePath <> "" Then
                        gLastSaveLocation = SavePathForm.txtDocPath.Text.Trim
                        gLastSaveDrive = SavePathForm.cbDrive.Text.Trim
                        If SavePathForm.chViewLocalNetwork.Checked = True Then
                            gLastSaveLocal = True
                        Else
                            gLastSaveLocal = False
                        End If
                    End If
                End If

            End If

        Catch ex As Exception
            If ex.Message.IndexOf("Access to the path") > -1 And _
               ex.Message.IndexOf("is denied") > -1 Then
                MsgBox("You don't have rights to write to that file")
            Else
                ShowError("Trouble sending report to PDF", ex)
            End If

            Return      'Not Fatal
        End Try

    End Sub

    Private Sub farMain3_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If iCallingFS IsNot Nothing Then
                '...close the FS if appropriate
                'NOTE: Closing the FS will trigger the reinstatement of the fp Controlbox and <Run> button!
                If iKeepFSOpen = False Then iCallingFS.Close()
            End If
        Catch ex As Exception
            ShowError("Trouble closing report form", ex)
        End Try
    End Sub

    'BHS 8/26/10
    ''' <summary> Save zoom for this user, this fs Class, so it can be reapplied next time </summary>
    Private Sub Viewer1_ZoomChanged(ByVal sender As Object, ByVal e As GrapeCity.ActiveReports.Viewer.Win.ZoomChangedEventArgs) Handles Viewer1.ZoomChanged
        If irpt IsNot Nothing Then
            SaveZoom(irpt.Name, CSng(e.Zoom))
        End If
    End Sub

    'BHS 8/26/10
    ''' <summary> Save resizing to irpt.Name window, rather than the fBase default of Me.Name </summary>
    Private Sub farMain3_ResizeEnd(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.ResizeEnd
        SaveWindowSize(irpt.Name, Me.Width, Me.Height)
    End Sub


End Class
