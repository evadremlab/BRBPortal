Imports System.Net
Imports System.IO
Imports System
Imports System.Xml
Imports System.Net.HttpWebRequest
Public Class Exchange
    Public Enum AuthType
        Windows
        FBA
    End Enum
    Private _AuthType As AuthType
    Private _Host As String = ""
    Private _Folder As String = ""
    Private _Email As String = ""
    Private _UserName As String = ""
    Private _UserID As String = ""
    Private _Domain As String = ""
    Private _Password As String = ""
    Private _CookieJar As CookieCollection = Nothing
    Private _Encode As New System.Text.UTF8Encoding
    Private _SaveAttachToFolder As String = ""

    Public Property AuthorityType() As AuthType
        Get
            Return _AuthType
        End Get
        Set(ByVal value As AuthType)
            _AuthType = value
        End Set
    End Property

    Public Property Host() As String
        Get
            Return _Host
        End Get
        Set(ByVal value As String)
            _Host = value
        End Set
    End Property

    Public Property Folder() As String
        Get
            Return _Folder
        End Get
        Set(ByVal value As String)
            _Folder = value
        End Set
    End Property

    Public Property UserName() As String
        Get
            Return _UserName
        End Get
        Set(ByVal value As String)
            _UserName = value
        End Set
    End Property

    Public Property UserID() As String
        Get
            Return _UserID
        End Get
        Set(ByVal value As String)
            _UserID = value
        End Set
    End Property

    Public Property Domain() As String
        Get
            Return _Domain
        End Get
        Set(ByVal value As String)
            _Domain = value
        End Set
    End Property

    Public Property Password() As String
        Get
            Return _Password
        End Get
        Set(ByVal value As String)
            _Password = value
        End Set
    End Property

    Public Sub New(ByVal aHost As String, ByVal aFolder As String, ByVal aUserName As String, ByVal aUserID As String, ByVal aPassword As String)
        _Host = aHost
        _Folder = aFolder
        _UserName = aUserName
        _UserID = aUserID
        _Password = aPassword
    End Sub

    Public Sub DownLoadAllAttacments(ByVal aSaveAttachFolder As String)
        _SaveAttachToFolder = aSaveAttachFolder
        If _Host = "" OrElse _
           _UserName = "" OrElse _
           _UserID = "" OrElse _
           _Folder = "" OrElse _
           _Password = "" Then
            MsgBox("Host, User Name, User ID, Email Folder, and Password are required")
            Return
        End If
        Try
            ' TODO: -GBV check if this is a FBA or Windows.
            _CookieJar = DoFormBaseAuthentication()
            If _CookieJar IsNot Nothing AndAlso _CookieJar.Count >= 2 Then
                CheckMails()
            Else
                Throw New Exception("Authentication Failed")
            End If
        Catch ex As Exception
            ShowError("Error Downloading Attachments", ex)
        End Try
    End Sub

    Private Sub CheckMails()
        Dim Request As System.Net.HttpWebRequest
        Dim Response As System.Net.HttpWebResponse
        Dim RequestStream As Stream
        Dim ResponseStream As Stream
        Dim ResponseXmlDoc As XmlDocument
        Dim DisplayNameNodes As XmlNodeList
        Dim MessageName As String = ""
        Dim Body() As Byte
        Dim strRootURI As String = "https://" & _Host & "/Exchange/" & _UserName & "/" & _Folder
        Dim SQLStr As String = ""

        ' Prepare request object
        Request = GetRequestObject(strRootURI, "SEARCH")
        Request.CookieContainer.Add(_CookieJar)
        Request.KeepAlive = True
        Request.AllowAutoRedirect = True
        Request.ContentType = "text/xml"
        Request.Headers.Add("Translate", "F")

        ' Build the SQL query.
        SQLStr = "<?xml version=""1.0""?>" & _
                 "<D:searchrequest xmlns:D = ""DAV:"" >" & _
                 "<D:sql>SELECT ""DAV:displayname"" FROM """ & strRootURI & """" & _
                 "WHERE ""DAV:ishidden"" = false AND ""DAV:isfolder"" = false" & _
                 "</D:sql></D:searchrequest>"

        ' Encode the query string
        Body = _Encode.GetBytes(SQLStr)

        ' Set the content length
        Request.ContentLength = Body.Length

        ' Get a reference to the request stream
        RequestStream = Request.GetRequestStream()

        ' Write the message body to the request stream
        RequestStream.Write(Body, 0, Body.Length)
        RequestStream.Close()

        ' Send the SEARCH method request and get the
        ' response from the server.
        Response = CType(Request.GetResponse(), HttpWebResponse)

        ' Get the XML response stream.
        ResponseStream = Response.GetResponseStream()

        ' Create the XmlDocument object from the XML response stream.
        ResponseXmlDoc = New System.Xml.XmlDocument
        ResponseXmlDoc.Load(ResponseStream)

        ' Build a list of the DAV:href XML nodes
        DisplayNameNodes = ResponseXmlDoc.GetElementsByTagName("a:displayname")

        ' Cycle through the list
        If DisplayNameNodes.Count > 0 Then
            For i As Integer = 0 To DisplayNameNodes.Count - 1
                MessageName = DisplayNameNodes(i).InnerText
                MessageName = MessageName.Replace(" ", "%20")
                GetAttachmentList(strRootURI & MessageName)
            Next
        End If
        ResponseStream.Close()
    End Sub
    Private Function GetRequestObject(ByVal aUri As String, ByVal aMethod As String) As HttpWebRequest
        Dim webrequest As HttpWebRequest
        Dim Uri As Uri = New Uri(aUri)

        webrequest = CType(System.Net.WebRequest.Create(Uri), HttpWebRequest)
        webrequest.CookieContainer = New CookieContainer
        webrequest.Method = aMethod

        Return webrequest
    End Function

    Private Function DoFormBaseAuthentication() As CookieCollection
        Dim server As String
        Dim request As HttpWebRequest
        Dim response As HttpWebResponse
        Dim stream As Stream
        Dim Body() As Byte


        Try
            server = "https://" & _Host & "/exchweb/bin/auth/owaauth.dll"
            request = GetRequestObject(server, "POST")
            request.CookieContainer = New CookieContainer()
            request.ContentType = "application/x-www-form-urlencoded"
            request.KeepAlive = True
            request.AllowAutoRedirect = True

            Body = _Encode.GetBytes("destination=https://" & _
                                   _Host & "/Exchange/" & _
                                   _UserName & "/&username=" & _
                                   _Domain & "\" & _UserID & _
                                   "&password=" & _Password & _
                                   "&SubmitCreds=Log+On&forcedownlevel=0&trusted=0")

            request.ContentLength = Body.Length

            ' Send the request to the server
            stream = request.GetRequestStream()
            stream.Write(Body, 0, Body.Length)
            stream.Close()

            ' Get Response
            response = CType(request.GetResponse(), HttpWebResponse)

            Return response.Cookies

        Catch ex As Exception
            Return Nothing
        End Try

    End Function

    Private Sub GetAttachmentList(ByVal aURI As String)
        Dim Request As System.Net.HttpWebRequest
        Dim Response As System.Net.HttpWebResponse
        Dim ResponseStream As Stream
        Dim ResponseXmlDoc As XmlDocument
        Dim root As XmlNode
        Dim nsmgr As XmlNamespaceManager
        Dim PropstatNodes As XmlNodeList
        Dim HrefNodes As XmlNodeList
        Dim PropNode As XmlNode
        Dim AttachmentURI As String = ""
        Dim AttachmentName As String = ""
        Dim AttachmentSize As Integer = 0
        Dim AttachmentType As String = ""

        Request = GetRequestObject(aURI, "X-MS-ENUMATTS")
        Request.CookieContainer.Add(_CookieJar)
        Request.KeepAlive = True
        Request.AllowAutoRedirect = True
        Response = CType(Request.GetResponse(), HttpWebResponse)
        ResponseStream = Response.GetResponseStream()
        ResponseXmlDoc = New System.Xml.XmlDocument
        ResponseXmlDoc.Load(ResponseStream)
        root = ResponseXmlDoc.DocumentElement
        nsmgr = New XmlNamespaceManager(ResponseXmlDoc.NameTable)
        nsmgr.AddNamespace("a", "DAV:")
        nsmgr.AddNamespace("d", "http://schemas.microsoft.com/mapi/proptag/")
        nsmgr.AddNamespace("e", "urn:schemas:httpmail:")
        PropstatNodes = root.SelectNodes("//a:propstat", nsmgr)
        HrefNodes = root.SelectNodes("//a:href", nsmgr)
        If HrefNodes.Count > 0 Then
            For j As Integer = 0 To HrefNodes.Count - 1
                AttachmentURI = HrefNodes(j).InnerText
                PropNode = PropstatNodes(j).SelectSingleNode("a:prop/e:attachmentfilename", nsmgr)
                AttachmentName = PropNode.InnerText
                PropNode = PropstatNodes(j).SelectSingleNode("a:prop/d:x0e200003", nsmgr)
                AttachmentSize = CInt(PropNode.InnerText)
                PropNode = PropstatNodes(j).SelectSingleNode("a:prop/d:x370e001f", nsmgr)
                AttachmentType = PropNode.InnerText
                DownloadAttachment(AttachmentURI, AttachmentName, AttachmentSize, AttachmentType)

            Next
        End If
        ResponseStream.Close()
    End Sub

    Private Sub DownloadAttachment(ByVal aURI As String, ByVal aName As String, ByVal aSize As Integer, ByVal aType As String)
        Dim Request As System.Net.HttpWebRequest
        Dim Response As System.Net.HttpWebResponse
        Dim ResponseStream As Stream
        Dim IsText As Boolean
        Dim Len As Integer

        If aType = "text/plain" Then
            IsText = True
        Else
            IsText = False
        End If

        Dim Fs As New FileStream(_SaveAttachToFolder & aName, FileMode.Create)
        Dim Buffer(aSize) As Byte

        Request = GetRequestObject(aURI, "GET")
        Request.CookieContainer.Add(_CookieJar)
        Request.KeepAlive = True
        Request.AllowAutoRedirect = True
        Request.Headers.Add("Translate", "F")

        Response = CType(Request.GetResponse(), HttpWebResponse)
        ResponseStream = Response.GetResponseStream()

        If Not IsText Then
            Len = ResponseStream.Read(Buffer, 0, aSize)
            While Len > 0
                Fs.Write(Buffer, 0, Len)
                Len = ResponseStream.Read(Buffer, 0, aSize)
            End While
            ResponseStream.Close()
        Else
            Dim Sr As New StreamReader(ResponseStream)
            Buffer = _Encode.GetBytes(Sr.ReadToEnd)
            Fs.Write(Buffer, 0, Buffer.Length)
            Sr.Close()
            ResponseStream.Close()
        End If
        Fs.Flush()
        Fs.Close()
    End Sub

End Class
