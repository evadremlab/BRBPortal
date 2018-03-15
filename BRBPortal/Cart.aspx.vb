Imports System.Xml

Public Class Cart
    Inherits Page

    Public iCartTbl As New DataTable
    Public iBalance As Decimal = CDec(0.00)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        If Session("UserCode") Is Nothing OrElse Session("BillingCode") Is Nothing OrElse
            Session("UserCode").ToString = "" OrElse Session("BillingCode").ToString = "" Then
            Response.Redirect("~\Account\Login", False)
            Return
        End If

        If Not IsPostBack Then
            'Build out the table if needed
            If iCartTbl Is Nothing OrElse iCartTbl.Columns.Count < 1 Then
                iCartTbl.Columns.Add("PropertyID", GetType(String))
                iCartTbl.Columns.Add("MainAddr", GetType(String))
                iCartTbl.Columns.Add("CurrFees", GetType(Decimal))
                iCartTbl.Columns.Add("PriorFees", GetType(Decimal))
                iCartTbl.Columns.Add("CurrPenalty", GetType(Decimal))
                iCartTbl.Columns.Add("PriorPenalty", GetType(Decimal))
                iCartTbl.Columns.Add("Credits", GetType(Decimal))
                iCartTbl.Columns.Add("Balance", GetType(Decimal))
            End If

            btnEdCart.Enabled = False
            btnPayCart.Enabled = False

            'Load the table from the Cart session variable if needed
            If Session("Cart") IsNot Nothing AndAlso Session("Cart").ToString > "" Then
                Dim doc As New XmlDocument
                doc.LoadXml(Session("Cart").ToString)

                For Each detail As XmlElement In doc.DocumentElement.GetElementsByTagName("property")
                    Dim NR As DataRow = iCartTbl.NewRow()
                    NR.Item("PropertyID") = detail.SelectSingleNode("propno").InnerText
                    NR.Item("MainAddr") = detail.SelectSingleNode("addr").InnerText
                    NR.Item("CurrFees") = detail.SelectSingleNode("cfees").InnerText
                    NR.Item("PriorFees") = detail.SelectSingleNode("pfees").InnerText
                    NR.Item("CurrPenalty") = detail.SelectSingleNode("cpen").InnerText
                    NR.Item("PriorPenalty") = detail.SelectSingleNode("ppen").InnerText
                    NR.Item("Credits") = detail.SelectSingleNode("creds").InnerText
                    NR.Item("Balance") = detail.SelectSingleNode("bal").InnerText
                    iCartTbl.Rows.Add(NR)
                Next detail

                gvCart.DataSource = iCartTbl
                gvCart.DataBind()
                gvCart.Columns(0).Visible = False       'Hide the PropertyID column

                Session("CartTbl") = iCartTbl

                btnEdCart.Enabled = True
                btnPayCart.Enabled = True

                If Session("FeesAll") Is Nothing Then
                    ShowFeesAll.Text = "All Fees and Penalties"
                Else
                    If Session("FeesAll").ToString = "AllFees" OrElse Session("FeesAll").ToString = "" Then
                        ShowFeesAll.Text = "All Fees and Penalties"
                    Else
                        ShowFeesAll.Text = "Fees Only"
                    End If
                End If
            Else
                ShowFeesAll.Text = "Nothing in your cart."
            End If
        End If

    End Sub

    Protected Sub CancelCart_Click(sender As Object, e As EventArgs) Handles btnCancelCart.Click
        Session("FeesAll") = "AllFees"      'Reset this to include everything
        Response.Redirect("~/Properties/MyProperties", False)
    End Sub

    Protected Sub PayCart_Click(sender As Object, e As EventArgs) Handles btnPayCart.Click
        Dim XMLstr As String
        Dim tSubTotal As Decimal

        If Session("Cart") Is Nothing OrElse Session("Cart").ToString.Length < 1 Then
            Response.Redirect("~/Properties/MyProperties", False)
            Return
        End If

        If iCartTbl Is Nothing OrElse iCartTbl.Columns.Count < 1 Then
            iCartTbl.Columns.Add("PropertyID", GetType(String))
            iCartTbl.Columns.Add("MainAddr", GetType(String))
            iCartTbl.Columns.Add("CurrFees", GetType(Decimal))
            iCartTbl.Columns.Add("PriorFees", GetType(Decimal))
            iCartTbl.Columns.Add("CurrPenalty", GetType(Decimal))
            iCartTbl.Columns.Add("PriorPenalty", GetType(Decimal))
            iCartTbl.Columns.Add("Credits", GetType(Decimal))
            iCartTbl.Columns.Add("Balance", GetType(Decimal))
        End If

        'Load the table from the Cart session variable if needed
        If Session("Cart") IsNot Nothing AndAlso Session("Cart").ToString > "" Then
            Dim doc As New XmlDocument
            doc.LoadXml(Session("Cart").ToString)

            For Each detail As XmlElement In doc.DocumentElement.GetElementsByTagName("property")
                Dim NR As DataRow = iCartTbl.NewRow()
                NR.Item("PropertyID") = detail.SelectSingleNode("propno").InnerText
                NR.Item("MainAddr") = detail.SelectSingleNode("addr").InnerText
                NR.Item("CurrFees") = detail.SelectSingleNode("cfees").InnerText
                NR.Item("PriorFees") = detail.SelectSingleNode("pfees").InnerText
                NR.Item("CurrPenalty") = detail.SelectSingleNode("cpen").InnerText
                NR.Item("PriorPenalty") = detail.SelectSingleNode("ppen").InnerText
                NR.Item("Credits") = detail.SelectSingleNode("creds").InnerText
                NR.Item("Balance") = detail.SelectSingleNode("bal").InnerText
                iCartTbl.Rows.Add(NR)
            Next detail
        End If

        'Build XML to send to ACI Universal Payment 
        XMLstr = "<html>"
        XMLstr += "<head>"
        XMLstr += "<title>Portal Test!</title>"
        XMLstr += "</head>"
        XMLstr += "<body>"
        XMLstr += "<form name=""FormName"" method = ""POST"" action=""https://staging.officialpayments.com/pc_entry_cobrand.jsp"">"
        XMLstr += "<input type = ""hidden"" name=""productId"" value=""24241003506464897434114212714156124"" /><br />"

        tSubTotal = CDec(0.00)
        For Each R As DataRow In iCartTbl.Rows
            tSubTotal += CDec(R.Item("Balance").ToString)
        Next

        If tSubTotal > CDec(500.0) Then tSubTotal = CDec(250.0)

        XMLstr += "<input type=""hidden"" name=""cde-BillingCode-1"" value=" & Session("BillingCode").ToString & " /><br />"
        XMLstr += "<input type=""hidden"" name=""paymentAmount"" value=" & tSubTotal & " /><br />"
        XMLstr += "<input type=""hidden"" size=""24"" maxlength=""20"" name=""returnUrl"" value=""http://rentportaldev.cityofberkeley.info/PmtPostBack"" /><br />"
        XMLstr += "<input type=""hidden="" size=""24"" maxlength=""20"" name=""errorUrl"" value=""http://rentportaldev.cityofberkeley.info/PmtPostBack"" /><br /> "
        XMLstr += "<input type=""hidden="" size=""24"" maxlength=""20"" name=""cancelUrl"" value=""http://rentportaldev.cityofberkeley.info/PmtPostBack"" /><br />"
        XMLstr += "<input type=""hidden="" size=""24"" maxlength=""20"" name=""postbackUrl"" value=""http://rentportaldev.cityofberkeley.info/PmtPostBack"" /><br />"
        XMLstr += "<input type=""text"" size=""24"" maxlength=""20"" name=""lockAmount"" value=""true"" /><br />"
        XMLstr += "<input type=""Submit"" value=""Continue &gt;&gt;"" width=""66"" height=""16"" border=""0"" alt=""Continue"" /><br />"
        XMLstr += "</form>"
        XMLstr += "</body>"
        XMLstr += "</html>"

        Session("PaymentAmt") = tSubTotal
        Session("PayCartXML") = XMLstr

        Response.Redirect("~/PayCart", False)

        'Testing Pmt Postback
        'Response.Redirect("~/PmtPostBack.aspx?uid=xyz&confirmation=abc123", False)

    End Sub

    Protected Sub EditCart_Click(sender As Object, e As EventArgs) Handles btnEdCart.Click
        Response.Redirect("~/EditCart", False)
    End Sub

    Sub gvCart_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)

        gvCart.PageIndex = e.NewPageIndex
        gvCart.DataSource = Session("CartTbl")
        gvCart.DataBind()
    End Sub

    Protected Sub gvCart_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim txtBalance As String = e.Row.Cells(7).Text
            iBalance = iBalance + CDec(txtBalance)
        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(7).Text = iBalance.ToString("c")
        End If
    End Sub

End Class