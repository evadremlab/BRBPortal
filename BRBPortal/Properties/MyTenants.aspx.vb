Imports QSILib

Public Class MyTenants
    Inherits System.Web.UI.Page
    Private iPropAddr As String = ""
    Private iPropertyNo As String = ""
    Private iUnitNum As String = ""
    Private iUnitID As String = ""

    Dim iTenants As New DataTable
    Dim iDVOut As DataView

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim wstr, wstr2, tPropStr As String

        If Session("UserCode") Is Nothing OrElse Session("BillingCode") Is Nothing OrElse
            Session("UserCode").ToString = "" OrElse Session("BillingCode").ToString = "" Then
            Response.Redirect("..\Account\Login", False)
            Return
        End If

        If Not IsPostBack Then
            iPropertyNo = Session("PropertyID").ToString
            iPropAddr = Session("PropAddr").ToString
            iUnitNum = Session("UnitNo").ToString
            iUnitID = Session("UnitID").ToString

            wstr = Session("UserCode").ToString
            wstr2 = Session("BillingCode").ToString

            tPropStr = GetPropertyTenants_Soap(iPropertyNo, wstr, wstr2, iUnitID)
            If tPropStr = "FAILURE" Then
                If iErrMsg.IndexOf("(500) Internal Server Error") > -1 Then iErrMsg = "(500) Internal Server Error."
                ShowDialogOK("Error: Error retrieving Tenants. " & iErrMsg, "View Tenants")
                'MsgBox("Error: Error retrieving Tenants.", MsgBoxStyle.OkOnly)
                Return
            End If

            iTenantsTbl.DefaultView.Sort = "LastName, FirstName ASC"
            iTenantsTbl = iTenantsTbl.DefaultView.ToTable

            gvTenants.DataSource = iTenantsTbl
            gvTenants.DataBind()

            Session("TenantsTbl") = iTenantsTbl

            MainAddress.Text = iPropAddr
            UnitNo.Text = iUnitNum
            BalAmt.Text = Session("PropBalance").ToString

            'Parse property string from GetPropertyTenants_Soap
            wstr2 = tPropStr
            wstr = ParseStr(wstr2, "::")        'Rented or Exempt
            If wstr.Length > wstr.IndexOf("=") Then UnitStat.Text = wstr.Substring(wstr.IndexOf("=") + 1)
            wstr = ParseStr(wstr2, "::")        'Housing Services
            If wstr.Length > wstr.IndexOf("=") Then HouseServs.Text = wstr.Substring(wstr.IndexOf("=") + 1)
            wstr = ParseStr(wstr2, "::")        'Start Date
            If wstr.Length > wstr.IndexOf("=") Then TenStDt.Text = wstr.Substring(wstr.IndexOf("=") + 1)
            wstr = ParseStr(wstr2, "::")        'Number of Tenants
            If wstr.Length > wstr.IndexOf("=") Then NumTenants.Text = wstr.Substring(wstr.IndexOf("=") + 1)
            wstr = ParseStr(wstr2, "::")        'Smoking Y/N
            If wstr.Length > wstr.IndexOf("=") Then SmokYN.Text = wstr.Substring(wstr.IndexOf("=") + 1)
            wstr = ParseStr(wstr2, "::")        'Smoking Start Date
            If wstr.Length > wstr.IndexOf("=") Then SmokDt.Text = wstr.Substring(wstr.IndexOf("=") + 1)
            wstr = ParseStr(wstr2, "::")        'Initial Rent
            If wstr.Length > wstr.IndexOf("=") Then InitRent.Text = wstr.Substring(wstr.IndexOf("=") + 1)
            wstr = ParseStr(wstr2, "::")        'Prior Tenancy End Date
            If wstr.Length > wstr.IndexOf("=") Then PriorEndDt.Text = wstr.Substring(wstr.IndexOf("=") + 1)
            wstr = ParseStr(wstr2, "::")        'Termination reason
            If wstr.Length > wstr.IndexOf("=") Then TermReason.Text = wstr.Substring(wstr.IndexOf("=") + 1)
            wstr = ParseStr(wstr2, "::")        'Billing (Owner) Contact
            If wstr.Length > wstr.IndexOf("=") Then OwnerName.Text = wstr.Substring(wstr.IndexOf("=") + 1)
            wstr = ParseStr(wstr2, "::")        'Agent Name
            If wstr.Length > wstr.IndexOf("=") Then AgentName.Text = wstr.Substring(wstr.IndexOf("=") + 1)

            FailureText.Text = ""
            ErrorMessage.Visible = False

        End If

    End Sub

    Protected Sub NextBtn_Click(sender As Object, e As EventArgs)
        Dim wstr As String = ""

        'If Stat1.Checked Or Ten1.Checked Then
        '    If Stat2.Checked Or Stat3.Checked Or Ten2.Checked Or Ten3.Checked Then
        '        'Error
        '    End If
        '    wstr = Unit1.InnerText
        'End If

        'If Stat2.Checked Or Ten2.Checked Then
        '    If Stat1.Checked Or Stat3.Checked Or Ten1.Checked Or Ten3.Checked Then
        '        'Error
        '    End If
        '    wstr = Unit2.InnerText
        'End If

        'If Stat3.Checked Or Ten3.Checked Then
        '    If Stat2.Checked Or Stat1.Checked Or Ten2.Checked Or Ten1.Checked Then
        '        'Error
        '    End If
        '    wstr = Unit3.InnerText
        'End If

        'If Stat1.Checked OrElse Stat2.Checked OrElse Stat3.Checked Then
        '    Response.Redirect("~/Properties/UpdateUnit?field1=" & PropertyAddress & "&field2=" & wstr)
        'End If

        'If Ten1.Checked OrElse Ten2.Checked OrElse Ten3.Checked Then
        '    Response.Redirect("~/Properties/MyTenants?field1=" & PropertyAddress & "&field2=" & wstr)
        'End If
    End Sub

    Protected Sub ToUnits_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Response.Redirect("~/Properties/MyUnits", False)
    End Sub

    Sub gvTenants_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)

        gvTenants.PageIndex = e.NewPageIndex
        gvTenants.DataSource = Session("TenantsTbl")
        gvTenants.DataBind()
    End Sub

    Protected Sub btnUpdTen_Click(sender As Object, e As EventArgs)

        ShowDialogYN("TensMovedOut", "Have all original tenants moved out?", "Update Tenancy")

        'If MsgBox("Have all original tenants moved out?", MsgBoxStyle.YesNo, "Confirmation") = MsgBoxResult.No Then
        '    MsgBox("New tenancy cannot be updated until all original tenants have moved out.", MsgBoxStyle.OkOnly)
        '    Response.Redirect("~/Properties/MyUnits", False)
        'Else
        '    Response.Redirect("~/Properties/UpdateTenancy", False)
        'End If
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
            Case "TensMovedOut"
                Response.Redirect("~/Properties/UpdateTenancy")
                Return

            Case Else
                'Response.Redirect("~/Properties/MyUnits", False)
                Return
        End Select
    End Sub

    ''' <summary>Receive No response from Dialog Box</summary>
    Protected Sub DialogResponseNo(sender As Object, e As EventArgs)

        Select Case hfDialogID.Value
            Case Else
                ShowDialogOK("New tenancy cannot be updated until all original tenants have moved out.", "Update Tenancy")
                Return
        End Select
    End Sub

End Class