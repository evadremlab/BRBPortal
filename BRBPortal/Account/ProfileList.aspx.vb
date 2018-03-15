Imports QSILib

Public Class ProfileList
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim RetStr, wstr, wstr2 As String

        If Not IsPostBack = True Then
            If Session("UserCode") Is Nothing OrElse Session("BillingCode") Is Nothing OrElse
                Session("UserCode").ToString = "" OrElse Session("BillingCode").ToString = "" Then
                Response.Redirect("..\Account\Login", False)
                Return
            End If

            wstr = Session("UserCode").ToString
            wstr2 = Session("BillingCode").ToString

            If wstr <> "" Or wstr2 <> "" Then
                RetStr = GetProfile_Soap(wstr, wstr2)

                'Parse return string
                If RetStr.Length > 0 Then
                    wstr2 = RetStr
                    wstr = ParseStr(wstr2, "::")    'User Code
                    If wstr.Length > wstr.IndexOf("=") Then UserIDCode1.Text = wstr.Substring(wstr.IndexOf("=") + 1)
                    wstr = ParseStr(wstr2, "::")    'Billing Code
                    If wstr.Length > wstr.IndexOf("=") Then BillCode1.Text = wstr.Substring(wstr.IndexOf("=") + 1)
                    wstr = ParseStr(wstr2, "::")    'First Name
                    wstr = ParseStr(wstr2, "::")    'Middle Name
                    wstr = ParseStr(wstr2, "::")    'Last Name
                    wstr = ParseStr(wstr2, "::")    'Suffifx
                    wstr = ParseStr(wstr2, "::")    'Full Name
                    If wstr.Length > wstr.IndexOf("=") Then FullName1.Text = wstr.Substring(wstr.IndexOf("=") + 1)
                    wstr = ParseStr(wstr2, "::")    'Mail Address
                    If wstr.Length > wstr.IndexOf("=") Then MailAddress1.Text = wstr.Substring(wstr.IndexOf("=") + 1)
                    wstr = ParseStr(wstr2, "::")    'Street Number
                    wstr = ParseStr(wstr2, "::")    'Street Name
                    wstr = ParseStr(wstr2, "::")    'Unit
                    wstr = ParseStr(wstr2, "::")    'Full Address
                    wstr = ParseStr(wstr2, "::")    'City
                    wstr = ParseStr(wstr2, "::")    'State
                    wstr = ParseStr(wstr2, "::")    'Zip
                    wstr = ParseStr(wstr2, "::")    'Country
                    wstr = ParseStr(wstr2, "::")    'Email
                    If wstr.Length > wstr.IndexOf("=") Then EmailAddress1.Text = wstr.Substring(wstr.IndexOf("=") + 1)
                    wstr = ParseStr(wstr2, "::")    'Phone
                    If wstr.Length > wstr.IndexOf("=") Then PhoneNo1.Text = wstr.Substring(wstr.IndexOf("=") + 1)
                    wstr = ParseStr(wstr2, "::")    'Question 1
                    If wstr.Length > wstr.IndexOf("=") Then Quest1.Text = wstr.Substring(wstr.IndexOf("=") + 1)
                    wstr = ParseStr(wstr2, "::")    'Answer 1
                    'If wstr.Length > wstr.IndexOf("=") Then Answer1.Text = wstr.Substring(wstr.IndexOf("=") + 1)
                    wstr = ParseStr(wstr2, "::")    'Questionn 2
                    If wstr.Length > wstr.IndexOf("=") Then Quest2.Text = wstr.Substring(wstr.IndexOf("=") + 1)
                    wstr = ParseStr(wstr2, "::")    'Answer 2
                    'If wstr.Length > wstr.IndexOf("=") Then Answer2.Text = wstr.Substring(wstr.IndexOf("=") + 1)
                    wstr = ParseStr(wstr2, "::")    'Agency Name
                    If wstr.Length > wstr.IndexOf("=") AndAlso Session("Relationship").ToString.ToUpper = "AGENT" Then
                        FullName1.Text = wstr.Substring(wstr.IndexOf("=") + 1)
                    End If
                Else
                    UserIDCode1.Text = ""
                    BillCode1.Text = ""
                    FullName1.Text = ""
                    MailAddress1.Text = ""
                    EmailAddress1.Text = ""
                    PhoneNo1.Text = ""
                    'Answer1.Text = ""
                    'Answer2.Text = ""
                    Quest1.Text = ""
                    Quest2.Text = ""
                End If
            Else
                UserIDCode1.Text = ""
                BillCode1.Text = ""
                FullName1.Text = ""
                MailAddress1.Text = ""
                EmailAddress1.Text = ""
                PhoneNo1.Text = ""
                'Answer1.Text = ""
                'Answer2.Text = ""
                Quest1.Text = ""
                Quest2.Text = ""
            End If
        End If

    End Sub

    Protected Sub EditProfile_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        Response.Redirect("~\Account\ProfileEdit.aspx", False)
    End Sub

    Protected Sub CancelList_Click(sender As Object, e As EventArgs)
        Response.Redirect("~\Home.aspx", False)
    End Sub
End Class