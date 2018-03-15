Imports QSILib

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fSaveFile
    Inherits Windows.Forms.fBase

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
        Me.gvFiles = New QSILib.qGVList
        Me.DName = New System.Windows.Forms.DataGridViewLinkColumn
        Me.FName = New System.Windows.Forms.DataGridViewLinkColumn
        Me.cbDrive = New QSILib.qComboBox
        Me.lblParentDir = New QSILib.qLabel
        Me.lblDocPath = New QSILib.qLabel
        Me.chViewLocalNetwork = New QSILib.qCheckBox
        Me.QLabel1 = New QSILib.qLabel
        Me.txtDocPath = New QSILib.qTextBox
        Me.QLabel2 = New QSILib.qLabel
        Me.txtFileName = New QSILib.qTextBox
        Me.btnSave = New System.Windows.Forms.Button
        CType(Me.iDS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.iEP, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gvFiles, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
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
        Me.gvFiles.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DName, Me.FName})
        Me.gvFiles.Location = New System.Drawing.Point(59, 90)
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
        Me.gvFiles.Size = New System.Drawing.Size(452, 454)
        Me.gvFiles.TabIndex = 1
        '
        'DName
        '
        Me.DName.DataPropertyName = "DName"
        Me.DName.HeaderText = "Directory/Folder"
        Me.DName.Name = "DName"
        Me.DName.ReadOnly = True
        Me.DName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.DName.Width = 200
        '
        'FName
        '
        Me.FName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.FName.DataPropertyName = "FName"
        Me.FName.HeaderText = "File Name"
        Me.FName.Name = "FName"
        Me.FName.ReadOnly = True
        Me.FName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        '
        'cbDrive
        '
        Me.cbDrive._IsKeyField = False
        Me.cbDrive._ValidateMaxValue = ""
        Me.cbDrive._ValidateMinValue = ""
        Me.cbDrive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbDrive.FormattingEnabled = True
        Me.cbDrive.Location = New System.Drawing.Point(60, 12)
        Me.cbDrive.Name = "cbDrive"
        Me.cbDrive.Size = New System.Drawing.Size(43, 21)
        Me.cbDrive.TabIndex = 0
        '
        'lblParentDir
        '
        Me.lblParentDir.AutoSize = True
        Me.lblParentDir.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblParentDir.ForeColor = System.Drawing.Color.Blue
        Me.lblParentDir.Location = New System.Drawing.Point(209, 15)
        Me.lblParentDir.Name = "lblParentDir"
        Me.lblParentDir.Size = New System.Drawing.Size(83, 13)
        Me.lblParentDir.TabIndex = 110
        Me.lblParentDir.Text = "Parent Directory"
        Me.lblParentDir.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblDocPath
        '
        Me.lblDocPath.AutoSize = True
        Me.lblDocPath.Location = New System.Drawing.Point(25, 39)
        Me.lblDocPath.Name = "lblDocPath"
        Me.lblDocPath.Size = New System.Drawing.Size(33, 13)
        Me.lblDocPath.TabIndex = 108
        Me.lblDocPath.Text = "Path*"
        Me.lblDocPath.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'chViewLocalNetwork
        '
        Me.chViewLocalNetwork._DataType = QSILib.qFunctions.DataTypeEnum.Str
        Me.chViewLocalNetwork._IsKeyField = False
        Me.chViewLocalNetwork.AutoSize = True
        Me.chViewLocalNetwork.Location = New System.Drawing.Point(391, 12)
        Me.chViewLocalNetwork.Name = "chViewLocalNetwork"
        Me.chViewLocalNetwork.Size = New System.Drawing.Size(121, 17)
        Me.chViewLocalNetwork.TabIndex = 107
        Me.chViewLocalNetwork.Text = "View Local Network"
        Me.chViewLocalNetwork.UseVisualStyleBackColor = True
        '
        'QLabel1
        '
        Me.QLabel1.AutoSize = True
        Me.QLabel1.Location = New System.Drawing.Point(22, 15)
        Me.QLabel1.Name = "QLabel1"
        Me.QLabel1.Size = New System.Drawing.Size(36, 13)
        Me.QLabel1.TabIndex = 106
        Me.QLabel1.Text = "Drive*"
        Me.QLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtDocPath
        '
        Me.txtDocPath._BindDef = "DocPath"
        Me.txtDocPath._Format = ""
        Me.txtDocPath._FormatNumber = ""
        Me.txtDocPath._IsKeyField = False
        Me.txtDocPath._ToolTip = "Enter the path of the document to load, or press the  [Browse] button."
        Me.txtDocPath._ValidateMaxValue = ""
        Me.txtDocPath._ValidateMinValue = ""
        Me.txtDocPath.Location = New System.Drawing.Point(60, 36)
        Me.txtDocPath.Name = "txtDocPath"
        Me.txtDocPath.Size = New System.Drawing.Size(452, 21)
        Me.txtDocPath.TabIndex = 1
        '
        'QLabel2
        '
        Me.QLabel2.AutoSize = True
        Me.QLabel2.Location = New System.Drawing.Point(0, 63)
        Me.QLabel2.Name = "QLabel2"
        Me.QLabel2.Size = New System.Drawing.Size(58, 13)
        Me.QLabel2.TabIndex = 112
        Me.QLabel2.Text = "File Name*"
        Me.QLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtFileName
        '
        Me.txtFileName._Format = ""
        Me.txtFileName._FormatNumber = ""
        Me.txtFileName._IsKeyField = False
        Me.txtFileName._ToolTip = "Enter the path of the document to load, or press the  [Browse] button."
        Me.txtFileName._ValidateMaxValue = ""
        Me.txtFileName._ValidateMinValue = ""
        Me.txtFileName.Location = New System.Drawing.Point(60, 60)
        Me.txtFileName.Name = "txtFileName"
        Me.txtFileName.Size = New System.Drawing.Size(385, 21)
        Me.txtFileName.TabIndex = 2
        '
        'btnSave
        '
        Me.btnSave.Image = Global.QSILib.My.Resources.Resources.SAVE
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(451, 59)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(61, 23)
        Me.btnSave.TabIndex = 3
        Me.btnSave.Text = "Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'fSaveFile
        '
        Me.AcceptButton = Me.btnSave
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(523, 565)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.txtFileName)
        Me.Controls.Add(Me.QLabel2)
        Me.Controls.Add(Me.cbDrive)
        Me.Controls.Add(Me.lblParentDir)
        Me.Controls.Add(Me.lblDocPath)
        Me.Controls.Add(Me.chViewLocalNetwork)
        Me.Controls.Add(Me.QLabel1)
        Me.Controls.Add(Me.txtDocPath)
        Me.Controls.Add(Me.gvFiles)
        Me.Name = "fSaveFile"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.Text = "Save File"
        CType(Me.iDS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.iEP, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gvFiles, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents gvFiles As QSILib.qGVList
    Friend WithEvents DName As System.Windows.Forms.DataGridViewLinkColumn
    Friend WithEvents FName As System.Windows.Forms.DataGridViewLinkColumn
    Friend WithEvents cbDrive As QSILib.qComboBox
    Friend WithEvents lblParentDir As QSILib.qLabel
    Friend WithEvents lblDocPath As QSILib.qLabel
    Friend WithEvents chViewLocalNetwork As QSILib.qCheckBox
    Friend WithEvents QLabel1 As QSILib.qLabel
    Friend WithEvents txtDocPath As QSILib.qTextBox
    Friend WithEvents QLabel2 As QSILib.qLabel
    Friend WithEvents txtFileName As QSILib.qTextBox
    Friend WithEvents btnSave As System.Windows.Forms.Button

End Class
