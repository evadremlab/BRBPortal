Imports System
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.EntityFramework
Imports Microsoft.AspNet.Identity.Owin
Imports Owin

Partial Public Class UpdateAccount
    Inherits Page
    'Protected Sub CreateUser_Click(sender As Object, e As EventArgs)
    '    Dim userName As String = ""
    '    Dim manager = Context.GetOwinContext().GetUserManager(Of ApplicationUserManager)()
    '    Dim signInManager = Context.GetOwinContext().Get(Of ApplicationSignInManager)()
    '    Dim user = New ApplicationUser() With {.UserName = userName, .Email = userName}
    '    Dim result = manager.Create(user, BillCode.Text)
    '    If result.Succeeded Then
    '        ' For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
    '        ' Dim code = manager.GenerateEmailConfirmationToken(user.Id)
    '        ' Dim callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request)
    '        ' manager.SendEmail(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=""" & callbackUrl & """>here</a>.")

    '        signInManager.SignIn(user, isPersistent:=False, rememberBrowser:=False)
    '        IdentityHelper.RedirectToReturnUrl(Request.QueryString("ReturnUrl"), Response)
    '    Else
    '        ErrorMessage.Text = result.Errors.FirstOrDefault()
    '    End If
    'End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim wstr As String = ""

        wstr = Request.QueryString("field1")
        UserIDCode.Text = wstr

        Select Case wstr
            Case "KSmith123"
                BillCode.Text = "KSmith"
                FirstName.Text = "Kelly"
                MidName.Text = ""
                LastName.Text = "Smith"
                Suffix.Text = ""
                MailAddress.Text = "1 Lane Street, Boston, MA 12345"
                EmailAddress.Text = "Ksmith@gmail.com"
                PhoneNo.Text = "(245)234-0000"
                SecAnswer.Text = "Ocean Blue"

            Case "SurferDude"
                BillCode.Text = "MSmith1"
                FirstName.Text = "Mark"
                MidName.Text = "E."
                LastName.Text = "Smith"
                Suffix.Text = "Jr."
                MailAddress.Text = "7 Wave Court, Santa Cruz, CA 94345"
                EmailAddress.Text = "SmithJr3@aol.com"
                PhoneNo.Text = "(923)646-3222"
                SecAnswer.Text = "Mouse Club"

            Case "MBtywed"
                BillCode.Text = "VSmith"
                FirstName.Text = "Vic"
                MidName.Text = ""
                LastName.Text = "Smith"
                Suffix.Text = ""
                MailAddress.Text = "123 Prince Street, Berkeley, CA 94704"
                EmailAddress.Text = "VictorS@KBS.com"
                PhoneNo.Text = "(510)981-4343"
                SecAnswer.Text = "Ocean Spray"
        End Select
    End Sub

    Protected Sub UpdateUser_Click(sender As Object, e As EventArgs)

        'API_UpdUser

        Response.Redirect("~Administration\SearchAccounts.aspx", False)
    End Sub

    Protected Sub ResetPaswd_Click(sender As Object, e As EventArgs)

        'Generate temporary password

        'Email temp password to user

        Response.Redirect("~Administration\SearchAccounts.aspx", False)
    End Sub

    Protected Sub Cancel_Click(sender As Object, e As EventArgs)
        Response.Redirect("~Administration\SearchAccounts.aspx", False)
    End Sub
End Class

