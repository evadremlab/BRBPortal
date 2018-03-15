Namespace Windows.Forms

    Public Class fhMain

        Public OKToShow As Boolean = False

        Sub New(ByVal aFName As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()
            Dim HelpPath As String = gHelpPath & "\" & aFName & ".htm"

            If aFName.IndexOf(".mht") > -1 Then
                HelpPath = gHelpPath & "\" & aFName
            End If
            ' Add any initialization after the InitializeComponent() call.

            'BHS 9/29/09
            'If gHelpPath.Length <= 0 Then HelpPath = gHelpPath & "\" & aFName & ".htm" 'Old way

            If System.IO.File.Exists(HelpPath) Then

                Me.WebBrowser1.Url = New System.Uri(HelpPath, System.UriKind.Absolute)
                If aFName.IndexOf(".mht") > -1 Then
                    Me.Text = Mid(aFName, 1, aFName.IndexOf(".mht"))
                Else
                    Me.Text = aFName & " Help"
                End If

                Me.CenterToScreen()
                OKToShow = True
            Else
                If gMissingHelpMsg.Length > 0 Then
                    MsgBox(gMissingHelpMsg, MsgBoxStyle.Information, "Help Not Available")
                Else
                    ErrMsg("Help Documentation for " & aFName & " not found", "This help documentation has not yet been created. " & _
                           "You can select Support or NUI Buzz for further assistance.")
                End If

            End If

        End Sub




    End Class

End Namespace
