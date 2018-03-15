Public Class fSyntaxError
    Private iError As String

    '''<summary> Called from ShowError in qFunctions </summary>
    Sub New(ByVal aSQLDescr As String, ByVal aError As String)
        InitializeComponent()
        txtCriteria.Text = aSQLDescr
        iError = aError
        If iError.Length = 0 Then
            btnDetails.Visible = False
            'iBadSQL = True  'Prevent the SQL from even being submitted by BuildDV
        End If


    End Sub

    '''<summary> Load form </summary>
    Private Sub fError_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If txtCriteria.Text.Length > 0 Then
            lblCriteria.Visible = True
        Else
            lblCriteria.Visible = False
        End If
    End Sub

    '''<summary> OK button clicked </summary>
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Try
            Close()
        Catch ex As Exception
            ShowError("Error closing Database Search Problem form", ex)
        End Try

    End Sub

    '''<summary> Detail button clicked </summary>
    Private Sub btnDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDetails.Click
        Try
            MsgBox(iError, MsgBoxStyle.Critical, "Error Detail")
        Catch ex As Exception
            ShowError("Error showing Details from Database Search Problem form", ex)
        End Try

    End Sub

    '''<summary> Help button clicked </summary>
    Private Sub btnSearchHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearchHelp.Click
        Try
            Dim frm As New QSILib.Windows.Forms.fhMain("Search")
            frm.Show()
        Catch ex As Exception
            ShowError("Error showing help", ex)
            Return  'Not fatal
        End Try
    End Sub

End Class