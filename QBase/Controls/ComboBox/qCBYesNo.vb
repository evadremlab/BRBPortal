Namespace Windows.Forms
    <System.ComponentModel.LookupBindingProperties("DataSource", "DisplayMember", "ValueMember", "SelectedValue")>
    Public Class qCBYesNo
        Inherits qComboBox

        Public Sub New()
            ' Fill ComboBox
            FillComboBox("=,Yes=Y,No=N", Me)
            Me.DropDownStyle = ComboBoxStyle.DropDownList
            Me.Width = 45

        End Sub

    End Class
End Namespace
