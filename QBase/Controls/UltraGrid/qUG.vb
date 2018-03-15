Imports System.Windows.Forms
Imports System.Drawing
Imports Infragistics.Win.UltraWinGrid


<System.Serializable(), _
 System.ComponentModel.ComplexBindingProperties("DataSource", "DataMember")> _
Public Class qUG
    Inherits UltraGrid

    Public Sub New()
    End Sub

#Region " KeyFields Property "

    ' Defines the edges of the container to which a certain control is bound.  When a control is anchored to an edge, the distance between the control's closest edge and the specified edge will remain constant.
    Private _KeyFieldsVar As String = String.Empty
    <System.ComponentModel.Category("Data"), _
            System.ComponentModel.Description("ColumnName1|ColumnName2...  List the column names that make up the key fields"), _
            System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
            System.ComponentModel.DefaultValue("")> _
    Public Overridable Property _KeyFields() As String
        Get
            Return _KeyFieldsVar
        End Get
        Set(ByVal value As String)
            _KeyFieldsVar = value
        End Set
    End Property
#End Region

#Region " Show Selection Bar "

    ' Defines the edges of the container to which a certain control is bound.  When a control is anchored to an edge, the distance between the control's closest edge and the specified edge will remain constant.
    Private _ShowSelectionBarVar As Boolean = True
    <System.ComponentModel.Category("Appearance"), _
            System.ComponentModel.Description("Enter false to hide selection bar"), _
            System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
            System.ComponentModel.DefaultValue("")> _
    Public Overridable Property _ShowSelectionBar() As Boolean
        Get
            Return _ShowSelectionBarVar
        End Get
        Set(ByVal value As Boolean)
            _ShowSelectionBarVar = value
        End Set
    End Property
#End Region


#Region "_EditFormName Property points to Edit Form to instantiate when user drills down in list"
    Private _ListEditFormNameVar As String = String.Empty
    <System.ComponentModel.Category("Data"), _
            System.ComponentModel.Description("The name of the Edit Form object to open when the user double-clicks a list line."), _
            System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
            System.ComponentModel.DefaultValue("")> _
            Public Overridable Property _ListEditFormName() As String
        Get
            Return _ListEditFormNameVar
        End Get
        Set(ByVal value As String)
            _ListEditFormNameVar = value
        End Set
    End Property
#End Region

End Class



