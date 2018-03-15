<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fShowTable
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.gvTable = New QSILib.Windows.Forms.qGVList
        Me.cbTables = New QSILib.Windows.Forms.qComboBox
        CType(Me.gvTable, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'gvTable
        '
        Me.gvTable._GVFoot = Nothing
        Me.gvTable._ShowSelectionBar = True
        Me.gvTable.AllowUserToAddRows = False
        Me.gvTable.AllowUserToDeleteRows = False
        Me.gvTable.AllowUserToOrderColumns = True
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black
        Me.gvTable.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.gvTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None
        Me.gvTable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.None
        Me.gvTable.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.gvTable.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.gvTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gvTable.Location = New System.Drawing.Point(0, 29)
        Me.gvTable.MultiSelect = False
        Me.gvTable.Name = "gvTable"
        Me.gvTable.ReadOnly = True
        Me.gvTable.RowHeadersVisible = False
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black
        Me.gvTable.RowsDefaultCellStyle = DataGridViewCellStyle3
        Me.gvTable.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.gvTable.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black
        Me.gvTable.RowTemplate.Height = 21
        Me.gvTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gvTable.Size = New System.Drawing.Size(1148, 420)
        Me.gvTable.TabIndex = 0
        '
        'cbTables
        '
        Me.cbTables._IsKeyField = False
        Me.cbTables.FormattingEnabled = True
        Me.cbTables.Location = New System.Drawing.Point(0, -1)
        Me.cbTables.Name = "cbTables"
        Me.cbTables.Size = New System.Drawing.Size(289, 24)
        Me.cbTables.TabIndex = 1
        '
        'fShowTable
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1145, 472)
        Me.Controls.Add(Me.cbTables)
        Me.Controls.Add(Me.gvTable)
        Me.Name = "fShowTable"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.Text = "ShowTable"
        CType(Me.gvTable, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gvTable As QSILib.Windows.Forms.qGVList
    Friend WithEvents cbTables As QSILib.Windows.Forms.qComboBox
End Class
