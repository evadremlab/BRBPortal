<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class feLoadQuery
    Inherits Windows.Forms.feMain

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
        Me.QLabel4 = New QSILib.Windows.Forms.qLabel()
        Me.QLabel3 = New QSILib.Windows.Forms.qLabel()
        Me.QLabel2 = New QSILib.Windows.Forms.qLabel()
        Me.QLabel1 = New QSILib.Windows.Forms.qLabel()
        Me.txtDescr = New QSILib.Windows.Forms.qTextBox()
        Me.QLabel6 = New QSILib.Windows.Forms.qLabel()
        Me.QLabel5 = New QSILib.Windows.Forms.qLabel()
        Me.txtTitle = New QSILib.Windows.Forms.qTextBox()
        Me.ddQueries = New QSILib.qDD()
        Me.ddQueryType = New QSILib.qDD()
        Me.QLabel7 = New QSILib.Windows.Forms.qLabel()
        Me.btnLoad = New System.Windows.Forms.Button()
        Me.QLabel8 = New QSILib.Windows.Forms.qLabel()
        Me.txtSavedBy = New QSILib.Windows.Forms.qTextBox()
        Me.txtCreatedDt = New QSILib.Windows.Forms.qTextBox()
        Me.QLabel9 = New QSILib.Windows.Forms.qLabel()
        Me.txtModifiedDt = New QSILib.Windows.Forms.qTextBox()
        Me.QLabel10 = New QSILib.Windows.Forms.qLabel()
        CType(Me.iDS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.iEP, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnNew
        '
        Me.btnNew.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnNew.Location = New System.Drawing.Point(183, 3)
        Me.ToolTip1.SetToolTip(Me.btnNew, "Clear this form so I can add new information (<Ctrl> E)")
        Me.btnNew.UseVisualStyleBackColor = False
        Me.btnNew.Visible = False
        '
        'btnDelete
        '
        Me.btnDelete.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnDelete.Location = New System.Drawing.Point(252, 4)
        Me.ToolTip1.SetToolTip(Me.btnDelete, "Permanently delete everything on this form (<Ctrl> D)")
        Me.btnDelete.UseVisualStyleBackColor = False
        Me.btnDelete.Visible = False
        '
        'btnSave
        '
        Me.btnSave.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnSave.Location = New System.Drawing.Point(321, 5)
        Me.ToolTip1.SetToolTip(Me.btnSave, "Save everything on this form (<Ctrl> S)")
        Me.btnSave.UseVisualStyleBackColor = False
        Me.btnSave.Visible = False
        '
        'lblFTitle
        '
        Me.lblFTitle.Location = New System.Drawing.Point(423, 0)
        Me.lblFTitle.Size = New System.Drawing.Size(98, 19)
        Me.lblFTitle.Text = "Load Query"
        '
        'DirtyTimer
        '
        Me.DirtyTimer.Enabled = True
        '
        'QLabel4
        '
        Me.QLabel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.QLabel4.Location = New System.Drawing.Point(508, 106)
        Me.QLabel4.Name = "QLabel4"
        Me.QLabel4.Size = New System.Drawing.Size(1, 237)
        Me.QLabel4.TabIndex = 992
        Me.QLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'QLabel3
        '
        Me.QLabel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.QLabel3.Location = New System.Drawing.Point(11, 106)
        Me.QLabel3.Name = "QLabel3"
        Me.QLabel3.Size = New System.Drawing.Size(1, 237)
        Me.QLabel3.TabIndex = 2
        Me.QLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'QLabel2
        '
        Me.QLabel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.QLabel2.Location = New System.Drawing.Point(11, 343)
        Me.QLabel2.Name = "QLabel2"
        Me.QLabel2.Size = New System.Drawing.Size(498, 1)
        Me.QLabel2.TabIndex = 990
        Me.QLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'QLabel1
        '
        Me.QLabel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.QLabel1.Location = New System.Drawing.Point(11, 105)
        Me.QLabel1.Name = "QLabel1"
        Me.QLabel1.Size = New System.Drawing.Size(498, 1)
        Me.QLabel1.TabIndex = 3
        Me.QLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtDescr
        '
        Me.txtDescr._Format = ""
        Me.txtDescr._FormatNumber = ""
        Me.txtDescr._IsKeyField = False
        Me.txtDescr._QueryDescr = "Description"
        Me.txtDescr._ReadAlways = True
        Me.txtDescr._ToolTip = "Enter a description for this query."
        Me.txtDescr._ValidateMaxValue = ""
        Me.txtDescr._ValidateMinValue = ""
        Me.txtDescr._ValidateRequired = True
        Me.txtDescr.Location = New System.Drawing.Point(200, 143)
        Me.txtDescr.Multiline = True
        Me.txtDescr.Name = "txtDescr"
        Me.txtDescr.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDescr.Size = New System.Drawing.Size(291, 162)
        Me.txtDescr.TabIndex = 5
        '
        'QLabel6
        '
        Me.QLabel6.Location = New System.Drawing.Point(108, 143)
        Me.QLabel6.Name = "QLabel6"
        Me.QLabel6.Size = New System.Drawing.Size(86, 21)
        Me.QLabel6.TabIndex = 995
        Me.QLabel6.Text = "Description "
        Me.QLabel6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'QLabel5
        '
        Me.QLabel5.Location = New System.Drawing.Point(108, 118)
        Me.QLabel5.Name = "QLabel5"
        Me.QLabel5.Size = New System.Drawing.Size(86, 21)
        Me.QLabel5.TabIndex = 994
        Me.QLabel5.Text = "Title "
        Me.QLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtTitle
        '
        Me.txtTitle._Format = ""
        Me.txtTitle._FormatNumber = ""
        Me.txtTitle._IsKeyField = False
        Me.txtTitle._QueryDescr = "Title"
        Me.txtTitle._ReadAlways = True
        Me.txtTitle._ToolTip = "Enter the title for the query."
        Me.txtTitle._ValidateMaxValue = ""
        Me.txtTitle._ValidateMinValue = ""
        Me.txtTitle._ValidateRequired = True
        Me.txtTitle.Location = New System.Drawing.Point(200, 118)
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.Size = New System.Drawing.Size(291, 21)
        Me.txtTitle.TabIndex = 4
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
        Me.ddQueries.TabIndex = 1
        Me.ddQueries.TextInfo = ""
        Me.ddQueries.ValueMember = ""
        '
        'ddQueryType
        '
        Me.ddQueryType._FindByNoSort = False
        Me.ddQueryType._FindBySortAscending = True
        Me.ddQueryType._Format = ""
        Me.ddQueryType._IsKeyField = False
        Me.ddQueryType._MustMatchList = True
        Me.ddQueryType._MustMatchTime = 800
        Me.ddQueryType._QueryDescr = "Query Type"
        Me.ddQueryType._ToolTip = "Select the type of query, from the list of choices."
        Me.ddQueryType.DataSource = Nothing
        Me.ddQueryType.DisplayMember = ""
        Me.ddQueryType.Location = New System.Drawing.Point(200, 40)
        Me.ddQueryType.MaxLength = 32767
        Me.ddQueryType.Name = "ddQueryType"
        Me.ddQueryType.SelectedIndex = -1
        Me.ddQueryType.SelectedValue = Nothing
        Me.ddQueryType.Size = New System.Drawing.Size(186, 21)
        Me.ddQueryType.TabIndex = 0
        Me.ddQueryType.TextInfo = ""
        Me.ddQueryType.ValueMember = ""
        '
        'QLabel7
        '
        Me.QLabel7.Location = New System.Drawing.Point(108, 40)
        Me.QLabel7.Name = "QLabel7"
        Me.QLabel7.Size = New System.Drawing.Size(86, 21)
        Me.QLabel7.TabIndex = 1001
        Me.QLabel7.Text = "Select a Query "
        Me.QLabel7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnLoad
        '
        Me.btnLoad.Image = Global.QSILib.My.Resources.Resources.Open
        Me.btnLoad.ImageAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.btnLoad.Location = New System.Drawing.Point(21, 118)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(65, 23)
        Me.btnLoad.TabIndex = 6
        Me.btnLoad.Text = "&Load"
        Me.btnLoad.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnLoad, "Load Query")
        Me.btnLoad.UseVisualStyleBackColor = True
        '
        'QLabel8
        '
        Me.QLabel8.Location = New System.Drawing.Point(18, 314)
        Me.QLabel8.Name = "QLabel8"
        Me.QLabel8.Size = New System.Drawing.Size(33, 21)
        Me.QLabel8.TabIndex = 1003
        Me.QLabel8.Text = "User "
        Me.QLabel8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtSavedBy
        '
        Me.txtSavedBy._Format = ""
        Me.txtSavedBy._FormatNumber = ""
        Me.txtSavedBy._IsKeyField = False
        Me.txtSavedBy._QueryDescr = "User"
        Me.txtSavedBy._ReadAlways = True
        Me.txtSavedBy._ValidateMaxValue = ""
        Me.txtSavedBy._ValidateMinValue = ""
        Me.txtSavedBy._ValidateRequired = True
        Me.txtSavedBy.Location = New System.Drawing.Point(57, 314)
        Me.txtSavedBy.Name = "txtSavedBy"
        Me.txtSavedBy.Size = New System.Drawing.Size(110, 21)
        Me.txtSavedBy.TabIndex = 1004
        '
        'txtCreatedDt
        '
        Me.txtCreatedDt._Format = "MM/dd/yyyy"
        Me.txtCreatedDt._FormatNumber = ""
        Me.txtCreatedDt._IsKeyField = False
        Me.txtCreatedDt._QueryDescr = "Created Date"
        Me.txtCreatedDt._ReadAlways = True
        Me.txtCreatedDt._ValidateMaxValue = ""
        Me.txtCreatedDt._ValidateMinValue = ""
        Me.txtCreatedDt._ValidateRequired = True
        Me.txtCreatedDt.Location = New System.Drawing.Point(249, 314)
        Me.txtCreatedDt.Name = "txtCreatedDt"
        Me.txtCreatedDt.Size = New System.Drawing.Size(68, 21)
        Me.txtCreatedDt.TabIndex = 1006
        Me.txtCreatedDt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'QLabel9
        '
        Me.QLabel9.Location = New System.Drawing.Point(180, 314)
        Me.QLabel9.Name = "QLabel9"
        Me.QLabel9.Size = New System.Drawing.Size(66, 21)
        Me.QLabel9.TabIndex = 1005
        Me.QLabel9.Text = "Created On "
        Me.QLabel9.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtModifiedDt
        '
        Me.txtModifiedDt._Format = "MM/dd/yyyy"
        Me.txtModifiedDt._FormatNumber = ""
        Me.txtModifiedDt._IsKeyField = False
        Me.txtModifiedDt._QueryDescr = "Modified Date"
        Me.txtModifiedDt._ReadAlways = True
        Me.txtModifiedDt._ValidateMaxValue = ""
        Me.txtModifiedDt._ValidateMinValue = ""
        Me.txtModifiedDt._ValidateRequired = True
        Me.txtModifiedDt.Location = New System.Drawing.Point(423, 314)
        Me.txtModifiedDt.Name = "txtModifiedDt"
        Me.txtModifiedDt.Size = New System.Drawing.Size(68, 21)
        Me.txtModifiedDt.TabIndex = 1008
        Me.txtModifiedDt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'QLabel10
        '
        Me.QLabel10.Location = New System.Drawing.Point(327, 314)
        Me.QLabel10.Name = "QLabel10"
        Me.QLabel10.Size = New System.Drawing.Size(93, 21)
        Me.QLabel10.TabIndex = 1007
        Me.QLabel10.Text = "Last Modified On "
        Me.QLabel10.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'feLoadQuery
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.ClientSize = New System.Drawing.Size(523, 377)
        Me.Controls.Add(Me.txtModifiedDt)
        Me.Controls.Add(Me.QLabel10)
        Me.Controls.Add(Me.txtCreatedDt)
        Me.Controls.Add(Me.QLabel9)
        Me.Controls.Add(Me.txtSavedBy)
        Me.Controls.Add(Me.QLabel8)
        Me.Controls.Add(Me.btnLoad)
        Me.Controls.Add(Me.QLabel7)
        Me.Controls.Add(Me.ddQueryType)
        Me.Controls.Add(Me.ddQueries)
        Me.Controls.Add(Me.txtDescr)
        Me.Controls.Add(Me.QLabel6)
        Me.Controls.Add(Me.QLabel5)
        Me.Controls.Add(Me.txtTitle)
        Me.Controls.Add(Me.QLabel4)
        Me.Controls.Add(Me.QLabel3)
        Me.Controls.Add(Me.QLabel2)
        Me.Controls.Add(Me.QLabel1)
        Me.ForeColor = System.Drawing.Color.Black
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "feLoadQuery"
        Me.Text = "Load Query"
        Me.Controls.SetChildIndex(Me.lblFTitle, 0)
        Me.Controls.SetChildIndex(Me.txtDirtyDisplay, 0)
        Me.Controls.SetChildIndex(Me.btnNew, 0)
        Me.Controls.SetChildIndex(Me.btnDelete, 0)
        Me.Controls.SetChildIndex(Me.btnSave, 0)
        Me.Controls.SetChildIndex(Me.QLabel1, 0)
        Me.Controls.SetChildIndex(Me.QLabel2, 0)
        Me.Controls.SetChildIndex(Me.QLabel3, 0)
        Me.Controls.SetChildIndex(Me.QLabel4, 0)
        Me.Controls.SetChildIndex(Me.txtTitle, 0)
        Me.Controls.SetChildIndex(Me.QLabel5, 0)
        Me.Controls.SetChildIndex(Me.QLabel6, 0)
        Me.Controls.SetChildIndex(Me.txtDescr, 0)
        Me.Controls.SetChildIndex(Me.ddQueries, 0)
        Me.Controls.SetChildIndex(Me.ddQueryType, 0)
        Me.Controls.SetChildIndex(Me.QLabel7, 0)
        Me.Controls.SetChildIndex(Me.btnLoad, 0)
        Me.Controls.SetChildIndex(Me.QLabel8, 0)
        Me.Controls.SetChildIndex(Me.txtSavedBy, 0)
        Me.Controls.SetChildIndex(Me.QLabel9, 0)
        Me.Controls.SetChildIndex(Me.txtCreatedDt, 0)
        Me.Controls.SetChildIndex(Me.QLabel10, 0)
        Me.Controls.SetChildIndex(Me.txtModifiedDt, 0)
        CType(Me.iDS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.iEP, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents QLabel4 As QSILib.Windows.Forms.qLabel
    Friend WithEvents QLabel3 As QSILib.Windows.Forms.qLabel
    Friend WithEvents QLabel2 As QSILib.Windows.Forms.qLabel
    Friend WithEvents QLabel1 As QSILib.Windows.Forms.qLabel
    Friend WithEvents txtDescr As QSILib.Windows.Forms.qTextBox
    Friend WithEvents QLabel6 As QSILib.Windows.Forms.qLabel
    Friend WithEvents QLabel5 As QSILib.Windows.Forms.qLabel
    Friend WithEvents txtTitle As QSILib.Windows.Forms.qTextBox
    Friend WithEvents ddQueries As QSILib.qDD
    Friend WithEvents ddQueryType As QSILib.qDD
    Friend WithEvents QLabel7 As QSILib.Windows.Forms.qLabel
    Friend WithEvents btnLoad As System.Windows.Forms.Button
    Friend WithEvents QLabel8 As QSILib.Windows.Forms.qLabel
    Friend WithEvents txtSavedBy As QSILib.Windows.Forms.qTextBox
    Friend WithEvents txtCreatedDt As QSILib.Windows.Forms.qTextBox
    Friend WithEvents QLabel9 As QSILib.Windows.Forms.qLabel
    Friend WithEvents txtModifiedDt As QSILib.Windows.Forms.qTextBox
    Friend WithEvents QLabel10 As QSILib.Windows.Forms.qLabel

End Class
