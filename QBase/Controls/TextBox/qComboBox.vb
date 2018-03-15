Namespace Windows.Forms
    <System.ComponentModel.LookupBindingProperties("DataSource", "DisplayMember", "ValueMember", "SelectedValue")>
    Public Class qComboBox
        Inherits System.Windows.Forms.ComboBox
        Private iOrigText As String = ""
        Private iOrigDDStyle As ComboBoxStyle

        'BHS 6/11/10  iSetWriterLevel = 0 when coming from SetFormAttributes or when programmer doesn't care.  
        '   For DRs, 1 if set in DrawItem and 2 if explicitly set elsewhere in the code.
        Public iSetWriterLevel As Integer = 0


#Region " QueryDef Property "

        'Query Field holds Table.Field.Type for building query SQL
        Private _QueryDefinition As String = String.Empty
        <System.ComponentModel.Category("Data"),
            System.ComponentModel.Description("TableName.FieldName[.Type] where type is optional, and may be Str, Num or Dat.  Used to build query SQL."),
            System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible),
            System.ComponentModel.DefaultValue("")>
        Public Overridable Property _QueryDef() As String
            Get
                Return _QueryDefinition
            End Get
            Set(ByVal value As String)
                _QueryDefinition = value
            End Set
        End Property

#End Region

#Region " QueryDescr Property "

        'Query Field holds Table.Field.Type for building query SQL
        Private _QueryDescrVar As String = String.Empty
        <System.ComponentModel.Category("Data"),
            System.ComponentModel.Description("Field description to use in iSQLDescr"),
            System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible),
            System.ComponentModel.DefaultValue("")>
        Public Overridable Property _QueryDescr() As String
            Get
                Return _QueryDescrVar
            End Get
            Set(ByVal value As String)
                _QueryDescrVar = value
            End Set
        End Property

#End Region

#Region " Tooltip Property "

        'Tooltip property holds online help for this textbox
        Private _toolTipVar As String = String.Empty
        <System.ComponentModel.Category("Data"),
            System.ComponentModel.Description("Tooltip help for this control."),
            System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible),
            System.ComponentModel.DefaultValue("")>
        Public Overridable Property _ToolTip() As String
            Get
                Return _toolTipVar
            End Get
            Set(ByVal value As String)
                _toolTipVar = value
            End Set
        End Property

#End Region

#Region " ValidateDate Property "
        Private _validateDateVar As Boolean = False
        ' Enforces the validation rule that the Text value must be a valid date.
        <System.ComponentModel.Category("Validation"),
     System.ComponentModel.Description("Enforces the validation rule that the Text value must be a valid date."),
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible),
     System.ComponentModel.DefaultValue(False)>
        Public Overridable Property _ValidateDate() As Boolean
            Get
                Return _validateDateVar
            End Get
            Set(ByVal value As Boolean)
                _validateDateVar = value
                If _validateDateVar = True Then
                    ' Turn Off Number Validation
                    _ValidateNumber = False
                End If
            End Set
        End Property
#End Region

#Region " ValidateNumber Property "
        Private _validateNumberVar As Boolean = False
        ' Enforces the validation rule that the Text value must be a valid number.
        <System.ComponentModel.Category("Validation"),
     System.ComponentModel.Description("Enforces the validation rule that the Text value must be a valid number."),
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible),
     System.ComponentModel.DefaultValue(False)>
        Public Overridable Property _ValidateNumber() As Boolean
            Get
                Return _validateNumberVar
            End Get
            Set(ByVal value As Boolean)
                _validateNumberVar = value
                If _ValidateNumber = True Then
                    ' Turn Off Date Validation
                    _ValidateDate = False
                End If
            End Set
        End Property
#End Region


#Region " ValidateMaxValue Property "   'BHS 6/24/08
        Private _validateMaxValueVar As String = ""
        ' Sets the top of a range for a number or date.  Recorded as a string and converted when used
        <System.ComponentModel.Category("Validation"),
     System.ComponentModel.Description("Sets the highest allowed number or date if you use ValidateDate or ValidateNumber."),
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible),
     System.ComponentModel.DefaultValue(False)>
        Public Overridable Property _ValidateMaxValue() As String
            Get
                Return _validateMaxValueVar
            End Get
            Set(ByVal value As String)
                _validateMaxValueVar = value
            End Set
        End Property
#End Region

#Region " ValidateMinValue Property "   'BHS 6/24/08
        Private _validateMinValueVar As String = ""
        ' Sets the bottom of a range for a number or date.  Recorded as a string and converted when used
        <System.ComponentModel.Category("Validation"),
     System.ComponentModel.Description("Sets the lowest allowed number or date if you use ValidateDate or ValidateNumber."),
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible),
     System.ComponentModel.DefaultValue(False)>
        Public Overridable Property _ValidateMinValue() As String
            Get
                Return _validateMinValueVar
            End Get
            Set(ByVal value As String)
                _validateMinValueVar = value
            End Set
        End Property
#End Region

#Region " BindDef Property "

        'BindDef names a Field within a Binding Source, to match this control to an element in the Binding Source's datasource.
        Private _BindDefVar As String = String.Empty
        <System.ComponentModel.Category("Data"),
            System.ComponentModel.Description("Field (Table.Field or just Field) in the code-behind select that will bind this control."),
            System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible),
            System.ComponentModel.DefaultValue("")>
        Public Overridable Property _BindDef() As String
            Get
                Return _BindDefVar
            End Get
            Set(ByVal value As String)
                _BindDefVar = value
            End Set
        End Property

#End Region

#Region " ValidateNotEmpty Property "
        Private _validateNotEmpty As Boolean = False
        ' Enforces the validation rule that the Text value must not be empty.

        <System.ComponentModel.Category("Validation"),
     System.ComponentModel.Description("Enforces the validation rule that the Text value must not be empty."),
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible),
     System.ComponentModel.DefaultValue(False)>
        Public Overridable Property _ValidateRequired() As Boolean
            Get
                Return _validateNotEmpty
            End Get
            Set(ByVal value As Boolean)
                _validateNotEmpty = value
            End Set
        End Property

#End Region

#Region " DataType Property "
        Private _DataTypeVar As DataTypeEnum = DataTypeEnum.Str
        <System.ComponentModel.Category("Data"),
     System.ComponentModel.Description("Used to generate SQL to save reporting temp tables back to the database."),
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible),
     System.ComponentModel.DefaultValue(GetType(DataTypeEnum), "Str")>
        Public Overridable Property _DataType() As DataTypeEnum
            Get
                Return _DataTypeVar
            End Get
            Set(ByVal value As DataTypeEnum)
                _DataTypeVar = value
            End Set
        End Property
#End Region

#Region " TextCase Property "   'BHS Readded 4/7/08 to support all caps multi-col comboboxes
        Private _textCaseVar As TextCaseEnum = TextCaseEnum.Normal
        ' Applies automatic character casing to the Text property.

        <System.ComponentModel.Category("Appearance"),
     System.ComponentModel.Description("Applies automatic character casing to the Text property."),
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible),
     System.ComponentModel.DefaultValue(GetType(TextCaseEnum), "Normal")>
        Public Overridable Property _TextCase() As TextCaseEnum
            Get
                Return _textCaseVar
            End Get
            Set(ByVal value As TextCaseEnum)
                _textCaseVar = value
            End Set
        End Property
#End Region

#Region " ReadOnly Property "
        Private _ReadOnlyVar As Boolean = False
        ' Dynamically set in the program to change a combo box's appearance and behavior (_ReadAlways sets it permanently)
        <System.ComponentModel.Category("Behavior"),
     System.ComponentModel.Description("Use _ReadAlways to set control permanently.  _ReadOnly gets set dynamically in the program."),
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible),
     System.ComponentModel.DefaultValue(False)>
        Public Overridable Property _ReadOnly() As Boolean
            Get
                Return _ReadOnlyVar
            End Get
            Set(ByVal value As Boolean)
                _ReadOnlyVar = value
            End Set
        End Property
#End Region

#Region " ReadAlways Property "
        Private _ReadAlwaysVar As Boolean = False
        ' Set ReadOnly = True permanently (_ReadOnly changes dynamically in the program_
        <System.ComponentModel.Category("Behavior"),
     System.ComponentModel.Description("Use _ReadAlways to set control permanently.  _ReadOnly gets set dynamically in the program."),
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible),
     System.ComponentModel.DefaultValue(False)>
        Public Overridable Property _ReadAlways() As Boolean
            Get
                Return _ReadAlwaysVar
            End Get
            Set(ByVal value As Boolean)
                _ReadAlwaysVar = value
            End Set
        End Property
#End Region

#Region " IsKeyField Property "

        'Query Field holds Table.Field.Type for building query SQL
        Private IsKeyField As Boolean = False
        <System.ComponentModel.Category("Data"),
            System.ComponentModel.Description("True if this field should be presented as a Key field when the New button is pressed."),
            System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible),
            System.ComponentModel.DefaultValue("")>
        Public Overridable Property _IsKeyField() As Boolean
            Get
                Return IsKeyField
            End Get
            Set(ByVal value As Boolean)
                IsKeyField = value
            End Set
        End Property

#End Region

#Region " Functions (SetWriter, SetTopSecret) "
        'Set control attributes based on whether user is a writer or not
        Public Sub SetWriter(ByVal aWriter As Boolean, Optional ByVal aSetWriterLevel As Integer = 0)
            'Don't act if a higher SetWriterLevel has already been set for this control
            If aSetWriterLevel < iSetWriterLevel Then Return
            iSetWriterLevel = aSetWriterLevel

            If aWriter = True Then
                Me._ReadOnly = False
                'Me._ReadAlways = False '...SDC 10/12/2009
                Me.TabStop = True
                Me.BackColor = QEntryBackColor
            Else
                Me._ReadOnly = True   'Prevents typing
                'Me._ReadAlways = True '...SDC 10/12/2009
                Me.TabStop = False   'Prevents user tabbing to control
                Me.BackColor = QReadOnlyBackColor
                Me.SelectionLength = 0  'BHS 5/5/08 Don't leave RO field selected
                Me.SelectionStart = 0
            End If
        End Sub
        'Set attributes to make control disabled and data invisible
        Public Sub SetTopSecret() 'GBV 7/14/2008
            Me.Enabled = False
            Me.DataBindings.Clear()
            Me.BackColor = Color.Black
            Me.ForeColor = Color.Black
            Me.Text = ""
        End Sub
#End Region

#Region " Combobox Control Events "
        ''BHS 6/18/09
        'Sub New()
        '    ' Required for ownerdraw
        '    Me.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        '    iOrigDDStyle = Me.DropDownStyle

        'End Sub

        ''BHS 6/18/09
        'Private Sub qComboBox_DrawItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles Me.DrawItem

        '    Dim G As System.Drawing.Graphics = e.Graphics
        '    Dim R As Rectangle = e.Bounds
        '    If e.Index >= 0 Then
        '        Dim CBText As String = Me.Text
        '        If iOrigDDStyle = ComboBoxStyle.DropDownList Then CBText = Me.Items(e.Index).ToString


        '        'Disabled Control Drawing

        '        If e.State = (DrawItemState.Disabled Or DrawItemState.ComboBoxEdit) Then
        '            e.Graphics.FillRectangle(New SolidBrush(Color.Gray), R)
        '            G.DrawString(CBText, e.Font, Brushes.Black, R)
        '            e.DrawFocusRectangle()

        '            '    'Enabled Control Drawing
        '        ElseIf e.State = DrawItemState.NoAccelerator + DrawItemState.NoFocusRect Then
        '            e.Graphics.FillRectangle(New SolidBrush(Color.White), R)
        '            G.DrawString(CBText, e.Font, Brushes.Black, R)
        '            e.DrawFocusRectangle()

        '            'Focused Control Drawing
        '        Else
        '            e.Graphics.FillRectangle(New SolidBrush(Color.Yellow), R)
        '            'CBText = Convert.ToString(Me.DisplayMember.  'Deb - I need some way to keep the dropdown population.  This drawing
        '            'seems to blank out the data
        '            G.DrawString(CBText, e.Font, Brushes.Black, R)
        '            e.DrawFocusRectangle()
        '        End If
        '    End If
        '    G.Dispose()




        '    '      System.Drawing.Graphics g = e.Graphics;    
        '    'Rectangle r = e.Bounds;    
        '    '    If (e.Index >= 0) Then
        '    '{        
        '    '    string label = this.Items[e.Index].ToString();        
        '    '    // This is how we draw a disabled control        
        '    '    if (e.State == (DrawItemState.Disabled | DrawItemState.NoAccelerator 
        '    '            | DrawItemState.NoFocusRect | DrawItemState.ComboBoxEdit))   
        '    '    {            
        '    '        e.Graphics.FillRectangle(new SolidBrush(Color.White), r);      
        '    '        g.DrawString(label, e.Font, Brushes.Black, r);            
        '    '        e.DrawFocusRectangle();        
        '    '    }        

        '    '    // This is how we draw the items in an enabled control that aren't        // in focus        
        '    '    else if (e.State == (DrawItemState.NoAccelerator |                              DrawItemState.NoFocusRect))        
        '    '    {            
        '    '        e.Graphics.FillRectangle(new SolidBrush(Color.White), r); 
        '    '        g.DrawString(label, e.Font, Brushes.Black, r);            
        '    '        e.DrawFocusRectangle();        
        '    '    }        

        '    '    // This is how we draw the focused items        
        '    '        Else
        '    '    {            
        '    '        e.Graphics.FillRectangle(new SolidBrush(Color.Blue), r); 
        '    '        g.DrawString(label, e.Font, Brushes.White, r);            
        '    '        e.DrawFocusRectangle();        
        '    '    }    
        '    '}    
        '    'g.Dispose();


        'End Sub
        ''BHS 6/18/09
        'Private Sub qComboBox_EnabledChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.EnabledChanged
        '    If (Me.Enabled) Then
        '        Me.DropDownStyle = iOrigDDStyle
        '    Else
        '        Me.DropDownStyle = ComboBoxStyle.DropDownList
        '    End If


        'End Sub


        'These events allow a Reader to see the dropdown, but not make changes.  No typing is allowed in the text field.
        Private Sub qComboBox_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DropDown
            iOrigText = Me.Text
        End Sub

        Private Sub qComboBox_DropDownClosed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DropDownClosed
            If Me._ReadOnly = True Then Me.Text = iOrigText
        End Sub



        Private Sub qComboBox_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
            If Me._ReadOnly = True Then e.SuppressKeyPress() = True
        End Sub

        'Private Sub qComboBox_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LostFocus
        '    Me.SelectionLength = 0
        'End Sub

        'Text case management   'BHS Readded 8/8/08
        Private Sub qComboBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.TextChanged

            Dim start As Integer = Me.SelectionStart
            Dim i As Integer = Me.SelectionLength

            ' Check for Text Case
            Select Case _textCaseVar
                Case TextCaseEnum.Cap1stLetter
                    ' Convert Text Value to Capitalize 1st Letter
                    Me.Text = Cap1stLetter(Me.Text)
                Case TextCaseEnum.Lower
                    ' Convert Text Value to Lower Case
                    Me.Text = Me.Text.ToLower
                Case TextCaseEnum.Upper
                    ' Convert Text Value to Upper Case
                    Me.Text = Me.Text.ToUpper
            End Select

            Me.SelectionStart = start
            Me.SelectionLength = i


        End Sub
#End Region

    End Class

End Namespace
