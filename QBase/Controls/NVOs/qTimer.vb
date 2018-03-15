Imports System.Windows.Forms

''' <summary>
''' Represents a custom Windows text box control.
''' </summary>
''' <remarks></remarks>
<System.Serializable(), _
 System.ComponentModel.DefaultBindingProperty("EventName")> _
Public Class qTimer
    Inherits System.Windows.Forms.Timer

#Region " EventName Property "
    Private _eventNameVar As String
    ' Defines event to run when timer ticks
    <System.ComponentModel.Description("Event to run when timer ticks"), _
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
     System.ComponentModel.DefaultValue("")> _
    Public Overridable Property _EventName() As String
        Get
            Return _eventNameVar
        End Get
        Set(ByVal value As String)
            _eventNameVar = value
        End Set
    End Property
#End Region

#Region " Params Property "
    Private _paramVar As Object
    ' Defines event to run when timer ticks
    <System.ComponentModel.Description("Pack any event parameters into this property"), _
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
     System.ComponentModel.DefaultValue("")> _
    Public Overridable Property _Param() As Object
        Get
            Return _paramVar
        End Get
        Set(ByVal value As Object)
            _paramVar = value
        End Set
    End Property
#End Region

End Class
