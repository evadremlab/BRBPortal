Namespace Windows.Forms
    Public Class qCBMultiCol
        Inherits qComboBox

        Public Sub New()
            ' Set DrawMode to Owner Fixed
            Me.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        End Sub

#Region "_ColumnDef Property - obsolete: defined in codebehind"

        '    'Query Field holds Table.Field.Type for building query SQL
        '    Private _ColDef As String = String.Empty
        '    <System.ComponentModel.Category("Data"), _
        '            System.ComponentModel.Description("ColName1=0,ColName2=100, ColName3=500, etc. Where numbers are tabstops in pixels."), _
        '            System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
        '            System.ComponentModel.DefaultValue("")> _
        '            Public Overridable Property _MultiColDef() As String
        '        Get
        '            Return _ColDef
        '        End Get
        '        Set(ByVal value As String)
        '            _ColDef = value
        '        End Set
        '    End Property

#End Region

#Region "Draw Logic"
        Private Sub qCBMultiCol_DrawItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles Me.DrawItem

            ' The system sometimes calls this method with an index of -1. If that happens, exit.
            If e.Index < 0 Then
                e.DrawBackground()
                e.DrawFocusRectangle()
                Exit Sub
            End If

            'BHS 1/29/08 Don't attempt to draw if a DataSource is not specified (happens w/ datarepeaters)
            If Me.DataSource Is Nothing Then
                'e.DrawBackground() GBV 1/29/2009
                'e.DrawFocusRectangle() GBV 1/29/2009
                Exit Sub
            End If

            ' Create a Brush
            Dim br As System.Drawing.Brush = System.Drawing.Brushes.Black

            ' Get a Reference to the Item to be Drawn
            Dim dr As DataRow = Nothing

            ' Check DataSource Type
            If Me.DataSource.GetType Is GetType(System.Windows.Forms.BindingSource) Then
                ' Check BindingSource DataSource Type
                If CType(Me.DataSource, System.Windows.Forms.BindingSource).DataSource.GetType Is GetType(System.Data.DataView) Then
                    ' Get a Reference to the Item to be Drawn
                    dr = CType(CType(Me.DataSource, System.Windows.Forms.BindingSource).Item(e.Index), DataRowView).Row
                ElseIf CType(Me.DataSource, System.Windows.Forms.BindingSource).DataSource.GetType Is GetType(System.Data.DataTable) Then
                    ' Get a Reference to the Item to be Drawn
                    dr = CType(CType(Me.DataSource, System.Windows.Forms.BindingSource).Item(e.Index), DataRow)
                End If
            Else
                If Me.DataSource.GetType Is GetType(System.Data.DataView) Then
                    If e.Index > CType(Me.DataSource, System.Data.DataView).Count - 1 Then
                        'e.DrawBackground()
                        'e.DrawFocusRectangle()
                        Exit Sub
                    End If
                    ' Get a Reference to the Item to be Drawn
                    dr = CType(CType(Me.DataSource, System.Data.DataView).Item(e.Index), DataRowView).Row
                Else
                    If Me.DataSource.GetType Is GetType(System.Data.DataTable) Then
                        If e.Index > CType(Me.DataSource, System.Data.DataTable).Rows.Count - 1 Then
                            'e.DrawBackground()
                            'e.DrawFocusRectangle()
                            Exit Sub
                        End If
                        ' Get a Reference to the Item to be Drawn
                        dr = CType(CType(Me.DataSource, System.Data.DataTable).Rows(e.Index), DataRow)
                    End If

                End If

            End If

            ' Use a Generic String Format to Draw the Columns
            Dim format As System.Drawing.StringFormat = System.Drawing.StringFormat.GenericTypographic

            ' Call These Methods to Get Items to Highlight Properly
            e.DrawBackground()
            e.DrawFocusRectangle()

            ' If the Item is Selected, Change the Text Color to White
            If (e.State And System.Windows.Forms.DrawItemState.Selected) = System.Windows.Forms.DrawItemState.Selected Then
                br = System.Drawing.Brushes.White
            End If
            format.LineAlignment = System.Drawing.StringAlignment.Center

            ' Loop Through Columns Drawing Strings
            For Each dc As DataColumn In dr.Table.Columns
                ' Check for Visible Extended Property
                If CType(dc.ExtendedProperties.ContainsKey("visible"), Boolean) = False _
                    OrElse CType(dc.ExtendedProperties.Item("visible"), Boolean) = True Then
                    ' Draw String
                    e.Graphics.DrawString(dr.Item(dc.ColumnName).ToString, Me.Font, br, CType(dc.ExtendedProperties.Item("x"), Integer), e.Bounds.Top + (e.Bounds.Height \ 2), format)
                End If
            Next

        End Sub
#End Region

    End Class
End Namespace
