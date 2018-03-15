Namespace Windows.Forms



    'Child Entry Forms inherit from here.
    Public Class fuMain

#Region "---------------------------- Documentation ------------------------------"
        'Utility Ancestor WinForm
        '7/06 - Just the window attributes for now...

#End Region

        '''<summary> Load Utility progress form </summary>
        Private Sub fuMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            SetColorScheme()  'Default fore and backcolors
        End Sub


    End Class

End Namespace
