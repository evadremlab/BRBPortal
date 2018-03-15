Imports IBM.Data.Informix
Public Class fProgress
    'Revisions:
    'BHS 8/24/07 On cancel, call a stored procedure on the server to terminate the job, and handle
    '   resulting connection error without notifying the user.
    Private iBW As System.ComponentModel.BackgroundWorker
    Private iCancel As Boolean = False

    Sub New(ByVal aBW As System.ComponentModel.BackgroundWorker, Optional ByVal aDescr As String = "")
        InitializeComponent()
        iBW = aBW
        txtDescr.Text = aDescr
    End Sub

    Private Sub fProgress_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Timer1.Start()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Timer1.Stop()
        Dim SW As New Stopwatch
        SW.Start()
        While iBW IsNot Nothing AndAlso iBW.IsBusy
            If iCancel Then Exit While
            'If Mid(My.User.Name, 1, 6) <> "AD\qsi" Then
            '    btnCancel.Visible = False
            '    btnCancel.Enabled = False
            'End If

            txtCount.Text = "(" & gBackgroundCount.ToString & ")"
            Threading.Thread.Sleep(1000)

            txtElapsed.Text = Format(SW.Elapsed.Hours, "00") & ":" & Format(SW.Elapsed.Minutes, "00") & ":" & Format(SW.Elapsed.Seconds, "00")
            'txtElapsed.Text = SW.Elapsed.Hours.ToString & ":" & SW.Elapsed.Minutes.ToString.ToString & ":" & SW.Elapsed.Seconds.ToString
            Application.DoEvents()
        End While

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Dim ID As Integer = Process.GetCurrentProcess.Id
        Dim MachName As String = My.Computer.Name
        Dim Result As String = ""
        'Try
        Dim Cn As New IfxConnection(gDBRootConnStr) 'Uses informix login instead of odbcusr
        Dim cmd As IfxCommand = BuildIfxCmd(Cn, "queryinterrupt", CommandType.StoredProcedure, 600) '5 minutes
        Dim Param As IfxParameter = BuildIfxParam("ProcessID", ID.ToString, ParameterDirection.Input, 20)
        cmd.Parameters.Add(Param)
        Param = BuildIfxParam("MachName", MachName, ParameterDirection.Input, 20)
        cmd.Parameters.Add(Param)
        Result = cmd.ExecuteScalar.ToString
        'Catch ex As Exception
        'MsgBox(ex.ToString)
        'End Try
        'MsgBox(Result)

        'Dim SID As Decimal = IfxGetNumber("SELECT sid FROM sysmaster:syssessions WHERE username = 'odbcusr' AND pid = " & ID.ToString & " AND tty = '" & MachName & "'")

        'iBW.CancelAsync()  BHS 8/24/07 Don't need, because job will terminate from server side

        'iPublicConn.Close()
        'iPublicConn.Dispose()
        iCancel = True
        Me.Close()
        Application.DoEvents()
    End Sub
End Class