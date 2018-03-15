Imports System.Web
Imports System.Web.UI
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.EntityFramework
Imports Microsoft.AspNet.Identity.Owin
Imports Microsoft.Owin.Security
Imports Owin
Imports System.Net
Imports System.Xml
Imports System.IO
Imports QSILib

Partial Public Class Login
    Inherits Page
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.Form.DefaultButton = Me.btnLogin.UniqueID
        UserIDCode.Focus()
    End Sub

    Protected Sub LogIn(sender As Object, e As EventArgs)
        Dim IDCode As String = ""
        Dim tBillCode As String = ""
        Dim Pwd As String = ""
        Dim ProfileStr As String = ""
        Dim wstr, wstr2 As String

        If IsValid Then
            'Temporary
            Dim result = SignInStatus.Success

            If UserIDCode.Text.Length > 0 Then
                IDCode = UserIDCode.Text
            ElseIf BillCode.Text.Length > 0 Then
                tBillCode = BillCode.Text
            End If
            Pwd = Password.Text

            result = UserAuth_Soap(IDCode, tBillCode, Pwd)

            Select Case result
                Case SignInStatus.Success
                    ProfileStr = GetProfile_Soap(IDCode, tBillCode)
                    If ProfileStr.Length > 0 Then
                        wstr2 = ProfileStr
                        wstr = ParseStr(wstr2, "::")    'User Code
                        If wstr.Length > wstr.IndexOf("=") Then IDCode = wstr.Substring(wstr.IndexOf("=") + 1)
                        wstr = ParseStr(wstr2, "::")    'Billing Code
                        If wstr.Length > wstr.IndexOf("=") Then tBillCode = wstr.Substring(wstr.IndexOf("=") + 1)
                    End If

                    Session("UserCode") = IDCode
                    Session("BillingCode") = tBillCode
                    Session("FirstTimeLogin") = iFirstlogin
                    Session("Relationship") = iRelate
                    Session("TempPwd") = iTempPwd

                    If iTempPwd.ToUpper = "TRUE" Then
                        wstr = ""
                        If iFirstlogin = True Then
                            Session("NextPage") = "ProfileConfirm"
                        Else
                            Session("NextPage") = "Home"
                        End If
                        Response.Redirect("~/Account/ManagePassword.aspx", False)
                        Exit Select
                    ElseIf iFirstlogin.ToUpper = "TRUE" Then
                        Session("NextPage") = "Home"
                        Response.Redirect("~/Account/ProfileConfirm.aspx", False)
                        Exit Select
                    Else
                        Response.Redirect("~/Home.aspx", False)
                        Exit Select
                    End If

                Case Else
                    FailureText.Text = "Invalid login attempt"
                    ErrorMessage.Visible = True

                    Session.Clear()

                    Exit Select
            End Select
        End If
    End Sub


End Class
