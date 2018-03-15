Namespace Windows.Forms

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class fuMain
        Inherits fBase


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
            Me.btnStart = New System.Windows.Forms.Button
            Me.lblFTitle = New System.Windows.Forms.Label
            CType(Me.iDS, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.iEP, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'btnStart
            '
            Me.btnStart.Image = Global.QSILib.My.Resources.Resources.Run
            Me.btnStart.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnStart.Location = New System.Drawing.Point(154, 88)
            Me.btnStart.Margin = New System.Windows.Forms.Padding(2)
            Me.btnStart.Name = "btnStart"
            Me.btnStart.Size = New System.Drawing.Size(65, 23)
            Me.btnStart.TabIndex = 13
            Me.btnStart.TabStop = False
            Me.btnStart.Text = "Start"
            Me.btnStart.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.btnStart.UseVisualStyleBackColor = True
            '
            'lblFTitle
            '
            Me.lblFTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblFTitle.AutoSize = True
            Me.lblFTitle.Font = New System.Drawing.Font("Arial", 12.0!, CType((System.Drawing.FontStyle.Italic Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblFTitle.ForeColor = System.Drawing.SystemColors.MenuHighlight
            Me.lblFTitle.Location = New System.Drawing.Point(299, 1)
            Me.lblFTitle.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
            Me.lblFTitle.Name = "lblFTitle"
            Me.lblFTitle.Size = New System.Drawing.Size(106, 19)
            Me.lblFTitle.TabIndex = 19
            Me.lblFTitle.Text = "Function Title"
            Me.lblFTitle.TextAlign = System.Drawing.ContentAlignment.TopRight
            '
            'fuMain
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
            Me.ClientSize = New System.Drawing.Size(396, 218)
            Me.Controls.Add(Me.lblFTitle)
            Me.Controls.Add(Me.btnStart)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.Margin = New System.Windows.Forms.Padding(2)
            Me.Name = "fuMain"
            Me.Text = "fuMain"
            CType(Me.iDS, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.iEP, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Protected WithEvents btnStart As System.Windows.Forms.Button
        Protected WithEvents lblFTitle As System.Windows.Forms.Label
    End Class

End Namespace