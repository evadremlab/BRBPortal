Imports System.Windows.Forms
Imports System.Drawing
Imports QSILib
Imports QSILib.Windows.Forms
Imports Microsoft.VisualBasic.PowerPacks

'BHS 11/24/09
'ToDo - Copy qDR to the side, and then try changing iTable to iDV, so we can sort the DR.

'NEEDS TESTING:
'  Multi-col combobox
'  What if CB.SelectedValue is different from Text?
'  Checkboxes


<System.Serializable(), _
 System.ComponentModel.ComplexBindingProperties("DataSource", "DataMember")> _
Public Class qDR
    Inherits Microsoft.VisualBasic.PowerPacks.DataRepeater

    Public iRebuildingDR As Boolean = False 'True means don't display datatable changes to the repeater
    Public iTable As DataTable  'Reference to the datatable this control presents
    Public iForm As QSILib.Windows.Forms.fBase  'Reference to the form this control is a member of
    Private iControlName As String              'Used for setting focus to a control

    Private WithEvents iTimer As New Timer  'Used in InsertRowOnLeaving
    Private WithEvents iTimer2 As New Timer 'Used in DataSourceChanged
    Private WithEvents iFocusTimer As New Timer

    Private iAutoRowIsInserted As Boolean = False

    Private iNotVisibleYet As Boolean = True    'Set to False once DR is visible once.  Until then, CurrentItem is Nothing.
    'BHS 4/26/12
    Private iIsInSetup As Boolean = False

    Public Shadows Event onItemValueNeeded(ByVal e As Microsoft.VisualBasic.PowerPacks.DataRepeaterItemValueEventArgs, ByVal aR As DataRow)



#Region "Documentation"

    'Quartet's implementation of a Data Repeater
    '  Setup - must be called from the descendant program once a datatable is established, to link the DR to it.
    '
    '  DrawItem - for each control in the DR Template
    '      1) Call DRControlEnter in this object that shows _tooltip help upon entering a control
    '      2) Call fBase.ValidateControl which calls OnValidateControl when user attempts to leave a control
    '  DrawItem can also be used in the descendant program to set control properties (SetWriter, etc.) based on data in the row
    '
    '  ItemValueNeeded - formats data coming from the datatable into the Data Repeater.  This logic is
    '  similar to qFunctions BindControl Bind.Format logic.  If logic is changed in one place, it should probably be changed in the other.
    '
    '  ItemValuePushed - sends data from the Data Repeater to the DataTable, translating "" to DBNull, for instance.  This logic is
    '  similar to qFunctions BindControl Bind.Parse logic.  If logic is changed in one place, it should probably be changed in the other.
    '
    '  InsertRow - a method callable by the descendant program to insert a row in the DR.  
    '
    '  RemoveRow - a method callable by the descendant program to remove a row from the DR.  
    '
    '  GetTableRowNum - Given a DR row, finds the datatable row, by counting from the top and ignoring deleted datatable rows.
    '
    'Strategy for keeping datatable rows in synch with datarepeater rows:
    '  Any time a row is inserted, turn off the DR.Datasource, insert the row in the datatable, and turn back on the DR.datasource.
    '  This guarantees that the DR row order is the same as the datatable's.
    '
    'DropDownList Comboboxes present a special challenge, because even though we populate the Combobox in the ItemClone event, the dropdown's
    '  items.count = 0 in the ItemValueNeeded event.  This is only a problem when we first fill the DR after assigning a new datasource, so we
    '  set a timer in the datasource changed, and refresh any Dropdownlist combo boxes from the iTable one tick later.  See AfterDataSourceChanged
    '  method for details.
    '
    'BHS 2/16/10 - Control events need to be forwarded to the descendant form through fBase.  
    'Events about the form, such as control.enter, leave, validate, and validating can simply call an existing fBase
    'function that deals with that form event.
    'Events about the control, such as OnDropDown, OnDropDownClosed, and SelectedIndexChanging, 
    'need to fire fBase functions that pass a reference to the specific control and the event information
    'to the descendant.  Coding for these events in the descendant must be based on the fBase event, rather than the contol event.
    'For instance, for a qDD Ondropdown, code for DRDDOnDropdown in the descendant rather than Ondropdown.
    '
    'BHS 3/5/10 qDD Validating and Validated fired only when qDD.CompositeValidating and CompositeValidated fires, so clicking DD arrow 
    'doesn't fire validating.  Handle statements point directly at iForm.methods.



#End Region

#Region " KeyFields Property "

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

#Region " InsertRowOnLeaving Property "

    Private _InsertRowOnLeavingVar As String = String.Empty
    <System.ComponentModel.Category("Behavior"), _
            System.ComponentModel.Description("The name of the control where OnValidated True causes a new row to be inserted in the DR.  Requires KeyFields property be filled in."), _
            System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible), _
            System.ComponentModel.DefaultValue("")> _
    Public Overridable Property _InsertRowOnLeaving() As String
        Get
            Return _InsertRowOnLeavingVar
        End Get
        Set(ByVal value As String)
            _InsertRowOnLeavingVar = value
        End Set
    End Property

#End Region

#Region "Methods and Events shared with the descendant"

    ''' <summary> Set necessary Data Repeater Instance Variables </summary>
    Public Function Setup(ByVal aTable As DataTable, _
                     ByVal aForm As QSILib.Windows.Forms.fBase, _
                     Optional ByVal aInsertAtTop As Boolean = True, _
                     Optional ByVal aInsertRow As Boolean = True) As DataRow
        'BHS 4/26/12
        iIsInSetup = True
        iTable = aTable
        iForm = aForm
        'SRM R5.1.12 12/20/11 Initialize Virtual mode 
        VirtualMode = False

        VirtualMode = True
        If iTable.Rows.Count = 0 And aInsertRow = True Then
            Return InsertRow(aInsertAtTop)
        Else
            'Show new table in DR
            iRebuildingDR = False
            DataSource = iTable
            Return Nothing
        End If

        'BHS 4/26/12
        iIsInSetup = False
    End Function

    'Create events for ControlEnter and OnValidate for each DR control
    Private Sub qDR_DrawItem(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.PowerPacks.DataRepeaterItemEventArgs) Handles Me.DrawItem
        Try
            For Each C As Control In e.DataRepeaterItem.Controls

                If isEntryControl(C) Then
                    If iForm Is Nothing Then
                        MsgBox("Programmer error - Data Repeater iForm not set")
                        Return
                    End If
                    Dim DD As qDD = TryCast(C, qDD)
                    If DD Is Nothing Then
                        'BHS 3/5/10
                        'Since DrawItem can get called more than once for a control, 
                        'we remove and add handler, so only one handler call is made to the method
                        RemoveHandler C.Validating, AddressOf iForm.EntryControl_Validating
                        RemoveHandler C.Validated, AddressOf iForm.EntryControl_Validated
                        RemoveHandler C.Enter, AddressOf iForm.EntryControl_Enter
                        RemoveHandler C.Leave, AddressOf iForm.EntryControl_Leave
                        AddHandler C.Validating, AddressOf iForm.EntryControl_Validating
                        AddHandler C.Validated, AddressOf iForm.EntryControl_Validated
                        AddHandler C.Enter, AddressOf iForm.EntryControl_Enter
                        AddHandler C.Leave, AddressOf iForm.EntryControl_Leave

                        'BHS 8/27/10 Set MaxLength if this is a qTextBox
                        Dim qT As qTextBox = TryCast(C, qTextBox)
                        If qT IsNot Nothing Then
                            If iTable IsNot Nothing Then
                                Dim ColName As String = qT._BindDef
                                Dim i As Integer = ColName.IndexOf(".")
                                ColName = Mid(ColName, i + 2)
                                'BHS 9/8/10
                                If ColName > "" Then
                                    Dim Col As DataColumn = iTable.Columns(ColName)
                                    If Col.MaxLength > -1 And qT.MaxLength > 32000 Then
                                        qT.MaxLength = Col.MaxLength
                                    End If
                                End If
                            End If
                        End If
                    Else
                        RemoveHandler DD.CompositeValidating, AddressOf iForm.EntryControl_Validating
                        RemoveHandler DD.CompositeValidated, AddressOf iForm.EntryControl_Validated
                        RemoveHandler C.Enter, AddressOf iForm.EntryControl_Enter
                        RemoveHandler C.Leave, AddressOf iForm.EntryControl_Leave
                        AddHandler DD.CompositeValidating, AddressOf iForm.EntryControl_Validating
                        AddHandler DD.CompositeValidated, AddressOf iForm.EntryControl_Validated
                        AddHandler C.Enter, AddressOf iForm.EntryControl_Enter
                        AddHandler C.Leave, AddressOf iForm.EntryControl_Leave
                        RemoveHandler DD.onDRDropDown, AddressOf iForm.RaiseDRDDOnDropdown
                        AddHandler DD.onDRDropDown, AddressOf iForm.RaiseDRDDOnDropdown
                        'BHS 6/8/10
                        RemoveHandler DD.onDRDropDownClosed, AddressOf iForm.RaiseDRDDOnDropdownClosed
                        AddHandler DD.onDRDropDownClosed, AddressOf iForm.RaiseDRDDOnDropdownClosed
                        'BHS 6/15/10
                        RemoveHandler DD.DRSelectedIndexChanged, AddressOf iForm.RaiseDRDDSelectedIndexChanged
                        AddHandler DD.DRSelectedIndexChanged, AddressOf iForm.RaiseDRDDSelectedIndexChanged
                        'BHS 8/5/10
                        RemoveHandler DD.onDRDoubleClick, AddressOf iForm.RaiseDRDDDoubleClick
                        AddHandler DD.onDRDoubleClick, AddressOf iForm.RaiseDRDDDoubleClick

                        'BHS 8/27/10
                        If iTable IsNot Nothing Then
                            Dim ColName As String = DD._BindDef
                            Dim i As Integer = ColName.IndexOf(".")
                            ColName = Mid(ColName, i + 2)
                            If ColName > "" Then
                                Dim Col As DataColumn = iTable.Columns(ColName)
                                If Col.MaxLength > -1 And DD.MaxLength > 32000 Then
                                    DD.MaxLength = Col.MaxLength
                                End If
                            End If
                        End If
                    End If

                    'AddHandler C.Leave, AddressOf ControlLeave

                    ''BHS 2/16/10
                    'Dim DD As qDD = TryCast(C, qDD)
                    'If DD IsNot Nothing Then
                    '    AddHandler DD.onDRDropDown, AddressOf OnDRDropDown
                    '    AddHandler DD.onDRDropDownClosed, AddressOf OnDRDropDownClosed
                    '    AddHandler DD.DRSelectedIndexChanged, AddressOf OnDRSelectedIndexChanged
                    'End If

                    'BHS 3/23 If within an feMain form, SetControl Attributes for this Control
                    ' GBV 7/23/2015 - Ticket 3952 - Added support for feMain2
                    Dim FeM As feMain = TryCast(Me.ParentForm, feMain)
                    If FeM IsNot Nothing Then
                        FeM.SetControlAttributes(C)
                    Else
                        Dim FeM2 As feMain2 = TryCast(Me.ParentForm, feMain2)
                        If FeM2 IsNot Nothing Then
                            FeM2.SetControlAttributes(C)
                        End If
                    End If

                    'BHS 8/24/10
                    'SRM 9/16/10 check if application wants to set background color
                    If gSetqDRBackgoundColor Then
                        If e.DataRepeaterItem.ItemIndex = Me.CurrentItemIndex Then
                            e.DataRepeaterItem.BackColor = QSelectionBackColor
                        Else
                            e.DataRepeaterItem.BackColor = Appl.QDefaultRowBackColor
                        End If
                    End If
                End If
            Next

        Catch ex As Exception
            ShowError("Unexpected error in DrawItem.", ex)
        End Try
    End Sub



    'PROGRAMMER'S NOTE - IF YOU MAKE CHANGES HERE, CONSIDER MAKING THEM IN qFunctions BindControl Binding.Format logic as well
    'Format data coming from the datatable into the Data Repeater
    Private Sub qDR_ItemValueNeeded(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.PowerPacks.DataRepeaterItemValueEventArgs) Handles Me.ItemValueNeeded
        'Both control.Text and e.Value must be set as strings
        If iRebuildingDR = True Then Return

        Try
            If e.ItemIndex > -1 And e.ItemIndex < ItemCount Then

                If iTable Is Nothing Then
                    MsgBox("Programmer Error - qDR.iTable needs to be set")
                    iRebuildingDR = True    'Prevent continued effort to refresh DR data
                    Return
                End If

                Dim TableRowNum As Integer = GetTableRowNum(e.ItemIndex)

                If TableRowNum = -1 Then
                    MsgBox("In ItemValueNeeded, Bad TableRowNum for DR ItemIndex = " & e.ItemIndex.ToString)
                    iRebuildingDR = True    'Prevent continued effort to refresh DR data
                    Return
                End If

                'Get current datatable row
                Dim R As DataRow = iTable.Rows(TableRowNum)
                If Not RowIsDeleted(R) Then

                    RaiseEvent onItemValueNeeded(e, R)

                    'qTextBox
                    Dim qT As qTextBox = TryCast(e.Control, qTextBox)
                    If qT IsNot Nothing Then
                        If qT._BindDef.Length > 0 Then
                            qT.Text = DBValueToString(R.Item(qT._BindDef), _
                                                  qT._DataType, qT._Format)
                            e.Value = qT.Text
                        End If
                        Return
                    End If


                    'qRC
                    Dim RC As qRC = TryCast(e.Control, qRC)
                    If RC IsNot Nothing Then
                        If RC._BindDef.Length > 0 Then
                            RC._DBText = DBValueToString(R.Item(RC._BindDef), _
                                                  RC._DataType)
                            e.Value = RC._DBText
                        End If
                        Return
                    End If

                    'DD 
                    Dim DD As qDD = TryCast(e.Control, qDD)
                    If DD IsNot Nothing Then
                        If DD._BindDef.Length > 0 Then
                            If DD._MustMatchList = True Then
                                'BHS 12/15/09 Don't try to put "" into a DBNull, or ItemValueNeeded will keep firing
                                If IsDBNull(DD.SelectedValue) Or _
                                 (Not IsDBNull(DD.SelectedValue) And DD.SelectedValue Is Nothing) Then
                                    If DBValueToString(R.Item(DD._BindDef), _
                                                                DD._DataType, DD._Format) > "" Then
                                        DD.SelectedValue = DBValueToString(R.Item(DD._BindDef), _
                                                                DD._DataType, DD._Format)
                                    End If
                                Else
                                    DD.SelectedValue = DBValueToString(R.Item(DD._BindDef), _
                                                                DD._DataType, DD._Format)
                                End If
                            Else
                                DD.TextInfo = DBValueToString(R.Item(DD._BindDef), _
                                                          DD._DataType, DD._Format)
                            End If
                            e.Value = DD.TextInfo
                        Else
                            'BHS 3/4/10 If _BindDef.Length = 0 Then don't update this control
                            'DD.TextInfo = DBValueToString(R.Item(DD._BindDef), _
                            '                           DD._DataType)
                            'e.Value = DD.TextInfo
                        End If
                        Return
                    End If


                    'qCB 
                    Dim qCB As qComboBox = TryCast(e.Control, qComboBox)
                    If qCB IsNot Nothing Then
                        If qCB._BindDef.Length > 0 Then
                            If qCB.DropDownStyle = ComboBoxStyle.DropDownList Then
                                'WE MAY WANT TO ADD _FORMAT TO qCB
                                If qCB.Items.Count = 0 Then
                                    'Can't assign SelectedValue when Items.Count = 0.  We'll try again 
                                    '  in the AfterDataSourceChanged method in this object
                                Else
                                    'BHS 12/15/09 Don't try to put "" into a DBNull, or ItemValueNeeded will keep firing
                                    If IsDBNull(qCB.SelectedValue) Then
                                        If DBValueToString(R.Item(qCB._BindDef), _
                                                                    qCB._DataType) > "" Then
                                            qCB.SelectedValue = DBValueToString(R.Item(qCB._BindDef), _
                                                                    qCB._DataType)
                                        End If
                                    Else
                                        qCB.SelectedValue = DBValueToString(R.Item(qCB._BindDef), _
                                                                    qCB._DataType)
                                    End If

                                End If
                                e.Value = qCB.Text
                            Else
                                qCB.Text = DBValueToString(R.Item(qCB._BindDef), _
                                                           qCB._DataType)
                                e.Value = qCB.Text
                            End If
                        End If
                        Return
                    End If

                    'qMaskedTextBox
                    Dim qMT As qMaskedTextBox = TryCast(e.Control, qMaskedTextBox)
                    If qMT IsNot Nothing Then
                        If qMT._BindDef.Length > 0 Then
                            qMT.Text = DBValueToString(R.Item(qMT._BindDef), _
                                                       qMT._DataType)
                            e.Value = qMT.Text
                        End If
                        Return
                    End If

                    'qDateTimePicker()
                    Dim qDP As qDateTimePicker = TryCast(e.Control, qDateTimePicker)
                    If qDP IsNot Nothing Then
                        If qDP._BindDef.Length > 0 Then
                            qDP.Text = DBValueToString(R.Item(qDP._BindDef), _
                                                       qDP._DataType)
                            e.Value = qDP.Text
                        End If
                        Return
                    End If

                    'qCheckBox
                    Dim qXB As qCheckBox = TryCast(e.Control, qCheckBox)
                    If qXB IsNot Nothing Then
                        If qXB._BindDef.Length > 0 Then
                            qXB.Checked = DBValueToBoolean(R.Item(qXB._BindDef), _
                                                            qXB._CheckedTrueValue)
                            'BHS 3/31/10 Removed setting e.Value.  We need to make sure that this saves to DB OK
                            'e.Value = qXB.Checked.ToString

                        End If
                        Return
                    End If

                End If
            End If

        Catch ex As Exception
            ShowError("Error in ItemValueNeeded", ex)
        End Try

    End Sub

    'PROGRAMMER'S NOTE - IF YOU MAKE CHANGES HERE, CONSIDER MAKING THEM IN qFunctions BindControl Binding.Parse logic as well
    'Parse data from DR to the datatable
    Private Sub qDR_ItemValuePushed(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.PowerPacks.DataRepeaterItemValueEventArgs) Handles Me.ItemValuePushed
        Try
            If iRebuildingDR = True Then Return


            If e.ItemIndex > -1 And e.ItemIndex < ItemCount Then
                If iTable Is Nothing Then
                    MsgBox("Programmer Error - qDR.iTable needs to be set")
                    iRebuildingDR = True    'Prevent continued effort to refresh DR data
                    Return
                End If

                Dim TableRowNum As Integer = GetTableRowNum(e.ItemIndex)

                If TableRowNum = -1 Then
                    'SRM R5.1.12 12/20/11 Removed to allow initializing DR on Next/Previous navigation
                    'MsgBox("In ItemValuePushed, Bad TableRowNum for DR ItemIndex = " & e.ItemIndex.ToString)
                    iRebuildingDR = True    'Prevent continued effort to push DR data
                    Return
                End If

                Dim R As DataRow = iTable.Rows(TableRowNum)
                If Not RowIsDeleted(R) Then

                    Dim qT As qTextBox = TryCast(e.Control, qTextBox)
                    If qT IsNot Nothing Then
                        If qT._BindDef.Length > 0 Then
                            Dim O As Object = StringToDBValue(e.Value.ToString, qT._DataType)
                            If O.ToString <> "QSI Bad Value" Then
                                If StringCompare(e.Value.ToString, GetItemString(R, qT._BindDef)) = False Then
                                    Try
                                        R.Item(qT._BindDef) = O
                                    Catch ex As Exception   'Ignore assignment problems
                                    End Try
                                End If
                            End If

                        End If
                        Return
                    End If

                    Dim RC As qRC = TryCast(e.Control, qRC)
                    If RC IsNot Nothing Then
                        If RC._BindDef.Length > 0 Then
                            Dim O As Object = StringToDBValue(e.Value.ToString, RC._DataType)
                            If O.ToString <> "QSI Bad Value" Then
                                If StringCompare(e.Value.ToString, GetItemString(R, RC._BindDef)) = False Then
                                    Try
                                        R.Item(RC._BindDef) = O
                                    Catch ex As Exception   'Ignore assignment problems
                                    End Try
                                End If
                            End If

                        End If
                        Return
                    End If

                    Dim DD As qDD = TryCast(e.Control, qDD)
                    If DD IsNot Nothing Then
                        If DD._BindDef.Length > 0 Then
                            Dim O As Object
                            If DD._MustMatchList = True Then
                                If DD.SelectedValue Is Nothing Then
                                    O = ""
                                Else
                                    O = StringToDBValue(DD.SelectedValue.ToString, DD._DataType)
                                End If
                            Else
                                O = StringToDBValue(e.Value.ToString, DD._DataType) 'Is this different from DD.TextInfo?
                            End If
                            If O.ToString <> "QSI Bad Value" Then
                                If StringCompare(O.ToString, GetItemString(R, DD._BindDef)) = False Then
                                    Try
                                        R.Item(DD._BindDef) = O
                                    Catch ex As Exception   'Ignore assignment problems
                                    End Try
                                End If
                            End If
                        End If
                        Return
                    End If


                    Dim qCB As qComboBox = TryCast(e.Control, qComboBox)
                    If qCB IsNot Nothing Then
                        If qCB._BindDef.Length > 0 Then
                            Dim O As Object
                            If qCB.DropDownStyle = ComboBoxStyle.DropDownList Then
                                If qCB.SelectedValue Is Nothing Then
                                    O = ""
                                Else
                                    O = StringToDBValue(qCB.SelectedValue.ToString, qCB._DataType)
                                End If
                            Else
                                O = StringToDBValue(e.Value.ToString, qCB._DataType)
                            End If
                            If O.ToString <> "QSI Bad Value" Then
                                If StringCompare(O.ToString, GetItemString(R, qCB._BindDef)) = False Then
                                    Try
                                        R.Item(qCB._BindDef) = O
                                    Catch ex As Exception
                                    End Try

                                End If
                            End If

                        End If
                        Return
                    End If

                    'qMaskedTextBox
                    Dim qMT As qMaskedTextBox = TryCast(e.Control, qMaskedTextBox)
                    If qMT IsNot Nothing Then
                        If qMT._BindDef.Length > 0 Then
                            Dim O As Object = StringToDBValue(e.Value.ToString, qMT._DataType)
                            If O.ToString <> "QSI Bad Value" Then
                                If StringCompare(e.Value.ToString, GetItemString(R, qMT._BindDef)) = False Then
                                    Try
                                        R.Item(qMT._BindDef) = O
                                    Catch ex As Exception   'Ignore assignment problems
                                    End Try
                                End If
                            End If

                        End If
                        Return
                    End If

                    'qDateTimePicker()
                    Dim qDP As qDateTimePicker = TryCast(e.Control, qDateTimePicker)
                    If qDP IsNot Nothing Then
                        If qDP._BindDef.Length > 0 Then
                            Dim O As Object = StringToDBValue(e.Value.ToString, qDP._DataType)
                            If O.ToString <> "QSI Bad Value" Then
                                If StringCompare(e.Value.ToString, GetItemString(R, qDP._BindDef)) = False Then
                                    Try
                                        R.Item(qDP._BindDef) = O
                                    Catch ex As Exception   'Ignore assignment problems
                                    End Try
                                End If
                            End If

                        End If
                        Return
                    End If

                    'qCheckBox
                    Dim qXB As qCheckBox = TryCast(e.Control, qCheckBox)
                    If qXB IsNot Nothing Then
                        If qXB._BindDef.Length > 0 Then
                            Dim O As Object = BooleanToDBValue(CBool(e.Value), qXB._CheckedTrueValue, qXB._CheckedFalseValue)
                            If O.ToString <> "QSI Bad Value" Then
                                If StringCompare(e.Value.ToString, GetItemString(R, qXB._BindDef)) = False Then
                                    Try
                                        R.Item(qXB._BindDef) = O
                                    Catch ex As Exception   'Ignore assignment problems
                                    End Try
                                End If
                            End If
                        End If
                    End If
                    Return

                End If
            End If

        Catch ex As Exception
            ShowError("Error in ItemValuePushed", ex)
        End Try
    End Sub



    'Swap shallow copies of a row's itemarray and rowstate
    ''' <summary> Pass the DR item number and True for up or False for down.  Returns False if ToRow out of range. </summary>
    Function MoveRow(ByVal aDRItemIndex As Integer, ByVal aUp As Boolean) As Boolean

        'Point To FromRow
        Dim FromRowNum As Integer = GetTableRowNum(aDRItemIndex)
        Dim FromR As DataRow = iTable.Rows(FromRowNum)

        'Point to ToRowNum based on aUp, making sure result is within the table's row array
        Dim ToRowNum As Integer = FromRowNum
        '...find the "To" row, but make sure it isn't deleted
        Do While 1 = 1
            If aUp = True Then ToRowNum -= 1 Else ToRowNum += 1
            If ToRowNum < 0 Or ToRowNum > iTable.Rows.Count - 1 Then
                Return False
            End If
            If iTable.Rows(ToRowNum).RowState <> DataRowState.Deleted Then Exit Do
        Loop

        'Create a separate ToRow and save its RowState
        Dim ToR As DataRow = iTable.NewRow
        ToR.ItemArray = iTable.Rows(ToRowNum).ItemArray
        Dim ToRState As DataRowState = iTable.Rows(ToRowNum).RowState

        'Turn off DR Datasource
        iRebuildingDR = True
        DataSource = Nothing

        'Do shallow copy of FromRow to ToRow, including row state
        iTable.Rows(ToRowNum).ItemArray = FromR.ItemArray
        SetRowState(iTable.Rows(ToRowNum), FromR.RowState)

        'Do a shallow copy of separate ToRow into FromRow, including row state
        iTable.Rows(FromRowNum).ItemArray = ToR.ItemArray
        SetRowState(iTable.Rows(FromRowNum), ToRState)

        'Turn on DR datasource back on
        iRebuildingDR = False
        DataSource = iTable

        'Point to the moved row
        If aUp = True Then Me.CurrentItemIndex -= 1
        If aUp = False Then Me.CurrentItemIndex += 1

        Return True

    End Function

    Sub SetRowState(ByRef aR As DataRow, ByVal aState As DataRowState)
        If aState = DataRowState.Unchanged Then aR.AcceptChanges()
        If aState = DataRowState.Added Then aR.AcceptChanges() : aR.SetAdded()
        If aState = DataRowState.Modified Then aR.AcceptChanges() : aR.SetModified()
    End Sub

    'Insert a DR row by inserting a row in the datatable (top or bottom)
    Function InsertRow(Optional ByVal aInsertAtTop As Boolean = True, _
                       Optional ByVal aFocusControlName As String = "") As DataRow

        'Force validation and ItemValueNeeded so data gets to datatable
        If Me.CurrentItem IsNot Nothing Then Me.CurrentItem.Controls(0).Focus()

        'Temporarily turn off interaction between datatable and DR
        iRebuildingDR = True
        DataSource = Nothing

        'Insert Table Row
        Dim R As DataRow = iTable.NewRow()
        If aInsertAtTop = True Then
            iTable.Rows.InsertAt(R, 0)  'Insert row at the top
        Else
            iTable.Rows.Add(R)  'Insert row at the bottom
        End If


        'Show new table in DR
        iRebuildingDR = False
        DataSource = iTable

        'Scroll to inserted row
        If Visible = True Then
            If aInsertAtTop = True Then
                CurrentItemIndex = 0
                ScrollItemIntoView(0)
            Else
                CurrentItemIndex = ItemCount - 1 'BHS 3/22/10 Changed 0 to ItemCount - 1
                ScrollItemIntoView(ItemCount - 1)
            End If
        End If

        If aFocusControlName > "" Then SetFocus(aFocusControlName)

        Return R
    End Function

    'BHS 3/4/11
    ''' <summary> Insert a DR row at a given position </summary>
    Function InsertRowAt(ByVal aPosition As Integer, _
                         Optional ByVal aFocusControlName As String = "") As DataRow

        Dim isVisible As Boolean = Me.Visible

        'Force validation and ItemValueNeeded so data gets to datatable
        If Me.CurrentItem IsNot Nothing Then Me.CurrentItem.Controls(0).Focus()

        'Temporarily turn off interaction between datatable and DR
        Me.iRebuildingDR = True
        Me.Visible = False
        Me.DataSource = Nothing

        'Insert Table Row
        Dim R As DataRow = Me.iTable.NewRow()
        Me.iTable.Rows.InsertAt(R, aPosition)

        'Show new table in DR
        Me.iRebuildingDR = False
        Me.DataSource = Me.iTable
        Me.Visible = isVisible

        'Scroll to inserted row
        If Me.Visible = True Then
            If aPosition < Me.ItemCount Then
                Me.CurrentItemIndex = aPosition
                Me.ScrollItemIntoView(aPosition)
            End If
        End If

        'BHS 3/15/11
        If aFocusControlName > "" Then SetFocus(aFocusControlName)

        Return R
    End Function

    Function RemoveRow(Optional ByVal aPrompt As String = "OK to remove this row?") As Boolean 'Remove the current row
        If iTable.Rows.Count = 0 Then Return False
        Try
            '...confirm user wants to remove this row
            If aPrompt.Length > 0 Then
                'DJW R5.3? 4/30/12 Added a title to the message box for consistency per DMS Ticket 1303.
                If MsgBox(aPrompt, MsgBoxStyle.YesNo, "Warning") <> MsgBoxResult.Yes Then Return False
            End If

            '...remove row
            If CurrentItemIndex > -1 Then
                RemoveAt(CurrentItemIndex)
                'For some reason, this also marks the datatable.RowState to Deleted, which is nice...
            End If

            Return True

        Catch ex As Exception
            ShowError("Error removing row", ex)
        End Try

    End Function

    ''' <summary> Returns True if aFieldName's Value matches that of another row.  aFieldName and aValue may have many values, separated by | </summary>
    Public Function DupeProblem(ByVal aFieldName As String, ByVal aValue As String, Optional ByVal aIgnoreEmptyStrings As Boolean = True) As Boolean
        Dim iCount As Integer = 0

        Dim FieldNames As String = aFieldName
        Dim FieldName As String = ParseStr(FieldNames, "|")

        Dim Values As String = aValue
        Dim Value As String = ParseStr(Values, "|")

        If Value = "" And aIgnoreEmptyStrings = True Then Return False

        Dim FullValue As String = ""
        Dim FullCompare As String = ""

        Do While FieldName.Length > 0
            FullValue += StandardValue(Value)    'Put standard date or numeric format on Value
            FieldName = ParseStr(FieldNames, "|")   'Get Next Field Name, if any
            If Values > "" Then Value = ParseStr(Values, "|")
        Loop

        For Each R As DataRow In iTable.Rows
            If Not RowIsDeleted(R) Then
                FieldNames = aFieldName
                FieldName = ParseStr(FieldNames, "|")
                Do While FieldName.Length > 0
                    FullCompare += StandardValue(GetItemString(R, FieldName))
                    FieldName = ParseStr(FieldNames, "|")
                Loop
                If FullValue = FullCompare Then iCount += 1
                FullCompare = ""

            End If
        Next

        Return iCount > 1

    End Function

    '''<summary>Finds Table Row that correspondes to DR ItemIndex </summary>
    Public Function GetTableRowNum(ByVal aDRItemIndex As Integer) As Integer
        'Count non-deleted rows until we get to the aDRItemIndex, and return that rownumber
        'SRM 06/12/2013 check if itable is not nothing befor going through rows -- cause fatal in 994
        If iTable IsNot Nothing Then
            Dim ValidRowNum As Integer = 0
            For TableRow As Integer = 0 To iTable.Rows.Count - 1
                Dim R As DataRow = iTable.Rows(TableRow)
                If Not RowIsDeleted(R) Then
                    If ValidRowNum = aDRItemIndex Then Return TableRow
                    ValidRowNum += 1
                End If
            Next
        End If

        Return -1   'Means routine is broken

    End Function

#End Region

#Region "Internal Methods and Events"

    'Private Sub DRControlEnter(ByVal sender As Object, ByVal e As System.EventArgs)
    '    If iForm Is Nothing Then
    '        MsgBox("Programmer error - Data Repeater iForm not set")
    '        Return
    '    End If

    '    'BHS 2/16/10  call iForm.EntryControl_Enter rather than redoing some of the logic
    '    iForm.EntryControl_Enter(Me, e)

    '    'Dim C As Control = CType(sender, Control)

    '    'If C Is Nothing Then
    '    '    iForm.ShowHelp("")
    '    'Else
    '    '    Dim qT As qTextBox = TryCast(C, qTextBox)
    '    '    If qT IsNot Nothing Then iForm.ShowHelp(qT._ToolTip)
    '    '    Dim DD As qDD = TryCast(C, qDD)
    '    '    If DD IsNot Nothing Then iForm.ShowHelp(DD._ToolTip)
    '    '    Dim qCB As qComboBox = TryCast(C, qComboBox)
    '    '    If qCB IsNot Nothing Then iForm.ShowHelp(qCB._ToolTip)
    '    '    Dim qCH As qCheckBox = TryCast(C, qCheckBox)
    '    '    If qCH IsNot Nothing Then iForm.ShowHelp(qCH._ToolTip)
    '    'End If
    'End Sub

    ''Trigger validation for all controls in repeater
    'Private Sub ControlValidating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs)
    '    If iForm Is Nothing Then
    '        MsgBox("Programmer error - Data Repeater iForm not set")
    '        Return
    '    End If

    '    iForm.ValidateControl(sender, e)

    'End Sub

    ''BHS 3/5/10
    ''Trigger validation for all controls in repeater
    'Private Sub ControlValidated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    If iForm Is Nothing Then
    '        MsgBox("Programmer error - Data Repeater iForm not set")
    '        Return
    '    End If

    '    Dim C As Control = CType(sender, Control)

    '    iForm.iEP.SetError(C, "")
    '    iForm.ShowStatus("")
    '    iForm.iOKToClose = True
    '    iForm.iLastEF = C


    'End Sub


    'Trigger OnDropDown for qDD
    Private Sub OnDRDropDown(ByRef aDD As qDD, ByRef aOK As Boolean)
        If iForm Is Nothing Then
            MsgBox("Programmer error - Data Repeater iForm not set")
            Return
        End If

        'BHS 2/16/10
        iForm.RaiseDRDDOnDropdown(aDD, aOK)

    End Sub

    'Trigger OnDRDropDownClosed for qDD
    Private Sub OnDRDropDownClosed(ByRef aDD As qDD)
        If iForm Is Nothing Then
            MsgBox("Programmer error - Data Repeater iForm not set")
            Return
        End If

        'BHS 2/16/10
        iForm.RaiseDRDDOnDropdownClosed(aDD)

    End Sub



    'Trigger OnDRSelectedIndexChanged for qDD
    Private Sub OnDRSelectedIndexChanged(ByRef aDD As qDD, ByRef aOK As Boolean)
        If iForm Is Nothing Then
            MsgBox("Programmer error - Data Repeater iForm not set")
            Return
        End If

        'BHS 2/16/10
        iForm.RaiseDRDDSelectedIndexChanged(aDD, aOK)

    End Sub

    ''Trigger OnDropDown for qDD
    'Private Sub OnDropDown(ByRef aDD As qDD, ByRef aOK As Boolean)
    '    If iForm Is Nothing Then
    '        MsgBox("Programmer error - Data Repeater iForm not set")
    '        Return
    '    End If

    '    'BHS 2/16/10
    '    iForm.RaiseDRDDOnDropdown(aDD, aOK)

    'End Sub


    'If _InsertRowOnLeaving and _KeyFields are specified, automatically insert a new row when leaving _InsertRowOnLeaving control
    Private Sub ControlLeave(ByVal sender As Object, ByVal e As System.EventArgs)
        If iRebuildingDR = True Then Return

        Dim C As Control = TryCast(sender, Control)
        If C IsNot Nothing Then
            If C.Name.Trim = Me._InsertRowOnLeaving.Trim Then
                iTimer.Start()
            End If
        End If
    End Sub

    'Timer routine to insert a row if there is not already an empty row
    Private Sub InsertRowOnLeaving(ByVal sender As Object, ByVal e As System.EventArgs) Handles iTimer.Tick
        iTimer.Stop()
        'Only insert a row if there isn't already an empty row
        If _KeyFields.Length > 0 Then
            For Each R As DataRow In iTable.Rows
                If Not RowIsDeleted(R) Then
                    Dim KeyFields As String = _KeyFields
                    Dim FirstKeyField = ParseStr(KeyFields, "|")
                    If FirstKeyField.Length > 0 Then
                        If R.Item(FirstKeyField).ToString.Length = 0 Then Return 'Already an inserted row
                    End If
                End If
            Next
        End If
        InsertRow()
    End Sub



    'When a datasource is changed, the combobox.items.count hasn't had a chance to grow above 0, so we have
    'to assign SelectedValues to dropdownlist comboboxes one tick after the datasource changes.
    Private Sub qDR_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DataSourceChanged
        If DataSource IsNot Nothing Then iTimer2.Start()
    End Sub

    'Call AfterDataSourceChanged if DR just became visible for the first time
    Private Sub qDR_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.VisibleChanged
        If Visible = True And iNotVisibleYet = True Then
            iNotVisibleYet = False
            iTimer2.Start()
        End If
    End Sub


    'Move qCB.iSelectedValueHold to SelectedValue, after waiting a tick for the qCB.Items array to be filled
    Private Sub AfterDataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles iTimer2.Tick
        If iTimer2 Is Nothing Then Return

        iTimer2.Stop()
        If ItemCount > 0 Then
            Dim CI As Integer = CurrentItemIndex

            For i As Integer = 0 To ItemCount - 1

                CurrentItemIndex = i
                'BHS 4/26/12
                If CurrentItem IsNot Nothing AndAlso iIsInSetup = False Then   'This won't work until the DR has been visible once

                    Dim TableRowNum As Integer = GetTableRowNum(i)

                    If TableRowNum = -1 Then
                        MsgBox("In AfterDataSourceChanged, Bad TableRowNum for DR ItemIndex = " & i.ToString)
                        Return
                    End If

                    'Get current datatable row
                    Dim R As DataRow = iTable.Rows(TableRowNum)
                    If Not RowIsDeleted(R) Then
                        For Each C As Control In CurrentItem.Controls
                            Dim qCB As qComboBox = TryCast(C, qComboBox)
                            If qCB IsNot Nothing Then
                                If qCB.DropDownStyle = ComboBoxStyle.DropDownList Then
                                    If qCB._BindDef IsNot Nothing AndAlso qCB._BindDef.Length > 0 Then
                                        qCB.SelectedValue = GetItemString(R, qCB._BindDef)
                                    End If
                                End If
                            End If
                        Next
                    End If
                End If
            Next

            CurrentItemIndex = CI
            If CurrentItem IsNot Nothing Then ScrollItemIntoView(CI)

        End If

    End Sub


    'Prevent user from using Delete key to delete a row - must use [-] button
    Private Sub qDR_UserDeletingItems(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.PowerPacks.DataRepeaterAddRemoveItemsCancelEventArgs) Handles Me.UserDeletingItems
        e.Cancel = True
    End Sub

    Public Sub SetFocus(ByVal aControlName As String)
        iControlName = aControlName
        iFocusTImer.Start()
    End Sub

    Private Sub iFocusTImer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles iFocusTImer.Tick
        iFocusTImer.Stop()
        CurrentItem.Focus()
        CurrentItem.Controls(iControlName).Focus()
    End Sub
#End Region

#Region "Functions To Be Moved to qFunctions"

    '''<summary>Remove Empty Rows, defined as rows with no entries in aPipeDelimitedListofTableFieldsToCheck fields.  Set aCheckForBlanks = False if your key has dates. </summary>
    Public Function RemoveEmptyRows(ByVal aTable As DataTable, _
                                    ByVal aPipeDelimitedListOfTableFieldsToCheck As String, _
                                    Optional ByVal aCheckForBlanks As Boolean = True) As Boolean
        Dim SelectStr As String = ""
        Dim FieldName As String = ""
        While aPipeDelimitedListOfTableFieldsToCheck.Length > 0
            FieldName = ParseStr(aPipeDelimitedListOfTableFieldsToCheck, "|")
            If SelectStr.Length > 0 Then SelectStr &= " And "
            If aCheckForBlanks = True Then
                SelectStr &= " ( " & FieldName & " Is NULL Or " & FieldName & " = '' ) "
            Else
                SelectStr &= " ( " & FieldName & " Is NULL ) "
            End If
        End While

        While aTable.Select(SelectStr).Count > 0
            aTable.Select(SelectStr)(0).Delete()
        End While

        Return True
    End Function


    Function DBValueToString(ByVal aValue As Object, _
                         ByVal aDataType As DataTypeEnum, _
                         Optional ByVal aFormat As String = "") As String

        Select Case aDataType

            Case DataTypeEnum.Dat   'Date
                If aValue Is System.DBNull.Value Then
                    Return ""
                End If

                If aFormat.Length = 0 Then
                    aFormat = "MM/dd/yyyy"
                End If

                If IsDate(aValue) Then
                    Return CDate(aValue).ToString(aFormat)
                Else
                    Return aValue.ToString
                End If


            Case DataTypeEnum.Num   'Numeric
                If aValue Is System.DBNull.Value Then
                    Return ""
                End If

                If IsNumeric(aValue) Then
                    If aFormat.Length = 0 Then Return aValue.ToString
                    Return CType(aValue, Double).ToString(aFormat)
                Else
                    Return aValue.ToString
                End If

            Case Else   'String
                If aValue Is System.DBNull.Value Then
                    Return ""
                End If

                Return aValue.ToString.Trim

        End Select

    End Function

    Function DBValueToBoolean(ByVal aValue As Object, _
                              Optional ByVal aTrueValue As String = "") As Boolean

        If aValue Is DBNull.Value Then Return False

        If aTrueValue.Length > 0 Then
            Return aValue.ToString.Trim = aTrueValue.Trim
        End If

        If TypeOf aValue Is Boolean Then Return CType(aValue, Boolean)

        If IsNumeric(aValue) Then
            If CDec(aValue) = 1.0 Then Return True
        End If

        Return False

    End Function


    Function StringToDBValue(ByVal aValue As String, _
                       ByVal aDataType As DataTypeEnum) As Object

        Select Case aDataType

            Case DataTypeEnum.Dat   'Date
                If aValue = "" Then Return DBNull.Value

                If IsDate(aValue) Then
                    Return CDate(aValue)
                Else
                    Return "QSI Bad Value"
                End If

            Case DataTypeEnum.Num   'Number
                If aValue = "" Then Return DBNull.Value

                If IsNumeric(aValue) Then
                    Return CType(aValue, Decimal)
                Else
                    Return "QSI Bad Value"
                End If

            Case Else   'String
                If aValue = "" Then
                    Return DBNull.Value
                Else
                    Return aValue
                End If

        End Select
    End Function


    Sub StringToDBValue(ByVal aValue As String, _
                        ByVal aDataType As DataTypeEnum, _
                        ByRef aDBObj As Object)
        Select Case aDataType

            Case DataTypeEnum.Dat   'Date
                If aValue = "" Then
                    aDBObj = DBNull.Value
                    Return
                End If

                If IsDate(aValue) Then
                    aDBObj = CType(aValue, Date)  'CAN VB CONVERT A DATE TO DATETIME?
                Else
                    'No change to aDBObj
                End If

            Case DataTypeEnum.Num   'Number
                If aValue = "" Then
                    aDBObj = DBNull.Value
                    Return
                End If
                If IsNumeric(aValue) Then
                    aDBObj = CType(aValue, Decimal)
                Else
                    'No change to aDBObj
                End If

            Case Else   'String
                If aValue = "" Then
                    aDBObj = DBNull.Value
                    Return
                End If
                aDBObj = aValue.Trim

        End Select
    End Sub

    Function BooleanToDBValue(ByVal aValue As Boolean, _
                              Optional ByVal aTrueValue As String = "", _
                              Optional ByVal aFalseValue As String = "") As Object

        If aValue = Nothing Then Return DBNull.Value

        'Use True and False value if specified
        If aTrueValue.Length > 0 Then
            If aValue = True Then
                Return aTrueValue
            Else
                Return aFalseValue
            End If
        End If

        'Use 1 and 0 if TrueValue is not specified
        If aValue = True Then
            Return 1
        Else
            Return 0
        End If

    End Function


    ''' <summary> Put a standard string format on a date or numeric value, for use in comparisons </summary>
    Public Function StandardValue(ByVal aValue As String) As String
        aValue = aValue.Trim
        If IsDate(aValue) Then Return CDate(aValue).ToString("MM/dd/yyyy")
        If IsNumeric(aValue) Then Return CDec(aValue).ToString("N6")
        Return aValue
    End Function

    ''' <summary> Loop through the data adapter to look for rows that are duplicates. </summary>
    Public Function CheckForDuplicateRows(ByVal aDRControl As String, _
                                          ByVal aDA As DataTable, _
                                          ByVal aDAField As String, _
                                          Optional ByVal aDAField2 As String = "", _
                                          Optional ByVal aDAField3 As String = "", _
                                          Optional ByVal aDAField4 As String = "", _
                                          Optional ByVal aDAField5 As String = "", _
                                          Optional ByVal aDAField6 As String = "") As Control

        'This function loops through the data adapter to look for rows that are duplicates.  
        'Upon finding the first duplicate row, it will return the control that is lower in the list
        For i = 0 To aDA.Rows.Count - 1
            If RowIsDeleted(aDA(i)) Then Continue For
            For j = i + 1 To aDA.Rows.Count - 1
                If RowIsDeleted(aDA(j)) Then Continue For
                If aDA.Rows(i).Item(aDAField).ToString = Trim(aDA.Rows(j).Item(aDAField).ToString) Then
                    If aDAField2 = "" OrElse (aDA.Rows(i).Item(aDAField2).ToString = Trim(aDA.Rows(j).Item(aDAField2).ToString)) Then
                        If aDAField3 = "" OrElse (aDA.Rows(i).Item(aDAField3).ToString = Trim(aDA.Rows(j).Item(aDAField3).ToString)) Then
                            If aDAField4 = "" OrElse (aDA.Rows(i).Item(aDAField4).ToString = Trim(aDA.Rows(j).Item(aDAField4).ToString)) Then
                                If aDAField5 = "" OrElse (aDA.Rows(i).Item(aDAField5).ToString = Trim(aDA.Rows(j).Item(aDAField5).ToString)) Then
                                    If aDAField6 = "" OrElse (aDA.Rows(i).Item(aDAField6).ToString = Trim(aDA.Rows(j).Item(aDAField6).ToString)) Then
                                        'We have a duplicate -- position to row in qDR and return the control
                                        PositionDR(j)
                                        Return Me.CurrentItem.Controls(aDRControl)
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            Next
        Next


        'No Duplicates
        Return Nothing

    End Function

    ''' <summary> Positions Data Repeater (CurrentitemIndex) to the row that is bound a Data Adapter Row. Returns False if no match. </summary>
    Public Function PositionDR(ByRef aDataAdapterIndex As Integer) As Boolean
        For ThisItem As Integer = 0 To Me.ItemCount - 1
            Me.CurrentItemIndex = ThisItem
            If Me.GetTableRowNum(Me.CurrentItemIndex) = aDataAdapterIndex Then Return True
        Next

        Return False
    End Function

#End Region

#Region "Obsolete"

    '=== From ItemValueNeeded:
    'If qT._BindDef.Length > 0 Then

    '    e.Value = DBValueToString(e.Value, _
    '                              DataTypeEnum.Dat, _
    '                              qT._Format)
    '    If qT._DataType = DataTypeEnum.Dat Then
    '        If R.Item(qT._BindDef) Is DBNull.Value Then
    '            e.Value = ""
    '        Else
    '            e.Value = R.Item(qT._BindDef)
    '            If IsDate(e.Value) Then
    '                If qT._Format.Length > 0 Then
    '                    e.Value = CDate(e.Value).ToString(qT._Format)
    '                Else
    '                    e.Value = CDate(e.Value).ToString("MM/dd/yyyy")
    '                End If

    '            End If
    '        End If
    '    ElseIf qT._DataType = DataTypeEnum.Num Then
    '        If R.Item(qT._BindDef) Is DBNull.Value Then
    '            e.Value = ""
    '        Else
    '            e.Value = R.Item(qT._BindDef)
    '            If IsNumeric(e.Value) Then
    '                If qT._Format.Length > 0 Then
    '                    e.Value = CType(e.Value, Double).ToString(qT._Format)
    '                    'Otherwise, leave e.Value alone
    '                End If
    '            End If
    '        End If
    '    Else    'String
    '        If R.Item(qT._BindDef) Is DBNull.Value Then
    '            e.Value = ""
    '        Else
    '            e.Value = R.Item(qT._BindDef).ToString.Trim
    '        End If
    '    End If
    'End If

    'If qCB._BindDef.Length > 0 Then
    '    If qCB._BindDef.Length > 0 Then
    '        If R.Item(qCB._BindDef) Is DBNull.Value Then
    '            e.Value = ""
    '        Else
    '            e.Value = R.Item(qCB._BindDef).ToString.Trim
    '        End If
    '    End If
    '    If qCB.DropDownStyle = ComboBoxStyle.DropDownList Then
    '        qCB.SelectedValue = e.Value
    '    Else
    '        e.Control.Text = e.Value.ToString.Trim
    '    End If

    'End If

    '=== From ItemValuePushed

    'If qT._DataType = DataTypeEnum.Dat Then
    '    If e.Value.ToString = "" Then
    '        R.Item(qT._BindDef) = DBNull.Value
    '    Else
    '        If IsDate(e.Value) Then    'BHS 9/3/08
    '            R.Item(qT._BindDef) = CDate(e.Value)
    '        Else
    '            '    Last value is left if Validation error catches invalid date
    '        End If
    '    End If
    'ElseIf qT._DataType = DataTypeEnum.Num Then
    '    If e.Value.ToString = "" Then
    '        R.Item(qT._BindDef) = DBNull.Value
    '    Else
    '        If IsNumeric(e.Value) Then
    '            R.Item(qT._BindDef) = CType(e.Value, Decimal)
    '        Else
    '            'Do nothing, and expect _validatenumber to catch invalid number
    '        End If
    '    End If
    'Else
    '    R.Item(qT._BindDef) = e.Value
    'End If



    'If qCB._BindDef.Length > 0 Then
    '    If e.Value.ToString = "" Then
    '        R.Item(qCB._BindDef) = DBNull.Value
    '    Else
    '        R.Item(qCB._BindDef) = e.Value
    '    End If

    'End If

    '===10/30/09 In ItemValueNeeded, change For Each C as Control to just use e.Control:

    'For Each C As Control In ItemTemplate.Controls
    '    If C.Name = e.Control.Name Then

    '        'qTextBox
    '        Dim qT As qTextBox = TryCast(C, qTextBox)
    '        If qT IsNot Nothing Then
    '            If qT._BindDef.Length > 0 Then
    '                qT.Text = DBValueToString(R.Item(qT._BindDef), _
    '                                      qT._DataType, qT._Format)
    '                e.Value = qT.Text
    '            End If
    '            Return
    '        End If

    '        'qCB 
    '        Dim qCB As qComboBox = TryCast(e.Control, qComboBox)
    '        If qCB IsNot Nothing Then
    '            If qCB._BindDef.Length > 0 Then
    '                If qCB.DropDownStyle = ComboBoxStyle.DropDownList Then
    '                    'WE MAY WANT TO ADD _FORMAT TO qCB
    '                    If qCB.Items.Count = 0 Then
    '                        'Can't assign SelectedValue when Items.Count = 0.  We'll try again 
    '                        '  in the AfterDataSourceChanged method in this object
    '                    Else
    '                        qCB.SelectedValue = DBValueToString(R.Item(qCB._BindDef), _
    '                                                        qCB._DataType)
    '                    End If
    '                    e.Value = qCB.Text
    '                Else
    '                    qCB.Text = DBValueToString(R.Item(qCB._BindDef), _
    '                                               qCB._DataType)
    '                    e.Value = qCB.Text
    '                End If
    '            End If
    '                Return
    '            End If

    '            'qMaskedTextBox
    '            Dim qMT As qMaskedTextBox = TryCast(C, qMaskedTextBox)
    '            If qMT IsNot Nothing Then
    '                If qMT._BindDef.Length > 0 Then
    '                    qMT.Text = DBValueToString(R.Item(qMT._BindDef), _
    '                                               qMT._DataType)
    '                    e.Value = qMT.Text
    '                End If
    '                Return
    '            End If

    '            'qDateTimePicker()
    '            Dim qDP As qDateTimePicker = TryCast(C, qDateTimePicker)
    '            If qDP IsNot Nothing Then
    '                If qDP._BindDef.Length > 0 Then
    '                    qDP.Text = DBValueToString(R.Item(qDP._BindDef), _
    '                                               qDP._DataType)
    '                    e.Value = qDP.Text
    '                End If
    '                Return
    '            End If

    '            'qCheckBox
    '            Dim qXB As qCheckBox = TryCast(C, qCheckBox)
    '            If qXB IsNot Nothing Then
    '                If qXB._BindDef.Length > 0 Then
    '                    qXB.Checked = DBValueToBoolean(R.Item(qXB._BindDef), _
    '                                                    qXB._CheckedTrueValue)
    '                    e.Value = qXB.Checked.ToString
    '                End If
    '                Return
    '            End If
    '        End If
    'Next

    '=== Same thing for ItemValuePushed
    'For Each C As Control In ItemTemplate.Controls
    '    If C.Name = e.Control.Name Then
    '        Dim qT As qTextBox = TryCast(C, qTextBox)
    '        If qT IsNot Nothing Then
    '            If qT._BindDef.Length > 0 Then
    '                R.Item(qT._BindDef) = StringToDBValue(e.Value.ToString, qT._DataType)
    '            End If
    '            Return
    '        End If

    '        Dim qCB As qComboBox = TryCast(e.Control, qComboBox)
    '        If qCB IsNot Nothing Then
    '            If qCB._BindDef.Length > 0 Then
    '                If qCB.DropDownStyle = ComboBoxStyle.DropDownList Then
    '                    R.Item(qCB._BindDef) = StringToDBValue(qCB.SelectedValue.ToString, qCB._DataType)
    '                Else
    '                    R.Item(qCB._BindDef) = StringToDBValue(e.Value.ToString, qCB._DataType)
    '                End If
    '            End If
    '        End If

    '        'qMaskedTextBox
    '        Dim qMT As qMaskedTextBox = TryCast(C, qMaskedTextBox)
    '        If qMT IsNot Nothing Then
    '            If qMT._BindDef.Length > 0 Then
    '                R.Item(qMT._BindDef) = StringToDBValue(e.Value.ToString, qMT._DataType)
    '            End If
    '            Return
    '        End If

    '        'qDateTimePicker()
    '        Dim qDP As qDateTimePicker = TryCast(C, qDateTimePicker)
    '        If qDP IsNot Nothing Then
    '            If qDP._BindDef.Length > 0 Then
    '                R.Item(qDP._BindDef) = StringToDBValue(e.Value.ToString, qDP._DataType)
    '            End If
    '            Return
    '        End If

    '        'qCheckBox
    '        Dim qXB As qCheckBox = TryCast(C, qCheckBox)
    '        If qXB IsNot Nothing Then
    '            If qXB._BindDef.Length > 0 Then
    '                R.Item(qXB._BindDef) = BooleanToDBValue(CBool(e.Value), qXB._CheckedTrueValue, qXB._CheckedFalseValue)
    '            End If
    '        End If
    '        Return
    '    End If

    'Next

#End Region

End Class


