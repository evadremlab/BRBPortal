<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fSyntaxError
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(fSyntaxError))
        Me.btnOK = New System.Windows.Forms.Button
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.btnDetails = New System.Windows.Forms.Button
        Me.btnSearchHelp = New System.Windows.Forms.Button
        Me.txtCriteria = New QSILib.Windows.Forms.qTextBox
        Me.txtMsg = New QSILib.Windows.Forms.qTextBox
        Me.lblMsg = New QSILib.Windows.Forms.qLabel
        Me.lblCriteria = New QSILib.Windows.Forms.qLabel
        Me.SuspendLayout()
        '
        'btnOK
        '
        Me.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnOK.Location = New System.Drawing.Point(122, 125)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 2
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnDetails
        '
        Me.btnDetails.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnDetails.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnDetails.Location = New System.Drawing.Point(312, 125)
        Me.btnDetails.Name = "btnDetails"
        Me.btnDetails.Size = New System.Drawing.Size(89, 23)
        Me.btnDetails.TabIndex = 6
        Me.btnDetails.Text = "Show Details"
        Me.btnDetails.UseVisualStyleBackColor = True
        '
        'btnSearchHelp
        '
        Me.btnSearchHelp.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnSearchHelp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSearchHelp.Location = New System.Drawing.Point(210, 125)
        Me.btnSearchHelp.Name = "btnSearchHelp"
        Me.btnSearchHelp.Size = New System.Drawing.Size(89, 23)
        Me.btnSearchHelp.TabIndex = 7
        Me.btnSearchHelp.Text = "Search Help"
        Me.btnSearchHelp.UseVisualStyleBackColor = True
        '
        'txtCriteria
        '
        Me.txtCriteria._Format = ""
        Me.txtCriteria._FormatNumber = ""
        Me.txtCriteria._IsKeyField = False
        Me.txtCriteria._ValidateMaxValue = ""
        Me.txtCriteria._ValidateMinValue = ""
        Me.txtCriteria.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCriteria.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.txtCriteria.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtCriteria.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCriteria.Location = New System.Drawing.Point(22, 73)
        Me.txtCriteria.Multiline = True
        Me.txtCriteria.Name = "txtCriteria"
        Me.txtCriteria.Size = New System.Drawing.Size(510, 33)
        Me.txtCriteria.TabIndex = 5
        Me.txtCriteria.TabStop = False
        Me.txtCriteria.Text = "Test"
        '
        'txtMsg
        '
        Me.txtMsg._Format = ""
        Me.txtMsg._FormatNumber = ""
        Me.txtMsg._IsKeyField = False
        Me.txtMsg._ValidateMaxValue = ""
        Me.txtMsg._ValidateMinValue = ""
        Me.txtMsg.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMsg.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.txtMsg.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtMsg.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMsg.Location = New System.Drawing.Point(22, 26)
        Me.txtMsg.MaximumSize = New System.Drawing.Size(526, 50)
        Me.txtMsg.MinimumSize = New System.Drawing.Size(526, 20)
        Me.txtMsg.Multiline = True
        Me.txtMsg.Name = "txtMsg"
        Me.txtMsg.Size = New System.Drawing.Size(526, 20)
        Me.txtMsg.TabIndex = 8
        Me.txtMsg.TabStop = False
        Me.txtMsg.Text = "Please check your Search criteria to make sure you have used valid numbers, dates" & _
            ", and search symbols."
        '
        'lblMsg
        '
        Me.lblMsg.AutoSize = True
        Me.lblMsg.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMsg.Location = New System.Drawing.Point(3, 9)
        Me.lblMsg.Name = "lblMsg"
        Me.lblMsg.Size = New System.Drawing.Size(228, 13)
        Me.lblMsg.TabIndex = 10
        Me.lblMsg.Text = "Unable to process your Search request"
        Me.lblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblCriteria
        '
        Me.lblCriteria.AutoSize = True
        Me.lblCriteria.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCriteria.Location = New System.Drawing.Point(3, 55)
        Me.lblCriteria.Name = "lblCriteria"
        Me.lblCriteria.Size = New System.Drawing.Size(95, 13)
        Me.lblCriteria.TabIndex = 11
        Me.lblCriteria.Text = "Search Criteria:"
        Me.lblCriteria.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'fSyntaxError
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(547, 150)
        Me.Controls.Add(Me.lblCriteria)
        Me.Controls.Add(Me.lblMsg)
        Me.Controls.Add(Me.txtMsg)
        Me.Controls.Add(Me.btnSearchHelp)
        Me.Controls.Add(Me.btnDetails)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.txtCriteria)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "fSyntaxError"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Database Search Problem"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents txtCriteria As QSILib.Windows.Forms.qTextBox
    Friend WithEvents btnDetails As System.Windows.Forms.Button
    Friend WithEvents btnSearchHelp As System.Windows.Forms.Button
    Friend WithEvents txtMsg As QSILib.Windows.Forms.qTextBox
    Friend WithEvents lblMsg As QSILib.Windows.Forms.qLabel
    Friend WithEvents lblCriteria As QSILib.Windows.Forms.qLabel
End Class
