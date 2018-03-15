' Remove Row Button that manages a gridview
Public Class qBtnRemoveGVRow
    Inherits System.Windows.Forms.Button

#Region "GV Property"
    Private _GridView As String = String.Empty
    <System.ComponentModel.Category("Data"), _
            System.ComponentModel.Description("The gridview that this button belongs to - Required."), _
            System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
            System.ComponentModel.DefaultValue("")> _
            Public Overridable Property _GVName() As String
        Get
            Return _GridView
        End Get
        Set(ByVal value As String)
            _GridView = value
        End Set
    End Property
#End Region

#Region "Events"
    Sub New()
        Me.Image = My.Resources.MINUS
        Me.Text = ""
        Me.Width = 15
        Me.Height = 12
        Me.ImageAlign = ContentAlignment.MiddleRight
        Me.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.FlatAppearance.BorderSize = 0
    End Sub
#End Region

End Class

