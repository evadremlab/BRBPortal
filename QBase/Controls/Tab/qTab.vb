Imports System.Windows.Forms

' Quartet Tab
Namespace Windows.Forms
    <System.Serializable(),
 System.ComponentModel.DefaultBindingProperty("Text")>
    Public Class qTab
        Inherits System.Windows.Forms.TabControl

        ''' <summary> For each Page in a Tab Control, if a control has a _QueryDescr and it has text or a check,
        ''' then the page is marked dirty with a [...] to the right of the Page.Text </summary>
        Public Function RefreshPages() As Boolean

            For Each P As TabPage In TabPages
                Dim dirtycount As Integer = 0
                Dim PageIsDirty As Boolean = False
                For Each C As Control In P.Controls

                    Dim qT As qTextBox
                    qT = TryCast(C, qTextBox)
                    If qT IsNot Nothing Then
                        If qT._QueryDescr.Length > 0 And qT.Text.Length > 0 Then
                            PageIsDirty = True
                            Exit For
                        End If

                    End If

                    Dim qCB As qComboBox
                    qCB = TryCast(C, qComboBox)
                    If qCB IsNot Nothing Then
                        'BHS 5/13/11 add not-nothing check
                        If qCB._QueryDescr IsNot Nothing AndAlso qCB.Text IsNot Nothing Then
                            If qCB._QueryDescr.Length > 0 And qCB.Text.Length > 0 Then
                                PageIsDirty = True
                                Exit For
                            End If
                        End If

                    End If

                    Dim qCH As qCheckBox
                    qCH = TryCast(C, qCheckBox)
                    If qCH IsNot Nothing Then
                        If qCH._QueryDescr.Length > 0 And qCH.Checked = True Then
                            PageIsDirty = True
                            Exit For
                        End If
                    End If

                    Dim qMT As qMaskedTextBox
                    qMT = TryCast(C, qMaskedTextBox)
                    If qMT IsNot Nothing Then
                        If qMT._QueryDescr.Length > 0 And qMT.Text.Length > 0 Then
                            PageIsDirty = True
                            Exit For
                        End If
                    End If

                Next

                ShowPageIsDirty(P, PageIsDirty)

            Next


            Return True
        End Function

        'Put a [...] at the end of Page.Text if the page is dirty
        Sub ShowPageIsDirty(ByVal aP As TabPage, ByVal aPageIsDirty As Boolean)
            If aPageIsDirty = True Then
                If aP.Text.IndexOf("[...]") = -1 Then aP.Text &= " [...]"
            Else
                Dim i As Integer = aP.Text.IndexOf("[...]")
                If i > 0 Then aP.Text = Mid(aP.Text, 1, i)
            End If
        End Sub


    End Class
End Namespace
