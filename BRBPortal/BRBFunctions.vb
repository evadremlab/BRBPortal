
Imports System.IO
Imports System.Net
Imports Microsoft.AspNet.Identity.Owin
Imports System.Xml
Imports System.Text.RegularExpressions
Imports QSILib

Public Module BRBFunctions
    Public iStatus As String = ""
    Public iRelate As String = ""
    Public iErrMsg As String = ""
    Public iFirstlogin As String = ""
    Public iTempPwd As String = ""
    Public iPropertyTbl As New DataTable
    Public iUnitsTbl As New DataTable
    Public iTenantsTbl As New DataTable
    Public iPropAddr As String = ""
    Public iAgentName As String = ""
    Public iBillContact As String = ""
    Public iBillAddr As String = ""
    Public iBillEmail As String = ""

    Public Function UserAuth_Soap(ByVal aID As String, ByVal aBillCd As String, ByVal aPwd As String) As SignInStatus
        Dim Request As WebRequest
        Dim Response As WebResponse
        Dim DataStream As Stream
        Dim Reader As StreamReader
        Dim SoapByte() As Byte
        Dim SoapStr As String
        'Dim status As String = ""
        'Dim relate As String = ""
        'Dim errMsg As String = ""
        'Dim firstlogin As String = ""
        'Dim tempPwd As String = ""

        SoapStr = "<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:api=""http://cityofberkeley.info/RTS/ClientPortal/API"">"
        SoapStr += "<soapenv:Header/>"
        SoapStr += "<soapenv:Body>"
        SoapStr += "<api:authenticateUserLogin>"
        SoapStr += "<authenticateUserReq>"
        If aID.Length > 0 Then
            SoapStr += "<!--Optional:--><userId>" & ChgXMLChars(aID) & "</userId>"
        Else
            SoapStr += "<!--Optional:--><userId>?</userId>"
        End If
        If aBillCd.Length > 0 Then
            SoapStr += "<!--Optional:--><billingCode>" & aBillCd & "</billingCode>"
        Else
            SoapStr += "<!--Optional:--><billingCode>?</billingCode>"
        End If
        SoapStr += "<pwd>" & ChgXMLChars(aPwd) & "</pwd>"
        SoapStr += "</authenticateUserReq>"
        SoapStr += "</api:authenticateUserLogin>"
        SoapStr += "</soapenv:Body>"
        SoapStr += "</soapenv:Envelope>"

        Try
            SoapByte = System.Text.Encoding.UTF8.GetBytes(SoapStr)

            Request = WebRequest.Create("http://cobwmisdv2.berkeley.root:5555/ws/RTSClientPortalAPI.API.WSD.AuthenticateUser/RTSClientPortalAPI_API_WSD_AuthenticateUser_Port")
            Request.Headers.Add("SOAPAction", "RTSClientPortalAPI_API_WSD_AuthenticateUser_Binder_authenticateUserLogin")
            Request.ContentType = "text/xml; charset=utf-8"
            Request.ContentLength = SoapByte.Length
            Request.Method = "POST"

            DataStream = Request.GetRequestStream()
            DataStream.Write(SoapByte, 0, SoapByte.Length)
            DataStream.Close()

            Response = Request.GetResponse()
            DataStream = Response.GetResponseStream()
            Reader = New StreamReader(DataStream)
            Dim SD2Request As String = Reader.ReadToEnd()

            DataStream.Close()
            Reader.Close()
            Response.Close()

            'Set session variables from response
            Dim doc As New XmlDocument
            doc.LoadXml(SD2Request)

            For Each detail As XmlElement In doc.DocumentElement.GetElementsByTagName("authenticateUserRes")
                iStatus = detail.ChildNodes(0).InnerText
                If iStatus.ToUpper = "SUCCESS" Then
                    If detail.SelectSingleNode("relationship") IsNot Nothing Then
                        iRelate = detail.SelectSingleNode("relationship").InnerText
                    Else
                        iRelate = ""
                    End If
                    iErrMsg = detail.SelectSingleNode("errorMsg").InnerText
                    iFirstlogin = detail.SelectSingleNode("isFirstLogin").InnerText
                    iTempPwd = detail.SelectSingleNode("isTemporaryPwd").InnerText
                Else
                    iErrMsg = detail.SelectSingleNode("errorMsg").InnerText
                    iRelate = ""
                    iFirstlogin = ""
                    iTempPwd = ""
                End If
            Next detail

            If iStatus.ToUpper = "SUCCESS" Then
                Return SignInStatus.Success
            Else
                Return SignInStatus.Failure
            End If

        Catch ex As WebException
            'MsgBox(ex.ToString())
            iErrMsg = ex.ToString
            Return SignInStatus.Failure
        End Try

    End Function

    Public Function ConfirmProfile_Soap(ByVal aID As String, ByVal aBillCd As String, ByVal aInits As String) As Boolean
        Dim Request As WebRequest
        Dim Response As WebResponse
        Dim DataStream As Stream
        Dim Reader As StreamReader
        Dim SoapByte() As Byte
        Dim SoapStr As String


        SoapStr = "<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:api=""http://cityofberkeley.info/RTS/ClientPortal/API"">"
        SoapStr += "<soapenv:Header/>"
        SoapStr += "<soapenv:Body>"
        SoapStr += "<api:confirmProfileInformation>"
        SoapStr += "<profileConfirmationReq>"
        If aID.Length > 0 Then
            SoapStr += "<!--Optional:--><userId>" & aID & "</userId>"
        Else
            SoapStr += "<!--Optional:--><userId>?</userId>"
        End If
        If aBillCd.Length > 0 Then
            SoapStr += "<!--Optional:--><billingCode>" & aBillCd & "</billingCode>"
        Else
            SoapStr += "<!--Optional:--><billingCode>?</billingCode>"
        End If
        SoapStr += "<declarationInitial>" & aInits & "</declarationInitial>"
        SoapStr += "</profileConfirmationReq>"
        SoapStr += "</api:confirmProfileInformation>"
        SoapStr += "</soapenv:Body>"
        SoapStr += "</soapenv:Envelope>"

        Try
            SoapByte = System.Text.Encoding.UTF8.GetBytes(SoapStr)

            Request = WebRequest.Create("http://cobwmisdv2.berkeley.root:5555/ws/RTSClientPortalAPI.API.WSD.ConfirmUserProfileByDeclaration/RTSClientPortalAPI_API_WSD_ConfirmUserProfileByDeclaration_Porthttp://cobwmisdv2.berkeley.root:5555/ws/RTSClientPortalAPI.API.WSD.ConfirmUserProfileByDeclaration/RTSClientPortalAPI_API_WSD_ConfirmUserProfileByDeclaration_Porthttp://cobwmisdv2.berkeley.root:5555/ws/RTSClientPortalAPI.API.WSD.ConfirmUserProfileByDeclaration/RTSClientPortalAPI_API_WSD_ConfirmUserProfileByDeclaration_Port")

            Request.Headers.Add("SOAPAction", "RTSClientPortalAPI_API_WSD_ConfirmUserProfileByDeclaration_Binder_confirmProfileInformation")
            Request.ContentType = "text/xml; charset=utf-8"
            Request.ContentLength = SoapByte.Length
            Request.Method = "POST"

            DataStream = Request.GetRequestStream()
            DataStream.Write(SoapByte, 0, SoapByte.Length)
            DataStream.Close()

            Response = Request.GetResponse()
            DataStream = Response.GetResponseStream()
            Reader = New StreamReader(DataStream)
            Dim SD2Request As String = Reader.ReadToEnd()

            DataStream.Close()
            Reader.Close()
            Response.Close()

            'Set session variables from response
            Dim doc As New XmlDocument
            doc.LoadXml(SD2Request)

            For Each detail As XmlElement In doc.DocumentElement.GetElementsByTagName("response")
                iStatus = detail.SelectSingleNode("status").InnerText
                If iStatus.ToUpper <> "SUCCESS" Then
                    iErrMsg = detail.SelectSingleNode("errorMsg").InnerText
                End If
            Next detail

            If iStatus.ToUpper = "SUCCESS" Then
                Return True
            Else
                Return False
            End If

        Catch ex As WebException
            iErrMsg = ex.ToString
            'MsgBox(ex.ToString())
            Return False
        End Try

    End Function

    Public Function UpdatePassword_Soap(ByVal aID As String, ByVal aBillCd As String, ByVal aCurrentPWD As String,
                                        ByVal aNewPWD As String, ByVal aReTypePWD As String) As Boolean
        Dim Request As WebRequest
        Dim Response As WebResponse
        Dim DataStream As Stream
        Dim Reader As StreamReader
        Dim SoapByte() As Byte
        Dim SoapStr As String


        SoapStr = "<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:api=""http://cityofberkeley.info/RTS/ClientPortal/API"">"
        SoapStr += "<soapenv:Header/>"
        SoapStr += "<soapenv:Body>"
        SoapStr += "<api:updateUserPassword>"
        SoapStr += "<updateUserPwdReq>"
        If aID.Length > 0 Then
            SoapStr += "<!--Optional:--><userId>" & aID & "</userId>"
        Else
            SoapStr += "<!--Optional:--><userId>?</userId>"
        End If
        If aBillCd.Length > 0 Then
            SoapStr += "<!--Optional:--><billingCode>" & aBillCd & "</billingCode>"
        Else
            SoapStr += "<!--Optional:--><billingCode>?</billingCode>"
        End If
        SoapStr += "<currentPwd>" & aCurrentPWD & "</currentPwd>"
        SoapStr += "<newPwd>" & aNewPWD & "</newPwd>"
        SoapStr += "<retypeNewPwd>" & aReTypePWD & "</retypeNewPwd>"
        SoapStr += "</updateUserPwdReq>"
        SoapStr += "</api:updateUserPassword>"
        SoapStr += "</soapenv:Body>"
        SoapStr += "</soapenv:Envelope>"

        Try
            SoapByte = System.Text.Encoding.UTF8.GetBytes(SoapStr)

            Request = WebRequest.Create("http://cobwmisdv2.berkeley.root:5555/ws/RTSClientPortalAPI.API.WSD.UpdatePassword/RTSClientPortalAPI_API_WSD_UpdatePassword_Port")

            Request.Headers.Add("SOAPAction", "RTSClientPortalAPI_API_WSD_UpdatePassword_Binder_updateUserPassword")
            Request.ContentType = "text/xml; charset=utf-8"
            Request.ContentLength = SoapByte.Length
            Request.Method = "POST"

            DataStream = Request.GetRequestStream()
            DataStream.Write(SoapByte, 0, SoapByte.Length)
            DataStream.Close()

            Response = Request.GetResponse()
            DataStream = Response.GetResponseStream()
            Reader = New StreamReader(DataStream)
            Dim SD2Request As String = Reader.ReadToEnd()

            DataStream.Close()
            Reader.Close()
            Response.Close()

            'Set session variables from response
            Dim doc As New XmlDocument
            doc.LoadXml(SD2Request)

            For Each detail As XmlElement In doc.DocumentElement.GetElementsByTagName("response")
                iStatus = detail.ChildNodes(0).InnerText
                If iStatus.ToUpper <> "SUCCESS" Then
                    iErrMsg = detail.ChildNodes(1).InnerText
                End If
            Next detail

            If iStatus.ToUpper = "SUCCESS" Then
                Return True
            Else
                Return False
            End If

        Catch ex As WebException
            iErrMsg = ex.ToString
            'MsgBox(ex.ToString())
            Return False
        End Try

    End Function

    Public Function GetProfile_Soap(ByVal aID As String, ByVal aBillCd As String) As String
        Dim Request As WebRequest
        Dim Response As WebResponse
        Dim DataStream As Stream
        Dim Reader As StreamReader
        Dim SoapByte() As Byte
        Dim SoapStr As String
        Dim RetStr As String = ""
        Dim tUserCode, tBillCode, tFullName, tFirst, tMid, tLast, tMailAddr, tEmail, tPhone, tSuffix, wstr, tAgent As String
        Dim tAnswer1, tAnswer2, tQuestion1, tQuestion2, tSNum, tSName, tUnit, tFullAddr, tCity, tST, tZip, tCntry As String

        iStatus = ""
        tUserCode = ""
        tBillCode = ""
        tFullName = ""
        tFirst = ""
        tMid = ""
        tLast = ""
        tMailAddr = ""
        tSNum = ""
        tSName = ""
        tUnit = ""
        tFullAddr = ""
        tCity = ""
        tST = ""
        tZip = ""
        tCntry = ""
        tEmail = ""
        tPhone = ""
        tAnswer1 = ""
        tAnswer2 = ""
        tQuestion1 = ""
        tQuestion2 = ""
        tSuffix = ""
        tAgent = ""

        SoapStr = "<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:api=""http://cityofberkeley.info/RTS/ClientPortal/API"">"
        SoapStr += "<soapenv:Header/>"
        SoapStr += "<soapenv:Body>"
        SoapStr += "<api:getProfileDetails>"
        SoapStr += "<request>"
        If aID.Length > 0 Then
            SoapStr += "<!--Optional:--><userId>" & aID & "</userId>"
        Else
            SoapStr += "<!--Optional:--><userId>?</userId>"
        End If
        If aBillCd.Length > 0 Then
            SoapStr += "<!--Optional:--><billingCode>" & aBillCd & "</billingCode>"
        Else
            SoapStr += "<!--Optional:--><billingCode>?</billingCode>"
        End If
        SoapStr += "</request>"
        SoapStr += "</api:getProfileDetails>"
        SoapStr += "</soapenv:Body>"
        SoapStr += "</soapenv:Envelope>"

        Try
            SoapByte = System.Text.Encoding.UTF8.GetBytes(SoapStr)

            Request = WebRequest.Create("http://cobwmisdv2.berkeley.root:5555/ws/RTSClientPortalAPI.API.WSD.GetUserProfile/RTSClientPortalAPI_API_WSD_GetUserProfile_Port")

            Request.Headers.Add("SOAPAction", "RTSClientPortalAPI_API_WSD_GetUserProfile_Binder_getProfileDetails")
            Request.ContentType = "text/xml; charset=utf-8"
            Request.ContentLength = SoapByte.Length
            Request.Method = "POST"

            DataStream = Request.GetRequestStream()
            DataStream.Write(SoapByte, 0, SoapByte.Length)
            DataStream.Close()

            Response = Request.GetResponse()
            DataStream = Response.GetResponseStream()
            Reader = New StreamReader(DataStream)
            Dim SD2Request As String = Reader.ReadToEnd()

            DataStream.Close()
            Reader.Close()
            Response.Close()

            'Set session variables from response
            Dim doc As New XmlDocument
            doc.LoadXml(SD2Request)

            For Each detail As XmlElement In doc.DocumentElement.GetElementsByTagName("profileDetails")
                For Each detailName As XmlElement In doc.DocumentElement.GetElementsByTagName("name")
                    For Each detailAddr As XmlElement In doc.DocumentElement.GetElementsByTagName("mailingAddress")
                        iStatus = "SUCCESS"
                        tUserCode = detail.SelectSingleNode("userId").InnerText
                        tBillCode = detail.SelectSingleNode("billingCode").InnerText
                        tFirst = detailName.SelectSingleNode("first").InnerText
                        If detailName.SelectSingleNode("middle").InnerText.Length > 0 Then
                            tMid = detailName.SelectSingleNode("middle").InnerText
                        End If
                        tLast = detailName.SelectSingleNode("last").InnerText
                        If detailName.SelectSingleNode("suffix").InnerText.Length > 0 Then
                            tSuffix = detailName.SelectSingleNode("suffix").InnerText
                        End If
                        If detailName.SelectSingleNode("agencyName").InnerText.Length > 0 Then
                            tAgent = detailName.SelectSingleNode("agencyName").InnerText
                        End If

                        'Build Mailing address
                        tMailAddr = detailAddr.SelectSingleNode("streetNumber").InnerText
                        tMailAddr += " " & detailAddr.SelectSingleNode("streetName").InnerText
                        If detailAddr.SelectSingleNode("unitNumber").InnerText.Length > 0 Then
                            tMailAddr += ", " & detailAddr.SelectSingleNode("unitNumber").InnerText
                        End If
                        tMailAddr += ", " & detailAddr.SelectSingleNode("city").InnerText
                        tMailAddr += ", " & detailAddr.SelectSingleNode("state").InnerText
                        tMailAddr += " " & detailAddr.SelectSingleNode("zip").InnerText

                        'Save address components
                        tSNum = detailAddr.SelectSingleNode("streetNumber").InnerText
                        tSName = detailAddr.SelectSingleNode("streetName").InnerText
                        tUnit = detailAddr.SelectSingleNode("unitNumber").InnerText
                        tFullAddr = detailAddr.SelectSingleNode("fullAddress").InnerText
                        tCity = detailAddr.SelectSingleNode("city").InnerText
                        tST = detailAddr.SelectSingleNode("state").InnerText
                        tZip = detailAddr.SelectSingleNode("zip").InnerText
                        tCntry = detailAddr.SelectSingleNode("country").InnerText

                        tEmail = detail.SelectSingleNode("emailAddress").InnerText
                        tPhone = detail.SelectSingleNode("phone").InnerText
                        tQuestion1 = detail.SelectSingleNode("securityQuestion1").InnerText
                        tAnswer1 = detail.SelectSingleNode("securityAnswer1").InnerText
                        tQuestion2 = detail.SelectSingleNode("securityQuestion2").InnerText
                        tAnswer2 = detail.SelectSingleNode("securityAnswer2").InnerText
                    Next detailAddr
                Next detailName
            Next detail

            If iStatus.ToUpper = "SUCCESS" Then
                wstr = tFirst
                If tMid.Length > 0 Then
                    wstr += " " & tMid
                End If
                wstr += " " & tLast & " " & tSuffix

                StripXMLChars(tFirst)
                StripXMLChars(tMid)
                StripXMLChars(tLast)
                StripXMLChars(wstr)
                StripXMLChars(tAnswer1)
                StripXMLChars(tAnswer2)

                RetStr = "UserCode=" & tUserCode & "::BillingCode=" & tBillCode & "::FirstName=" & StripXMLChars(tFirst) &
                         "::MidName=" & tMid & "::LastName=" & StripXMLChars(tLast) & "::Suffix=" & tSuffix &
                         "::FullName=" & StripXMLChars(wstr) & "::MailAddr=" & StripXMLChars(tMailAddr) &
                         "::StNum=" & StripXMLChars(tSNum) & "::StName=" & StripXMLChars(tSName) &
                         "::Unit=" & tUnit & "::FullAddr=" & tFullAddr & "::City=" & tCity & "::State=" & tST &
                         "::Zip=" & tZip & "::Country=" & tCntry & "::Email=" & tEmail & "::Phone=" & tPhone &
                         "::Question1=" & tQuestion1 & "::Answer1=" & tAnswer1 & "::Question2=" & tQuestion2 &
                         "::Answer2=" & tAnswer2 & "::AgentName=" & StripXMLChars(tAgent)
                Return RetStr
            Else
                'Should be blank since the API failed
                iStatus = "FAILURE"
                RetStr = ""
                Return RetStr
            End If

        Catch ex As WebException
            iErrMsg = ex.ToString
            'MsgBox(ex.ToString())
            Return False
        End Try

    End Function

    Public Function UpdateProfile_Soap(ByVal aXML As String) As Boolean
        'The incoming XML should have the following structure:
        '   UserID          - User ID from screen
        '   Billing Code    - Billing code
        '   First Name      - 
        '   Middle Name     -
        '   Last Name       -
        '   Suffix          -   
        '   Name Display    -
        '   Street Number   -
        '   Street Name     -
        '   Unit Number     -
        '   Full Address    -
        '   City            -
        '   State           - 2-character state
        '   Zip             -
        '   Country         -
        '   Email Address   -
        '   Phone Number    -
        '   Question 1      -
        '   Answer 1        -
        '   Question 2      -
        '   Answer 2        -
        '   Agency Name     -
        '
        'Each field should be separated by a double-colon (::)

        Dim Request As WebRequest
        Dim Response As WebResponse
        Dim DataStream As Stream
        Dim Reader As StreamReader
        Dim SoapByte() As Byte
        Dim SoapStr As String
        Dim RetStr As String = ""
        Dim tUserCode, tBillCode, tFullName, tFirst, tMid, tLast, tMailAddr, tEmail, tPhone, tSuffix As String
        Dim tAnswer1, tAnswer2, tQuestion1, tQuestion2, tSNum, tSName, tUnit, tFullAddr As String
        Dim tCity, tST, tZip, tCntry, wstr, wstr2, tAgentName As String

        iStatus = ""
        tUserCode = ""
        tBillCode = ""
        tFullName = ""
        tFirst = ""
        tMid = ""
        tLast = ""
        tMailAddr = ""
        tSNum = ""
        tSName = ""
        tUnit = ""
        tFullAddr = ""
        tCity = ""
        tST = ""
        tZip = ""
        tCntry = ""
        tEmail = ""
        tPhone = ""
        tAnswer1 = ""
        tAnswer2 = ""
        tQuestion1 = ""
        tQuestion2 = ""
        tSuffix = ""
        tAgentName = ""

        wstr2 = aXML
        wstr = ParseStr(wstr2, "::")    'User Code
        If wstr.Length > 0 Then tUserCode = wstr
        wstr = ParseStr(wstr2, "::")    'Billing Code
        If wstr.Length > 0 Then tBillCode = wstr
        wstr = ParseStr(wstr2, "::")    'First Name
        If wstr.Length > 0 Then tFirst = wstr
        wstr = ParseStr(wstr2, "::")    'Middle Name
        If wstr.Length > 0 Then tMid = wstr
        wstr = ParseStr(wstr2, "::")    'Last Name
        If wstr.Length > 0 Then tLast = wstr
        wstr = ParseStr(wstr2, "::")    'Suffifx
        If wstr.Length > 0 Then tSuffix = wstr
        wstr = ParseStr(wstr2, "::")    'Name Display - blanks for now
        wstr = ParseStr(wstr2, "::")    'Street Number
        If wstr.Length > 0 Then tSNum = wstr
        wstr = ParseStr(wstr2, "::")    'Street Name
        If wstr.Length > 0 Then tSName = wstr
        wstr = ParseStr(wstr2, "::")    'Unit Number
        If wstr.Length > 0 Then tUnit = wstr
        wstr = ParseStr(wstr2, "::")    'Full Address
        If wstr.Length > 0 Then tMailAddr = wstr
        wstr = ParseStr(wstr2, "::")    'City
        If wstr.Length > 0 Then tCity = wstr
        wstr = ParseStr(wstr2, "::")    'State abbrev
        If wstr.Length > 0 Then tST = wstr
        wstr = ParseStr(wstr2, "::")    'Zip
        If wstr.Length > 0 Then tZip = wstr
        wstr = ParseStr(wstr2, "::")    'Country
        If wstr.Length > 0 Then tCntry = wstr
        wstr = ParseStr(wstr2, "::")    'Email
        If wstr.Length > 0 Then tEmail = wstr
        wstr = ParseStr(wstr2, "::")    'Phone Number
        If wstr.Length > 0 Then tPhone = wstr
        wstr = ParseStr(wstr2, "::")    'Question 1
        If wstr.Length > 0 Then tQuestion1 = wstr
        wstr = ParseStr(wstr2, "::")    'Answer 1
        If wstr.Length > 0 Then tAnswer1 = wstr
        wstr = ParseStr(wstr2, "::")    'Question 2
        If wstr.Length > 0 Then tQuestion2 = wstr
        wstr = ParseStr(wstr2, "::")    'Answer 2
        If wstr.Length > 0 Then tAnswer2 = wstr
        wstr = ParseStr(wstr2, "::")    'Agent Name
        If wstr.Length > 0 Then tAgentName = wstr

        SoapStr = "<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:api=""http://cityofberkeley.info/RTS/ClientPortal/API"">"
        SoapStr += "<soapenv:Header/>"
        SoapStr += "<soapenv:Body>"
        SoapStr += "<api:updateUserProfile>"
        SoapStr += "<updateUserProfileReq>"
        If tUserCode.Length > 0 Then
            SoapStr += "<!--Optional:--><userId>" & tUserCode & "</userId>"
        Else
            SoapStr += "<!--Optional:--><userId></userId>"
        End If
        If tBillCode.Length > 0 Then
            SoapStr += "<!--Optional:--><billingCode>" & tBillCode & "</billingCode>"
        Else
            SoapStr += "<!--Optional:--><billingCode></billingCode>"
        End If
        SoapStr += "<name>"
        SoapStr += "<first>" & ChgXMLChars(tFirst) & "</first>"
        If tMid.Length > 0 Then
            SoapStr += "<!--Optional:--><middle>" & tMid & "</middle>"
        Else
            SoapStr += "<!--Optional:--><middle></middle>"
        End If
        SoapStr += "<last>" & ChgXMLChars(tLast) & "</last>"
        SoapStr += "<suffix>" & tSuffix & "</suffix>"
        SoapStr += "<!--Optional:--><nameLastFirstDisplay></nameLastFirstDisplay>"
        SoapStr += "<!--Optional:--><agencyName>" & ChgXMLChars(tAgentName) & "</agencyName>"
        SoapStr += "</name>"
        SoapStr += "<mailingAddress>"
        SoapStr += "<!--Optional:--><streetNumber>" & tSNum & "</streetNumber>"
        SoapStr += "<!--Optional:--><streetName>" & ChgXMLChars(tSName) & "</streetName>"
        SoapStr += "<!--Optional:--><unitNumber>" & tUnit & "</unitNumber>"
        SoapStr += "<fullAddress>" & ChgXMLChars(tFullAddr) & "</fullAddress>"
        SoapStr += "<!--Optional:--><city>" & tCity & "</city>"
        SoapStr += "<!--Optional:--><state>" & tST & "</state>"
        SoapStr += "<!--Optional:--><zip>" & tZip & "</zip>"
        SoapStr += "<!--Optional:--><country>" & tCntry & "</country>"
        SoapStr += "</mailingAddress>"
        SoapStr += "<emailAddress>" & tEmail & "</emailAddress>"
        SoapStr += "<phone>" & tPhone & "</phone>"
        SoapStr += "<securityQuestion1>" & tQuestion1 & "</securityQuestion1>"
        SoapStr += "<securityAnswer1>" & ChgXMLChars(tAnswer1) & "</securityAnswer1>"
        SoapStr += "<securityQuestion2>" & tQuestion2 & "</securityQuestion2>"
        SoapStr += "<securityAnswer2>" & ChgXMLChars(tAnswer2) & "</securityAnswer2>"
        SoapStr += "</updateUserProfileReq>"
        SoapStr += "<isActive>Y</isActive>"
        SoapStr += "</api:updateUserProfile>"
        SoapStr += "</soapenv:Body>"
        SoapStr += "</soapenv:Envelope>"

        Try
            SoapByte = System.Text.Encoding.UTF8.GetBytes(SoapStr)

            Request = WebRequest.Create("http://cobwmisdv2.berkeley.root:5555/ws/RTSClientPortalAPI.API.WSD.UpdateUserProfile/RTSClientPortalAPI_API_WSD_UpdateUserProfile_Port")

            Request.Headers.Add("SOAPAction", "RTSClientPortalAPI_API_WSD_UpdateUserProfile_Binder_updateUserProfile")
            Request.ContentType = "text/xml; charset=utf-8"
            Request.ContentLength = SoapByte.Length
            Request.Method = "POST"

            DataStream = Request.GetRequestStream()
            DataStream.Write(SoapByte, 0, SoapByte.Length)
            DataStream.Close()

            Response = Request.GetResponse()
            DataStream = Response.GetResponseStream()
            Reader = New StreamReader(DataStream)
            Dim SD2Request As String = Reader.ReadToEnd()

            DataStream.Close()
            Reader.Close()
            Response.Close()

            'Set session variables from response
            Dim doc As New XmlDocument
            doc.LoadXml(SD2Request)

            For Each detail As XmlElement In doc.DocumentElement.GetElementsByTagName("response")
                iStatus = detail.ChildNodes(0).InnerText
                If iStatus.ToUpper = "FAILURE" Then
                    iErrMsg = detail.ChildNodes(2).InnerText
                End If
            Next detail

            'For Each detail As XmlElement In doc.DocumentElement.GetElementsByTagName("profileDetails")
            '    For Each detailName As XmlElement In doc.DocumentElement.GetElementsByTagName("name")
            '        For Each detailAddr As XmlElement In doc.DocumentElement.GetElementsByTagName("mailingAddress")
            '            iStatus = "SUCCESS"
            '        Next detailAddr
            '    Next detailName
            'Next detail

            If iStatus.ToUpper = "SUCCESS" Then
                Return True
            Else
                iStatus = "FAILURE"
                'RetStr = ""
                Return False
            End If

        Catch ex As WebException
            iErrMsg = ex.ToString
            'MsgBox(ex.ToString())
            Return False
        End Try

    End Function

    Public Function Register_Soap(ByVal aSoapStr As String) As Boolean
        Dim Request As WebRequest
        Dim Response As WebResponse
        Dim DataStream As Stream
        Dim Reader As StreamReader
        Dim SoapByte() As Byte
        'Dim SoapStr As String
        Dim status As String = ""
        Dim errMsg As String = ""

        'SoapStr = aSoapStr

        Try
            SoapByte = System.Text.Encoding.UTF8.GetBytes(aSoapStr)

            Request = WebRequest.Create("http://cobwmisdv2.berkeley.root:5555/ws/RTSClientPortalAPI.API.WSD.ValidateRegistrationRequest/RTSClientPortalAPI_API_WSD_ValidateRegistrationRequest_Port")

            Request.Headers.Add("SOAPAction", "RTSClientPortalAPI.API.WSD.ValidateRegistrationRequest")
            Request.ContentType = "text/xml; charset=utf-8"
            Request.ContentLength = SoapByte.Length
            Request.Method = "POST"

            DataStream = Request.GetRequestStream()
            DataStream.Write(SoapByte, 0, SoapByte.Length)
            DataStream.Close()

            Response = Request.GetResponse()
            DataStream = Response.GetResponseStream()
            Reader = New StreamReader(DataStream)
            Dim SD2Request As String = Reader.ReadToEnd()

            DataStream.Close()
            Reader.Close()
            Response.Close()

            'Set session variables from response
            Dim doc As New XmlDocument
            doc.LoadXml(SD2Request)

            For Each detail As XmlElement In doc.DocumentElement.GetElementsByTagName("response")
                iStatus = detail.ChildNodes(0).InnerText
                If iStatus <> "SUCCESS" Then
                    iErrMsg = detail.ChildNodes(1).InnerText
                End If
            Next detail

            If iStatus.ToUpper = "SUCCESS" Then
                Return True
            Else
                Return False
            End If

        Catch ex As WebException
            'MsgBox(ex.ToString())
            iErrMsg = ex.ToString
            Return False
        End Try

    End Function

    Public Function GetUserProperties_Soap(ByVal aID As String, ByVal aBillCd As String) As Boolean
        Dim Request As WebRequest
        Dim Response As WebResponse
        Dim DataStream As Stream
        Dim Reader As StreamReader
        Dim SoapByte() As Byte
        Dim SoapStr As String

        iStatus = ""

        'Build out the table if needed
        If iPropertyTbl.Columns.Count < 1 Then
            iPropertyTbl.Columns.Add("chkProp", GetType(String))
            iPropertyTbl.Columns.Add("PropertyID", GetType(String))
            iPropertyTbl.Columns.Add("MainAddr", GetType(String))
            iPropertyTbl.Columns.Add("CurrFees", GetType(Decimal))
            iPropertyTbl.Columns.Add("PriorFees", GetType(Decimal))
            iPropertyTbl.Columns.Add("CurrPenalty", GetType(Decimal))
            iPropertyTbl.Columns.Add("PriorPenalty", GetType(Decimal))
            iPropertyTbl.Columns.Add("Credits", GetType(Decimal))
            iPropertyTbl.Columns.Add("Balance", GetType(Decimal))
            iPropertyTbl.Columns.Add("btnUpdProp", GetType(String))
        End If

        'Clear out any properties from prior call
        If iPropertyTbl.Rows.Count > 0 Then iPropertyTbl.Clear()

        SoapStr = "<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:api=""http://cityofberkeley.info/RTS/ClientPortal/API"">"
        SoapStr += "<soapenv:Header/>"
        SoapStr += "<soapenv:Body>"
        SoapStr += "<api:getProfileProperties>"
        SoapStr += "<request>"
        If aID.Length > 0 Then
            SoapStr += "<!--Optional:--><userId>" & aID & "</userId>"
        Else
            SoapStr += "<!--Optional:--><userId>?</userId>"
        End If
        If aBillCd.Length > 0 Then
            SoapStr += "<!--Optional:--><billingCode>" & aBillCd & "</billingCode>"
        Else
            SoapStr += "<!--Optional:--><billingCode>?</billingCode>"
        End If
        SoapStr += "</request>"
        SoapStr += "</api:getProfileProperties>"
        SoapStr += "</soapenv:Body>"
        SoapStr += "</soapenv:Envelope>"

        Try
            SoapByte = System.Text.Encoding.UTF8.GetBytes(SoapStr)

            Request = WebRequest.Create("http://cobwmisdv2.berkeley.root:5555/ws/RTSClientPortalAPI.API.WSD.GetUserProfilePropertiesList/RTSClientPortalAPI_API_WSD_GetUserProfilePropertiesList_Port")

            Request.Headers.Add("SOAPAction", "RTSClientPortalAPI_API_WSD_GetUserProfilePropertiesList_Binder_getProfileProperties")
            Request.ContentType = "text/xml; charset=utf-8"
            Request.ContentLength = SoapByte.Length
            Request.Method = "POST"

            DataStream = Request.GetRequestStream()
            DataStream.Write(SoapByte, 0, SoapByte.Length)
            DataStream.Close()

            Response = Request.GetResponse()
            DataStream = Response.GetResponseStream()
            Reader = New StreamReader(DataStream)
            Dim SD2Request As String = Reader.ReadToEnd()

            DataStream.Close()
            Reader.Close()
            Response.Close()

            'Set session variables from response
            Dim doc As New XmlDocument
            doc.LoadXml(SD2Request)

            For Each detail As XmlElement In doc.DocumentElement.GetElementsByTagName("response")
                For Each detailProperty As XmlElement In detail.GetElementsByTagName("profileProperty")
                    For Each detailAmounts As XmlElement In detailProperty.GetElementsByTagName("balanceAmounts")
                        iStatus = detail.SelectSingleNode("status").InnerText
                        If iStatus.ToUpper = "FAILURE" Then
                            iErrMsg = detail.SelectSingleNode("errorMsg").InnerText
                        End If
                        Dim NR As DataRow = iPropertyTbl.NewRow()
                        NR.Item("PropertyID") = detailProperty.SelectSingleNode("propertyId").InnerText
                        NR.Item("MainAddr") = detailProperty.SelectSingleNode("address").InnerText
                        NR.Item("CurrFees") = detailAmounts.SelectSingleNode("currentFees").InnerText
                        NR.Item("PriorFees") = detailAmounts.SelectSingleNode("priorFees").InnerText
                        NR.Item("CurrPenalty") = detailAmounts.SelectSingleNode("currentPenalty").InnerText
                        NR.Item("PriorPenalty") = detailAmounts.SelectSingleNode("priorPenalties").InnerText
                        NR.Item("Credits") = detailAmounts.SelectSingleNode("credit").InnerText
                        NR.Item("Balance") = detailAmounts.SelectSingleNode("totalBalance").InnerText
                        iPropertyTbl.Rows.Add(NR)
                    Next detailAmounts
                Next detailProperty
            Next detail

            If iStatus.ToUpper = "SUCCESS" Then
                Return True
            Else
                iStatus = "FAILURE"
                Return False
            End If

        Catch ex As WebException
            iErrMsg = ex.ToString
            'MsgBox(ex.ToString())
            Return False
        End Try

    End Function

    Public Function GetPropertyUnits_Soap(ByVal aPropID As String, ByVal aID As String, ByVal aBillCd As String,
                                          Optional ByVal aUnitID As String = "") As String
        Dim Request As WebRequest
        Dim Response As WebResponse
        Dim DataStream As Stream
        Dim Reader As StreamReader
        Dim SoapByte() As Byte
        Dim SoapStr As String
        Dim tServices, tUnitInfo, tOccBy, tExempt, tStartDt As String

        iStatus = ""
        tUnitInfo = ""

        'Build out the table if needed
        If iUnitsTbl.Columns.Count < 1 Then
            iUnitsTbl.Columns.Add("chkUnit", GetType(Boolean))
            iUnitsTbl.Columns.Add("chkTenants", GetType(Boolean))
            iUnitsTbl.Columns.Add("UnitID", GetType(String))
            iUnitsTbl.Columns.Add("UnitNo", GetType(String))
            iUnitsTbl.Columns.Add("UnitStatID", GetType(String))
            iUnitsTbl.Columns.Add("UnitStatCode", GetType(String))
            iUnitsTbl.Columns.Add("CPUnitStatCode", GetType(String))
            iUnitsTbl.Columns.Add("CPUnitStatDisp", GetType(String))
            iUnitsTbl.Columns.Add("RentCeiling", GetType(Decimal))
            iUnitsTbl.Columns.Add("StartDt", GetType(Date))
            iUnitsTbl.Columns.Add("HServices", GetType(String))
        End If

        'Clear out any units from prior call
        If iUnitsTbl.Rows.Count > 0 Then iUnitsTbl.Clear()

        SoapStr = "<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:api=""http://cityofberkeley.info/RTS/ClientPortal/API"">"
        SoapStr += "<soapenv:Header/>"
        SoapStr += "<soapenv:Body>"
        SoapStr += "<api:getPropertyAndUnitDetails>"
        SoapStr += "<propertyId>" & aPropID & "</propertyId>"
        SoapStr += "<request>"
        If aID.Length > 0 Then
            SoapStr += "<!--Optional:--><userId>" & aID & "</userId>"
        Else
            SoapStr += "<!--Optional:--><userId>?</userId>"
        End If
        If aBillCd.Length > 0 Then
            SoapStr += "<!--Optional:--><billingCode>" & aBillCd & "</billingCode>"
        Else
            SoapStr += "<!--Optional:--><billingCode>?</billingCode>"
        End If
        SoapStr += "</request>"
        SoapStr += "</api:getPropertyAndUnitDetails>"
        SoapStr += "</soapenv:Body>"
        SoapStr += "</soapenv:Envelope>"

        Try
            SoapByte = System.Text.Encoding.UTF8.GetBytes(SoapStr)

            Request = WebRequest.Create("http://cobwmisdv2.berkeley.root:5555/ws/RTSClientPortalAPI.API.WSD.GetPropertyAndUnitDetails/RTSClientPortalAPI_API_WSD_GetPropertyAndUnitDetails_Port")

            Request.Headers.Add("SOAPAction", "RTSClientPortalAPI_API_WSD_GetPropertyAndUnitDetails_Binder_getPropertyAndUnitDetails")
            Request.ContentType = "text/xml; charset=utf-8"
            Request.ContentLength = SoapByte.Length
            Request.Method = "POST"

            DataStream = Request.GetRequestStream()
            DataStream.Write(SoapByte, 0, SoapByte.Length)
            DataStream.Close()

            Response = Request.GetResponse()
            DataStream = Response.GetResponseStream()
            Reader = New StreamReader(DataStream)
            Dim SD2Request As String = Reader.ReadToEnd()

            DataStream.Close()
            Reader.Close()
            Response.Close()

            'Set session variables from response
            Dim doc As New XmlDocument
            doc.LoadXml(SD2Request)

            For Each detail As XmlElement In doc.DocumentElement.GetElementsByTagName("propertyAndUnitsRes")
                iStatus = "SUCCESS"
                iBillAddr = ""
                iAgentName = ""
                iPropAddr = ""
                If detail.SelectSingleNode("address").SelectSingleNode("mainStreetAddress") IsNot Nothing Then
                    iPropAddr = detail.SelectSingleNode("address").SelectSingleNode("mainStreetAddress").InnerText
                End If

                If detail.SelectSingleNode("billingDetails").SelectSingleNode("billingAddress").SelectSingleNode("mainStreetAddress") IsNot Nothing Then
                    iBillAddr = detail.SelectSingleNode("billingDetails").SelectSingleNode("billingAddress").SelectSingleNode("mainStreetAddress").InnerText
                End If

                If detail.SelectSingleNode("agentDetails") IsNot Nothing Then
                    iAgentName = detail.SelectSingleNode("agentDetails").SelectSingleNode("agentContactName").SelectSingleNode("nameLastFirstDisplay").InnerText
                    iAgentName = ChgXMLChars(iAgentName)
                End If

                tUnitInfo = ""
                For Each detailUnits As XmlElement In detail.GetElementsByTagName("units")
                    tServices = ""
                    tOccBy = ""
                    Dim NR As DataRow = iUnitsTbl.NewRow()
                    NR.Item("chkUnit") = False
                    NR.Item("chkTenants") = False
                    NR.Item("UnitID") = detailUnits.SelectSingleNode("unitId").InnerText
                    NR.Item("UnitNo") = detailUnits.SelectSingleNode("unitNumber").InnerText
                    NR.Item("UnitStatID") = detailUnits.SelectSingleNode("unitStatusId").InnerText
                    NR.Item("UnitStatCode") = detailUnits.SelectSingleNode("unitStatusCode").InnerText
                    NR.Item("CPUnitStatCode") = detailUnits.SelectSingleNode("clientPortalUnitStatusCode").InnerText
                    If detailUnits.SelectSingleNode("rentCeiling").InnerText.Length > 0 Then
                        NR.Item("RentCeiling") = detailUnits.SelectSingleNode("rentCeiling").InnerText
                    Else
                        NR.Item("RentCeiling") = CDec(0.00)
                    End If
                    If detailUnits.SelectSingleNode("unitStatusAsOfDate") IsNot Nothing Then
                        If Not IsDBNull(detailUnits.SelectSingleNode("unitStatusAsOfDate").InnerText) Then
                            NR.Item("StartDt") = CDate(detailUnits.SelectSingleNode("unitStatusAsOfDate").InnerText)
                        End If
                    End If

                    For Each detailService As XmlElement In detailUnits.GetElementsByTagName("housingServices")
                        If tServices.Length > 0 Then
                            tServices += ", " & detailService.SelectSingleNode("serviceName").InnerText
                        Else
                            tServices = detailService.SelectSingleNode("serviceName").InnerText
                        End If
                    Next detailService

                    NR.Item("HServices") = tServices

                    Select Case NR.Item("UnitStatCode").ToString.ToUpper
                        Case "OOCC"
                            NR.Item("CPUnitStatDisp") = "Owner-Occupied"
                        Case "SEC8"
                            NR.Item("CPUnitStatDisp") = "Section 8"
                        Case "RENTED"
                            NR.Item("CPUnitStatDisp") = "Rented or Available for Rent"
                        Case "FREE"
                            NR.Item("CPUnitStatDisp") = "Rent-Free"
                        Case "NAR"
                            NR.Item("CPUnitStatDisp") = "Not Available for Rent"
                        Case "SPLUS"
                            NR.Item("CPUnitStatDisp") = "Shelter Plus"
                        Case "DUPLEX"
                            NR.Item("CPUnitStatDisp") = "Owner-occupied Duplex"
                        Case "COMM"
                            NR.Item("CPUnitStatDisp") = "Commercial"
                        Case "SHARED"
                            NR.Item("CPUnitStatDisp") = "Owner Shares Kit/Bath"
                        Case "MISC"
                            NR.Item("CPUnitStatDisp") = "Miscellaneous Exempt"
                    End Select

                    iUnitsTbl.Rows.Add(NR)

                    'If a unit was passed in fill out information if this is the correct unit
                    If aUnitID.Length > 0 AndAlso aUnitID = NR.Item("UnitID").ToString Then
                        tExempt = ""
                        tStartDt = ""
                        If NR.Item("StartDt").ToString IsNot Nothing And NR.Item("StartDt").ToString.Length > 0 Then
                            tStartDt = CDate(NR.Item("StartDt").ToString).ToString("MM/dd/yyyy")
                        End If

                        For Each detailOccBy As XmlElement In detailUnits.GetElementsByTagName("occupants")
                            If tOccBy.Length > 0 Then
                                tOccBy += ", " & detailOccBy.SelectSingleNode("name").SelectSingleNode("nameLastFirstDisplay").InnerText
                            Else
                                tOccBy = detailOccBy.SelectSingleNode("name").SelectSingleNode("nameLastFirstDisplay").InnerText
                            End If
                        Next detailOccBy

                        tOccBy = StripXMLChars(tOccBy)

                        Select Case NR.Item("UnitStatCode").ToString.ToUpper
                            Case "OOCC"
                                tExempt = "Owner-Occupied"
                            Case "SEC8"
                                tExempt = "Section 8"
                            Case "RENTED"
                                tExempt = "Rented or Available for Rent"
                            Case "FREE"
                                tExempt = "Rent-Free"
                            Case "NAR"
                                tExempt = "Not Available for Rent"
                            Case "SPLUS"
                                tExempt = "Shelter Plus"
                            Case "DUPLEX"
                                tExempt = "Owner-occupied Duplex"
                            Case "COMM"
                                tExempt = "Commercial"
                            Case "SHARED"
                                tExempt = "Owner Shares Kit/Bath"
                            Case "MISC"
                                tExempt = "Miscellaneous Exempt"
                        End Select

                        tUnitInfo = "CPStatus=" & NR.Item("CPUnitStatCode").ToString & "::ExReason=" & tExempt & "::StartDt=" &
                                    tStartDt & "::OccBy=" & tOccBy & "::UnitID=" & NR.Item("UnitID").ToString

                    End If
                Next detailUnits
            Next detail

            If iStatus.ToUpper <> "SUCCESS" Then
                iStatus = "FAILURE"
            End If

            If tUnitInfo.Length > 0 Then Return tUnitInfo
            Return iStatus

        Catch ex As WebException
            iErrMsg = ex.ToString
            'MsgBox(ex.ToString())
            Return "FAILURE"
        End Try

    End Function

    Public Function GetPropertyTenants_Soap(ByVal aPropID As String, ByVal aID As String, ByVal aBillCd As String,
                                            ByVal aUnitID As String) As String
        Dim Request As WebRequest
        Dim Response As WebResponse
        Dim DataStream As Stream
        Dim Reader As StreamReader
        Dim SoapByte() As Byte
        Dim SoapStr As String
        Dim tServices, tUnitInfo, tExempt, tStartDt, tPriorDt, tPriorReas, tSmokYN, tSmokDt, tInitRent As String
        Dim TenCnt As Integer

        iStatus = ""
        tUnitInfo = ""

        'Build out the table if needed
        If iTenantsTbl.Columns.Count < 1 Then
            iTenantsTbl.Columns.Add("TenantID", GetType(String))
            iTenantsTbl.Columns.Add("FirstName", GetType(String))
            iTenantsTbl.Columns.Add("LastName", GetType(String))
            iTenantsTbl.Columns.Add("DispName", GetType(String))
            iTenantsTbl.Columns.Add("PhoneNo", GetType(String))
            iTenantsTbl.Columns.Add("EmailAddr", GetType(String))
        End If

        'Clear out any units from prior call
        If iTenantsTbl.Rows.Count > 0 Then iTenantsTbl.Clear()

        SoapStr = "<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:api=""http://cityofberkeley.info/RTS/ClientPortal/API"">"
        SoapStr += "<soapenv:Header/>"
        SoapStr += "<soapenv:Body>"
        SoapStr += "<api:getPropertyAndUnitDetails>"
        SoapStr += "<propertyId>" & aPropID & "</propertyId>"
        SoapStr += "<request>"
        If aID.Length > 0 Then
            SoapStr += "<!--Optional:--><userId>" & aID & "</userId>"
        Else
            SoapStr += "<!--Optional:--><userId>?</userId>"
        End If
        If aBillCd.Length > 0 Then
            SoapStr += "<!--Optional:--><billingCode>" & aBillCd & "</billingCode>"
        Else
            SoapStr += "<!--Optional:--><billingCode>?</billingCode>"
        End If
        SoapStr += "</request>"
        SoapStr += "</api:getPropertyAndUnitDetails>"
        SoapStr += "</soapenv:Body>"
        SoapStr += "</soapenv:Envelope>"

        Try
            SoapByte = System.Text.Encoding.UTF8.GetBytes(SoapStr)

            Request = WebRequest.Create("http://cobwmisdv2.berkeley.root:5555/ws/RTSClientPortalAPI.API.WSD.GetPropertyAndUnitDetails/RTSClientPortalAPI_API_WSD_GetPropertyAndUnitDetails_Port")

            Request.Headers.Add("SOAPAction", "RTSClientPortalAPI_API_WSD_GetPropertyAndUnitDetails_Binder_getPropertyAndUnitDetails")
            Request.ContentType = "text/xml; charset=utf-8"
            Request.ContentLength = SoapByte.Length
            Request.Method = "POST"

            DataStream = Request.GetRequestStream()
            DataStream.Write(SoapByte, 0, SoapByte.Length)
            DataStream.Close()

            Response = Request.GetResponse()
            DataStream = Response.GetResponseStream()
            Reader = New StreamReader(DataStream)
            Dim SD2Request As String = Reader.ReadToEnd()

            DataStream.Close()
            Reader.Close()
            Response.Close()

            'Set session variables from response
            Dim doc As New XmlDocument
            doc.LoadXml(SD2Request)

            For Each detail As XmlElement In doc.DocumentElement.GetElementsByTagName("propertyAndUnitsRes")
                iStatus = "SUCCESS"
                iBillAddr = ""
                iAgentName = ""
                iPropAddr = ""
                iBillContact = ""
                iBillEmail = ""

                If detail.SelectSingleNode("address").SelectSingleNode("mainStreetAddress") IsNot Nothing Then
                    iPropAddr = detail.SelectSingleNode("address").SelectSingleNode("mainStreetAddress").InnerText
                End If

                If detail.SelectSingleNode("billingDetails").SelectSingleNode("billingAddress").SelectSingleNode("mainStreetAddress") IsNot Nothing Then
                    iBillAddr = detail.SelectSingleNode("billingDetails").SelectSingleNode("billingAddress").SelectSingleNode("mainStreetAddress").InnerText
                End If

                If detail.SelectSingleNode("ownerContactName").SelectSingleNode("nameLastFirstDisplay") IsNot Nothing Then
                    iBillContact = detail.SelectSingleNode("ownerContactName").SelectSingleNode("nameLastFirstDisplay").InnerText
                    iBillContact = StripXMLChars(iBillContact)
                End If

                If detail.SelectSingleNode("billingDetails").SelectSingleNode("contact").SelectSingleNode("emailAddress") IsNot Nothing Then
                    iBillEmail = detail.SelectSingleNode("billingDetails").SelectSingleNode("contact").SelectSingleNode("emailAddress").InnerText
                End If

                If detail.SelectSingleNode("agentDetails").SelectSingleNode("agentContactName") IsNot Nothing Then
                    iAgentName = detail.SelectSingleNode("agentDetails").SelectSingleNode("agentContactName").SelectSingleNode("nameLastFirstDisplay").InnerText
                    iAgentName = StripXMLChars(iAgentName)
                End If

                If iAgentName.Length < 1 Then
                    If detail.SelectSingleNode("agentDetails").SelectSingleNode("agencyName") IsNot Nothing Then
                        iAgentName = detail.SelectSingleNode("agentDetails").SelectSingleNode("agencyName").InnerText
                        iAgentName = StripXMLChars(iAgentName)
                    End If
                End If

                tUnitInfo = ""
                TenCnt = 0

                For Each detailUnits As XmlElement In detail.GetElementsByTagName("units")
                    tServices = ""

                    'Fill out information if this is the correct unit
                    If aUnitID = detailUnits.SelectSingleNode("unitId").InnerText Then
                        tExempt = ""
                        tStartDt = ""

                        If detailUnits.SelectSingleNode("tenancyStartDate") IsNot Nothing Then
                            If Not IsDBNull(detailUnits.SelectSingleNode("tenancyStartDate").InnerText) Then
                                tStartDt = CDate(detailUnits.SelectSingleNode("tenancyStartDate").InnerText)
                            End If
                        End If

                        For Each detailService As XmlElement In detailUnits.GetElementsByTagName("housingServices")
                            If tServices.Length > 0 Then
                                tServices += ", " & detailService.SelectSingleNode("serviceName").InnerText
                            Else
                                tServices = detailService.SelectSingleNode("serviceName").InnerText
                            End If
                        Next detailService

                        TenCnt = 0
                        If detailUnits.GetElementsByTagName("noOfOccupants").Item(0) IsNot Nothing Then
                            TenCnt = CInt(detailUnits.GetElementsByTagName("noOfOccupants").Item(0).InnerText)
                        End If

                        tInitRent = "0.00"
                        If detailUnits.GetElementsByTagName("initialRent").Item(0) IsNot Nothing Then
                            tInitRent = detailUnits.GetElementsByTagName("initialRent").Item(0).InnerText
                        End If

                        tPriorDt = ""
                        If detailUnits.GetElementsByTagName("datePriorTenancyEnded").Item(0) IsNot Nothing Then
                            tPriorDt = detailUnits.GetElementsByTagName("datePriorTenancyEnded").Item(0).InnerText
                        End If

                        tPriorReas = ""
                        If detailUnits.GetElementsByTagName("reasonPriorTenancyEnded").Item(0) IsNot Nothing Then
                            tPriorReas = detailUnits.GetElementsByTagName("reasonPriorTenancyEnded").Item(0).InnerText
                        End If

                        tSmokYN = ""
                        If detailUnits.GetElementsByTagName("smokingProhibitionInLeaseStatus").Item(0) IsNot Nothing Then
                            tSmokYN = detailUnits.GetElementsByTagName("smokingProhibitionInLeaseStatus").Item(0).InnerText
                        End If

                        tSmokDt = ""
                        If detailUnits.GetElementsByTagName("smokingProhibitionEffectiveDate").Item(0) IsNot Nothing Then
                            tSmokDt = detailUnits.GetElementsByTagName("smokingProhibitionEffectiveDate").Item(0).InnerText
                        End If

                        For Each detailOccBy As XmlElement In detailUnits.GetElementsByTagName("occupants")
                            Dim NR As DataRow = iTenantsTbl.NewRow()
                            NR.Item("TenantID") = detailOccBy.SelectSingleNode("occupantId").InnerText
                            NR.Item("FirstName") = detailOccBy.SelectSingleNode("name").SelectSingleNode("firstName").InnerText
                            NR.Item("LastName") = detailOccBy.SelectSingleNode("name").SelectSingleNode("lastName").InnerText
                            NR.Item("DispName") = detailOccBy.SelectSingleNode("name").SelectSingleNode("nameLastFirstDisplay").InnerText
                            NR.Item("PhoneNo") = detailOccBy.SelectSingleNode("contactInfo").SelectSingleNode("phoneNumber").InnerText
                            NR.Item("EmailAddr") = detailOccBy.SelectSingleNode("contactInfo").SelectSingleNode("emailAddress").InnerText
                            iTenantsTbl.Rows.Add(NR)
                        Next detailOccBy

                        tUnitInfo = "CPStatus=" & detailUnits.SelectSingleNode("clientPortalUnitStatusCode").InnerText &
                                    "::HServices=" & tServices & "::StartDt=" & tStartDt & "::NumTenants=" & TenCnt.ToString &
                                    "::SmokeYN=" & tSmokYN & "::SmokeDt=" & tSmokDt & "::InitRent=" & tInitRent &
                                    "::PriorEndDt=" & tPriorDt & "::TermReason=" & tPriorReas & "::OwnerName=" & iBillContact &
                                    "::AgentName=" & iAgentName & "::UnitID=" & detailUnits.SelectSingleNode("unitId").InnerText &
                                    "::OwnerEmail=" & iBillEmail

                    End If
                Next detailUnits
            Next detail

            If iStatus.ToUpper <> "SUCCESS" Then
                iStatus = "FAILURE"
            End If

            If tUnitInfo.Length > 0 Then Return tUnitInfo
            Return iStatus

        Catch ex As WebException
            iErrMsg = ex.ToString
            'MsgBox(ex.ToString())
            Return "FAILURE"
        End Try

    End Function

    Public Function SaveCart_Soap(ByVal aSoapStr As String) As Boolean
        Dim Request As WebRequest
        Dim Response As WebResponse
        Dim DataStream As Stream
        Dim Reader As StreamReader
        Dim SoapByte() As Byte
        'Dim SoapStr As String
        Dim status As String = ""
        Dim errMsg As String = ""

        'SoapStr = aSoapStr

        Try
            SoapByte = System.Text.Encoding.UTF8.GetBytes(aSoapStr)

            Request = WebRequest.Create("http://cobwmisdv2.berkeley.root:5555/ws/RTSClientPortalAPI.API.WSD.SavePaymentCartDetails/RTSClientPortalAPI_API_WSD_SavePaymentCartDetails_Port")

            Request.Headers.Add("SOAPAction", "RTSClientPortalAPI_API_WSD_SavePaymentCartDetails_Binder_savePaymentCart")
            Request.ContentType = "text/xml; charset=utf-8"
            Request.ContentLength = SoapByte.Length
            Request.Method = "POST"

            DataStream = Request.GetRequestStream()
            DataStream.Write(SoapByte, 0, SoapByte.Length)
            DataStream.Close()

            Response = Request.GetResponse()
            DataStream = Response.GetResponseStream()
            Reader = New StreamReader(DataStream)
            Dim SD2Request As String = Reader.ReadToEnd()

            DataStream.Close()
            Reader.Close()
            Response.Close()

            'Set session variables from response
            Dim doc As New XmlDocument
            doc.LoadXml(SD2Request)

            For Each detail As XmlElement In doc.DocumentElement.GetElementsByTagName("response")
                iStatus = detail.ChildNodes(0).InnerText
                If iStatus.ToUpper <> "SUCCESS" Then
                    iErrMsg = detail.ChildNodes(1).InnerText
                End If
            Next detail

            If iStatus.ToUpper = "SUCCESS" Then
                Return True
            Else
                Return False
            End If

        Catch ex As WebException
            iErrMsg = ex.ToString
            'MsgBox(ex.ToString())
            Return False
        End Try

    End Function

    Public Function SaveUnit_Soap(ByVal aSoapStr As String) As Boolean
        Dim Request As WebRequest
        Dim Response As WebResponse
        Dim DataStream As Stream
        Dim Reader As StreamReader
        Dim SoapByte() As Byte
        'Dim SoapStr As String
        Dim status As String = ""
        Dim errMsg As String = ""

        'SoapStr = aSoapStr

        Try
            SoapByte = System.Text.Encoding.UTF8.GetBytes(aSoapStr)

            Request = WebRequest.Create("http://cobwmisdv2.berkeley.root:5555/ws/RTSClientPortalAPI.API.WSD.UpdateUnitStatusChange/RTSClientPortalAPI_API_WSD_UpdateUnitStatusChange_Port")

            Request.Headers.Add("SOAPAction", "RTSClientPortalAPI_API_WSD_UpdateUnitStatusChange_Binder_updateUnitStatusChange")
            Request.ContentType = "text/xml; charset=utf-8"
            Request.ContentLength = SoapByte.Length
            Request.Method = "POST"

            DataStream = Request.GetRequestStream()
            DataStream.Write(SoapByte, 0, SoapByte.Length)
            DataStream.Close()

            Response = Request.GetResponse()
            DataStream = Response.GetResponseStream()
            Reader = New StreamReader(DataStream)
            Dim SD2Request As String = Reader.ReadToEnd()

            DataStream.Close()
            Reader.Close()
            Response.Close()

            'Set session variables from response
            Dim doc As New XmlDocument
            doc.LoadXml(SD2Request)

            For Each detail As XmlElement In doc.DocumentElement.GetElementsByTagName("response")
                iStatus = detail.ChildNodes(0).InnerText
                If iStatus.ToUpper <> "SUCCESS" Then
                    iErrMsg = detail.ChildNodes(1).InnerText
                End If
            Next detail

            If iStatus.ToUpper = "SUCCESS" Then
                Return True
            Else
                Return False
            End If

        Catch ex As WebException
            iErrMsg = ex.ToString
            'MsgBox(ex.ToString())
            Return False
        End Try

    End Function

    Public Function SaveTenant_Soap(ByVal aSoapStr As String) As Boolean
        Dim Request As WebRequest
        Dim Response As WebResponse
        Dim DataStream As Stream
        Dim Reader As StreamReader
        Dim SoapByte() As Byte
        'Dim SoapStr As String
        Dim status As String = ""
        Dim errMsg As String = ""

        'SoapStr = aSoapStr

        Try
            SoapByte = System.Text.Encoding.UTF8.GetBytes(aSoapStr)

            Request = WebRequest.Create("http://cobwmisdv2.berkeley.root:5555/ws/RTSClientPortalAPI.API.WSD.UpdateUnitTenancy/RTSClientPortalAPI_API_WSD_UpdateUnitTenancy_Port")

            Request.Headers.Add("SOAPAction", "RTSClientPortalAPI_API_WSD_UpdateUnitTenancy_Binder_updateUnitTenancy")
            Request.ContentType = "text/xml; charset=utf-8"
            Request.ContentLength = SoapByte.Length
            Request.Method = "POST"

            DataStream = Request.GetRequestStream()
            DataStream.Write(SoapByte, 0, SoapByte.Length)
            DataStream.Close()

            Response = Request.GetResponse()
            DataStream = Response.GetResponseStream()
            Reader = New StreamReader(DataStream)
            Dim SD2Request As String = Reader.ReadToEnd()

            DataStream.Close()
            Reader.Close()
            Response.Close()

            'Set session variables from response
            Dim doc As New XmlDocument
            doc.LoadXml(SD2Request)

            For Each detail As XmlElement In doc.DocumentElement.GetElementsByTagName("response")
                iStatus = detail.ChildNodes(0).InnerText
                If iStatus.ToUpper <> "SUCCESS" Then
                    iErrMsg = detail.ChildNodes(1).InnerText
                End If
            Next detail

            If iStatus.ToUpper = "SUCCESS" Then
                Return True
            Else
                Return False
            End If

        Catch ex As WebException
            iErrMsg = ex.ToString
            'MsgBox(ex.ToString())
            Return False
        End Try

    End Function

    Public Function ValidateReset_Soap(ByVal aSoapStr As String) As Boolean
        Dim Request As WebRequest
        Dim Response As WebResponse
        Dim DataStream As Stream
        Dim Reader As StreamReader
        Dim SoapByte() As Byte
        Dim SoapStr As String

        SoapStr = aSoapStr

        Try
            SoapByte = System.Text.Encoding.UTF8.GetBytes(SoapStr)

            Request = WebRequest.Create("http://cobwmisdv2.berkeley.root:5555/ws/RTSClientPortalAPI.API.WSD.ValidateResetPasswordRequest/RTSClientPortalAPI_API_WSD_ValidateResetPasswordRequest_Port")

            Request.Headers.Add("SOAPAction", "RTSClientPortalAPI_API_WSD_ValidateResetPasswordRequest_Binder_validateResetUserPassword")
            Request.ContentType = "text/xml; charset=utf-8"
            Request.ContentLength = SoapByte.Length
            Request.Method = "POST"

            DataStream = Request.GetRequestStream()
            DataStream.Write(SoapByte, 0, SoapByte.Length)
            DataStream.Close()

            Response = Request.GetResponse()
            DataStream = Response.GetResponseStream()
            Reader = New StreamReader(DataStream)
            Dim SD2Request As String = Reader.ReadToEnd()

            DataStream.Close()
            Reader.Close()
            Response.Close()

            'Set session variables from response
            Dim doc As New XmlDocument
            doc.LoadXml(SD2Request)

            For Each detail As XmlElement In doc.DocumentElement.GetElementsByTagName("response")
                iStatus = detail.ChildNodes(0).InnerText
                If iStatus.ToUpper <> "SUCCESS" Then
                    iErrMsg = detail.ChildNodes(1).InnerText
                End If
            Next detail

            If iStatus.ToUpper = "SUCCESS" Then
                Return True
            Else
                Return False
            End If


        Catch ex As WebException
            iErrMsg = ex.ToString
            Return False
        End Try

    End Function


    Public Function CheckPswdRules(ByVal aPwd As String, ByVal aUserID As String) As Boolean
        Dim RetValue As Boolean = False
        Dim tResult As Boolean = True
        Dim testB As Boolean
        Dim RegexObj As Regex = New Regex("[a-zA-Z0-9!@#$%^&_*]")

        'Test Length
        If aPwd.Length < 7 Or aPwd.Length > 20 Then tResult = False

        'Must contain one special character, one number and one character
        testB = False
        If RegexObj.IsMatch(aPwd) = True Then testB = True
        If testB = False Then tResult = False

        'UserID must not appear in password
        testB = False
        If aPwd.IndexOf(aUserID) > -1 Then testB = True
        If testB = True Then tResult = False

        RetValue = tResult

        Return RetValue
    End Function

    Public Function ChgXMLChars(ByVal aValue As String) As String
        Dim RetStr As String = aValue

        If aValue.Length < 1 Then Return aValue

        RetStr = Replace(RetStr, "&", "&amp;")       'Ampersand
        RetStr = Replace(RetStr, "<", "&lt;")        'Less than
        RetStr = Replace(RetStr, ">", "&gt;")        'Greater than

        Return RetStr

    End Function

    Public Function StripXMLChars(ByVal aValue As String) As String
        Dim RetStr As String = aValue

        If aValue.Length < 1 Then Return aValue

        RetStr = Replace(RetStr, "&amp;", "&")       'Ampersand
        RetStr = Replace(RetStr, "&lt;", "<")        'Less than
        RetStr = Replace(RetStr, "&gt;", ">")        'Greater than

        Return RetStr

    End Function

End Module
