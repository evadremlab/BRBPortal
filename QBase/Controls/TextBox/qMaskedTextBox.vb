<System.ComponentModel.DefaultBindingProperty("Text")> _
Public Class qMaskedTextBox
    Inherits System.Windows.Forms.MaskedTextBox

    'BHS 6/11/10  iSetWriterLevel = 0 when coming from SetFormAttributes or when programmer doesn't care.  
    '   For DRs, 1 if set in DrawItem and 2 if explicitly set elsewhere in the code.
    Public iSetWriterLevel As Integer = 0

#Region " Mask Property "
    ' Sets the string governing the input allowed for this control.
    <System.ComponentModel.Category("Behavior"), _
     System.ComponentModel.Description("Sets the string governing the input allowed for this control."), _
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
     System.ComponentModel.RefreshProperties(System.ComponentModel.RefreshProperties.All), _
     System.ComponentModel.DefaultValue("")> _
    Public Shadows Property Mask() As String
        Get
            Return MyBase.Mask
        End Get
        Set(ByVal value As String)
            MyBase.Mask = value
            Me.MaskType = GetMaskType(value)
        End Set
    End Property
#End Region

#Region " MaskType Property "
    Private _maskType As MaskTypeEnum
    ' Sets the string governing the input allowed for this control.
    <System.ComponentModel.Category("Behavior"), _
     System.ComponentModel.Description("Ties mask string to enumerated list of possibities."), _
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
     System.ComponentModel.RefreshProperties(System.ComponentModel.RefreshProperties.All), _
     System.ComponentModel.DefaultValue(GetType(MaskTypeEnum), "None")> _
    Public Overridable Property MaskType() As MaskTypeEnum
        Get
            Return _maskType
        End Get
        Set(ByVal value As MaskTypeEnum)
            _maskType = value
            If _maskType <> MaskTypeEnum.Custom Then
                MyBase.Mask = GetMask(value)
            End If
        End Set
    End Property
#End Region

#Region " TextMaskFormat Property "
    ''' <summary>
    ''' Indicates whether the string returned from the Text property includes literals and/or prompt characters.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Category("Behavior"), _
     System.ComponentModel.Description("Indicates whether the string returned from the Text property includes literals and/or prompt characters."), _
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
     System.ComponentModel.DefaultValue(GetType(System.Windows.Forms.MaskFormat), "ExcludePromptAndLiterals")> _
    Public Shadows Property TextMaskFormat() As System.Windows.Forms.MaskFormat
        Get
            Return MyBase.TextMaskFormat
        End Get
        Set(ByVal value As System.Windows.Forms.MaskFormat)
            MyBase.TextMaskFormat = value
        End Set
    End Property
#End Region

#Region " QueryDef Property "

    'Query Field holds Table.Field.Type for building query SQL
    Private _QueryDefVar As String = String.Empty
    <System.ComponentModel.Category("Data"), _
            System.ComponentModel.Description("TableName.FieldName[.Type] where type is optional, and may be Str, Num or Dat.  Used to build query SQL."), _
            System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
            System.ComponentModel.DefaultValue("")> _
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
    <System.ComponentModel.Category("Data"), _
            System.ComponentModel.Description("Field description to use in iSQLDescr"), _
            System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
            System.ComponentModel.DefaultValue("")> _
            Public Overridable Property _QueryDescr() As String
        Get
            Return _QueryDescrVar
        End Get
        Set(ByVal value As String)
            _QueryDescrVar = value
        End Set
    End Property

#End Region

#Region " BindDef Property "

    'BindDef names a Field within a Binding Source, to match this control to an element in the Binding Source's datasource.
    Private _BindDefVar As String = String.Empty
    <System.ComponentModel.Category("Data"), _
            System.ComponentModel.Description("Field (Table.Field or just Field) in the code-behind select that will bind this control."), _
            System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
            System.ComponentModel.DefaultValue("")> _
            Public Overridable Property _BindDef() As String
        Get
            Return _BindDefVar
        End Get
        Set(ByVal value As String)
            _BindDefVar = value
        End Set
    End Property

#End Region

#Region " IsKeyField Property "

    'Query Field holds Table.Field.Type for building query SQL
    Private IsKeyField As Boolean = False
    <System.ComponentModel.Category("Data"), _
            System.ComponentModel.Description("True if field should be presented as a Key field when in New/Edit modes"), _
            System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
            System.ComponentModel.DefaultValue("")> _
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
    <System.ComponentModel.Category("Behavior"), _
     System.ComponentModel.Description("Use _ReadAlways to set control permanently.  _ReadOnly gets set dynamically in the program."), _
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
     System.ComponentModel.DefaultValue(False)> _
    Public Overridable Property _ReadAlways() As Boolean
        Get
            Return _ReadAlwaysVar
        End Get
        Set(ByVal value As Boolean)
            _ReadAlwaysVar = value
        End Set
    End Property
#End Region

#Region " Tooltip Property "

    'Tooltip property holds online help for this textbox
    Private _toolTipVar As String = String.Empty
    <System.ComponentModel.Category("Data"), _
            System.ComponentModel.Description("Tooltip help for this textbox."), _
            System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
            System.ComponentModel.DefaultValue("")> _
            Public Overridable Property _ToolTip() As String
        Get
            Return _toolTipVar
        End Get
        Set(ByVal value As String)
            _toolTipVar = value
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
    <System.ComponentModel.Category("Validation"), _
     System.ComponentModel.Description("Enforces the validation rule that the Text value must not be empty."), _
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
     System.ComponentModel.DefaultValue(False)> _
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
    <System.ComponentModel.Category("Data"), _
     System.ComponentModel.Description("Used to generate SQL to save reporting temp tables back to the database."), _
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
     System.ComponentModel.DefaultValue(GetType(DataTypeEnum), "Str")> _
    Public Overridable Property _DataType() As DataTypeEnum
        Get
            Return _DataTypeVar
        End Get
        Set(ByVal value As DataTypeEnum)
            _DataTypeVar = value
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
    'Set attributes to make control disabled and data invisible
    Public Sub SetTopSecret() 'GBV 7/14/2008
        Me.Enabled = False
        Me.DataBindings.Clear()
        Me.BackColor = Color.Black
        Me.ForeColor = Color.Black
        Me.Text = ""
    End Sub
#End Region

#Region " Constructor "
    Public Sub New()
        ' Initialize qMaskedTextBox
        MyBase.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals
    End Sub
#End Region

    'BHS 11/20/09
    'When clicking into a Masked Text box, if there are blanks to the left of the insertion point, go to the first blank.
    Private Sub qMaskedTextBox_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Click
        For i = 1 To Text.Length
            If MyBase.SelectionStart < i Then Return 'If no blanks enountered before selection point, leave selection point alone
            If Mid(Text, i, 1) = " " Then
                MyBase.SelectionStart = i - 1
                MyBase.SelectionLength = 0
                Return
            End If
        Next
    End Sub

    Private Sub qMaskedTextBox_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        'BHS 10/25/10
        If e.KeyData = gSelectAllCode Then
            SelectAll()
            Return
        End If
    End Sub
End Class
