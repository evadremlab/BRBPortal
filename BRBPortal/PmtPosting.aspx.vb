Imports System.Xml
Imports System.IO
Imports System.Net

Public Class PmtPosting
    Inherits System.Web.UI.Page

    'Private Sub PmtPosting_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit

    '    Dim DataStream As Stream = Request.InputStream
    '    Dim Reader As StreamReader

    '    DataStream.Seek(0, SeekOrigin.Begin)

    '    Reader = New StreamReader(DataStream)
    '    Dim tBody As String = Reader.ReadToEnd()

    '    Response.Write("<Location>PmtPostback_PreInit</Location><br />")
    '    Response.Write("<tBody.Count>" & tBody.Length.ToString & "</tBody.Count><br />")

    '    Response.Write("<tBody>" & tBody & "</tBody><br />")

    '    DataStream.Close()
    '    Reader.Close()

    '    Dim doc As New XmlDocument
    '    doc.LoadXml(tBody)

    '    Session("tBody") = doc.OuterXml.ToString

    'End Sub

    Protected Sub PmtPosting_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim wstr As String = ""
        'Dim tXMLstr As String
        Dim tArgs As NameValueCollection
        Dim tKey, tVal As String
        'Dim arr1() As String

        'Response.Write("<Location>In Page_Load</Location><br />")
        'Response.Write("<tBody>" & Session("tBody").ToString & "</tBody><br />")

        'tArgs = Request.Form

        'wstr = ""

        'arr1 = tArgs.AllKeys
        'Response.Write("tArgs.AllKeys.Count = " & tArgs.AllKeys.Count.ToString & "<br />")
        'For loop1 = 0 To arr1.GetUpperBound(0)
        '    Response.Write("Form: " & arr1(loop1) & "<br />")
        'Next loop1

        tArgs = Request.QueryString

        wstr = ""
        If tArgs.Count > 0 Then
            wstr = "tArgs.QueryString.Count = " & tArgs.Count.ToString & "<br />"
            For i = 0 To tArgs.Count - 1
                If tArgs.HasKeys Then
                    tKey = tArgs.GetKey(i).ToString
                    tVal = tArgs.Get(i).ToString
                    If i > 0 Then wstr += " - "
                    wstr += tKey & "=" & tVal
                End If
            Next
        End If

        'Response.Write("Results: " & wstr & "<br />")

        MiscMsgs.Text = wstr

        'Dim doc As New XmlDocument
        'doc.LoadXml(tXMLstr)

        'For Each detail As XmlElement In doc.DocumentElement
        '    MiscMsgs.Text = detail.SelectSingleNode("resultText").InnerText
        '    ConfNo.Text = detail.SelectSingleNode("receiptNumber").InnerText
        'Next

        'wstr = Request.QueryString("uid")
        'ConfNo.Text = Request.QueryString("confirmation")

        '''''https://rentportaldev.cityofberkeley.info/PmtPostBack
    End Sub


    Protected Sub RtnHome_Click(sender As Object, e As EventArgs) Handles btnRtnHome.Click
        Response.Redirect("~/Home", False)
    End Sub

End Class