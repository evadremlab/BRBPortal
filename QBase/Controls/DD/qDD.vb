#Region "Documentation"
' Class to provide a multi-column combo box. 
' After placing the control in a form, you need to do
' the following to load it with data:
' ddBox.AddColumn(<column name>, Optional column width, optional column header name)
' ... as many as needed
' ddBox.DataSource = BuildDV(SQL, True/False)
' By default, the dropdown will automatically size to accommodate the largest value.
' This cam be overriden by setting the _AutoAdjustColumns to False.
' 
'NotifyPropertyChanged alerts the host of this custom control that a property has been changed.  This will trigger
'a DataRepeater's ItemValuePushed event.  NotifyPropertyChanged is triggered when the textbox.Validated event fires.

'BHS 1/29/10
'1) Modifier for txtCode and btnDD must be Friend, not Public.  If modifier is public, datarepeater acts like
'the control is not enabled.  (Why?)
'
'2) Changed Overrides Text property to TextInfo, so that PropertyChanged event would get seen by datarepeater.
'It was probably a mistake to override an existing Text property.  This means we'll have to change all our qDR.Text
'references to qDR.TextInfo.
'
'3) Gabriel should look at how to fill the gvList at the moment the datasource is assigned, rather than wait for
'gvList to become visible.  qDR needs gvList to be populated when assigning initial values when MustMatch = True.

'BHS 2/16/10 - added OnDRDropdown to handle ondropdown inside a DR

'BHS 3/5/10 - Added DisplayMember and ValueMember properties to avoid seeing error when designer displays a qDD

'BHS 3/8/10 - Timing issues:
'Goal - don't fire Me.Validating when popup is being opened:
'   On btnDD.Click, Set _MouseDownJustPressed = True, and run timer to turn it back to False.  
'   If _MouseDownJustPressed = True then OnValidating doesn't fire.
'Goal - don't re-open popup if it is already open and user clicks dropdown.  Clicking dropdown causes 
'Popup to lose focus and close automatically, so:
'On Popup.Close, Set _PopupJustClose = True, and run timer to turn it back to False.  If _PopupJustClose = True then ignore btnDD.Click.

'BHS 3/10/10 - We must detect the difference between leaving the popup for the Composite (MouseDown in btnDD or txtCode) and 
'  leaving it for another field.  We want CompositeValidating to only fire in the second case.  In the btnDD.MouseDown Event, we 
'  set _MouseDownJustPressed = True, and do an Application.DoEvents in Me.Validating to see if it has been set.  We only fire
'  CompositeValidating if it hasn't been set.

'BHS 3/12/10 - Only RaiseEvent CompositeValidating once, by setting a timer to ignore a repetition within a short time.

'BHS 3/15/10 - Set iGV = New DataGridView and refresh columns, etc. every time DataSource is set.  This solves a nasty problem that
'  the iGV is unchangeable after its parent has been moved from fBase to iPopup.
'BHS 3/16/10 - Revers 3/15 change, re-assigning iGV.Parent when iPopup is closing



' Issues remaining
' (1) qDD's parent tab must be current before loading iGV
' (2) Sorting and searching logic when headers are visible
' (3) Make txtCode and btnDD private so the programmer is not tempted to mess with them.
#End Region

Imports System.ComponentModel
Imports QSILib.Windows.Forms
Imports System.Windows.Forms

'See QSIServer F:\QSI\CodingStandard\How To Use qDD and qDR for qDD behavior, and how to use it in common situations.

'BHS 10/8/12 - In VS2010, qDR.ItemValuePushed fires as soon as TextInfo changes.  This is different from VS2008,
'   where qDR.ItemValuePushed happens as you leave the control.  In some cases in 2010, qDR.ItemValuePushed can happen before
'   qDD.iSelectedValue is set, and for MustMatch=True, the old iSelectedValue gets pushed to the qDR table.  To fix this,
'   qDD now calls NotifyPropertyChanged("TextInfo") after iSelectedValue gets set in Set Property SelectedValue and 
'   Set Property SelectedIndex.  This makes more calls to qDR.ItemValuePushed, but guarantees that it will be called 
'   after the qDD SelectedValue has been set.

<System.ComponentModel.LookupBindingProperties("DataSource", "DisplayMember", "ValueMember", "SelectedValue")> _
<Bindable(True), DefaultBindingProperty("TextInfo")> _
Public Class qDD
    Implements INotifyPropertyChanged

#Region "Declarations"


    Public Event PropertyChanged(ByVal sender As Object, _
                                 ByVal e As PropertyChangedEventArgs) _
                                 Implements INotifyPropertyChanged.PropertyChanged

    Private WithEvents iGV As New DataGridView   ' Datagridview that acts as the dropdown surface

    Public Event onTxtCode_Entered()
    Public Event onTxtCode_Validated()
    Public Event onTxtCode_GotFocus()
    Public Event onDropDown(ByVal Sender As Object, ByRef aOK As Boolean) ' added boolean parameter - GBV 1/5/2010. GBV 6/10/2014 Added Sender
    Protected Friend Event onDRDropDown(ByRef aDD As qDD, ByRef aOK As Boolean)   'BHS 2/16/10
    Public Event onDropDownClosed()
    Protected Friend Event onDRDropDownClosed(ByRef aDD As qDD)   'BHS 2/16/10
    Public Event CompositeValidated(ByVal Sender As Object, ByVal e As System.EventArgs)
    Public Event CompositeValidating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
    Public Event SelectedIndexChanged(ByVal sender As Object, ByRef e As DataGridViewRowEventArgs)
    Protected Friend Event DRSelectedIndexChanged(ByRef aDD As qDD, ByRef aOK As Boolean)
    Public Event DD_DoubleClick As System.EventHandler
    Protected Friend Event onDRDoubleClick(ByRef aDD As qDD)    'BHS 8/5/10
    Public iDropDownRowCount As Integer = 8
    Private WithEvents iFBase As Form = Nothing ' GBV 1/2/2010
    Private WithEvents iParent As Control
    Private iHeaderHeightIncluded As Boolean = False
    Private iOrigLocation As Point ' GBV 1/2/2010
    Private iSelectedIndex As Integer = -1
    Private iAutoAdjustColumns As Boolean = True
    Private iFindStr As String = ""
    Private WithEvents iTimer As New Timer
    Private iTable As DataTable = Nothing ' GBV 2/1/2010
    Private iRow As DataRow = Nothing ' GBV 2/1/2010
    Private WithEvents iPopUp As PopUpWindow ' GBV 2/3/2010
    Public iAutoGenerateColumns As Boolean = True   'BHS 4/12/10

    'BHS 6/11/10  iSetWriterLevel = 0 when coming from SetFormAttributes or when programmer doesn't care.  
    '   For DRs, 1 if set in DrawItem and 2 if explicitly set elsewhere in the code.
    Public iSetWriterLevel As Integer = 0
    Public iKeyJustPressed As Boolean = False

    'Shared variables with timers
    'BHS 3/31/10 Private to Shared seems to solve rapid dropdown clicks causing dropdown to not close.
    Shared _PopupJustClose As Boolean = False
    Private WithEvents PopupClosedTimer As New Timer

    Shared _MouseDownJustPressed As Boolean = False
    Private WithEvents MouseDownTimer As New Timer

    Shared _TextJustEntered As Boolean = False
    Private WithEvents TextEnteredTimer As New Timer
    Private iTextJustEntered As String = ""

    Shared _CompositeValidatingJustRaised As Boolean = False
    Private WithEvents CompositeValidatingTimer As New Timer

    Shared _TestCounter As Integer = 0

#End Region

#Region " ========================================== Properties ======================================"

    'TextInfo - the current text value - set at runtime
    'Value held in txtCode.Text, instead of a private variable
    Public Overrides Property Text() As String
        Get
            Return TextInfo
        End Get
        Set(ByVal value As String)
            TextInfo = value
        End Set
    End Property

    'TextInfo
    Public Property TextInfo() As String
        Get
            Return txtCode.Text
        End Get
        Set(ByVal value As String)
            If StringCompare(txtCode.Text, value, _DataType, False) = False Then
                Dim OldValue As String = txtCode.Text
                txtCode.Text = value

                NotifyPropertyChanged("TextInfo")

                ' Update datasource object - GBV 12/31/2009
                If Me.DataBindings.Count > 0 AndAlso _
                   _MustMatchListVar = False Then
                    Me.DataBindings(0).WriteValue()
                End If
            End If  'End value different from txtCode.Text
        End Set
    End Property

    'NotifyPropertyChanged
    Private Sub NotifyPropertyChanged(ByVal aPropertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(aPropertyName))
    End Sub


    'SelectedValue - the current selected value - set at runtime
    Private iSelectedValue As Object
    Public Property SelectedValue() As Object
        Get
            Return iSelectedValue
        End Get
        Set(ByVal value As Object)  'Position the grid pointer and assign if SelectedValue is a match in the iGV
            'AddWatch("Set DD Selected Value", "", "DD")
            If value Is Nothing OrElse _
               IsDBNull(value) Then
                iSelectedValue = Nothing
                'If _MustMatchList = True Then
                txtCode.Text = "" ' GBV 8/8/2014 - Both modes should have the text cleared
                'End If
                ' GBV 6/12/2014 - Update the datasource
                If Me.DataBindings.Count > 0 AndAlso _
                   _MustMatchListVar = True Then
                    Me.DataBindings(0).DataSourceNullValue = Convert.DBNull
                    'Me.DataBindings(0).WriteValue()
                End If
                iSelectedIndex = -1
                iGV.ClearSelection()
                iGV.CurrentCell = Nothing
                For Each R As DataGridViewRow In iGV.Rows
                    R.Selected = False
                Next
                Return
            End If

            If iGV.Rows.Count > 0 Then
                For Each R As DataGridViewRow In iGV.Rows
                    If R.Cells(_SelectedValueColumn).Value.ToString = value.ToString Then
                        For i = 0 To iGV.Columns.Count - 1
                            If iGV.Columns(i).Visible Then
                                iGV.CurrentCell = R.Cells(i)
                                Exit For
                            End If
                        Next
                        R.Selected = True ' GBV 12/30/2009 turns on highlight

                        Me.TextInfo = FormatString(R.Cells(_TextColumn).Value.ToString)   'Show text value in the textbox


                        ' GBV 12/31/2009
                        If iSelectedValue Is Nothing OrElse _
                           StringCompare(iSelectedValue.ToString, value.ToString, _DataType, False) = False Then
                            iSelectedValue = value

                            'BHS 10/8/12
                            NotifyPropertyChanged("TextInfo")

                            ' Try to get table row - GBV 2/1/2010
                            Dim SQLStr As String = ""
                            For i As Integer = 0 To iGV.Columns.Count - 1
                                If i < iGV.Columns.Count - 1 Then
                                    SQLStr &= iGV.Columns(i).DataPropertyName & " = '" & R.Cells(i).Value.ToString & "' AND "
                                Else
                                    SQLStr &= iGV.Columns(i).DataPropertyName & " = '" & R.Cells(i).Value.ToString & "'"
                                End If
                            Next
                            Try
                                If iTable.Select(SQLStr).Count > 0 Then
                                    iRow = iTable.Select(SQLStr)(0)
                                End If
                            Catch ex As Exception
                            End Try

                            ' Update datasource object - GBV 12/31/2009
                            If Me.DataBindings.Count > 0 AndAlso _
                               _MustMatchListVar = True Then
                                Me.DataBindings(0).WriteValue()
                            End If


                        End If
                        ' if the index changed ...
                        If iGV.Rows.IndexOf(R) <> iSelectedIndex Then
                            iSelectedIndex = iGV.Rows.IndexOf(R)
                            Dim e As New DataGridViewRowEventArgs(R)
                            RaiseEvent SelectedIndexChanged(Me, e)
                            RaiseEvent DRSelectedIndexChanged(Me, False)
                        End If
                        Return
                    End If
                Next
            Else ' GBV 2/1/2010
                If iTable Is Nothing Then Return
                If iTable.Select(_SelectedValueColumnVar & " = '" & Clean(value.ToString) & "'").Count > 0 Then
                    iRow = iTable.Select(_SelectedValueColumnVar & " = '" & Clean(value.ToString) & "'")(0)
                    Me.TextInfo = FormatString(iRow(_TextColumnVar).ToString)
                    If iSelectedValue Is Nothing OrElse _
                       StringCompare(iSelectedValue.ToString, value.ToString, _DataType, False) = False Then
                        iSelectedValue = value

                        'BHS 10/8/12
                        NotifyPropertyChanged("TextInfo")

                        ' Update datasource object - GBV 12/31/2009 
                        If Me.DataBindings.Count > 0 AndAlso _
                           _MustMatchListVar = True Then
                            Me.DataBindings(0).WriteValue()
                        End If
                    End If
                    Return
                Else
                    iRow = Nothing
                End If
            End If
            iSelectedValue = Nothing    'Otherwise, don't set iSelectedValue

            'BHS 10/8/12
            NotifyPropertyChanged("TextInfo")

            If _MustMatchList = True Then
                txtCode.Text = ""           ' Clear the text field - GBV 1/2/2010
            End If

        End Set
    End Property

    'BHS 8/27/10
    ''' <summary> Allow the programmer to set MaxLength </summary>
    Public Property MaxLength() As Integer
        Get
            Return txtCode.MaxLength
        End Get
        Set(ByVal value As Integer)
            txtCode.MaxLength = value
        End Set
    End Property

    'BHS 4/20/10
    ''' <summary> Return formmated date or number </summary>
    Function FormatString(ByVal aStr As String) As String
        If Me._DataType = DataTypeEnum.Dat Then
            If aStr Is Nothing Or aStr Is DBNull.Value Or aStr = "" Then
                Return ""
            Else
                Dim d As Date = CDate(aStr)
                If _Format.Length > 0 Then
                    Return d.ToString(_Format)
                Else
                    Return d.ToString("MM/dd/yyyy")
                End If
            End If
        End If
        If Me._DataType = DataTypeEnum.Num Then
            'BHS 9/15/10
            'If aStr Is Nothing Or aStr Is DBNull.Value Then
            '    Return ""
            If aStr Is Nothing OrElse _
               aStr Is DBNull.Value OrElse _
               aStr = "" Then
                Return ""
            Else
                Dim n As Double = CType(aStr, Double)
                If _Format.Length > 0 Then
                    Return n.ToString(_Format)
                End If
            End If
        End If
        Return aStr
    End Function

    'Assign Datasource to iGV.DataSource - set at runtime
    Private iDataSource As Object = Nothing

    Public Overridable Property DataSource() As Object
        Get
            Return iDataSource
        End Get
        Set(ByVal value As Object)
            AddWatch("Set DD DataSource", "", "DD")
            'SetupGV()

            'iGV.AutoGenerateColumns = False 'BHS 2/18/2010
            If value Is Nothing Then  ' GBV 1/5/2010
                If iTable IsNot Nothing Then iTable.Rows.Clear()
                'iGV.Visible = False
                iTable = Nothing
                iRow = Nothing
                iDataSource = Nothing
                iGV.DataSource = Nothing
            Else
                If Not TypeOf (value) Is DataView Then ' GBV 2/1/2010
                    ' Programmer error
                    MsgBox("Programmer error: Only Dataviews are allowed as Datasource in a qDD")
                    Return
                End If

                iDataSource = value
                iTable = CType(value, DataView).Table ' GBV 2/1/2010

                'iGV.Columns.Clear() 'BHS 4/20/10
                iGV.AutoGenerateColumns = iAutoGenerateColumns
                iGV.DataSource = iTable
                iGV.RowTemplate.Height = 15 'BHS 7/26/10


                Dim InvCols As New ArrayList    'Invisible columns  GABRIEL - WHY IS THIS NECESSARY?
                For i As Integer = 0 To iGV.Columns.Count - 1
                    If Not iGV.Columns(i).Visible Then InvCols.Add(i)
                Next

                If iFindByNoSort = False Then
                    If iFindBySortAscending = True Then
                        CType(iDataSource, DataView).Sort = iFindByColumn
                    Else
                        CType(iDataSource, DataView).Sort = iFindByColumn & " Desc"
                    End If
                End If

                For i = 0 To InvCols.Count - 1
                    iGV.Columns(CInt(InvCols.Item(i))).Visible = False
                Next

                'If MustMatchList = True, try to select the row that txtCode.Text points to
                If _MustMatchList = True And txtCode.Text.Length > 0 Then
                    Dim i As Integer = GetGVRowIndexLike(txtCode.Text, True)
                    If i > -1 Then SelectedIndex = i
                End If

            End If
        End Set
    End Property

    'BHS 3/5/10
    Public Property DisplayMember() As String
        Get
            Return _TextColumnVar
        End Get
        Set(ByVal value As String)
            _TextColumnVar = value
        End Set
    End Property

    Public Property ValueMember() As String
        Get
            Return _SelectedValueColumnVar
        End Get
        Set(ByVal value As String)
            _SelectedValueColumnVar = value
        End Set
    End Property

    '_BindDef names a Field within a Binding Source, to match this control to an element in the Binding Source's datasource.
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

    '_Format is similar to qTextBox implementation - allows using a format strng for a date or a number
    Private _formatVar As String = ""
    <System.ComponentModel.Category("Format"), _
     System.ComponentModel.Description("Enter a Format String for a date or number.  Must be used in conjunction with the DataType property"), _
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
     System.ComponentModel.DefaultValue(False)> _
    Public Overridable Property _Format() As String
        Get
            Return _formatVar
        End Get
        Set(ByVal value As String)
            _formatVar = value
        End Set
    End Property


    '_TextColumn names a column that will be bound to Me.TextInfo
    'Can be set at design time or runtime
    Private _TextColumnVar As String = String.Empty
    <System.ComponentModel.Category("Data"), _
            System.ComponentModel.Description("The Select column that will be bound to Me.TextInfo"), _
            System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
            System.ComponentModel.DefaultValue("")> _
    Public Overridable Property _TextColumn() As String
        Get
            Return _TextColumnVar
        End Get
        Set(ByVal value As String)
            _TextColumnVar = value
            If iFindByColumn = "" Then
                iFindByColumn = _TextColumnVar
            End If
        End Set
    End Property

    '_TextColumn names a column that will be bound to Me.TextInfo
    'Can be set at design time or runtime
    Private _SelectedValueColumnVar As String = String.Empty
    <System.ComponentModel.Category("Data"), _
            System.ComponentModel.Description("The Select column that will be bound to Me.SelectedValue"), _
            System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
            System.ComponentModel.DefaultValue("")> _
    Public Overridable Property _SelectedValueColumn() As String
        Get
            Return _SelectedValueColumnVar
        End Get
        Set(ByVal value As String)
            _SelectedValueColumnVar = value
        End Set
    End Property

    '_DataType
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

    '_MustMatchList
    Private _MustMatchListVar As Boolean = False
    <System.ComponentModel.Category("Data"), _
     System.ComponentModel.Description("True means textbox data must match list before saving"), _
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
     System.ComponentModel.DefaultValue(False)> _
    Public Overridable Property _MustMatchList() As Boolean
        Get
            Return _MustMatchListVar
        End Get
        Set(ByVal value As Boolean)
            _MustMatchListVar = value
        End Set
    End Property

    '_MustMatchTimer
    Private _MustMatchTimeVar As Integer = 800
    <System.ComponentModel.Category("Data"), _
     System.ComponentModel.Description("Number of milleseconds before must match text entry starts a new list search.  Default is .8 seconds"), _
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
     System.ComponentModel.DefaultValue(False)> _
    Public Overridable Property _MustMatchTime() As Integer
        Get
            Return _MustMatchTimeVar
        End Get
        Set(ByVal value As Integer)
            _MustMatchTimeVar = value
        End Set
    End Property

    'Tooltip property holds online help for this textbox
    Private _toolTipVar As String = String.Empty
    <System.ComponentModel.Category("Data"), _
            System.ComponentModel.Description("Tooltip help for this control."), _
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

    'Query Field holds Table.Field.Type for building query SQL
    Private _QueryDefinition As String = String.Empty
    <System.ComponentModel.Category("Data"), _
            System.ComponentModel.Description("TableName.FieldName[.Type] where type is optional, and may be Str, Num or Dat.  Used to build query SQL."), _
            System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
            System.ComponentModel.DefaultValue("")> _
    Public Overridable Property _QueryDef() As String
        Get
            Return _QueryDefinition
        End Get
        Set(ByVal value As String)
            _QueryDefinition = value
        End Set
    End Property

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

    Private _textCaseVar As System.Windows.Forms.CharacterCasing = CharacterCasing.Normal
    ' Applies automatic character casing to the Textinfo property.
    <System.ComponentModel.Category("Appearance"), _
     System.ComponentModel.Description("Applies automatic character casing to the TextInfo property."), _
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
     System.ComponentModel.DefaultValue(GetType(System.Windows.Forms.CharacterCasing), "Normal")> _
    Public Overridable Property _TextCase() As System.Windows.Forms.CharacterCasing
        Get
            Return _textCaseVar
        End Get
        Set(ByVal value As System.Windows.Forms.CharacterCasing)
            _textCaseVar = value
            txtCode.CharacterCasing = value
        End Set
    End Property

    Private _validateDateVar As Boolean = False
    ' Enforces the validation rule that the TextInfo value must be a valid date.
    <System.ComponentModel.Category("Validation"), _
     System.ComponentModel.Description("Enforces the validation rule that the TextInfo value must be a valid date."), _
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
     System.ComponentModel.DefaultValue(False)> _
    Public Overridable Property _ValidateDate() As Boolean
        Get
            Return _validateDateVar
        End Get
        Set(ByVal value As Boolean)
            _validateDateVar = value
            If _validateDateVar = True Then
                ' Turn Off Number Validation
                _ValidateNumber = False
            End If
        End Set
    End Property

    Private _validateNumberVar As Boolean = False
    ' Enforces the validation rule that the TextInfo value must be a valid number.
    <System.ComponentModel.Category("Validation"), _
     System.ComponentModel.Description("Enforces the validation rule that the TextInfo value must be a valid number."), _
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
     System.ComponentModel.DefaultValue(False)> _
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

    Private _validateNotEmpty As Boolean = False
    ' Enforces the validation rule that the TextInfo value must not be empty.

    <System.ComponentModel.Category("Validation"), _
     System.ComponentModel.Description("Enforces the validation rule that the TextInfo value must not be empty."), _
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
     System.ComponentModel.DefaultValue(False)> _
    Public Overridable Property _ValidateRequired() As Boolean
        Get
            Return _validateNotEmpty
        End Get
        Set(ByVal value As Boolean)
            _validateNotEmpty = value
            If _validateNotEmpty = True AndAlso _ReadOnly = False AndAlso _
               gUseRequiredBackColor = True Then
                txtCode.BackColor = QRequiredBackColor
            End If
        End Set
    End Property

    Private _ReadOnlyVar As Boolean = False
    ' Dynamically set in the program to change a combo box's appearance and behavior (_ReadAlways sets it permanently)
    <System.ComponentModel.Category("Behavior"), _
     System.ComponentModel.Description("Use _ReadAlways to set control permanently.  _ReadOnly gets set dynamically in the program."), _
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
     System.ComponentModel.DefaultValue(False)> _
    Public Overridable Property _ReadOnly() As Boolean
        Get
            Return _ReadOnlyVar
        End Get
        Set(ByVal value As Boolean)
            _ReadOnlyVar = value
            txtCode.ReadOnly = value
            If value = True Then
                btnDD.Enabled = False
            Else
                btnDD.Enabled = True
            End If
        End Set
    End Property

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
    ''' <summary> (Read Only) Returns the number of columns in the dropdown </summary>
    Public ReadOnly Property ColumnCount() As Integer  ' GBV 1/3/2010
        Get
            Return iGV.Columns.Count
        End Get
    End Property

    ''' <summary> (Read Only) Returns the total number of rows in the dropdown </summary>
    Public ReadOnly Property RowCount() As Integer ' GBV 1/3/2010
        Get
            Return iGV.Rows.Count
        End Get
    End Property

    'SelectedIndex
    Public Property SelectedIndex() As Integer
        Get
            Return iSelectedIndex
        End Get
        Set(ByVal value As Integer)
            If value <= -1 Then
                iSelectedIndex = -1
                SelectedValue = ""
                TextInfo = ""
                'If iGV.Visible Then iGV.Visible = False
                If value <> iSelectedIndex AndAlso value >= -1 Then
                    iSelectedIndex = value
                    RaiseEvent SelectedIndexChanged(Me, Nothing)
                    RaiseEvent DRSelectedIndexChanged(Me, Nothing)
                End If
            Else
                If value > iGV.Rows.Count - 1 Then Return
                If value <> iSelectedIndex Then
                    Dim R As DataGridViewRow = iGV.Rows(value)
                    For i As Integer = 0 To iGV.Columns.Count - 1
                        If iGV.Columns(i).Visible Then
                            iGV.CurrentCell = R.Cells(i)
                            Exit For
                        End If
                    Next
                    R.Selected = True ' GBV 12/30/2009 turns on highlight
                    Me.TextInfo = R.Cells(_TextColumn).Value.ToString   'Show text value in the textbox
                    ' GBV 12/31/2009
                    If iSelectedValue Is Nothing OrElse _
                       StringCompare(iSelectedValue.ToString, _
                                     R.Cells(_SelectedValueColumn).Value.ToString, _
                                     _DataType, False) = False Then
                        iSelectedValue = R.Cells(_SelectedValueColumn).Value.ToString

                        'BHS 10/8/12
                        NotifyPropertyChanged("TextInfo")

                        ' Try to get table row - GBV 2/1/2010
                        Dim SQLStr As String = ""
                        For i As Integer = 0 To iGV.Columns.Count - 1
                            If i < iGV.Columns.Count - 1 Then
                                SQLStr &= iGV.Columns(i).DataPropertyName & " = '" & R.Cells(i).Value.ToString & "' AND "
                            Else
                                SQLStr &= iGV.Columns(i).DataPropertyName & " = '" & R.Cells(i).Value.ToString & "'"
                            End If
                        Next

                        Try
                            If iTable.Select(SQLStr).Count > 0 Then
                                iRow = iTable.Select(SQLStr)(0)
                            End If
                        Catch ex As Exception
                        End Try

                        ' Update datasource object - GBV 12/31/2009
                        If Me.DataBindings.Count > 0 AndAlso _
                           _MustMatchListVar = True Then
                            Me.DataBindings(0).WriteValue()
                        End If
                        iSelectedIndex = value
                    End If
                    Dim e As New DataGridViewRowEventArgs(R)
                    RaiseEvent SelectedIndexChanged(Me, e)
                    RaiseEvent DRSelectedIndexChanged(Me, False)
                End If
            End If

        End Set
    End Property

    'AutoAdjustColumns
    <System.ComponentModel.Category("Data"), _
     System.ComponentModel.Description("True means column width will be dynamically adjusted"), _
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
     System.ComponentModel.DefaultValue(True)> _
    Public Property _AutoAdjustColumns() As Boolean
        Get
            Return iAutoAdjustColumns
        End Get
        Set(ByVal value As Boolean)
            iAutoAdjustColumns = value
        End Set
    End Property

    'FindByColumn
    Private iFindByColumn As String = ""
    <System.ComponentModel.Category("Data"), _
     System.ComponentModel.Description("DropDown column used for incremental searches. Defaults to _TextColumn"), _
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
     System.ComponentModel.DefaultValue("")> _
    Public Property _FindByColumn() As String
        Get
            Return iFindByColumn
        End Get
        Set(ByVal value As String)
            iFindByColumn = value
        End Set
    End Property

    'FindBySortAscending
    Private iFindBySortAscending As Boolean = True
    <System.ComponentModel.Category("Data"), _
     System.ComponentModel.Description("Set to False to support FindByColumn that is sorted Descending"), _
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
     System.ComponentModel.DefaultValue("")> _
    Public Property _FindBySortAscending() As Boolean
        Get
            Return iFindBySortAscending
        End Get
        Set(ByVal value As Boolean)
            iFindBySortAscending = value
        End Set
    End Property

    'FindByNoSort
    Private iFindByNoSort As Boolean = False
    <System.ComponentModel.Category("Data"), _
     System.ComponentModel.Description("Set to True to prevent sorting on FindByColumn "), _
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
     System.ComponentModel.DefaultValue("")> _
    Public Property _FindByNoSort() As Boolean
        Get
            Return iFindByNoSort
        End Get
        Set(ByVal value As Boolean)
            iFindByNoSort = value
        End Set
    End Property
#End Region

#Region " ========================================== Load ======================================"


    Private Sub qDD_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Leave
        If _ReadOnly = True Then
            Me.TabStop = False   'Prevents user tabbing to control
            Me.txtCode.BackColor = QReadOnlyBackColor
        End If
        PositionLeft()  'BHS 11/15/10
    End Sub

    Private Sub qDD_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Height = 21
        txtCode.Height = 21
        CompositeValidatingTimer.Interval = 50

        'BHS 7/30/10 consider setting Me.CausesValidation = False if we stop using the Me.Validating event
        iParent = Me.Parent
        iFBase = Me.ParentForm

        SetupGV()

    End Sub

    Private Sub SetupGV()

        iGV.GridColor = Color.White
        iGV.MultiSelect = False ' GBV 12/30/2009
        iGV.AllowUserToOrderColumns = False
        iGV.BackgroundColor = Color.White
        iGV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        iGV.CellBorderStyle = DataGridViewCellBorderStyle.None
        iGV.ReadOnly = True
        iGV.ScrollBars = ScrollBars.Vertical
        iGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        iGV.RowHeadersVisible = False
        iGV.AllowUserToAddRows = False
        iGV.Margin = New System.Windows.Forms.Padding(0)
        iGV.AutoGenerateColumns = iAutoGenerateColumns
        'BHS 3/9/10
        iGV.CausesValidation = False
        'SRM 8/23/10
        iGV.AllowUserToResizeColumns = False
        iGV.AllowUserToResizeRows = False

        If iFBase IsNot Nothing Then
            iFBase.Controls.Add(iGV) ' GBV 1/2/2010 - make it a child of the form, 
        End If                       ' so that the gv will populate

        iGV.Anchor = AnchorStyles.Left Or AnchorStyles.Top

        iGV.ColumnHeadersVisible = False
        iGV.Visible = False

        RemoveHandler iGV.CellClick, AddressOf GVCellClick
        AddHandler iGV.CellClick, AddressOf GVCellClick

    End Sub

    Public Sub AddColumn(ByVal aColName As String, Optional ByVal aColWidth As Integer = 0, Optional ByVal aColHeader As String = "", Optional ByVal aColVisible As Boolean = True, Optional ByVal aColFormat As String = "")

        If aColHeader.Length > 0 Then iGV.ColumnHeadersVisible = True


        iGV.Columns.Add(aColName, aColHeader)
        'BHS 4/20/10 Only specify width if user specified width
        If aColWidth > 0 Then
            iGV.Columns(iGV.Columns.Count - 1).Width = aColWidth
            iAutoAdjustColumns = False
        End If

        iGV.Columns(iGV.Columns.Count - 1).DataPropertyName = aColName
        iGV.Columns(iGV.Columns.Count - 1).Visible = aColVisible
        If aColFormat.Length > 0 Then iGV.Columns(iGV.Columns.Count - 1).DefaultCellStyle.Format = aColFormat 'BHS 4/20/10

        'If user explicitly adds a column, then AutoGenerateColumns is False
        iAutoGenerateColumns = False

    End Sub

    ''' <summary> Clear all columns in the dropdown </summary>
    Public Sub ClearColumns()  ' GBV 1/3/2010
        iGV.Columns.Clear()
        If iTable IsNot Nothing Then iTable.Columns.Clear() ' GBV 2/1/2010
    End Sub

#End Region

#Region " ========================================== Control Functions ======================================"


    ''' <summary> Get first datarow from aTable that is LIKE aValue.  May return Nothing </summary>
    Public Function GetGVRowIndexLike(ByVal aValue As String, ByVal aExactMatch As Boolean) As Integer
        'The (0) at the end returns the first row of the array of rows found
        Dim Rows As DataRow() = Nothing
        Dim R As DataRow = Nothing
        Dim wstr As String = ""

        If iTable Is Nothing Then Return -1

        'BHS 4/20/10 Handle GetGVRow for dates and numbers
        Dim DataTypeName As String = iTable.Columns(iFindByColumn).DataType.Name
        If Mid(DataTypeName, 1, 4) = "Date" Then
            If aValue.Length > 5 AndAlso IsDate(aValue) AndAlso _
                           CType((aValue), DateTime) > CType("1/1/1980", DateTime) Then
                Dim D As Date = CType((aValue), Date)
                Dim DateStr As String = CType(D, String)
                If D.Day > 0 Then
                    Rows = iTable.Select(iFindByColumn & " >= #" & DateStr & " 12:00am# And " & _
                               iFindByColumn & " <= #" & DateStr & " 11:59pm#")
                End If
            End If
        ElseIf DataTypeName = "Decimal" Or Mid(DataTypeName, 1, 3) = "Int" Or DataTypeName = "Double" Then
            If aValue.Length > 0 AndAlso IsNumeric(aValue) Then
                Dim D As Decimal
                Try
                    D = Convert.ToDecimal(aValue)
                Catch ex As Exception
                    Return -1
                End Try
                Rows = iTable.Select(iFindByColumn & " = " & D.ToString)
            End If
        Else    'String
            '03/22/2012 SRM Fixed to check for "%" correctly
            '10/25/2012 DJW strip out brackets
            wstr = RemoveStr(aValue, "[")
            If StringCompare(aValue, wstr) = False Then aValue = wstr
            wstr = RemoveStr(aValue, "]")
            If StringCompare(aValue, wstr) = False Then aValue = wstr
            wstr = RemoveStr(aValue, "*")
            If StringCompare(aValue, wstr) = False Then aValue = wstr

            If aExactMatch = True Then
                Rows = iTable.Select(iFindByColumn & " ='" & Replace(Clean(aValue), "%", "[%]") & "'")
            Else
                If _FindBySortAscending = True Or iFindByNoSort = True Then
                    Rows = iTable.Select(iFindByColumn & " LIKE '" & Replace(Clean(aValue), "%", "[%]") & "%'")
                Else
                    'If descending, then reverse the Select order so we highlight the top row
                    Rows = iTable.Select(iFindByColumn & " LIKE '" & Replace(Clean(aValue), "%", "[%]") & "%'", iFindByColumn & " Desc")
                End If

            End If
        End If

        'BHS 5/13/10
        If Rows IsNot Nothing AndAlso Rows.Count > 0 Then
            R = Rows(0)
        Else
            Return -1
        End If

        If R Is Nothing Then Return -1

        'BHS 4/27/10
        Dim i As Integer = -1
        If iFindByNoSort = False Then
            If iTable.DefaultView.Sort = "" Then
                If iFindBySortAscending = True Then
                    iTable.DefaultView.Sort = iFindByColumn
                Else
                    iTable.DefaultView.Sort = iFindByColumn & " Desc"
                End If

            End If

            i = iTable.DefaultView.Find(R.Item(iFindByColumn).ToString)
        Else
            For i = 0 To iGV.Rows.Count - 1
                Dim CellIndex As Integer = iGV.Columns.Item(iFindByColumn).Index '020911 - SRM - Added to make ShowValueFirst work in FillDD
                If Mid(iGV.Rows.Item(i).Cells(CellIndex).Value.ToString, 1, aValue.Length) = Mid(aValue, 1, aValue.Length) Then
                    Exit For
                End If
                If i = iGV.Rows.Count - 1 Then
                    i = -1
                    Exit For
                End If
            Next
        End If

        If i >= 0 And iGV.Rows.Count > i Then
            Return i
        Else
            Return -1   'Didn't find the row
        End If
    End Function

    Sub SetSelectedValueBasedOnText(ByVal aValue As String, ByVal aExactMatch As Boolean)
        If iDataSource IsNot Nothing Then
            Dim i As Integer = GetGVRowIndexLike(aValue, aExactMatch)
            If i > -1 Then
                SelectedValue = iGV.Rows(i).Cells(_SelectedValueColumnVar).Value.ToString
            End If
        End If
    End Sub


    ''' <summary> iGV.Height is set every time we prepare to open the iPopUp </summary>
    Private Sub SetiGVHeightAndWidth()

        '=== Height ===
        Dim Rows As Integer = iDropDownRowCount
        Dim Height As Integer = 0
        Dim Width As Integer = 0

        'Reduce Rows if we have fewer than the default rows to show
        If iTable IsNot Nothing Then
            If iTable.Rows.Count < Rows Then Rows = iTable.Rows.Count
        End If

        Height = iGV.RowTemplate.Height * (Rows) + 5 'BHS 7/26/10
        'THIS LEAVES NO HORIZONTAL LINE AT THE BOTTOM OF THE POPUP

        'Add height for column headers, if visible
        If iGV.ColumnHeadersVisible = True Then
            Height += iGV.ColumnHeadersHeight
        End If

        '=== Width ===
        'iGV.Width = 0
        For i = 0 To iGV.Columns.Count - 1
            If iGV.Columns(i).Visible = True Then
                If iAutoAdjustColumns = True Then iGV.AutoResizeColumn(i)
                iGV.Columns(i).Width += 5
                Width += iGV.Columns(i).Width
            End If
        Next

        ' Increase width if we need a scrollbar
        If iGV.Rows.Count <= iDropDownRowCount Then
            Width += 6  'BHS 7/26/10 2 to 6
        Else
            Width += 25 'BHS 7/26/10 20 to 25
        End If

        'BHS 3/17/10  Make the dropdown width at least as wide as the text part of the dropdown.
        If Width < txtCode.Width Then
            If iGV.Columns.Count > 0 Then
                'Increase the width of the last visible column
                For i As Integer = iGV.Columns.Count - 1 To 0 Step -1
                    If iGV.Columns(i).Visible = True Then
                        iGV.Columns(i).Width += txtCode.Width - Width
                    End If
                Next
            End If
            Width = txtCode.Width
        End If

        iGV.Height = Height
        iGV.Width = Width

    End Sub

    'Set attributes to make control disabled and data invisible
    Public Sub SetTopSecret() 'GBV 7/14/2008
        Me.Enabled = False
        Me.DataBindings.Clear()
        Me.BackColor = Color.Black
        Me.ForeColor = Color.Black
        Me.TextInfo = ""
    End Sub

    'Set control attributes based on whether user is a writer or not
    Public Sub SetWriter(ByVal aWriter As Boolean, Optional ByVal aSetWriterLevel As Integer = 0)
        'Don't act if a higher SetWriterLevel has already been set for this control
        AddWatch("SetWriter ", Me.Name & "  " & aWriter)

        If aSetWriterLevel < iSetWriterLevel Then Return
        iSetWriterLevel = aSetWriterLevel

        If aWriter = True Then
            Me._ReadOnly = False
            Me.TabStop = True
            If gUseRequiredBackColor = True And Me._ValidateRequired = True Then
                txtCode.BackColor = QRequiredBackColor
            Else
                txtCode.BackColor = QEntryBackColor
            End If
        Else
            Me._ReadOnly = True   'Prevents typing
            Me.TabStop = False   'Prevents user tabbing to control
            Me.txtCode.BackColor = QReadOnlyBackColor
            Me.txtCode.SelectionLength = 0  'BHS 5/5/08 Don't leave RO field selected
            Me.txtCode.SelectionStart = 0
        End If
    End Sub

    'BHS 5/11/10
    Public Sub Sort(ByVal aSortString As String)
        iTable.DefaultView.Sort = aSortString
    End Sub

    'SRM 11/11/10
    Public Function GetMaxLength() As Integer
        Return txtCode.MaxLength
    End Function

    'SRM 11/15/10
    Public Sub PositionLeft()
        txtCode.SelectionLength = 0
        txtCode.SelectionStart = 0
    End Sub

    'BHS 7/27/12
    ''' <summary> Format new entries based on standard _Format, where appropriate </summary>
    Private Sub txtCode_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtCode.Validating

        If _DataType = DataTypeEnum.Dat AndAlso _Format.Length > 0 AndAlso IsDate(txtCode.Text) = True Then
            txtCode.Text = Format(CDate(txtCode.Text), _Format)
        ElseIf _DataType = DataTypeEnum.Num AndAlso _Format.Length > 0 AndAlso IsNumeric(txtCode.Text) = True Then
            txtCode.Text = Format(CDec(txtCode.Text), _Format)
        End If
    End Sub

#End Region

    'GBV 8/11/2014
    ''' <summary>
    ''' Sets the cursor at the end of text
    ''' </summary>
    ''' <remarks>This is to be used only in copy/paste operations.</remarks>
    Public Sub SetCursorPosition()
        txtCode.Select(txtCode.Text.Length, 0)
    End Sub

#Region " ========================================== Control Events ======================================"

    'txtCode GotFocus
    Private Sub txtCode_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCode.GotFocus
        RaiseEvent onTxtCode_GotFocus()
        If _MustMatchListVar Then
            txtCode.BackColor = Color.DarkBlue
            txtCode.ForeColor = Color.White
            txtCode.SelectAll()
        End If
    End Sub

    'TxtCode.KeyDown
    'If user types down arrow or right arrow, move selected item down one.  
    'If up arrow or left arrow, move selected item up one.
    Private Sub txtCode_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtCode.KeyDown

        ' GBV 1/15/2015 - ignore Alt or Ctrl keys
        If e.Alt Then Return
        If e.Control Then Return

        'BHS 10/25/10 SelectAll
        If e.KeyData = gSelectAllCode Then
            txtCode.SelectAll()
            Return
        End If

        If _ReadOnlyVar = True Then Return
        If e.KeyCode = Keys.Down OrElse e.KeyCode = Keys.Right Then
            'BHS 10/20/10 open dropdown if Control DownArrow
            If e.Modifiers = Keys.Control Then
                btnDD.PerformClick()
            Else
                SelectedIndex += 1
                e.SuppressKeyPress = True
                txtCode.Focus()
                Return
            End If
        End If
        If e.KeyCode = Keys.Up OrElse e.KeyCode = Keys.Left Then
            'BHS 6/12/13 Prevent reducing SelectedIndex < 0
            If SelectedIndex > 0 Then SelectedIndex -= 1
            e.SuppressKeyPress = True
            txtCode.Focus()
            Return
        End If
        If _MustMatchListVar = True Then

            If _TextJustEntered = True Then
                iFindStr &= TranslateKeyCode(e)
                AddWatch("Append FindStr = " & iFindStr)
            Else
                iFindStr = TranslateKeyCode(e)
                AddWatch("FindStr = " & iFindStr)
            End If

            _TextJustEntered = True
            iTimer.Interval = _MustMatchTimeVar
            iTimer.Stop()   'Stop any existing timer delay, because we're going to start it over again
            iTimer.Start()

            SetSelectedValueBasedOnText(iFindStr, False)    'Doesn't need to be an exact match

            e.SuppressKeyPress = True
            txtCode.Focus()
        Else
            RaiseEvent onTxtCode_Entered()
        End If
    End Sub

    Private Sub TextEnteredTimer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEnteredTimer.Tick
        TextEnteredTimer.Stop()
        _TextJustEntered = False
    End Sub

    'Detect MouseDown in txtCode
    Private Sub txtCode_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtCode.MouseDown
        _TestCounter += 1
        _MouseDownJustPressed = True
        AddWatch("txtCode.MouseDown", _TestCounter.ToString)
        MouseDownTimer.Start()
    End Sub

    'txtCode Double-Click
    Private Sub txtCode_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCode.DoubleClick
        RaiseEvent DD_DoubleClick(sender, e)
        RaiseEvent onDRDoubleClick(Me)
    End Sub

    'txtCode Lost Focus
    Private Sub txtCode_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCode.LostFocus
        If _MustMatchListVar = True Then
            If Me._ReadOnly = True Then
                Me.TabStop = False   'Prevents user tabbing to control
                Me.txtCode.BackColor = QReadOnlyBackColor
            Else
                Me.TabStop = True
                If gUseRequiredBackColor = True And Me._ValidateRequired = True Then
                    txtCode.BackColor = QRequiredBackColor
                Else
                    txtCode.BackColor = QEntryBackColor
                End If
            End If

            txtCode.ForeColor = Color.Black
        End If
    End Sub

    'txtCode Validated
    Private Sub txtCode_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCode.Validated
        NotifyPropertyChanged("TextInfo")   'BHS 1/27/10
        RaiseEvent onTxtCode_Validated()
    End Sub


    'Me.Validating
    Private Sub qDD_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Me.Validating
        _TestCounter += 1

        'Only RaiseEvent CompositeValidating if we didn't just click btnDD or txtCode in this control.  Since iPopup and gvList
        'are created at runtime, moving to and from them can cause Validating events that we don't want to communicate.

        'BHS 4/9/13 remove this line, parallel to how we do it in Oakland,
        '  to get the descendant's OnValidateControl event to fire even if they just entered a letter.
        'If _MouseDownJustPressed = False And _TextJustEntered = False Then




        'In a DataRepeater, this event gets fired twice.  Ignore the second firing.
        If _CompositeValidatingJustRaised = False Then

            'This is tricky and possibly dangerous - If I don't do the Application.DoEvents loop, _MouseDownJustPressed doesn't show up 
            'as fired.  I believe the loop allows time for the asynchronous MouseDown event to change the _MouseDownJustPressed value.
            'In general you would not want to try to interrupt a Validating event, since it is part of a timed sequence (Validating,
            'Validated, Leave), and I'm hoping that the asynch MouseDown is the only thing that is allowed to get out of order here.

            'BHS 10/22/10
            'For i As Integer = 1 To 10000
            '    Application.DoEvents()
            '    If _MouseDownJustPressed = True Then Return
            '    If _TextJustEntered = True Then Return
            '    If _CompositeValidatingJustRaised = True Then Return
            'Next

            AddWatch("qDD Validating", _MouseDownJustPressed.ToString & " " & _TestCounter.ToString)
            _CompositeValidatingJustRaised = True
            CompositeValidatingTimer.Start()

            RaiseEvent CompositeValidating(Me, e)   'Send composite control, not the textbox component

        End If
        'End If

    End Sub

    'Turn of _CompositeValidatingJustRaised
    Private Sub CompositeValidatingTimer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles CompositeValidatingTimer.Tick
        CompositeValidatingTimer.Stop()
        _CompositeValidatingJustRaised = False
    End Sub

    'Me.Validated
    Private Sub qDD_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Validated
        RaiseEvent CompositeValidated(Me, e)   'Send composite control, not the textbox component
    End Sub

    'Detect MouseDown in btnDD
    Private Sub btnDD_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnDD.MouseDown

        _TestCounter += 1
        _MouseDownJustPressed = True
        AddWatch("btnDD.MouseDown", _TestCounter.ToString)
        MouseDownTimer.Start()
    End Sub


    'BHS 1/29/10 Made btnDD Friend instead of Public to make Click Event work (!)
    Private Sub btnDD_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDD.Click
        Dim i As Integer = -1

        AddWatch("btnDD.Click", _MouseDownJustPressed.ToString)

        'If Popup was just open, clicking btnDD should have no effect other than to close the popup, which happens automatically
        If _PopupJustClose = True Then
            Return
        End If

        'If iPopUp Is Nothing OrElse iPopUp.Visible = False Then
        Dim aOK As Boolean = True  ' GBV 1/5/2010 Added boolean param. to dropdown event
        RaiseEvent onDropDown(Me, aOK) ' GBV 6/9/2014 Added Sender.
        RaiseEvent onDRDropDown(Me, aOK)
        If Not aOK Then Return

        'BHS 3/15/10 Let possible datasource replacement logic run
        For i = 1 To 10000
            Application.DoEvents()
        Next

        Dim P As Point

        P.X = 0
        P.Y = txtCode.Height

        'If MustMatchList = False then find the first row LIKE txtCode.Text
        If _MustMatchListVar = False Then
            If txtCode.Text <> "" Then ' GBV 2/1/2010
                iFindStr = txtCode.Text
                i = GetGVRowIndexLike(iFindStr, False)

                If i >= 0 AndAlso iGV.Rows.Count > i Then
                    iGV.Rows(i).Selected = True
                    For j As Integer = 0 To iGV.Columns.Count - 1
                        If iGV.Columns(j).Visible Then
                            iGV.CurrentCell = iGV.Rows(i).Cells(j)
                            Exit For
                        End If
                    Next
                    iSelectedIndex = i
                End If

                iFindStr = ""
            End If

        ElseIf iSelectedIndex > -1 And iSelectedIndex < iGV.Rows.Count Then
            Dim R As DataGridViewRow = iGV.Rows(iSelectedIndex)
            For j As Integer = 0 To iGV.Columns.Count - 1
                If iGV.Columns(j).Visible Then
                    iGV.CurrentCell = R.Cells(j)
                End If
            Next
            'If MustMatchList = True and iSelectedIndex is not set, look for an exact match and point to it if you find it
        ElseIf iRow IsNot Nothing AndAlso iGV.Rows.Count > 0 Then ' GBV 2/1/2010
            If iFindByNoSort = True Then '2/16/2011 SRM
                For i = 0 To iGV.Rows.Count - 1
                    Dim CellIndex As Integer = iGV.Columns.Item(iFindByColumn).Index
                    Dim Value As String = iRow.Item(iFindByColumn).ToString
                    If Mid(iGV.Rows.Item(i).Cells(CellIndex).Value.ToString, 1, Value.Length) = Mid(Value, 1, Value.Length) Then
                        Exit For
                    End If
                    If i = iGV.Rows.Count - 1 Then
                        i = -1
                        Exit For
                    End If
                Next
            Else
                i = CType(iDataSource, DataView).Find(iRow.Item(iFindByColumn).ToString)
            End If

            If i >= 0 And iGV.Rows.Count > i Then
                iGV.Rows(i).Selected = True
                For j As Integer = 0 To iGV.Columns.Count - 1
                    If iGV.Columns(j).Visible Then
                        iGV.CurrentCell = iGV.Rows(i).Cells(j)
                        Exit For
                    End If
                Next
                iSelectedIndex = i
            End If

        End If

        ' Open dropdown

        iGV.Visible = True ' make it visible now, otherwise it does not adjust

        SetiGVHeightAndWidth()

        AddWatch("iPopup Show")
        iPopUp = New PopUpWindow(iGV)

        'SM 11/11/11 (R5.1.11) address System.ObjectDisposedException: Cannot access a disposed object.
        If iPopUp.IsDisposed = False And Me.IsDisposed = False Then
            iPopUp.Show(Me, P)
            iPopUp.Focus()
        End If

    End Sub

    'MouseDown Timer
    Private Sub MouseDownTimer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles MouseDownTimer.Tick
        MouseDownTimer.Stop()
        _MouseDownJustPressed = False
    End Sub

    'GV Cell Click
    Sub GVCellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
        Try

            If e.RowIndex > -1 Then
                If _TextColumn.Length > 0 Then
                    If _ReadOnly = False And _ReadAlways = False Then
                        ' GBV 12/30/2009
                        Me.SelectedValue = iGV.Rows(e.RowIndex).Cells(_SelectedValueColumnVar).Value.ToString
                    End If
                End If

                iPopUp.Close()
                iSelectedIndex = e.RowIndex
                Dim RowEvent As New DataGridViewRowEventArgs(iGV.Rows(e.RowIndex))
                RaiseEvent SelectedIndexChanged(Me, RowEvent)
                RaiseEvent DRSelectedIndexChanged(Me, False)
                txtCode.Focus()

            End If

        Catch ex As Exception
            ShowError("Unexpected error clicking dropdown line", ex)
        End Try
    End Sub

    'iGV KeyDown
    'If in normal letter range, accumulate string to find on
    Private Sub iGV_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles iGV.KeyDown
        'Ignore Shift Key
        If e.KeyValue = 16 Then Return


        'Down arrow or right arrow move down one line
        'BHS 10/20/10 open dropdown if Control DownArrow
        If e.KeyCode = Keys.Down OrElse e.KeyCode = Keys.Right Then
            If e.Modifiers = Keys.Control Then
                btnDD.PerformClick()
            Else
                SelectedIndex += 1
                e.SuppressKeyPress = True
                iGV.Focus()
                Return
            End If
        End If

        'Up arrow or left arrow move up one line
        If e.KeyCode = Keys.Up OrElse e.KeyCode = Keys.Left Then
            SelectedIndex -= 1
            e.SuppressKeyPress = True
            iGV.Focus()
            Return
        End If

        'Escape, close without setting SelectedValue
        If e.KeyCode = Keys.Escape Then
            If iPopUp.Visible Then iPopUp.Close()
            Return
        End If

        'Enter key, close after setting SelectedValue to current row
        If e.KeyCode = Keys.Enter Then
            'set selected value to value in iGV row and close
            Dim R As DataGridViewRow = iGV.CurrentRow
            If R IsNot Nothing Then
                Me.SelectedValue = R.Cells(_SelectedValueColumnVar).Value.ToString
            End If
            'iGV.Visible = False
            If iPopUp.Visible Then iPopUp.Close()
            e.Handled = True
            Return
        End If

        'Backspace key, reduce iFindStr by one character
        If e.KeyCode = Keys.Back Then
            If iFindStr.Length > 0 Then
                iFindStr = Mid(iFindStr, 1, iFindStr.Length - 1)
            End If
        Else
            iFindStr &= TranslateKeyCode(e)
        End If

        AddWatch(iFindStr)

        If iTable IsNot Nothing Then
            Dim i As Integer = GetGVRowIndexLike(iFindStr, False)
            If i >= 0 And iGV.Rows.Count > i Then
                iGV.Rows(i).Selected = True
                For j As Integer = 0 To iGV.Columns.Count - 1
                    If iGV.Columns(j).Visible Then
                        iGV.CurrentCell = iGV.Rows(i).Cells(j)
                        Exit For
                    End If
                Next
            End If
        End If

    End Sub

    'iGV.RowsAdded
    Private Sub iGV_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles iGV.RowsAdded
        If iTable Is Nothing Then Return
        If iGV.Rows.Count = iTable.Rows.Count Then
            If iSelectedIndex = -1 And iSelectedValue IsNot Nothing Then
                Dim value As Object = iSelectedValue.ToString
                iSelectedValue = "!!!!!!!!!!!!!"
                SelectedValue = value.ToString
            End If
        End If
    End Sub

    'Initialize iFindStr unless text has just been entered
    Private Sub iTimer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles iTimer.Tick

        iTimer.Stop()

        If _TextJustEntered = False Then
            iFindStr = ""
        End If

        _TextJustEntered = False

    End Sub

    'iPopup Closing
    Private Sub iPopUp_Closing(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripDropDownClosingEventArgs) Handles iPopUp.Closing
        _PopupJustClose = True
        PopupClosedTimer.Interval = 200
        PopupClosedTimer.Start()
        'BHS DEBUG
        iPopUp._Content.MaximumSize = Nothing
        If iFBase IsNot Nothing Then
            iGV.Visible = False
            iFBase.Controls.Add(iGV) ' GBV 1/2/2010 - make it a child of the form, 
        End If

    End Sub

    'iPopup Closed
    Private Sub iPopUp_Closed(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripDropDownClosedEventArgs) Handles iPopUp.Closed

        iPopUp.ClearPopUp()
        iFindStr = ""   'Since we accumulate iFindStr, we need to intialize it when the popup closes


        iPopUp = Nothing
        RaiseEvent onDropDownClosed()
        RaiseEvent onDRDropDownClosed(Me)
    End Sub

    'iPopup Closed Timer
    Private Sub PopupClosedTimer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles PopupClosedTimer.Tick
        PopupClosedTimer.Stop()
        _PopupJustClose = False
        PositionLeft()  'BHS 11/15/10
    End Sub

    'iPopup Got Focus
    Private Sub iPopUp_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles iPopUp.GotFocus
        iGV.Focus()
        iFindStr = ""
    End Sub

#End Region


#Region "Obsolete"

    'Private Sub qDD_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.GotFocus
    '    iDropDownOpen = False
    'End Sub

    'Private Sub btnDD_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDD.GotFocus
    '    iDropDownOpen = False
    'End Sub

    'Sub New()

    '    ' This call is required by the Windows Form Designer.
    '    InitializeComponent()

    '    ' Add any initialization after the InitializeComponent() call.
    '    'Create ColDefs table
    '    If iColDef.Columns.Count = 0 Then
    '        iColDef.Columns.Add("ColName", GetType(System.String))
    '        iColDef.Columns.Add("ColWidth", GetType(System.Int32))
    '        iColDef.Columns.Add("ColHeader", GetType(System.String))
    '        iColDef.Columns.Add("ColVisible", GetType(System.Boolean))
    '    End If

    'End Sub

    '''' <summary> Clear all Rows in the dropdown </summary>
    'Public Sub ClearRows()
    '    iGV.Rows.Clear()
    '    If iTable IsNot Nothing Then iTable.Rows.Clear() ' GBV 2/1/2010
    '    iRow = Nothing
    'End Sub

    'Function GetfBaseLocation() As Point
    '    Dim C As Control = Me
    '    Dim X As Integer = 0
    '    Dim Y As Integer = 0
    '    Dim P As Point

    '    P = PointToScreen(Me.Location)
    '    'P = PointToClient(P)

    '    'Get position of this control in the fBase form
    '    'While C IsNot Nothing
    '    '    If C Is iFBase Then Exit While
    '    '    If TypeOf (C) Is Microsoft.VisualBasic.PowerPacks.DataRepeaterItem Then
    '    '        X += 20
    '    '        Y += 4
    '    '    Else
    '    '        X += C.Location.X
    '    '        Y += C.Location.Y
    '    '    End If
    '    '    C = C.Parent
    '    'End While

    '    'Control not in an fBase form
    '    'If Not TypeOf (C) Is fBase Then  GBV - control will always be in a form - 1/2/2010
    '    '    Return aC.Location
    '    'End If

    '    'aFBaseRef = TryCast(C, fBase) GBV 1/2/2010
    '    'iFBase = TryCast(C, Form)

    '    'P.X = X
    '    'P.Y = Y
    '    Return P

    'End Function


    'BHS 3/16/10 no longer used:
    ''Fit the Gridview within the fBase form
    'Function FitGV() As Point
    '    Dim X As Integer = iOrigLocation.X
    '    Dim Y As Integer = iOrigLocation.Y

    '    ' GBV 1/1/2010
    '    If iFBase Is Nothing Then Return iGV.Location

    '    'Gridview is in an fBase form, but is too wide to fit
    '    If iGV.Width + X + 10 > iFBase.Width Then
    '        'X = iFBase.Width - iGV.Width - 10 GBV 1/2/2010
    '        X -= ((iGV.Width - Me.Width) + 10) ' Switch it to the other end of the control
    '        If X < 0 Then X = 0
    '    End If

    '    'Gridview is too tall to fit.
    '    If iGV.Height + 50 + Y > iFBase.Height Then
    '        'Y = iFBase.Height - iGV.Height - 50   - GBV 1/2/2010
    '        Y -= (iGV.Height + Me.Height + 2) ' Switch it above the control
    '        If Y < 0 Then Y = 0
    '    End If

    '    Dim P As Point
    '    P.X = X
    '    P.Y = Y
    '    Return P

    'End Function

    '''' <summary> Clear all columns in the dropdown </summary>
    'Public Sub ClearColumns()  ' GBV 1/3/2010
    '    iGV.Columns.Clear()
    '    If iTable IsNot Nothing Then iTable.Columns.Clear() ' GBV 2/1/2010
    'End Sub

    'Private Sub txtCode_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtCode.KeyUp
    '    If _MustMatchListVar = True Then
    '        e.SuppressKeyPress = True
    '    Else
    '        If e.KeyValue < 35 Or e.KeyValue > 40 Then RaiseEvent onTxtCode_Entered() 'Skip arrow keys
    '    End If
    '    txtCode.Focus()
    'End Sub

    ' GBV 1/3/2010 - This emulates combobox behavior
    'Private Sub iGV_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles iGV.Leave
    '    If iGV.Visible = True Then
    '        iGV.Visible = False
    '        'RaiseEvent onDropDownClosed()
    '    End If
    'End Sub

    'Private Sub iGV_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles iGV.VisibleChanged
    '    If iGV.Visible = False Then
    '        RaiseEvent onDropDownClosed()
    '    End If

    'End Sub

    'Private Sub Control_MouseDown(ByVal sender As System.Object, _
    'ByVal e As System.Windows.Forms.MouseEventArgs) Handles iParent.MouseDown, iFBase.MouseDown
    '    If Not Me.Capture Then iGV.Visible = False
    'End Sub
#End Region

    
End Class


