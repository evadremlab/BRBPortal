Imports System.ComponentModel
Imports System.Windows.Forms

'<System.ComponentModel.DesignTimeVisible(False)> _
<System.Serializable(), _
System.ComponentModel.DefaultBindingProperty("_DBText")> _
Public Class qRC
    Inherits System.Windows.Forms.GroupBox
    Implements INotifyPropertyChanged

#Region " Documentation "
    'qRC - Radio Button Control
    'BHS 7/12/2012
    'qRC is designed to hold any number of qRB radio buttons.  Outside programs
    '   interface with this qRC, not the underlying qRB buttons.  ._DBText holds
    '   a text representation of the current value of this control.  Changes to that
    '   value (either by the user clicking a new qRB or the program setting the
    '   value) will result in a redisplay of the qRB button checks.
    '
    'Dependencies:
    '   1) Each qRB inside the qRC groupbox must have a unique._DBText value.
    '   2) Each qRB's parent must be this qRC (no panels or tabs in the middle)
    '
    'If ._DBText does not match any qRB._DBText values, then no qRBs will be checked.


    '_DBText holds the binding value for the control.  A change to _DBText will cause
    '   the child radio buttons' checked property to be refreshed.  If this control is
    '   in a qDR, the qDR.ItemValuePushed event is triggered.

#End Region
    
#Region " DBText Property "

    'DBText holds the database value associated with this control.
    Private _DBTextVar As String = String.Empty
    <System.ComponentModel.Category("Data"), _
            System.ComponentModel.Description("Current Value of this control that will be bound to the database."), _
            System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
            System.ComponentModel.DefaultValue("")> _
            Public Overridable Property _DBText() As String
        Get
            Return _DBTextVar
        End Get
        Set(ByVal value As String)
            _DBTextVar = value
            SetRBChecks(value)     'Check qRB that matches this value

            'Manually trigger property changed, so Data Repeater ItemValuePushed event
            '   will be triggered
            NotifyPropertyChanged("_DBText")

            'If Me.DataBindings.Count > 0 Then
            '    Me.DataBindings(0).WriteValue()
            'End If
        End Set
    End Property

    'NotifyPropertyChanged
    Private Sub NotifyPropertyChanged(ByVal aPropertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(aPropertyName))
    End Sub

    Public Event PropertyChanged(ByVal sender As Object, _
                                 ByVal e As PropertyChangedEventArgs) _
                                 Implements INotifyPropertyChanged.PropertyChanged

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
            System.ComponentModel.Description("Tooltip help for this radio controlbox."), _
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

#Region " Functions "
    Public Function Clear() As Boolean
        Dim RB As qRB
        For Each C As Control In Me.Controls
            RB = TryCast(C, qRB)
            If RB IsNot Nothing Then
                RB.Checked = False
            End If
        Next
        Return True
    End Function

    Public Function SetRBChecks(ByVal aValue As String) As Boolean
        Dim RB As qRB
        For Each C As Control In Me.Controls
            RB = TryCast(C, qRB)
            If RB IsNot Nothing Then
                If RB._DBText = aValue Then
                    RB.Checked = True
                Else
                    RB.Checked = False
                End If
            End If
        Next
        Return True
    End Function


    Public Sub SetWriter(ByVal aWriter As Boolean, Optional ByVal aSetWriterLevel As Integer = 0)
        ''Don't act if a higher SetWriterLevel has already been set for this control
        'If aSetWriterLevel < iSetWriterLevel Then Return
        'iSetWriterLevel = aSetWriterLevel

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
