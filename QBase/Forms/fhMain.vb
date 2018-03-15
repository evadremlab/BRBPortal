Namespace Windows.Forms

    Public Class fhMain

        Public OKToShow As Boolean = False

        Sub New(ByVal aFName As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            Dim HelpPath As String = gHelpPath & "\" & aFName & ".htm"
            If gHelpPath.Length <= 0 Then HelpPath = gHelpPath & "\" & aFName & ".htm" 'Old way

            If System.IO.File.Exists(HelpPath) Then

                Me.WebBrowser1.Url = New System.Uri(HelpPath, System.UriKind.Absolute)
                Me.Text = aFName & " Help"
                Me.CenterToScreen()
                OKToShow = True
            Else
                If gMissingHelpMsg.Length > 0 Then
                    MsgBox(gMissingHelpMsg, MsgBoxStyle.Information, "Help Not Available")
                Else
                    ErrMsg("Help Documentation for " & aFName & " not found", "This help documentation has not yet been created.  Please notify Quartet at 650 343 0310 so we can make sure it gets added")
                End If

            End If

        End Sub


    End Class

End Namespace
