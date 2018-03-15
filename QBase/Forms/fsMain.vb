'Imports DataDynamics.ActiveReports

Namespace Windows.Forms

    Public Class fsMain
        Public iCancel As Boolean = False
        Public iCallingFP As fpMainVersion = Nothing
        Public iCallingFP2 As fpMain2 = Nothing     '...added 07/21/2014 as part of implementing fpMain2 (SDC)
        Public icallingFPNoVers As fpMain = Nothing

#Region "---------------------------- Documentation ------------------------------"
        'Status Form, used during updates and reports
        'Descendants will have update and report logic in them


#End Region

        Public Overridable Sub ReEnter(ByVal aPresentMode As String)
            MsgBoxErr("Programmer Error", "Need OnReEnter logic in fs form")
        End Sub

        Public Overridable Sub BtnCustom1()

        End Sub

        Public Overridable Sub BtnCustom2()

        End Sub

        Public Overridable Function ReportViewerClosing() As Boolean
            Return True
        End Function

        'Public Overridable Function AdobeReportVersion() As Document.Document
        '    Return Nothing
        'End Function

        'Public Overridable Function AdobeReportVersion2() As GrapeCity.ActiveReports.Document.SectionDocument
        '    Return Nothing
        'End Function

        Private Sub fsMain_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
            'BHS 4/17/09 force windowstate to normal if coming in from parameter window, or from report
            If iCallingFP Is Nothing AndAlso iCallingFP2 Is Nothing Then
                Me.WindowState = FormWindowState.Normal
            End If

            'BHS 10/11/11 make sure fsMain.MDIParent is valid
            If Me.MdiParent Is Nothing OrElse Me.MdiParent.IsMdiContainer = False Then
                Me.MdiParent = gMDIForm
            End If

        End Sub

#Region "---------------------------- Events ------------------------------"

        '''<summary> Cancel button clicked </summary>
        Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            Dim Answer As MsgBoxResult

            Answer = MsgBoxQuestion("Ok to cancel?", , "Report Interrupted")
            If Answer = MsgBoxResult.Yes Then
                If iCallingFP IsNot Nothing Then
                    iCallingFP.ControlBox = True
                    iCallingFP.btnRun.Enabled = True
                End If
                If iCallingFP2 IsNot Nothing Then
                    iCallingFP2.ControlBox = True
                    iCallingFP2.btnRun.Enabled = True
                End If
                iCancel = True
            End If

        End Sub





        'When fs closes, reinstate parameter screen Controlbox and <Run> button
        Private Sub fsMain_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
            If iCallingFP IsNot Nothing Then
                iCallingFP.ControlBox = True
                iCallingFP.btnRun.Enabled = True
            End If
            If iCallingFP2 IsNot Nothing Then
                iCallingFP2.ControlBox = True
                iCallingFP2.btnRun.Enabled = True
            End If
            If icallingFPNoVers IsNot Nothing Then
                icallingFPNoVers.ControlBox = True
                icallingFPNoVers.btnRun.Enabled = True
            End If
            Me.WindowState = FormWindowState.Normal 'BHS 4/17/09 So that future windows appear Normal
        End Sub

#End Region


    End Class
End Namespace