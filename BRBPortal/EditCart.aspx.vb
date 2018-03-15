Imports System.Xml

Public Class EditCart
    Inherits Page

    Public iCartTbl As New DataTable
    Public iBalance As Decimal = CDec(0.00)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

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

            'Load the table from the Cart session variable if needed
            If Session("Cart") IsNot Nothing AndAlso Session("Cart").ToString > "" Then
                OrigCart.Value = Session("Cart").ToString

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

            gvCart.DataSource = iCartTbl
            gvCart.DataBind()
            gvCart.Columns(0).Visible = False       'Hide the PropertyID column

            If Session("FeesAll") Is Nothing Then
                FeesAll.SelectedValue = "AllFees"
            Else
                FeesAll.SelectedValue = Session("FeesAll").ToString
            End If

        End If

    End Sub

    Protected Sub CancEditCart_Click(sender As Object, e As EventArgs) Handles btnCancEditCart.Click
        Session("Cart") = OrigCart.Value.ToString
        Response.Redirect("~/Cart", False)
    End Sub

    Protected Sub SaveCart_Click(sender As Object, e As EventArgs) Handles btnSaveCart.Click
        Dim iCartStr, SaveXML As String
        Dim rnd1 As New Random()

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

        For Each R As GridViewRow In gvCart.Rows
            Dim NR As DataRow = iCartTbl.NewRow()
            NR.Item("PropertyID") = R.Cells(1).Text
            'NR.Item("PropertyID") = CType(R.FindControl("hfPropID"), HiddenField).Value.ToString
            NR.Item("MainAddr") = R.Cells(2).Text
            NR.Item("CurrFees") = CDec(R.Cells(3).Text)
            NR.Item("PriorFees") = CDec(R.Cells(4).Text)
            NR.Item("CurrPenalty") = CDec(R.Cells(5).Text)
            NR.Item("PriorPenalty") = CDec(R.Cells(6).Text)
            NR.Item("Credits") = CDec(R.Cells(7).Text)
            NR.Item("Balance") = CDec(R.Cells(8).Text)
            iCartTbl.Rows.Add(NR)
        Next

        iCartStr = ""
        SaveXML = ""
        If iCartTbl.Rows.Count > 0 Then
            iCartStr = "<properties>"

            For Each R As DataRow In iCartTbl.Rows
                iCartStr += "<property><propno>" & R.Item("PropertyID").ToString & "</propno><addr>" & R.Item("MainAddr").ToString &
                            "</addr><cfees>" & R.Item("CurrFees").ToString & "</cfees><pfees>" & R.Item("PriorFees").ToString &
                            "</pfees><cpen>" & R.Item("CurrPenalty").ToString & "</cpen><ppen>" & R.Item("PriorPenalty").ToString &
                            "</ppen><creds>" & R.Item("Credits").ToString & "</creds><bal>" & R.Item("Balance").ToString & "</bal></property>"
            Next
            iCartStr += "</properties>"
        End If
        Session("Cart") = iCartStr
        Session("CartTbl") = iCartTbl
        Session("FeesAll") = FeesAll.SelectedValue.ToString

        'Need to Save cart to RTS

        SaveXML = "<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:api=""http://cityofberkeley.info/RTS/ClientPortal/API"">"
        SaveXML += "<soapenv:Header/>"
        SaveXML += "<soapenv:Body>"
        SaveXML += "<api:savePaymentCart>"
        SaveXML += "<savePaymentToCart>"
        SaveXML += "<cartId>" & rnd1.Next().ToString & "</cartId>"
        SaveXML += "<paymentConfirmationNo/>"
        SaveXML += "<paymentReceivedAmt/>"

        If FeesAll.SelectedValue.ToString = "AllFees" Then
            SaveXML += "<isFeeOnlyPaid>False</isFeeOnlyPaid>"
        Else
            SaveXML += "<isFeeOnlyPaid>True</isFeeOnlyPaid>"
        End If

        If iCartTbl.Rows.Count > 0 Then
            Dim ctr As Integer = 0
            For Each R As DataRow In iCartTbl.Rows
                ctr += 1
                SaveXML += "<items>"
                SaveXML += "<itemId>" & ctr.ToString & "</itemId>"
                SaveXML += "<propertyId>" & R.Item("PropertyID").ToString & "</propertyId>"
                SaveXML += "<propertyMainStreetAddress>" & R.Item("MainAddr").ToString & "</propertyMainStreetAddress>"
                If FeesAll.SelectedValue.ToString = "AllFees" Then
                    SaveXML += "<fee>" & CDec(R.Item("CurrFees").ToString) + CDec(R.Item("PriorFees").ToString) + CDec(R.Item("Credits").ToString) & "</fee>"
                    SaveXML += "<penalties>" & CDec(R.Item("CurrPenalty").ToString) + CDec(R.Item("PriorPenalty").ToString) & "</penalties>"
                Else
                    SaveXML += "<fee>" & CDec(R.Item("CurrFees").ToString) + CDec(R.Item("PriorFees").ToString) & "</fee>"
                    SaveXML += "<penalties>0.00</penalties>"
                End If
                SaveXML += "<balance>" & R.Item("Balance").ToString & "</balance>"
                SaveXML += "</items>"
            Next
        End If

        SaveXML += "</savePaymentToCart>"
        SaveXML += "<request>"
        If Session("UserCode").ToString.Length > 0 Then
            SaveXML += "<!--Optional:--><userId>" & Session("UserCode").ToString & "</userId>"
        Else
            SaveXML += "<!--Optional:--><userId>?</userId>"
        End If
        If Session("BillingCode").ToString.Length > 0 Then
            SaveXML += "<!--Optional:--><billingCode>" & Session("BillingCode").ToString & "</billingCode>"
        Else
            SaveXML += "<!--Optional:--><billingCode>?</billingCode>"
        End If
        SaveXML += "</request>"
        SaveXML += "</api:savePaymentCart>"
        SaveXML += "</soapenv:Body>"
        SaveXML += "</soapenv:Envelope>"

        If SaveCart_Soap(SaveXML) = False Then
            If iErrMsg.IndexOf("(500) Internal Server Error") > -1 Then iErrMsg = "(500) Internal Server Error."
            ShowDialogOK("Error: Problem saving cart to RTS. " & iErrMsg, "Edit Cart <Save>")
            'MsgBox("Error: Problem saving cart to RTS.", MsgBoxStyle.OkOnly, "Edit Cart <Save>")
        End If

        Response.Redirect("~/Cart", False)

    End Sub

    Sub gvCart_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)

        gvCart.PageIndex = e.NewPageIndex
        gvCart.DataSource = Session("CartTbl")
        gvCart.DataBind()
    End Sub

    Protected Sub gvCart_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim txtBalance As String = e.Row.Cells(8).Text
            iBalance = iBalance + CDec(txtBalance)
        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(8).Text = iBalance.ToString("c")
        End If
    End Sub

    Protected Sub gvCart_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        'Dim tPropNo As String
        Dim RowNum As Integer
        Dim iCartStr As String

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

        RowNum = e.CommandArgument
        iCartTbl.Rows(RowNum).Delete()

        gvCart.DataSource = iCartTbl
        gvCart.DataBind()
        gvCart.Columns(0).Visible = False

        iCartStr = ""
        If iCartTbl.Rows.Count > 0 Then
            iCartStr = "<properties>"
            For Each R As DataRow In iCartTbl.Rows
                iCartStr += "<property><propno>" & R.Item("PropertyID").ToString & "</propno><addr>" & R.Item("MainAddr").ToString &
                    "</addr><cfees>" & R.Item("CurrFees").ToString & "</cfees><pfees>" & R.Item("PriorFees").ToString &
                    "</pfees><cpen>" & R.Item("CurrPenalty").ToString & "</cpen><ppen>" & R.Item("PriorPenalty").ToString &
                    "</ppen><creds>" & R.Item("Credits").ToString & "</creds><bal>" & R.Item("Balance").ToString & "</bal></property>"
            Next
            iCartStr += "</properties>"
        End If
        Session("Cart") = iCartStr

    End Sub

    Protected Sub FeesAll_SelectedIndexChanged(sender As Object, e As EventArgs)

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

        If FeesAll.Text = "AllFees" Then
            For Each R As DataRow In iCartTbl.Rows
                R.Item("Balance") = CDec(R.Item("CurrFees").ToString) + CDec(R.Item("PriorFees").ToString) + CDec(R.Item("CurrPenalty").ToString) +
                                    CDec(R.Item("PriorPenalty").ToString) + CDec(R.Item("Credits").ToString)
            Next
        Else
            For Each R As DataRow In iCartTbl.Rows
                R.Item("Balance") = CDec(R.Item("CurrFees").ToString) + CDec(R.Item("PriorFees").ToString)
            Next
        End If

        gvCart.DataSource = iCartTbl
        gvCart.DataBind()
        gvCart.Columns(0).Visible = False       'Hide the PropertyID column
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