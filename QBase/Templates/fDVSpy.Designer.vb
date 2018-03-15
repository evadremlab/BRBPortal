<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fDVSpy
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
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
        Me.cbDVList = New QSILib.Windows.Forms.qComboBox
        Me.gvList = New QSILib.Windows.Forms.qGVList
        CType(Me.gvList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cbDVList
        '
        Me.cbDVList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbDVList.FormattingEnabled = True
        Me.cbDVList.Location = New System.Drawing.Point(2, -1)
        Me.cbDVList.Name = "cbDVList"
        Me.cbDVList.Size = New System.Drawing.Size(252, 21)
        Me.cbDVList.TabIndex = 0
        '
        'gvList
        '
        Me.gvList._GVFoot = Nothing
        Me.gvList._ShowSelectionBar = True
        Me.gvList.AllowUserToAddRows = False
        Me.gvList.AllowUserToDeleteRows = False
        Me.gvList.AllowUserToOrderColumns = True
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black
        Me.gvList.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.gvList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None
        Me.gvList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.None
        Me.gvList.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.gvList.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.gvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gvList.Location = New System.Drawing.Point(2, 27)
        Me.gvList.MultiSelect = False
        Me.gvList.Name = "gvList"
        Me.gvList.ReadOnly = True
        Me.gvList.RowHeadersVisible = False
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black
        Me.gvList.RowsDefaultCellStyle = DataGridViewCellStyle3
        Me.gvList.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(180, Byte), Integer))
        Me.gvList.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black
        Me.gvList.RowTemplate.Height = 21
        Me.gvList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gvList.Size = New System.Drawing.Size(892, 447)
        Me.gvList.TabIndex = 1
        '
        'fDVSpy
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(892, 473)
        Me.Controls.Add(Me.gvList)
        Me.Controls.Add(Me.cbDVList)
        Me.Name = "fDVSpy"
        Me.Text = "fDVSpy"
        CType(Me.gvList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cbDVList As QSILib.Windows.Forms.qComboBox
    Friend WithEvents gvList As QSILib.Windows.Forms.qGVList
End Class
