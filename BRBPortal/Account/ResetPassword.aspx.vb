Imports System
Imports QSILib

Partial Public Class ResetPassword
    Inherits System.Web.UI.Page

    Protected Property StatusMessage() As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack = True Then
            Quest1.Text = ""
            Quest2.Text = ""
            btnResetPWD.Enabled = False
            FailureText.Text = ""
            ErrMessage.Visible = False
        End If

    End Sub

    Protected Sub Reset_Click(sender As Object, e As EventArgs)
        Dim XMLstr As String

        XMLstr = "<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:api=""http://cityofberkeley.info/RTS/ClientPortal/API"">"
        XMLstr += "<soapenv:Header/> "
        XMLstr += "<soapenv:Body>"
        XMLstr += "<api:validateResetUserPassword>"
        XMLstr += "<resetUserPwdReq>"
        XMLstr += "<!--Optional:-->"
        XMLstr += "<userId>" & UserIDCode.Text & "</userId>"
        XMLstr += "<!--Optional:-->"
        XMLstr += "<billingCode>" & BillingCode.Text & "</billingCode>"
        XMLstr += "<securityQuestion1>" & Quest1.Text & "</securityQuestion1>"
        XMLstr += "<securityAnswer1>" & Answer1.Text & "</securityAnswer1>"
        XMLstr += "<securityQuestion2>" & Quest2.Text & "</securityQuestion2>"
        XMLstr += "<securityAnswer2>" & Answer2.Text & "</securityAnswer2>"
        XMLstr += "</resetUserPwdReq>"
        XMLstr += "</api:validateResetUserPassword>"
        XMLstr += "</soapenv:Body>"
        XMLstr += "</soapenv:Envelope>"

        If ValidateReset_Soap(XMLstr) = True Then
            ShowDialogOK("Temporary password has been sent. Please login using temporary password.", "Forgot Password")
            Response.Redirect("~\Account\Login.aspx", False)
        Else
            ShowDialogOK("Security answer(s) did not match.", "Reset Password")
            Return
        End If

    End Sub

    Protected Sub SecurityAnswer_TextChanged(sender As Object, e As EventArgs) Handles Answer1.TextChanged, Answer2.TextChanged

        If (Answer1.Text.Length > 0 AndAlso Quest1.Text.Length > 0) _
            OrElse (Answer2.Text.Length > 0 AndAlso Quest2.Text.Length > 0) Then
            btnResetPWD.Enabled = True
        Else
            btnResetPWD.Enabled = False
        End If
    End Sub

    Protected Sub UserIDCode_TextChanged(sender As Object, e As EventArgs) Handles UserIDCode.TextChanged
        Dim RetStr, wstr, wstr2 As String

        'Get profile information for this UserIDCode
        RetStr = GetProfile_Soap(UserIDCode.Text, BillingCode.Text)

        'Parse return string
        If RetStr.Length > 0 Then
            wstr2 = RetStr
            wstr = ParseStr(wstr2, "::")    'User Code
            'If wstr.Length > wstr.IndexOf("=") Then UserIDCode.Text = wstr.Substring(wstr.IndexOf("=") + 1)
            wstr = ParseStr(wstr2, "::")    'Billing Code
            If wstr.Length > wstr.IndexOf("=") Then BillingCode.Text = wstr.Substring(wstr.IndexOf("=") + 1)
            wstr = ParseStr(wstr2, "::")    'First Name
            wstr = ParseStr(wstr2, "::")    'Middle Name
            wstr = ParseStr(wstr2, "::")    'Last Name
            wstr = ParseStr(wstr2, "::")    'Suffifx
            wstr = ParseStr(wstr2, "::")    'Full Name
            wstr = ParseStr(wstr2, "::")    'Mail Address
            wstr = ParseStr(wstr2, "::")    'Street Number
            wstr = ParseStr(wstr2, "::")    'Street Name
            wstr = ParseStr(wstr2, "::")    'Unit
            wstr = ParseStr(wstr2, "::")    'Full Address
            wstr = ParseStr(wstr2, "::")    'City
            wstr = ParseStr(wstr2, "::")    'State
            wstr = ParseStr(wstr2, "::")    'Zip
            wstr = ParseStr(wstr2, "::")    'Country
            wstr = ParseStr(wstr2, "::")    'Email
            wstr = ParseStr(wstr2, "::")    'Phone
            wstr = ParseStr(wstr2, "::")    'Question 1
            If wstr.Length > wstr.IndexOf("=") Then Quest1.Text = wstr.Substring(wstr.IndexOf("=") + 1)
            wstr = ParseStr(wstr2, "::")    'Answer 1
            'If wstr.Length > wstr.IndexOf("=") Then Session("SecAnsw1") = wstr.Substring(wstr.IndexOf("=") + 1)
            wstr = ParseStr(wstr2, "::")    'Questionn 2
            If wstr.Length > wstr.IndexOf("=") Then Quest2.Text = wstr.Substring(wstr.IndexOf("=") + 1)
            wstr = ParseStr(wstr2, "::")    'Answer 2
            'If wstr.Length > wstr.IndexOf("=") Then Session("SecAnsw2") = wstr.Substring(wstr.IndexOf("=") + 1)
        Else
            ShowDialogOK("Error: Invalid User ID or Billing Code.", "Reset Password")
            'MsgBox("Error: Invalid User ID or Billing Code.", vbOKOnly)
            'Session("SecAnsw1") = ""
            'Session("SecAnsw2") = ""
            Return
        End If

        btnResetPWD.Enabled = True
    End Sub

    Protected Sub BillingCode_TextChanged(sender As Object, e As EventArgs) Handles BillingCode.TextChanged
        Dim RetStr, wstr, wstr2 As String

        'Get profile information for this UserIDCode
        RetStr = GetProfile_Soap(UserIDCode.Text, BillingCode.Text)

        'Parse return string
        If RetStr.Length > 0 Then
            wstr2 = RetStr
            wstr = ParseStr(wstr2, "::")    'User Code
            If wstr.Length > wstr.IndexOf("=") Then UserIDCode.Text = wstr.Substring(wstr.IndexOf("=") + 1)
            wstr = ParseStr(wstr2, "::")    'Billing Code
            If wstr.Length > wstr.IndexOf("=") Then BillingCode.Text = wstr.Substring(wstr.IndexOf("=") + 1)
            wstr = ParseStr(wstr2, "::")    'First Name
            wstr = ParseStr(wstr2, "::")    'Middle Name
            wstr = ParseStr(wstr2, "::")    'Last Name
            wstr = ParseStr(wstr2, "::")    'Suffifx
            wstr = ParseStr(wstr2, "::")    'Full Name
            wstr = ParseStr(wstr2, "::")    'Mail Address
            wstr = ParseStr(wstr2, "::")    'Street Number
            wstr = ParseStr(wstr2, "::")    'Street Name
            wstr = ParseStr(wstr2, "::")    'Unit
            wstr = ParseStr(wstr2, "::")    'Full Address
            wstr = ParseStr(wstr2, "::")    'City
            wstr = ParseStr(wstr2, "::")    'State
            wstr = ParseStr(wstr2, "::")    'Zip
            wstr = ParseStr(wstr2, "::")    'Country
            wstr = ParseStr(wstr2, "::")    'Email
            wstr = ParseStr(wstr2, "::")    'Phone
            wstr = ParseStr(wstr2, "::")    'Question 1
            If wstr.Length > wstr.IndexOf("=") Then Quest1.Text = wstr.Substring(wstr.IndexOf("=") + 1)
            wstr = ParseStr(wstr2, "::")    'Answer 1
            'If wstr.Length > wstr.IndexOf("=") Then Session("SecAnsw1") = wstr.Substring(wstr.IndexOf("=") + 1)
            wstr = ParseStr(wstr2, "::")    'Question 2
            If wstr.Length > wstr.IndexOf("=") Then Quest2.Text = wstr.Substring(wstr.IndexOf("=") + 1)
            wstr = ParseStr(wstr2, "::")    'Answer 2
            'If wstr.Length > wstr.IndexOf("=") Then Session("SecAnsw2") = wstr.Substring(wstr.IndexOf("=") + 1)

            btnResetPWD.Enabled = True
        Else
            ShowDialogOK("Error: Invalid User ID or Billing Code.", "Reset Password")
            'MsgBox("Error: Invalid User ID or Billing Code.", vbOKOnly)
            'Session("SecAnsw1") = ""
            'Session("SecAnsw2") = ""

            btnResetPWD.Enabled = False
            Return
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