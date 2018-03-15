Imports System.Xml

Public Class PayCart
    Inherits Page

    'Public iCartTbl As New DataTable
    'Public iBalance As Decimal = CDec(0.00)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Response.Write(Session("PayCartXML").ToString)
    End Sub

End Class