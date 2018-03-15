Imports System.Windows.Forms
Imports System.Drawing
Namespace Windows.Forms
    <System.ComponentModel.ComplexBindingProperties("DataSource", "DataMember")>
    Public Class qGVList
        Inherits qGVBase

        Public Sub New()
            ' Initialize DataGridView Properties

            Me.AllowUserToAddRows = False
            Me.AllowUserToDeleteRows = False
            Me.AllowUserToOrderColumns = True

            Me.MultiSelect = False
            Me.ReadOnly = True
            Me.RowHeadersVisible = False
            Me.SelectionMode = DataGridViewSelectionMode.FullRowSelect

            Me.BackgroundColor = QListBackColor
            'Me.RowTemplate.DefaultCellStyle.BackColor = QListBackColor
            'Me.RowTemplate.DefaultCellStyle.ForeColor = Color.Black
            Me.ColumnHeadersDefaultCellStyle.BackColor = QListBackColor
            Me.RowsDefaultCellStyle.BackColor = QDefaultRowBackColor
            Me.RowsDefaultCellStyle.ForeColor = QForeColor
            Me.AlternatingRowsDefaultCellStyle.BackColor = QAltRowBackColor
            Me.AlternatingRowsDefaultCellStyle.ForeColor = QForeColor
            Me.RowTemplate.DefaultCellStyle.SelectionBackColor = QSelectionBackColor
            Me.RowTemplate.DefaultCellStyle.SelectionForeColor = QSelectionForeColor
            Me.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
            Me.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
            Me.RowTemplate.Height = 21

        End Sub

    End Class
End Namespace
