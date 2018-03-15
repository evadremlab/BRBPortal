Namespace Windows.Forms
    <System.Serializable()>
    Public Class qGVTextBoxColumn
        Inherits DataGridViewTextBoxColumn

#Region " ColumnFormat Property "
        Public _dataGridViewColumnFormat As ColumnFormatEnum = ColumnFormatEnum.None
        <System.ComponentModel.Category("Appearance"),
     System.ComponentModel.Description("The format to apply to all cells in the column."),
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible),
     System.ComponentModel.DefaultValue(GetType(ColumnFormatEnum), "None")>
        Public Overridable Property _ColumnFormat() As ColumnFormatEnum
            Get
                Return _dataGridViewColumnFormat
            End Get
            Set(ByVal value As ColumnFormatEnum)
                _dataGridViewColumnFormat = value
            End Set
        End Property
#End Region

#Region " DataType Property "
        Public _dataGridViewDataType As DataTypeEnum = DataTypeEnum.Str
        <System.ComponentModel.Category("Data"),
     System.ComponentModel.Description("Used to generate SQL to save reporting temp tables back to the database."),
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible),
     System.ComponentModel.DefaultValue(GetType(DataTypeEnum), "Str")>
        Public Overridable Property _DataType() As DataTypeEnum
            Get
                Return _dataGridViewDataType
            End Get
            Set(ByVal value As DataTypeEnum)
                _dataGridViewDataType = value
            End Set
        End Property
#End Region

        'May not need, since tabstop may be set to 0
        '#Region " Protect Property "
        '        Private _dataGridViewColumnProtect As Boolean = False
        '        <System.ComponentModel.Category("Behavior"), _
        '         System.ComponentModel.Description("Protect True prevents user from entering cell."), _
        '         System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
        '         System.ComponentModel.DefaultValue(False)> _
        '        Public Overridable Property ColumnProtect() As Boolean
        '            Get
        '                Return _dataGridViewColumnProtect
        '            End Get
        '            Set(ByVal value As Boolean)
        '                _dataGridViewColumnProtect = value
        '            End Set
        '        End Property
        '#End Region

#Region " TabStop "
        Private _dataGridViewColumnTabStop As Integer = 0
        <System.ComponentModel.Category("Behavior"),
     System.ComponentModel.Description("TabStop determines order cursor moves through row.  0 means cursor won't stop in this cell."),
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible),
     System.ComponentModel.DefaultValue(0)>
        Public Overridable Property _TabStop() As Integer
            Get
                Return _dataGridViewColumnTabStop
            End Get
            Set(ByVal value As Integer)
                _dataGridViewColumnTabStop = value
            End Set
        End Property
#End Region

#Region " ColumnTextCase Property "
        Private _dataGridViewColumnTextCase As TextCaseEnum = TextCaseEnum.Normal
        <System.ComponentModel.Category("Appearance"),
     System.ComponentModel.Description("Apply automatic character casing to all cells in the column."),
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible),
     System.ComponentModel.DefaultValue(GetType(TextCaseEnum), "Normal")>
        Public Overridable Property ColumnTextCase() As TextCaseEnum
            Get
                Return _dataGridViewColumnTextCase
            End Get
            Set(ByVal value As TextCaseEnum)
                _dataGridViewColumnTextCase = value
            End Set
        End Property
#End Region

#Region " Resizable Property "
        <System.ComponentModel.DefaultValue(GetType(System.Windows.Forms.DataGridViewTriState), "True")>
        Public Overrides Property Resizable() As System.Windows.Forms.DataGridViewTriState
            Get
                Return MyBase.Resizable
            End Get
            Set(ByVal value As System.Windows.Forms.DataGridViewTriState)
                MyBase.Resizable = value
            End Set
        End Property
#End Region

#Region " Clone Function "
        ' Required to Allow Custom Settings to Persist at Design Time
        Public Overrides Function Clone() As Object
            ' Declare and Instantiate Custom Column as Clone from MyBase
            Dim column As qGVTextBoxColumn = CType(MyBase.Clone, qGVTextBoxColumn)
            ' Set Custom Properties on Clone
            column._ColumnFormat = _dataGridViewColumnFormat
            column.ColumnTextCase = _dataGridViewColumnTextCase
            column._TabStop = _dataGridViewColumnTabStop
            column._DataType = _dataGridViewDataType
            ' Return Clone
            Return column
        End Function
#End Region

    End Class

End Namespace
