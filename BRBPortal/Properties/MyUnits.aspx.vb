Public Class MyUnits
    Inherits System.Web.UI.Page
    Private iPropertyAddress As String = ""
    Private iPropertyNo As String = ""

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

            wstr = Session("UserCode").ToString
            wstr2 = Session("BillingCode").ToString

            tPropStr = GetPropertyUnits_Soap(iPropertyNo, wstr, wstr2)
            If tPropStr = "FAILURE" Then
                If iErrMsg.IndexOf("(500) Internal Server Error") > -1 Then iErrMsg = "(500) Internal Server Error."
                ShowDialogOK("Error: Error retrieving Units. " & iErrMsg, "View Units")
                'MsgBox("Error: Error retrieving Units.", MsgBoxStyle.OkOnly)
                Return
            End If

            iUnitsTbl.DefaultView.Sort = "UnitNo ASC"
            iUnitsTbl = iUnitsTbl.DefaultView.ToTable

            gvUnits.DataSource = iUnitsTbl
            gvUnits.DataBind()

            gvUnits.Columns(2).Visible = False      'Do this so it stores the value in the GV but doesn't show it

            MainAddress.Text = iPropAddr
            BillAddr.Text = iBillAddr
            MgrName.Text = iAgentName

            Session("UnitsTbl") = iUnitsTbl
        End If

    End Sub

    Protected Sub OnRowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            'Dim cb1 As CheckBox = TryCast(e.Row.Cells(0).Controls(0), CheckBox)
            'Dim cb2 As CheckBox = TryCast(e.Row.Cells(1).Controls(0), CheckBox)
            'cb1.Enabled = True
            'cb2.Enabled = True
        End If
    End Sub

    Protected Sub NextBtn_Click(sender As Object, e As EventArgs)
        Dim tUnit As String = ""
        Dim tUnitStr As String = ""
        Dim tUnitID As String = ""
        Dim tsep As String = "::"

        If gvUnits.Rows.Count > 0 Then
            For Each row As GridViewRow In gvUnits.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    tUnitID = row.Cells(2).Text         'Unit ID
                    tUnit = row.Cells(3).Text           'Unit Number
                    'Session("UnitNo") = tUnit
                    Session("UpdTenants") = False

                    Dim chkRow As CheckBox = TryCast(row.Cells(0).Controls(1), CheckBox)    'Unit Status
                    If chkRow.Checked Then
                        'Check if the Tenancy Update was also checked
                        Dim chkRow3 As CheckBox = TryCast(row.Cells(1).Controls(1), CheckBox)    'Tenancy Update
                        If chkRow3.Checked Then
                            Session("UpdTenants") = True
                        End If
                        Session("UnitNo") = tUnit
                        Session("UnitID") = tUnitID

                        'Build XML string for the Unit page
                        tUnitStr = iPropertyNo & tsep & Session("PropAddr").ToString & tsep & tUnit & tsep
                        tUnitStr += row.Cells(5).Text & tsep                                'Unit Status
                        Session("UpdUnitInfo") = tUnitStr

                        Response.Redirect("~/Properties/UpdateUnit", False)
                        Exit For
                    End If

                    Dim chkRow2 As CheckBox = TryCast(row.Cells(1).Controls(1), CheckBox)    'Tenancy Update
                    If chkRow2.Checked Then
                        Session("UnitNo") = tUnit
                        Session("UnitID") = tUnitID
                        Response.Redirect("~/Properties/MyTenants", False)
                    End If
                End If
            Next
        End If

    End Sub

    Protected Sub ToProperty_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Response.Redirect("~/MyProperties/MyProperties", False)
    End Sub

    Protected Sub RemAgent_Click(sender As Object, e As EventArgs) Handles btnRemAgnt.Click

        ShowDialogOK("Please call the Rent Board to remove an agent.", "List Units")
    End Sub

    Sub gvUnits_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)

        'If gvUnits.Rows.Count = 10 Then Return

        gvUnits.PageIndex = e.NewPageIndex
        gvUnits.DataSource = Session("UnitsTbl")
        gvUnits.DataBind()

        gvUnits.Columns(2).Visible = False      'Do this so it stores the value in the GV but doesn't show it

    End Sub

    Protected Sub cbUnit_CheckedChanged(sender As Object, e As EventArgs)

        Dim tChkBox As CheckBox = TryCast(sender, CheckBox)
        Dim tRow As GridViewRow = tChkBox.Parent.Parent

        For Each Row As GridViewRow In gvUnits.Rows
            If Row.RowIndex = tRow.RowIndex Then Continue For

            Dim tOthrChkBox As CheckBox = TryCast(Row.Cells(0).Controls(1), CheckBox)
            If tOthrChkBox.Checked = True Then tOthrChkBox.Checked = False
        Next

    End Sub

    Protected Sub cbTenant_CheckedChanged(sender As Object, e As EventArgs)

        Dim tChkBox As CheckBox = TryCast(sender, CheckBox)
        Dim tRow As GridViewRow = tChkBox.Parent.Parent

        For Each Row As GridViewRow In gvUnits.Rows
            If Row.RowIndex = tRow.RowIndex Then Continue For

            Dim tOthrChkBox As CheckBox = TryCast(Row.Cells(1).Controls(1), CheckBox)
            If tOthrChkBox.Checked = True Then tOthrChkBox.Checked = False
        Next

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