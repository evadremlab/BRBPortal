Public Class Prompt
    Public iAnswer As String = ""

    '''<summary> Pass window title and prompt text </summary>
    Sub New(ByVal aTitle As String, ByVal aPrompt As String)
        InitializeComponent()

        Me.Text = aTitle
        lblPrompt.Text = aPrompt

        'Move other controls based on length of prompt
        Me.Width = lblPrompt.Location.X + lblPrompt.Width + 20 + txtPrompt.Width + 40

        Dim P As Point
        P.X = lblPrompt.Location.X + lblPrompt.Width + 10
        P.Y = lblPrompt.Location.Y
        txtPrompt.Location = P
        P.Y += 37
        btnOK.Location = P
        P.X += btnOK.Width + 50
        btnCancel.Location = P

        Me.CenterToParent()

        txtPrompt.Focus()


    End Sub

    '''<summary> OK Clicked </summary>
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        iAnswer = txtPrompt.Text
        Close()
    End Sub

    '''<summary> Cancel clicked </summary>
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        iAnswer = ""
        Close()
    End Sub
End Class