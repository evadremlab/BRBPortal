Imports DataDynamics.ActiveReports
Imports QSILib.Windows.Forms

Public Class farMain2
    Public irpt As DataDynamics.ActiveReports.ActiveReport
    Public iAlreadyRun As Boolean = False
    Public iCallingFP As fpMainVersion = Nothing
    Public iCallingFP2 As fpMain2 = Nothing
    Public iCallingFS As fsMain = Nothing   'BHS 2/9/09
    Private iKeepFSOpen As Boolean = False

    '''<summary> Pass Report reference, and whether you've already executed rpt.run() </summary>
    Sub New(ByVal arpt As DataDynamics.ActiveReports.ActiveReport, Optional ByVal aAlreadyRun As Boolean = False)
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

        ' GBV 7/23/2014 - if this was called from a cron job, export to pdf and close form
        If gIsFromCron Then
            Try
                If iAlreadyRun = False Then irpt.Run()
                Dim Path As String = ExportPath("PDFReport", ".pdf")
                Dim Exp As New DataDynamics.ActiveReports.Export.Pdf.PdfExport
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

            '...disable the <Cancel> button in the fs, if appropriate
            If iCallingFS IsNot Nothing Then
                iCallingFS.btnCancel.Enabled = False
            End If


            Viewer1.Document = irpt.Document
            Viewer1.AutoSize = True
            Viewer1.ReportViewer.ViewType = Viewer.ViewType.ContinuousScroll


            Dim Sep As New DataDynamics.ActiveReports.Toolbar.Separator
            Viewer1.Toolbar.Tools.Add(Sep)

            'Make Annotations button not visible
            Me.Viewer1.Toolbar.Tools.Item(23).Visible = False
            Me.Viewer1.Toolbar.Tools.Item(23).Enabled = False
            Me.Viewer1.Toolbar.Tools.Item(24).Visible = False   'Extra separator bar

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
            If iAlreadyRun = False Then irpt.Run()


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
    Private Sub Viewer1_HyperLink(ByVal sender As Object, ByVal e As DataDynamics.ActiveReports.Viewer.HyperLinkEventArgs) Handles Viewer1.HyperLink
        If e.HyperLink.ToLower.IndexOf(".pdf") > -1 Then
            System.Diagnostics.Process.Start("AcroRd32.exe", """" & e.HyperLink & """")
            e.Handled = True
        End If
    End Sub

    '''<summary> Toolbar clicked </summary>
    Private Sub Viewer1_ToolClick(ByVal sender As Object, ByVal e As DataDynamics.ActiveReports.Toolbar.ToolClickEventArgs) Handles Viewer1.ToolClick
        Try

            If e.Tool.Id = 333 Then
                'BHS 6/1/09 Allow a special PDF version of the document (for instance to not use Javascript references that would work in the report viewer)
                'Dim Doc As Document.Document = iCallingFS.AdobeReportVersion()
                Dim Doc As Document.Document = Nothing
                If iCallingFS IsNot Nothing Then Doc = iCallingFS.AdobeReportVersion()
                If Doc Is Nothing Then Doc = Viewer1.Document

                Dim Path As String = ExportPath("PDFReport", ".pdf")
                Dim Exp As New DataDynamics.ActiveReports.Export.Pdf.PdfExport
                Exp.Export(Doc, Path)


                'BHS 4/3/09  Allow user to save PDF before viewing.
                Dim MBR As MsgBoxResult = MsgBox("Save this PDF before viewing?", _
                                                 MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2, _
                                                 "View PDF")
                If MBR <> MsgBoxResult.Yes Then
                    System.Diagnostics.Process.Start("AcroRd32.exe", """" & Path & """")
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
                        Try
                            Exp.Export(Doc, SavePath)
                            System.Diagnostics.Process.Start("AcroRd32.exe", """" & SavePath & """")  'BHS 4/21/09 Open the newly saved path, if available
                            'Return
                        Catch ex As Exception
                            If ex.Message.ToLower.IndexOf("part of the path") > -1 Then
                                MsgBox("Invalid Path - try again", MsgBoxStyle.Exclamation, "Can't Save File")
                                Return
                            Else
                                'ShowError("Trouble writing to specified file", ex)
                                ' GBV 4/11/2014 - Make error not fatal (ticket 3065) 
                                MsgBox("Trouble writing to specified file: " & SavePath, MsgBoxStyle.Exclamation, "Can't Save File")
                                Return ' Not Fatal
                            End If
                        End Try

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

            End If

            If e.Tool.Id = 334 Then Me.Close()

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

    Private Sub btnToList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnToList.Click
        Try
            If iCallingFS Is Nothing Then
                MsgBoxErr("Programmer Error", "FS must set iCallingFS before opening farMain2")
            Else
                Me.SendToBack()
                MdiParent.Refresh()
                iCallingFS.ReEnter("List")
                iKeepFSOpen = True
                Me.Close()

                'iCallingFS.BringToFront()   'BHS 4/17/09
                'iCallingFS.WindowState = FormWindowState.Normal
                'Application.DoEvents()
                'iCallingFS.ReEnter("List")
                'iKeepFSOpen = True
                'Me.Close()
            End If
        Catch ex As Exception
            ShowError("Unable to send report to list", ex)
            Return  '...not fatal
        End Try
    End Sub

    Private Sub farMain2_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If iCallingFS IsNot Nothing Then
                '...check with the fs to see if we should continue the close!
                If iCallingFS.ReportViewerClosing() = False Then
                    e.Cancel = True
                    Return
                End If
                '...close the FS if appropriate
                'NOTE: Closing the FS will trigger the reinstatement of the fp Controlbox and <Run> button!
                If iKeepFSOpen = False Then iCallingFS.Close()
            End If
        Catch ex As Exception
            ShowError("Trouble closing report form", ex)
        End Try
    End Sub

    'Let the fs object process Custom1 button
    Private Sub btnCustom1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCustom1.Click
        iCallingFS.BtnCustom1()
    End Sub

    'Let the fs object process Custom2 button
    Private Sub btnCustom2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCustom2.Click
        iCallingFS.BtnCustom2()
    End Sub

    'BHS 8/26/10
    ''' <summary> Save zoom for this user, this fs Class, so it can be reapplied next time </summary>
    Private Sub Viewer1_ZoomChanged(ByVal sender As Object, ByVal e As DataDynamics.ActiveReports.Viewer.ZoomChangedEventArgs) Handles Viewer1.ZoomChanged
        If irpt IsNot Nothing Then
            SaveZoom(irpt.Name, e.Zoom)
        End If
    End Sub

    'BHS 8/26/10
    ''' <summary> Save resizing to irpt.Name window, rather than the fBase default of Me.Name </summary>
    Private Sub farMain_ResizeEnd(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.ResizeEnd
        SaveWindowSize(irpt.Name, Me.Width, Me.Height)
    End Sub
End Class