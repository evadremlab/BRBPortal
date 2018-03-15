' Insert Row Button that manages a gridview
Public Class qBtnInsertGVRow
    Inherits System.Windows.Forms.Button

#Region "_GVName Property points to Parent GV"
    Private _GridViewName As String = String.Empty
    <System.ComponentModel.Category("Data"), _
            System.ComponentModel.Description("The gridview that this button belongs to - Required."), _
            System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
            System.ComponentModel.DefaultValue("")> _
            Public Overridable Property _GVName() As String
        Get
            Return _GridViewName
        End Get
        Set(ByVal value As String)
            _GridViewName = value
        End Set
    End Property
#End Region


#Region " DrillDown Property "
    Private _DrillDownVar As Boolean = False
    <System.ComponentModel.Category("Data"), _
     System.ComponentModel.Description("True drills down to new form.  False inserts row in GV"), _
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
     System.ComponentModel.DefaultValue(False)> _
    Public Overridable Property _DrillDown() As Boolean
        Get
            Return _DrillDownVar
        End Get
        Set(ByVal value As Boolean)
            _DrillDownVar = value
        End Set
    End Property
#End Region

#Region "_SpecialCommand Property defines special button action"
    'Private _Command As String = String.Empty
    '<System.ComponentModel.Category("Data"), _
    '        System.ComponentModel.Description("Special button command - not required."), _
    '        System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
    '        System.ComponentModel.DefaultValue("")> _
    '        Public Overridable Property _SpecialCommand() As String
    '    Get
    '        Return _Command
    '    End Get
    '    Set(ByVal value As String)
    '        _Command = value
    '    End Set
    'End Property
#End Region

#Region "Events"
    Sub New()
        Me.Image = My.Resources.PLUS
        Me.Text = ""
        Me.Width = 15
        Me.Height = 12
        Me.ImageAlign = ContentAlignment.MiddleRight
        Me.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.FlatAppearance.BorderSize = 0
    End Sub

#End Region

End Class

