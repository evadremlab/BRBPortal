Public Class PopUpWindow
    Inherits System.Windows.Forms.ToolStripDropDown
    Public _Content As DataGridView
    Private _Host As System.Windows.Forms.ToolStripControlHost
    Public Sub New(ByRef content As DataGridView)
        ' Basic setup
        Me.AutoSize = False
        Me.DoubleBuffered = True
        Me.ResizeRedraw = True
        Me.DefaultDropDownDirection = ToolStripDropDownDirection.BelowRight
        Me.BackColor = Color.White

        content.Location = Point.Empty
        Me._Content = content
        content.MaximumSize = content.Size

        Me._Host = New System.Windows.Forms.ToolStripControlHost(content)

        ' Positioning and Sizing
        SizePopup(content)
        
        ' Add host to the list
        Me.Items.Add(Me._Host)
    End Sub

    Public Sub SizePopup(ByRef content As DataGridView)
        'If content.Height <= content.RowTemplate.Height * content.Rows.Count Then
        Dim S As Size = content.Size
        S.Height += 4
        Me.MinimumSize = content.MinimumSize
        Me.MaximumSize = S
        Me.Size = S
        'Else
        'Me.MinimumSize = content.MinimumSize
        'Me.MaximumSize = content.Size
        'Me.Size = content.Size
        'End If
    End Sub
    Public Sub SetDataSource(ByVal aValue As Object)
        Me._Content.DataSource = CType(aValue, DataTable)
    End Sub

    Public Sub ClearPopUp()
        '_Content.Rows.Clear()
        'Me._Host = Nothing
    End Sub

End Class
