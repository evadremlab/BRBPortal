Public Class Logout
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        Session.Clear()

        Response.Redirect("~/Account/Login", False)
    End Sub
End Class