Imports System.Xml
Imports System.IO
Imports System.Net

'public strAuthorizationCode As String

Public Class PmtPostBack
    Inherits System.Web.UI.Page

    'Dim strPayIdnt As String
    Public strResultCode As String
    Public strResultText As String

    'public strTransactionDate As String
    'public strTransactionTime As String

    Public strPaymentAmount As String
    Public strTransactionFee As String
    Public strTotalCharge As String
    ' public strAccountType As String

    Public strPaymentID As String
    Public strReceiptNumber As String
    Public tHTMLstr As String

    Private Sub PmtPostBack_Init(sender As Object, e As EventArgs) Handles Me.Init

        'Dim DataStream As Stream = Request.InputStream
        'Dim Reader As StreamReader

        'DataStream.Seek(0, SeekOrigin.Begin)

        'Reader = New StreamReader(DataStream)
        'Dim tBody As String = Reader.ReadToEnd()

        'Response.Write("In PmtPostback_Init ... <br /> tBody.Count = " & tBody.Length.ToString & "<br />")

        'Response.Write("tBody ::" & tBody & "<br />")

        'DataStream.Close()
        'Reader.Close()

    End Sub

    Private Sub PmtPostBack_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        'Dim tBodyByte() As Byte
        Dim tBodyStr As String
        'Dim tArgs As NameValueCollection
        'Dim arr1() As String

        'Dim textReader = New IO.StreamReader(Request.InputStream)

        'Request.InputStream.Seek(0, IO.SeekOrigin.Begin)
        'textReader.DiscardBufferedData()
        'tBodyStr = textReader.ReadToEnd()

        Dim DataStream As Stream = Request.InputStream
        Dim Reader As StreamReader

        DataStream.Seek(0, SeekOrigin.Begin)

        Reader = New StreamReader(DataStream)
        tBodyStr = Reader.ReadToEnd()

        Response.Write("tBodyStr=" & tBodyStr)

        'tArgs = Request.Params

        'arr1 = tArgs.AllKeys
        'Response.Write("tArgs.AllKeys.Count = " & tArgs.AllKeys.Count.ToString & "<br />")
        'For loop1 = 0 To arr1.GetUpperBound(0)
        '    Response.Write("Form: " & arr1(loop1) & "<br />")
        'Next loop1

        'Dim Xmlin = XDocument.Load(textReader)

        'Dim strXml As String = Xmlin.ToString

        ''Request.Headers.AllKeys
        'Dim doc As New XmlDocument
        'doc.LoadXml(strXml)

        'Dim txmllist As XmlNodeList

        'txmllist = doc.GetElementsByTagName("paymentIdentifier")

        'For Each detail As XmlNode In txmllist
        '    strPaymentID = detail.InnerText
        'Next detail

        'txmllist = doc.GetElementsByTagName("resultCode")
        'strResultCode = ""
        'For Each detail As XmlNode In txmllist
        '    strResultCode = detail.InnerText
        'Next detail

        'txmllist = doc.GetElementsByTagName("resultText")

        'For Each detail As XmlNode In txmllist
        '    strResultText = detail.InnerText
        'Next detail

        'If strResultCode = "A" Then
        '    txmllist = doc.GetElementsByTagName("receiptNumber")

        '    For Each detail As XmlNode In txmllist
        '        strReceiptNumber = detail.InnerText
        '    Next detail

        '    txmllist = doc.GetElementsByTagName("paymentID")

        '    For Each detail As XmlNode In txmllist
        '        strPaymentID = detail.InnerText
        '    Next detail

        '    txmllist = doc.GetElementsByTagName("paymentAmount")

        '    For Each detail As XmlNode In txmllist
        '        strPaymentAmount = detail.InnerText
        '    Next detail

        '    txmllist = doc.GetElementsByTagName("transactionFee")

        '    For Each detail As XmlNode In txmllist
        '        strTransactionFee = detail.InnerText
        '    Next detail

        '    txmllist = doc.GetElementsByTagName("totalCharge")

        '    For Each detail As XmlNode In txmllist
        '        strTotalCharge = detail.InnerText
        '    Next detail
        'End If

    End Sub

    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '    Dim wstr As String = ""
    '    'Dim tXMLstr As String
    '    'Dim tArgs As NameValueCollection
    '    'Dim tKey, tVal As String
    '    'Dim arr1() As String

    '    If strResultCode = "A" Then
    '        tHTMLstr = "<table><tr><td colspan=""2"">Success!</td></tr>"
    '        tHTMLstr += "<tr><td>Payment Amount</td><td>" & strPaymentAmount & "</td></tr>"
    '        tHTMLstr += "<tr><td>Transaction Fees</td><td>" & strTransactionFee & "</td></tr>"
    '        tHTMLstr += "<tr><td>Total Charge</td><td>" & strTotalCharge & "</td></tr>"
    '        tHTMLstr += "<tr><td>Confirmation Number</td><td>" & strReceiptNumber & "</td></tr>"
    '        tHTMLstr += "</table>"

    '        Response.Write(tHTMLstr)
    '    End If









    '    'Response.Write("<Location>In Page_Load</Location><br />")
    '    'Response.Write("<tBody>" & Session("tBody").ToString & "</tBody><br />")

    '    'tArgs = Request.Form

    '    'wstr = ""

    '    'arr1 = tArgs.AllKeys
    '    'Response.Write("tArgs.AllKeys.Count = " & tArgs.AllKeys.Count.ToString & "<br />")
    '    'For loop1 = 0 To arr1.GetUpperBound(0)
    '    '    Response.Write("Form: " & arr1(loop1) & "<br />")
    '    'Next loop1

    '    'tArgs = Request.QueryString

    '    'If tArgs.Count > 0 Then
    '    '    wstr = "tArgs.QueryString.Count = " & tArgs.Count.ToString & "<br />"
    '    '    For i = 0 To tArgs.Count - 1
    '    '        If tArgs.HasKeys Then
    '    '            tKey = tArgs.GetKey(i).ToString
    '    '            tVal = tArgs.Get(i).ToString
    '    '            If i > 0 Then wstr += " - "
    '    '            wstr += tKey & "=" & tVal
    '    '        End If
    '    '    Next
    '    'End If

    '    'Response.Write("Results: " & wstr & "<br />")

    '    'MiscMsgs.Text = wstr

    '    'Dim doc As New XmlDocument
    '    'doc.LoadXml(tXMLstr)

    '    'For Each detail As XmlElement In doc.DocumentElement
    '    '    MiscMsgs.Text = detail.SelectSingleNode("resultText").InnerText
    '    '    ConfNo.Text = detail.SelectSingleNode("receiptNumber").InnerText
    '    'Next

    '    'wstr = Request.QueryString("uid")
    '    'ConfNo.Text = Request.QueryString("confirmation")

    '    '''''https://rentportaldev.cityofberkeley.info/PmtPostBack
    'End Sub

End Class