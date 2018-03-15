Namespace Windows.Forms
    Public Class qDD

        Protected Event onTxtCode_Entered()
        Protected Event onTxtDescr_Entered()
        Protected Event onTxtCode_Validated()
        Protected Event onTxtDescr_Validated()
        Protected Event onTxtCode_GotFocus()
        Protected Event onTxtDescr_GotFocus()

        Private Sub txtCode_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCode.GotFocus
            RaiseEvent onTxtCode_GotFocus()
        End Sub

        Private Sub txtDescr_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtDescr.GotFocus
            RaiseEvent onTxtDescr_GotFocus()
        End Sub

        Private Sub txtCode_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtCode.KeyUp
            If e.KeyValue < 35 Or e.KeyValue > 40 Then RaiseEvent onTxtCode_Entered() 'Skip arrow keys
        End Sub

        Private Sub TextDescr_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TxtDescr.KeyUp
            If e.KeyValue < 35 Or e.KeyValue > 40 Then RaiseEvent onTxtDescr_Entered() 'Skip arrow keys
        End Sub

        Private Sub txtCode_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCode.Validated
            RaiseEvent onTxtCode_Validated()
        End Sub

        Private Sub TxtDescr_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtDescr.Validated
            RaiseEvent onTxtDescr_Validated()
        End Sub
    End Class
End Namespace

