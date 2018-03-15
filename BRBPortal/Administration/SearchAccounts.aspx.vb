Public Class SearchAccounts
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim str As String


    End Sub

    Protected Sub UpdAcctbtn_Click(sender As Object, e As EventArgs) Handles btnUpdAcct.Click
        Dim wstr As String = ""

        If Row1Sel.Checked Then wstr = UserID1.InnerText
        If Row2Sel.Checked Then wstr = UserID2.InnerText
        If Row3Sel.Checked Then wstr = UserID3.InnerText
        Response.Redirect("~/Administration/UpdateAccount?field1=" & wstr)
    End Sub

    Protected Sub CreateAcct_Click(sender As Object, e As EventArgs) Handles btnCreateAcct.Click
        Response.Redirect("~/Administration/CreateAccount")
    End Sub

    Protected Sub Searchbtn_Click(sender As Object, e As EventArgs) Handles btnSearch.Click

    End Sub
End Class