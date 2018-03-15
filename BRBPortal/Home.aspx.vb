Public Class Home
    Inherits System.Web.UI.Page
    Public BillingCode As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim wstr As String = ""

        If Session("UserCode") Is Nothing OrElse Session("BillingCode") Is Nothing OrElse
            Session("UserCode").ToString = "" OrElse Session("BillingCode").ToString = "" Then
            Response.Redirect("~\Account\Login", False)
            Return
        End If

        wstr = Session("UserCode").ToString
        BillingCode = wstr
    End Sub

    Protected Sub MngSel_Click(sender As Object, e As EventArgs)
        If HomeOption.SelectedValue = "MngProfile" Then
            Response.Redirect("~\Account\ProfileList.aspx", False)
        End If

        If HomeOption.SelectedValue = "MngPay" Then
            Response.Redirect("~\Properties\MyProperties.aspx", False)
        End If
    End Sub
End Class