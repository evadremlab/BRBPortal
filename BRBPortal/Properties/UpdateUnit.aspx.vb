Imports QSILib

Public Class UpdateUnit
    Inherits System.Web.UI.Page
    Private iPropertyAddress As String = ""
    Private iUnitNum As String = ""
    Private iUnitID As String = ""
    Private iPropertyNo As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim wstr, wstr2, tUnitInfo As String

        If Session("UserCode") Is Nothing OrElse Session("BillingCode") Is Nothing OrElse
            Session("UserCode").ToString = "" OrElse Session("BillingCode").ToString = "" Then
            Response.Redirect("..\Account\Login", False)
            Return
        End If

        If Not IsPostBack Then
            iPropertyNo = Session("PropertyID").ToString
            iPropertyAddress = Session("PropAddr").ToString
            iUnitNum = Session("UnitNo").ToString
            iUnitID = Session("UnitID").ToString

            wstr = Session("UserCode").ToString
            wstr2 = Session("BillingCode").ToString

            tUnitInfo = GetPropertyUnits_Soap(iPropertyNo, wstr, wstr2, iUnitID)
            If tUnitInfo = "FAILURE" Then
                ShowDialogOK("Error: Error retrieving Unit ID " & iUnitID & " Unit No " & iUnitNum & ".", "Update Units")
                Return
            End If

            'Parse tUnitInfo for the header information
            wstr2 = tUnitInfo
            wstr = ParseStr(wstr2, "::")        'Rented or Exempt
            If wstr.Length > wstr.IndexOf("=") Then UnitStatus.Text = wstr.Substring(wstr.IndexOf("=") + 1)
            wstr = ParseStr(wstr2, "::")        'Exemption Reason
            If wstr.Length > wstr.IndexOf("=") Then ExemptReas.Text = wstr.Substring(wstr.IndexOf("=") + 1)
            wstr = ParseStr(wstr2, "::")        'Start Date
            If wstr.Length > wstr.IndexOf("=") Then UnitStartDt.Text = wstr.Substring(wstr.IndexOf("=") + 1)
            wstr = ParseStr(wstr2, "::")        'Occupied By
            If wstr.Length > wstr.IndexOf("=") Then UnitOccBy.Text = wstr.Substring(wstr.IndexOf("=") + 1)
            wstr = ParseStr(wstr2, "::")        'Unit ID
            If wstr.Length > wstr.IndexOf("=") Then hfUnitID.Value = wstr.Substring(wstr.IndexOf("=") + 1)

            If UnitStatus.Text = "Rented" Then
                ExemptReas.Text = ""
                ExemptReason.SelectedIndex = -1
                ExemptGroup.Visible = False
                CommUseGrp.Visible = False
                PMUnitGrp.Visible = False
                OwnerShrGrp.Visible = False
                AsOfDtGrp.Visible = False
                DtStrtdGrp.Visible = False
                OccByGrp.Visible = False
                ContractGrp.Visible = False

            Else
                ExemptReason.SelectedValue = ExemptReas.Text
                ExemptGroup.Visible = True
                CommUseGrp.Visible = False
                PMUnitGrp.Visible = False
                OwnerShrGrp.Visible = False
                AsOfDtGrp.Visible = False
                DtStrtdGrp.Visible = False
                OccByGrp.Visible = False
                ContractGrp.Visible = False

                Select Case ExemptReason.SelectedValue.ToString.ToUpper
                    Case "NAR"
                        ExemptGroup.Visible = True
                        CommUseGrp.Visible = False
                        PMUnitGrp.Visible = False
                        OwnerShrGrp.Visible = False
                        AsOfDtGrp.Visible = True
                        DtStrtdGrp.Visible = False
                        OccByGrp.Visible = False
                        ContractGrp.Visible = False
                        OtherList.Visible = False
                        OtherList.ClearSelection()

                    Case "OOCC"
                        ExemptGroup.Visible = True
                        CommUseGrp.Visible = False
                        PMUnitGrp.Visible = False
                        OwnerShrGrp.Visible = False
                        AsOfDtGrp.Visible = False
                        DtStrtdGrp.Visible = True
                        OccByGrp.Visible = True
                        ContractGrp.Visible = False
                        OtherList.Visible = False
                        OtherList.ClearSelection()

                    Case "SEC8"
                        ExemptGroup.Visible = True
                        CommUseGrp.Visible = False
                        PMUnitGrp.Visible = False
                        OwnerShrGrp.Visible = False
                        AsOfDtGrp.Visible = False
                        DtStrtdGrp.Visible = True
                        OccByGrp.Visible = False
                        ContractGrp.Visible = True
                        OtherList.Visible = False
                        OtherList.ClearSelection()

                    Case "FREE"
                        ExemptGroup.Visible = True
                        CommUseGrp.Visible = False
                        PMUnitGrp.Visible = False
                        OwnerShrGrp.Visible = False
                        AsOfDtGrp.Visible = False
                        DtStrtdGrp.Visible = True
                        OccByGrp.Visible = True
                        ContractGrp.Visible = False
                        OtherList.Visible = False
                        OtherList.ClearSelection()

                    Case "OTHER"
                        ExemptGroup.Visible = True
                        CommUseGrp.Visible = False
                        PMUnitGrp.Visible = False
                        OwnerShrGrp.Visible = False
                        AsOfDtGrp.Visible = False
                        DtStrtdGrp.Visible = False
                        OccByGrp.Visible = False
                        ContractGrp.Visible = False
                        OtherList.Visible = True
                        OtherList.ClearSelection()

                End Select
            End If

            MainAddress.Text = iPropertyAddress
            UnitNo.Text = iUnitNum
            NewUnit.SelectedValue = UnitStatus.Text

            FailureText.Text = ""
            ErrorMessage.Visible = True

        End If

    End Sub

    Protected Sub UpdateUnit_Click(sender As Object, e As EventArgs)
        Dim SaveXML, tExempt As String

        If DeclareInits.Text.Length < 1 Or chkDeclare.Checked = False Then
            ShowDialogOK("You must check the declaration box and enter your initials prior to submitting any changes.", "Update Unit")
            Return
        End If

        'Make sure certain fields are filled out correctly
        If NewUnit.SelectedValue.ToString.ToUpper = "EXEMPT" Then

            Select Case ExemptReason.SelectedValue.ToString.ToUpper
                Case "NAR"
                    If UnitAsOfDt.Text.Length < 1 Then
                        ShowDialogOK("As of Date must be entered.")
                        Return
                    End If

                Case "OOCC", "FREE"
                    If StartDt.Text.Length < 1 Then
                        ShowDialogOK("Date Started must be entered.")
                        Return
                    End If

                    If OccupiedBy.Text = "" Then
                        ShowDialogOK("Occupied By must be entered.")
                        Return
                    End If

                Case "SEC8"
                    If StartDt.Text.Length < 1 Then
                        ShowDialogOK("Date Started must be entered.")
                        Return
                    End If

                    If ContractNo.Text = "" Then
                        ShowDialogOK("Contract # must be entered.")
                        Return
                    End If

                Case "OTHER"
                    Select Case OtherList.SelectedValue.ToString.ToUpper
                        Case "COMM"
                            If StartDt.Text.Length < 1 Then
                                ShowDialogOK("Date Started must be entered.")
                                Return
                            End If

                            If CommUseDesc.Text = "" Then
                                ShowDialogOK("Description of the current commercial use must be entered.")
                                Return
                            End If

                            If RB1Y.Checked = True AndAlso CommZoneUse.Value.ToString = "" Then
                                ShowDialogOK("When zoned for commercial use is Yes the description must also be entered.")
                                Return
                            End If

                        Case "MISC"
                            If StartDt.Text.Length < 1 Then
                                ShowDialogOK("Date Started must be entered.")
                                Return
                            End If

                            If PropMgrName.Text = "" Then
                                ShowDialogOK("Property manager name must be entered.")
                                Return
                            End If

                            If PMEmailPhone.Text = "" Then
                                ShowDialogOK("Property manager email/phone must be entered.")
                                Return
                            End If

                        Case "SHARED"
                            If StartDt.Text.Length < 1 Then
                                ShowDialogOK("Date Started must be entered.")
                                Return
                            End If

                            If PrincResYN.SelectedValue.ToString = "Yes" Then

                            End If

                            If MultiUnitYN.SelectedValue.ToString = "Yes" AndAlso OtherUnits.Text = "" Then
                                ShowDialogOK("If the owner resides in more than one unit you must also state which units they reside in.")
                                Return
                            End If

                            If TenantNames.Text = "" OrElse TenantContacts.Text = "" Then
                                ShowDialogOK("Tenant name(s) and contact information must be entered.")
                                Return
                            End If

                        Case "SPLUS"
                            If StartDt.Text.Length < 1 Then
                                ShowDialogOK("Date Started must be entered.")
                                Return
                            End If

                            If ContractNo.Text = "" Then
                                ShowDialogOK("Contract # must be entered.")
                                Return
                            End If

                    End Select
            End Select

        End If

        'API_SaveUnit
        SaveXML = "<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:api=""http://cityofberkeley.info/RTS/ClientPortal/API"">"
        SaveXML += "<soapenv:Header/>"
        SaveXML += "<soapenv:Body>"
        SaveXML += "<api:updateUnitStatusChange>"
        SaveXML += "<unitStatusChangeReq>"
        SaveXML += "<userId>" & Session("UserCode").ToString & "</userId>"
        SaveXML += "<propertyId>" & Session("PropertyID").ToString & "</propertyId>"
        SaveXML += "<unitId>" & hfUnitID.Value.ToString & "</unitId>"
        SaveXML += "<clientPortalUnitStatusCode>" & NewUnit.SelectedValue.ToString & "</clientPortalUnitStatusCode>"
        SaveXML += "<unitStatus>" & NewUnit.SelectedValue.ToString & "</unitStatus>"

        tExempt = ""
        If NewUnit.SelectedValue.ToString = "Exempt" Then
            If ExemptReason.SelectedValue.ToString.ToUpper = "OTHER" Then
                tExempt = OtherList.SelectedValue.ToString.ToUpper
            Else
                tExempt = ExemptReason.SelectedValue.ToString.ToUpper
            End If
        End If

        SaveXML += "<!--Optional:--><exemptionReason>" & tExempt & "</exemptionReason>"
        If UnitAsOfDt.Text.Length > 0 Then
            SaveXML += "<unitStatusAsOfDate>" & CDate(UnitAsOfDt.Text).ToString("MM/dd/yyyy") & "</unitStatusAsOfDate>"
        Else
            SaveXML += "<unitStatusAsOfDate>" & Today.ToString("MM/dd/yyyy") & "</unitStatusAsOfDate>"
        End If

        SaveXML += "<declarationInitial>" & DeclareInits.Text & "</declarationInitial>"
        SaveXML += "<questions>"

        If UnitAsOfDt.Text.Length > 0 Then
            SaveXML += "<!--Optional:--><asOfDate>" & CDate(UnitAsOfDt.Text).ToString("MM/dd/yyyy") & "</asOfDate>"
        Else
            SaveXML += "<!--Optional:--><asOfDate></asOfDate>"
        End If

        If StartDt.Text.Length > 0 Then
            SaveXML += "<!--Optional:--><dateStarted>" & CDate(StartDt.Text).ToString("MM/dd/yyyy") & "</dateStarted>"
        Else
            SaveXML += "<!--Optional:--><dateStarted></dateStarted>"
        End If
        SaveXML += "<!--Optional:--><occupiedBy>" & ChgXMLChars(OccupiedBy.Text) & "</occupiedBy>"
        SaveXML += "<!--Optional:--><contractNo>" & ContractNo.Text & "</contractNo>"
        SaveXML += "<!--Optional:--><commeUseDesc>" & ChgXMLChars(CommUseDesc.Text) & "</commeUseDesc>"
        SaveXML += "<!--Optional:--><isCommeUseZoned>" & ChgXMLChars(CommZoneUse.Value.ToString) & "</isCommeUseZoned>"
        SaveXML += "<!--Optional:--><isExclusivelyForCommeUse>" & CommResYN.SelectedValue.ToString & "</isExclusivelyForCommeUse>"

        'Next 3 removed  when Owner Occupied Exempt Duplex was removed from the Other dropdown
        SaveXML += "<!--Optional:--><_x0035_0PercentAsOf31Dec1979></_x0035_0PercentAsOf31Dec1979>"
        SaveXML += "<!--Optional:--><ownerOccupantName></ownerOccupantName>"
        SaveXML += "<!--Zero or more repetitions:--><namesOfownersOfRecord></namesOfownersOfRecord>"

        SaveXML += "<!--Optional:--><nameOfPropertyManagerResiding>" & PropMgrName.Text & "</nameOfPropertyManagerResiding>"
        SaveXML += "<!--Optional:--><emailOfPhoneOfPropertyManagerResiding>" & PMEmailPhone.Text & "</emailOfPhoneOfPropertyManagerResiding>"
        SaveXML += "<!--Optional:--><IsOwnersPrinciplePlaceOfResidence>" & PrincResYN.SelectedValue.ToString & "</IsOwnersPrinciplePlaceOfResidence>"
        SaveXML += "<!--Optional:--><doesOwnerResideInOtherUnitOfThisUnitProperty>" & MultiUnitYN.SelectedValue.ToString & "</doesOwnerResideInOtherUnitOfThisUnitProperty>"
        SaveXML += "<!--Zero or more repetitions:--><tenantsAndContactInfo>"
        SaveXML += "<name>" & ChgXMLChars(TenantNames.Text) & "</name>"
        SaveXML += "<contactInfo>" & ChgXMLChars(TenantContacts.Text) & "</contactInfo>"
        SaveXML += "</tenantsAndContactInfo>"

        SaveXML += "</questions>"
        SaveXML += "</unitStatusChangeReq>"
        SaveXML += "</api:updateUnitStatusChange>"
        SaveXML += "</soapenv:Body>"
        SaveXML += "</soapenv:Envelope>"

        If SaveUnit_Soap(SaveXML) = False Then
            If iErrMsg.IndexOf("(500) Internal Server Error") > -1 Then iErrMsg = "(500) Internal Server Error."
            ShowDialogOK("Error: Problem saving unit to RTS. " & iErrMsg)
            Return
        End If

        If Session("UpdTenants") = True Then
            Response.Redirect("~\Properties\UpdateTenancy.aspx", False)
        Else
            Response.Redirect("~\Properties\MyUnits.aspx", False)
        End If

    End Sub

    Protected Sub CancelEdit_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Response.Redirect("~\Properties\MyUnits.aspx", False)
    End Sub

    Protected Sub RB1_Clicked(sender As Object, e As EventArgs)

        If RB1N.Checked = True Then CommZoneUse.Value = ""

    End Sub

    Protected Sub NewUnit_Clicked(sender As Object, e As EventArgs)

        If NewUnit.SelectedValue = "Rented" Then
            ExemptGroup.Visible = False
            ExemptReason.Visible = False
            OtherList.Visible = False
            ExemptReason.ClearSelection()
            OtherList.ClearSelection()
            AsOfDtGrp.Visible = True
        Else
            ExemptGroup.Visible = True
            ExemptReason.Visible = True
            OtherList.Visible = True
            ExemptReason.ClearSelection()
            OtherList.ClearSelection()
            AsOfDtGrp.Visible = False
        End If
    End Sub

    Protected Sub Exemption_Clicked(sender As Object, e As EventArgs)

        Select Case ExemptReason.SelectedValue.ToString.ToUpper
            Case "NAR"
                ExemptGroup.Visible = True
                CommUseGrp.Visible = False
                PMUnitGrp.Visible = False
                OwnerShrGrp.Visible = False
                AsOfDtGrp.Visible = True
                DtStrtdGrp.Visible = False
                OccByGrp.Visible = False
                ContractGrp.Visible = False
                OtherList.Visible = False
                OtherList.ClearSelection()

            Case "OOCC"
                ExemptGroup.Visible = True
                CommUseGrp.Visible = False
                PMUnitGrp.Visible = False
                OwnerShrGrp.Visible = False
                AsOfDtGrp.Visible = False
                DtStrtdGrp.Visible = True
                OccByGrp.Visible = True
                ContractGrp.Visible = False
                OtherList.Visible = False
                OtherList.ClearSelection()

            Case "SEC8"
                ExemptGroup.Visible = True
                CommUseGrp.Visible = False
                PMUnitGrp.Visible = False
                OwnerShrGrp.Visible = False
                AsOfDtGrp.Visible = False
                DtStrtdGrp.Visible = True
                OccByGrp.Visible = False
                ContractGrp.Visible = True
                OtherList.Visible = False
                OtherList.ClearSelection()

            Case "FREE"
                ExemptGroup.Visible = True
                CommUseGrp.Visible = False
                PMUnitGrp.Visible = False
                OwnerShrGrp.Visible = False
                AsOfDtGrp.Visible = False
                DtStrtdGrp.Visible = True
                OccByGrp.Visible = True
                ContractGrp.Visible = False
                OtherList.Visible = False
                OtherList.ClearSelection()

            Case "OTHER"
                ExemptGroup.Visible = True
                CommUseGrp.Visible = False
                PMUnitGrp.Visible = False
                OwnerShrGrp.Visible = False
                AsOfDtGrp.Visible = False
                DtStrtdGrp.Visible = False
                OccByGrp.Visible = False
                ContractGrp.Visible = False
                OtherList.Visible = True
                OtherList.ClearSelection()

        End Select

    End Sub

    Protected Sub OtherList_Clicked(sender As Object, e As EventArgs)

        Select Case OtherList.SelectedValue.ToString.ToUpper
            Case "COMM"    'Commercial Use
                CommUseGrp.Visible = True
                DtStrtdGrp.Visible = True
                ContractGrp.Visible = False
                PMUnitGrp.Visible = False
                OwnerShrGrp.Visible = False

            Case "MISC"    'Property Managers Unit
                CommUseGrp.Visible = False
                DtStrtdGrp.Visible = True
                ContractGrp.Visible = False
                PMUnitGrp.Visible = True
                OwnerShrGrp.Visible = False

            Case "SHARED"    'Owner share kitchen & bath with tenant
                CommUseGrp.Visible = False
                DtStrtdGrp.Visible = True
                ContractGrp.Visible = False
                PMUnitGrp.Visible = False
                OwnerShrGrp.Visible = True

            Case "SPLUS"    'Shelter plus care
                CommUseGrp.Visible = False
                DtStrtdGrp.Visible = True
                ContractGrp.Visible = True
                PMUnitGrp.Visible = False
                OwnerShrGrp.Visible = False

        End Select

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