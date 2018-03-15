Imports System.Windows.Forms

' Quartet Text Box
Namespace Windows.Forms
    <System.Serializable(),
 System.ComponentModel.DefaultBindingProperty("Text")>
    Public Class qLabel
        Inherits System.Windows.Forms.Label

        Public Sub New()
            Me.AutoSize = True
            Me.AutoSize = False
            Me.Width = 100
            Me.Height = 21
            Me.TextAlign = ContentAlignment.MiddleRight
        End Sub

        '#Region " AutoSize property starts as False"   'BHS 6/27/08  Default True so New(), above, will get serialized (written to script)
        '    <System.ComponentModel.Category("Layout"), _
        '     System.ComponentModel.Description("Set to false to allow sizing of the label"), _
        '     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
        '     System.ComponentModel.DefaultValue(False)> _
        '    Public Overrides Property AutoSize() As Boolean
        '        Get
        '            Return MyBase.AutoSize
        '        End Get
        '        Set(ByVal value As Boolean)
        '            MyBase.AutoSize = value
        '        End Set
        '    End Property
        '#End Region

    End Class
End Namespace

