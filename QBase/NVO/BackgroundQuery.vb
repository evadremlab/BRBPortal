'Imports IBM.Data.Informix
Imports System.ComponentModel
Imports QSILib.Windows.Forms
Imports QSILib

'BackgroundSQL Class allows multiple background workers for a user
Public Class BackgroundQuery

    Private WithEvents iBW As New BackgroundWorker
    Private iCallingForm As fBase
    Private iSW As New Stopwatch
    Private ifrm As fProgress
    Public iEx As Exception
    Public iProcID As String = ""
    Public iIsRunning As Boolean = True
    Private iSQLConnection As New SqlClient.SqlConnection(gSQLConnStr.Replace("ptswriter", "ptswriter2")) 'GBV 1/20/2014

    ' GBV 1/17/2014 - Added optional argument to allow auto cancelling in nn seconds
    '''<summary> Start background Query </summary>
    Public Sub New(ByVal aSQL As String, ByRef aForm As fBase, Optional ByVal aTitle As String = "", Optional aCancelInSeconds As Integer = 0)
        iSQLConnection.Open()
        Dim SPID As Integer = CInt(SQLGetNumber(iSQLConnection, "SELECT @@SPID", "", True, True))
        iCallingForm = aForm
        gBackgroundCount += 1
        iBW.WorkerSupportsCancellation = True
        iBW.RunWorkerAsync(aSQL)
        iProcID = Process.GetCurrentProcess.Id.ToString
        ifrm = New fProgress(iBW, aTitle, Process.GetCurrentProcess.Id.ToString, aCancelInSeconds)
        ifrm.iSPID = SPID ' GBV 1/20/2014 - used to KILL connection
        ifrm.iSQLConnection = iSQLConnection ' GBV 1/20/2014 - connection should be closed on cancel

        ' GBV - 11/14/2014
        If gIsFromCron Then
            ifrm.Timer1_Tick(Nothing, Nothing)
        Else
            ifrm.ShowDialog()
        End If

    End Sub

    'Frm cancels iBW directly, so don't need logic here.

    '''<summary> Run BuildDV </summary>
    Private Sub BW_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles iBW.DoWork
        'Dim SQL As String = e.Argument.ToString
        iSW.Start()
        'Dim DAD As New IfxDataAdapter(SQL, iPublicConn)
        'If Not iPublicConn.State = ConnectionState.Open Then iPublicConn.Open()
        'Dim DSD As New DataSet

        'Try
        '    DAD.SelectCommand.CommandTimeout = 1200  'BHS 20 minutes
        '    DAD.Fill(DSD)
        '    TrimTable(DSD.Tables(0))
        '    DAD.Dispose()
        'Catch ex As Exception
        '    'Ignore abandoned mutex
        '    If ex.Message.IndexOf("abandoned mutex") = -1 Then TryError("IfxBuildDV --- " & SQL, ex)
        'End Try
        'Dim DV As DataView = DSD.Tables(0).DefaultView

        Try
            'Note, this always runs against the Appl.ConnType.  We may need to change this in the future if we want to do
            'long running queries against informix AND sql server.
            'ifrm.lblExeProcID.Text = Process.GetCurrentProcess.Id.ToString
            'BHS 7/21/09 remove final True parameter to make it ODBC, since it is always ODBC from now on
            'Dim DV As DataView = IfxBuildDV(e.Argument.ToString, False, "str", gODBCConnStr)  'BHS 7/7/09 use ODBCDataAdapter

            'BHS 12/7/09 Final False in call below forces use of a separate connection string for background queries, 
            'so they can clean up while new queries are started

            Dim DV As DataView
            If Appl.ConnType = "SQL" Then
                ' GBV 1/20/2014 - Using the same connection opened before
                Dim SQLStr As String = IfxToSQLSyntax(e.Argument.ToString)
                DV = SQLBuildDV(SQLStr, iSQLConnection)
            Else
                DV = IfxBuildDV(e.Argument.ToString, False, , gODBCConnStr, , , False)  'BHS 7/7/09 use ODBCDataAdapter - GBV 7/24/2008
            End If

            If iBW.CancellationPending = False Then e.Result = DV
            'Normally, LogError is called from TryError, but since this is a background thread we 
            'Catch ex As Exception When LogError("BackgroundQuery::DoWork", ex)
        Catch ex As Exception
            'Don't report network error - this is the symptom if the job is cancelled from the server.
            If ex.ToString.ToLower.IndexOf("error occurred in network function") > -1 Then Return
            If ex.ToString.ToLower.IndexOf("communication link failure") > -1 Then Return
            ' GBV 1/20/2014 - error returned when killing process in All-SQL
            If ex.ToString.ToLower.IndexOf("A severe error ocurred on the current command") > -1 Then Return
            If ex.ToString.ToLower.IndexOf("An existing connection was forcibly closed by the remote host") > -1 Then Return
            'If ex.ToString.ToLower.IndexOf("protected memory") > -1 Then Return '7/7/09

            iEx = ex 'Otherwise, make the error available to the calling thread
            Return
        End Try



    End Sub

    '''<summary> Write result to iDV so client can see it </summary>
    Private Sub BW_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles iBW.RunWorkerCompleted
        'BHS 12/4/09    If cancel happened after population was complete, we need to close the connection with the "communication link failure"
        If gODBCCn IsNot Nothing AndAlso _
           gODBCCn.State = ConnectionState.Open Then gODBCCn.Close()

        ' GBV 1/20/2014
        If iSQLConnection.State = ConnectionState.Open Then iSQLConnection.Close()

        'Trying to get form to close before the rest of the processing happens
        If ifrm IsNot Nothing Then
            ifrm.btnCancel.Visible = False
            ifrm.txtDescr.Text = "Calculating"
            ifrm.Refresh()
            ifrm.Close()
            Application.DoEvents()
        End If
        
        iSW.Stop()
        gBackgroundCount -= 1
        iCallingForm.iElapsed = iSW.Elapsed
        iIsRunning = False
        

        If e.Result IsNot Nothing Then
            iCallingForm.BackgroundQueryComplete(CType(e.Result, DataView))
        Else
            iCallingForm.BackgroundQueryCancelled()
        End If
    End Sub

End Class
