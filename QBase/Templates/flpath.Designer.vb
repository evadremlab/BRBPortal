<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class flpath
    Inherits QSILib.Windows.Forms.fBase

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
        Me.cbDrive = New QSILib.qComboBox
        Me.lblParentDir = New QSILib.qLabel
        Me.lblDocPath = New QSILib.qLabel
        Me.chViewLocalNetwork = New QSILib.qCheckBox
        Me.QLabel1 = New QSILib.qLabel
        Me.gvFiles = New QSILib.qGVList
        Me.DName = New System.Windows.Forms.DataGridViewLinkColumn
        Me.txtPath = New QSILib.qTextBox
        Me.btnAccept = New System.Windows.Forms.Button
        CType(Me.iDS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.iEP, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gvFiles, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cbDrive
        '
        Me.cbDrive._IsBrowseDD = False
        Me.cbDrive._IsKeyField = False
        Me.cbDrive._ValidateMaxValue = ""
        Me.cbDrive._ValidateMinValue = ""
        Me.cbDrive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbDrive.FormattingEnabled = True
        Me.cbDrive.Location = New System.Drawing.Point(45, 10)
        Me.cbDrive.Name = "cbDrive"
        Me.cbDrive.Size = New System.Drawing.Size(43, 21)
        Me.cbDrive.TabIndex = 105
        '
        'lblParentDir
        '
        Me.lblParentDir.AutoSize = True
        Me.lblParentDir.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblParentDir.ForeColor = System.Drawing.Color.Blue
        Me.lblParentDir.Location = New System.Drawing.Point(194, 13)
        Me.lblParentDir.Name = "lblParentDir"
        Me.lblParentDir.Size = New System.Drawing.Size(83, 13)
        Me.lblParentDir.TabIndex = 111
        Me.lblParentDir.Text = "Parent Directory"
        Me.lblParentDir.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblDocPath
        '
        Me.lblDocPath.AutoSize = True
        Me.lblDocPath.Location = New System.Drawing.Point(10, 37)
        Me.lblDocPath.Name = "lblDocPath"
        Me.lblDocPath.Size = New System.Drawing.Size(33, 13)
        Me.lblDocPath.TabIndex = 110
        Me.lblDocPath.Text = "Path*"
        Me.lblDocPath.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'chViewLocalNetwork
        '
        Me.chViewLocalNetwork._DataType = QSILib.qFunctions.DataTypeEnum.Str
        Me.chViewLocalNetwork._IsKeyField = False
        Me.chViewLocalNetwork.AutoSize = True
        Me.chViewLocalNetwork.Location = New System.Drawing.Point(376, 10)
        Me.chViewLocalNetwork.Name = "chViewLocalNetwork"
        Me.chViewLocalNetwork.Size = New System.Drawing.Size(121, 17)
        Me.chViewLocalNetwork.TabIndex = 108
        Me.chViewLocalNetwork.Text = "View Local Network"
        Me.chViewLocalNetwork.UseVisualStyleBackColor = True
        '
        'QLabel1
        '
        Me.QLabel1.AutoSize = True
        Me.QLabel1.Location = New System.Drawing.Point(7, 13)
        Me.QLabel1.Name = "QLabel1"
        Me.QLabel1.Size = New System.Drawing.Size(36, 13)
        Me.QLabel1.TabIndex = 107
        Me.QLabel1.Text = "Drive*"
        Me.QLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'gvFiles
        '
        Me.gvFiles._GVFoot = Nothing
        Me.gvFiles._ShowSelectionBar = True
        Me.gvFiles.AllowUserToAddRows = False
        Me.gvFiles.AllowUserToDeleteRows = False
        Me.gvFiles.AllowUserToOrderColumns = True
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black
        Me.gvFiles.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.gvFiles.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None
        Me.gvFiles.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.None
        Me.gvFiles.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.gvFiles.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.gvFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gvFiles.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DName})
        Me.gvFiles.Location = New System.Drawing.Point(10, 61)
        Me.gvFiles.MultiSelect = False
        Me.gvFiles.Name = "gvFiles"
        Me.gvFiles.ReadOnly = True
        Me.gvFiles.RowHeadersVisible = False
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black
        Me.gvFiles.RowsDefaultCellStyle = DataGridViewCellStyle3
        Me.gvFiles.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(180, Byte), Integer))
        Me.gvFiles.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black
        Me.gvFiles.RowTemplate.Height = 21
        Me.gvFiles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gvFiles.Size = New System.Drawing.Size(490, 415)
        Me.gvFiles.TabIndex = 109
        '
        'DName
        '
        Me.DName.DataPropertyName = "DName"
        Me.DName.HeaderText = "Directory/Folder"
        Me.DName.Name = "DName"
        Me.DName.ReadOnly = True
        Me.DName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.DName.Width = 470
        '
        'txtPath
        '
        Me.txtPath._BindDef = "Path"
        Me.txtPath._Format = ""
        Me.txtPath._FormatNumber = ""
        Me.txtPath._IsKeyField = False
        Me.txtPath._ToolTip = "Enter the path of the directory, or press the  [Browse] button."
        Me.txtPath._ValidateMaxValue = ""
        Me.txtPath._ValidateMinValue = ""
        Me.txtPath.Location = New System.Drawing.Point(45, 34)
        Me.txtPath.Name = "txtPath"
        Me.txtPath.Size = New System.Drawing.Size(452, 21)
        Me.txtPath.TabIndex = 106
        '
        'btnAccept
        '
        Me.btnAccept.Location = New System.Drawing.Point(217, 482)
        Me.btnAccept.Name = "btnAccept"
        Me.btnAccept.Size = New System.Drawing.Size(75, 23)
        Me.btnAccept.TabIndex = 112
        Me.btnAccept.Text = "Accept"
        Me.btnAccept.UseVisualStyleBackColor = True
        '
        'flpath
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(507, 511)
        Me.Controls.Add(Me.btnAccept)
        Me.Controls.Add(Me.cbDrive)
        Me.Controls.Add(Me.lblParentDir)
        Me.Controls.Add(Me.lblDocPath)
        Me.Controls.Add(Me.chViewLocalNetwork)
        Me.Controls.Add(Me.QLabel1)
        Me.Controls.Add(Me.gvFiles)
        Me.Controls.Add(Me.txtPath)
        Me.Name = "flpath"
        Me.Text = "Set Directory"
        CType(Me.iDS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.iEP, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gvFiles, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cbDrive As QSILib.qComboBox
    Friend WithEvents lblParentDir As QSILib.qLabel
    Friend WithEvents lblDocPath As QSILib.qLabel
    Friend WithEvents chViewLocalNetwork As QSILib.qCheckBox
    Friend WithEvents QLabel1 As QSILib.qLabel
    Friend WithEvents gvFiles As QSILib.qGVList
    Friend WithEvents txtPath As QSILib.qTextBox
    Friend WithEvents DName As System.Windows.Forms.DataGridViewLinkColumn
    Friend WithEvents btnAccept As System.Windows.Forms.Button
End Class
