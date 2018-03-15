Public Class fError
    Private iUserTime As String
    Private iMsg As String
    Private iGenMsg As String
    Private iDetailMsg As String

    Sub New(ByVal aUserTime As String, ByVal aMsg As String, ByVal aGenMsg As String, ByVal aDetailMsg As String)
        InitializeComponent()
        iUserTime = aUserTime
        iMsg = aMsg
        iGenMsg = aGenMsg
        iDetailMsg = aDetailMsg
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Close()
    End Sub

    Private Sub btnDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDetails.Click
        MsgBox(iDetailMsg, MsgBoxStyle.Critical, "Error Detail")
    End Sub

    Private Sub fError_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtUserTime.Text = iUserTime
        txtMsg.Text = iMsg
        txtGeneral.Text = iGenMsg

        'Move txtGeneral based on txtMsg.Height
        Dim P As Point = txtGeneral.Location
        P.Y = txtMsg.Height + 14
        txtGeneral.Location = P

        'Move txtUserTime based on txtMsg.Height
        P = txtUserTime.Location
        P.Y = txtMsg.Height + 57
        txtUserTime.Location = P

    End Sub
End Class