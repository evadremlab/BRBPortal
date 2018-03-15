'Imports IBM.Data.Informix
'This module holds global variables
Public Module Appl

    'Test 2
#Region "-------------- Documentation -------------------"
    'BHS 11/26/07 Added AssemblyNames array to allow any number of Assembly Names (Projects)
    '  to search through
#End Region

#Region "-------------- App Global Constants -------------------"
    'PlantID.net
    Public gTestProduction As Boolean
    'Most of these global values are initialized in MDI.load
    Public gMenu As System.Windows.Forms.MenuStrip
    Public gMDIForm As System.Windows.Forms.Form
    Public gMDIStatusLabel As ToolStripStatusLabel  'Pointer for messages posted back to the MDI

    'BHSCONV 6/26/12 Default ConnType to IFX to make sure crons use appropriate connection
    Public ConnType As String = "SQL"   'Default database server (SQL or IFX)
    'Public AssemblyName As String = ""  'Used for reflection to gettype(<objectname>)  Removed BHS 11/26/07
    'Public AssemblyName2 As String = ""
    Public AssemblyNames As New ArrayList   'Allow any number of Assembly Names (Projects) to search through
    Public AppDatabase As String = ""    'Allows programmers to use test db
    Public gBackgroundCount As Integer = 0  'Number of background processes this user has running
    Public gMissingHelpMsg As String = ""
    Public gOrSymbol As String = ","
    Public gOrSymbol2 As String = "{OR}"
    Public gNullSymbol As String = "{NULL}"
    Public gNullSymbol2 As String = "{EMPTY}"
    Public gNotNullSymbol As String = "{NOTNULL}"
    Public gNotNullSymbol2 As String = "{NOT NULL}"
    Public gNotNullSymbol3 As String = "{NOTEMPTY}"
    Public gNotNullSymbol4 As String = "{NOT EMPTY}"
    Public gAndSymbol As String = "+"
    Public gBetweenSymbol As String = "-"
    Public gErrorLogPath As String = ""
    Public gWatchOn As Boolean = False
    Public gHelpImages As String = ""
    Public gHelpPath As String = ""
    Public gGenErrorMsg As String = ""
    Public gInvSharePath As String = ""
    Public gSavedReportsPath As String = ""
    Public gDBServer As String = ""
    Public gDBRootConnStr As String = ""
    Public gReleaseContext As String = ""   'Used to describe how this release is to be used
    Public gRptWinTitle As String = ""
    Public gDatabaseName As String = ""

    'These are all set in App startup routine
    Public gAppConnStr As String = ""
    Public gSQLConnStr As String = ""
    Public gSQLDocsConnStr As String = ""
    Public gODBCConnStr As String = ""
    Public gAuthConnStr As String = ""
    Public gNCDConnStr As String = ""
    Public gHelpConnStr As String = ""
    Public gMDConnStr As String = ""
    Public gDMSConnStr As String = ""
    Public gReportServer As String = ""
    Public gTestUserAuth As String = ""     'Set to reader or writer to skip authority checks while testing
    Public gNameValueDelimiter As String = "="
    Public gNameValuePairDelimiter As String = ","
    Public gScannedInvoiceInBox As String = ""
    Public gScannedInvoiceOutBox As String = ""
    'Added 6/26/12 to allow programs to update tables that only exist in production, such as gMenu
    Public gSQLProdConnStr As String = ""

    'Added 12/11/12to allow programs to update tables in Test SQL Server, no matter what version of NUI
    Public gSQLTestConnStr As String = ""


    'Email settings
    Public gErrorAlphaEmailRecipients As New ArrayList
    Public gErrorProdEmailRecipients As New ArrayList
    Public gErrorAcctgEmailRecipients As New ArrayList
    Public gEmailSender As String = ""

    'Set conversion from two-digit to four-digit years.  Initialized in MDI.Load.  
    'DotNet defaults to 2029 at VB2005, but we typically want 2049.
    Public gDateFormatInterface As System.Globalization.DateTimeFormatInfo = Nothing


    Public Auth As Authority    'Global Authority class allows overridable functions at client level
    Public gAuth As Authority
    Public gUserName As String = ""     'BHS 6/16/08
    'BHS 1/26/17 This may not be properly used in PlantID
    Public gUserInit As String = ""
    Public gShortName As String = ""    'BHS 1/27/17 key to Users table

    Public gPermissionsDV As DataView    ' GBV 6/19/08
    Public gUserOffDV As DataView ' GBV 6/20/2008

    ' Gladstone "Mode" - GBV 8/14/2008
    Public gIsGladstone As Boolean = False

    'Version can be qsi, alpha, beta, prod, dms, or hotfix
    Public gVersion As String = ""
    Public gSuspendLiveDatabaseProtection As Boolean = False

    ' Demo "mode" - GBV 1/5/2009
    Public gIsDemo As Boolean = False

    'Partition display (grid and dropdown) settings
    Public gOnePartition As Boolean = False
    Public gPart1Abbrev As String = "OTT"
    Public gPart1Name As String = "OTT"
    Public gPart2Abbrev As String = "BK"
    Public gPart2Name As String = "Berkeley"
    Public gPart3Abbrev As String = "SD"
    Public gPart3Name As String = "San Diego"
    Public gPart4Abbrev As String = "SF"
    Public gPart4Name As String = "San Francisco"

    'Partition-specific settings
    Public gOTT As String = "OTT"
    Public gBK As String = "BK"
    Public gSD As String = "SD"
    Public gSF As String = "SF"
    Public gUCTT As String = "UCTT"
    Public gCPI As String = "CPI" ' GBV 2/16/2012 (R5.1.13)

    ' Last Partition used - GBV 8/14/2008
    Public gLastPartition As String = ""

    'Last save to location - DJW 01/21/2015
    Public gLastSaveDrive As String = ""
    Public gLastSaveLocation As String = ""
    Public gLastSaveLocal As Boolean = False

    'Last document upload locations - SDC 04/30/2015
    Public gLastDocUploadLoc_DMS As String = ""
    Public gLastDocUploadLoc_PTS As String = ""

    ' Global Connection objects
    Public gODBCCn As Odbc.OdbcConnection = Nothing
    Public gSQLCn As Data.SqlClient.SqlConnection = Nothing

    'Global QBE Syntax Error gets set to True in AddWhere if invalid date or number,
    'and gets initialized to False in the beginning of fBase.Query()
    Public gQBESyntaxError As Boolean = False

    Public gTempDirectory As String = "H:\PTSW\Reports\Temp"
    '
    ' Temporary global variable to indicate BuildSQL routine encountered a bad number or date
    'Public iBadSQL As Boolean = False

    ' Set backgound on Data Repeater row
    Public gSetqDRBackgoundColor As Boolean = False

    Public gSelectAllCode As Integer = 196673      'Ctrl Shift A

    Public gIfxCaseInsensitiveColumns As String = ""

    Public gEnvironment As String = "" ' GBV 1/8/2014 to set connection string for DR environment
    Public gFileServer As String = "" ' GBV 1/9/2014 to set file server name for DR environment
    Public gIsFromCron As Boolean = False ' GBV 7/21/2014 - If functions are run unattended, skip user imput and other processes.
    Public gCronRPTPath As String = "" ' GBV 7/23/2014 - export path to be picked up by cron



#End Region

#Region "-------------- Color Definitions -------------------"

    'These colors should be reassigned in the menu program so they can vary by client or project

    'Form background color
    Public QBackColor As Color = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(246, Byte), Integer))

    'Form standard text color
    Public QForeColor As Color = Color.Black
    Public QEmphasisForeColor As Color = Color.Red

    'Entry box background colors
    Public QEntryBackColor As Color = Color.White
    Public QReadOnlyBackColor As Color = QBackColor
    Public QRequiredBackColor As Color = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(160, Byte), Integer))
    Public gUseRequiredBackColor As Boolean = False

    '--- Lists ---
    'Column Header Background Color
    Public QColHeaderBackColor As Color = System.Drawing.Color.FromArgb(CType(CType(214, Byte), Integer), CType(CType(214, Byte), Integer), CType(CType(255, Byte), Integer))

    'Basic list background until rows are added
    Public QListBackColor As Color = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))

    'Row background color
    Public QDefaultRowBackColor As Color = Color.White
    Public QAltRowBackColor As Color = QBackColor

    'List selection background and text color
    Public QSelectionBackColor As Color = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(180, Byte), Integer))
    Public QSelectionForeColor As Color = Color.Black

#End Region

#Region "-------------- App-Level Functions -------------------"

    ''BHS 12/8/08 Called from IfxBuildDV    
    '''' <summary> Changes OTT: to OTS: and UCTT: to UCS: </summary>
    Function PrepareSQLForStaticTest(ByVal aSQL As String) As String
        'If gPart1Abbrev = "OTS" Then
        '    SubstituteAllStr(aSQL, "OTT:", "OTS:")
        '    SubstituteAllStr(aSQL, "UCTT:", "UCS:")
        'End If
        Return aSQL
    End Function

    '''<summary> Returns False if we're working with the Live Database, and this is a Version of the Application 
    ''' that shouldn't update Live Database(demo, alpha, beta, or qsi)</summary>
    Function OKToWriteToDB() As Boolean

        'BHS DEBUG - USEFUL FOR LIVE DATABASE DEBUGGING
        'Return True

        'Always write unless we're talking about the Live Database
        If gDatabaseName <> "Live Database" Then Return True
        If gSuspendLiveDatabaseProtection = True Then Return True


        'If iNeedToUpdateLiveDatabase is set true for the active form, then allow UpdateLiveDatabase
        If gMDIForm.ActiveMdiChild IsNot Nothing Then
            Dim F As QSILib.Windows.Forms.fBase = TryCast(gMDIForm.ActiveMdiChild, QSILib.Windows.Forms.fBase)
            If F IsNot Nothing Then
                If F.iNeedToUpdateLiveDatabase = True Then Return True
            End If
        Else
            'BHS 11/16/09 Allow writing to Live Database from Menu
            Return True
        End If

        'Prevent writing to Live Database if application is a test version
        If gVersion.ToLower = "qsi" Or gVersion = "demo" Or gVersion = "alpha" Or gVersion = "beta" Then Return False

        Return True     'Allow updates for hotfix, prod or dms
    End Function

#End Region
End Module
