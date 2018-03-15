Imports System.Windows.Forms
Imports System.Drawing
Namespace Windows.Forms
    <System.ComponentModel.ComplexBindingProperties("DataSource", "DataMember")>
    Public Class qGVEdit
        Inherits qGVBase

        Public Sub New()
            ' Initialize DataGridView Properties

            Me.AllowUserToAddRows = False
            Me.AllowUserToDeleteRows = False
            Me.AllowUserToOrderColumns = True
            Me.RowTemplate.DefaultCellStyle.BackColor = QEntryBackColor 'Color.White
            Me.RowTemplate.DefaultCellStyle.ForeColor = QForeColor      'Color.Black
            Me.ColumnHeadersDefaultCellStyle.BackColor = QBackColor
            'System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
            Me.BackgroundColor = QBackColor
            'System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
            Me.RowTemplate.DefaultCellStyle.SelectionBackColor = QSelectionBackColor 'Color.AliceBlue
            Me.RowTemplate.DefaultCellStyle.SelectionForeColor = QSelectionForeColor 'Color.Black
            Me.MultiSelect = False
            Me.RowHeadersVisible = False
            Me.SelectionMode = DataGridViewSelectionMode.CellSelect
            Me.EditMode = DataGridViewEditMode.EditOnEnter
            Me.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None

        End Sub


#Region " BlankRowOnEmpty Property "
        ' Indicates whether to add a blank row if the GV is empty, while in edit mode.
        Private BlankRowOnEmptyVar As Boolean = True
        <System.ComponentModel.Category("Data"),
            System.ComponentModel.Description("True to make sure there is always at least a blank row in this gridview entry environment"),
            System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible),
            System.ComponentModel.DefaultValue("")>
        Public Overridable Property _BlankRowOnEmpty() As Boolean
            Get
                Return BlankRowOnEmptyVar
            End Get
            Set(ByVal value As Boolean)
                BlankRowOnEmptyVar = value
            End Set
        End Property
#End Region
    End Class
End Namespace
