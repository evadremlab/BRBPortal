Imports QSILib

Public Class UpdateTenancy
    Inherits System.Web.UI.Page
    Private iPropertyAddress As String = ""
    Private iUnitNum As String = ""
    Private iUnitID As String = ""
    Private iPropertyNo As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim wstr, wstr2, wstr3, tUnitInfo As String

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

            tUnitInfo = GetPropertyTenants_Soap(iPropertyNo, wstr, wstr2, iUnitID)
            If tUnitInfo = "FAILURE" Then
                ShowDialogOK("Error: Error retrieving Unit " & iUnitNum & ".", "Update Tenancy")
                Return
            End If

            'Parse tUnitInfo for the header information
            wstr2 = tUnitInfo
            wstr = ParseStr(wstr2, "::")            'Rented or Exempt
            If wstr.Length > wstr.IndexOf("=") Then UnitStatus.Text = wstr.Substring(wstr.IndexOf("=") + 1)

            wstr = ParseStr(wstr2, "::")            'Housing Services
            If wstr.Length > wstr.IndexOf("=") Then
                Dim tOther As Boolean = False

                wstr = wstr.Substring(wstr.IndexOf("=") + 1)
                Do While wstr.Length > 0
                    wstr3 = ParseStr(wstr, ", ")
                    For i = 0 To 8
                        If HServs.Items(i).Text = wstr3 Then
                            HServs.Items(i).Selected = True
                        End If
                        If wstr3.ToUpper = "OTHER" Then tOther = True
                    Next
                Loop
                If tOther = False Then
                    HServOthrBox.Enabled = False
                Else
                    HServOthrBox.Enabled = True
                End If
            End If

            wstr = ParseStr(wstr2, "::")        'Start Date
            If wstr.Length > wstr.IndexOf("=") Then
                TenStDt.Text = wstr.Substring(wstr.IndexOf("=") + 1)
                hfOrigTenStDt.Value = wstr.Substring(wstr.IndexOf("=") + 1)
            Else
                hfOrigTenStDt.Value = ""
            End If

            wstr = ParseStr(wstr2, "::")        'Number of Tenants
            If wstr.Length > wstr.IndexOf("=") Then NumTenants.Text = wstr.Substring(wstr.IndexOf("=") + 1)
            wstr = ParseStr(wstr2, "::")        'Smoking Y/N
            If wstr.Length > wstr.IndexOf("=") Then
                If wstr.Substring(wstr.IndexOf("=") + 1) = "Y" Then
                    RB1Y.Checked = True
                    RB1N.Checked = False
                Else
                    RB1N.Checked = True
                    RB1Y.Checked = False
                End If
            End If
            wstr = ParseStr(wstr2, "::")        'Smoking Start Date
            If wstr.Length > wstr.IndexOf("=") Then SmokeDt.Text = wstr.Substring(wstr.IndexOf("=") + 1)
            wstr = ParseStr(wstr2, "::")        'Initial Rent
            If wstr.Length > wstr.IndexOf("=") Then InitRent.Text = wstr.Substring(wstr.IndexOf("=") + 1)
            wstr = ParseStr(wstr2, "::")        'Prior Tenancy End Date
            If wstr.Length > wstr.IndexOf("=") Then PTenDt.Text = wstr.Substring(wstr.IndexOf("=") + 1)
            wstr = ParseStr(wstr2, "::")        'Termination reason
            If wstr.Length > wstr.IndexOf("=") Then TermReas.Text = wstr.Substring(wstr.IndexOf("=") + 1)
            wstr = ParseStr(wstr2, "::")        'Billing (Owner) Contact
            If wstr.Length > wstr.IndexOf("=") Then OwnerName.Text = wstr.Substring(wstr.IndexOf("=") + 1)
            wstr = ParseStr(wstr2, "::")        'Agent Name
            If wstr.Length > wstr.IndexOf("=") Then AgentName.Text = wstr.Substring(wstr.IndexOf("=") + 1)
            wstr = ParseStr(wstr2, "::")        'Unit ID
            If wstr.Length > wstr.IndexOf("=") Then
                hfUnitID.Value = wstr.Substring(wstr.IndexOf("=") + 1)
            Else
                hfUnitID.Value = ""
            End If
            wstr = ParseStr(wstr2, "::")        'Billing (Owner) Email
            If wstr.Length > wstr.IndexOf("=") Then
                hfOwnerEmail.Value = wstr.Substring(wstr.IndexOf("=") + 1)
            Else
                hfOwnerEmail.Value = ""
            End If

            MainAddress.Text = iPropertyAddress
            UnitNo.Text = iUnitNum
            BalAmt.Text = Session("PropBalance").ToString

            iTenantsTbl.DefaultView.Sort = "LastName, FirstName ASC"
            iTenantsTbl = iTenantsTbl.DefaultView.ToTable

            'Save the original tenants to compare against when they save
            Session("OrigTenantsTbl") = iTenantsTbl
            iTenantsTbl.Clear()

            gvTenants.DataSource = iTenantsTbl
            gvTenants.DataBind()

            Session("TenantsTbl") = iTenantsTbl
            'Session("OrigTenantsTbl") = iTenantsTbl

            AddTenant.Visible = False
        End If

    End Sub

    Protected Sub CancelEdit_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Response.Redirect("~\Properties\MyTenants.aspx", False)
    End Sub

    Protected Sub AddTenant_Click(sender As Object, e As EventArgs) Handles btnAddTenant.Click
        AddTenant.Visible = True
    End Sub

    Protected Sub UpdateTenancy_Click(sender As Object, e As EventArgs) Handles btnUpdTen.Click
        Dim SaveXML, wstr As String
        Dim FoundEmail, TenantsChgd As Boolean

        If chkDeclare.Checked = False OrElse DeclareInits.Text.Length < 1 Then
            ShowDialogOK("You must check the declaration box and enter your initials prior to submitting any changes.", "Update Tenancy")
            Return
        End If

        'Check rules to see if we have what we need before trying to update the Tenant information
        If gvTenants.Rows.Count < 1 Then
            ShowDialogOK("Please enter at least one tenant.")
            Return
        End If

        'Check that there is at least one email and that it is different than the Owner's
        FoundEmail = False

        For Each R As GridViewRow In gvTenants.Rows
            If R.Cells(5).Text.Length > 0 Then FoundEmail = True
            'If R.DataItem("EmailAddr").ToString.Length > 0 Then FoundEmail = True

            'Also check email is different than owner email
            If hfOwnerEmail.Value.Length > 0 AndAlso R.Cells(5).Text = hfOwnerEmail.Value.ToString Then
                ShowDialogOK("Tenant email cannot be the same as the Owner's email address.")
                Return
            End If
        Next

        If FoundEmail = False Then
            ShowDialogOK("At least one Tenant must have an email address.")
            Return
        End If

        'Check that the new tenants are different than the old tenants
        Dim tOrigTenants As DataTable = Session("OrigTenantsTbl")
        TenantsChgd = True

        For Each NewR As DataRow In iTenantsTbl.Rows
            For Each OrigR As DataRow In tOrigTenants.Rows
                If NewR.Item("FirstName").ToString = OrigR.Item("FirstName").ToString OrElse
                   NewR.Item("LastName").ToString = OrigR.Item("LastName").ToString OrElse
                   NewR.Item("PhoneNo").ToString = OrigR.Item("PhoneNo").ToString OrElse
                   NewR.Item("EmailAddr").ToString = OrigR.Item("EmailAddr").ToString Then
                    TenantsChgd = False
                End If
            Next
        Next

        If TenantsChgd = False Then
            ShowDialogOK("New tenants may not be the same as the original tenants.")
            Return
        End If

        If TenStDt.Text.Length < 1 OrElse TenStDt.Text = "" Then
            ShowDialogOK("Tenant start date must be entered.")
            Return
        Else
            If hfOrigTenStDt.Value > "" AndAlso CDate(hfOrigTenStDt.Value.ToString) > CDate(TenStDt.Text) Then
                ShowDialogOK("Tenant start date must be after the existing start date.")
                Return
            End If
        End If

        If RB1Y.Checked Then
            If SmokeDt.Text.Length < 1 OrElse SmokeDt.Text = "" Then
                ShowDialogOK("Smoking effective date must be entered when smoking prohibited is Yes.")
                Return
            End If

            If CDate(TenStDt.Text) > CDate(SmokeDt.Text) Then
                ShowDialogOK("Smoking prohibition date must be on or after Tenancy start date.")
                Return
            End If
        End If

        If HServsOthr.Checked Then
            If HServOthrBox.Text.Length < 1 Then
                ShowDialogOK("When the Other exemption is selected you must select an other option.")
                Return
            End If
        End If

        'Build soap string
        SaveXML = "<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:api=""http://cityofberkeley.info/RTS/ClientPortal/API"" >"
        SaveXML += "<soapenv:Header/>"
        SaveXML += "<soapenv:Body>"
        SaveXML += "<api:updateUnitTenancy>"
        SaveXML += "<unitTenancyUpdateReq>"
        SaveXML += "<userId>" & Session("UserCode").ToString & "</userId>"
        SaveXML += "<propertyId>" & Session("PropertyID").ToString & "</propertyId>"
        SaveXML += "<unitId>" & hfUnitID.Value.ToString & "</unitId>"
        SaveXML += "<unitStatus>" & UnitStatus.Text & "</unitStatus>"
        SaveXML += "<initialRent>" & InitRent.Text & "</initialRent>"

        If TenStDt.Text.Length > 0 Then
            wstr = CDate(TenStDt.Text).ToString("MM/dd/yyyy")
            SaveXML += "<tenancyStartDate>" & wstr & "</tenancyStartDate>"
        Else
            SaveXML += "<tenancyStartDate></tenancyStartDate>"
        End If

        If PTenDt.Text.Length > 0 Then
            wstr = CDate(PTenDt.Text).ToString("MM/dd/yyyy")
            SaveXML += "<priorTenancyEndDate>" & wstr & "</priorTenancyEndDate>"
        Else
            SaveXML += "<priorTenancyEndDate></priorTenancyEndDate>"
        End If

        SaveXML += "<!--Zero or more repetitions:-->"

        For i = 0 To 8
            If HServs.Items(i).Selected = True Then
                SaveXML += "<housingServices>"
                SaveXML += "<serviceName>" & HServs.Items(i).Text & "</serviceName>"
                SaveXML += "</housingServices>"
            End If
        Next

        SaveXML += "<!--Optional:-->"
        SaveXML += "<otherHousingService>" & HServOthrBox.Text & "</otherHousingService>"
        SaveXML += "<noOfTenants>" & NumTenants.Text & "</noOfTenants>"

        If RB1Y.Checked = True Then
            SaveXML += "<smokingProhibitionInLeaseStatus>true</smokingProhibitionInLeaseStatus>"
            wstr = CDate(SmokeDt.Text).ToString("MM/dd/yyyy")
            SaveXML += "<smokingProhibitionEffectiveDate>" & wstr & "</smokingProhibitionEffectiveDate>"
        Else
            SaveXML += "<smokingProhibitionInLeaseStatus>false</smokingProhibitionInLeaseStatus>"
            SaveXML += "<smokingProhibitionEffectiveDate></smokingProhibitionEffectiveDate>"
        End If

        SaveXML += "<reasonForTermination>" & TermReas.SelectedItem.ToString & "</reasonForTermination>"
        SaveXML += "<otherReasonForTermination>" & ChgXMLChars(TermDescr.Text) & "</otherReasonForTermination>"
        SaveXML += "<!--Optional:-->"
        SaveXML += "<explainInvoluntaryTermination>" & ChgXMLChars(TermDescr.Text) & "</explainInvoluntaryTermination>"
        SaveXML += "<!--1 or more repetitions:-->"

        For Each R As DataRow In iTenantsTbl.Rows
            SaveXML += "<tenants>"
            SaveXML += "<code></code>"
            SaveXML += "<name>"
            SaveXML += "<first>" & ChgXMLChars(R.Item("FirstName").ToString) & "</first>"
            SaveXML += "<!--Optional:--><middle></middle>"
            SaveXML += "<last>" & ChgXMLChars(R.Item("LastName").ToString) & "</last>"
            SaveXML += "<suffix></suffix>"
            SaveXML += "<!--Optional:--><nameLastFirstDisplay></nameLastFirstDisplay>"
            SaveXML += "<!--Optional:--><agencyName></agencyName>"
            SaveXML += "</name>"
            SaveXML += "<phoneNumber>" & R.Item("PhoneNo").ToString & "</phoneNumber>"
            SaveXML += "<emailAddress>" & R.Item("EmailAddr").ToString & "</emailAddress>"
            SaveXML += "</tenants>"
        Next

        SaveXML += "<declarationInitial>" & DeclareInits.Text & "</declarationInitial>"
        SaveXML += "</unitTenancyUpdateReq>"
        SaveXML += "</api:updateUnitTenancy>"
        SaveXML += "</soapenv:Body>"
        SaveXML += "<soapenv:Envelope>"

        'Update the tenant
        If SaveTenant_Soap(SaveXML) = False Then
            If iErrMsg.IndexOf("(500) Internal Server Error") > -1 Then iErrMsg = "(500) Internal Server Error."
            If iErrMsg.IndexOf("Unable to invoke adapter service") > -1 Then iErrMsg = "Unable to invoke adapter service."
            ShowDialogOK("Error saving tenants information. " & iErrMsg, "Update Tenancy")
            Return
        End If

        Response.Redirect("~\Properties\MyTenants.aspx", False)

    End Sub

    Sub gvTenants_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)

        gvTenants.PageIndex = e.NewPageIndex
        gvTenants.DataSource = Session("TenantsTbl")
        gvTenants.DataBind()
    End Sub

    Protected Sub HServsOthr_CheckedChanged(sender As Object, e As EventArgs)

        If HServsOthr.Checked = True Then
            HServOthrBox.Enabled = True
        Else
            HServOthrBox.Enabled = False
        End If
    End Sub

    Protected Sub SaveNewTenant_Click(sender As Object, e As EventArgs) Handles SaveNewTen.Click
        'Dim tTenID, tFName, tMid, tLName, tDispName, tPhone, tEmail As String

        'Build out the table if needed
        If iTenantsTbl.Columns.Count < 1 Then
            iTenantsTbl.Columns.Add("TenantID", GetType(String))
            iTenantsTbl.Columns.Add("FirstName", GetType(String))
            iTenantsTbl.Columns.Add("LastName", GetType(String))
            iTenantsTbl.Columns.Add("DispName", GetType(String))
            iTenantsTbl.Columns.Add("PhoneNo", GetType(String))
            iTenantsTbl.Columns.Add("EmailAddr", GetType(String))
        End If

        Dim NewTenants As DataTable = iTenantsTbl.Clone()

        For Each R As DataRow In iTenantsTbl.Rows
            Dim tRow As DataRow = NewTenants.NewRow()

            tRow.Item("TenantID") = R.Item("TenantID").ToString
            tRow.Item("FirstName") = R.Item("FirstName").ToString
            tRow.Item("LastName") = R.Item("LastName").ToString
            tRow.Item("DispName") = R.Item("DispName").ToString
            tRow.Item("PhoneNo") = R.Item("PhoneNo").ToString
            tRow.Item("EmailAddr") = R.Item("EmailAddr").ToString

            NewTenants.Rows.Add(tRow)
        Next

        Dim tRow2 As DataRow = NewTenants.NewRow()
        tRow2.Item("TenantID") = ""
        tRow2.Item("FirstName") = NewFirst.Text
        tRow2.Item("LastName") = NewLast.Text
        tRow2.Item("DispName") = NewFirst.Text & " " & NewLast.Text
        tRow2.Item("PhoneNo") = NewPhon.Text
        tRow2.Item("EmailAddr") = NewEmail.Text
        NewTenants.Rows.Add(tRow2)

        iTenantsTbl = NewTenants.Copy()

        gvTenants.DataSource = iTenantsTbl
        gvTenants.DataBind()

        Session("TenantsTbl") = iTenantsTbl

        NewFirst.Text = ""
        NewLast.Text = ""
        NewPhon.Text = ""
        NewEmail.Text = ""
        AddTenant.Visible = False

    End Sub

    Protected Sub CancelNewTenant_Click(sender As Object, e As EventArgs) Handles CancelNewTen.Click
        NewFirst.Text = ""
        NewLast.Text = ""
        NewPhon.Text = ""
        NewEmail.Text = ""
        AddTenant.Visible = False
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