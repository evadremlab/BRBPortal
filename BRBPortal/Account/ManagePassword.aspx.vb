Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.Owin

Partial Public Class ManagePassword
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("NextPage") Is Nothing Then Session("NextPage") = "ProfileList"
    End Sub

    Protected Sub ChangePassword_Click(sender As Object, e As EventArgs)
        Dim result = SignInStatus.Success
        Dim tSuccess As Boolean = False
        Dim tUser, tBill As String

        tUser = Session("UserCode").ToString
        tBill = Session("BillingCode").ToString

        'First, Validate the user id/password is correct
        result = UserAuth_Soap(tUser, tBill, CurrentPassword.Text)
        If result = SignInStatus.Failure Then
            ShowDialogOK("Current password is incorrect.", "Change Password")
            Return
        End If

        'Second verify password rules have been passed
        If CheckPswdRules(NewPWD.Value.ToString, tUser) = False Then
            ShowDialogOK("Password rules Not met. Must contain at least one number, one letter, one symbol (!@#$%^&_*) " &
                         "and be 7-20 characters and not contain part of you user id.", "Change Password")
            Return
        End If

        'Third, call API to reset the password
        If UpdatePassword_Soap(tUser, tBill, ChgXMLChars(CurrentPassword.Text), ChgXMLChars(NewPWD.Value.ToString),
                               ChgXMLChars(ConfirmNewPassword.Text)) = False Then
            ShowDialogOK("Error changing password. " & iErrMsg, "Change Password")
            Return
        End If

        If Session("NextPage").ToString = "ProfileConfirm" Then
            Response.Redirect("~/Account/ProfileConfirm.aspx", False)
        ElseIf Session("NextPage").ToString = "ProfileList" Then
            Response.Redirect("~/Account/ProfileList.aspx", False)
        Else
            Response.Redirect("~/Home.aspx", False)
        End If

    End Sub

    ''' <summary>Show a Dialog box with just OK.  </summary>
    Protected Sub ShowDialogOK(aMessage As String, Optional aTitle As String = "Status")
        'hfDialogID.Value = aDialogID
        ClientScript.RegisterStartupScript(Me.GetType(), "Popup", "ShowPopupOK('" + aMessage + "', '" + aTitle + "');", True)
    End Sub

    ''' <summary>Show a Yes/No Dialog box.  aDialogID defines the question for DialogResponse (below). </summary>
    Protected Sub ShowDialogYN(aDialogID As String, aMessage As String, aTitle As String, Optional aDialogData As String = "")
        hfDialogID.Value = aDialogID
        ClientScript.RegisterStartupScript(Me.GetType(), "Popup", "ShowPopupYN('" + aMessage + "', '" + aTitle + "');", True)
    End Sub

    ''' <summary>Receive Yes response from Dialog Box</summary>
    Protected Sub DialogResponseYes(sender As Object, e As EventArgs)

        Select Case hfDialogID.Value
            Case Else
                'Response.Redirect("~/Properties/MyUnits", False)
                Return
        End Select
    End Sub

    ''' <summary>Receive No response from Dialog Box</summary>
    Protected Sub DialogResponseNo(sender As Object, e As EventArgs)

        Select Case hfDialogID.Value
            Case Else
                Return
        End Select
    End Sub

End Class

