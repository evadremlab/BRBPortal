Imports System.Data
Imports System.Data.SqlClient
Imports System.Transactions
Imports System.Windows.Forms
Imports QSILib
'Imports Infragistics.Win.UltraWinGrid
Imports System.Data.Odbc
'Imports Microsoft.VisualBasic.PowerPacks
'Imports Microsoft.Win32

' =========================================== QSILib Windows Form Ancestor =======================================
Namespace Windows.Forms

    Public Class fBase
        Inherits System.Windows.Forms.Form

#Region "-------------------- Documentation ----------------------"
        'Original: BHS, May 2006

        'fBase defines instance varaibles and general procedures that are available
        'in all application forms.  All application forms derive from fBase:

        'fBase
        '   flMain
        '       <child list forms>
        '   feMain
        '       <child edit forms>

        'Instance Variables are prefixed with i

        'Child class methods are called by raising events and through 
        'overridable methods. (e.g. RaiseEvent OnSave, ShowStatus())

        'Child classes call fBase methods directly (e.g. NewRecord())

        'PowerBuilder Note:
        'Different from PB, methods for raised events should only occur in one child class in 
        'the class hierarchy.  To chain processing between several levels, use 
        'overridable methods.  An Overrides function can call its ancestor with
        'MyBase.<functionName>.

        'OnSetGVDefaultFields - Fires when a GV row is inserted.  Note, don't try to 
        '   set a key field in edit mode if it is a PK child and PK constraints are 
        '   defined for the dataset - this seems to destroy the GV row.

        'OnKeyFieldEntered - fires when a key field is entered in New mode.  If match,
        '   child sets iKey and returns true.  If no match but all key fields are entered,
        '   child sets key fields in GVs.

        'Typical Load Sequence of Events
        '  fe_Main.Load calls Setup Form, SetContext, and LoadForm
        '  fbase.LoadForm calls child_OnLoadForm to load data
        '  fbase.AfterLoadForm does several things:
        '    * insert blank rows in GVs
        '    * Set up validation handles for each entry control
        '    * Set up an Entry Field array for each entry field
        '    * Sup up a handler for btnRemoveRow
        '  fbase.SetFormAttributes sets control and button attributes based on New/Edit and Reader/Writer

        '7/7 BHS Add iDS.Clear and reset iEntryFields at beginning of New logic.
        '11/3/06 BHS Make Control Search in BuildQuery recursive
        '12/15/06 BHS Move FormSetup From LoadForm to SetContext (only runs once per form)
        '1/15/07 BHS _qGVEdit._BlankRowOnEmpty. GV data entry columns not sortable.
        'BHS 7/16/7 Only set grid read-only fields background color if not qGVList type
        '8/30/07 BHS GV::CellFormatting sets formatted empty dates to blank, avoiding 1/1/01 formatting.

        '6/20/08 BHS GBV Change permissions tracking, removing IsReader, changing IsWriter, Changing feMain.OnSetAttributes,
        '  creating IsCampusWriter to be overridden in client programs where appropriate.
        '6/23/08 BHS GBV Force iFName to Mid(objectname, 3).  No longer set in the client.
        '7/14/2008 GBV create iIsWriter property with different scope for Get and Set, so it can be read from
        '              everywhere, but set only from QBase.
        '8/8/8 BHS SetGVProperties forces DefaultCellStyle.SelectionBackColor to QdefaultRowBackColor if _ShowSelectionBar is False.
        '8/11/08 BHS iIsDirty is changed to a Property.  When iIsDirty is changed, SetDirtyIndicator allows a visual display (see feMain).
        '   To support dirty processing, Programmer declares WithEvents tables to be tracked.  After loading the table, it is added to 
        '   the iDirtyTables arraylist.  After OnLoad, the DirtyTableChanging event is assigned to iDirtyTables, to track any changes to the 
        '   tables being tracked.  If a change is detected, iIsDirty is set to True.  This replaces dependency on the EFields array.
        '8/11/08 BHS Remove all references to EF.  Use new iIsDirty logic above, and refer to a control's original values through the DataTable,
        '   Row and Column that is bound to the control.
        'ToDo Line 2662 LastEntryControl for a GridView
        '9/03/08 BHS Force user to deal with errors before closing (Windows FormClosing event)
        '9/30/09 BHS Add QueryAfterNewRecordSave overridable function so descendant can override default behavior of Query()
        'BHS 5/6/11 and 5/11/11 Add ControlIsDirty logic
        'BHS 6/28/12 Surround calls to descendant events triggered by user actions with
        '   a Try block and a standard ShowError
        
        'BHS 6/29/12 Pessimistic Locking

#End Region

#Region "-------------------- Functions to handle automatic scaling -GBV 9/23/2014 -----------"
        Public Sub New()
            ' This call is required by the Windows Form Designer.
            InitializeComponent()
            'BHS 1/26/17 removed to get to compile
            'AddHandler SystemEvents.UserPreferenceChanged, New UserPreferenceChangedEventHandler(AddressOf SystemEvents_UserPreferenceChangesEventHandler)
        End Sub
        'BHS 1/27/17 removed to get to compile
        'Private Sub SystemEvents_UserPreferenceChangesEventHandler(ByVal sender As Object, ByVal e As UserPreferenceChangedEventArgs)
        '    If (e.Category = UserPreferenceCategory.Window) Then
        '        Me.Font = SystemFonts.IconTitleFont
        '    End If
        'End Sub

#End Region

#Region "-------------------- Instance Variables and Event Declarations ----------------------"
        'Properties
        Protected Property iIsDirty() As Boolean
            Get
                Return iIsDirtyVar
            End Get
            Set(ByVal value As Boolean)
                If iIsDirtySuspended = False Then
                    iIsDirtyVar = value

                    'BHS 5/11/11 If setting iIsDirty False, also reset iOrigValue in current control
                    If value = False Then
                        'If iOrigValue IsNot Nothing AndAlso iOrigValue > "" Then  BHS 5/25/11 do this in all circumstances
                        If iCurrentControlName.Length > 0 Then
                            Dim C As Control = FindControl(iCurrentControlName, Me)
                            If C IsNot Nothing Then SetiOrigValue(C)
                            'End If
                        End If
                    End If

                    DirtyTimer.Start()
                    'Post("SetDirtyIndicator")   'Show yellow to left of New button if Dirty
                End If
            End Set
        End Property

        Public Property iIsWriter() As Boolean
            Get
                Return _iIsWriter
            End Get
            Friend Set(ByVal value As Boolean)
                _iIsWriter = value
            End Set
        End Property

        'Visual controls
        Protected iGVs As ArrayList = New ArrayList    'List of all GridViews to track events for
        Protected iUGs As ArrayList = New ArrayList 'List of UltraGrids to track events for
        Protected iAGV As DataGridView                 'Currently Active Gridview
        Protected iBSs As ArrayList = New ArrayList     'List of all Binding Sources
        Public iParams As ArrayList = New ArrayList      'Name/Value pairs
        Protected iDirtyTables As ArrayList = New ArrayList           'DataTables to capture change events on
        Protected iDS As DataSet = New DataSet
        Protected iDA As SqlDataAdapter                 'Default Data Adapter for SQL Edit environment
        Protected iDAIfx As OdbcDataAdapter             'Default Data Adapter for Ifx Edit Environment
        Protected iEP As ErrorProvider = New ErrorProvider

        'Instance Variables
        Public iShowProgrammerWarnings As Boolean = False   'Set this to true if you want to see warnings about dependencies you may not have set
        Public iMode As String = ""             'Edit, List, Report, Update
        Public iFName As String                 'Function Name for Online Help and Permissions identification 
        Public iKey As String = ""              'Key, delimited with |
        Public iIsNew As Boolean = False        'True/False isNew
        'Public iWasNew As Boolean = False       'True if user NewRecord was called (user is adding a new record)
        Private iIsDirtyVar As Boolean = False      'True/False isDirty
        Private iIsDirtySuspended As Boolean = False    'Set to True when changes are made that should not make the form dirty
        Private _iIsWriter As Boolean = False     'True/False isWriter
        Public iIsCampusWriter As Boolean = False 'True/False IsCampusWriter - GBV 6/21/2008
        Public iCampusWriterChecked As Boolean = False ' True/False IsCampusWriter function was run - GBV 7/15/2008
        Public iSaveThroughDS As Boolean = True 'True/False do we enforce DS constraints?
        Public iSQLCn As SqlConnection          'The connection for managing transactions 
        Public iODBCCn As OdbcConnection = Nothing        'The connection for managing ODBC transactions to Informix
        Public iSQLTran As SqlTransaction       'Transaction to manage multiple updates as a unit
        Public iODBCTran As OdbcTransaction     'Transaction to manage multiple Informix updates as a unit
        Public iFirstKeyField As String = ""    'Name of first key field
        Public iFirstNonKeyField As String = "" 'Name of first non-key field
        Public iKeyFields As ArrayList = New ArrayList  'List of Key field controls
        Private iAutoFirstKey As String = ""
        Private iAutoFirstNonKey As String = ""
        Private iAutoKeyFields As String = ""
        Public iBtnRemoveRow As Button = Nothing    'Universal remove row button
        Public iBtnInsertRow As Button = Nothing    'Universal insert row button
        Public iSQLDescr As String = ""         'Description of latest SQL (created in BuildQuery)
        Public iTestConstraints As Boolean = False
        Public iLastEF As Control               'Tracks where last legal focus was  BHS 8/11/08
        Public iShowHelp As Boolean = True      'Allows programmer to turn off showing help when working with many qGVTextBoxCols
        Public iElapsed As TimeSpan
        'References to List Objects (used in Edit Mode)
        Public iListForm As fBase               'Reference to calling list
        Public iListNavigator As BindingNavigator       'Reference to List BN
        Public iListGV As DataGridView          'Reference to active List GV
        ' Public iListUG As qUG                   'Reference to active List UG
        'Public iListDR As qDR                   'Reference to active DataRepeater
        Public iListSQL As String = ""          'SQL String to build the current list
        Public iNoList As Boolean = False       'True to suppress List display
        Public iConnType As String = ""         'Used in onSetContext to override Appl.ConnType.  This is better than changing Appl.ConnType,
        '                                        which could affect other open forms.  BHS 7/10/08
        Public iOKToClose As Boolean = True     'Set to False to prevent form from closing  BHS 9/4/08
        Protected iIsLoading As Boolean = False   'Set to True while form is loading
        Public iNeedToUpdateLiveDatabase As Boolean = False 'Set to True for utilities designed to update Live Database, even from test apps
        Public iOrigValue As String = ""        'Control Enter sets this value for comparing in OnValidateControl
        Public iCurrentControlName As String = ""   'BHS 10/12/10 - Name of data entry control last entered
        'See feCPIReview.vb as an example
        Protected iAutoClear As Boolean = False 'Descendant sets to true if clear logic has been moved to OnClear event
        Public iRememberResize As Boolean = True    'Allow descendant to turn off default rememer resize logic
        Public iCheckEveryKeyField As Boolean = False 'GBV 4/19/11 - If True, validation checks of each key field
        'BHS 9/28/11 Changed from False to True, to avoid concurrency message for non-key fields showing up.
        Public iAcceptConcurrency As Boolean = True 'GBV 8/12/11 - If True, Concurrency message will not popup
        'BHS 6/29/12
        ''Public iRecordLock As New RecordLock()
        'GBV 8/29/2012
        Public iGenericObject As Object = Nothing ' Used to pass things like data tables, data views, etc. to the child.

        ' GBV 6/12/2015 - Ticket 1245
        Public iEditForm As fBase = Nothing ' To handle retrieval of a single record after saving new record

        'SDC 6/16/2015 - Ticket 1518
        Public iRefreshControlText As Boolean = True    'To all user to reset control's text

        'GBV 8/10/2015 - Ticket 2439
        Public iIsClosed As Boolean = False ' Set to True when form is closed.

        ' GBV 8/10/2015 - Ticket 2439
        Public FormID As String = Guid.NewGuid().ToString() ' Need a unique identifier for the form

        'BHSCONV 4/30/12
        Public WithEvents iSQLDA1 As SqlDataAdapter
        Public WithEvents iSQLDA2 As SqlDataAdapter
        Public WithEvents iSQLDA3 As SqlDataAdapter
        Public WithEvents iSQLDA4 As SqlDataAdapter
        Public WithEvents iSQLDA5 As SqlDataAdapter
        Public WithEvents iSQLDA6 As SqlDataAdapter
        Public WithEvents iSQLDA7 As SqlDataAdapter
        Public WithEvents iSQLDA8 As SqlDataAdapter
        Public WithEvents iSQLDA9 As SqlDataAdapter
        Public iSQLDATableNames As New ArrayList

        Protected Event OnSQLDARowUpdated(ByVal sender As Object, ByVal e As System.Data.SqlClient.SqlRowUpdatedEventArgs)

        'Setup Events
        Protected Event OnSetContext()         'Set info from derived class
        Protected Event OnIsWriter(ByRef aOnIsWriterResult As Boolean)            ' Allow programmer to override general iswriter logic
        Protected Event OnIsCampusWriter()      ' Allow programmer to override general isCampusWriter Logic
        Protected Event OnIsReader()            ' Allow programmer to override general isreader logic
        Protected Event OnSetStartFocus()       ' Override default start focus behavior (First Key on New, First
        '                                         Non Key on Edit)
        Protected Event OnAfterSetContext(ByRef aOK As Boolean)    'Set info second pass
        Protected Event OnLoadForm()
        Protected Event OnSetFormAttributes()  'Check auth, protect fields
        Protected Event OnAfterLoadForm()      'After data loaded processing
        Protected Event OnEndOfLoad()           'Very last thing before load is done
        Protected Event OnSetMenu()            'Called any time available commands change
        Protected Event OnMenuClick(ByVal aItemName As String, ByVal aTag As String)
        Protected Event OnBackgroundQueryComplete(ByVal aDV As DataView)
        Protected Event OnBackgroundQueryCancelled()
        Protected Event OnPassData(ByVal aOb As Object)
        Protected Event OnPassParameter(ByVal aSource As String, ByVal aParameter As String)

        'List Events
        Protected Event OnQuery()              'Select list based on QBE
        'Protected Event OnGetKey(ByRef aGV As DataGridView, ByVal aRowNum As Integer)             'Set iKey based on list row selected  Removed BHS 11/30/06

        'Edit Events
        Protected Event OnNewRecord(ByRef aOK As Boolean)      'New button clicked
        Protected Event OnCheckDirty(ByRef aActedOn As Boolean)                         'Is Edit Form dirty?
        Protected Event OnKeyFieldEntered(ByRef aMatch As Boolean)  'Returns True if iKey matches DB record

        Protected Event OnOKToDeletePrompt(ByRef aPrompt As String)    'OK to delete Msg
        Protected Event OnDeleteForm(ByRef aOK As Boolean)     'Delete button clicked

        Protected Event OnSaveClicked(ByRef aOK As Boolean)    'Save button clicked
        Protected Event OnSaveForm(ByRef aOK As Boolean)       'Save procedure
        Protected Event OnAfterSave()                           'Allow descendant to do post-save logic
        Protected Event OnAfterDelete()                           'Allow descendent to process after record deleted (used for applying filter)

        Protected Event OnInsertRow(ByRef aGV As DataGridView, ByRef aActedOn As Boolean) 'Insert Row in aGV
        Protected Event OnSetGVDefaultFields(ByRef aGV As DataGridView, ByRef aR As DataGridViewRow)
        Protected Event OnRemoveGVRow(ByRef aGV As DataGridView, ByVal aRow As DataGridViewRow, ByRef aActedOn As Boolean) 'Remove Row in aGV
        Protected Event OnRemoveGVRowPrompt(ByRef aGV As DataGridView, ByVal aRow As DataGridViewRow, ByRef aPrompt As String) 'Remove Row Prompt
        Protected Event OnValidateControl(ByVal aColName As String, ByRef aValue As String, ByVal aRow As Integer, ByRef aErrorText As String)          'Validate Control
        Protected Event OnValidateForm(ByRef aSender As Object, ByRef aErrorText As String)
        'Post Event
        Protected Event OnPost(ByVal aObject As Object)
        ''' <summary> Allow programmer to override default permissions behavior for turning off
        ''' command buttons.  If you use this, be sure to set aActedOn to True to avoid ancestor processing </summary>
        Protected Event OnCmdButtonsVisible(ByRef aActedOn As Boolean)  'BHS 6/27/08
        'BHS 2/16/10
        Protected Event DRDDOnDropDown(ByRef aDD As qDD, ByRef aOK As Boolean) 'Pass DR DD control event to form descendant
        Protected Event DRDDOnDropDownClosed(ByRef aDD As qDD) 'Pass DR DD control event to form descendant
        Protected Event DRDDSelectedIndexChanged(ByRef aDD As qDD, ByRef aOK As Boolean) 'Pass DR DD control event to form descendant
        Protected Event DRDDOnDoubleClick(ByRef aDD As qDD)   'Pass DR DD double-click event to form descendant
        Protected Event OnTab(ByRef msg As System.Windows.Forms.Message, _
                              ByVal keyData As System.Windows.Forms.Keys, _
                              ByRef aCancelCmd As Boolean)

        Public WithEvents DirtyTimer As New Timer

#End Region

#Region "-------------------- Form Setup -------------------------------"

        'BHS 4/21/09
        '''<summary> Allow a calling program to pass a parameter string to any child </summary>
        Sub PassParameter(ByVal aSource As String, ByVal aParameter As String)
            RaiseEvent OnPassParameter(aSource, aParameter)
        End Sub

        '''<summary> Prepare Form for initial display </summary>
        Function SetContext() As Boolean
            Dim ProgrammerError As String = ""
            Dim OK As Boolean = True
            'BHS 6/22/10
            iFName = Mid(Me.Name, 3)


            iIsLoading = True   'BHS 10/27/08

            Try
                RaiseEvent OnSetContext()   'Load values for this function
            Catch ex As Exception
                ShowError("Unexpected error loading form (OnSetContext)", ex)
            End Try

            SetGVProperties(iGVs, iMode)

            If Not AfterSetContext() Then Return False

            FormSetup() 'BHS 12/15/06 do this once (before loading data) to avoid duplicate handles

            'BHS 6/20/08 Removed isReader check - the menu prevents users from reading what they're not allowed to see

            Return True

        End Function

        '''<summary> Called at the end of SetContext </summary>
        Function AfterSetContext() As Boolean  'Confirms dependencies required at this level
            Dim ProgrammerError As String = ""
            Dim ProgrammerWarn As String = ""

            Dim OK As Boolean = True

            'Raise child event for special handling
            Try
                RaiseEvent OnAfterSetContext(OK)    'Confirms dependencies required at child level
            Catch ex As Exception
                ShowError("Unexpected error loading form (AfterSetContext)", ex)
            End Try

            If OK = False Then Return False

            'If child has no problems, do generic dependency checking
            If iMode.ToLower = "edit" Then
                'Allow invalid field values until save time
                'If System.ComponentModel.DesignerProperties.GetIsInDesignMode(Me) = False Then iDS.EnforceConstraints = False

                'BHS 9/11/08 If fBase.ActiveForm IsNot Nothing Then iDS.EnforceConstraints = False
                If Not InDevEnv() Then iDS.EnforceConstraints = False

                'Edit form required information
                'If Not iFName > " " Then ProgrammerError = "Missing FName" BHS 6/23/08

                If Not iFirstKeyField > " " Then ProgrammerWarn = "Missing iFirstKeyField"

                'BHS 8/15/08  If not specified, focus control doesn't happen
                If Not iFirstNonKeyField > " " Then ProgrammerWarn = "Missing iFirstNonKeyField"

                'BHS 8/15/08 If not specified, no KeyField logic for testing if a New entry matches an existing one.
                If iKeyFields.Count < 1 Then ProgrammerWarn = "Missing iKeyFields"

                'BHS 8/15/08 If not specified then we assume we're adding a new record.  iKey is initialized to ""
                'If iKey Is Nothing Then ProgrammerError = "iKey not set"

                'Edit forms that derive from lists need to be able to reference the list.  If any list references are set, all must be set.
                If iListForm IsNot Nothing Or iListNavigator IsNot Nothing Or iListGV IsNot Nothing Then
                    If iListForm Is Nothing Then ProgrammerError = "iListForm missing"
                    'BHS 1/30/08 If iListNavigator Is Nothing Then ProgrammerError = "iListNavigator missing"
                    If iListGV Is Nothing Then ProgrammerError = "iListGV, iListUG, or iListDR should be specified in OnSetContext"
                End If

                'Edit form required data objects
                If iBSs.Count < 1 Then ProgrammerWarn = "iBS missing"
                If iDS Is Nothing Then ProgrammerWarn = "iDS missing"

                'If these are missing at run time, show error
                If Not InDevEnv() Then
                    If ProgrammerError > " " Then
                        ProgrammerErr(ProgrammerError)
                        Return False
                    End If
                    If iShowProgrammerWarnings = True Then
                        If ProgrammerWarn > " " Then
                            ProgrammerErr(ProgrammerWarn)
                            Return False
                        End If
                    End If
                End If
            End If

            If iMode.ToLower = "list" Then
                'If Not iFName > " " Then ProgrammerError = "Missing FName" BHS 6/23/08
                If iListNavigator Is Nothing Then ProgrammerError = "iListNavigator missing"
                If iListGV Is Nothing Then
                    'If iListUG Is Nothing And iListDR Is Nothing Then   'BHS 12/2/08
                    ProgrammerError = "iListGV, iListUG or iListDR should be specified in OnSetContext"
                    'End If
                End If

                If iBSs.Count < 1 Then ProgrammerError = "iBS missing"
                If Not iListNavigator Is Nothing Then
                    If iListNavigator.BindingSource Is Nothing Then ProgrammerError = "iListNavigator.BindingSource missing"
                End If

                'Only check at runtime, not at design time
                'If Not Windows.Forms.fBase.ActiveForm Is Nothing Then
                If Not InDevEnv() Then
                    If ProgrammerError > " " Then
                        MsgBoxErr("Programmer Error", ProgrammerError)
                        Return False
                    End If
                End If
            End If

            Return True
        End Function

        'TODO SetGVProperties is also set up in qFunctions
        '''<summary> Set properties for a list of GVs </summary>
        Sub SetGVProperties(ByRef aGVs As ArrayList, ByVal aType As String)
            Dim GV As DataGridView
            'Dim Col As DataGridViewColumn

            'Set Universal attributes first, then specific ones for list or edit
            For Each GV In aGVs
                If TypeOf (GV) Is qGVBase Then
                    Dim qGV As qGVBase = CType(GV, qGVBase)
                End If
            Next

            If aType.ToLower = "list" Then
                'Let qGVList attributes stand - otherwise set them here
                For Each GV In aGVs
                    SetListGVProperties(GV)
                    'If TypeOf (GV) Is qGVBase Then
                    '    GV.RowTemplate.DefaultCellStyle.BackColor = Nothing
                    '    GV.RowTemplate.DefaultCellStyle.ForeColor = Nothing
                    '    GV.BackgroundColor = QListBackColor
                    '    If CType(GV, qGVBase)._ShowSelectionBar = True Then
                    '        GV.RowTemplate.DefaultCellStyle.SelectionBackColor = QSelectionBackColor
                    '        GV.RowTemplate.DefaultCellStyle.SelectionForeColor = QSelectionForeColor
                    '    Else
                    '        GV.RowTemplate.DefaultCellStyle.SelectionBackColor = Nothing
                    '        GV.RowTemplate.DefaultCellStyle.SelectionForeColor = QForeColor
                    '        GV.DefaultCellStyle.SelectionBackColor = QDefaultRowBackColor  'BHS 8/8/8   Replace dark blue
                    '    End If

                    '    GV.BackgroundColor = QListBackColor
                    '    GV.ColumnHeadersDefaultCellStyle.BackColor = QColHeaderBackColor
                    '    GV.RowsDefaultCellStyle.BackColor = QDefaultRowBackColor
                    '    GV.RowsDefaultCellStyle.ForeColor = QForeColor
                    '    GV.AlternatingRowsDefaultCellStyle.BackColor = QAltRowBackColor
                    '    GV.AlternatingRowsDefaultCellStyle.ForeColor = QForeColor
                    '    Return
                    'End If

                    'If Not TypeOf (GV) Is qGVList Then
                    '    GV.AllowUserToAddRows = False
                    '    GV.AllowUserToDeleteRows = False
                    '    GV.AllowUserToOrderColumns = True
                    '    GV.RowTemplate.DefaultCellStyle.BackColor = QListBackColor
                    '    GV.RowTemplate.DefaultCellStyle.ForeColor = Color.Black
                    '    GV.ColumnHeadersDefaultCellStyle.BackColor = QColHeaderBackColor
                    '    GV.BackgroundColor = QListBackColor
                    '    GV.RowTemplate.DefaultCellStyle.SelectionBackColor = QSelectionBackColor
                    '    GV.RowTemplate.DefaultCellStyle.SelectionForeColor = QSelectionForeColor
                    '    GV.MultiSelect = False
                    '    GV.RowHeadersVisible = False
                    '    GV.SelectionMode = DataGridViewSelectionMode.FullRowSelect
                    '    GV.ColumnHeadersDefaultCellStyle.BackColor = QColHeaderBackColor

                    'End If
                Next
            End If

            If aType.ToLower = "edit" Then

                For Each GV In aGVs
                    SetEditGVProperties(GV)
                    'If TypeOf (GV) Is qGVList Then
                    '    GV.RowTemplate.DefaultCellStyle.BackColor = Nothing
                    '    GV.RowTemplate.DefaultCellStyle.ForeColor = Nothing
                    '    GV.BackgroundColor = QListBackColor
                    '    If CType(GV, qGVBase)._ShowSelectionBar = True Then
                    '        GV.RowTemplate.DefaultCellStyle.SelectionBackColor = QSelectionBackColor
                    '        GV.RowTemplate.DefaultCellStyle.SelectionForeColor = QSelectionForeColor
                    '    Else
                    '        GV.RowTemplate.DefaultCellStyle.SelectionBackColor = Nothing
                    '        GV.RowsDefaultCellStyle.SelectionBackColor = QDefaultRowBackColor
                    '        GV.AlternatingRowsDefaultCellStyle.SelectionBackColor = QAltRowBackColor
                    '        GV.RowTemplate.DefaultCellStyle.SelectionForeColor = QForeColor
                    '    End If

                    '    GV.BackgroundColor = QListBackColor
                    '    GV.ColumnHeadersDefaultCellStyle.BackColor = QColHeaderBackColor
                    '    GV.RowsDefaultCellStyle.BackColor = QDefaultRowBackColor
                    '    GV.RowsDefaultCellStyle.ForeColor = QForeColor
                    '    GV.AlternatingRowsDefaultCellStyle.BackColor = QAltRowBackColor
                    '    GV.AlternatingRowsDefaultCellStyle.ForeColor = QForeColor

                    'Else
                    '    If GV.AllowUserToAddRows = True Then
                    '        MsgBoxErr("Programmer Error", "GV.AllowUserToAddRows must be False in Designer")
                    '    End If
                    '    GV.AllowUserToAddRows = False
                    '    GV.AllowUserToDeleteRows = False
                    '    GV.AllowUserToOrderColumns = True
                    '    GV.RowTemplate.DefaultCellStyle.BackColor = QListBackColor 'Color.White
                    '    GV.RowTemplate.DefaultCellStyle.ForeColor = QForeColor      'Color.Black
                    '    GV.ColumnHeadersDefaultCellStyle.BackColor = QBackColor
                    '    'System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
                    '    GV.BackgroundColor = QBackColor
                    '    'System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))
                    '    GV.RowTemplate.DefaultCellStyle.SelectionBackColor = QSelectionBackColor 'Color.AliceBlue
                    '    GV.RowTemplate.DefaultCellStyle.SelectionForeColor = QSelectionForeColor 'Color.Black
                    '    GV.MultiSelect = False
                    '    'GV.ReadOnly = Nothing    'Set individually
                    '    GV.RowHeadersVisible = False
                    '    GV.SelectionMode = DataGridViewSelectionMode.CellSelect
                    '    GV.EditMode = DataGridViewEditMode.EditOnEnter
                    '    GV.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
                    '    'GV.ReadOnly = False
                    '    For Each Col In GV.Columns
                    '        If Col.ReadOnly = True Then
                    '            Col.CellTemplate.Style.BackColor = QListBackColor
                    '        Else
                    '            Col.CellTemplate.Style.BackColor = QEntryBackColor
                    '        End If
                    '        Col.SortMode = DataGridViewColumnSortMode.NotSortable
                    '    Next

                    'End If

                Next
            End If

        End Sub

        '''<summary> Assign tooltips based on control properties </summary>
        Sub FormSetup()
            Dim C As Control

            Dim toolTip1 As New ToolTip()
            SetToolTipProperties(toolTip1)

            For Each C In Me.Controls
                AssignToolTips(toolTip1, C)    'Call recursive routine
            Next

            SetColorScheme()    'Default fore and backcolors

            SetHandles()

        End Sub

        '''<summary> Set control colors, so they can be controlled from the App object </summary>
        Function SetColorScheme() As Boolean
            Me.BackColor = QBackColor
            Me.ForeColor = QForeColor
            Return True
        End Function

        '''<summary> Define global events for entry controls and buttons </summary>
        Sub SetHandles()
            Dim C As Control
            Dim GV As DataGridView

            For Each C In Me.Controls
                HandleControl(C)    'Recursive
            Next

            For Each GV In iGVs 'All GridViews
                AddHandler GV.CellValidating, AddressOf gvCellValidating
                AddHandler GV.CellEndEdit, AddressOf gvCellEndEdit
                AddHandler GV.DataError, AddressOf gvDataError
                'AddHandler GV.GotFocus, AddressOf gvGotFocus
                AddHandler GV.CellEnter, AddressOf gvcellenter
                AddHandler GV.CellFormatting, AddressOf gvCellFormatting
            Next


        End Sub

        '''<summary> Attaches custom events to all appropriate controls in the form </summary>
        Sub HandleControl(ByVal aC As Control)
            'Set global entry control events
            If TypeOf (aC) Is TextBox Or TypeOf (aC) Is ComboBox Or TypeOf (aC) Is qTextBox Or TypeOf (aC) Is qComboBox Or TypeOf (aC) Is qMaskedTextBox Or TypeOf (aC) Is qCheckBox Or TypeOf (aC) Is qDateTimePicker Or TypeOf (aC) Is qRC Then
                'BHS 2/9/10 From Oakland - don't handle qTextbox in qDD
                If Not TypeOf (aC.Parent) Is QSILib.qDD Then
                    AddHandler aC.Validating, AddressOf EntryControl_Validating
                    AddHandler aC.Validated, AddressOf EntryControl_Validated
                    AddHandler aC.Enter, AddressOf EntryControl_Enter
                    AddHandler aC.Leave, AddressOf EntryControl_Leave
                End If
            End If

            'BHS 2/9/10 From Oakland
            Dim DD As QSILib.qDD = TryCast(aC, QSILib.qDD)
            If DD IsNot Nothing Then
                AddHandler DD.CompositeValidating, AddressOf EntryControl_Validating    'Avoid responding to txtCode validating - only respond when leaving the whole composite control
                'AddHandler aC.Validated, AddressOf EntryControl_Validated  
                AddHandler DD.CompositeValidated, AddressOf EntryControl_Validated  'BHS 3/5/10
                AddHandler aC.Enter, AddressOf EntryControl_Enter
                AddHandler aC.Leave, AddressOf EntryControl_Leave
            End If

            'Set global button events
            If aC.GetType.Name.ToLower = "qbtninsertgvrow" Or aC.GetType.Name.ToLower = "qbtnremovegvrow" Then
                Dim B As Button = CType(aC, Button)
                AddHandler B.Click, AddressOf gvButton_Click
            End If

            'Recursive
            Dim C As Control
            For Each C In aC.Controls
                HandleControl(C)
            Next

        End Sub

        ''' <summary> DirtyTable Column had something typed into it </summary>
        Private Sub DirtyTableChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

            '10/25/12 - SRM - Added this line to avoid accessing deleted lines which happends on rare occasiojns for some reason.
            If e.Row.RowState = DataRowState.Deleted Then Return

            Dim a As String
            Dim b As String

            If IsDBNull(e.Row.Item(e.Column)) Then
                a = Nothing
            Else
                a = e.Row.Item(e.Column).ToString.Trim
            End If

            If e.ProposedValue Is Nothing Then
                b = Nothing
            Else
                b = e.ProposedValue.ToString.Trim
            End If

            If a Is Nothing And (b IsNot Nothing AndAlso b <> "") Then iIsDirty = True
            If b Is Nothing And (a IsNot Nothing AndAlso a <> "") Then iIsDirty = True

            Dim t As String = e.Column.DataType.ToString

            If t = "System.DateTime" Then
                If DateCompare(a, b) = False Then iIsDirty = True
                Return
            End If

            If t = "System.Decimal" Or t = "System.int16" Or t = "System.int32" Or t = "system.single" Or t = "system.double" Then
                If NumberCompare(a, b) = False Then iIsDirty = True
                Return
            End If

            'Case-sensitive text comparison even though Option Compare is Text
            If StrComp(a, b, CompareMethod.Binary) <> 0 Then iIsDirty = True

        End Sub

        'Note tooltips for GVs are assigned at the columnheader level in design mode
        '''<summary> Assign tool tips to all controls in a control (recursive) </summary>
        Sub AssignToolTips(ByVal aToolTip As ToolTip, ByVal aC As Control)
            Dim C2 As Control
            Dim Str As String

            'Take from Tag if available
            If Not IsNothing(aC.Tag) Then
                If TypeOf (aC.Tag) Is String Then
                    Str = aC.Tag.ToString
                    If Mid(Str, 1, 4).ToLower = "tip:" Then
                        aToolTip.SetToolTip(aC, Mid(Str, 5))
                    End If
                End If
            End If

            'Take from _ToolTip if available
            If TypeOf (aC) Is qTextBox Then
                If CType(aC, qTextBox)._ToolTip > "" Then
                    aToolTip.SetToolTip(aC, CType(aC, qTextBox)._ToolTip)
                End If
            End If

            If TypeOf (aC) Is qMaskedTextBox Then
                If CType(aC, qMaskedTextBox)._ToolTip > "" Then
                    aToolTip.SetToolTip(aC, CType(aC, qMaskedTextBox)._ToolTip)
                End If
            End If


            'BHS 2/9/10 From Oakland
            If TypeOf (aC) Is QSILib.qDD Then
                If CType(aC, QSILib.qDD)._ToolTip > "" Then
                    'BHS 1/12/12 R5.1.12 Assign tooltip to qDD textbox
                    'Apparently, tooltip feature doesn't work in a composite control, so we assigned it to the qDD textbox
                    Dim DD As qDD = TryCast(aC, qDD)
                    If DD IsNot Nothing Then
                        Dim DDTextBox As TextBox = DD.txtCode
                        If DDTextBox IsNot Nothing Then
                            aToolTip.SetToolTip(DDTextBox, CType(aC, QSILib.qDD)._ToolTip)
                        End If
                    End If
                    'aToolTip.SetToolTip(aC, CType(aC, QSILib.qDD)._ToolTip)
                    'END BHS 1/12/12 change
                End If
            End If

            If TypeOf (aC) Is qComboBox Then
                If CType(aC, qComboBox)._ToolTip > "" Then
                    aToolTip.SetToolTip(aC, CType(aC, qComboBox)._ToolTip)
                End If
            End If

            If TypeOf (aC) Is qCheckBox Then
                If CType(aC, qCheckBox)._ToolTip > "" Then
                    aToolTip.SetToolTip(aC, CType(aC, qCheckBox)._ToolTip)
                End If
            End If

            If TypeOf (aC) Is qRC Then
                If CType(aC, qRC)._ToolTip > "" Then
                    aToolTip.SetToolTip(aC, CType(aC, qRC)._ToolTip)
                End If
            End If

            If TypeOf (aC) Is qDateTimePicker Then
                If CType(aC, qDateTimePicker)._ToolTip > "" Then
                    aToolTip.SetToolTip(aC, CType(aC, qDateTimePicker)._ToolTip)
                End If
            End If

            For Each C2 In aC.Controls   'Call this routine recursively
                AssignToolTips(aToolTip, C2)
            Next

        End Sub

        Function InDevEnv() As Boolean
            Return Process.GetCurrentProcess().ProcessName.ToUpper().Equals("DEVENV")
        End Function

        ''' <summary> Send data to descendant for it to use </summary>
        Sub PassData(ByVal aOb As Object)
            RaiseEvent OnPassData(aOb)
        End Sub

#End Region

#Region "-------------------- Load Form -------------------------------"

        '''<summary> Load Form </summary>
        Function LoadForm() As Boolean
            'Initialize values   BHS 7/14/08
            iIsLoading = True   'BHS 10/27/08
            iDS.Clear()
            iIsNew = False
            iIsDirty = False
            iOKToClose = True
            ClearErrors(Me) 'BHS 5/11/11 added (Me) to get recursive clearing
            ShowStatus("")  'BHS 5/11/11
            iIsClosed = False  'GBV 8/10/2015 - Ticket 2439

            'Default connection  (BHS 1/29/07)
            'If fBase.ActiveForm IsNot Nothing Then
            If Not InDevEnv() Then
                If Not Appl.ConnType = "IFX" Then
                    iSQLCn = CreateConnection()
                End If
            End If

            Try
                'BHS 6/29/12 Initialize iRecordLock
                ''  iRecordLock.Clear()

                RaiseEvent OnLoadForm() 'Fill form with data based on logic in Child
            Catch ex As Exception
                ShowError("Unexpected error loading form (OnLoadForm)", ex)
            End Try


            'BHS 7/14/08   Call NewRecord at end of OnLoadForm, if programmer doesn't
            'BHS 7/15/08 Require programmer to make NewRecord call in OnLoad
            'If iMode = "edit" And iKey IsNot Nothing AndAlso iKey = "" Then
            '    If iIsNew = False Then NewRecord()
            'End If

            SetFormAttributes()     'Set protected/unprotected attributes

            Dim UO As New UserObjects()
            Dim UserObjectCount As Integer = UO.ucount
            If Mid(gUserName, 1, 3) = "qsi" Then ShowStatus(UserObjectCount.ToString)

            Try
                RaiseEvent OnAfterLoadForm() 'Add blank lines to detail areas, etc.
            Catch ex As Exception
                ShowError("Unexpected error loading form (OnAfterLoadForm)", ex)
            End Try

            'GBV 7/24/2014 - set menu only if not from cron
            If Not gIsFromCron Then SetMenu() 'Set Menu based on button attributes

            'Set Dirty Handling
            SetDirtyHandling() 'GBV 10/28/2008
            'For Each DT As DataTable In iDirtyTables   GBV 10/28/2008
            '    SetDirtyIndicator(iIsDirty())   'Show only if we are tracking dirty tables
            '    AddHandler DT.ColumnChanging, AddressOf DirtyTableChanging
            'Next

            'BHS 8/18/10    Set window size to the one last used

            ' GBV - 9/15/2014 - Do this only for normal win dows
            If Me.WindowState = FormWindowState.Normal Then
                SetWindowSize(Me.Name, Me.Size)
                ' GBV 8/18/2014 - never larger than the MDI container
                If Me.MdiParent IsNot Nothing Then
                    If Me.Height > Me.MdiParent.Height - 130 Then
                        Me.Height = Me.MdiParent.Height - 130
                    End If
                    If Me.Width > Me.MdiParent.Width - 50 Then
                        Me.Width = Me.MdiParent.Width - 50
                    End If
                End If
            End If


            'Future project to automatically load maxlength values in string textbox fields
            Post("SetStartFocus")
            iIsLoading = False  'BHS 10/27/08
            Return True
        End Function

        'BHS 5/11/11 Removed in favor of ClearErrors(Me)
        '''' <summary> Remove all error displays on the form </summary>
        'Sub ClearErrors()   'BHS 12/5/08
        '    For Each C As Control In Me.Controls
        '        iEP.SetError(C, "")
        '    Next
        'End Sub

        Protected Sub SetDirtyHandling()
            For Each DT As DataTable In iDirtyTables
                SetDirtyIndicator(iIsDirty())   'Show only if we are tracking dirty tables
                AddHandler DT.ColumnChanging, AddressOf DirtyTableChanging
            Next
        End Sub


        '''<summary> Set menu attributes based on visiblity of buttons </summary>
        Function SetMenu() As Boolean

            ' GBV 7/24/2014 - setmenu only if not from cron
            If gIsFromCron Then Return True

            Dim TSI As ToolStripItem
            Dim MI As ToolStripMenuItem
            Dim MISub As ToolStripMenuItem
            Dim Tag As String = ""
            Dim qMI As qMenuItem
            Dim isActive As Boolean = False

            'Disable each menu item with a tag, unless it matches a visible/enabled button
            'If fBase.ActiveForm IsNot Nothing Then
            If Not InDevEnv() Then
                For Each MI In Appl.gMenu.Items
                    For Each TSI In MI.DropDownItems
                        'Assign based on tag first
                        If TypeOf TSI Is ToolStripMenuItem Then
                            MISub = CType(TSI, ToolStripMenuItem)
                            If Not IsNothing(MISub.Tag) AndAlso MISub.Tag.ToString > " " Then
                                isActive = CheckForVisibleButton(MISub.Tag.ToString)
                                MISub.Visible = isActive
                                MISub.Enabled = isActive
                            End If
                        End If
                        'Assign based on _ButtonName if available
                        If TypeOf TSI Is qMenuItem Then
                            qMI = CType(TSI, qMenuItem)
                            If Not IsNothing(qMI._ButtonName) AndAlso qMI._ButtonName > " " Then
                                isActive = CheckForVisibleButton(qMI._ButtonName)
                                qMI.Visible = isActive
                                qMI.Enabled = isActive
                            End If
                        End If
                    Next
                Next

            End If

            'Call Child routine for additional menu settings
            Try
                RaiseEvent OnSetMenu()
            Catch ex As Exception
                ShowError("Unexpected error setting up menu (OnSetMenu)", ex)
            End Try

        End Function

        '''<summary> Is this button visible on the form? </summary>
        Function CheckForVisibleButton(ByVal aButtonName As String) As Boolean
            Dim B As Button
            Dim TSI As ToolStripItem

            For Each C As Control In Me.Controls
                If TypeOf C Is Button Then
                    B = CType(C, Button)
                    'BHS 7/15/10 Change Return True to Return B.visible
                    If aButtonName.ToLower = B.Name.ToString.ToLower Then Return B.Visible
                End If
            Next

            If Not IsNothing(iListNavigator) Then
                For Each TSI In iListNavigator.Items
                    If TypeOf TSI Is ToolStripButton Then
                        If aButtonName.ToLower = TSI.Name.ToString.ToLower Then Return True
                    End If
                Next
            End If

            Return False

        End Function

        '''<summary> Set focus based on whether we're in New or Edit mode.  Let programmer override focus with OnSetStartFocus,
        ''' and finally call OnEndOfLoad </summary>
        Sub SetStartFocus()
            'BHS 2/9/09  Refresh Tab Count, if appropriate
            For Each C As Control In Me.Controls
                Dim T As qTab = TryCast(C, qTab)
                If T IsNot Nothing Then T.RefreshPages()
            Next

            If iMode.ToLower = "edit" Then
                If iIsNew = True Then
                    'BHS 5/6/11 if iFirstKeyField is not defined, use iCurrentControlName
                    If iFirstKeyField > "" Then
                        SetFocusControl(iFirstKeyField)
                    ElseIf iCurrentControlName > "" Then
                        SetFocusControl(iCurrentControlName)
                    End If

                Else
                    If iFirstNonKeyField > "" Then
                        SetFocusControl(iFirstNonKeyField)
                    ElseIf iCurrentControlName > "" Then
                        SetFocusControl(iCurrentControlName)
                    End If
                End If
            End If
            RaiseEvent OnSetStartFocus()    'Allow programmer to override default behavior
            'TightenCascade()   BHS 11/12/08
            Try
                RaiseEvent OnEndOfLoad()
            Catch ex As Exception
                ShowError("Unexpected error loading form (OnEndOfLoad)", ex)
            End Try

        End Sub

        Sub TightenCascade()
            If InDevEnv() = False Then      'This seems to cause a Microsoft error when in the Development Environment
                Dim P As New Point(0, 0)

                'Find lowest form so far and snug this up below it
                If Me.MdiParent IsNot Nothing AndAlso Me.MdiParent.MdiChildren IsNot Nothing Then
                    If Me.MdiParent.MdiChildren.Count < 2 Then
                        P.X = 0
                        P.Y = 0
                        Me.Location = P
                    Else
                        For Each F As Form In Me.MdiParent.MdiChildren
                            If F.Equals(Me) Then    'Don't consider this form
                                Continue For
                            End If
                            If F.WindowState = FormWindowState.Normal Then
                                If P.Y < F.Location.Y Then  'Find the lowest form
                                    P = F.Location
                                End If
                            End If
                        Next
                        P.X += 22
                        P.Y += 22
                        Me.Location = P
                    End If
                End If
            End If
        End Sub

        '''<summary> Stub to capture menu being clicked.  Logic should be handled in descendant </summary>
        Overridable Function ReceiveMenuClick(ByVal aMenuItemName As String, ByVal aMenuItemTag As String) As Boolean
            MsgBoxErr("Programmer Error", aMenuItemName & " " & aMenuItemTag & " not handled in decendant")
            Return True

        End Function

        ''' <summary> This calls OnCmdButtonsVisible event and returns aActedOn </summary>
        Function ClientSetCmdButtonsVisible() As Boolean    'BHS 6/27/08
            Dim ActedOn As Boolean
            Try
                RaiseEvent OnCmdButtonsVisible(ActedOn)
            Catch ex As Exception
                ShowError("Unexpected error when setting buttons visible", ex)
            End Try

            Return ActedOn
        End Function

#End Region

#Region "-------------------- New Record Logic -------------------------------"
        '1) NewRecord(CheckDirty) checks for changes before clearing the form:
        '       OnCheckDirty consults the child form for special checkdirty logic
        '       Otherwise, if an iDirtyTable has had data changed iCheckDirty is True
        '2) If the user says go ahead, or form is not dirty, then:
        '       NewRecord() clears the form and sets up for entering a new record

        '''<summary> NewRecord(CheckDirty) </summary>
        Function NewRecord(ByVal CheckDirty As Boolean) As Boolean
            Dim Answer As MsgBoxResult
            Dim BS As BindingSource

            'Commit any changes
            For Each BS In iBSs
                BS.EndEdit()
            Next

            Me.CheckDirty() 'Generic dirty checking

            If iIsDirty Then
                Answer = MsgBoxQuestion("OK to leave record without saving?")
                If Answer = MsgBoxResult.No Then
                    Return False 'Cancel
                Else
                    iEP.Clear() 'We're not saving and we're moving to a new record, so clear errors
                End If
            End If

            NewRecord()

            'SRM - 06/14/2012 - Added these 2 events when the "NEW" Button is clicked
            Try
                RaiseEvent OnAfterLoadForm()
                RaiseEvent OnEndOfLoad()
            Catch ex As Exception
                ShowError("Unexpected error after loading a new record", ex)
            End Try

            Return True

        End Function

        ''' <summary> Before asking OK To Save, make sure btnSave is not set to Not Visible </summary>
        Function OKToAskOKToSave() As Boolean
            Dim C As Control = FindControl("btnSave", Me)
            If C IsNot Nothing Then
                Dim B As Button = TryCast(C, Button)
                If B IsNot Nothing Then
                    Return B.Visible
                End If
            End If
            Return True

        End Function
        '''<summary> New Record without (or after) dirty checking '''</summary> 
        Function NewRecord() As Boolean
            Dim NewOK As Boolean = True
            Dim GV As DataGridView

            iIsNew = True
            'iWasNew = True

            '12/3/12 DJW - Reset iKey to blank for new records
            iKey = ""

            iDS.Clear() '7/7/BHS  Initialize dataset
            LoadNew()
            iIsDirty() = False  'BHS 8/11/08

            'Insert empty row if needed
            For Each GV In iGVs
                If GV.Rows.Count() = 0 Then
                    If TypeOf (GV) Is qGVEdit Then
                        Dim qGV As qGVEdit = CType(GV, qGVEdit)
                        If qGV._BlankRowOnEmpty = False Then Continue For
                    End If
                    InsertRow(GV)
                End If
            Next

            'Set key field protected/unprotected attributes
            SetFormAttributes()

            'Run custom logic in child
            Try
                RaiseEvent OnNewRecord(NewOK)
            Catch ex As Exception
                ShowError("Unexpected error opening a new record (OnNewRecord)", ex)
            End Try

            Return NewOK

        End Function

        '''<summary> Add empty record - we do this to BS rather than the form so that it can be saved back to the datatable </summary>
        Public Overridable Function LoadNew() As Boolean
            Dim BS As BindingSource

            'Try
            'Put an empty row in each binding source (master and any details)
            For Each BS In iBSs
                Dim BSRow As Object = BS.AddNew()

                'Dim DR As DataRow = CType(BSRow, DataRow)

                'BS.AddNew()
                BS.EndEdit()

                'DR.AcceptChanges()

                'BS.DataSource.Tabddle.AcceptChanges()
            Next

            'BHS 11/30/12
            ''iRecordLock.Clear()

            'Catch ex As Exception
            'TryError("fBase.LoadNew Error:", ex)
            'End Try

            Return True

        End Function

        '''<summary> Generic Checkdirty looks for any changes in the dataset </summary>
        Function CheckDirty() As Boolean
            'Dim EF As EntryField
            Dim ActedOn As Boolean = False

            'Try
            If Not iIsWriter Then  'GBV 7/14/2008
                iIsDirty = False
                Return False
            End If

            Try
                RaiseEvent OnCheckDirty(ActedOn)   'Check child for special dirty logic
            Catch ex As Exception
                ShowError("Unexpected error when checking form for changes (OnCheckDirty)", ex)
            End Try

            If Not iIsDirty And ActedOn Then Return False
            If iIsDirty Or ActedOn = True Then Return True 'BHS 1/15/07 added check for ActedOn flag.

            Return iIsDirty

        End Function


        '''<summary> Set all entry fields not dirty </summary>
        Sub SetNotDirty()
            iIsDirty() = False
            'For Each EF As EntryField In iEntryFields
            '    EF.ResetOrigValue()
            'Next
        End Sub

#End Region

#Region "-------------------- Delete and Save Record Logic -------------------------------"
        '''<summary> Delete Key clicked - Prompt to confirm, delete, and bring up blank form </summary>
        Overridable Function DeleteClicked() As Boolean
            Dim Prompt As String = "OK to permanently delete " + iKey
            Dim DeleteOK As Boolean = False

            Try
                RaiseEvent OnOKToDeletePrompt(Prompt)   'Allow client to supply a custom prompt
            Catch ex As Exception
                ShowError("Unexpected error confirming delete", ex)
            End Try


            If Not MsgBoxQuestion(Prompt + "?", , "Warning") = MsgBoxResult.Yes Then Return False

            iOKToClose = True   'BHS 9/15/08

            Try
                RaiseEvent OnDeleteForm(DeleteOK)
            Catch ex As Exception
                ShowError("Unexpected error deleting from the database (OnDeleteForm)", ex)
            End Try

            If DeleteOK Then
                iIsDirty = False    'BHS 10/10/08 No longer need to prompt if they want to save
                If iListForm IsNot Nothing AndAlso iListForm.IsDisposed = False Then 'SRM 0/05/2012
                    'BHS 4/17/13
                    If iListForm.iListGV IsNot Nothing Then
                        If iListGV.RowCount > 0 Then
                            iListForm.Query() 'Reselect List Pop (BHS 7/7)
                        End If
                    End If
                End If

                Try
                    RaiseEvent OnAfterDelete()
                Catch ex As Exception
                    ShowError("Unexpected error after deletion", ex)
                End Try

                Close()
            End If

            Return DeleteOK

        End Function

        '''<summary> Save key </summary>
        Function SaveClicked() As Boolean
            Dim OKToContinue As Boolean = True
            Dim BS As BindingSource

            'Commit any changes
            For Each BS In iBSs
                BS.EndEdit()
            Next

            Try
                RaiseEvent OnSaveClicked(OKToContinue)  'In case child wants to do extra checking
            Catch ex As Exception
                ShowError("Unexpected error preparing to Save (OnSaveClicked)", ex)
            End Try

            If OKToContinue Then
                Return SaveForm()
            End If

            Return False

        End Function

        '''<summary> Save Form does validation, calls OnSaveForm, commits results, and sets up for further data entry </summary>
        Function SaveForm() As Boolean
            'Dim OrigPos As Integer = CType(iListNavigator.PositionItem.Text, Integer)
            Dim SaveOK As Boolean = False
            Dim GV As DataGridView
            'Dim EF As EntryField

            ShowStatus("One moment...")


            If Not ValidateForm() Then Return False
            Try
                'BHS 10/16/06 - depend on each child app to clean out its own rows in ValidateForm...For Each GV In iGVs
                ' RemoveEmptyGVRows(GV, "logname")
                ' Next

                If iSaveThroughDS And iTestConstraints = False Then
                    'NOTE TO PROGRAMMER - IF THERE IS A CONSTRAINT PROB YOU CAN'T FIND, COMMENT OUT THIS LINE
                    'AND THEN THE SAVE WILL COMPLAIN
                    iDS.EnforceConstraints = True 'Database reqd fields, etc. enforced
                End If

            Catch ex As Exception
                'TryError("Data fails database constraints", ex)
                'BHS 6/29/12
                Throw New Exception()
                Return False

            End Try

            'BHS 6/29/12 - If we had a record lock but we don't any more, Return False
            ''If iRecordLock IsNot Nothing AndAlso
            ''   iRecordLock.LockHasBeenTested = True AndAlso
            ''   iRecordLock.IsLocked = True Then
            ''    If iRecordLock.CheckLock = False Then
            ''        ShowStatus("Not Saved")
            ''        Return False
            ''    End If

            ''End If

            'Guarantee we have a new transaction
            iSQLTran = Nothing
            iSQLCn = Nothing
            iODBCTran = Nothing

            Try
                RaiseEvent OnSaveForm(SaveOK)
            Catch ex As Exception
                ShowError("Unexpected error during save (OnSaveForm)", ex)
            End Try

            iAcceptConcurrency = False ' GBV 8/12/2011 - Force to pop concurrency box on next save.

            If SaveOK Then

                iDS.AcceptChanges()

                If iSQLTran IsNot Nothing Then
                    iSQLTran.Commit()
                    iSQLTran.Dispose()
                    iSQLCn.Close()
                End If

                'BHS 5/8/10 Supports simultaneous Ifx and SQL transactions
                If iODBCTran IsNot Nothing Then
                    Try
                        iODBCTran.Commit()
                        ' Catch ex As Exception
                        'BHS 7/10/08  Ignore exceptions until Informix supports transactions.  Then take the Catch out.  
                    Finally
                        iODBCTran.Dispose()
                        iODBCCn.Close()
                    End Try
                End If
            End If

            'If New Record, redo query 
            If iIsNew = True Then
                If iListForm IsNot Nothing Then iListForm.QueryAfterNewRecordSave()
            Else
                If iListForm IsNot Nothing Then iListForm.SelectListRow(iKey)
            End If

            'Provide at least one blank line in each GV
            For Each GV In iGVs
                If GV.Rows.Count() = 0 Then
                    If TypeOf (GV) Is qGVEdit Then
                        Dim qGV As qGVEdit = CType(GV, qGVEdit)
                        If qGV._BlankRowOnEmpty = False Then Continue For
                    End If
                    InsertRow(GV)
                End If
            Next

            'BHS 8/11/08 'Reset iEntryFields OrigValue
            'For Each EF In iEntryFields
            '    EF.ResetOrigValue()
            'Next

            iIsDirty = False
            If iIsNew = True Then
                iMode = "edit"  'BHS 9/21/10
                iIsNew = False  'BHS 6/24/08
                SetFormAttributes()
            End If

            iEP.Clear()     'BHS 9/18/08 Only needed if some other part of the code didn't clear it

            If SaveOK Then
                ShowStatus("Saved")
            Else
                ShowStatus("Not Saved")
            End If


            Try
                RaiseEvent OnAfterSave()
            Catch ex As Exception
                ShowError("Unexpected error after save (OnAfterSave)", ex)
            End Try


            'BHS 5/8/10  Added check for iSQLTran.Connection
            If iSQLTran IsNot Nothing AndAlso iSQLTran.Connection IsNot Nothing Then
                ShowStatus("Not Saved")
                'iSQLTran.Rollback()    BHS 10/7/08
                'iSQLTran.Dispose()
            End If

            'BHS 5/8/10  Added check for iSQLTran.Connection
            If iODBCTran IsNot Nothing AndAlso iODBCTran.Connection IsNot Nothing Then
                ShowStatus("Not Saved")
                If iODBCTran.Connection IsNot Nothing Then
                    iODBCTran.Rollback()   'BHS 4/28/09 - normally done done in SaveTable
                    iODBCTran.Dispose()
                End If
            End If

            If iSaveThroughDS Then iDS.EnforceConstraints = False

            Return SaveOK

        End Function


        '''<summary> SQL Server SaveTable opens a tran if needed, and does deletes before adds.  Rollback if prob.  
        ''' Leave to calling program to commit so multiple tables can be updated in one transaction. </summary>
        Function SaveTable(ByVal aDA As SqlDataAdapter, ByVal aT As DataTable, ByVal aTableType As String) As Boolean
            'BHS 4/28/09  Set iSQLCn and iSQLTran and then call SaveTable2, in qFunctions
            '   This avoids having two very similar functions in two places
            If aDA.UpdateCommand Is Nothing Then
                Throw New Exception("You may not write to the Live Database from a Test Application")
            End If

            If iSQLCn Is Nothing Then
                iSQLCn = aDA.UpdateCommand.Connection
            End If
            If iSQLCn.State = ConnectionState.Closed Then iSQLCn.Open() 'BHS 1/28/08 Moved from previous If block
            If iSQLTran Is Nothing Then iSQLTran = iSQLCn.BeginTransaction
            Return SaveTable2(aDA, aT, aTableType, iSQLCn, iSQLTran)
        End Function


        'Dim TD, TM, TA As DataTable
        'Dim Str, Err As String

        ''Set up connection and Transaction if not open yet
        '    If iSQLCn Is Nothing Then
        '        iSQLCn = aDA.UpdateCommand.Connection
        '    End If

        '    If iSQLCn.State = ConnectionState.Closed Then iSQLCn.Open() 'BHS 1/28/08 Moved from previous If block
        '    If iSQLTran Is Nothing Then iSQLTran = iSQLCn.BeginTransaction

        ''Assign iSQLTransaction to aDA commands
        '    aDA.UpdateCommand.Transaction = iSQLTran
        '    aDA.InsertCommand.Transaction = iSQLTran
        '    aDA.DeleteCommand.Transaction = iSQLTran

        '    Try

        '        If aTableType = "D" Then
        '            TD = aT.GetChanges(DataRowState.Deleted)
        '            TM = aT.GetChanges(DataRowState.Modified)
        '            TA = aT.GetChanges(DataRowState.Added)

        ''Delete detail records
        '            If Not TD Is Nothing Then
        '                aDA.Update(TD)
        '                TD.Dispose()
        '            End If

        ''Add detail records
        '            If Not TA Is Nothing Then
        '                aDA.Update(TA)
        '                TA.Dispose()
        '            End If

        ''Modify detail records
        '            If Not TM Is Nothing Then
        '                aDA.Update(TM)
        '                TM.Dispose()
        '            End If

        '        Else

        ''Update master record
        '            aDA.Update(aT)

        '        End If

        '    Catch ex As Exception
        '        Err = ex.ToString
        '        Str = RollBackTran(iSQLTran)
        '        iSQLTran.Dispose()
        '        If Str > " " Then Err += " *** AND ROLLBACK EXCEPTION TOO: " + Str
        '        ErrMsg("SaveTable Error", Err)
        '        Return False

        '    End Try

        ''TrimTable(aT)

        '    Return True

        'End Function


        '''<summary> Informix SaveTable opens a tran if needed, and does deletes before adds.  Rollback if prob.  
        ''' Leave to calling program to commit so multiple tables can be updated in one transaction. 
        '''  Always uses an ODBC connection and transaction </summary>
        Function IfxSaveTable(ByVal aDA As OdbcDataAdapter, ByVal aT As DataTable, ByVal aTableType As String) As Boolean
            'BHS 4/28/09  Set iODBCCn and iODBCTran and then call SaveTable2, in qFunctions
            '   This avoids having two very similar functions in two places

            'BHSCONV
            If ConnType <> "IFX" Then
                If iSQLCn Is Nothing Then

                    iSQLCn = GetSQLDA(aT.TableName).UpdateCommand.Connection
                End If

                If iSQLCn.State = ConnectionState.Closed Then iSQLCn.Open()
                If iSQLTran Is Nothing Then iSQLTran = iSQLCn.BeginTransaction()

                Return SaveTable2(GetSQLDA(aT.TableName), aT, aTableType, iSQLCn, iSQLTran)
            Else
                If aDA.UpdateCommand Is Nothing Then
                    Throw New Exception("You may not write to the Live Database from a Test Application")
                End If
            End If


            'Set up connection and Transaction if not open yet
            If iODBCCn Is Nothing Then
                iODBCCn = aDA.UpdateCommand.Connection
            End If

            If iODBCCn.State = ConnectionState.Closed Then iODBCCn.Open()
            If iODBCTran Is Nothing Then iODBCTran = iODBCCn.BeginTransaction()

            Return IfxSaveTable2(aDA, aT, aTableType, iODBCCn, iODBCTran)
        End Function


        'Dim TD, TM, TA As DataTable
        'Dim Str, Err As String

        ''Set up connection and Transaction if not open yet
        '    If iODBCCn Is Nothing Then
        '        iODBCCn = aDA.UpdateCommand.Connection
        '    End If

        '    If iODBCCn.State = ConnectionState.Closed Then iODBCCn.Open()
        '    If iODBCTran Is Nothing Then iODBCTran = iODBCCn.BeginTransaction()

        ''Assign iSQLTransaction to aDA commands
        '    aDA.UpdateCommand.Transaction = iODBCTran
        '    aDA.InsertCommand.Transaction = iODBCTran
        '    aDA.DeleteCommand.Transaction = iODBCTran

        '    Try

        '        If aTableType = "D" Then
        '            TD = aT.GetChanges(DataRowState.Deleted)
        '            TM = aT.GetChanges(DataRowState.Modified)
        '            TA = aT.GetChanges(DataRowState.Added)

        ''Delete detail records
        '            If Not TD Is Nothing Then
        '                aDA.Update(TD)
        '                TD.Dispose()
        '            End If

        ''Add detail records
        '            If Not TA Is Nothing Then
        '                aDA.Update(TA)
        '                TA.Dispose()
        '            End If

        ''Modify detail records
        '            If Not TM Is Nothing Then
        '                aDA.Update(TM)
        '                TM.Dispose()
        '            End If

        '        Else

        ''Update master record
        '            aDA.Update(aT)
        ''iODBCTran.Rollback()    'BHS 7/9/08 DEBUG

        '        End If

        '    Catch ex As Exception
        '        Err = ex.ToString
        '        Str = RollBackTran(iODBCTran)
        '        iODBCTran.Dispose()
        '        If Str > " " Then Err += " *** AND ROLLBACK EXCEPTION TOO: " + Str
        '        ErrMsg("SaveTable Error", Err)
        '        Return False

        '    End Try

        ''TrimTable(aT)

        '    Return True

        'End Function

        'BHS 4/28/09 - Moved to qFunctions so it is always available.  Client now has to supply the transaction.
        ''''<summary> When an SQLDataAdapter is used to save a table, this function can be called from the DA.SQLRowUpdated event
        ''''  to set the iDS table's identity value while still in the transaction. </summary>
        'Sub SetTableIdentity(ByVal aTableName As String, ByVal aIdentityColName As String, ByVal e As System.Data.SqlClient.SqlRowUpdatedEventArgs)
        '    If e.Errors Is Nothing Then
        '        If e.StatementType = StatementType.Insert Then
        '            Dim SQL As String = "Select " & aIdentityColName & " From " & aTableName & " Where " & _
        '                  aIdentityColName & " = @@Identity"
        '            Dim cmd As New SqlCommand(SQL, e.Command.Connection)
        '            If iSQLTran IsNot Nothing Then cmd.Transaction = iSQLTran
        '            Dim dr As SqlDataReader = cmd.ExecuteReader()
        '            If dr.Read Then
        '                Dim RO As Boolean = e.Row.Table.Columns(aIdentityColName).ReadOnly  'BHS 2/19/08
        '                e.Row.Table.Columns(aIdentityColName).ReadOnly = False
        '                e.Row.Item(aIdentityColName) = dr.Item(aIdentityColName)
        '                e.Row.Table.Columns(aIdentityColName).ReadOnly = RO
        '            End If

        '            dr.Close()
        '        End If
        '    End If
        'End Sub

        ''''<summary> When an ODBCDataAdapter is used to save a table, this function can be called from the DA.ODBCRowUpdated event
        ''''  to set the iDS table's identity value. </summary>
        'Sub SetTableIdentity(ByVal aPartn As String, ByVal aTableName As String, _
        '                        ByVal aColName As String, ByVal aTrans As OdbcTransaction, _
        '                        ByRef e As System.Data.Odbc.OdbcRowUpdatedEventArgs)
        '    If e.Errors Is Nothing Then
        '        If e.StatementType = StatementType.Insert Then
        '            Dim cmd As New OdbcCommand("SELECT dbinfo('sqlca.sqlerrd1') FROM " & _
        '                       aPartn & ":systables where tabname = '" & aTableName & "'", _
        '                       aTrans.Connection)
        '            cmd.Transaction = aTrans
        '            Dim RO As Boolean = e.Row.Table.Columns(aColName).ReadOnly  'GBV 3/28/2009
        '            e.Row.Table.Columns(aColName).ReadOnly = False
        '            e.Row.Item(aColName) = CInt(cmd.ExecuteScalar)
        '            e.Row.Table.Columns(aColName).ReadOnly = RO ' GBV 3/28/2009
        '        End If
        '    End If
        'End Sub

        '''<summary> Stub for decendant routine to select a GV or UG row based on aKey </summary>
        Overridable Function SelectListRow(ByVal aKey As String) As Boolean
            Return True
        End Function

        ''' <summary> Stub for decendat routine to handle whether/how to Query after saving a new record </summary>
        Overridable Function QueryAfterNewRecordSave() As Boolean
            Return False
        End Function

        '''<summary> Stub for descendant to specify a select that returns column names that match GridView.Columns.DataPropertyName.
        ''' The select dataset value will be written to the GridView row </summary>
        Overridable Function SelectRowValues(ByVal aKey As String) As String
            Return ""
        End Function
#End Region

#Region "-------------------- Other Commands -------------------------------"
        '''<summary> Calls child OnQuery event </summary>
        Function Query() As Boolean
            gQBESyntaxError = False
            Try
                RaiseEvent OnQuery()
            Catch ex As Exception
                ShowError("Unexpected error during query (OnQuery)", ex)
            End Try

            Return True
        End Function

        '''<summary> Handle + amd - GridView Button Click </summary>
        Private Sub gvButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
            'To handle special command, like insert at, insert at top, etc., uncomment special command logic in qBtnInsertGVRow control, have the programmer fill in the _SpecialCommand property, and then manage the special logic here.
            If sender.GetType.Name.ToLower = "qbtninsertgvrow" Then
                Dim BtnInsert As qBtnInsertGVRow = CType(sender, qBtnInsertGVRow)

                If BtnInsert._GVName = "" Then
                    MsgBoxErr("Programmer Error", "Need to fill in _GVName button property on qBtnInsertGVRow")
                    Return
                End If

                Dim C As Control = FindControl(BtnInsert._GVName, Me)
                If C.Name.ToLower <> BtnInsert._GVName.ToLower Then
                    MsgBoxErr("Programmer Error", "_GVName didn't match a valid gridview on qBtnInsertGVRow")
                    Return
                End If

                Dim GV As qGVBase = TryCast(C, qGVBase)
                If GV Is Nothing Then
                    MsgBoxErr("Programmer Error", "_GVName must be of type qGVBase")
                    Return
                End If

                If BtnInsert._DrillDown = True Then
                    If GV._ListEditFormName = "" Then
                        MsgBoxErr("Programmer Error", "_ListEditFormName is required if GV is marked drill down")
                        Return
                    End If
                    If GV._ListEditFormName = "none" Then Return

                    'If GV.SelectedRows.Count > 0 Then  BHS 2/12/08
                    iKey = ""
                    OpenEditFormFromList(GV._ListEditFormName, iKey, CType(GV, DataGridView))
                    'End If
                Else
                    InsertRow(CType(GV, DataGridView))
                End If

            End If

            If sender.GetType.Name.ToLower = "qbtnremovegvrow" Then
                Dim BtnRemove As qBtnRemoveGVRow = CType(sender, qBtnRemoveGVRow)

                If BtnRemove._GVName = "" Then
                    MsgBoxErr("Programmer Error", "Need to fill in _GVName button property on qBtnRemoveGVRow")
                    Return
                End If

                Dim C As Control = FindControl(BtnRemove._GVName, Me)
                If C.Name.ToLower <> BtnRemove._GVName.ToLower Then
                    MsgBoxErr("Programmer Error", "_GVName didn't match a valid gridview on qBtnRemoveGVRow")
                    Return
                End If

                Dim GV As DataGridView = CType(C, DataGridView)
                RemoveSelectedGVRow(GV)

            End If

        End Sub

        '''<summary> Insert Row in qGVEdit </summary>
        Function InsertRow(ByRef aGV As qGVEdit) As Boolean
            Dim GV As DataGridView = CType(aGV, DataGridView)
            Return InsertRow(GV)
        End Function

        '''<summary> Insert Row in GV </summary>
        Function InsertRow(ByRef aGV As DataGridView) As Boolean
            Dim ActedOn As Boolean = False
            Dim R As DataGridViewRow
            Dim BS As BindingSource = CType(aGV.DataSource, BindingSource) 'DEBUG is this OK?  Ever use other binding source?
            Dim C As Control = Nothing

            Try
                RaiseEvent OnInsertRow(aGV, ActedOn) 'Allow non-standard handling in child
            Catch ex As Exception
                ShowError("Unexpected error inserting a row in " & aGV.Name & " grid", ex)
            End Try


            If Not ActedOn Then

                With aGV
                    iDS.EnforceConstraints = False

                    Dim BSRow As Object = BS.AddNew()

                    BS.EndEdit()
                    BS.MoveLast()   'Move selection to last row, where we inserted

                    'Fill in default fields from child
                    iIsDirtySuspended = True
                    Try
                        RaiseEvent OnSetGVDefaultFields(aGV, .Rows(.Rows.Count - 1))
                    Catch ex As Exception
                        ShowError("Unexpected error setting default fields in " & aGV.Name, ex)
                    End Try

                    iIsDirtySuspended = False

                    R = .Rows(.Rows.Count - 1)  'last row

                    'BHS 8/11/08
                    'For Each Cell In R.Cells()
                    '    Dim EF As EntryField = New EntryField
                    '    EF.GV = aGV
                    '    EF.ColName = Cell.OwningColumn.Name
                    '    EF.Row = R.Index
                    '    EF.OrigValue = Cell.FormattedValue.ToString 'Default value is orig
                    '    iEntryFields.Add(EF)
                    'Next

                End With

            End If

            Return True
        End Function

        '''<summary> Remove qGVEdit Row, after prompting the user to make sure </summary>
        Function RemoveSelectedGVRow(ByRef aGV As qGVEdit) As Boolean
            Dim GV As DataGridView = CType(aGV, DataGridView)
            RemoveSelectedGVRow(GV)
        End Function

        '''<summary> Remove GV row, after prompting the user to make sure </summary>
        Function RemoveSelectedGVRow(ByRef aGV As DataGridView) As Boolean
            Dim Str, WholeString, PartString As String
            Dim KeyValue As String = ""
            Dim R As DataGridViewRow
            Dim RowNum As Integer
            Dim C As DataGridViewCell
            'Dim EF As EntryField = Nothing
            Str = ""

            With aGV
                R = .SelectedCells(0).OwningRow
                If R Is Nothing Then
                    MsgBoxInfo("Select a row in the list and then you can remove it")
                    Return False
                Else

                    RaiseEvent OnRemoveGVRowPrompt(aGV, R, Str)
                    If Str > "" Then GoTo PromptUser
                    'Construct a generic prompt
                    RowNum = R.Index
                    If TypeOf (aGV) Is qGVBase Then
                        If Not IsNothing(CType(aGV, qGVBase)._KeyFields) Then
                            WholeString = CType(aGV, qGVBase)._KeyFields
                            GoTo ParseWholeString
                        End If
                    End If
                    If Not IsNothing(aGV.Tag) Then
                        WholeString = aGV.Tag.ToString
ParseWholeString:
                        While WholeString > " "
                            PartString = ParseStr(WholeString, "|")
                            If .Rows(RowNum).Cells(PartString).Visible = True Then
                                KeyValue = KeyValue & .Rows(RowNum).Cells(PartString).Value.ToString & "  "
                            End If
                        End While
                        Str = "OK to delete " & KeyValue & "?"
                    Else
                        Str = "OK to delete " + .SelectedCells(0).Value.ToString + "?"
                    End If
                    If Not Str > " " Then Str = "OK to delete this row? "
PromptUser:
                    If MsgBoxQuestion(Str) = MsgBoxResult.Yes Then
                        RemoveGVRow(aGV, R, RowNum)
                        iIsDirty = True
                    End If

                    'Highlight first visible cell of previous row
                    If RowNum + 1 > .Rows.Count Then RowNum = .Rows.Count - 1
                    If RowNum >= 0 Then
                        R = .Rows(RowNum)
                        R.Selected = True
                        For Each C In R.Cells
                            If C.Visible = True Then
                                C.Selected = True
                                'DEBUG not allowing immediate edit yet...
                                Exit For
                            End If
                        Next
                    End If
                End If

                If .Rows.Count = 0 Then
                    If TypeOf (aGV) Is qGVEdit Then
                        Dim qGV As qGVEdit = CType(aGV, qGVEdit)
                        If qGV._BlankRowOnEmpty = False Then GoTo GVSkipIt
                    End If
                    InsertRow(aGV) 'Make sure there is always at least one blank row
GVSkipIt:
                End If

            End With

            Return True
        End Function

        '''<summary>  Remove a Row from a GV </summary>
        Function RemoveGVRow(ByVal aGV As DataGridView, ByVal aRow As DataGridViewRow, ByVal aRowNum As Integer) As Boolean
            Dim ActedOnInChild As Boolean = False
            'Dim EF As EntryField = Nothing

            Try
                RaiseEvent OnRemoveGVRow(aGV, aRow, ActedOnInChild) 'Allow non-standard handling in child
            Catch ex As Exception
                ShowError("Unexpected error removing a row from " & aGV.Name, ex)
            End Try


            If Not ActedOnInChild Then

                'Try
                'Remove the grid row
                aGV.Rows.Remove(aRow)

                'Remove entry fields associated with the row
                'BHS 8/11/08 StartEFSearch:
                '                For Each EF In iEntryFields
                '                    If EF.GV IsNot Nothing Then
                '                        If EF.GV.Name = aGV.Name Then
                '                            If EF.Row = aRowNum Then
                '                                iEntryFields.Remove(EF)
                '                                GoTo StartEFSearch
                '                            End If
                '                        End If
                '                    End If
                '                Next

                'BHS 8/11/08 'Then subtract 1 from higher rows
                'For Each EF In iEntryFields
                '    If EF.GV IsNot Nothing Then
                '        If EF.GV.Name = aGV.Name Then
                '            If EF.Row > aRowNum Then EF.Row = EF.Row - 1
                '        End If
                '    End If
                'Next

                'Catch ex As Exception
                '   TryError("Problem in fBase.RemoveGVRow", ex)
                'End Try


            End If

        End Function

        '''<summary> Remove GV rows where aTestField is DBNull, Nothing, or "" </summary>
        Function RemoveEmptyGVRows(ByVal aGV As DataGridView, ByVal aTestField As String) As Boolean
            Dim R As DataGridViewRow
            Dim RowNum As Integer

            'Try
StartRowSearch:
            For Each R In aGV.Rows
                RowNum = R.Index

                'Remove empty detail rows
                If IsDBNull(R.Cells(aTestField)) Then
                    RemoveGVRow(aGV, R, RowNum)
                    GoTo StartRowSearch
                End If
                If R.Cells(aTestField).Value.ToString Is Nothing Or R.Cells(aTestField).Value.ToString < " " Then
                    RemoveGVRow(aGV, R, RowNum)
                    GoTo StartRowSearch
                End If

            Next
            'Catch ex As Exception
            'TryError("Problem in fBase.RemoveEmptyGVRows", ex)
            'End Try
        End Function

        'Stub allows descendants to set Dirty Indicator
        Overridable Sub SetDirtyIndicator(ByVal aIsDirty As Boolean)

        End Sub
#End Region

#Region "-------------------- Other Methods ------------------------------"

        '''<summary> Set Form attributes, such as protection, background color, and focus, 
        ''' based on New/Edit and Permissions.  Calls child OnSetFormAttributes. </summary>
        Sub SetFormAttributes()
            'If fBase.ActiveForm IsNot Nothing Then
            If Not InDevEnv() Then
                iFName = Mid(Me.Name, 3)
                IsWriter(iFName)   'Set iIsWriter before attributes are set
            End If

            Try
                RaiseEvent OnSetFormAttributes()
            Catch ex As Exception
                ShowError("Unexpected error setting form attributes (OnSetFormAttributes)", ex)
            End Try


        End Sub

        '''<summary> Get Key from a specific row in a GridView
        '''   Assumes GV.Tag or _KeyFieldsholds definition of key cell names </summary>
        Function GetKey(ByRef aGV As DataGridView, ByVal aRowNum As Integer) As String
            Dim CellNames As String = ""
            Dim CellName As String = ""
            Dim Key As String = ""

            If aGV Is Nothing Then Return ""
            'BHS 1/30/08 above replaced the following:  If iListGV Is Nothing And iListUG Is Nothing Then Return "" 'If no attached List in Edit mode   Removed references to iListGV in this routine.

            If aGV.Tag IsNot Nothing Then CellNames = aGV.Tag.ToString

            If TypeOf (aGV) Is qGVBase Then
                If Not CType(aGV, qGVBase)._KeyFields Is Nothing Then
                    CellNames = CType(aGV, qGVBase)._KeyFields
                End If
            End If

            'RaiseEvent OnGetKey(aGV, aRow, Key) 'Special handling in the child

            If Key = "" Then    'If child didn't set key, follow generic logic
                If CellNames > "" Then
                    'If iListGV IsNot Nothing Then
                    Do
                        CellName = ParseStr(CellNames, "|")
                        If CellName > "" Then
                            If Key > "" Then Key &= "|"
                            Key &= aGV.Rows(aRowNum).Cells(CellName).Value.ToString
                            'Key &= iListGV.Rows(aRowNum).Cells(CellName).Value.ToString
                        Else
                            Exit Do
                        End If
                    Loop
                    Return Key
                    'End If
                Else
                    'Suppress error message to accommodate report list windows with no edit forms!  SDC 03/18/2009
                    'MsgBoxErr("Programmer Error", "Missing Key Cell Names Tag in GV " + aGV.Name)
                End If
            Else
                'Suppress error message to accommodate report list windows with no edit forms!  SDC 03/18/2009
                'MsgBoxErr("Programmer Error", "Missing Key Cell Names Tag in GV ")
            End If

            Return Key

        End Function

        ''<summary> Get Key from a specific row in an UltraGrid
        ''   Assumes UG._KeyFields holds definition of key cell names </summary>
        'Function GetKey(ByRef aUG As qUG, ByVal aRowNum As Integer) As String
        '    Dim CellNames As String = ""
        '    Dim CellName As String = ""
        '    Dim Key As String = ""

        '    If aUG Is Nothing Then Return ""
        '    'If iListUG Is Nothing Then Return "" 'If no attached List in Edit mode

        '    CellNames = aUG._KeyFields

        '    'RaiseEvent OnGetKey(aUG, aRow, Key) 'Special handling in the child 
        '    '   (may not be needed with _KeyFields)

        '    If CellNames > "" Then
        '        Do
        '            CellName = ParseStr(CellNames, "|")
        '            If CellName > "" Then
        '                If Key > "" Then Key &= "|"
        '                Key &= aUG.Rows(aRowNum).Cells(CellName).Value.ToString
        '            Else
        '                Exit Do
        '            End If
        '        Loop
        '        Return Key
        '    Else
        '        MsgBoxErr("Programmer Error", "Missing UG._KeyFields in  " + aUG.Name)
        '    End If

        '    Return Key

        'End Function

        '''<summary> Stub for ShowStatus in descendant class </summary>
        Overridable Function ShowStatus(ByVal aMessage As String) As Boolean

            MsgBoxErr("Programmer Error", "ShowStatus Function required in derived class")  'Generally in flMain or feMain

            Return False

        End Function

        '''<summary> Stub for ShowProgress in descendant class </summary>
        Overridable Function ShowProgress(ByVal aMessage As String) As Boolean

            MsgBoxErr("Programmer Error", "ShowProgress Function required in derived class") 'Generally in flMain or feMain

            Return False

        End Function

        '''<summary> Stub for ShowProgress in descendant class </summary>
        Overridable Function ShowProgress(ByVal aMessage As String, ByVal aShowHourGlass As Boolean) As Boolean

            MsgBoxErr("Programmer Error", "ShowProgress Function required in derived class") 'Generally in flMain or feMain

            Return False

        End Function

        '''<summary> Stub for ShowHelp in descendant class.  </summary>
        Overridable Function ShowHelp(ByVal aMessage As String) As Boolean
            MsgBoxErr("Programmer Error", "ShowHelp function required in derived class") 'Generally in flMain or feMain
            Return False
        End Function

        '''<summary> Find a control by name, and give it focus </summary>
        Function SetFocusControl(ByVal ControlName As String) As Boolean
            Dim C As Control = FindControl(ControlName, Me)
            'BHS 5/6/11
            If C IsNot Nothing Then SetiOrigValue(C)

            If C.Name.ToLower = ControlName.ToLower Then
                C.Focus()
                Return True
            Else
                Return False
            End If

        End Function

        '''<summary> Find a GV Cell by Row and Column, and give it focus </summary>
        Function SetFocusCell(ByVal aGVName As String, ByVal aRow As Integer, ByVal aCol As Integer) As Boolean
            Dim C As Control = FindControl(aGVName, Me)
            If C IsNot Nothing Then
                If TypeOf C Is DataGridView Then
                    Dim GV As DataGridView = CType(C, DataGridView)
                    GV.Focus()

                    iOrigValue = C.Text 'SRM 10/27/10  Need this here because we never re-enter field when navigating to next/prevuious record
                    Dim Cell As DataGridViewCell = GV.Rows(aRow).Cells(aCol)
                    Cell.Selected = True
                    GV.CurrentCell = Cell
                End If
            End If

        End Function

        '''<summary> Add to iSQLDescr </summary>
        Sub AddToSQLDescr(ByVal aColName As String, ByVal aValue As String)
            Dim FirstChar As String = Mid(aValue, 1, 1)

            If iSQLDescr.Length > 0 Then iSQLDescr += ", "

            If FirstChar > "z" Then
                iSQLDescr += aColName & " " & aValue
            Else
                iSQLDescr += aColName + " = " & aValue
            End If
        End Sub


        ''' <summary> Get value from control for loading into Where Clause
        '''    Excludes checkboxes, which are not used in QBE </summary>
        Sub GetQueryString(ByVal StartControl As Control, ByRef aSQL As String)
            Dim C, ChildC As Control
            Dim T As TextBox
            Dim CB As ComboBox
            Dim qT As qTextBox
            Dim qMT As qMaskedTextBox
            Dim qCB As qComboBox
            Dim DD As QSILib.qDD
            Dim qCBM As qCBMultiCol
            Dim ColName As String = ""
            Dim ColType As String = ""
            Dim Value As String = ""
            Dim Type As String = ""

            'For Each C In StartControl.Controls
            C = StartControl

            If C.Name.Length > 1 Then
                If isQueryControl(C, Type) Then
                    Select Case Type

                        Case "textbox"
                            'Build Query String
                            T = CType(C, TextBox)
                            If T.Text > "" Then
                                If T.Tag IsNot Nothing Then
                                    If GetTagColInfo(T.Tag, ColName, ColType) Then
                                        aSQL = AddWhere(aSQL, ColName, T.Text, ColType)
                                        AddToSQLDescr(ColName, T.Text)
                                    End If
                                End If
                            End If
                        Case "qtextbox".ToLower
                            qT = CType(C, qTextBox)
                            If qT.Text > "" Then
                                If ParseQueryDef(qT._QueryDef, ColName, ColType) Then
                                    'BHS 8/10/10 added logic to check _DataType
                                    If qT._DataType = DataTypeEnum.Dat Then ColType = "dat"
                                    If qT._DataType = DataTypeEnum.Num Then ColType = "num"
                                    aSQL = AddWhere(aSQL, ColName, qT.Text, ColType)
                                    If qT._QueryDescr.Length > 0 Then
                                        AddToSQLDescr(qT._QueryDescr, qT.Text)
                                    Else
                                        AddToSQLDescr(ColName, qT.Text)
                                    End If
                                End If
                            End If
                        Case "qmaskedtextbox".ToLower
                            qMT = CType(C, qMaskedTextBox)
                            If qMT.Text > "" Then
                                If ParseQueryDef(qMT._QueryDef, ColName, ColType) Then
                                    If qMT._DataType = DataTypeEnum.Dat Then ColType = "dat"
                                    If qMT._DataType = DataTypeEnum.Num Then ColType = "num"
                                    aSQL = AddWhere(aSQL, ColName, qMT.Text, ColType)
                                    If qMT._QueryDescr.Length > 0 Then
                                        AddToSQLDescr(qMT._QueryDescr, qMT.Text)
                                    Else
                                        AddToSQLDescr(ColName, qMT.Text)
                                    End If
                                End If
                            End If
                        Case "combobox"
                            CB = CType(C, ComboBox)
                            If CB.Text.ToString > "" Then
                                If CB.Tag IsNot Nothing Then
                                    If GetTagColInfo(CB.Tag, ColName, ColType) Then
                                        'Send Selected Value if filled in
                                        'AFTER RELEASE 3, CHECK IF DROPDOWNLIST BEFORE USING SELECTED VALUE
                                        Value = CB.Text
                                        If CB.SelectedValue IsNot Nothing Then
                                            If CB.SelectedValue.ToString > "" Then
                                                Value = CB.SelectedValue.ToString
                                            End If
                                        End If
                                        aSQL = AddWhere(aSQL, ColName, Value, ColType)
                                        AddToSQLDescr(ColName, CB.Text) ' 12/6/2010 GBV replaced value with CB.Text
                                    End If
                                End If
                            End If
                        Case "qcombobox"
                            qCB = CType(C, qComboBox)
                            If qCB.Text.ToString > "" Then
                                If ParseQueryDef(qCB._QueryDef, ColName, ColType) Then
                                    If qCB._DataType = DataTypeEnum.Dat Then ColType = "dat"
                                    If qCB._DataType = DataTypeEnum.Num Then ColType = "num"
                                    'Send Selected Value if filled in
                                    Value = qCB.Text
                                    If qCB.SelectedValue IsNot Nothing Then
                                        'AFTER RELEASE 3, CHECK IF DROPDOWNLIST BEFORE USING SELECTED VALUE
                                        If qCB.SelectedValue.ToString > "" Then
                                            Value = qCB.SelectedValue.ToString
                                        End If
                                    End If
                                    aSQL = AddWhere(aSQL, ColName, Value, ColType)
                                    If qCB._QueryDescr.Length > 0 Then
                                        'AddToSQLDescr(qCB._QueryDescr, Value) GBV 12/6/2010
                                        AddToSQLDescr(qCB._QueryDescr, qCB.Text) '12/6/2010 GBV replaced value with qCB.Text
                                    Else
                                        AddToSQLDescr(ColName, qCB.Text) '12/6/2010 GBV replaced value with qCB.Text
                                    End If
                                End If
                            End If

                            'BHS 2/9/10 From Oakland
                        Case "qdd"
                            DD = CType(C, QSILib.qDD)
                            If DD.Text.ToString > "" Then
                                If ParseQueryDef(DD._QueryDef, ColName, ColType) Then
                                    If DD._DataType = DataTypeEnum.Dat Then ColType = "dat"
                                    If DD._DataType = DataTypeEnum.Num Then ColType = "num"
                                    Value = DD.Text
                                    If DD._MustMatchList = True Then
                                        '04/14/2011 SRM added check to make sure selected value is not nothing
                                        If DD.SelectedValue IsNot Nothing AndAlso DD.SelectedValue.ToString > "" Then
                                            Value = DD.SelectedValue.ToString
                                        End If
                                    End If
                                    aSQL = AddWhere(aSQL, ColName, Value, ColType)
                                    If DD._QueryDescr.Length > 0 Then
                                        AddToSQLDescr(DD._QueryDescr, DD.Text) ' 12/6/2010 GBV replaced value with DD.Text
                                    Else
                                        AddToSQLDescr(ColName, DD.Text) ' 12/6/2010 GBV replaced value with DD.Text
                                    End If
                                End If
                            End If

                        Case "qcbmulticol"
                            qCBM = CType(C, qCBMultiCol)
                            If qCBM.Text.ToString > "" Then
                                If ParseQueryDef(qCBM._QueryDef, ColName, ColType) Then
                                    If qCBM._DataType = DataTypeEnum.Dat Then ColType = "dat"
                                    If qCBM._DataType = DataTypeEnum.Num Then ColType = "num"
                                    'Send Selected Value if filled in
                                    Value = qCBM.Text
                                    If qCBM.SelectedValue IsNot Nothing Then
                                        If qCBM.SelectedValue.ToString > "" Then
                                            Value = qCBM.SelectedValue.ToString
                                        End If
                                    End If
                                    aSQL = AddWhere(aSQL, ColName, Value, ColType)
                                    If qCBM._QueryDescr.Length > 0 Then
                                        AddToSQLDescr(qCBM._QueryDescr, Value)
                                    Else
                                        AddToSQLDescr(ColName, Value)
                                    End If
                                End If
                            End If

                    End Select
                End If
            End If


            For Each ChildC In C.Controls
                GetQueryString(ChildC, aSQL)
            Next
            'Next

        End Sub

        ''' <summary> Add to Where Clause of aSQL based on form controls' _QueryDef property </summary>
        Function BuildQuery(ByVal aSQL As String) As String

            'Initialize iSQLDescr
            iSQLDescr = ""
            GetQueryString(Me, aSQL)

            Return aSQL

        End Function

        ''' <summary> If control is a query control, return true after setting aType.
        '''   Note, checkbox has no automatic logic as a query control because of being only two states </summary>
        Function isQueryControl(ByVal aC As Control, ByRef aType As String) As Boolean

            aType = ""

            If aC.Name.ToString.StartsWith("q_") Then
                If TypeOf (aC) Is TextBox Then aType = "textbox"
                If TypeOf (aC) Is ComboBox Then aType = "combobox"
                If TypeOf (aC) Is qTextBox Then aType = "qtextbox"
                If TypeOf (aC) Is qComboBox Then aType = "qcombobox"
                If TypeOf (aC) Is QSILib.qDD Then aType = "qdd"
                If TypeOf (aC) Is qMaskedTextBox Then aType = "qmaskedtextbox"
                If TypeOf (aC) Is qCBMultiCol Then aType = "qcbmulticol"

                Return True
            End If

            If TypeOf (aC) Is qTextBox Then
                aType = "qtextbox"
                Return CType(aC, qTextBox)._QueryDef > " "   'True if QueryDef is entered
            End If

            If TypeOf (aC) Is qMaskedTextBox Then
                aType = "qmaskedtextbox"
                Return CType(aC, qMaskedTextBox)._QueryDef > " "   'True if QueryDef is entered
            End If

            'BHS 2/9/10 From Oakland
            If TypeOf (aC) Is QSILib.qDD Then
                aType = "qdd"
                Return CType(aC, QSILib.qDD)._QueryDef > " "   'True if QueryDef is entered
            End If

            If TypeOf (aC) Is qComboBox Then
                aType = "qcombobox"
                Return CType(aC, qComboBox)._QueryDef > " "   'True if QueryDef is entered
            End If


            If TypeOf (aC) Is qCBMultiCol Then
                aType = "qcbmulticol"
                Return CType(aC, qCBMultiCol)._QueryDef > " "   'True if QueryDef is entered
            End If

            If TypeOf (aC) Is qRC Then
                aType = "qrc"
                Return CType(aC, qRC)._QueryDef > " "
            End If

            Return False
        End Function

        ''' <summary> Call OnBackgroundQueryComplete event </summary>
        Function BackgroundQueryComplete(ByVal aDV As DataView) As Boolean
            Try
                RaiseEvent OnBackgroundQueryComplete(aDV)
            Catch ex As Exception
                ShowError("Unexpected error completing background query", ex)
            End Try

        End Function

        ''' <summary> Call OnBackgroundQueryCancelled event </summary>
        Function BackgroundQueryCancelled() As Boolean
            Try
                RaiseEvent OnBackgroundQueryCancelled()
            Catch ex As Exception
                ShowError("Unexpected error cancelling a background query", ex)
            End Try

        End Function

        '''' <summary> Clear all of the query text controls'  </summary>
        'Function ClearQueryControl() As Boolean
        '    ClearQueryControl(Me)
        '    Return True
        'End Function

        ''' <summary> Clears all query controls'   </summary>
        Sub ClearQueryControls(ByVal StartControl As Control, Optional ByVal aQueryControls As Boolean = True)
            Dim C, Childc As Control
            Dim Type As String = ""

            If iAutoClear Then
                'For Each C In StartControl.Controls
                C = StartControl
                If aQueryControls Then
                    If C.Name.Length > 1 Then
                        If isQueryControl(C, Type) Then
                            Dim Ch As qCheckBox = TryCast(C, qCheckBox) '8/6/2010 SRM if type is qcheckbox then uncheck
                            If Ch IsNot Nothing Then
                                Ch.Checked = False
                            ElseIf Type = "qRC" Then
                                Dim RC As qRC = TryCast(C, qRC)
                                If RC IsNot Nothing Then RC.Clear()
                            ElseIf Type = "qcombobox" Then ' GBV 2/5/2011
                                Dim CB As qComboBox = TryCast(C, qComboBox)
                                If CB IsNot Nothing Then
                                    If CB.DropDownStyle = ComboBoxStyle.DropDownList Then
                                        If CB.Items.Count > 0 Then
                                            CB.SelectedIndex = 0
                                        End If
                                    Else
                                        CB.Text = ""
                                    End If
                                End If
                            ElseIf Type = "qcbmulticol" Then ' GBV 2/5/2011
                                Dim CB As qCBMultiCol = TryCast(C, qCBMultiCol)
                                If CB IsNot Nothing Then
                                    If CB.DropDownStyle = ComboBoxStyle.DropDownList Then
                                        If CB.Items.Count > 0 Then
                                            CB.SelectedIndex = 0
                                        End If
                                    Else
                                        CB.Text = ""
                                    End If
                                End If
                            ElseIf Type = "qdd" Then ' GBV 2/7/2011
                                Dim DD As qDD = TryCast(C, qDD)
                                If DD IsNot Nothing Then
                                    If DD._MustMatchList = True Then
                                        If DD.RowCount > 0 Then
                                            DD.SelectedIndex = 0
                                        End If
                                    Else
                                        C.Text = ""
                                    End If
                                End If
                            Else
                                C.Text = ""
                            End If
                        End If
                    End If

                Else
                    Dim T As qTextBox = TryCast(C, qTextBox)
                    If T IsNot Nothing Then
                        T.Text = ""
                    End If

                    Dim MT As qMaskedTextBox = TryCast(C, qMaskedTextBox)
                    If MT IsNot Nothing Then
                        MT.Text = ""
                    End If

                    Dim CB As qComboBox = TryCast(C, qComboBox)
                    If CB IsNot Nothing Then
                        CB.Text = ""
                    End If

                    Dim DD As qDD = TryCast(C, qDD)
                    If DD IsNot Nothing Then
                        DD.Text = ""
                    End If

                    Dim CHBX As CheckBox = TryCast(C, CheckBox)
                    If CHBX IsNot Nothing Then
                        CHBX.Checked = False
                    End If

                    Dim QCHBX As qCheckBox = TryCast(C, qCheckBox)
                    If QCHBX IsNot Nothing Then
                        QCHBX.Checked = False
                    End If

                    Dim RC As qRC = TryCast(C, qRC)
                    If RC IsNot Nothing Then
                        RC.Clear()
                    End If

                    Dim LB As ListBox = TryCast(C, ListBox)
                    If LB IsNot Nothing Then
                        LB.Text = ""
                    End If

                    Dim CBM As qCBMultiCol = TryCast(C, qCBMultiCol)
                    If CBM IsNot Nothing Then
                        CBM.Text = ""
                    End If
                End If

                For Each Childc In C.Controls
                    ClearQueryControls(Childc, aQueryControls)
                Next

            End If
        End Sub

        ''' <summary> Open Edit Form From List.  Overridable. </summary>
        Public Overridable Function OpenEditFormFromList(ByVal aFormName As String, ByVal aKey As String, Optional ByVal aGV As DataGridView = Nothing) As Boolean
            Dim frm As fBase
            If aFormName.Length < 1 Then
                ProgrammerErr("Must supply form name (fBase.OpenEditFormFromList)")
                Return False
            End If
            'If aGV Is Nothing And aUG Is Nothing Then
            '    ProgrammerErr("Must supply either aGV or uGV (fBase.OpenEditFormFromList)")
            '    Return False
            'End If
            frm = GetEditForm(aFormName)
            If frm Is Nothing Then
                ProgrammerErr("FormName doesn't match an object in the application (fBase.OpenEditFormFromList)")
                Return False
            End If

            frm.iKey = aKey
            frm.iListForm = Me
            If aGV IsNot Nothing Then
                frm.iListGV = aGV
                'ElseIf aUG IsNot Nothing Then
                '    frm.iListUG = aUG
                'Else
                '    frm.iListDR = aDR
            End If
            frm.MdiParent = Me.MdiParent()

            frm.Show()

            Return True

        End Function

        ''' <summary> Assign a DS Table to a WithEvents DataTable, and start checking for changes </summary>
        Sub StartDirtyTracking(ByVal aDSTable As DataTable, ByRef aDataTable As DataTable)
            aDataTable = aDSTable
            iDirtyTables.Add(aDataTable)
        End Sub

        ''' <summary> Present an Error Message without logging, translation, etc. </summary>
        Sub MsgBoxErr(ByVal aMsg As String, Optional ByVal aDetail As String = "", Optional ByVal aTitle As String = "")
            If gIsFromCron Then Return 'GBV 11/14/2014
            If aDetail.Length > 0 Then
                aMsg = aMsg + Chr(13) + "-----------------------------------------------------------------------" + Chr(13) + Chr(13) + aDetail
            End If
            If aTitle.Length = 0 Then aTitle = Me.Text
            MsgBox(aMsg, MsgBoxStyle.Exclamation, aTitle)
        End Sub

        ''' <summary> Present an Information Message without logging, translation, etc. </summary>
        Sub MsgBoxInfo(ByVal aMsg As String, Optional ByVal aDetail As String = "", Optional ByVal aTitle As String = "")
            If gIsFromCron Then Return ' GBV 11/14/2014
            If aDetail.Length > 0 Then
                aMsg = aMsg + Chr(13) + "-----------------------------------------------------------------------" + Chr(13) + Chr(13) + aDetail
            End If
            If aTitle.Length = 0 Then aTitle = Me.Text
            MsgBox(aMsg, MsgBoxStyle.Information, aTitle)
        End Sub

        ''' <summary> Present a Question Message Box </summary>
        Function MsgBoxQuestion(ByVal aMsg As String, Optional ByVal aDetail As String = "", Optional ByVal aTitle As String = "") As MsgBoxResult
            If gIsFromCron Then Return Nothing ' GBV 11/14/2014
            If aDetail.Length > 0 Then
                aMsg = aMsg + Chr(13) + "-----------------------------------------------------------------------" + Chr(13) + Chr(13) + aDetail
            End If
            If aTitle.Length = 0 Then aTitle = Me.Text
            Return MsgBox(aMsg, MsgBoxStyle.YesNo, aTitle)
        End Function

        ''' <summary> Raise Event OnTab </summary>
        Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, _
                                                   ByVal keyData As System.Windows.Forms.Keys) As Boolean
            Dim CancelCmd As Boolean = False
            If keyData = Keys.Tab Then RaiseEvent OnTab(msg, keyData, CancelCmd)
            If CancelCmd = False Then
                Return MyBase.ProcessCmdKey(msg, keyData)
            End If
            Return True

        End Function

        ''' <summary> Make flMain.find1 callable from feMain </summary>
        Public Overridable Sub find1()

        End Sub

#End Region

#Region "-------------------- Save Report and Query Parameters ------------------------------"

        ''' <summary> Save all Single-Entry Control values to tRpt and tRptCol </summary>
        Function UpdateQueryVersion() As Boolean
            Dim SQL As String = ""
            Dim WhereClause As String = " Where classname = '" & GetObjName() & "'" & _
               " And title = 'Query' And savedby = '" & gUserName & "'"

            'Add a tRpt record if one doesn't exist
            SQL = "Select count(*) From tRpt " & WhereClause
            If SQLGetNumber(SQL) = 0 Then
                SQL = " Insert Into tRpt " & _
                    " (classname, title, savedby, sortno) Values " & _
                    " ('" & GetObjName() & "', 'Query', '" & gUserName & "', 1)"
            End If
            SQLDoSQL(SQL, True) 'Force write, even to Live Database

            'Add a record for each control
            SQLDoSQL("Delete from tRptCol " & WhereClause, True)    'Force write to Live
            ControlToRptCol(Me, "Query")

        End Function

        ''' <summary> Recursively move Single-Entry Control values to tRptCol </summary>
        Sub ControlToRptCol(ByVal aC As Control, ByVal aTitle As String, Optional ByVal aClassname As String = "")
            Dim Val As String = ""
            '...aClassname may be passed from feSaveQuery; else, retrieve it here
            Dim Classname As String = aClassname
            If Classname = "" Then Classname = GetObjName()
            For Each C As Control In aC.Controls
                If isSingleEntryControl(C) AndAlso C.Name > "" AndAlso C.Name <> "cbVersion" Then   'BHS 8/7/08 <> cbVersion
                    Val = PrepareSQLSearchString(C.Text)
                    If TypeOf C Is qRC Then
                        Dim RC As qRC = CType(C, qRC)
                        Val = PrepareSQLSearchString(RC._DBText)
                    End If
                    If TypeOf C Is qCheckBox Then   'BHS 8/15/08
                        Dim CH As qCheckBox = CType(C, qCheckBox)
                        Val = "0"
                        If CH.Checked = True Then Val = "1"
                    End If
                    If TypeOf C Is RadioButton Then ' GBV 10/26/2009
                        Dim RB As RadioButton = CType(C, RadioButton)
                        Val = "0"
                        If RB.Checked Then Val = "1"
                    End If
                    ''BHS 8/8/8  DropDownList ComboBoxes store their value, not their text
                    'If TypeOf (C) Is ComboBox Then
                    '    Dim cb As ComboBox = CType(C, ComboBox)
                    '    If cb.DropDownStyle = ComboBoxStyle.DropDownList Then
                    '        If cb.SelectedIndex > -1 Then
                    '            Val = cb.SelectedValue.ToString
                    '        Else
                    '            Val = ""
                    '        End If
                    '    End If
                    'End If
                    SQLDoSQL("Insert Into tRptCol (classname, title, savedby, columnname, columnvalue) " & _
                       " Values ('" & Classname & "', '" & aTitle & "', '" & gUserName & "', '" & _
                       C.Name & "', '" & Val & "')", True)  'Force Write to Live
                End If

                'BHS 2/9/10 Don't drill down into the sub-controls of a qDD
                If Not TypeOf (C) Is QSILib.qDD Then
                    ControlToRptCol(C, aTitle, Classname)  'Recursive
                End If

            Next
        End Sub

        ''' <summary> Recursively move Single-Entry Control values to tQBE_Columns </summary>
        Sub ControlToQBEColumns(ByVal aC As Control, ByVal aQBERecno As Integer)
            Dim Val As String = ""

            For Each C As Control In aC.Controls
                If isSingleEntryControl(C) AndAlso C.Name > "" AndAlso C.Name <> "cbVersion" Then   'BHS 8/7/08 <> cbVersion
                    Val = PrepareSQLSearchString(C.Text)
                    If TypeOf C Is qRC Then
                        Dim RC As qRC = CType(C, qRC)
                        Val = PrepareSQLSearchString(RC._DBText)
                    End If
                    If TypeOf C Is qCheckBox Then   'BHS 8/15/08
                        Dim CH As qCheckBox = CType(C, qCheckBox)
                        Val = "0"
                        If CH.Checked = True Then Val = "1"
                    End If
                    If TypeOf C Is RadioButton Then ' GBV 10/26/2009
                        Dim RB As RadioButton = CType(C, RadioButton)
                        Val = "0"
                        If RB.Checked Then Val = "1"
                    End If
                    'NOTE: We save records for every column so that loading a version will replace all QBE fields on the screen
                    ' GBV 4/21/2015 - Ticket 3666 - moved all tQBE_xxx tables to IASCommon database
                    SQLDoSQL("Insert Into IASCommon..tQBE_Columns (QBERecno, ColumnName, ColumnValue) " & _
                       " Values (" & aQBERecno.ToString & ", '" & C.Name & "', '" & Val & "')", True)  'Force Write to Live
                End If

                'BHS 2/9/10 Don't drill down into the sub-controls of a qDD
                If Not TypeOf (C) Is QSILib.qDD Then
                    ControlToQBEColumns(C, aQBERecno)  'Recursive
                End If

            Next
        End Sub

        ''' <summary> Fill all controls based on cbVersion.SelectedValue </summary>
        Sub FillQBEDefaults(Optional ByVal aTitle As String = "Query", Optional ByVal aSavedBy As String = "")
            'If fBase.ActiveForm IsNot Nothing Then
            If Not InDevEnv() Then
                'BHS 5/2/08 Don't need to consider other connections because tRptCol is only in SQL Server
                'BHS 8/7/08 <> cbVersion
                'SDC 4/14/2014 aTitle may have the name of the default version
                If aSavedBy = "" Then aSavedBy = gUserName
                Dim DV As DataView = SQLBuildDV("Select * From tRptCol Where classname = '" & _
                  GetObjName() & "'" & " And title = '" & aTitle & "' And savedby = '" & aSavedBy & _
                  "' And ColumnName <> 'cbVersion'", gSQLConnStr)
                FillControlsFromtRptColDV(DV)
            End If
        End Sub

        ''' <summary> Fill all controls based on selected QBE record number </summary>
        Sub FillQBEDefaults_ByRecno(ByVal aQBERecno As Integer)
            If Not InDevEnv() Then
                ' GBV 4/21/2015 - Ticket 3666 - moved all tQBE_xxx tables to IASCommon database
                Dim dv As DataView = SQLBuildDV("Select * From IASCommon..tQBE_Columns Where QBERecno = " & aQBERecno.ToString & _
                                                " And ColumnName <> 'cbVersion'", gSQLConnStr)
                FillControlsFromtRptColDV(dv)
            End If
        End Sub

        'BHS 8/8/8 Called from FillQBEDefaults and FillControlsFromVersion
        ''' <summary> Fill Controls from RptCol DV </summary>
        Sub FillControlsFromtRptColDV(ByVal aDV As DataView)
            Dim C As Control
            For Each R As DataRow In aDV.Table.Rows
                C = FindControl(R.Item("columnname").ToString, Me)
                If Not IsNothing(C) Then
                    If TypeOf (C) Is ComboBox Then
                        Dim cb As ComboBox = CType(C, ComboBox)
                        Dim i As Integer = cb.FindStringExact(R.Item("columnvalue").ToString)
                        If i > -1 Then
                            cb.SelectedIndex = i
                        Else
                            If cb.DropDownStyle = ComboBoxStyle.DropDownList Then
                                cb.Text = ""
                            Else
                                cb.Text = R.Item("columnvalue").ToString
                            End If
                        End If
                    ElseIf TypeOf C Is qRC Then
                        Dim RC As qRC = CType(C, qRC)
                        RC._DBText = R.Item("columnvalue").ToString
                    ElseIf TypeOf C Is qCheckBox Then   'BHS 8/15/08
                        Dim Ch As qCheckBox = CType(C, qCheckBox)
                        Ch.Checked = R.Item("columnvalue").ToString = "1"
                    ElseIf TypeOf C Is RadioButton Then ' GBV 10/26/2009
                        Dim RB As RadioButton = CType(C, RadioButton)
                        RB.Checked = R.Item("columnvalue").ToString = "1"
                    ElseIf TypeOf C Is QSILib.qDD Then
                        Dim dd As QSILib.qDD = CType(C, QSILib.qDD)
                        dd.TextInfo = R.Item("columnvalue").ToString

                        Dim i As Integer = dd.GetGVRowIndexLike(dd.TextInfo, True)
                        If i > -1 Then
                            dd.SelectedIndex = i
                        Else
                            'SRM 10/04/2012 - if value is not in the drop down list and it is mustmmatch, blank out the text
                            ' GBV 6/2/2015 - Added check to gIsFromCron
                            If dd._MustMatchList = True AndAlso Not gIsFromCron Then dd.Text = ""
                        End If

                    Else    'Not a ComboBox
                        C.Text = R.Item("columnvalue").ToString
                    End If
                End If
            Next
        End Sub

        ''' <summary> Return form object name, no longer than 24 characters </summary>
        Function GetObjName() As String
            Return Mid(Me.Name, 1, 24)
        End Function

        ''' <summary> Save all iGVs Column order and width to tRpt and tRptCol </summary>
        Function UpdateLayoutVersion() As Boolean
            Dim SQL As String = ""
            Dim WhereClause As String = " Where classname = '" & GetObjName() & "'" & _
               " And (title = 'Layout' Or title = 'GV Sort') And savedby = '" & gUserName & "'"

            'Add a tRpt record if one doesn't exist
            SQL = "Select count(*) From tRpt " & WhereClause
            If SQLGetNumber(SQL) = 0 Then
                SQL = " Insert Into tRpt " & _
                    " (classname, title, savedby, sortno) Values " & _
                    " ('" & GetObjName() & "', 'Layout', '" & gUserName & "', 1)"
            End If
            SQLDoSQL(SQL, True) 'Force write, even to Live Database

            'Add a record for each control
            SQLDoSQL("Delete from tRptCol " & WhereClause, True)    'Force write to Live

            Dim Nam As String = ""
            Dim Val As String = ""

            For i = 0 To iGVs.Count - 1
                Dim GV As DataGridView = CType(iGVs(i), DataGridView)
                For Each C As DataGridViewColumn In GV.Columns
                    If C.Visible = False Then Continue For
                    Nam = GV.Name & "|" & C.Name
                    Val = C.DisplayIndex.ToString("00") & "|" & C.Width.ToString
                    SQLDoSQL("Insert Into tRptCol (classname, title, savedby, columnname, columnvalue) " & _
                      " Values ('" & GetObjName() & "', 'Layout', '" & gUserName & "', '" & _
                      Nam & "', '" & Val & "')", True)  'Force Write to Live
                Next
                If GV.SortedColumn IsNot Nothing Then
                    Nam = GV.Name & "|" & GV.SortedColumn.Name
                    Val = "Ascending"
                    If GV.SortOrder.ToString.IndexOf("Descending") > -1 Then Val = "Descending"
                    SQLDoSQL("Insert Into tRptCol (classname, title, savedby, columnname, columnvalue) " & _
                      " Values ('" & GetObjName() & "', 'GV Sort', '" & gUserName & "', '" & _
                      Nam & "', '" & Val & "')", True)  'Force Write to Live
                End If
                'BHS 8/10/10 Also save form size  8/18 moved to ResizeEnd
                'Nam = "DefltFormSize"
                'Val = Me.Height.ToString & "|" & Me.Width.ToString
                'SQLDoSQL("Insert Into tRptCol (classname, title, savedby, columnname, columnvalue) " & _
                '      " Values ('" & GetObjName() & "', 'Layout', '" & gUserName & "', '" & _
                '      Nam & "', '" & Val & "')", True)
            Next

        End Function

        ''' <summary> Set GV Layout from RptCol DV </summary>
        Sub SetGVLayout()

            If Not InDevEnv() Then
                Dim DV As DataView = SQLBuildDV("Select * From tRptCol Where classname = '" & _
                  GetObjName() & "'" & " And (title = 'Layout' or title = 'GV Sort') " & _
                  " And savedby = '" & gUserName & "'" & _
                  " Order By ColumnValue", gSQLConnStr)

                Dim GV As DataGridView
                Dim GVName, ColName As String
                Dim Ord, Wid As String

                For Each R As DataRow In DV.Table.Rows
                    ColName = R.Item("columnName").ToString
                    GVName = ParseStr(ColName, "|")
                    Wid = R.Item("columnValue").ToString
                    Ord = ParseStr(Wid, "|")

                    'BHS 8/10/10  Set form size if ColName = "DefltFormSize"  BHS 8/24/10 Moved to fbase::ResizeEnd
                    'If GVName = "DefltFormSize" Then
                    '    'Dim H As String = ParseStr(Wid, "|")
                    '    Dim S As New Size(CInt(Wid), CInt(Ord))
                    '    Me.Size = S
                    '    Continue For
                    'End If

                    For i = 0 To iGVs.Count - 1
                        If CType(iGVs(i), DataGridView).Name = GVName Then
                            GV = CType(iGVs(i), DataGridView)
                            'BHS 8/18/10
                            'BHS R5.1.12 12/20/11 Ignore problems that might be caused by non-data-bound columns
                            Try

                                If Ord = "Ascending" Then
                                    Dim qGV As qGVBase = TryCast(GV, qGVBase)
                                    If GV.Columns(ColName) IsNot Nothing Then
                                        GV.Sort(GV.Columns(ColName), System.ComponentModel.ListSortDirection.Ascending)
                                        If qGV IsNot Nothing Then
                                            qGV.iSortColumn = GV.Columns(ColName)
                                            qGV.iSortDirection = System.ComponentModel.ListSortDirection.Ascending
                                        End If
                                    End If
                                    Continue For
                                End If
                                If Ord = "Descending" Then
                                    Dim qGV As qGVBase = TryCast(GV, qGVBase)
                                    If GV.Columns(ColName) IsNot Nothing Then
                                        GV.Sort(GV.Columns(ColName), System.ComponentModel.ListSortDirection.Descending)
                                        If qGV IsNot Nothing Then
                                            qGV.iSortColumn = GV.Columns(ColName)
                                            qGV.iSortDirection = System.ComponentModel.ListSortDirection.Descending
                                        End If
                                    End If
                                    Continue For
                                End If

                            Catch ex As Exception
                                'Ignore any problems, such as those caused by trying to sort non-data-bound columns
                            End Try

                            Try
                                If GV.Columns.Count > 0 Then
                                    If GV.Columns(ColName) IsNot Nothing Then
                                        If GV.Columns.Item(ColName) IsNot Nothing And CInt(Ord) < GV.Columns.Count Then
                                            GV.Columns.Item(ColName).DisplayIndex = CInt(Ord)
                                            GV.Columns.Item(ColName).Width = CInt(Wid)
                                        End If
                                    End If

                                End If
                            Catch ex As Exception
                                'Ignore any problems, such as those caused by trying to sort non-data-bound columns
                            End Try
                        End If
                    Next
                Next

            End If
        End Sub

#End Region

#Region "-------------------- Validation ------------------------------"

        'Form Validation (everything after this is about control validation)
        'This is called just before saving form
        'BHS 3/26/08 Call OnValidateForm after checking that all required fields are entered
        ''' <summary> Call OnValidateForm and handle possible errors </summary>
        Function ValidateForm() As Boolean
            Dim ErrorText As String = ""
            Dim Sender As Object = New Object

            'BHS 2/7/08 Check all fields for _ValidateRequired
            Dim ReqField As String = CheckRequiredFields(Me)
            If ReqField.Length = 0 Then
                'BHS 3/26/08 No required field problem, so run OnValidateForm   
                Try
                    RaiseEvent OnValidateForm(Sender, ErrorText)
                Catch ex As Exception
                    ShowError("Unexpected error validating form (OnValidateForm)", ex)
                End Try

                'No problems - Clear all error indicators and return true
                If ErrorText = "" Then
                    For Each GV As DataGridView In iGVs
                        iEP.SetError(GV, "")
                    Next

                    'BHS 3/9/9 Clear errors on all controls
                    ClearErrors(Me) 'Clear all controls, recursively    'BHS 11/3/09

                    ShowStatus("")
                    iOKToClose = True
                    Return True
                End If
                'Otherwise, drop down for regular error display logic

            Else  'Required field problem - set Sender to control so error will show next to it
                Sender = TryCast(FindControl(ReqField, Me), Control)
                If Sender Is Nothing Then   'This would only fire if there is a bug in the library
                    MsgBoxErr("Programmer Error", "Library error: Sender not set in library for required field: " & ReqField & " (fBase.ValidateForm)")
                    Return False
                End If

                ErrorText = "Required Field"

                'BHS 5/24/13 show more information if we can
                Dim qT = TryCast(Sender, qTextBox)
                If qT IsNot Nothing Then
                    If qT._QueryDescr.Length > 0 Then
                        ErrorText = qT._QueryDescr & " - required field"
                    End If
                End If
                Dim qDD = TryCast(Sender, qDD)
                If qDD IsNot Nothing Then
                    If qDD._QueryDescr.Length > 0 Then
                        ErrorText = qDD._QueryDescr & " - required field"
                    End If
                End If
                Dim qCB = TryCast(Sender, qComboBox)
                If qCB IsNot Nothing Then
                    If qCB._QueryDescr.Length > 0 Then
                        ErrorText = qCB._QueryDescr & " - required field"
                    End If
                End If

            End If

            If Sender Is Nothing Then   'Warn the programmer that OnValidate didn't return a Sender
                MsgBoxErr("Programmer Error", "You must set Sender in OnValidate Form when returning an error (fBase.ValidateForm)")
            Else
                Dim C As Control = TryCast(Sender, Control)
                If C Is Nothing Then
                    ShowStatus("")
                    MsgBoxErr(ErrorText)
                Else
                    ShowControlError(Sender, ErrorText)
                End If
            End If

            Return False

        End Function

        'BHS 11/3/09 Recursively clear errors from all controls, including child controls
        Sub ClearErrors(ByVal aC As Control)
            For Each C As Control In aC.Controls
                ClearErrors(C)
                iEP.SetError(C, "")
            Next
        End Sub

        'BHS 2/7/08
        ''' <summary> Recursively check all controls to make sure required fields are entered
        '''   Checkboxes cannot be required </summary>
        Function CheckRequiredFields(ByVal aC As Control) As String
            Dim qT As qTextBox = TryCast(aC, qTextBox)
            If qT IsNot Nothing Then
                If qT._ValidateRequired = True Then
                    If qT.Text.Length = 0 Then Return qT.Name
                End If
            End If

            'BHS 2/9/10 From Oakland
            Dim DD As QSILib.qDD = TryCast(aC, QSILib.qDD)
            If DD IsNot Nothing Then
                If DD._ValidateRequired = True Then
                    If DD.Text.Length = 0 Then Return DD.Name
                End If
            End If

            Dim qCB As qComboBox = TryCast(aC, qComboBox)
            If qCB IsNot Nothing Then
                If qCB._ValidateRequired = True Then
                    If qCB.Text.Length = 0 Then Return qCB.Name
                End If
            End If

            Dim qMT As qMaskedTextBox = TryCast(aC, qMaskedTextBox)
            If qMT IsNot Nothing Then
                If qMT._ValidateRequired = True Then
                    If qMT.Text.Length = 0 Then Return qMT.Name
                    Dim wstr As String = Replace(qMT.Text, "-", "").Trim
                    If wstr = "" Then Return qMT.Name
                End If
            End If

            For Each C As Control In aC.Controls
                Dim S As String = CheckRequiredFields(C)
                If S.Length > 0 Then Return S
            Next
            Return ""
        End Function

        ''' <summary> Returns True if good (no dupe key), False if dupe found.
        ''' Dependencies - aGV.Tag or aGV._KeyFields must name the key columns </summary>
        Overridable Function GVDupeKeyCheck(ByVal aGV As DataGridView) As Boolean

            Dim R, R2 As DataGridViewRow
            Dim K, K2 As String
            Dim KeyFields As String = aGV.Tag.ToString

            If TypeOf (aGV) Is qGVBase Then KeyFields = CType(aGV, qGVBase)._KeyFields

            If KeyFields > "" Then
            Else
                MsgBoxErr("Programmer Error", "aGV tag or qGVBase._KeyFields is missing")
                Return False
            End If

            For Each R In aGV.Rows
                K = BuildKey(KeyFields, R)
                For Each R2 In aGV.Rows
                    If R.Index <> R2.Index Then
                        K2 = BuildKey(KeyFields, R2)
                        If K = K2 Then
                            Return False    'Match found, dupecheck fails
                        End If
                    End If
                Next
            Next

            Return True

        End Function

        ''' <summary> Return iKey string based on aKey, which names the fields, and a GV Row </summary>
        Function BuildKey(ByVal aKey As String, ByVal aR As DataGridViewRow) As String
            Dim Part As String
            Dim Value As String = ""

            Part = ParseStr(aKey, "|")

            While Part > " "
                Value = Value & aR.Cells(Part).Value.ToString.Trim.ToLower + "|"
                Part = ParseStr(aKey, "|")
            End While

            Return Mid(Value, 1, Value.Length - 1)

        End Function

        '------------------------- CONTROL VALIDATION SETUP

        ''' <summary> Validating Event kicks off KeyField checking and ValidateControl function </summary>
        Friend Sub EntryControl_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
            Dim C As Control
            Dim Match As Boolean = False
            Dim CheckForMatch As Boolean = True
            Dim KeyField As Boolean = False

            'If New Mode and key fields match, sync to list and reload the form 
            For Each C In iKeyFields
                If C.Equals(sender) And iIsNew Then KeyField = True
                If KeyField AndAlso iCheckEveryKeyField Then ' GBV 4/19/2011
                    If C.Text <> "" Then
                        CheckForMatch = True
                        Exit For
                    Else
                        CheckForMatch = False
                    End If
                ElseIf Not iCheckEveryKeyField Then
                    If C.Text Is Nothing Or C.Text = "" Then CheckForMatch = False
                End If
            Next

            If KeyField And CheckForMatch Then
                Try
                    RaiseEvent OnKeyFieldEntered(Match)
                Catch ex As Exception
                    ShowError("Unexpected error managing key field entry (OnKeyFieldEntered)", ex)
                End Try

                If Match Then
                    iIsNew = False
                    iMode = "edit"
                    If Not iListForm Is Nothing Then iListForm.SelectListRow(iKey)
                    LoadForm()
                End If
            End If

            C = CType(sender, Control)

            'Run main validation logic in child
            ValidateControl(sender, e)

        End Sub

        ''' <summary> Validated Event clears any error and status messages </summary>
        Friend Sub EntryControl_Validated(ByVal sender As Object, ByVal e As System.EventArgs)
            If TypeOf (sender) Is TextBox Or TypeOf (sender) Is ComboBox Or TypeOf (sender) Is qTextBox Or TypeOf (sender) Is qComboBox Or TypeOf (sender) Is qMaskedTextBox Or TypeOf (sender) Is qCheckBox Or TypeOf (sender) Is QSILib.qDD Or TypeOf (sender) Is QSILib.qRC Then
                Dim C As Control = CType(sender, Control)

                iEP.SetError(C, "")
                ShowStatus("")
                iOKToClose = True
                iLastEF = C

            End If

            'BHS 2/9/09  Refresh Tab Count, if appropriate
            For Each C As Control In Me.Controls
                Dim T As qTab = TryCast(C, qTab)
                If T IsNot Nothing Then T.RefreshPages()
            Next

        End Sub

        'TODO Compare this logic to Parsing and Formating of single-entry controls.  We can probably come up with a more general solution that includes numbers and all date formats.
        'BHS 8/30/07  (bug fixed by SDC 9/10/2007)
        ''' <summary> GV Cell Formatting </summary>
        Private Sub gvCellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs)

            '-----------
            'BHS DEBUG TEST 9/11/13
            'DAVID - TRY RUNNING THIS IN DEBUGGER TO SEE IF THE SENDER IS THE GV, and if the GV has focus.  I'm thinking it shouldn't if you're moving between QBE fields.
            Dim GV As DataGridView = TryCast(sender, DataGridView)
            If GV IsNot Nothing Then
                If GV.Focused = False Then Return
            End If
            '-----------------


            If e.CellStyle.Format = "MM/dd/yyyy" And e.Value IsNot Nothing Then
                If e.Value.ToString.Length > 0 Then e.Value = Format(e.Value, "MM/dd/yyyy")
                If e.Value.ToString = "01/01/0001" Then e.Value = ""
            End If
            If e.CellStyle.Format = "MM/dd/yy" And e.Value IsNot Nothing Then
                If e.Value.ToString.Length > 0 Then e.Value = Format(e.Value, "MM/dd/yy")
                If e.Value.ToString = "01/01/01" Then e.Value = ""
            End If
        End Sub

        ''' <summary> GV Cell Validating event kicks off ValidateGridControl function </summary>
        Private Sub gvCellValidating(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellValidatingEventArgs)
            Dim GV As DataGridView = CType(sender, DataGridView)

            'Don't validate when setting up GV, only when it is in focus
            If GV.ContainsFocus = True Then ValidateGridControl(sender, e)

        End Sub

        ''' <summary> GV Cell End Edit event clears any error and status messages </summary>
        Private Sub gvCellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
            Dim GV As DataGridView = CType(sender, DataGridView)

            GV.Rows(e.RowIndex).ErrorText = ""
            ShowStatus("")

        End Sub

        'BHS 5/6/11
        ''' <summary> Explicitly set iCurrentControlName and iOrigValue </summary>
        Sub SetiOrigValue(ByVal aControl As Control)
            iCurrentControlName = aControl.Name
            iOrigValue = aControl.Text
            If TypeOf (aControl) Is qCheckBox Then
                Dim qCB As qCheckBox = CType(aControl, qCheckBox)
                If qCB.Checked = True Then
                    If qCB._CheckedTrueValue.Length > 0 Then    'BHS 1/11/10
                        iOrigValue = qCB._CheckedTrueValue
                    Else
                        iOrigValue = "1"
                    End If
                Else
                    If qCB._CheckedFalseValue.Length > 0 Then
                        iOrigValue = qCB._CheckedFalseValue
                    Else
                        iOrigValue = "0"
                    End If
                End If
            End If
            If TypeOf (aControl) Is qRC Then
                Dim RC As qRC = CType(aControl, qRC)
                iOrigValue = RC._DBText
            End If
        End Sub
        ''' <summary> Validated Event clears any error and status messages </summary>
        Friend Sub EntryControl_Enter(ByVal sender As Object, ByVal e As System.EventArgs)

            'Show tooltip in ShowHelp
            If TypeOf (sender) Is TextBox Or TypeOf (sender) Is ComboBox Or TypeOf (sender) Is qTextBox Or TypeOf (sender) Is qComboBox Or TypeOf (sender) Is qMaskedTextBox Or TypeOf (sender) Is qCheckBox Or TypeOf (sender) Is qRC Or TypeOf (sender) Is qDateTimePicker Or TypeOf (sender) Is QSILib.qDD Then
                Dim C As Control = CType(sender, Control)
                iCurrentControlName = C.Name
                iOrigValue = C.Text 'BHS 1/11/10    'For comparison in OnValidateControl
                If TypeOf (sender) Is qRC Then
                    Dim RC As qRC = CType(sender, qRC)
                    iOrigValue = RC._DBText
                End If
                Dim Str As String = ""
                If Not IsNothing(C.Tag) Then
                    If TypeOf (C.Tag) Is String Then
                        Str = C.Tag.ToString
                        If Mid(Str, 1, 4).ToLower = "tip:" Then
                            ShowHelp(Mid(Str, 5))
                        End If
                    End If
                End If
                If TypeOf (C) Is qTextBox Then
                    If CType(C, qTextBox)._ToolTip > "" Then
                        ShowHelp(CType(C, qTextBox)._ToolTip)
                    End If
                End If
                If TypeOf (C) Is qMaskedTextBox Then
                    If CType(C, qMaskedTextBox)._ToolTip > "" Then
                        ShowHelp(CType(C, qMaskedTextBox)._ToolTip)
                        'BHS 10/8/09
                        CType(C, qMaskedTextBox).SelectAll()
                    End If
                End If
                'BHS 2/9/10 From Oakland
                If TypeOf (C) Is QSILib.qDD Then
                    If CType(C, QSILib.qDD)._ToolTip > "" Then
                        ShowHelp(CType(C, QSILib.qDD)._ToolTip)
                    End If
                End If

                If TypeOf (C) Is qComboBox Then
                    If CType(C, qComboBox)._ToolTip > "" Then
                        ShowHelp(CType(C, qComboBox)._ToolTip)
                    End If
                End If
                If TypeOf (C) Is qRC Then
                    Dim RC As qRC = CType(C, qRC)
                    iOrigValue = RC._DBText
                    If CType(C, qRC)._ToolTip > "" Then
                        ShowHelp(CType(C, qRC)._ToolTip)
                    End If
                End If
                If TypeOf (C) Is qCheckBox Then
                    Dim qCB As qCheckBox = CType(C, qCheckBox)
                    If qCB.Checked = True Then
                        If qCB._CheckedTrueValue.Length > 0 Then    'BHS 1/11/10
                            iOrigValue = qCB._CheckedTrueValue
                        Else
                            iOrigValue = "1"
                        End If
                    Else
                        If qCB._CheckedFalseValue.Length > 0 Then
                            iOrigValue = qCB._CheckedFalseValue
                        Else
                            iOrigValue = "0"
                        End If
                    End If
                    If CType(C, qCheckBox)._ToolTip > "" Then
                        ShowHelp(CType(C, qCheckBox)._ToolTip)
                    End If
                End If
                If TypeOf (C) Is qDateTimePicker Then
                    If CType(C, qDateTimePicker)._ToolTip > "" Then
                        ShowHelp(CType(C, qDateTimePicker)._ToolTip)
                    End If
                End If
                Post("selectall")
            End If
        End Sub

        ''' <summary> Turn off help message when we leave an entry control </summary>
        Friend Sub EntryControl_Leave(ByVal sender As Object, ByVal e As System.EventArgs)
            ShowHelp("")
        End Sub


        'BHS 2/16/10
        ''' <summary> Pass DR qDD OnDropDown event to descendant </summary>
        Friend Sub RaiseDRDDOnDropdown(ByRef aDD As qDD, ByRef aOK As Boolean)
            Try
                RaiseEvent DRDDOnDropDown(aDD, aOK)
            Catch ex As Exception
                ShowError("Unexpected error handling a datarepeater dropdown", ex)
            End Try

        End Sub

        'BHS 2/16/10
        ''' <summary> Pass DR qDD OnDropDown event to descendant </summary>
        Friend Sub RaiseDRDDOnDropdownClosed(ByRef aDD As qDD)
            Try
                RaiseEvent DRDDOnDropDownClosed(aDD)
            Catch ex As Exception
                ShowError("Unexpected error closing a datarepeater dropdown", ex)
            End Try

        End Sub

        'BHS 2/16/10

        ''' <summary> Pass DR qDD OnDropDown event to descendant </summary>
        Friend Sub RaiseDRDDSelectedIndexChanged(ByRef aDD As qDD, ByRef aOK As Boolean)
            Try
                RaiseEvent DRDDSelectedIndexChanged(aDD, aOK)
            Catch ex As Exception
                ShowError("Unexpected error on index change of datarepeater dropdown", ex)
            End Try

        End Sub

        'BHS 8/5/10
        ''' <summary> Pass DR qDD double-click event to descendant </summary>
        Friend Sub RaiseDRDDDoubleClick(ByRef aDD As qDD)
            Try
                RaiseEvent DRDDOnDoubleClick(aDD)
            Catch ex As Exception
                ShowError("Unexpected error on double click of datarepeater dropdown", ex)
            End Try

        End Sub

        ''' <summary> Show tooltip in ShowHelp upon entering gv cell </summary>
        Sub gvcellenter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
            Dim C As DataGridViewCell
            Dim GV As DataGridView = CType(sender, DataGridView)

            If iShowHelp = False Then Return

            C = GV.CurrentCell

            If C Is Nothing Then
                ShowHelp("")
            Else
                Dim i As Integer = C.ColumnIndex
                ShowHelp(CType(sender, DataGridView).Columns(i).ToolTipText)
                Return
            End If

            'Skip to next non-RO cell in GV, if one is avaiilable
            If C.ReadOnly = True Then
                Dim NextCell As DataGridViewCell = C
                While NextCell.ColumnIndex < GV.ColumnCount - 1
                    NextCell = GV.Rows(C.RowIndex).Cells(NextCell.ColumnIndex + 1)
                    If NextCell.OwningColumn.ReadOnly = False Then
                        Post("GoToNextField", NextCell)
                        Return
                    End If
                End While

                'Otherwise return user to the last GV field he was in
                Post("GoToLastField")
            End If


        End Sub

        ''' <summary> Called By Post, sets current cell based on cell passed to it </summary>
        Sub GoToNextField(ByVal aParam As Object)
            Dim C As DataGridViewCell = CType(aParam, DataGridViewCell)
            Dim GV As DataGridView = C.DataGridView
            If C.Visible = True Then GV.CurrentCell = C
        End Sub

        ''' <summary> Called By Post, sets current cell to last entry field gv cell </summary>
        Sub GoToLastField()
            iLastEF.Focus()
        End Sub

        '--------------------- HANDLE CONTROL VALIDATION EVENTS

        ''' <summary> Handle single-entry control Validating event </summary>
        Function ValidateControl(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) As Boolean
            Dim wstr As String
            Dim ErrorText As String = ""
            Dim ColName As String = ""
            Dim ColValue As String = ""
            Dim BS As BindingSource
            Dim C As Control = CType(sender, Control)
            Dim T As TextBox
            Dim qT As qTextBox
            Dim qMT As qMaskedTextBox
            Dim CB As ComboBox
            Dim XB As CheckBox
            Dim qCB As qComboBox
            Dim DP As DateTimePicker
            Dim DD As QSILib.qDD

            If TypeOf (sender) Is TextBox Then  'Includes qTextBox, etc.
                T = CType(sender, TextBox)
                If T.ReadOnly Then Return True 'SRM 09/14/10 -- for read only fields, return without any checks
                ColName = T.Name
                ColValue = T.Text
            ElseIf TypeOf (sender) Is qMaskedTextBox Then
                qMT = CType(sender, qMaskedTextBox)
                If qMT.ReadOnly Or qMT._ReadAlways Then Return True 'SRM 09/14/10 -- for read only fields, return without any checks
                ColName = qMT.Name
                ColValue = qMT.Text
            ElseIf TypeOf (sender) Is qRC Then
                Dim RC As qRC = CType(sender, qRC)
                If RC._ReadAlways = True Then Return True
                ColName = RC.Name
                ColValue = RC._DBText
            ElseIf TypeOf (sender) Is ComboBox Then 'Includes qComboBox, etc.
                CB = CType(sender, ComboBox)
                ColName = CB.Name  'Changed from Text 11/13/06
                ColValue = CB.Text
            ElseIf TypeOf (sender) Is CheckBox Then 'Includes qCheckBox
                XB = CType(sender, CheckBox)
                ColName = XB.Name
                ColValue = "0"  'BHS 8/15/08
                If XB.Checked = True Then ColValue = "1"
            ElseIf TypeOf (sender) Is DateTimePicker Then 'Includes qDateTimePicker
                DP = CType(sender, DateTimePicker)
                ColName = DP.Name
                ColValue = DP.Text
            ElseIf TypeOf (sender) Is QSILib.qDD Then  'BHS 2/9/10 From Oakland
                DD = CType(sender, QSILib.qDD)
                If DD._ReadOnly Or DD._ReadAlways Then Return True 'SRM 09/14/10 -- for read only fields, return without any checks

                ColName = DD.Name
                ColValue = DD.Text
            Else
                MsgBoxErr("Programmer Error", "Need to set control type in fBase.ValidateControl")
            End If


            'Clear any previous error
            ShowStatus(ErrorText)
            iEP.SetError(C, "")
            iOKToClose = True

            'Property-based validation fires before OnValidateControl
            If sender.GetType.Name.ToLower = "qtextbox" Then
                qT = CType(sender, qTextBox)
                If qT.ReadOnly Or qT._ReadAlways Then Return True 'SRM 09/14/10 -- for read only fields, return without any checks
                If Len(qT.Text) > 0 Then

                    If qT._ValidateDate = True Then
                        Try

                            If IsDate(qT.Text) = False Then
                                ErrorText = "Invalid date"
                                GoTo HandleError
                            End If

                            'SRM 20140314 Convert year if only 2 digits is entered
                            wstr = qT.Text
                            'First test if user entered a year
                            Dim Count As Integer = 0
                            For i As Integer = 1 To wstr.Length
                                If Not IsNumeric(Mid(wstr, i, 1)) Then Count = Count + 1
                            Next

                            If Count > 1 Then 'only convert year if use entered more than 1 seperator
                                Try
                                    For i As Integer = wstr.Length To 1 Step -1
                                        If Not IsNumeric(Mid(wstr, i, 1)) Then
                                            Dim wint As Integer = CInt(Mid(wstr, i + 1))
                                            If wint < 50 Then
                                                wstr = "20" & wint.ToString("00")
                                                wstr = Mid(qT.Text, 1, i) & wstr
                                                If IsDate(wstr) Then
                                                    qT.Text = wstr
                                                    ColValue = wstr
                                                End If

                                            ElseIf wint < 100 Then
                                                wstr = "19" & wint.ToString("00")
                                                wstr = Mid(qT.Text, 1, i) & wstr
                                                If IsDate(wstr) Then
                                                    qT.Text = wstr
                                                    ColValue = wstr
                                                End If
                                            End If
                                            Exit For
                                        End If
                                    Next
                                Catch ex As Exception
                                    'Ignore errors -- we just couldn't default the year for some reason
                                End Try
                            End If

                            If IsDate(qT.Text) = False Then
                                'IsDate thinks 11/222/14 is a vlaid date but 11/222/2014 is not
                                ErrorText = "Invalid date"
                                GoTo HandleError
                            End If

                            If IsDate(qT._ValidateMinValue) Then
                                If CDate(qT.Text) < CDate(qT._ValidateMinValue) Then
                                    ErrorText = "Minimum date allowed is " & qT._ValidateMinValue
                                    GoTo HandleError
                                End If
                            End If
                            If IsDate(qT._ValidateMaxValue) Then
                                If CDate(qT.Text) > CDate(qT._ValidateMaxValue) Then
                                    ErrorText = "Maximum date allowed is " & qT._ValidateMaxValue
                                    GoTo HandleError
                                End If
                            End If
                            'Default daterange for _ValidateDate is 1/1/1950 to 12/31/2099
                            If (qT._ValidateMinValue Is Nothing Or qT._ValidateMinValue = "") And _
                               (qT._ValidateMaxValue Is Nothing Or qT._ValidateMaxValue = "") Then
                                If CDate(qT.Text) < CDate("1/1/1950") Or CDate(qT.Text) > CDate("12/31/2099") Then
                                    ErrorText = "Date must be between 1/1/1950 and 12/31/2099"
                                    GoTo HandleError
                                End If
                            End If

                        Catch ex As Exception
                            'if any error assume it is an invalid date
                            ErrorText = "Invalid date"
                            GoTo HandleError
                        End Try
                    End If

                    'BHS 10/22/09 ADD QBE CHECKS FOR DATE AND NUMBER
                    '...

                    If qT._ValidateNumber = True Then
                        If IsNumeric(qT.Text) = False Then
                            ErrorText = "Invalid number"
                            GoTo HandleError
                        End If
                        If IsNumeric(qT._ValidateMinValue) Then
                            If CDbl(qT.Text) < CDbl(qT._ValidateMinValue) Then
                                ErrorText = "Minimum number allowed is " & qT._ValidateMinValue
                                GoTo HandleError
                            End If
                        End If
                        If IsNumeric(qT._ValidateMaxValue) Then
                            If CDbl(qT.Text) > CDbl(qT._ValidateMaxValue) Then
                                ErrorText = "Maximum number allowed is " & qT._ValidateMaxValue
                                GoTo HandleError
                            End If
                        End If
                    End If
                End If
            End If

            If sender.GetType.Name.ToLower = "qmaskedtextbox" Then
                qMT = CType(sender, qMaskedTextBox)
                'No logic here at this point
            End If

            'BHS 2/9/10 From Oakland
            If sender.GetType.Name.ToLower = "qdd" Then
                DD = CType(sender, QSILib.qDD)
                If DD._ReadOnly Or DD._ReadAlways Then Return True 'SRM 09/14/10 -- for read only fields, return without any checks

                'If DD.iInDropDown = True Then Return True 'Don't fire validation if we're still in the dropdown
                If DD.Focused = True Then Return True
                If Len(DD.Text) > 0 Then
                    If DD._ValidateDate = True Then
                        If IsDate(DD.Text) = False Then
                            ErrorText = "Invalid date"
                            GoTo HandleError
                        End If
                    End If
                    If DD._ValidateNumber = True Then
                        If IsNumeric(DD.Text) = False Then
                            ErrorText = "Invalid number"
                            GoTo HandleError
                        End If
                    End If
                End If
            End If

            If sender.GetType.Name.ToLower = "qcombobox" Then
                qCB = CType(sender, qComboBox)
                If qCB._ReadOnly Or qCB._ReadAlways Then Return True 'SRM 09/14/10 -- for read only fields, return without any checks

                If Len(qCB.Text) > 0 Then
                    If qCB._ValidateDate = True Then
                        If IsDate(qCB.Text) = False Then
                            ErrorText = "Invalid date"
                            GoTo HandleError
                        End If
                        If IsDate(qCB._ValidateMinValue) Then
                            If CDate(qCB.Text) < CDate(qCB._ValidateMinValue) Then
                                ErrorText = "Minimum date allowed is " & qCB._ValidateMinValue
                                GoTo HandleError
                            End If
                        End If
                        If IsDate(qCB._ValidateMaxValue) Then
                            If CDate(qCB.Text) > CDate(qCB._ValidateMaxValue) Then
                                ErrorText = "Maximum date allowed is " & qCB._ValidateMaxValue
                                GoTo HandleError
                            End If
                        End If

                    End If
                    If qCB._ValidateNumber = True Then
                        If IsNumeric(qCB.Text) = False Then
                            ErrorText = "Invalid number"
                            GoTo HandleError
                        End If
                        If IsNumeric(qCB._ValidateMinValue) Then
                            If CDbl(qCB.Text) < CDbl(qCB._ValidateMinValue) Then
                                ErrorText = "Minimum number allowed is " & qCB._ValidateMinValue
                                GoTo HandleError
                            End If
                        End If
                        If IsNumeric(qCB._ValidateMaxValue) Then
                            If CDbl(qCB.Text) > CDbl(qCB._ValidateMaxValue) Then
                                ErrorText = "Maximum number allowed is " & qCB._ValidateMaxValue
                                GoTo HandleError
                            End If
                        End If
                    End If
                End If
            End If
            'Call derived class to check validation rules
            Try
                RaiseEvent OnValidateControl(ColName, ColValue, 0, ErrorText)
            Catch ex As Exception
                ShowError("Unexpected error validating a control (OnValidateControl)", ex)
            End Try


            'BHS 11/14/06 Put back whatever the programmer might have put in ColValue
            'SDC 06/16/15 Skip this logic if user's code has logic (such as a prompt) that might allow the wrong field
            '   to be reset
            If iRefreshControlText = True Then
                If TypeOf (sender) Is CheckBox Then
                    XB = CType(C, CheckBox)
                    'BHS 8/17/09 Only change control if there is a difference - this avoids unneccessary screen paints
                    If XB.Checked <> (ColValue.ToLower = "1") Then XB.Checked = (ColValue.ToLower = "1") 'BHS 8/15/08
                ElseIf TypeOf (sender) Is qRC Then
                    Dim RC = CType(C, qRC)
                    If StringCompare(RC._DBText, ColValue) = False Then
                        RC._DBText = ColValue
                    End If
                Else
                    'BHS 8/17/09 Only change control if there is a difference - this avoids unneccessary screen paints
                    If StringCompare(C.Text, ColValue) = False Then
                        C.Text = ColValue 'Sent back from OnValidateControl
                    End If

                    'BHS 8/24/09 Format a qTextBox if it is numeric and _FormatNumber is given
                    If TypeOf (sender) Is qTextBox And IsNumeric(ColValue) = True Then
                        qT = CType(C, qTextBox)
                        If qT._FormatNumber.Length > 0 Then
                            C.Text = Format(CType(ColValue, Double), qT._FormatNumber)
                        End If
                    End If

                    'BHS 2/5/08 Format Date if appropriate
                    If TypeOf (sender) Is qTextBox AndAlso CType(sender, qTextBox)._FormatDate = True Then
                        If IsDate(ColValue) Then
                            'SRM 20140314 Convert year if only 2 digits is entered
                            wstr = ColValue

                            'First test if user entered a year
                            Dim Count As Integer = 0
                            For i As Integer = 1 To wstr.Length
                                If Not IsNumeric(Mid(wstr, i, 1)) Then Count = Count + 1
                            Next

                            If Count > 1 Then 'only convert year if use entered more than 1 seperator
                                For i As Integer = wstr.Length To 1 Step -1
                                    If Not IsNumeric(Mid(wstr, i, 1)) Then
                                        Dim wint As Integer = CInt(Mid(wstr, i + 1))
                                        If wint < 50 Then
                                            wstr = "20" & wint.ToString("00")
                                            ColValue = Mid(ColValue, 1, i) & wstr
                                        ElseIf wint < 100 Then
                                            wstr = "19" & wint.ToString("00")
                                            ColValue = Mid(ColValue, 1, i) & wstr
                                        End If
                                        Exit For
                                    End If
                                Next
                            End If
                            If IsDate(ColValue) Then C.Text = Format(CType(ColValue, Date), "MM/dd/yyyy")
                        End If

                    End If
                End If
            Else
                '...in case user set the variable in OnValidateControl to skip refreshing C.Text, reset the variable here
                iRefreshControlText = True
            End If

HandleError:
            'Show error if one is returned
            If ErrorText > "" Then
                'Tie error icon to control
                iEP.SetIconAlignment(C, ErrorIconAlignment.MiddleLeft)
                iEP.SetError(C, ErrorText)
                ShowStatus(ErrorText)   'Display error text in StatusMsg
                e.Cancel = True         'Prevent user from leaving control
                C.Text = ColValue   'Sent back from OnValidateControl
                Post("BringToFront")    'BHS 9/24/08 make sure error is still visible in case they opened another form.
            Else
                'Commit changes
                For Each BS In iBSs
                    BS.EndEdit()
                Next
                e.Cancel = False
            End If
            'Catch ex As Exception
            'TryError("Validate Control Error:", ex)
            'End Try

            Return ErrorText = ""

        End Function

        ''' <summary> Show an error associated with a control </summary>
        Function ShowControlError(ByVal sender As Object, ByVal aErrorText As String) As Boolean
            Dim C As Control = CType(sender, Control)

            'Try

            If aErrorText > "" Then
                'Tie error icon to control
                iEP.SetIconAlignment(C, ErrorIconAlignment.MiddleLeft)
                iEP.SetError(C, aErrorText)
                ShowStatus(aErrorText)   'Display error text in StatusMsg
                iOKToClose = False
            End If

            Return True

        End Function

        ''' <summary> Handle GV cell validating event </summary>
        Function ValidateGridControl(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellValidatingEventArgs) As Boolean
            Dim ErrorText As String = ""
            Dim ColValue As String = e.FormattedValue.ToString
            Dim OrigColValue As String = ColValue
            Dim GV As DataGridView = CType(sender, DataGridView)
            Dim ColName As String = GV.Columns(e.ColumnIndex).Name
            Dim BS As BindingSource

            'Don't validate when setting up GV, only when it is in focus
            If GV.ContainsFocus = False Then Return True

            'Clear any previous error
            ShowStatus(ErrorText)
            iEP.SetError(GV, "")
            iOKToClose = True

            'Call derived class to check validation rules
            Try
                RaiseEvent OnValidateControl(ColName, ColValue, e.RowIndex, ErrorText)
            Catch ex As Exception
                ShowError("Unexpected error validating a control (OnValidateControl)", ex)
            End Try


            'BHS 11/14/06 Put back whatever the programmer might have put in ColValue
            'If ColValue <> OrigColValue Then  BHS 11/30/06 Always put back ColValue - useful on first value of new row
            GV.Rows(e.RowIndex).Cells(ColName).Value = ColValue

            'Show error if one is returned
            If ErrorText > "" Then
                'GV.Rows(e.RowIndex).Cells(ColName).Value = ColValue 'From OnValidateControl
                iEP.SetIconAlignment(GV, ErrorIconAlignment.MiddleLeft)
                iEP.SetError(GV, ErrorText)
                ShowStatus(ErrorText)   'Display error text in StatusMsg
                e.Cancel = True         'Prevent user from leaving control
                'GV.Rows(e.RowIndex).Cells(ColName).Value = ColValue 'From OnValidateControl
            Else
                iOKToClose = True
                For Each BS In iBSs
                    BS.EndEdit()
                Next
            End If

            Return ErrorText = ""

        End Function

        ''' <summary> Handle general GV problem not caught by OnValidateControl business rules </summary>
        Private Sub gvDataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs)

            If e.Exception.Message.IndexOf("constrained to be unique") > 0 Then
                ShowStatus("Duplicates in list not allowed")
            Else
                If e.Exception.Message.IndexOf("does not allow nulls") > 0 Or e.Exception.Message.ToLower.IndexOf("violationg non-null") > 0 Then
                    ShowStatus("Required field")
                Else
                    If e.Exception.Message.IndexOf("nput string was not in a correct format") > 0 Then
                        ShowStatus("Bad input format in column " & e.ColumnIndex.ToString)
                    End If
                End If
            End If

        End Sub

        ''' <summary> Ignore validation problems if user is closing form </summary>
        Private Sub feMain_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
            If iOKToClose = False Then
                Dim Answer As MsgBoxResult = MsgBoxQuestion("OK to leave form without saving?") 'BHS 9/18/08
                If Answer = MsgBoxResult.No Then
                    e.Cancel = True
                Else
                    e.Cancel = False
                    ShowStatus("")
                    iIsDirty = False    'BHS 9/24/08 Don't ask if they want to save, if they've already decided to leave without saving

                    'BHS 6/29/12
                    'iRecordLock.Clear()
                End If
            End If
            'And iIsNew = False Then e.Cancel = True 'BHS 9/18/08
            If e.Cancel = False Then
                'iRecordLock.Clear() 'BHS 6/29/12
                iEP.Dispose() 'BHS 9/3/08
                iIsClosed = True 'GBV 8/10/2015 - Ticket 2439
            End If



            'e.Cancel = False   BHS 9/3/08  Changed this to force user to deal with errors before closing

            ' GBV 9/23/2014 - Release automatic scaling handler
            'BHS 1/26/17 Remove to fix compile
            'RemoveHandler SystemEvents.UserPreferenceChanged, New UserPreferenceChangedEventHandler(AddressOf SystemEvents_UserPreferenceChangesEventHandler)
        End Sub

#End Region

#Region "-------------------- Authority Functions -------------------------"

        ''' <summary> Uses CheckPermLevel in UCBase ClientAuthority to decide
        ''' if user may write in this record and function </summary>
        Function IsWriter(ByVal aFunctName As String) As Boolean
            'BHS 5/22/09  Prevent anyone from writing to Live Database if we're in a Test Version of the NUI
            If OKToWriteToDB() = False Then Return False

            'If fBase.ActiveForm Is Nothing Then Return True 'Don't check authority in the DESIGNER
            If InDevEnv() Then Return True

            'If gTestUserAuth.ToLower = "writer" Then 'GBV 7/17/2008 ' GBV changed 1/10/2011
            '    iIsWriter = True
            'Else
            If gTestUserAuth.ToLower = "reader" Then
                iIsWriter = False
            Else
                'BHS 1/26/17 for iIsWriter = True
                'iIsWriter = Auth.IsWriter(aFunctName)  'Logic at Client level
                iIsWriter = True
            End If

            'BHS 5/14/10
            iIsWriter = DescendantOverrideIsWriter(iIsWriter)

            'Dim OnIsWriterResult As Boolean = iIsWriter
            'RaiseEvent OnIsWriter(OnIsWriterResult) ' BHS Test whether this works - 6/1/09
            'iIsWriter = OnIsWriterResult

        End Function

        ' BHS 5/14/10 Make OnIsWriter callable from feMain, etc.
        ''' <summary> Allow descendant to have the last say in whether user is a writer </summary>
        Function DescendantOverrideIsWriter(ByVal aIsWriter As Boolean) As Boolean
            Try
                RaiseEvent OnIsWriter(aIsWriter)
            Catch ex As Exception
                ShowError("Unexpected error checking permissions (OnIsWriter)", ex)
            End Try

            Return aIsWriter
        End Function

        ''' <summary> Manually set iIsWriter </summary>
        Public Sub SetIsWriter(ByVal aOK As Boolean)
            iIsWriter = aOK
        End Sub

        ''' <summary> aCampus for this record.  aHomeType = "HomeLic", "HomePros" or "HomeAcct") </summary>
        Function isCampusWriter(ByVal aCampus As String, ByVal aHomeType As String) As Boolean

            'If fBase.ActiveForm Is Nothing Then ' do not check in designer
            If InDevEnv() Then
                iIsCampusWriter = True
                iCampusWriterChecked = True
                Return True
            End If

            If My.Application.Info.Title = "DMS" Then
                If iIsWriter = True Then Return True
                Return False
            End If

            'If isCampusWriter is called from the OnLoadForm event, it is probably before iFName has been set in SetFormAttributes
            If IsDBNull(iFName) OrElse iFName = "" Then
                MsgBoxInfo("Programmer Note: isCampusWriter called before form attributes have been set--iFName is blank!", , "isCampusWriter")
            End If

            If Not IsDBNull(iFName) AndAlso iFName > "" AndAlso Auth.GetPermLevel(iFName) = 3 Then ' if global writer, do not check - GBV 7/15/2008
                iIsCampusWriter = True
            Else
                iIsCampusWriter = Auth.isCampusWriter(aCampus, aHomeType)  'Logic at Client level
            End If

            'This function was run
            iCampusWriterChecked = True

            ' Raise event for the benefit of the client
            Try
                RaiseEvent OnIsCampusWriter()
            Catch ex As Exception
                ShowError("Unexpected error checking permissions (OnIsCampusWriter)", ex)
            End Try


            Return iIsCampusWriter
        End Function

        '''<summary> Not supported in UC implementation - always returns False</summary>
        Function IsReader(ByVal aLogName As String, ByVal aFunctName As String) As Boolean

            'If fBase.ActiveForm Is Nothing Then Return True 'Don't check authority in designer
            If InDevEnv() Then Return True
            Return Auth.IsReader(aLogName, aFunctName)  'Logic at Client level

        End Function

        '''<summary> Not supported in UC implementation - always returns False</summary>
        Function HasAuthority(ByVal aLogName As String, ByVal aFunctName As String, ByVal aMinAuth As Integer) As Boolean

            Return Auth.HasAuthority(aLogName, aFunctName, aMinAuth)

        End Function

#End Region

#Region "-------------------- Post Event -------------------------"
        '''<summary> Set post event and start timer </summary>
        Sub Post(ByVal aEventName As String)
            PostTimer._EventName = aEventName
            PostTimer.Start()
        End Sub

        '''<summary> Set post event and a parameter, and start timer </summary>
        Sub Post(ByVal aEventName As String, ByVal aParameter As Object)
            PostTimer._EventName = aEventName
            PostTimer._Param = aParameter
            PostTimer.Start()
        End Sub

        '''<summary> Stop timer and run the post event </summary>
        Private Sub PostTimer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles PostTimer.Tick
            PostTimer.Stop()

            'Need to find Execute command to run event subroutine without needing Case statement
            Select Case PostTimer._EventName.ToLower
                Case "setstartfocus"
                    SetStartFocus()
                Case "gotolastfield"
                    GoToLastField()
                Case "gotonextfield"
                    GoToNextField(PostTimer._Param)
                    'BHS 7/6/10  Doesn't reliably work because some other timer event can overwrite it.  Use DirtyTimer instead.
                    'Case "setdirtyindicator"
                    '   SetDirtyIndicator(iIsDirty)
                Case "bringtofront"
                    Me.BringToFront()
                Case "selectall"    'BHS 10/9/09 SelectAll for current Masked TextBox
                    For Each C As Control In Me.Controls
                        If C.Focused = True Then
                            Dim qMT As qMaskedTextBox = TryCast(C, qMaskedTextBox)
                            If qMT IsNot Nothing Then qMT.SelectAll()
                        End If
                    Next

                Case "close"
                    'This doesn't always work, because some other timer event overwrites it.  Use CloseTimer timer.
                    Me.Close()
                Case "endofclear"
                    'BHS 2/9/09  Refresh Tab Count, if appropriate
                    For Each C As Control In Me.Controls
                        Dim T As qTab = TryCast(C, qTab)
                        If T IsNot Nothing Then T.RefreshPages()
                    Next
                Case Else
                    RaiseEvent OnPost(PostTimer._Param)
            End Select
        End Sub

        '''<summary> Close Timer closes this form </summary>
        Private Sub CloseTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseTimer.Tick
            CloseTimer.Stop()
            Me.Close()
        End Sub


        '''<summary> Dirty Timer closes this form </summary>
        Private Sub DirtyTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DirtyTimer.Tick
            DirtyTimer.Stop()
            SetDirtyIndicator(iIsDirty)

        End Sub

#End Region

#Region "-------------------- Form Events ------------------------"

        Private Sub fBase_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            TightenCascade()
        End Sub

        '''<summary> For GVs with footers, Window resize resizes GV, so AdjustForScrollBar 
        '''(makes Main GV bigger if footer doesn't need scroll bar </summary>
        Private Sub Form_ResizeEnd(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.ResizeEnd
            Try
                For Each GV As DataGridView In iGVs
                    If TypeOf (GV) Is qGVBase AndAlso CType(GV, qGVBase)._GVFoot IsNot Nothing Then
                        AdjustForScrollBar(CType(GV, qGVBase), CType(GV, qGVBase)._GVFoot)
                    End If
                Next
                'BHS 8/17/10
                If iIsLoading = False Then
                    If WindowState = FormWindowState.Normal Then
                        If iRememberResize = True Then
                            SaveWindowSize(Me.Name, Me.Width, Me.Height)
                        End If
                    End If
                End If
            Catch ex As Exception
                ShowError("Error resizing form", ex)
                Return  'Not fatal
            End Try
        End Sub

        ' GBV 9/15/2014 - If the form is not sizeable, ResizeEnd is never triggered. When the form
        ' is maximized and restored, this event triggers and the size must be saved.
        Private Sub fBase_SizeChanged(sender As System.Object, e As System.EventArgs) Handles MyBase.SizeChanged
            If Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable Then Return
            Try
                For Each GV As DataGridView In iGVs
                    If TypeOf (GV) Is qGVBase AndAlso CType(GV, qGVBase)._GVFoot IsNot Nothing Then
                        AdjustForScrollBar(CType(GV, qGVBase), CType(GV, qGVBase)._GVFoot)
                    End If
                Next
                If iIsLoading = False Then
                    If WindowState = FormWindowState.Normal Then
                        If iRememberResize = True Then
                            SaveWindowSize(Me.Name, Me.Width, Me.Height)
                        End If
                    End If
                End If
            Catch ex As Exception
                ShowError("Error resizing form", ex)
                Return  'Not fatal
            End Try
            
        End Sub
#End Region

#Region "-------------------- Obsolete ------------------------------"
        'Public iErrors As ArrayList = New ArrayList     'List of error messages


        'Private Sub fBase_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'End Sub

        'Private Sub InitializeComponent()  ---This is in fBase.Designer, and doesn't need to be here (i think)---
        '    Me.SuspendLayout()
        '    '
        '    'fBase
        '    '
        '    Me.ClientSize = New System.Drawing.Size(292, 266)
        '    Me.Name = "fBase"
        '    Me.ResumeLayout(False)

        'End Sub

        ''Return this function's name - Not used anywhere
        'Function GetiFName() As String
        '    If iFName Is Nothing Then iFName = ""
        '    Return iFName
        'End Function

        ''Insert Row 
        'Function InsertRow(ByRef aGV As DataGridView) As Boolean
        '    Dim ActedOn As Boolean = False
        '    Dim R As DataGridViewRow

        'Dim BS As BindingSource = CType(iGVs(0).DataSource, BindingSource) 'DEBUG is this OK?  Ever use other binding source?
        '    Dim C As Control = Nothing
        '    Dim Cell As DataGridViewCell
        '    iDS.EnforceConstraints = False
        '    RaiseEvent OnInsertRow(aGV, ActedOn) 'Allow non-standard handling in child

        '    If Not ActedOn Then
        '        'Don't Insert a row if there is already a blank row
        '        With aGV
        '            For Each R In aGV.Rows
        '                If R.Tag IsNot Nothing Then
        '                    If R.Tag.ToString = "BlankRow" Then Return False
        '                End If
        '            Next

        '            Dim BSRow As Object = BS.AddNew()
        '            'Dim DR As DataRow = CType(BSRow, DataRow)



        '            BS.EndEdit(

        '            '.DataSource.AcceptChanges() 'Change row state from Added to Not Modified for dirty checking
        '            'DR.AcceptChanges()


        '            R = .Rows(.Rows.Count - 1)
        '            R.Tag = "BlankRow"

        '            For Each Cell In R.Cells()
        '                Dim EF As EntryField = New EntryField
        '                EF.GV = aGV
        '                EF.ColName = Cell.OwningColumn.Name
        '                EF.Row = R.Index
        '                EF.OrigValue = Cell.FormattedValue.ToString
        '                iEntryFields.Add(EF)
        '            Next

        '            RaiseEvent OnSetGVDefaultFields(aGV, .Rows(.Rows.Count - 1))

        '        End With

        '        'Focus on cell in last row - DEBUG gets focus but can't start entering...
        '        'aGV.Focus()
        '        'aGV.Rows(aGV.Rows.Count - 1).Cells(1).Selected = True
        '        'MsgBox(aGV.Rows(aGV.Rows.Count - 1).Tag)


        '    End If

        '    'DEBUG
        '    'If iBtnRemoveRow IsNot Nothing Then
        '    '    SetGVButtons(aGV, aGV.Rows.Count - 1)
        '    'End If

        '    Return True
        'End Function




        ''''' From SetContext:
        'If fBase.ActiveForm IsNot Nothing Then
        '    If Not IsReader(Auth.GetLogName(), iFName) Then    'Check user auth
        '        'If Not fBase.ActiveForm Is Nothing Then  'If not design time...
        '        MsgBox("User " + My.User.Name + " doesn't have permission to use this function (" & iFName & ").  See your supervisor", MsgBoxStyle.Exclamation, "Not Enough Authority")
        '        Return False
        '        'End If
        '    End If
        'End If

        'Catch ex As Exception
        '   TryError("fBase.SetContext Error:", ex)
        'End Try

        '''''From SetGVProperties
        'GV.RowTemplate.DefaultCellStyle.SelectionBackColor = QSelectionBackColor 'Color.AliceBlue
        'GV.RowTemplate.DefaultCellStyle.SelectionForeColor = QSelectionForeColor 'Color.Black
        'GV.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        'ElseIf TypeOf (GV) Is qGVList Then
        '   GV.RowTemplate.DefaultCellStyle.SelectionBackColor = QSelectionBackColor 'Color.AliceBlue
        '  GV.RowTemplate.DefaultCellStyle.SelectionForeColor = QSelectionForeColor 'Color.Black
        ' GV.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))

        'BHS 8/11/08 Removed
        'Overridable Function RowDirty(ByVal aGV As DataGridView, ByVal aRow As Integer) As Boolean
        '    Dim EF As EntryField

        '    'Try

        '    For Each EF In iEntryFields

        '        'GV Compare
        '        If EF.GV IsNot Nothing Then
        '            If EF.GV.Name = aGV.Name Then
        '                If EF.Row = aRow Then
        '                    If EF.OrigValue <> EF.GetCurrentValue Then
        '                        Return True
        '                    End If
        '                End If
        '            End If
        '        End If
        '    Next

        '    'Catch ex As Exception
        '    'TryError("fBase.RowDirty Error:", ex)
        '    'End Try

        '    Return False

        'End Function

        ''BHS 8/11/08
        'Overridable Function GetDetailEF(ByVal aGV As DataGridView, ByVal aRow As Integer, ByVal aName As String) As EntryField
        '    Dim EF As EntryField

        '    For Each EF In iEntryFields

        '        'GV Compare
        '        If EF.GV IsNot Nothing Then
        '            If EF.GV.Name = aGV.Name Then
        '                If EF.Row = aRow Then
        '                    If EF.ColName.ToLower = aName.ToLower Then
        '                        Return EF
        '                    End If
        '                End If
        '            End If
        '        End If
        '    Next

        '    Dim EF2 As EntryField = Nothing

        '    Return EF2

        'End Function

        'BHS 8/11/08
        'Overridable Function GetEF(ByVal aControlName As String) As EntryField
        '    Dim EF As EntryField

        '    For Each EF In iEntryFields
        '        If EF.C IsNot Nothing Then  'BHS 2/7/08
        '            If EF.C.Name.ToLower = aControlName.ToLower Then
        '                Return EF
        '            End If
        '        End If
        '    Next

        '    Dim EF2 As EntryField = Nothing
        '    Return EF2

        'End Function

        ''BHS 8/11/08
        ''Create the iEntryFields array based on controls and GV cells in the form
        'Public Overridable Function InitializeEntryFields() As Boolean
        '    Dim C As Control
        '    Dim GV As DataGridView
        '    Dim EF As EntryField
        '    Dim Cell As DataGridViewCell
        '    Dim R As DataGridViewRow

        '    'Initialize the Entry Fields Array List
        '    iEntryFields = New ArrayList

        '    'Add all standard entry controls to array
        '    For Each C In Me.Controls
        '        AddToEntryArray(C)
        '    Next

        '    For Each GV In iGVs 'All GridViews

        '        'Set up Entry Field array
        '        For Each R In GV.Rows
        '            For Each Cell In R.Cells
        '                EF = New EntryField     'New object to be added to iEntryFields
        '                EF.GV = GV
        '                EF.ColName = Cell.OwningColumn.Name
        '                EF.Row = R.Index
        '                EF.OrigValue = Cell.FormattedValue.ToString
        '                iEntryFields.Add(EF)
        '                'EF = CType(iEntryFields(iEntryFields.Count - 1), EntryField)
        '                ' MsgBox(iEntryFields.Count.ToString, MsgBoxStyle.Information, EF.OrigValue)
        '            Next
        '        Next
        '    Next

        'End Function

        'BHS 8/11/08
        'Sub AddToEntryArray(ByVal aC As Control)
        '    Dim C2 As Control
        '    Dim EF As EntryField

        '    Select Case aC.GetType.Name.ToLower
        '        Case "textbox", "qtextbox", "combobox", "qcombobox", "qmaskedtextbox"
        '            EF = New EntryField     'New object for each one to be added to iEntryFields
        '            EF.C = aC
        '            EF.OrigValue = aC.Text
        '            iEntryFields.Add(EF)
        '    End Select

        '    'Recursive call to get to controls within controls
        '    For Each C2 In aC.Controls
        '        AddToEntryArray(C2)
        '    Next
        'End Sub

        'BHS 8/11/08 'Set all controls within a control not dirty (recursive)
        'Sub SetControlNotDirty(ByVal aC As Control)
        '    Dim EF As New EntryField()
        '    EF = Me.GetEF(aC.Name)
        '    If EF IsNot Nothing Then EF.ResetOrigValue()
        '    For Each C As Control In aC.Controls
        '        SetControlNotDirty(C)
        '    Next
        'End Sub

        'Function BuildQuery(ByVal aSQL As String) As String
        '    Dim C As Control
        '    Dim T As TextBox
        '    Dim CB As ComboBox
        '    Dim ColName As String = ""
        '    Dim ColType As String = ""
        '    Dim Value As String = ""

        '    'Initialize iSQLDescr
        '    iSQLDescr = ""

        '    For Each C In Me.Controls

        '        If C.Name.ToString.Substring(0, 2) = "q_" Then

        '            If TypeOf (C) Is TextBox Then
        '                'Build Query String
        '                T = CType(C, TextBox)
        '                If T.Text > "" Then
        '                    If GetTagColInfo(T.Tag, ColName, ColType) Then
        '                        aSQL = AddWhere(aSQL, ColName, T.Text, ColType)
        '                        AddToSQLDescr(ColName, T.Text)
        '                    End If
        '                End If
        '            End If

        '            If TypeOf (C) Is ComboBox Then
        '                CB = CType(C, ComboBox)
        '                If CB.Text.ToString > "" Then
        '                    If GetTagColInfo(CB.Tag, ColName, ColType) Then
        '                        'Send Selected Value if filled in
        '                        Value = CB.Text
        '                        If CB.SelectedValue IsNot Nothing Then
        '                            If CB.SelectedValue.ToString > "" Then
        '                                Value = CB.SelectedValue.ToString
        '                            End If
        '                        End If
        '                        aSQL = AddWhere(aSQL, ColName, Value, ColType)
        '                        AddToSQLDescr(ColName, Value)
        '                    End If
        '                End If
        '            End If


        '        End If
        '    Next

        '    Return aSQL

        'End Function

        'BHS 8/11/08
        ''Find and return Entry Field based on a GV and Cell
        'Function GetGVEntryField(ByVal aGV As DataGridView, ByVal aCell As DataGridViewCell, ByRef aEF As EntryField) As Boolean
        '    Dim EF As EntryField = Nothing

        '    'Try

        '    For Each EF In iEntryFields
        '        If EF.GV IsNot Nothing Then
        '            If EF.GV.Equals(aGV) Then
        '                If aCell.RowIndex = EF.Row And aCell.OwningColumn.Name = EF.ColName Then
        '                    aEF = EF
        '                    Return True
        '                End If
        '            End If
        '        End If
        '    Next

        '    'Catch ex As Exception
        '    'TryError("GetGVEntryField Error:", ex)
        '    'End Try
        '    Return False

        'End Function

        ''Get User intials based on GetLogName    IN FUNCTION
        'Public Function GetInits() As String
        '    Dim LogName As String
        '    Dim Inits As String = ""
        '    Dim ob As Object

        '    LogName = GetLogName()

        '    Dim cn As SqlConnection = New SqlConnection(My.Settings.Item("authConnStr").ToString)

        '    ob = DoSQL(cn, "Select inits from t_pbuser where logname = '" + LogName + "'", "")
        '    If ob Is Nothing Then Return ""
        '    Return CType(ob, String)

        'End Function

#End Region

#Region "-------------------- BHSCONV Routines ------------------------"
        'BHSCONV
        Private Sub iSQLDA_RowUpdated(ByVal sender As Object, _
                       ByVal e As System.Data.SqlClient.SqlRowUpdatedEventArgs) _
                       Handles iSQLDA1.RowUpdated, iSQLDA2.RowUpdated, iSQLDA3.RowUpdated, _
                       iSQLDA4.RowUpdated, iSQLDA5.RowUpdated, iSQLDA6.RowUpdated, _
                       iSQLDA7.RowUpdated, iSQLDA8.RowUpdated, iSQLDA9.RowUpdated

            Try
                RaiseEvent OnSQLDARowUpdated(sender, e)   'Load values for this function
            Catch ex As Exception
                ShowError("Unexpected error handling a row update (OnSQLDARowUpdated)", ex)
            End Try

        End Sub

        Public Function GetSQLDA(ByVal aTableName As String) As SqlDataAdapter
            Dim i As Integer = iSQLDATableNames.LastIndexOf(aTableName) + 1
            Select Case i
                Case 1
                    Return iSQLDA1
                Case 2
                    Return iSQLDA2
                Case 3
                    Return iSQLDA3
                Case 4
                    Return iSQLDA4
                Case 5
                    Return iSQLDA5
                Case 6
                    Return iSQLDA6
                Case 7
                    Return iSQLDA7
                Case 8
                    Return iSQLDA8
                Case 9
                    Return iSQLDA9

                Case Else
                    MsgBox("PROGRAMMER ERROR - iSQLDA not found")
            End Select
            
            Return Nothing
        End Function

        Public Function GetSQLDANo(ByVal aTableName As String) As Integer
            Return iSQLDATableNames.LastIndexOf(aTableName) + 1
        End Function
#End Region

        'BHS Added 7/6/15
        ''' <summary> Move GV Row up or down </summary>
        Function MoveGVRow(aGV As DataGridView, Optional aDirection As String = "up") As Boolean

            If aGV Is Nothing OrElse aGV.DataSource Is Nothing Then Return False

            Dim BS As BindingSource = CType(aGV.DataSource, BindingSource)

            If aGV.SelectedRows.Count = 0 Then
                MsgBox("You must select a row before attempting to move it.")
                Return False
            End If

            Dim i As Integer = aGV.SelectedRows(0).Index
            If aDirection = "up" Then
                If i = 0 Then
                    MsgBox("First row can't be moved up further.")
                    Return False
                End If
            Else
                'If not up, then down
                If i = aGV.Rows.Count - 1 Then
                    MsgBox("Last row can't be moved down further.")
                    Return False
                End If
            End If

            ' Get the data row
            Dim Cm As CurrencyManager = CType(BindingContext(BS), CurrencyManager)
            Dim R As DataRow = CType(Cm.Current, DataRowView).Row
            Dim Table As DataTable = R.Table

            ' Save the row
            Dim R2 As DataRow = Table.NewRow
            R2.ItemArray = R.ItemArray

            ' Remove the row and insert it one position up
            Table.Rows.RemoveAt(i)
            If aDirection = "up" Then
                Table.Rows.InsertAt(R2, i - 1)
            Else
                'Or one position down
                Table.Rows.InsertAt(R2, i + 1)
            End If

            ' Select the row and set the binding source to the new position
            If aDirection = "up" Then
                aGV.Rows(i - 1).Selected = True
                Cm.Position = i - 1
            Else
                aGV.Rows(i + 1).Selected = True
                Cm.Position = i + 1
            End If

            BS.ResetCurrentItem()

            Return True
        End Function

    End Class

End Namespace



