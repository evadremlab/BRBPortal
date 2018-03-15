Imports System.Windows.Forms

Namespace Windows.Forms


    'Quartet CheckBox

    '<System.ComponentModel.DesignTimeVisible(False)> _
    <System.Serializable(), _
System.ComponentModel.DefaultBindingProperty("Text")> _
Public Class qCheckBox
    Inherits System.Windows.Forms.CheckBox

    'BHS 6/11/10  iSetWriterLevel = 0 when coming from SetFormAttributes or when programmer doesn't care.  
    '   For DRs, 1 if set in DrawItem and 2 if explicitly set elsewhere in the code.
    Public iSetWriterLevel As Integer = 0

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

#Region " DataType Property "
    Private _DataTypeVar As DataTypeEnum = DataTypeEnum.Str
    <System.ComponentModel.Category("Data"), _
     System.ComponentModel.Description("Used to generate SQL to save reporting temp tables back to the database."), _
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
     System.ComponentModel.DefaultValue(GetType(DataTypeEnum), "Num")> _
    Public Overridable Property _DataType() As DataTypeEnum
        Get
            Return _DataTypeVar
        End Get
        Set(ByVal value As DataTypeEnum)
            _DataTypeVar = value
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

    'BHS Next three added 8/15/08

#Region " IsKeyField Property "

    'Query Field holds Table.Field.Type for building query SQL
    Private IsKeyField As Boolean = False
    <System.ComponentModel.Category("Data"), _
            System.ComponentModel.Description("True if this field should be presented as a Key field when the New button is pressed."), _
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

#Region " TransparentDisplay Property "
    Private _TransparentDisplayVar As Boolean = False
    <System.ComponentModel.Category("Behavior"), _
     System.ComponentModel.Description("TransparentDisplay will remove border and make background QBackColor."), _
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
     System.ComponentModel.DefaultValue(False)> _
    Public Overridable Property _TransparentDisplay() As Boolean
        Get
            Return _TransparentDisplayVar
        End Get
        Set(ByVal value As Boolean)
            _TransparentDisplayVar = value
        End Set
    End Property
#End Region


    'BHS Next two added 8/18/08
#Region " CheckedTrueValue "

    'BindDef names a Field within a Binding Source, to match this control to an element in the Binding Source's datasource.
    Private _CheckedTrueValueVar As String = String.Empty
    <System.ComponentModel.Category("Data"), _
            System.ComponentModel.Description("Leave this blank to use regular 1=True, 0=Falue integer values.  Enter to force a different database string value for Checked = True."), _
            System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
            System.ComponentModel.DefaultValue("")> _
            Public Overridable Property _CheckedTrueValue() As String
        Get
            Return _CheckedTrueValueVar
        End Get
        Set(ByVal value As String)
            _CheckedTrueValueVar = value
        End Set
    End Property

#End Region

#Region " CheckedFalseValue "

    'BindDef names a Field within a Binding Source, to match this control to an element in the Binding Source's datasource.
    Private _CheckedFalseValueVar As String = String.Empty
    <System.ComponentModel.Category("Data"), _
            System.ComponentModel.Description("Leave this blank to use regular 1=True, 0=Falue integer values.  Enter to force a different database string value for Checked = False."), _
            System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
            System.ComponentModel.DefaultValue("")> _
            Public Overridable Property _CheckedFalseValue() As String
        Get
            Return _CheckedFalseValueVar
        End Get
        Set(ByVal value As String)
            _CheckedFalseValueVar = value
        End Set
    End Property

#End Region

#Region "Events"

    'Make a visible change when the checkbox gets focus
    Private Sub qCheckBox_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Enter
        FlatStyle = System.Windows.Forms.FlatStyle.Popup
    End Sub

    Private Sub qCheckBox_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Leave
        FlatStyle = System.Windows.Forms.FlatStyle.Standard
    End Sub

#End Region


#Region "Functions"
    Public Sub SetWriter(ByVal aWriter As Boolean, Optional ByVal aSetWriterLevel As Integer = 0)
        'Don't act if a higher SetWriterLevel has already been set for this control
        If aSetWriterLevel < iSetWriterLevel Then Return
        iSetWriterLevel = aSetWriterLevel

        If aWriter = True Then
            Enabled = True
            TabStop = True
        Else
            Enabled = False   'Prevents typing
            TabStop = False   'Prevents user tabbing to control
        End If
    End Sub
#End Region

End Class


End Namespace