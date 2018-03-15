Namespace Windows.Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class fBrowse2
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
            Me.components = New System.ComponentModel.Container()
            Me.lblRows = New System.Windows.Forms.Label()
            Me.btnCancel = New System.Windows.Forms.Button()
            Me.btnAccept = New System.Windows.Forms.Button()
            Me.FilterTimer = New System.Windows.Forms.Timer(Me.components)
            Me.btnClear = New System.Windows.Forms.Button()
            Me.lblAll = New QSILib.qLabel()
            Me.chkAll = New QSILib.qCheckBox()
            Me.txtFilter = New QSILib.qTextBox()
            Me.txtDescr = New QSILib.qTextBox()
            Me.txtCode = New QSILib.qTextBox()
            Me.txtFilter2 = New QSILib.qTextBox()
            Me.btnAdd = New System.Windows.Forms.Button()
            Me.SuspendLayout()
            '
            'lblRows
            '
            Me.lblRows.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblRows.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
            Me.lblRows.Location = New System.Drawing.Point(491, 422)
            Me.lblRows.Name = "lblRows"
            Me.lblRows.Size = New System.Drawing.Size(78, 16)
            Me.lblRows.TabIndex = 17
            Me.lblRows.Text = "Rows"
            Me.lblRows.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'btnCancel
            '
            Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnCancel.Image = Global.QSILib.My.Resources.Resources.DELETE
            Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnCancel.Location = New System.Drawing.Point(287, 422)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(70, 20)
            Me.btnCancel.TabIndex = 22
            Me.btnCancel.Text = "Cancel"
            Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.btnCancel.UseVisualStyleBackColor = True
            '
            'btnAccept
            '
            Me.btnAccept.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnAccept.Image = Global.QSILib.My.Resources.Resources.PASTE
            Me.btnAccept.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnAccept.Location = New System.Drawing.Point(204, 422)
            Me.btnAccept.Name = "btnAccept"
            Me.btnAccept.Size = New System.Drawing.Size(70, 20)
            Me.btnAccept.TabIndex = 21
            Me.btnAccept.Text = "Accept"
            Me.btnAccept.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.btnAccept.UseVisualStyleBackColor = True
            '
            'FilterTimer
            '
            '
            'btnClear
            '
            Me.btnClear.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnClear.Image = Global.QSILib.My.Resources.Resources.Clear
            Me.btnClear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnClear.Location = New System.Drawing.Point(0, 422)
            Me.btnClear.Name = "btnClear"
            Me.btnClear.Size = New System.Drawing.Size(109, 20)
            Me.btnClear.TabIndex = 35
            Me.btnClear.Text = "Clear All Selects"
            Me.btnClear.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.btnClear.UseVisualStyleBackColor = True
            '
            'lblAll
            '
            Me.lblAll.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
            Me.lblAll.Location = New System.Drawing.Point(0, 0)
            Me.lblAll.Name = "lblAll"
            Me.lblAll.Size = New System.Drawing.Size(66, 27)
            Me.lblAll.TabIndex = 34
            Me.lblAll.Text = "Select/Clear Visible"
            Me.lblAll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'chkAll
            '
            Me.chkAll._DataType = QSILib.qFunctions.DataTypeEnum.Str
            Me.chkAll._IsKeyField = False
            Me.chkAll.AutoSize = True
            Me.chkAll.Location = New System.Drawing.Point(25, 28)
            Me.chkAll.Name = "chkAll"
            Me.chkAll.Size = New System.Drawing.Size(15, 14)
            Me.chkAll.TabIndex = 27
            Me.chkAll.UseVisualStyleBackColor = True
            '
            'txtFilter
            '
            Me.txtFilter._Format = ""
            Me.txtFilter._FormatNumber = ""
            Me.txtFilter._IsKeyField = False
            Me.txtFilter._ValidateMaxValue = ""
            Me.txtFilter._ValidateMinValue = ""
            Me.txtFilter.Location = New System.Drawing.Point(422, 26)
            Me.txtFilter.Name = "txtFilter"
            Me.txtFilter.Size = New System.Drawing.Size(147, 20)
            Me.txtFilter.TabIndex = 26
            '
            'txtDescr
            '
            Me.txtDescr._Format = ""
            Me.txtDescr._FormatNumber = ""
            Me.txtDescr._IsKeyField = False
            Me.txtDescr._ValidateMaxValue = ""
            Me.txtDescr._ValidateMinValue = ""
            Me.txtDescr.Location = New System.Drawing.Point(180, 26)
            Me.txtDescr.Name = "txtDescr"
            Me.txtDescr.Size = New System.Drawing.Size(147, 20)
            Me.txtDescr.TabIndex = 25
            '
            'txtCode
            '
            Me.txtCode._Format = ""
            Me.txtCode._FormatNumber = ""
            Me.txtCode._IsKeyField = False
            Me.txtCode._ValidateMaxValue = ""
            Me.txtCode._ValidateMinValue = ""
            Me.txtCode.Location = New System.Drawing.Point(74, 26)
            Me.txtCode.Name = "txtCode"
            Me.txtCode.Size = New System.Drawing.Size(100, 20)
            Me.txtCode.TabIndex = 24
            '
            'txtFilter2
            '
            Me.txtFilter2._Format = ""
            Me.txtFilter2._FormatNumber = ""
            Me.txtFilter2._IsKeyField = False
            Me.txtFilter2._ValidateMaxValue = ""
            Me.txtFilter2._ValidateMinValue = ""
            Me.txtFilter2.Location = New System.Drawing.Point(333, 26)
            Me.txtFilter2.Name = "txtFilter2"
            Me.txtFilter2.Size = New System.Drawing.Size(83, 20)
            Me.txtFilter2.TabIndex = 26
            '
            'btnAdd
            '
            Me.btnAdd.Image = Global.QSILib.My.Resources.Resources._NEW
            Me.btnAdd.Location = New System.Drawing.Point(551, 2)
            Me.btnAdd.Name = "btnAdd"
            Me.btnAdd.Size = New System.Drawing.Size(18, 23)
            Me.btnAdd.TabIndex = 36
            Me.btnAdd.UseVisualStyleBackColor = True
            Me.btnAdd.Visible = False
            '
            'fBrowse2
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
            Me.ClientSize = New System.Drawing.Size(581, 440)
            Me.Controls.Add(Me.btnAdd)
            Me.Controls.Add(Me.btnClear)
            Me.Controls.Add(Me.lblAll)
            Me.Controls.Add(Me.chkAll)
            Me.Controls.Add(Me.txtFilter2)
            Me.Controls.Add(Me.txtFilter)
            Me.Controls.Add(Me.txtDescr)
            Me.Controls.Add(Me.txtCode)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnAccept)
            Me.Controls.Add(Me.lblRows)
            Me.Name = "fBrowse2"
            Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "fBrowse"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents FilterTimer As System.Windows.Forms.Timer
        Protected WithEvents lblRows As System.Windows.Forms.Label
        Protected WithEvents btnCancel As System.Windows.Forms.Button
        Public WithEvents btnAccept As System.Windows.Forms.Button
        Protected WithEvents txtDescr As qTextBox
        Protected WithEvents txtCode As qTextBox
        Protected WithEvents txtFilter As qTextBox
        Public WithEvents chkAll As QSILib.qCheckBox
        Public WithEvents lblAll As QSILib.qLabel
        Public WithEvents btnClear As System.Windows.Forms.Button
        Protected WithEvents txtFilter2 As QSILib.qTextBox
        Public WithEvents btnAdd As System.Windows.Forms.Button
    End Class
End Namespace

