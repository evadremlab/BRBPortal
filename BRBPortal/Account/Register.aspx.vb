Imports System
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.EntityFramework
Imports Microsoft.AspNet.Identity.Owin
Imports Owin
Imports QSILib

Partial Public Class Register
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        ErrorMsg.Visible = False
        FailureText.Text = ""

        If Not IsPostBack Then
            PropRelate.SelectedValue = "Owner"
            NameGrp.Visible = True
            AgencyGrp.Visible = False
        End If

    End Sub

    Protected Sub RegisterUser_Click(sender As Object, e As EventArgs)
        Dim SoapStr As String

        If PropRelate.SelectedValue.ToString = "Agent" Then
            If AgencyName.Text = "" Then
                ShowDialogOK("Agency name must be entered when property relationship is Agent.")
                Return
            End If
        End If

        If PropRelate.SelectedValue.ToString = "Owner" Then
            If FirstName.Text = "" OrElse LastName.Text = "" Then
                ShowDialogOK("First and Last name must be entered when property relationship is Owner.")
                Return
            End If
        End If

        SoapStr = "<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:api=""http://cityofberkeley.info/RTS/ClientPortal/API"">"
        SoapStr += "<soapenv:Header/>"
        SoapStr += "<soapenv:Body>"
        SoapStr += "<api:validateRegistrationRequest>"
        SoapStr += "<registrationRequestReq>"
        SoapStr += "<profileDetails>"
        SoapStr += "<userId>" & ReqUserID.Text & "</userId>"
        SoapStr += "<billingCode>" & BillCode.Text & "</billingCode>"
        SoapStr += "<name>"
        SoapStr += "<first>" & ChgXMLChars(FirstName.Text) & "</first>"
        SoapStr += "<!--Optional:--><middle>" & ChgXMLChars(MidName.Text) & "</middle>"
        SoapStr += "<last>" & ChgXMLChars(LastName.Text) & "</last>"

        If Suffix.SelectedIndex < 1 Then
            SoapStr += "<suffix></suffix>"
        Else
            SoapStr += "<suffix>" & Suffix.Text & "</suffix>"
        End If

        SoapStr += "<!--Optional:--><nameLastFirstDisplay></nameLastFirstDisplay>"
        SoapStr += "<!--Optional:--><agencyName>" & ChgXMLChars(AgencyName.Text) & "</agencyName>"
        SoapStr += "</name>"
        SoapStr += "<mailingAddress>"
        SoapStr += "<!--Optional:--><streetNumber>" & StNum.Text & "</streetNumber>"
        SoapStr += "<!--Optional:--><streetName>" & ChgXMLChars(StName.Text) & "</streetName>"
        SoapStr += "<!--Optional:--><unitNumber>" & StUnit.Text & "</unitNumber>"
        SoapStr += "<!--Optional:--><fullAddress></fullAddress>"
        SoapStr += "<!--Optional:--><city>" & StCity.Text & "</city>"
        SoapStr += "<!--Optional:--><state>" & StState.Text & "</state>"
        SoapStr += "<!--Optional:--><zip>" & StZip.Text & "</zip>"
        SoapStr += "<!--Optional:--><country>" & StCountry.Text & "</country>"
        SoapStr += "</mailingAddress>"
        SoapStr += "<emailAddress>" & EmailAddress.Text & "</emailAddress>"
        SoapStr += "<phone>" & PhoneNo.Text & "</phone>"
        SoapStr += "<securityQuestion1>" & Quest1.Text & "</securityQuestion1>"
        SoapStr += "<securityAnswer1>" & Answer1.Text & "</securityAnswer1>"
        SoapStr += "<securityQuestion2>" & Quest2.Text & "</securityQuestion2>"
        SoapStr += "<securityAnswer2>" & Answer2.Text & "</securityAnswer2>"
        SoapStr += "</profileDetails>"
        SoapStr += "<propertyDetails>"
        SoapStr += "<relationship>" & PropRelate.SelectedValue.ToString & "</relationship>"
        SoapStr += "<ownerLastName>" & PropOwnLastName.Text & "</ownerLastName>"
        SoapStr += "<address>" & PropAddress.Text & "</address>"
        SoapStr += "<purchaseYear>" & PurchaseYear.Text & "</purchaseYear>"
        SoapStr += "</propertyDetails>"
        SoapStr += "</registrationRequestReq>"
        SoapStr += "</api:validateRegistrationRequest>"
        SoapStr += "</soapenv:Body>"
        SoapStr += "</soapenv:Envelope>"

        If Register_Soap(SoapStr) = False Then
            If iErrMsg.IndexOf("(500) Internal Server Error") > -1 Then iErrMsg = "(500) Internal Server Error."
            ShowDialogOK("Error registering request." & iErrMsg, "Register User")
            Return
        End If

        Response.Redirect("~/Account/Login.aspx", False)

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

    Protected Sub PropRelate_SelectedIndexChanged(sender As Object, e As EventArgs)

        If PropRelate.SelectedValue = "Owner" Then
            NameGrp.Visible = True
            AgencyGrp.Visible = False
            AgencyName.Text = ""
        Else
            NameGrp.Visible = False
            AgencyGrp.Visible = True
            FirstName.Text = ""
            MidName.Text = ""
            LastName.Text = ""
            Suffix.Text = ""
            Suffix.SelectedIndex = 0
        End If

    End Sub

End Class

