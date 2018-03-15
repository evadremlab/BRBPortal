<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class feSaveQuery
    Inherits Windows.Forms.feMain

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.rbNew = New QSILib.qRB()
        Me.rbUpdate = New QSILib.qRB()
        Me.ddQueries = New QSILib.qDD()
        Me.QLabel1 = New QSILib.qLabel()
        Me.QLabel2 = New QSILib.qLabel()
        Me.QLabel3 = New QSILib.qLabel()
        Me.QLabel4 = New QSILib.qLabel()
        Me.txtTitle = New QSILib.qTextBox()
        Me.QLabel5 = New QSILib.qLabel()
        Me.QLabel6 = New QSILib.qLabel()
        Me.txtDescr = New QSILib.qTextBox()
        Me.QLabel8 = New QSILib.qLabel()
        Me.ddShared = New QSILib.qDD()
        Me.chkDefaultQuery = New QSILib.qCheckBox()
        Me.btnShowFiles = New System.Windows.Forms.Button()
        Me.gbRecurrence = New System.Windows.Forms.GroupBox()
        Me.pDaily = New System.Windows.Forms.Panel()
        Me.QLabel10 = New QSILib.qLabel()
        Me.QLabel7 = New QSILib.qLabel()
        Me.pYearly = New System.Windows.Forms.Panel()
        Me.ddMonth = New QSILib.qDD()
        Me.QLabel15 = New QSILib.qLabel()
        Me.ddDOM2 = New QSILib.qDD()
        Me.QLabel13 = New QSILib.qLabel()
        Me.QLabel14 = New QSILib.qLabel()
        Me.rbYearly = New QSILib.qRB()
        Me.rbMonthly = New QSILib.qRB()
        Me.rbWeekly = New QSILib.qRB()
        Me.rbDaily = New QSILib.qRB()
        Me.pWeekly = New System.Windows.Forms.Panel()
        Me.QLabel9 = New QSILib.qLabel()
        Me.cbAllDays = New QSILib.qCheckBox()
        Me.cbSaturday = New QSILib.qCheckBox()
        Me.cbFriday = New QSILib.qCheckBox()
        Me.cbThursday = New QSILib.qCheckBox()
        Me.cbWednesday = New QSILib.qCheckBox()
        Me.cbTuesday = New QSILib.qCheckBox()
        Me.cbMonday = New QSILib.qCheckBox()
        Me.cbSunday = New QSILib.qCheckBox()
        Me.pMonthly = New System.Windows.Forms.Panel()
        Me.ddDOM1 = New QSILib.qDD()
        Me.QLabel12 = New QSILib.qLabel()
        Me.QLabel11 = New QSILib.qLabel()
        Me.cbEmailResults = New QSILib.qCheckBox()
        Me.lblccusers = New QSILib.qLabel()
        Me.drCCUsers = New QSILib.qDR()
        Me.txtUserName = New QSILib.qTextBox()
        Me.ddCCuser = New QSILib.qDD()
        Me.lblCol1 = New QSILib.qLabel()
        Me.lblCol2 = New QSILib.qLabel()
        Me.btnRemoveCC = New System.Windows.Forms.Button()
        Me.btnInsertCC = New System.Windows.Forms.Button()
        Me.txtMenuName = New QSILib.qTextBox()
        Me.lblScreenResults = New QSILib.qLabel()
        Me.lblReportResults = New QSILib.qLabel()
        Me.gvResults = New QSILib.qGVEdit()
        Me.sel = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.title = New QSILib.qGVTextBoxColumn()
        Me.Panel1 = New System.Windows.Forms.Panel()
        CType(Me.iDS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.iEP, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbRecurrence.SuspendLayout()
        Me.pDaily.SuspendLayout()
        Me.pYearly.SuspendLayout()
        Me.pWeekly.SuspendLayout()
        Me.pMonthly.SuspendLayout()
        Me.drCCUsers.ItemTemplate.SuspendLayout()
        Me.drCCUsers.SuspendLayout()
        CType(Me.gvResults, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnNew
        '
        Me.btnNew.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnNew.Location = New System.Drawing.Point(330, 3)
        Me.ToolTip1.SetToolTip(Me.btnNew, "Clear this form so I can add new information (<Ctrl> E)")
        Me.btnNew.UseVisualStyleBackColor = False
        Me.btnNew.Visible = False
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(21, 143)
        Me.ToolTip1.SetToolTip(Me.btnDelete, "Permanently delete everything on this form (<Ctrl> D)")
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(21, 118)
        Me.ToolTip1.SetToolTip(Me.btnSave, "Save everything on this form (<Ctrl> S)")
        '
        'lblFTitle
        '
        Me.lblFTitle.Location = New System.Drawing.Point(795, 0)
        Me.lblFTitle.Size = New System.Drawing.Size(97, 19)
        Me.lblFTitle.Text = "Save Query"
        '
        'DirtyTimer
        '
        Me.DirtyTimer.Enabled = True
        '
        'rbNew
        '
        Me.rbNew.AutoSize = True
        Me.rbNew.Location = New System.Drawing.Point(51, 42)
        Me.rbNew.Name = "rbNew"
        Me.rbNew.Size = New System.Drawing.Size(112, 17)
        Me.rbNew.TabIndex = 981
        Me.rbNew.TabStop = True
        Me.rbNew.Text = "New Saved Query"
        Me.rbNew.UseVisualStyleBackColor = True
        '
        'rbUpdate
        '
        Me.rbUpdate.AutoSize = True
        Me.rbUpdate.Location = New System.Drawing.Point(51, 67)
        Me.rbUpdate.Name = "rbUpdate"
        Me.rbUpdate.Size = New System.Drawing.Size(143, 17)
        Me.rbUpdate.TabIndex = 982
        Me.rbUpdate.TabStop = True
        Me.rbUpdate.Text = "Update My Saved Query"
        Me.rbUpdate.UseVisualStyleBackColor = True
        '
        'ddQueries
        '
        Me.ddQueries._FindByNoSort = False
        Me.ddQueries._FindBySortAscending = True
        Me.ddQueries._Format = ""
        Me.ddQueries._IsKeyField = False
        Me.ddQueries._MustMatchList = True
        Me.ddQueries._MustMatchTime = 800
        Me.ddQueries._ToolTip = "Select an existing query to work with."
        Me.ddQueries.DataSource = Nothing
        Me.ddQueries.DisplayMember = ""
        Me.ddQueries.Location = New System.Drawing.Point(200, 65)
        Me.ddQueries.MaxLength = 32767
        Me.ddQueries.Name = "ddQueries"
        Me.ddQueries.SelectedIndex = -1
        Me.ddQueries.SelectedValue = Nothing
        Me.ddQueries.Size = New System.Drawing.Size(291, 21)
        Me.ddQueries.TabIndex = 983
        Me.ddQueries.TextInfo = ""
        Me.ddQueries.ValueMember = ""
        '
        'QLabel1
        '
        Me.QLabel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.QLabel1.Location = New System.Drawing.Point(11, 105)
        Me.QLabel1.Name = "QLabel1"
        Me.QLabel1.Size = New System.Drawing.Size(868, 1)
        Me.QLabel1.TabIndex = 985
        Me.QLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'QLabel2
        '
        Me.QLabel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.QLabel2.Location = New System.Drawing.Point(11, 507)
        Me.QLabel2.Name = "QLabel2"
        Me.QLabel2.Size = New System.Drawing.Size(868, 1)
        Me.QLabel2.TabIndex = 986
        Me.QLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'QLabel3
        '
        Me.QLabel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.QLabel3.Location = New System.Drawing.Point(11, 106)
        Me.QLabel3.Name = "QLabel3"
        Me.QLabel3.Size = New System.Drawing.Size(1, 401)
        Me.QLabel3.TabIndex = 987
        Me.QLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'QLabel4
        '
        Me.QLabel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.QLabel4.Location = New System.Drawing.Point(878, 106)
        Me.QLabel4.Name = "QLabel4"
        Me.QLabel4.Size = New System.Drawing.Size(1, 401)
        Me.QLabel4.TabIndex = 988
        Me.QLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtTitle
        '
        Me.txtTitle._Format = ""
        Me.txtTitle._FormatNumber = ""
        Me.txtTitle._IsKeyField = False
        Me.txtTitle._QueryDescr = "Title"
        Me.txtTitle._ToolTip = "Enter the title for the query."
        Me.txtTitle._ValidateMaxValue = ""
        Me.txtTitle._ValidateMinValue = ""
        Me.txtTitle._ValidateRequired = True
        Me.txtTitle.Location = New System.Drawing.Point(200, 118)
        Me.txtTitle.MaxLength = 60
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.Size = New System.Drawing.Size(291, 21)
        Me.txtTitle.TabIndex = 989
        '
        'QLabel5
        '
        Me.QLabel5.Location = New System.Drawing.Point(108, 118)
        Me.QLabel5.Name = "QLabel5"
        Me.QLabel5.Size = New System.Drawing.Size(86, 21)
        Me.QLabel5.TabIndex = 990
        Me.QLabel5.Text = "Title*"
        Me.QLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'QLabel6
        '
        Me.QLabel6.Location = New System.Drawing.Point(108, 143)
        Me.QLabel6.Name = "QLabel6"
        Me.QLabel6.Size = New System.Drawing.Size(86, 21)
        Me.QLabel6.TabIndex = 991
        Me.QLabel6.Text = "Description*"
        Me.QLabel6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtDescr
        '
        Me.txtDescr._Format = ""
        Me.txtDescr._FormatNumber = ""
        Me.txtDescr._IsKeyField = False
        Me.txtDescr._QueryDescr = "Description"
        Me.txtDescr._ToolTip = "Enter a description for this query."
        Me.txtDescr._ValidateMaxValue = ""
        Me.txtDescr._ValidateMinValue = ""
        Me.txtDescr._ValidateRequired = True
        Me.txtDescr.Location = New System.Drawing.Point(200, 143)
        Me.txtDescr.MaxLength = 2000
        Me.txtDescr.Multiline = True
        Me.txtDescr.Name = "txtDescr"
        Me.txtDescr.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDescr.Size = New System.Drawing.Size(291, 143)
        Me.txtDescr.TabIndex = 992
        '
        'QLabel8
        '
        Me.QLabel8.Location = New System.Drawing.Point(108, 291)
        Me.QLabel8.Name = "QLabel8"
        Me.QLabel8.Size = New System.Drawing.Size(86, 21)
        Me.QLabel8.TabIndex = 993
        Me.QLabel8.Text = "Share Option*"
        Me.QLabel8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ddShared
        '
        Me.ddShared._FindByNoSort = True
        Me.ddShared._FindBySortAscending = True
        Me.ddShared._Format = ""
        Me.ddShared._IsKeyField = False
        Me.ddShared._MustMatchList = True
        Me.ddShared._MustMatchTime = 800
        Me.ddShared._QueryDescr = "Shared Option"
        Me.ddShared._ToolTip = "Enter the share option for this query, from the list of choices."
        Me.ddShared._ValidateRequired = True
        Me.ddShared.DataSource = Nothing
        Me.ddShared.DisplayMember = ""
        Me.ddShared.Location = New System.Drawing.Point(200, 291)
        Me.ddShared.MaxLength = 32767
        Me.ddShared.Name = "ddShared"
        Me.ddShared.SelectedIndex = -1
        Me.ddShared.SelectedValue = Nothing
        Me.ddShared.Size = New System.Drawing.Size(123, 21)
        Me.ddShared.TabIndex = 996
        Me.ddShared.TextInfo = ""
        Me.ddShared.ValueMember = ""
        '
        'chkDefaultQuery
        '
        Me.chkDefaultQuery._DataType = QSILib.qFunctions.DataTypeEnum.Str
        Me.chkDefaultQuery._IsKeyField = False
        Me.chkDefaultQuery._QueryDescr = "Default Query Checkbox"
        Me.chkDefaultQuery._ToolTip = "Check this box if the query is your default query for this function."
        Me.chkDefaultQuery.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkDefaultQuery.Location = New System.Drawing.Point(348, 291)
        Me.chkDefaultQuery.Name = "chkDefaultQuery"
        Me.chkDefaultQuery.Size = New System.Drawing.Size(104, 20)
        Me.chkDefaultQuery.TabIndex = 999
        Me.chkDefaultQuery.Text = "Set As Default"
        Me.chkDefaultQuery.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkDefaultQuery.UseVisualStyleBackColor = True
        '
        'btnShowFiles
        '
        Me.btnShowFiles.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnShowFiles.Location = New System.Drawing.Point(21, 185)
        Me.btnShowFiles.Name = "btnShowFiles"
        Me.btnShowFiles.Size = New System.Drawing.Size(75, 23)
        Me.btnShowFiles.TabIndex = 1003
        Me.btnShowFiles.Text = "Show Files"
        Me.btnShowFiles.UseVisualStyleBackColor = False
        Me.btnShowFiles.Visible = False
        '
        'gbRecurrence
        '
        Me.gbRecurrence.Controls.Add(Me.pDaily)
        Me.gbRecurrence.Controls.Add(Me.pYearly)
        Me.gbRecurrence.Controls.Add(Me.rbYearly)
        Me.gbRecurrence.Controls.Add(Me.rbMonthly)
        Me.gbRecurrence.Controls.Add(Me.rbWeekly)
        Me.gbRecurrence.Controls.Add(Me.rbDaily)
        Me.gbRecurrence.Controls.Add(Me.pWeekly)
        Me.gbRecurrence.Controls.Add(Me.pMonthly)
        Me.gbRecurrence.Location = New System.Drawing.Point(32, 346)
        Me.gbRecurrence.Name = "gbRecurrence"
        Me.gbRecurrence.Size = New System.Drawing.Size(459, 142)
        Me.gbRecurrence.TabIndex = 1007
        Me.gbRecurrence.TabStop = False
        Me.gbRecurrence.Text = "Recurrence Pattern"
        '
        'pDaily
        '
        Me.pDaily.Controls.Add(Me.QLabel10)
        Me.pDaily.Controls.Add(Me.QLabel7)
        Me.pDaily.Location = New System.Drawing.Point(105, 15)
        Me.pDaily.Name = "pDaily"
        Me.pDaily.Size = New System.Drawing.Size(343, 116)
        Me.pDaily.TabIndex = 4
        '
        'QLabel10
        '
        Me.QLabel10.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.QLabel10.Location = New System.Drawing.Point(276, 10)
        Me.QLabel10.Name = "QLabel10"
        Me.QLabel10.Size = New System.Drawing.Size(52, 21)
        Me.QLabel10.TabIndex = 992
        Me.QLabel10.Text = "Daily"
        Me.QLabel10.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.QLabel10.Visible = False
        '
        'QLabel7
        '
        Me.QLabel7.Location = New System.Drawing.Point(9, 8)
        Me.QLabel7.Name = "QLabel7"
        Me.QLabel7.Size = New System.Drawing.Size(161, 21)
        Me.QLabel7.TabIndex = 0
        Me.QLabel7.Text = "Will run on all weekdays"
        Me.QLabel7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'pYearly
        '
        Me.pYearly.Controls.Add(Me.ddMonth)
        Me.pYearly.Controls.Add(Me.QLabel15)
        Me.pYearly.Controls.Add(Me.ddDOM2)
        Me.pYearly.Controls.Add(Me.QLabel13)
        Me.pYearly.Controls.Add(Me.QLabel14)
        Me.pYearly.Location = New System.Drawing.Point(105, 15)
        Me.pYearly.Name = "pYearly"
        Me.pYearly.Size = New System.Drawing.Size(343, 116)
        Me.pYearly.TabIndex = 1015
        '
        'ddMonth
        '
        Me.ddMonth._FindByNoSort = True
        Me.ddMonth._FindBySortAscending = True
        Me.ddMonth._Format = ""
        Me.ddMonth._IsKeyField = False
        Me.ddMonth._MustMatchList = True
        Me.ddMonth._MustMatchTime = 800
        Me.ddMonth._QueryDescr = "Yearly Recurrence Month"
        Me.ddMonth._ToolTip = "Choose the month for the yearly email."
        Me.ddMonth.DataSource = Nothing
        Me.ddMonth.DisplayMember = ""
        Me.ddMonth.Location = New System.Drawing.Point(99, 83)
        Me.ddMonth.MaxLength = 32767
        Me.ddMonth.Name = "ddMonth"
        Me.ddMonth.SelectedIndex = -1
        Me.ddMonth.SelectedValue = Nothing
        Me.ddMonth.Size = New System.Drawing.Size(79, 21)
        Me.ddMonth.TabIndex = 1000
        Me.ddMonth.TextInfo = ""
        Me.ddMonth.ValueMember = ""
        '
        'QLabel15
        '
        Me.QLabel15.Location = New System.Drawing.Point(7, 83)
        Me.QLabel15.Name = "QLabel15"
        Me.QLabel15.Size = New System.Drawing.Size(86, 21)
        Me.QLabel15.TabIndex = 999
        Me.QLabel15.Text = "Month*"
        Me.QLabel15.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ddDOM2
        '
        Me.ddDOM2._FindByNoSort = True
        Me.ddDOM2._FindBySortAscending = True
        Me.ddDOM2._Format = ""
        Me.ddDOM2._IsKeyField = False
        Me.ddDOM2._MustMatchList = True
        Me.ddDOM2._MustMatchTime = 800
        Me.ddDOM2._QueryDescr = "Yearly Day of Month"
        Me.ddDOM2._ToolTip = "Choose the day of month for the yearly email."
        Me.ddDOM2.DataSource = Nothing
        Me.ddDOM2.DisplayMember = ""
        Me.ddDOM2.Location = New System.Drawing.Point(281, 83)
        Me.ddDOM2.MaxLength = 32767
        Me.ddDOM2.Name = "ddDOM2"
        Me.ddDOM2.SelectedIndex = -1
        Me.ddDOM2.SelectedValue = Nothing
        Me.ddDOM2.Size = New System.Drawing.Size(48, 21)
        Me.ddDOM2.TabIndex = 998
        Me.ddDOM2.TextInfo = ""
        Me.ddDOM2.ValueMember = ""
        '
        'QLabel13
        '
        Me.QLabel13.Location = New System.Drawing.Point(189, 83)
        Me.QLabel13.Name = "QLabel13"
        Me.QLabel13.Size = New System.Drawing.Size(86, 21)
        Me.QLabel13.TabIndex = 997
        Me.QLabel13.Text = "Day of Month*"
        Me.QLabel13.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'QLabel14
        '
        Me.QLabel14.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.QLabel14.Location = New System.Drawing.Point(276, 10)
        Me.QLabel14.Name = "QLabel14"
        Me.QLabel14.Size = New System.Drawing.Size(52, 21)
        Me.QLabel14.TabIndex = 992
        Me.QLabel14.Text = "Yearly"
        Me.QLabel14.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.QLabel14.Visible = False
        '
        'rbYearly
        '
        Me.rbYearly.AutoSize = True
        Me.rbYearly.Location = New System.Drawing.Point(26, 100)
        Me.rbYearly.Name = "rbYearly"
        Me.rbYearly.Size = New System.Drawing.Size(55, 17)
        Me.rbYearly.TabIndex = 3
        Me.rbYearly.TabStop = True
        Me.rbYearly.Text = "Yearly"
        Me.rbYearly.UseVisualStyleBackColor = True
        '
        'rbMonthly
        '
        Me.rbMonthly.AutoSize = True
        Me.rbMonthly.Location = New System.Drawing.Point(26, 75)
        Me.rbMonthly.Name = "rbMonthly"
        Me.rbMonthly.Size = New System.Drawing.Size(63, 17)
        Me.rbMonthly.TabIndex = 2
        Me.rbMonthly.TabStop = True
        Me.rbMonthly.Text = "Monthly"
        Me.rbMonthly.UseVisualStyleBackColor = True
        '
        'rbWeekly
        '
        Me.rbWeekly.AutoSize = True
        Me.rbWeekly.Location = New System.Drawing.Point(26, 50)
        Me.rbWeekly.Name = "rbWeekly"
        Me.rbWeekly.Size = New System.Drawing.Size(60, 17)
        Me.rbWeekly.TabIndex = 1
        Me.rbWeekly.TabStop = True
        Me.rbWeekly.Text = "Weekly"
        Me.rbWeekly.UseVisualStyleBackColor = True
        '
        'rbDaily
        '
        Me.rbDaily.AutoSize = True
        Me.rbDaily.Location = New System.Drawing.Point(26, 25)
        Me.rbDaily.Name = "rbDaily"
        Me.rbDaily.Size = New System.Drawing.Size(48, 17)
        Me.rbDaily.TabIndex = 0
        Me.rbDaily.TabStop = True
        Me.rbDaily.Text = "Daily"
        Me.rbDaily.UseVisualStyleBackColor = True
        '
        'pWeekly
        '
        Me.pWeekly.Controls.Add(Me.QLabel9)
        Me.pWeekly.Controls.Add(Me.cbAllDays)
        Me.pWeekly.Controls.Add(Me.cbSaturday)
        Me.pWeekly.Controls.Add(Me.cbFriday)
        Me.pWeekly.Controls.Add(Me.cbThursday)
        Me.pWeekly.Controls.Add(Me.cbWednesday)
        Me.pWeekly.Controls.Add(Me.cbTuesday)
        Me.pWeekly.Controls.Add(Me.cbMonday)
        Me.pWeekly.Controls.Add(Me.cbSunday)
        Me.pWeekly.Location = New System.Drawing.Point(105, 15)
        Me.pWeekly.Name = "pWeekly"
        Me.pWeekly.Size = New System.Drawing.Size(343, 116)
        Me.pWeekly.TabIndex = 1012
        '
        'QLabel9
        '
        Me.QLabel9.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.QLabel9.Location = New System.Drawing.Point(276, 10)
        Me.QLabel9.Name = "QLabel9"
        Me.QLabel9.Size = New System.Drawing.Size(52, 21)
        Me.QLabel9.TabIndex = 991
        Me.QLabel9.Text = "Weekly"
        Me.QLabel9.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.QLabel9.Visible = False
        '
        'cbAllDays
        '
        Me.cbAllDays._DataType = QSILib.qFunctions.DataTypeEnum.Str
        Me.cbAllDays._IsKeyField = False
        Me.cbAllDays._QueryDescr = "Weekly Every Day"
        Me.cbAllDays._ToolTip = "Check this box if email should be sent weekly on every day."
        Me.cbAllDays.AutoSize = True
        Me.cbAllDays.Location = New System.Drawing.Point(152, 85)
        Me.cbAllDays.Name = "cbAllDays"
        Me.cbAllDays.Size = New System.Drawing.Size(72, 17)
        Me.cbAllDays.TabIndex = 7
        Me.cbAllDays.Text = "ALL DAYS"
        Me.cbAllDays.UseVisualStyleBackColor = True
        '
        'cbSaturday
        '
        Me.cbSaturday._DataType = QSILib.qFunctions.DataTypeEnum.Str
        Me.cbSaturday._IsKeyField = False
        Me.cbSaturday._QueryDescr = "Weekly Each Saturday"
        Me.cbSaturday._ToolTip = "Check this box if email should be sent weekly each Saturday."
        Me.cbSaturday.AutoSize = True
        Me.cbSaturday.Location = New System.Drawing.Point(152, 60)
        Me.cbSaturday.Name = "cbSaturday"
        Me.cbSaturday.Size = New System.Drawing.Size(70, 17)
        Me.cbSaturday.TabIndex = 6
        Me.cbSaturday.Text = "Saturday"
        Me.cbSaturday.UseVisualStyleBackColor = True
        '
        'cbFriday
        '
        Me.cbFriday._DataType = QSILib.qFunctions.DataTypeEnum.Str
        Me.cbFriday._IsKeyField = False
        Me.cbFriday._QueryDescr = "Weekly Each Friday"
        Me.cbFriday._ToolTip = "Check this box if email should be sent weekly each Friday."
        Me.cbFriday.AutoSize = True
        Me.cbFriday.Location = New System.Drawing.Point(152, 35)
        Me.cbFriday.Name = "cbFriday"
        Me.cbFriday.Size = New System.Drawing.Size(56, 17)
        Me.cbFriday.TabIndex = 5
        Me.cbFriday.Text = "Friday"
        Me.cbFriday.UseVisualStyleBackColor = True
        '
        'cbThursday
        '
        Me.cbThursday._DataType = QSILib.qFunctions.DataTypeEnum.Str
        Me.cbThursday._IsKeyField = False
        Me.cbThursday._QueryDescr = "Weekly Each Thursday"
        Me.cbThursday._ToolTip = "Check this box if email should be sent weekly each Thursday."
        Me.cbThursday.AutoSize = True
        Me.cbThursday.Location = New System.Drawing.Point(152, 10)
        Me.cbThursday.Name = "cbThursday"
        Me.cbThursday.Size = New System.Drawing.Size(71, 17)
        Me.cbThursday.TabIndex = 4
        Me.cbThursday.Text = "Thursday"
        Me.cbThursday.UseVisualStyleBackColor = True
        '
        'cbWednesday
        '
        Me.cbWednesday._DataType = QSILib.qFunctions.DataTypeEnum.Str
        Me.cbWednesday._IsKeyField = False
        Me.cbWednesday._QueryDescr = "Weekly Each Wednesday"
        Me.cbWednesday._ToolTip = "Check this box if email should be sent weekly each Wednesday."
        Me.cbWednesday.AutoSize = True
        Me.cbWednesday.Location = New System.Drawing.Point(36, 85)
        Me.cbWednesday.Name = "cbWednesday"
        Me.cbWednesday.Size = New System.Drawing.Size(83, 17)
        Me.cbWednesday.TabIndex = 3
        Me.cbWednesday.Text = "Wednesday"
        Me.cbWednesday.UseVisualStyleBackColor = True
        '
        'cbTuesday
        '
        Me.cbTuesday._DataType = QSILib.qFunctions.DataTypeEnum.Str
        Me.cbTuesday._IsKeyField = False
        Me.cbTuesday._QueryDescr = "Weekly Each Tuesday"
        Me.cbTuesday._ToolTip = "Check this box if email should be sent weekly each Tuesday."
        Me.cbTuesday.AutoSize = True
        Me.cbTuesday.Location = New System.Drawing.Point(36, 60)
        Me.cbTuesday.Name = "cbTuesday"
        Me.cbTuesday.Size = New System.Drawing.Size(67, 17)
        Me.cbTuesday.TabIndex = 2
        Me.cbTuesday.Text = "Tuesday"
        Me.cbTuesday.UseVisualStyleBackColor = True
        '
        'cbMonday
        '
        Me.cbMonday._DataType = QSILib.qFunctions.DataTypeEnum.Str
        Me.cbMonday._IsKeyField = False
        Me.cbMonday._QueryDescr = "Weekly Each Monday"
        Me.cbMonday._ToolTip = "Check this box if email should be sent weekly each Monday."
        Me.cbMonday.AutoSize = True
        Me.cbMonday.Location = New System.Drawing.Point(36, 35)
        Me.cbMonday.Name = "cbMonday"
        Me.cbMonday.Size = New System.Drawing.Size(64, 17)
        Me.cbMonday.TabIndex = 1
        Me.cbMonday.Text = "Monday"
        Me.cbMonday.UseVisualStyleBackColor = True
        '
        'cbSunday
        '
        Me.cbSunday._DataType = QSILib.qFunctions.DataTypeEnum.Str
        Me.cbSunday._IsKeyField = False
        Me.cbSunday._QueryDescr = "Weekly Each Sunday"
        Me.cbSunday._ToolTip = "Check this box if email should be sent weekly each Sunday."
        Me.cbSunday.AutoSize = True
        Me.cbSunday.Location = New System.Drawing.Point(36, 10)
        Me.cbSunday.Name = "cbSunday"
        Me.cbSunday.Size = New System.Drawing.Size(62, 17)
        Me.cbSunday.TabIndex = 0
        Me.cbSunday.Text = "Sunday"
        Me.cbSunday.UseVisualStyleBackColor = True
        '
        'pMonthly
        '
        Me.pMonthly.Controls.Add(Me.ddDOM1)
        Me.pMonthly.Controls.Add(Me.QLabel12)
        Me.pMonthly.Controls.Add(Me.QLabel11)
        Me.pMonthly.Location = New System.Drawing.Point(105, 15)
        Me.pMonthly.Name = "pMonthly"
        Me.pMonthly.Size = New System.Drawing.Size(343, 116)
        Me.pMonthly.TabIndex = 1012
        '
        'ddDOM1
        '
        Me.ddDOM1._FindByNoSort = True
        Me.ddDOM1._FindBySortAscending = True
        Me.ddDOM1._Format = ""
        Me.ddDOM1._IsKeyField = False
        Me.ddDOM1._MustMatchList = True
        Me.ddDOM1._MustMatchTime = 800
        Me.ddDOM1._QueryDescr = "Monthly Day of Month"
        Me.ddDOM1._ToolTip = "Choose the day of month for the monthly email."
        Me.ddDOM1.DataSource = Nothing
        Me.ddDOM1.DisplayMember = ""
        Me.ddDOM1.Location = New System.Drawing.Point(99, 58)
        Me.ddDOM1.MaxLength = 32767
        Me.ddDOM1.Name = "ddDOM1"
        Me.ddDOM1.SelectedIndex = -1
        Me.ddDOM1.SelectedValue = Nothing
        Me.ddDOM1.Size = New System.Drawing.Size(48, 21)
        Me.ddDOM1.TabIndex = 1000
        Me.ddDOM1.TextInfo = ""
        Me.ddDOM1.ValueMember = ""
        '
        'QLabel12
        '
        Me.QLabel12.Location = New System.Drawing.Point(7, 58)
        Me.QLabel12.Name = "QLabel12"
        Me.QLabel12.Size = New System.Drawing.Size(86, 21)
        Me.QLabel12.TabIndex = 999
        Me.QLabel12.Text = "Day of Month*"
        Me.QLabel12.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'QLabel11
        '
        Me.QLabel11.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.QLabel11.Location = New System.Drawing.Point(276, 10)
        Me.QLabel11.Name = "QLabel11"
        Me.QLabel11.Size = New System.Drawing.Size(52, 21)
        Me.QLabel11.TabIndex = 992
        Me.QLabel11.Text = "Monthly"
        Me.QLabel11.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.QLabel11.Visible = False
        '
        'cbEmailResults
        '
        Me.cbEmailResults._DataType = QSILib.qFunctions.DataTypeEnum.Str
        Me.cbEmailResults._IsKeyField = False
        Me.cbEmailResults._QueryDescr = "Email Results To Me"
        Me.cbEmailResults._ToolTip = "Check this box if you would like to receive email results for this saved query."
        Me.cbEmailResults.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cbEmailResults.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbEmailResults.Location = New System.Drawing.Point(32, 316)
        Me.cbEmailResults.Name = "cbEmailResults"
        Me.cbEmailResults.Size = New System.Drawing.Size(182, 20)
        Me.cbEmailResults.TabIndex = 1011
        Me.cbEmailResults.Text = "Email Results To Me  "
        Me.cbEmailResults.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cbEmailResults.UseVisualStyleBackColor = True
        '
        'lblccusers
        '
        Me.lblccusers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblccusers.Location = New System.Drawing.Point(508, 316)
        Me.lblccusers.Name = "lblccusers"
        Me.lblccusers.Size = New System.Drawing.Size(170, 21)
        Me.lblccusers.TabIndex = 1017
        Me.lblccusers.Text = "CC These Users:"
        Me.lblccusers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'drCCUsers
        '
        '
        'drCCUsers.ItemTemplate
        '
        Me.drCCUsers.ItemTemplate.Controls.Add(Me.txtUserName)
        Me.drCCUsers.ItemTemplate.Controls.Add(Me.ddCCuser)
        Me.drCCUsers.ItemTemplate.Size = New System.Drawing.Size(310, 24)
        Me.drCCUsers.Location = New System.Drawing.Point(543, 361)
        Me.drCCUsers.Name = "drCCUsers"
        Me.drCCUsers.Size = New System.Drawing.Size(318, 127)
        Me.drCCUsers.TabIndex = 1018
        Me.drCCUsers.Text = "QDR1"
        '
        'txtUserName
        '
        Me.txtUserName._BindDef = "fullname"
        Me.txtUserName._Format = ""
        Me.txtUserName._FormatNumber = ""
        Me.txtUserName._IsKeyField = False
        Me.txtUserName._ReadAlways = True
        Me.txtUserName._ToolTip = "CC User Name"
        Me.txtUserName._TransparentDisplay = True
        Me.txtUserName._ValidateMaxValue = ""
        Me.txtUserName._ValidateMinValue = ""
        Me.txtUserName.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.txtUserName.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtUserName.Location = New System.Drawing.Point(127, 3)
        Me.txtUserName.Name = "txtUserName"
        Me.txtUserName.Size = New System.Drawing.Size(150, 18)
        Me.txtUserName.TabIndex = 1
        '
        'ddCCuser
        '
        Me.ddCCuser._BindDef = "CCUser"
        Me.ddCCuser._FindByColumn = "value"
        Me.ddCCuser._FindByNoSort = False
        Me.ddCCuser._FindBySortAscending = True
        Me.ddCCuser._Format = ""
        Me.ddCCuser._IsKeyField = False
        Me.ddCCuser._MustMatchList = True
        Me.ddCCuser._MustMatchTime = 800
        Me.ddCCuser._QueryDescr = "CC User ID"
        Me.ddCCuser._SelectedValueColumn = "value"
        Me.ddCCuser._TextColumn = "value"
        Me.ddCCuser._ToolTip = "Enter another user ID to also receive the email."
        Me.ddCCuser.DataSource = Nothing
        Me.ddCCuser.DisplayMember = "value"
        Me.ddCCuser.Location = New System.Drawing.Point(8, 0)
        Me.ddCCuser.MaxLength = 32767
        Me.ddCCuser.Name = "ddCCuser"
        Me.ddCCuser.SelectedIndex = -1
        Me.ddCCuser.SelectedValue = Nothing
        Me.ddCCuser.Size = New System.Drawing.Size(118, 21)
        Me.ddCCuser.TabIndex = 0
        Me.ddCCuser.TextInfo = ""
        Me.ddCCuser.ValueMember = "value"
        '
        'lblCol1
        '
        Me.lblCol1.BackColor = System.Drawing.Color.FromArgb(CType(CType(190, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lblCol1.Location = New System.Drawing.Point(572, 343)
        Me.lblCol1.Name = "lblCol1"
        Me.lblCol1.Size = New System.Drawing.Size(112, 15)
        Me.lblCol1.TabIndex = 1020
        Me.lblCol1.Text = "User ID"
        Me.lblCol1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblCol2
        '
        Me.lblCol2.BackColor = System.Drawing.Color.FromArgb(CType(CType(190, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lblCol2.Location = New System.Drawing.Point(690, 343)
        Me.lblCol2.Name = "lblCol2"
        Me.lblCol2.Size = New System.Drawing.Size(148, 15)
        Me.lblCol2.TabIndex = 1021
        Me.lblCol2.Text = "User Name"
        Me.lblCol2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnRemoveCC
        '
        Me.btnRemoveCC.Image = Global.QSILib.My.Resources.Resources.MINUS
        Me.btnRemoveCC.Location = New System.Drawing.Point(507, 387)
        Me.btnRemoveCC.Name = "btnRemoveCC"
        Me.btnRemoveCC.Size = New System.Drawing.Size(25, 23)
        Me.btnRemoveCC.TabIndex = 1023
        Me.ToolTip1.SetToolTip(Me.btnRemoveCC, "Remove the selected email CC recipient.")
        Me.btnRemoveCC.UseVisualStyleBackColor = True
        '
        'btnInsertCC
        '
        Me.btnInsertCC.Image = Global.QSILib.My.Resources.Resources.PLUS
        Me.btnInsertCC.Location = New System.Drawing.Point(507, 361)
        Me.btnInsertCC.Name = "btnInsertCC"
        Me.btnInsertCC.Size = New System.Drawing.Size(25, 23)
        Me.btnInsertCC.TabIndex = 1022
        Me.ToolTip1.SetToolTip(Me.btnInsertCC, "Add an email CC recipient.")
        Me.btnInsertCC.UseVisualStyleBackColor = True
        '
        'txtMenuName
        '
        Me.txtMenuName._Format = ""
        Me.txtMenuName._FormatNumber = ""
        Me.txtMenuName._IsKeyField = False
        Me.txtMenuName._ReadAlways = True
        Me.txtMenuName._TransparentDisplay = True
        Me.txtMenuName._ValidateMaxValue = ""
        Me.txtMenuName._ValidateMinValue = ""
        Me.txtMenuName.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.txtMenuName.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtMenuName.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMenuName.Location = New System.Drawing.Point(497, 42)
        Me.txtMenuName.Multiline = True
        Me.txtMenuName.Name = "txtMenuName"
        Me.txtMenuName.Size = New System.Drawing.Size(384, 45)
        Me.txtMenuName.TabIndex = 1037
        Me.txtMenuName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblScreenResults
        '
        Me.lblScreenResults.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblScreenResults.Location = New System.Drawing.Point(508, 118)
        Me.lblScreenResults.Name = "lblScreenResults"
        Me.lblScreenResults.Size = New System.Drawing.Size(264, 21)
        Me.lblScreenResults.TabIndex = 1044
        Me.lblScreenResults.Text = "Available Results (select at least one):"
        Me.lblScreenResults.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblReportResults
        '
        Me.lblReportResults.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReportResults.Location = New System.Drawing.Point(508, 118)
        Me.lblReportResults.Name = "lblReportResults"
        Me.lblReportResults.Size = New System.Drawing.Size(264, 36)
        Me.lblReportResults.TabIndex = 1045
        Me.lblReportResults.Text = "The format saved in the report screen will be attached to the email."
        Me.lblReportResults.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'gvResults
        '
        Me.gvResults._BlankRowOnEmpty = True
        Me.gvResults._GVFoot = Nothing
        Me.gvResults._ShowSelectionBar = True
        Me.gvResults.AllowUserToAddRows = False
        Me.gvResults.AllowUserToDeleteRows = False
        Me.gvResults.AllowUserToOrderColumns = True
        Me.gvResults.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.gvResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None
        Me.gvResults.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Tahoma", 8.25!)
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.gvResults.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.gvResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gvResults.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.sel, Me.title})
        Me.gvResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me.gvResults.Location = New System.Drawing.Point(543, 145)
        Me.gvResults.MultiSelect = False
        Me.gvResults.Name = "gvResults"
        Me.gvResults.RowHeadersVisible = False
        Me.gvResults.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White
        Me.gvResults.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black
        Me.gvResults.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(180, Byte), Integer))
        Me.gvResults.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black
        Me.gvResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.gvResults.Size = New System.Drawing.Size(318, 162)
        Me.gvResults.TabIndex = 1053
        '
        'sel
        '
        Me.sel.DataPropertyName = "sel"
        Me.sel.FalseValue = "False"
        Me.sel.HeaderText = "Select"
        Me.sel.Name = "sel"
        Me.sel.ToolTipText = "Check this box if the available results are to be emailed."
        Me.sel.TrueValue = "True"
        Me.sel.Width = 50
        '
        'title
        '
        Me.title.DataPropertyName = "title"
        Me.title.HeaderText = "Title"
        Me.title.Name = "title"
        Me.title.ReadOnly = True
        Me.title.Width = 245
        '
        'Panel1
        '
        Me.Panel1.Location = New System.Drawing.Point(19, 316)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(853, 172)
        Me.Panel1.TabIndex = 1060
        '
        'feSaveQuery
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.ClientSize = New System.Drawing.Size(893, 544)
        Me.Controls.Add(Me.gvResults)
        Me.Controls.Add(Me.lblReportResults)
        Me.Controls.Add(Me.lblScreenResults)
        Me.Controls.Add(Me.txtMenuName)
        Me.Controls.Add(Me.btnRemoveCC)
        Me.Controls.Add(Me.btnInsertCC)
        Me.Controls.Add(Me.lblCol2)
        Me.Controls.Add(Me.lblCol1)
        Me.Controls.Add(Me.drCCUsers)
        Me.Controls.Add(Me.lblccusers)
        Me.Controls.Add(Me.cbEmailResults)
        Me.Controls.Add(Me.gbRecurrence)
        Me.Controls.Add(Me.btnShowFiles)
        Me.Controls.Add(Me.chkDefaultQuery)
        Me.Controls.Add(Me.ddShared)
        Me.Controls.Add(Me.QLabel8)
        Me.Controls.Add(Me.txtDescr)
        Me.Controls.Add(Me.QLabel6)
        Me.Controls.Add(Me.QLabel5)
        Me.Controls.Add(Me.txtTitle)
        Me.Controls.Add(Me.QLabel4)
        Me.Controls.Add(Me.QLabel3)
        Me.Controls.Add(Me.QLabel2)
        Me.Controls.Add(Me.QLabel1)
        Me.Controls.Add(Me.ddQueries)
        Me.Controls.Add(Me.rbUpdate)
        Me.Controls.Add(Me.rbNew)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.ForeColor = System.Drawing.Color.Black
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "feSaveQuery"
        Me.Text = "Save Query"
        Me.Controls.SetChildIndex(Me.Panel1, 0)
        Me.Controls.SetChildIndex(Me.rbNew, 0)
        Me.Controls.SetChildIndex(Me.rbUpdate, 0)
        Me.Controls.SetChildIndex(Me.ddQueries, 0)
        Me.Controls.SetChildIndex(Me.QLabel1, 0)
        Me.Controls.SetChildIndex(Me.QLabel2, 0)
        Me.Controls.SetChildIndex(Me.QLabel3, 0)
        Me.Controls.SetChildIndex(Me.QLabel4, 0)
        Me.Controls.SetChildIndex(Me.txtTitle, 0)
        Me.Controls.SetChildIndex(Me.QLabel5, 0)
        Me.Controls.SetChildIndex(Me.QLabel6, 0)
        Me.Controls.SetChildIndex(Me.txtDescr, 0)
        Me.Controls.SetChildIndex(Me.QLabel8, 0)
        Me.Controls.SetChildIndex(Me.ddShared, 0)
        Me.Controls.SetChildIndex(Me.chkDefaultQuery, 0)
        Me.Controls.SetChildIndex(Me.btnShowFiles, 0)
        Me.Controls.SetChildIndex(Me.gbRecurrence, 0)
        Me.Controls.SetChildIndex(Me.cbEmailResults, 0)
        Me.Controls.SetChildIndex(Me.lblccusers, 0)
        Me.Controls.SetChildIndex(Me.drCCUsers, 0)
        Me.Controls.SetChildIndex(Me.lblCol1, 0)
        Me.Controls.SetChildIndex(Me.lblCol2, 0)
        Me.Controls.SetChildIndex(Me.btnInsertCC, 0)
        Me.Controls.SetChildIndex(Me.btnRemoveCC, 0)
        Me.Controls.SetChildIndex(Me.txtMenuName, 0)
        Me.Controls.SetChildIndex(Me.lblScreenResults, 0)
        Me.Controls.SetChildIndex(Me.lblReportResults, 0)
        Me.Controls.SetChildIndex(Me.gvResults, 0)
        Me.Controls.SetChildIndex(Me.btnDelete, 0)
        Me.Controls.SetChildIndex(Me.btnSave, 0)
        Me.Controls.SetChildIndex(Me.lblFTitle, 0)
        Me.Controls.SetChildIndex(Me.txtDirtyDisplay, 0)
        Me.Controls.SetChildIndex(Me.btnNew, 0)
        CType(Me.iDS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.iEP, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbRecurrence.ResumeLayout(False)
        Me.gbRecurrence.PerformLayout()
        Me.pDaily.ResumeLayout(False)
        Me.pYearly.ResumeLayout(False)
        Me.pWeekly.ResumeLayout(False)
        Me.pWeekly.PerformLayout()
        Me.pMonthly.ResumeLayout(False)
        Me.drCCUsers.ItemTemplate.ResumeLayout(False)
        Me.drCCUsers.ResumeLayout(False)
        CType(Me.gvResults, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents rbNew As QSILib.qRB
    Friend WithEvents rbUpdate As QSILib.qRB
    Friend WithEvents ddQueries As QSILib.qDD
    Friend WithEvents QLabel1 As QSILib.qLabel
    Friend WithEvents QLabel2 As QSILib.qLabel
    Friend WithEvents QLabel3 As QSILib.qLabel
    Friend WithEvents QLabel4 As QSILib.qLabel
    Friend WithEvents txtTitle As QSILib.qTextBox
    Friend WithEvents QLabel5 As QSILib.qLabel
    Friend WithEvents QLabel6 As QSILib.qLabel
    Friend WithEvents txtDescr As QSILib.qTextBox
    Friend WithEvents QLabel8 As QSILib.qLabel
    Friend WithEvents ddShared As QSILib.qDD
    Friend WithEvents chkDefaultQuery As QSILib.qCheckBox
    Friend WithEvents btnShowFiles As System.Windows.Forms.Button
    Friend WithEvents gbRecurrence As System.Windows.Forms.GroupBox
    Friend WithEvents pDaily As System.Windows.Forms.Panel
    Friend WithEvents rbYearly As QSILib.qRB
    Friend WithEvents rbMonthly As QSILib.qRB
    Friend WithEvents rbWeekly As QSILib.qRB
    Friend WithEvents rbDaily As QSILib.qRB
    Friend WithEvents cbEmailResults As QSILib.qCheckBox
    Friend WithEvents QLabel7 As QSILib.qLabel
    Friend WithEvents pWeekly As System.Windows.Forms.Panel
    Friend WithEvents QLabel10 As QSILib.qLabel
    Friend WithEvents QLabel9 As QSILib.qLabel
    Friend WithEvents cbAllDays As QSILib.qCheckBox
    Friend WithEvents cbSaturday As QSILib.qCheckBox
    Friend WithEvents cbFriday As QSILib.qCheckBox
    Friend WithEvents cbThursday As QSILib.qCheckBox
    Friend WithEvents cbWednesday As QSILib.qCheckBox
    Friend WithEvents cbTuesday As QSILib.qCheckBox
    Friend WithEvents cbMonday As QSILib.qCheckBox
    Friend WithEvents cbSunday As QSILib.qCheckBox
    Friend WithEvents pMonthly As System.Windows.Forms.Panel
    Friend WithEvents QLabel11 As QSILib.qLabel
    Friend WithEvents pYearly As System.Windows.Forms.Panel
    Friend WithEvents ddMonth As QSILib.qDD
    Friend WithEvents QLabel15 As QSILib.qLabel
    Friend WithEvents ddDOM2 As QSILib.qDD
    Friend WithEvents QLabel13 As QSILib.qLabel
    Friend WithEvents QLabel14 As QSILib.qLabel
    Friend WithEvents lblccusers As QSILib.qLabel
    Friend WithEvents drCCUsers As QSILib.qDR
    Friend WithEvents ddCCuser As QSILib.qDD
    Friend WithEvents lblCol1 As QSILib.qLabel
    Friend WithEvents txtUserName As QSILib.qTextBox
    Friend WithEvents lblCol2 As QSILib.qLabel
    Friend WithEvents btnRemoveCC As System.Windows.Forms.Button
    Friend WithEvents btnInsertCC As System.Windows.Forms.Button
    Friend WithEvents ddDOM1 As QSILib.qDD
    Friend WithEvents QLabel12 As QSILib.qLabel
    Friend WithEvents txtMenuName As QSILib.qTextBox
    Friend WithEvents lblScreenResults As QSILib.qLabel
    Friend WithEvents lblReportResults As QSILib.qLabel
    Friend WithEvents gvResults As QSILib.qGVEdit
    Friend WithEvents sel As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents title As QSILib.qGVTextBoxColumn
    Friend WithEvents Panel1 As System.Windows.Forms.Panel

End Class
