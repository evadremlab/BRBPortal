Imports QSILib

Public Class ProfileEdit
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
                    Session("ProfileXML") = RetStr
                    wstr2 = RetStr
                    wstr = ParseStr(wstr2, "::")    'User Code
                    If wstr.Length > wstr.IndexOf("=") Then UserIDCode2.Text = wstr.Substring(wstr.IndexOf("=") + 1)
                    wstr = ParseStr(wstr2, "::")    'Billing Code
                    If wstr.Length > wstr.IndexOf("=") Then BillCode2.Text = wstr.Substring(wstr.IndexOf("=") + 1)
                    wstr = ParseStr(wstr2, "::")    'First Name
                    If wstr.Length > wstr.IndexOf("=") Then FirstName.Text = wstr.Substring(wstr.IndexOf("=") + 1)
                    wstr = ParseStr(wstr2, "::")    'Middle Name
                    If wstr.Length > wstr.IndexOf("=") Then MidName.Text = wstr.Substring(wstr.IndexOf("=") + 1)
                    wstr = ParseStr(wstr2, "::")    'Last Name
                    If wstr.Length > wstr.IndexOf("=") Then LastName.Text = wstr.Substring(wstr.IndexOf("=") + 1)
                    wstr = ParseStr(wstr2, "::")    'Suffifx
                    If wstr.Length > wstr.IndexOf("=") Then Suffix.Text = wstr.Substring(wstr.IndexOf("=") + 1)
                    wstr = ParseStr(wstr2, "::")    'Full Name
                    wstr = ParseStr(wstr2, "::")    'Mail Address
                    wstr = ParseStr(wstr2, "::")    'Street Number
                    If wstr.Length > wstr.IndexOf("=") Then StNum.Text = wstr.Substring(wstr.IndexOf("=") + 1)
                    wstr = ParseStr(wstr2, "::")    'Street Name
                    If wstr.Length > wstr.IndexOf("=") Then StName.Text = wstr.Substring(wstr.IndexOf("=") + 1)
                    wstr = ParseStr(wstr2, "::")    'Unit
                    If wstr.Length > wstr.IndexOf("=") Then StUnit.Text = wstr.Substring(wstr.IndexOf("=") + 1)
                    wstr = ParseStr(wstr2, "::")    'Full Address
                    wstr = ParseStr(wstr2, "::")    'City
                    If wstr.Length > wstr.IndexOf("=") Then StCity.Text = wstr.Substring(wstr.IndexOf("=") + 1)
                    wstr = ParseStr(wstr2, "::")    'State
                    If wstr.Length > wstr.IndexOf("=") Then StState.Text = wstr.Substring(wstr.IndexOf("=") + 1)
                    wstr = ParseStr(wstr2, "::")    'Zip
                    If wstr.Length > wstr.IndexOf("=") Then StZip.Text = wstr.Substring(wstr.IndexOf("=") + 1)
                    wstr = ParseStr(wstr2, "::")    'Country
                    If wstr.Length > wstr.IndexOf("=") Then StCountry.Text = wstr.Substring(wstr.IndexOf("=") + 1)
                    wstr = ParseStr(wstr2, "::")    'Email
                    If wstr.Length > wstr.IndexOf("=") Then EmailAddress.Text = wstr.Substring(wstr.IndexOf("=") + 1)
                    wstr = ParseStr(wstr2, "::")    'Phone
                    If wstr.Length > wstr.IndexOf("=") Then PhoneNo.Text = wstr.Substring(wstr.IndexOf("=") + 1)
                    wstr = ParseStr(wstr2, "::")    'Question 1
                    If wstr.Length > wstr.IndexOf("=") Then Quest1.Text = wstr.Substring(wstr.IndexOf("=") + 1)
                    wstr = ParseStr(wstr2, "::")    'Answer 1
                    If wstr.Length > wstr.IndexOf("=") Then Answer1.Text = wstr.Substring(wstr.IndexOf("=") + 1)
                    wstr = ParseStr(wstr2, "::")    'Question 2
                    If wstr.Length > wstr.IndexOf("=") Then Quest2.Text = wstr.Substring(wstr.IndexOf("=") + 1)
                    wstr = ParseStr(wstr2, "::")    'Answer 2
                    If wstr.Length > wstr.IndexOf("=") Then Answer2.Text = wstr.Substring(wstr.IndexOf("=") + 1)
                    wstr = ParseStr(wstr2, "::")    'Agency Name
                    If wstr.Length > wstr.IndexOf("=") Then AgencyName.Text = wstr.Substring(wstr.IndexOf("=") + 1)

                    If Session("Relationship") IsNot Nothing AndAlso Session("Relationship").ToString.ToUpper = "OWNER" Then
                        Relationship.Text = "Owner"
                        OwnerGrp.Visible = True
                        AgencyGrp.Visible = False
                    Else
                        Relationship.Text = "Agent"
                        OwnerGrp.Visible = False
                        AgencyGrp.Visible = True
                    End If
                Else
                    UserIDCode2.Text = ""
                    BillCode2.Text = ""
                    FirstName.Text = ""
                    MidName.Text = ""
                    LastName.Text = ""
                    Suffix.Text = ""
                    StNum.Text = ""
                    StName.Text = ""
                    StUnit.Text = ""
                    StCity.Text = ""
                    StState.Text = ""
                    StZip.Text = ""
                    StCountry.Text = ""
                    EmailAddress.Text = ""
                    PhoneNo.Text = ""
                    Quest1.Text = ""
                    Answer1.Text = ""
                    Quest2.Text = ""
                    Answer2.Text = ""
                    AgencyName.Text = ""
                    OwnerGrp.Visible = True
                    AgencyGrp.Visible = False
                End If
            Else
                UserIDCode2.Text = ""
                BillCode2.Text = ""
                FirstName.Text = ""
                MidName.Text = ""
                LastName.Text = ""
                Suffix.Text = ""
                StNum.Text = ""
                StName.Text = ""
                StUnit.Text = ""
                StCity.Text = ""
                StState.Text = ""
                StZip.Text = ""
                StCountry.Text = ""
                EmailAddress.Text = ""
                PhoneNo.Text = ""
                Quest1.Text = ""
                Answer1.Text = ""
                Quest2.Text = ""
                Answer2.Text = ""
                AgencyName.Text = ""
                OwnerGrp.Visible = True
                AgencyGrp.Visible = False
            End If
        End If
    End Sub

    Protected Sub UpdateProfile_Click(sender As Object, e As EventArgs)
        Dim tXML As String = ""
        Dim tsep As String = "::"

        'Owner / Agent edits
        If Session("Relationship").ToString.ToUpper = "OWNER" Then
            If FirstName.Text.Length < 1 OrElse LastName.Text.Length < 1 Then
                ShowDialogOK("When Relationship is Owner, first and last name must be entered.")
                Return
            End If
        Else
            If AgencyName.Text.Length < 1 Then
                ShowDialogOK("When Relationship is Agent, agency name must be entered.")
                Return
            End If
        End If

        'Update User Profile - build XML to pass to update
        tXML = UserIDCode2.Text & tsep & BillCode2.Text & tsep & FirstName.Text & tsep & MidName.Text & tsep &
               LastName.Text & tsep

        If Suffix.SelectedIndex > 0 Then
            tXML += Suffix.Text & tsep
        Else
            tXML += " " & tsep
        End If

        'Name Display - leave blank for now
        tXML += " " & tsep & StNum.Text & tsep & StName.Text & tsep & StUnit.Text & tsep
        tXML += StNum.Text & " " & StName.Text & " " & StUnit.Text & tsep                'Full Address without city/st/zip/country
        tXML += StCity.Text & tsep

        If StState.SelectedIndex > -1 Then
            tXML += StState.Text & tsep
        Else
            tXML += " " & tsep
        End If

        tXML += StZip.Text & tsep & StCountry.Text & tsep & EmailAddress.Text & tsep & PhoneNo.Text &
                tsep & Quest1.Text & tsep & Answer1.Text & tsep & Quest2.Text & tsep & Answer2.Text & tsep & AgencyName.Text


        If UpdateProfile_Soap(tXML) = False Then
            ShowDialogOK("Error: Problem updating user profile.", "Profile Update")
            'FailureText.Text = "Error: Problem updating user profile. " & iErrMsg
            'ErrorMessage.Visible = True
            Return
            'MsgBox("Error: Problem updating user profile.", MsgBoxStyle.OkOnly, "Profile Update")
        End If

        Response.Redirect("~\Account\ProfileList.aspx", False)
    End Sub

    Protected Sub CancelEdit_Click(sender As Object, e As EventArgs)
        Response.Redirect("~\Account\ProfileList.aspx", False)
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