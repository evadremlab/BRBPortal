'BHS Namespace QSILib

' Represents a custom Windows control that allows the user to select a date and a time and to display the date and time with a specified format.

<System.ComponentModel.DefaultBindingProperty("Value")> _
Public Class qDateTimePicker
    Inherits System.Windows.Forms.DateTimePicker

    Private _emptyDate As Date = #1/1/1900# ' Default Empty Date

    Private WithEvents _qMaskedTextBox As qMaskedTextBox

#Region " ValueIsNothing Property "
    Private _valueIsNothing As Boolean = True
    ' Gets or sets whether the current date/time value for this control is Nothing.

    <System.ComponentModel.Category("Behavior"), _
     System.ComponentModel.Description("Gets or sets whether the current date/time value for this control is Nothing."), _
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
     System.ComponentModel.DefaultValue(True)> _
    Public Overridable Property ValueIsNothing() As Boolean
        Get
            ' Check for Nothing Value
            If MyBase.Value.ToShortDateString = _emptyDate.ToShortDateString _
                    Or _valueIsNothing = True Then
                Return True
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            If value = True Then
                ' Clear qMaskedTextBox Value
                _qMaskedTextBox.Text = String.Empty
                ' Show MaskedTextEdit
                _qMaskedTextBox.Visible = True
                ' Set Focus to qMaskedTextBox
                _qMaskedTextBox.Focus()
            Else
                ' Hide MaskedTextEdit
                _qMaskedTextBox.Visible = False
            End If
            ' Set ValueIsNothing to Value
            _valueIsNothing = value
        End Set
    End Property
#End Region

#Region " Value Property "
    ''' <summary>
    ''' Gets or sets the date/time value assigned to the control.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.DefaultValue(GetType(DateTime), Nothing)> _
    Public Overloads Property Value() As Date
        Get
            If _valueIsNothing = True Then
                ' Return Empty Date
                Return Nothing
            Else
                ' Return Base Value
                Return MyBase.Value
            End If
        End Get
        Set(ByVal value As Date)
            ' Check for EmptyDate
            If value = Nothing Then
                ' Set ValueIsNothing to True
                Me.ValueIsNothing = True
            ElseIf value.ToShortDateString = _emptyDate.ToShortDateString Then
                ' Set ValueIsNothing to True
                Me.ValueIsNothing = True
            Else
                ' Set ValueIsNothing to False
                Me.ValueIsNothing = False
                ' Set Base Value
                MyBase.Value = value
            End If
        End Set
    End Property
#End Region

#Region " Format Property "
    ''' <summary>
    ''' Gets or sets the format of the date and time displayed in the control.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.DefaultValue(GetType(System.Windows.Forms.DateTimePickerFormat), "Short")> _
    Public Overloads Property Format() As System.Windows.Forms.DateTimePickerFormat
        Get
            Return MyBase.Format
        End Get
        Set(ByVal value As System.Windows.Forms.DateTimePickerFormat)
            MyBase.Format = value
        End Set
    End Property
#End Region

#Region " BindDef Property "

    'BindDef describes a Field (Table.Field, or just Field), to match this control to an element in the select data set that will be bound to the form in the code behind.
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

#Region " Tooltip Property "

    'Tooltip property holds online help for this textbox
    Private _toolTipVar As String = String.Empty
    <System.ComponentModel.Category("Data"), _
            System.ComponentModel.Description("Tooltip help for this datetimepicker."), _
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

#Region " Constructor "
    Public Sub New()
        ' Check for EmptyDate Value in My.Settings

        'BHS If IsDate(My.Settings.EmptyDate) Then
        '_emptyDate = CType(My.Settings.EmptyDate, Date)
        'End If

        ' Initialize qMaskedTextBox
        Me._qMaskedTextBox = New qMaskedTextBox
        Me.SuspendLayout()
        '
        'qMaskedTextBox
        '
        Me._qMaskedTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._qMaskedTextBox.Font = Me.Font
        Me._qMaskedTextBox.Location = New System.Drawing.Point(-1, -1)
        Me._qMaskedTextBox.Name = "qMaskedTextBox"
        Me._qMaskedTextBox.Size = New System.Drawing.Size(Me.Width - 19, Me.Height)
        Me._qMaskedTextBox.Mask = "99/99/9999"
        Me._qMaskedTextBox.PromptChar = CType(" ", Char)
        Me._qMaskedTextBox.ValidatingType = GetType(System.DateTime)
        Me._qMaskedTextBox.TabIndex = 0
        '
        'DateTimePicker
        '
        Me.Controls.Add(Me._qMaskedTextBox)
        ' Set Format to Short
        Me.Format = System.Windows.Forms.DateTimePickerFormat.Short
        '' Set CustomFormat for EmptyDate
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub
#End Region

#Region " qMaskedTextBox Control Events "
    Private Sub _qMaskedTextBox_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles _qMaskedTextBox.Enter
        ' Set Cursor to First Character Position
        _qMaskedTextBox.Select(0, 0)
    End Sub

    Private Sub _qMaskedTextBox_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles _qMaskedTextBox.MouseDown
        ' Set Cursor to First Character Position
        _qMaskedTextBox.Select(0, 0)
    End Sub

    Private Sub _qMaskedTextBox_TypeValidationCompleted(ByVal sender As Object, ByVal e As System.Windows.Forms.TypeValidationEventArgs) Handles _qMaskedTextBox.TypeValidationCompleted
        ' Check for Invalid Input
        If e.IsValidInput = False Then
            ' Clear qMaskedTextBox
            _qMaskedTextBox.Text = String.Empty
        End If
    End Sub

    Private Sub _qMaskedTextBox_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles _qMaskedTextBox.Validated
        ' Check for Valid Date
        If IsDate(_qMaskedTextBox.Text) = True Then
            ' Set Value at Descendant Level
            Me.Value = CType(_qMaskedTextBox.Text, DateTime)
        End If
    End Sub
#End Region

#Region " DateTimePicker Control Events "
    Private Sub DateTimePicker_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        ' Check for Delete Key
        If e.KeyCode = System.Windows.Forms.Keys.Delete Then
            ' Clear Value
            Me.ValueIsNothing = True
        End If
    End Sub

    Private Sub DateTimePicker_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.ValueChanged
        ' Check for Base Value Equal to Empty Date
        If MyBase.Value.ToShortDateString = _emptyDate.ToShortDateString Then
            Me.ValueIsNothing = True
        Else
            Me.ValueIsNothing = False
        End If
    End Sub
#End Region

End Class
