Imports System.Windows.Forms

' Quartet Text Display Box - to show multi-line display with no possibility of data entry
Namespace Windows.Forms
    <System.Serializable(),
 System.ComponentModel.DefaultBindingProperty("Text")>
    Public Class qTextDisplay
        Inherits qTextBox

        Public Sub New()
            ' Initialize DataGridView Properties
            Me.BorderStyle = System.Windows.Forms.BorderStyle.None  'No borders
            Me.BackColor = QBackColor   'Transparent
            Me.ForeColor = QForeColor   'Regular Foreground color

            Me.Multiline = True         'Allow long text to wrap

            Me.ReadOnly = True          'Users never change
            Me.Enabled = False
            Me.TabStop = False

            Me.Height = 20              'Standard Size
            Me.Width = 200

        End Sub

        '#Region " DataType Property "
        '        Private _DataTypeVar As DataTypeEnum = DataTypeEnum.Str
        '        <System.ComponentModel.Category("Data"), _
        '         System.ComponentModel.Description("Used to generate SQL to save reporting temp tables back to the database."), _
        '         System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
        '         System.ComponentModel.DefaultValue(GetType(DataTypeEnum), "Str")> _
        '        Public Overridable Property _DataType() As DataTypeEnum
        '            Get
        '                Return _DataTypeVar
        '            End Get
        '            Set(ByVal value As DataTypeEnum)
        '                _DataTypeVar = value
        '            End Set
        '        End Property
        '#End Region

    End Class

End Namespace
