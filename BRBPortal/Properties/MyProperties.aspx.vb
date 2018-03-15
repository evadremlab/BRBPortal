Imports System.Xml

Public Class MyProperties
    Inherits System.Web.UI.Page

    Public iNumProps As Integer
    Public iCartTbl As New DataTable
    Public iCartStr As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim wstr, wstr2 As String

        If Session("UserCode") Is Nothing OrElse Session("BillingCode") Is Nothing OrElse
            Session("UserCode").ToString = "" OrElse Session("BillingCode").ToString = "" Then
            Response.Redirect("..\Account\Login", False)
            Return
        End If

        If Not IsPostBack Then
            wstr = Session("UserCode").ToString
            wstr2 = Session("BillingCode").ToString

            'Get user properties
            If wstr > "" OrElse wstr2 > "" Then
                If GetUserProperties_Soap(wstr, wstr2) = False Then
                    'MsgBox("Error: Invalid user id or billing code.", MsgBoxStyle.OkOnly, "Property List")
                    Return
                End If

                'Fill table on screen from iPropertyTbl
                iNumProps = iPropertyTbl.Rows.Count
                gvProperties.DataSource = iPropertyTbl
                gvProperties.DataBind()
                gvProperties.Columns(1).Visible = False       'Set PropertyID to non-visible

                Session("PropertyTbl") = iPropertyTbl
            End If

            'Temporary
            'Session("Cart") = "<properties><property><propno>6209</propno><addr>3006 MLK JR WAY</addr><cfees>500.00</cfees><pfees>100.00</pfees><cpen>50.00</cpen><ppen>44.00</ppen><creds>22.00</creds><bal>672.00</bal></property></properties>"
        End If

    End Sub

    Protected Sub AddCart_Click(sender As Object, e As EventArgs) Handles btnAddToCart.Click
        Dim RowFound As Boolean = False
        Dim tPropNo As String

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

        For Each row As GridViewRow In gvProperties.Rows
            If row.RowType = DataControlRowType.DataRow Then
                Dim chkRow As CheckBox = TryCast(row.Cells(0).FindControl("chkProp"), CheckBox)
                If chkRow.Checked Then
                    tPropNo = row.Cells(1).Text

                    For Each R As DataRow In iCartTbl.Rows
                        If R.Item("PropertyID").ToString = tPropNo Then
                            RowFound = True
                            R.Item("MainAddr") = row.Cells(2).Text
                            R.Item("CurrFees") = CDec(row.Cells(3).Text)
                            R.Item("PriorFees") = CDec(row.Cells(4).Text)
                            R.Item("CurrPenalty") = CDec(row.Cells(5).Text)
                            R.Item("PriorPenalty") = CDec(row.Cells(6).Text)
                            R.Item("Credits") = CDec(row.Cells(7).Text)
                            R.Item("Balance") = CDec(row.Cells(8).Text)
                        End If
                    Next

                    If RowFound = False Then
                        Dim NR As DataRow = iCartTbl.NewRow
                        NR.Item("PropertyID") = tPropNo
                        NR.Item("MainAddr") = row.Cells(2).Text
                        NR.Item("CurrFees") = CDec(row.Cells(3).Text)
                        NR.Item("PriorFees") = CDec(row.Cells(4).Text)
                        NR.Item("CurrPenalty") = CDec(row.Cells(5).Text)
                        NR.Item("PriorPenalty") = CDec(row.Cells(6).Text)
                        NR.Item("Credits") = CDec(row.Cells(7).Text)
                        NR.Item("Balance") = CDec(row.Cells(8).Text)
                        iCartTbl.Rows.Add(NR)
                    End If

                End If
            End If
        Next

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

        'Loop through and uncheck all rows
        For Each row As GridViewRow In gvProperties.Rows
            If row.RowType = DataControlRowType.DataRow Then
                Dim chkRow As CheckBox = TryCast(row.Cells(0).FindControl("chkProp"), CheckBox)
                If chkRow.Checked Then chkRow.Checked = False
            End If
        Next

    End Sub

    Protected Sub UpdatePropClicked(sender As Object, e As GridViewCommandEventArgs)
        Dim tPropNo, tMainAddr As String
        Dim RowNum As Integer
        Dim tBalance As Decimal
        Dim tRow As GridViewRow

        RowNum = e.CommandArgument
        tRow = gvProperties.Rows(RowNum)
        tPropNo = iPropertyTbl.Rows(RowNum).Item("PropertyID").ToString
        Session("PropertyID") = tPropNo
        tMainAddr = iPropertyTbl.Rows(RowNum).Item("MainAddr").ToString
        Session("PropAddr") = tMainAddr
        tBalance = CDec(iPropertyTbl.Rows(RowNum).Item("Balance").ToString)
        Session("PropBalance") = tBalance

        Response.Redirect("~/Properties/MyUnits")
    End Sub

    'Protected Sub RowDataBound(sender As Object, e As GridViewRowEventArgs)
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        'Dim checkBox As CheckBox = TryCast(e.Row.Cells(0).Controls(0), CheckBox)
    '        'checkBox.Enabled = True
    '    End If
    'End Sub
    Sub gvProperties_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)

        gvProperties.PageIndex = e.NewPageIndex
        gvProperties.DataSource = Session("PropertyTbl")
        gvProperties.DataBind()
    End Sub

End Class