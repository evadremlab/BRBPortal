Namespace Windows.Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class fBrowse
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
            Me.lblRows = New System.Windows.Forms.Label
            Me.btnClear = New System.Windows.Forms.Button
            Me.btnCancel = New System.Windows.Forms.Button
            Me.btnAccept = New System.Windows.Forms.Button
            Me.txtCode = New qTextBox
            Me.txtDescr = New qTextBox
            Me.txtFilter = New qTextBox
            Me.FilterTimer = New System.Windows.Forms.Timer(Me.components)
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
            'btnClear
            '
            Me.btnClear.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnClear.Image = Global.QSILib.My.Resources.Resources._NEW
            Me.btnClear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnClear.Location = New System.Drawing.Point(3, 422)
            Me.btnClear.Name = "btnClear"
            Me.btnClear.Size = New System.Drawing.Size(94, 20)
            Me.btnClear.TabIndex = 23
            Me.btnClear.Text = "Clear Selects"
            Me.btnClear.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.btnClear.UseVisualStyleBackColor = True
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
            'txtCode
            '
            Me.txtCode._ProtectedField = False
            Me.txtCode.Location = New System.Drawing.Point(74, 26)
            Me.txtCode.Name = "txtCode"
            Me.txtCode.Size = New System.Drawing.Size(100, 20)
            Me.txtCode.TabIndex = 24
            '
            'txtDescr
            '
            Me.txtDescr._ProtectedField = False
            Me.txtDescr.Location = New System.Drawing.Point(180, 26)
            Me.txtDescr.Name = "txtDescr"
            Me.txtDescr.Size = New System.Drawing.Size(147, 20)
            Me.txtDescr.TabIndex = 25
            '
            'txtFilter
            '
            Me.txtFilter._ProtectedField = False
            Me.txtFilter.Location = New System.Drawing.Point(422, 26)
            Me.txtFilter.Name = "txtFilter"
            Me.txtFilter.Size = New System.Drawing.Size(147, 20)
            Me.txtFilter.TabIndex = 26
            '
            'FilterTimer
            '
            '
            'fBrowse
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
            Me.ClientSize = New System.Drawing.Size(581, 440)
            Me.Controls.Add(Me.txtFilter)
            Me.Controls.Add(Me.txtDescr)
            Me.Controls.Add(Me.txtCode)
            Me.Controls.Add(Me.btnClear)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnAccept)
            Me.Controls.Add(Me.lblRows)
            Me.Name = "fBrowse"
            Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "fBrowse"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents FilterTimer As System.Windows.Forms.Timer
        Protected WithEvents lblRows As System.Windows.Forms.Label
        Protected WithEvents btnClear As System.Windows.Forms.Button
        Protected WithEvents btnCancel As System.Windows.Forms.Button
        Protected WithEvents btnAccept As System.Windows.Forms.Button
        Protected WithEvents txtDescr As qTextBox
        Protected WithEvents txtCode As qTextBox
        Protected WithEvents txtFilter As qTextBox
    End Class
End Namespace

