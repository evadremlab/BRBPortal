Imports Microsoft.Reporting.WinForms

Namespace Windows.Forms

    'QUESTION - why can't I resize the report viewer?  (line 63)

    Public Class frMain

        Public iWinTitle As String = ""
        Public iRptWidth As Integer = 0
        Public iRptHeight As Integer = 0
        Public iRptPath As String = ""
        Public iReportServerUrl As System.Uri
        Public iParams As New ArrayList
        Public iShowParamArea As Boolean = False

        Public Sub New()
            InitializeComponent()
        End Sub

        '''<summary> Pass Window Title, size of window, and path to report </summary>
        Public Sub New(ByVal WinTitle As String, ByVal RptWidth As Integer, ByVal RptHeight As Integer, ByVal RptPath As String)
            InitializeComponent()
            iWinTitle = WinTitle
            iRptWidth = RptWidth
            iRptHeight = RptHeight
            iRptPath = RptPath

        End Sub

        '''<summary> Load Report </summary>
        Public Sub LoadReport(ByVal WinTitle As String, ByVal RptWidth As Integer, ByVal RptHeight As Integer, ByVal RptPath As String, Optional ByVal aZoom As Integer = 100)

            iWinTitle = WinTitle
            iRptWidth = RptWidth
            iRptHeight = RptHeight
            iRptPath = RptPath
            Me.Width = RptWidth + 7
            Me.Height = RptHeight + 27
            'ReportViewer1.ZoomPercent = aZoom
        End Sub

        '''<summary> Build up iParams array of report parameter values - these will set up report parameters
        '''   in the load routine. </summary>
        Public Sub AddParam(ByVal aName As String, ByVal aValue As String)
            Dim P As New strParam

            P.Name = aName
            P.Value = aValue
            iParams.Add(P)

        End Sub

        '''<summary> Load Report Viewer </summary>
        Private Sub frMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                Dim P As strParam
                Dim i As Integer = 0

                'Set generic report server
                Me.ReportViewer1.ServerReport.ReportServerUrl = New System.Uri(gReportServer, System.UriKind.Absolute)

                'Allow calling program to set different report server (not likely)
                If iReportServerUrl IsNot Nothing Then
                    Me.ReportViewer1.ServerReport.ReportServerUrl = iReportServerUrl
                End If

                'Load generic parameters and run report
                If iWinTitle < " " Or iRptWidth < 1 Or iRptHeight < 1 Or iRptPath < " " Then
                    'If fBase.ActiveForm IsNot Nothing Then ProgrammerErr("frMain parameters missing")
                    If Not InDevEnv() Then ProgrammerErr("frMain parameters missing")
                Else
                    Me.Text = iWinTitle
                    With Me.ReportViewer1
                        .ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Remote
                        .ServerReport.ReportPath = iRptPath

                        If iParams.Count > 0 Then
                            Dim RptParams(iParams.Count - 1) As ReportParameter
                            For Each P In iParams
                                RptParams(i) = New ReportParameter(P.Name, P.Value)
                                i = i + 1
                            Next
                            .ServerReport.SetParameters(RptParams)
                        End If
                        '.SetDisplayMode(DisplayMode.PrintLayout)
                        .Size = New System.Drawing.Size(iRptWidth, iRptHeight)
                        .ShowParameterPrompts = iShowParamArea
                        .ShowPromptAreaButton = iShowParamArea

                        .SetDisplayMode(DisplayMode.PrintLayout)
                        '.RefreshReport()
                        'Timer1.Start()


                    End With
                End If
            Catch ex As Exception
                ShowError("Error setting up form", ex)
                closetimer.Start()
            End Try
        End Sub

        '''<summary> Set Print Layout </summary>
        Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
            Timer1.Stop()
            ReportViewer1.SetDisplayMode(DisplayMode.PrintLayout)
        End Sub

        '''<summary> Close form </summary>
        Private Sub closetimer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles closetimer.Tick
            closetimer.Stop()
            Close()
        End Sub
    End Class

End Namespace
