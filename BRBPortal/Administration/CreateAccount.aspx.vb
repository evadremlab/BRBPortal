Imports System
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.EntityFramework
Imports Microsoft.AspNet.Identity.Owin
Imports Owin

Partial Public Class CreateAccount
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub Cancel_Click(sender As Object, e As EventArgs)
        Response.Redirect("~Administration\SearchAccounts.aspx", False)
    End Sub

    Protected Sub CreateUser_Click(sender As Object, e As EventArgs)

        'API_CreateUser

        Response.Redirect("~Administration\SearchAccounts.aspx", False)
    End Sub
End Class

