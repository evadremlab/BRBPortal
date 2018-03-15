Imports System.Windows.Forms

' Quartet Text Box
Namespace Windows.Forms
    <System.Serializable(),
 System.ComponentModel.DefaultBindingProperty("Text")>
    Public Class qTextBox
        Inherits System.Windows.Forms.TextBox

        'BHS 6/11/10  iSetWriterLevel = 0 when coming from SetFormAttributes or when programmer doesn't care.  
        '   For DRs, 1 if set in DrawItem and 2 if explicitly set elsewhere in the code.
        Public iSetWriterLevel As Integer = 0

        Sub New()   'Make textboxes the same height as comboboxes
            Me.AutoSize = False
            Me.Height = 21
        End Sub

#Region " TextCase Property "
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

#Region " QueryDef Property "

        'Query Field holds Table.Field.Type for building query SQL
        Private _QueryDefVar As String = String.Empty
        <System.ComponentModel.Category("Data"),
            System.ComponentModel.Description("TableName.FieldName[.Type] where type is optional, and may be Str, Num or Dat.  Used to build query SQL."),
            System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible),
            System.ComponentModel.DefaultValue("")>
        Public Overridable Property _QueryDef() As String
            Get
                Return _QueryDefVar
            End Get
            Set(ByVal value As String)
                _QueryDefVar = value
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

#Region " TransparentDisplay Property "
        Private _TransparentDisplayVar As Boolean = False
        <System.ComponentModel.Category("Behavior"),
     System.ComponentModel.Description("TransparentDisplay will remove border and make background QBackColor."),
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible),
     System.ComponentModel.DefaultValue(False)>
        Public Overridable Property _TransparentDisplay() As Boolean
            Get
                Return _TransparentDisplayVar
            End Get
            Set(ByVal value As Boolean)
                _TransparentDisplayVar = value
            End Set
        End Property
#End Region

#Region " Format Property "
        Private _formatVar As String = ""
        <System.ComponentModel.Category("Format"),
     System.ComponentModel.Description("Enter a Format String for a date or number.  Must be used in conjunction with the DataType property"),
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible),
     System.ComponentModel.DefaultValue(False)>
        Public Overridable Property _Format() As String
            Get
                Return _formatVar
            End Get
            Set(ByVal value As String)
                _formatVar = value
            End Set
        End Property
#End Region

#Region " Tooltip Property "

        'Tooltip property holds online help for this textbox
        Private _toolTipVar As String = String.Empty
        <System.ComponentModel.Category("Data"),
            System.ComponentModel.Description("Tooltip help for this textbox."),
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

#Region " FormatDate Property "
        Private _formatDateVar As Boolean = False
        ' Enforces the validation rule that the Text value must be a valid date.

        <System.ComponentModel.Category("Format"),
     System.ComponentModel.Description("Formats a data as it is bound and during Control Validation."),
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible),
     System.ComponentModel.DefaultValue(False)>
        Public Overridable Property _FormatDate() As Boolean
            Get
                Return _formatDateVar
            End Get
            Set(ByVal value As Boolean)
                _formatDateVar = value
            End Set
        End Property
#End Region

#Region " FormatNumber Property "
        Private _formatNumberVar As String = ""
        ' Enforces the validation rule that the Text value must be a valid date.

        <System.ComponentModel.Category("Format"),
     System.ComponentModel.Description("Formats a number based on the string you supply."),
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible),
     System.ComponentModel.DefaultValue(False)>
        Public Overridable Property _FormatNumber() As String
            Get
                Return _formatNumberVar
            End Get
            Set(ByVal value As String)
                _formatNumberVar = value
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
                If _ValidateDate = True Then
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

#Region " ValidateRequired Property "
        Private _validateNotEmpty As Boolean = False
        ''' <summary>
        ''' Enforces the validation rule that the Text value must not be empty.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
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

#Region " AutoSize property starts as False and is visible in designer "   'BHS 6/27/08
        'Overrides the textbox class, making AutoSize viewable in intellesense.  For some reason it doesn't show in the designer
        <System.ComponentModel.Category("Layout"),
     System.ComponentModel.Description("Set to false to allow sizing of the textbox"),
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible),
     System.ComponentModel.DefaultValue(False)>
        Public Overrides Property AutoSize() As Boolean
            Get
                Return MyBase.AutoSize
            End Get
            Set(ByVal value As Boolean)
                MyBase.AutoSize = value
            End Set
        End Property
#End Region

#Region " TextBox Control Events "

        Private Sub qTextBox_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
            'BHS 10/25/10
            If e.KeyData = gSelectAllCode Then
                SelectAll()
                Return
            End If
        End Sub

        Private Sub TextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.TextChanged

            Dim start As Integer = Me.SelectionStart

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


        End Sub

        '12/11/06 BHS This logic in fBase.ValidateControl
        'Private Sub TextBox_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Me.Validating

        '    Dim valid As Boolean = True

        '    ' Check for Date Validation
        '    If _validateDate = True Then
        '        If Microsoft.VisualBasic.IsDate(Me.Text) = False Then
        '            valid = False
        '            MessageBox.Show("Value must be a valid date.", "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '        End If
        '    End If
        '    ' Check for Number Validation
        '    If _validateNumber = True Then
        '        If Microsoft.VisualBasic.IsNumeric(Me.Text) = False Then
        '            valid = False
        '            MessageBox.Show("Value must be a valid number.", "Invalid Number", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '        End If
        '    End If
        '    ' Check for Not Empty Validation
        '    If _validateNotEmpty = True Then
        '        If Me.Text.Trim = String.Empty Then
        '            valid = False
        '            MessageBox.Show("Value must not be empty.", "Missing Value", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '        End If
        '    End If

        '    If valid = False Then
        '        e.Cancel = True
        '    End If

        'End Sub
#End Region

#Region " Functions (SetWriter, SetTransparentDisplay, SetTopSecret) "
        'Set control attributes based on whether user is a writer or not
        Public Sub SetWriter(ByVal aWriter As Boolean, Optional ByVal aSetWriterLevel As Integer = 0)
            'Don't act if a higher SetWriterLevel has already been set for this control
            If aSetWriterLevel < iSetWriterLevel Then Return
            iSetWriterLevel = aSetWriterLevel

            If aWriter = True Then
                Me.ReadOnly = False
                'Me._ReadAlways = False '...SDC 10/12/2009
                Me.TabStop = True
                Me.BackColor = QEntryBackColor
            Else
                Me.ReadOnly = True   'Prevents typing
                'Me._ReadAlways = True '...SDC 10/12/2009
                Me.TabStop = False   'Prevents user tabbing to control
                Me.BackColor = QReadOnlyBackColor
            End If
        End Sub

        'Set attributes to make a control transparent
        Public Sub SetTransparentDisplay()
            Me.ReadOnly = True
            Me.TabStop = False
            Me.BackColor = QBackColor
            Me.BorderStyle = System.Windows.Forms.BorderStyle.None
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





    End Class
End Namespace
