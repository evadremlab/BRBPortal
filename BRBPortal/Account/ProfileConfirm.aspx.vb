Imports QSILib

Public Class ProfileConfirm
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim RetStr, wstr, wstr2 As String

        If Not IsPostBack = True Then
            If Session("UserCode") Is Nothing OrElse Session("BillingCode") Is Nothing OrElse
            Session("UserCode").ToString = "" OrElse Session("BillingCode").ToString = "" Then
                Response.Redirect("..\Account\Login", False)
                Return
            End If

            RetStr = GetProfile_Soap(Session("UserCode").ToString, Session("BillingCode").ToString)

            'Parse return string
            If RetStr.Length > 0 Then
                wstr2 = RetStr
                wstr = ParseStr(wstr2, "::")
                If wstr.Length > wstr.IndexOf("=") Then UserIDCode0.Text = wstr.Substring(wstr.IndexOf("=") + 1)
                wstr = ParseStr(wstr2, "::")
                If wstr.Length > wstr.IndexOf("=") Then BillCode0.Text = wstr.Substring(wstr.IndexOf("=") + 1)
                wstr = ParseStr(wstr2, "::")    'First Name
                wstr = ParseStr(wstr2, "::")    'Middle Name
                wstr = ParseStr(wstr2, "::")    'Last Name
                wstr = ParseStr(wstr2, "::")    'Suffifx
                wstr = ParseStr(wstr2, "::")    'Full Name
                If wstr.Length > wstr.IndexOf("=") Then FullName0.Text = wstr.Substring(wstr.IndexOf("=") + 1)
                wstr = ParseStr(wstr2, "::")
                If wstr.Length > wstr.IndexOf("=") Then MailAddress0.Text = wstr.Substring(wstr.IndexOf("=") + 1)
                wstr = ParseStr(wstr2, "::")
                If wstr.Length > wstr.IndexOf("=") Then EmailAddress0.Text = wstr.Substring(wstr.IndexOf("=") + 1)
                wstr = ParseStr(wstr2, "::")
                If wstr.Length > wstr.IndexOf("=") Then PhoneNo0.Text = wstr.Substring(wstr.IndexOf("=") + 1)
                wstr = ParseStr(wstr2, "::")    'Question 1
                wstr = ParseStr(wstr2, "::")    'Answer 1
                wstr = ParseStr(wstr2, "::")    'Questionn 2
                wstr = ParseStr(wstr2, "::")    'Answer 2
                wstr = ParseStr(wstr2, "::")    'Agency Name
                If wstr.Length > wstr.IndexOf("=") AndAlso Session("Relationship").ToString.ToUpper = "AGENT" Then
                    FullName0.Text = wstr.Substring(wstr.IndexOf("=") + 1)
                End If
            Else
                UserIDCode0.Text = ""
                BillCode0.Text = ""
                FullName0.Text = ""
                MailAddress0.Text = ""
                EmailAddress0.Text = ""
                PhoneNo0.Text = ""
            End If

            btnSubmit.Enabled = False
        End If

    End Sub

    Protected Sub SubmitProfile_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Dim tSuccess As Boolean = False

        tSuccess = ConfirmProfile_Soap(UserIDCode0.Text, BillCode0.Text, DeclareInits.Text)

        If tSuccess = False Then
            ShowDialogOK("Error updating confirmation.", "Confirm Profile")
            'MsgBox("Error updating confirmation.")
        End If

        Response.Redirect("..\Home.aspx", False)
    End Sub

    Protected Sub CancelProfile_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Session.Clear()
        Response.Redirect("..\Account\Login", False)
    End Sub

    Protected Sub chkDeclare_CheckedChanged(sender As Object, e As EventArgs) Handles chkDeclare.CheckedChanged

        If chkDeclare.Checked = True AndAlso DeclareInits.Text.Length > 0 Then
            btnSubmit.Enabled = True
        Else
            btnSubmit.Enabled = False
        End If
    End Sub

    Protected Sub DeclareInits_TextChanged(sender As Object, e As EventArgs) Handles DeclareInits.TextChanged

        If DeclareInits.Text.Length > 0 AndAlso chkDeclare.Checked = True Then
            btnSubmit.Enabled = True
        Else
            btnSubmit.Enabled = False
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