
Imports System.Windows.Forms

'Quartet CheckBox

'<System.ComponentModel.DesignTimeVisible(False)> _
<System.Serializable(), _
System.ComponentModel.DefaultBindingProperty("Text")> _
Public Class qRB
    Inherits System.Windows.Forms.RadioButton

#Region " Documentation "
    'qRB (Radio Button) 
    'BHS 7/12/2012
    'qRB is designed to be housed in a qRC (Radio Control).  Its _dbText property is
    '   set at design time, and the value is passed to the parent qRC if this qRB is
    '   checked.  If the qRB is unchecked, then "" is sent to the qRC parent.
    '
    'Assumptions required to make this work:
    '   1)  All qRB radio buttons in a qRC have different _DBText values.
    '   2)  Each qRB's parent is a qRC Radio Control.  Don't insert panels or tabs
    '         between a qRB and its parent qRC.
#End Region

#Region " DBText Property "

    'DBText defines the database value defined by this control.
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
            If Me.DataBindings.Count > 0 Then
                Me.DataBindings(0).WriteValue()
            End If
        End Set
    End Property

#End Region

#Region " Events "

    ''' <summary>Set qRC Parent._DBText if this radio button is checked or unchecked </summary>
    Private Sub qRB_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.CheckedChanged
        Dim RC As qRC = TryCast(Me.Parent, qRC)
        If RC IsNot Nothing Then
            If Me.Checked = True Then
                RC._DBText = Me._DBText
            End If
        End If
    End Sub

#End Region

End Class
