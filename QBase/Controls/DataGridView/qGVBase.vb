Imports System.Windows.Forms
Imports System.Drawing

Namespace Windows.Forms

    <System.Serializable(),
 System.ComponentModel.ComplexBindingProperties("DataSource", "DataMember")>
    Public Class qGVBase
        Inherits System.Windows.Forms.DataGridView

        'BHS 8/26/10
        Public iSortColumn As DataGridViewColumn = Nothing  'Remember sorted column between datasoruce reassignments
        Public iSortDirection As System.ComponentModel.ListSortDirection = System.ComponentModel.ListSortDirection.Ascending

        Public Sub New()
            ' Initialize DataGridView Properties
            Me.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top _
            Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            Me.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        End Sub

#Region " Anchor Property "
        ' Defines the edges of the container to which a certain control is bound.  When a control is anchored to an edge, the distance between the control's closest edge and the specified edge will remain constant.

        <System.ComponentModel.DefaultValue(GetType(AnchorStyles), "15")>
        Public Overrides Property Anchor() As System.Windows.Forms.AnchorStyles
            Get
                Return MyBase.Anchor
            End Get
            Set(ByVal value As System.Windows.Forms.AnchorStyles)
                MyBase.Anchor = value
            End Set
        End Property
#End Region

#Region " KeyFields Property "

        ' Defines the edges of the container to which a certain control is bound.  When a control is anchored to an edge, the distance between the control's closest edge and the specified edge will remain constant.
        Private _KeyFieldsVar As String = String.Empty
        <System.ComponentModel.Category("Data"),
            System.ComponentModel.Description("ColumnName1|ColumnName2...  List the column names that make up the key fields"),
            System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible),
            System.ComponentModel.DefaultValue("")>
        Public Overridable Property _KeyFields() As String
            Get
                Return _KeyFieldsVar
            End Get
            Set(ByVal value As String)
                _KeyFieldsVar = value
            End Set
        End Property

        ' Defines the key fields passed from the list form to the edit form
        Private _KeyFieldsTableVar As String = String.Empty
        <System.ComponentModel.Category("Data"),
            System.ComponentModel.Description("TableName...  Enter the table name or abbrev in the iListSQL Statement for the key fields"),
            System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible),
            System.ComponentModel.DefaultValue("")>
        Public Overridable Property _KeyFieldsTable() As String
            Get
                Return _KeyFieldsTableVar
            End Get
            Set(ByVal value As String)
                _KeyFieldsTableVar = value
            End Set
        End Property

#End Region

#Region "_EditFormName Property points to Edit Form to instantiate when user drills down in list"
        Private _ListEditFormNameVar As String = String.Empty
        <System.ComponentModel.Category("Data"),
            System.ComponentModel.Description("The name of the Edit Form object to open when the user double-clicks a list line."),
            System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible),
            System.ComponentModel.DefaultValue("")>
        Public Overridable Property _ListEditFormName() As String
            Get
                Return _ListEditFormNameVar
            End Get
            Set(ByVal value As String)
                _ListEditFormNameVar = value
            End Set
        End Property
#End Region

#Region "_GVFoot Property "
        ' Defines the name of a Footer GV to associate with this GV.  Blank means no footer GV
        Private _GVFootVar As qGVList = Nothing
        <System.ComponentModel.Category("Data"),
            System.ComponentModel.Description("Reference a footer GV, if appropriate"),
            System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible),
            System.ComponentModel.DefaultValue("")>
        Public Overridable Property _GVFoot() As qGVList
            Get
                Return _GVFootVar
            End Get
            Set(ByVal value As qGVList)
                _GVFootVar = value
            End Set
        End Property
#End Region

#Region " Show Selection Bar "

        ' Defines the edges of the container to which a certain control is bound.  When a control is anchored to an edge, the distance between the control's closest edge and the specified edge will remain constant.
        Private _ShowSelectionBarVar As Boolean = True
        <System.ComponentModel.Category("Appearance"),
            System.ComponentModel.Description("Enter false to hide selection bar"),
            System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible),
            System.ComponentModel.DefaultValue("")>
        Public Overridable Property _ShowSelectionBar() As Boolean
            Get
                Return _ShowSelectionBarVar
            End Get
            Set(ByVal value As Boolean)
                _ShowSelectionBarVar = value
            End Set
        End Property
#End Region

#Region " AutoSizeColumnsMode Property "
        ''' <summary>
        ''' Determines auto size mode for the visible columns.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.DefaultValue(GetType(DataGridViewAutoSizeColumnsMode), "AllCells")>
        Public Shadows Property AutoSizeColumnsMode() As DataGridViewAutoSizeColumnsMode
            Get
                Return MyBase.AutoSizeColumnsMode
            End Get
            Set(ByVal value As DataGridViewAutoSizeColumnsMode)
                MyBase.AutoSizeColumnsMode = value
            End Set
        End Property
#End Region

#Region " AutoSizeRowsMode Property "
        ''' <summary>
        ''' Determines auto size mode for the visible rows.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.DefaultValue(GetType(DataGridViewAutoSizeRowsMode), "AllCells")>
        Public Shadows Property AutoSizeRowsMode() As DataGridViewAutoSizeRowsMode
            Get
                Return MyBase.AutoSizeRowsMode
            End Get
            Set(ByVal value As DataGridViewAutoSizeRowsMode)
                MyBase.AutoSizeRowsMode = value
            End Set
        End Property
#End Region

#Region "Cell Formatting commented out 3/18/07 BHS"
        'BHS Removed 3/18/07 to improve performance
        'Private Sub qGVBase_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Me.CellFormatting

        '    ' Check for Column Type
        '    Select Case Me.Columns(e.ColumnIndex).GetType.Name
        '        Case "DataGridViewTextBoxColumn"

        '        Case "qGVTextBoxColumn"
        '            ' Check for Column Format
        '            Select Case CType(Me.Columns(e.ColumnIndex), qGVTextBoxColumn)._ColumnFormat
        '                Case ColumnFormatEnum.None
        '                Case ColumnFormatEnum.PhoneNumber
        '                    ' Format Phone Number Column
        '                    e.Value = FormatPhoneNumber(e.Value.ToString)
        '                    e.FormattingApplied = True
        '                Case ColumnFormatEnum.SocialSecurityNumber
        '                    ' Format Social Security Column
        '                    e.Value = FormatSocialSecurityNumber(e.Value.ToString)
        '                    e.FormattingApplied = True
        '                Case ColumnFormatEnum.Status
        '                    ' Format Status Column
        '                    e.Value = FormatStatus(CType(e.Value, Integer))
        '                    e.FormattingApplied = True
        '            End Select
        '            ' Check for Text Case
        '            Select Case CType(Me.Columns(e.ColumnIndex), qGVTextBoxColumn).ColumnTextCase
        '                Case TextCaseEnum.Normal
        '                Case TextCaseEnum.Lower
        '                    ' Convert Text to Lower Case
        '                    e.Value = e.Value.ToString.ToLower
        '                    e.FormattingApplied = True
        '                Case TextCaseEnum.Upper
        '                    ' Convert Text to Upper Case
        '                    e.Value = e.Value.ToString.ToUpper
        '                    e.FormattingApplied = True
        '                Case TextCaseEnum.Cap1stLetter
        '                    ' Convert Text to Capitalize 1st Letter of All Words
        '                    e.Value = Cap1stLetter(e.Value.ToString)
        '                    e.FormattingApplied = True
        '            End Select
        '    End Select

        'End Sub

#End Region

#Region "Events"    'Always change DBNulls to "" on the way into the GV, and from "" to DBNull on the way back to the DB.

        ''' <summary> Format Nulls as "" coming from DB to Grid cell </summary>
        Private Sub qGVBase_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Me.CellFormatting

            '-----------
            'BHS DEBUG TEST 9/11/13
            'DAVID - TRY RUNNING THIS IN DEBUGGER TO SEE IF THE SENDER IS THE GV, and if the GV has focus.  I'm thinking it shouldn't if you're moving between QBE fields.
            ' GBV 6/17/2015 - Commented this out - Ticket 3714
            'Dim GV As DataGridView = TryCast(sender, DataGridView)
            'If GV IsNot Nothing Then
            '    If GV.Focused = False Then Return
            'End If
            '-----------------

            If e.Value Is Nothing OrElse
           e.Value Is System.DBNull.Value Then
                e.Value = "" 'Turn Nulls to blank
            End If

            'e.Value.ToString = "1/1/0001 12:00:00 AM" Then ' GBV 6/17/2015 - Added this line - Ticket 3714 - Took it out 6/25/2015

        End Sub
        ''' <summary> Format "" as Nulls coming from Cell to DB </summary>
        Private Sub qGVBase_CellParsing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellParsingEventArgs) Handles Me.CellParsing
            If e.Value.ToString = "" OrElse e.Value.ToString = "1/1/0001 12:00:00 AM" Then e.Value = System.DBNull.Value 'Turn blanks to Nulls
        End Sub

        ''' <summary> If sort column has been saved, redo the sort when DataSource is reset </summary>
        Private Sub qGVBase_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DataSourceChanged
            ' GBV 6/17/2015 - Ticket 3714
            ' GBV 6/25/2015 - commented this out per Sajeel
            'If Me.Visible = False Then Return
            'For Each R As DataGridViewRow In Me.Rows
            '    For i = 0 To Me.Columns.Count - 1
            '        If R.Cells(i).Value Is Nothing OrElse R.Cells(i).Value Is System.DBNull.Value Then
            '            R.Cells(i).Style.NullValue = ""
            '            R.Cells(i).Value = ""
            '        End If
            '    Next
            'Next
            ' ******* End of Ticket 3714 *********

            If iSortColumn IsNot Nothing Then
                'BHS 12/10/10 refer to the column as a member of this datagridview
                Me.Sort(Me.Columns(iSortColumn.Name), iSortDirection)
            End If

        End Sub

        ''Move column event in gvMain
        'Private Sub gvMain_ColumnDisplayIndexChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles Me.ColumnDisplayIndexChanged
        '    'MsgBox(e.Column.Name & " " & e.Column.DisplayIndex.ToString)
        '    If Me._GVFoot IsNot Nothing Then
        '        Dim CName As String = e.Column.Name & "Foot"
        '        Me._GVFoot.Columns.Item(CName).DisplayIndex = e.Column.DisplayIndex
        '    End If
        'End Sub

        ''Column width change event in gvMain
        'Private Sub gvMain_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles Me.ColumnWidthChanged
        '    If Me._GVFoot IsNot Nothing Then
        '        Dim CName As String = e.Column.Name & "Foot"
        '        If Me._GVFoot.Columns.Count > 0 Then Me._GVFoot.Columns.Item(CName).Width = e.Column.Width
        '        AdjustForScrollBar(Me, Me._GVFoot)
        '    End If

        'End Sub



#End Region
    End Class


End Namespace
