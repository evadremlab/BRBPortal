Imports System.Windows.Forms

Namespace Windows.Forms

    ' Insert Row Button that manages a gridview
    Public Class qMenuItem
        Inherits System.Windows.Forms.ToolStripMenuItem

#Region "FormName points to object to run"
        Private FormName As String = String.Empty
        <System.ComponentModel.Category("Data"),
            System.ComponentModel.Description("The object to be opened when the user clicks this menu item."),
            System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible),
            System.ComponentModel.DefaultValue("")>
        Public Overridable Property _FormName() As String
            Get
                Return FormName
            End Get
            Set(ByVal value As String)
                FormName = value
            End Set
        End Property
#End Region

#Region "FunctNo defines the menu item's function number"
        Private FunctNo As String = ""
        <System.ComponentModel.Category("Data"),
            System.ComponentModel.Description("The function number to assign for this item"),
            System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible),
            System.ComponentModel.DefaultValue("")>
        Public Overridable Property _FunctNo() As String
            Get
                Return FunctNo
            End Get
            Set(ByVal value As String)
                FunctNo = value
            End Set
        End Property
#End Region
#Region "ButtonName points to button click"
        Private ButtonName As String = String.Empty
        <System.ComponentModel.Category("Data"),
            System.ComponentModel.Description("The button to tie this menu item to."),
            System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible),
            System.ComponentModel.DefaultValue("")>
        Public Overridable Property _ButtonName() As String
            Get
                Return ButtonName
            End Get
            Set(ByVal value As String)
                ButtonName = value
            End Set
        End Property
#End Region

        'BHS Added 8/18/08
#Region "InDevelopment prevents menu item from being accessed by end users"
        Private InDevelopmentVar As Boolean = False
        <System.ComponentModel.Category("Data"),
            System.ComponentModel.Description("Make this True to prevent end users from seeing this menu option."),
            System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible),
            System.ComponentModel.DefaultValue("")>
        Public Overridable Property _InDevelopment() As Boolean
            Get
                Return InDevelopmentVar
            End Get
            Set(ByVal value As Boolean)
                InDevelopmentVar = value
            End Set
        End Property
#End Region

    End Class

End Namespace
