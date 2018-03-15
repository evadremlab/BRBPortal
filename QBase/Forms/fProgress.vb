
Public Class fProgress
    Private iBW As System.ComponentModel.BackgroundWorker
    Public iCancel As Boolean = False
    Private iProcID As String = ""
    Private iCancelInSeconds As Integer = 0 ' GBV 1/17/2014
    Public iSPID As Integer = 0
    Public iSQLConnection As SqlClient.SqlConnection = Nothing

    ' GBV 1/17/2014 - Added optional argument to allow auto cancelling in nn seconds
    ' GBV 6/4/2014 - made aBW parameter "ByRef"
    ' SRM 5/11/2015 - Removed Informix Logic

    '''<summary> Pass background worker pointer, and a description to show in the progress window.text </summary>
    Sub New(ByRef aBW As System.ComponentModel.BackgroundWorker, Optional ByVal aDescr As String = "", Optional ByVal aProcID As String = "", Optional ByVal aCancelInSeconds As Integer = 0)
        InitializeComponent()
        iBW = aBW
        txtDescr.Text = aDescr
        iProcID = aProcID
        If aCancelInSeconds > 0 Then iCancelInSeconds = aCancelInSeconds ' GBV 1/17/2014
    End Sub

    '''<summary> Start timer to run logic after regular load finisheds </summary>
    Private Sub fProgress_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Timer1.Start()
    End Sub

    '''<summary> Check once a second to see if background worker has finished, or if iCancel has been set true </summary>
    Public Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Timer1.Stop()
        Dim SW As New Stopwatch
        SW.Start()
        While iBW IsNot Nothing AndAlso iBW.IsBusy
            If iCancel Then Exit While

            txtCount.Text = "(" & gBackgroundCount.ToString & ")"
            Threading.Thread.Sleep(1000)

            txtElapsed.Text = Format(SW.Elapsed.Hours, "00") & ":" & Format(SW.Elapsed.Minutes, "00") & ":" & Format(SW.Elapsed.Seconds, "00")

            Application.DoEvents()
            If iCancelInSeconds > 0 Then ' GBV 1/17/2014 - Auto-cancelling
                If SW.Elapsed.TotalSeconds > iCancelInSeconds Then
                    btnCancel.PerformClick()
                    MsgBox("Your query exceeded the time allowed to non-administrators.", MsgBoxStyle.Information)
                    Exit While
                End If
            End If
        End While

    End Sub

    '''<summary> User clicks cancel button </summary>
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
       
        Try
            ' GBV 1/20/2014 - Kill server process
            If iSQLConnection IsNot Nothing AndAlso iSQLConnection.State = ConnectionState.Open Then
                If iSPID > 0 Then
                    ' GBV 1/21/2010 - we need to pop this connection out of the pool, because after the KILL it could not be reused
                    If iSQLConnection IsNot Nothing Then SqlClient.SqlConnection.ClearPool(iSQLConnection)
                    SQLDoSQL("KILL " & iSPID.ToString)
                End If
            End If
                If iBW IsNot Nothing Then ' GBV 6/4/2014 - prevent NullReferenceException errors
                    iBW.CancelAsync()
                    iBW = Nothing
                End If

        Catch ex As Exception
            ShowError("Problem cancelling search", ex)
        Finally
            If iSQLConnection IsNot Nothing AndAlso iSQLConnection.State <> ConnectionState.Closed Then iSQLConnection.Close()
        End Try

        iCancel = True
        Me.Close()
        Application.DoEvents()
    End Sub

End Class