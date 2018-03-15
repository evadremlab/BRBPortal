Imports System.Windows.Forms
Imports System.Drawing
Namespace Windows.Forms
    <System.ComponentModel.ComplexBindingProperties("DataSource", "DataMember")>
    Public Class qGVFoot
        Inherits qGVBase

#Region "New"
        Public Sub New()

            ' Initialize DataGridView Properties
            Me.AllowUserToAddRows = False
            Me.AllowUserToDeleteRows = False
            Me.AllowUserToOrderColumns = False

            Me.MultiSelect = False
            Me.ReadOnly = True
            Me.RowHeadersVisible = False
            Me.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            Me.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal
            Me.CellBorderStyle = DataGridViewCellBorderStyle.None
            Me.BorderStyle = System.Windows.Forms.BorderStyle.None

            Me.ColumnHeadersVisible = True
            Me.CellBorderStyle = DataGridViewCellBorderStyle.None
            Me.ColumnHeadersHeight = 4
            Me.ColumnHeadersDefaultCellStyle.BackColor = QReadOnlyBackColor
            Me.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            Me.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None

            Me.BackgroundColor = QDefaultRowBackColor
            Me.RowsDefaultCellStyle.BackColor = QDefaultRowBackColor
            Me.RowsDefaultCellStyle.ForeColor = QForeColor
            Me.RowTemplate.DefaultCellStyle.SelectionBackColor = QDefaultRowBackColor
            Dim F As New Font(System.Drawing.FontFamily.GenericSansSerif, 8.25, FontStyle.Bold, GraphicsUnit.Point)
            Me.DefaultCellStyle.Font = F
            Me.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
            Me.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
            Me.RowTemplate.Height = 21
            Me.BorderStyle = System.Windows.Forms.BorderStyle.None
            Me.Height = 36

        End Sub

#End Region

#Region "_GVMain Property "
        ' Defines the name of a Footer GV to associate with this GV.  Blank means no footer GV
        Private _GVMainVar As qGVBase = Nothing
        <System.ComponentModel.Category("Data"),
                System.ComponentModel.Description("Reference a Main GV (required)"),
                System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible),
                System.ComponentModel.DefaultValue("")>
        Public Overridable Property _GVMain() As qGVBase
            Get
                Return _GVMainVar
            End Get
            Set(ByVal value As qGVBase)
                _GVMainVar = value
            End Set
        End Property
#End Region

#Region "Events"
        'Scroll event in gvFooter
        Private Sub gvFooter_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles Me.Scroll
            If Me._GVMain IsNot Nothing Then
                If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                    Me._GVMain.HorizontalScrollingOffset = e.NewValue
                End If
            End If
        End Sub
#End Region

    End Class
End Namespace


