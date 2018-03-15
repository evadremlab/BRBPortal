Imports System.Web.Optimization
Imports System.Web.SessionState
Imports QSILib

Public Class Global_asax
    Inherits HttpApplication

    Sub Application_Start(sender As Object, e As EventArgs)
        ' Fires when the application is started
        RouteConfig.RegisterRoutes(RouteTable.Routes)
        BundleConfig.RegisterBundles(BundleTable.Bundles)
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)

        ' Fires when the session is started
        'Set database connection based on computer and application directory
        If Request.PhysicalApplicationPath.ToUpper.IndexOf("TEST") > -1 Then
            gAppConnStr = "Data Source=QS-FS-X64;Initial Catalog=RTS2;Integrated Security=True"
            'If XPS
            'If My.Computer.Name = "BruceXPS" Then
            '    gAppConnStr = "Data Source=BruceXPS\RTS2;Initial Catalog=PlantIDTest;Integrated Security=True"
            'End If
            ''If Laptop - THIS NEEDS UPGRADE TO SQL2012
            'If My.Computer.Name = "USER-PC" Then
            '    gAppConnStr = "Data Source=USER-PC\RTS2;Initial Catalog=PlantIDTest;Integrated Security=True"
            'End If
            ''If Los Altos - THIS NEEDS UPGRADE TO SQL2012
            'If My.Computer.Name = "LOSALTOSWIN7" Then
            '    gAppConnStr = "Data Source=LOSALTOSWIN7\RTS2;Initial Catalog=PlantIDTest;Integrated Security=True"
            'End If
            ''If Sausalito - THIS NEEDS UPGRADE TO SQL2012
            'If My.Computer.Name = "SAUS2014" Then
            '    gAppConnStr = "Data Source=SAUS2014\RTS2;Initial Catalog=PlantIDTest;Integrated Security=True"
            'End If
            Session("Test") = True
        Else
            gAppConnStr = "Data Source=QS-FS-X64;Initial Catalog=RTS2;Integrated Security=True"
            'If XPS
            'If My.Computer.Name = "BruceXPS" Then
            '    gAppConnStr = "Data Source=BruceXPS\PLANTID12;Initial Catalog=PlantID;Integrated Security=True"
            'End If
            ''Laptop  - THIS NEEDS UPGRADE TO SQL2012
            'If My.Computer.Name = "USER-PC" Then
            '    gAppConnStr = "Data Source=USER-PC\PLANTID;Initial Catalog=PlantID;Integrated Security=True"
            'End If
            ''If Los Altos - THIS NEEDS UPGRADE TO SQL2012
            'If My.Computer.Name = "LOSALTOSWIN7" Then
            '    gAppConnStr = "Data Source=LOSALTOSWIN7\PLANTID;Initial Catalog=PlantID;Integrated Security=True"
            'End If
            ''If Sausalito - THIS NEEDS UPGRADE TO SQL2012
            'If My.Computer.Name = "SAUS2014" Then
            '    gAppConnStr = "Data Source=SAUS2014\PLANTID12;Initial Catalog=PlantID;Integrated Security=True"
            'End If
            Session("Test") = False
        End If

        'Update gSQLConnStr, which is what the new QBase uses
        gSQLConnStr = gAppConnStr

        'Initialize Session Variables
        '  If "IsCurrent" is not true, then we've lost the Session and we show timeout page
        Session("IsCurrent") = True

        'Session.Add("gAppConnStr", gAppConnStr)
        Session("gAppConnStr") = gAppConnStr
        Session("UserName") = ""        'Users.ShortName
        Session("UserFullName") = ""    'Users.FullNme
        Session("UserIsReviewer") = ""  'Blank means not set.  Y or N when set
        Session("UserIsEditor") = ""    'Blank means not set.  Y or N when set
        Session("FromURL") = ""     'Set during load of each page, so it can be returned to
        Session("Ex") = Nothing     'Exception Object
        Session("ExMsg") = ""       'Exception Message

        Session("NextControlName") = Nothing    'Track where to set focus when returning to this page
        Session("SearchCriteriaDV") = Nothing   'DV of Search Criteria for display
        Session("SearchDV") = Nothing           'DV of Search results (can be long)
        Session("PlantCurrentRecord") = Nothing
        Session("PlantCount") = Nothing
        Session("PhotoCurrentRecord") = Nothing
        Session("PhotoCount") = Nothing
        Session("TotalPhotoCount") = Nothing
        Session("QueryDescr") = ""

        Session("Taxon") = ""
        Session("ATitle") = ""
        Session("AType") = ""   'Plant List, Default Taxon, or Glossary
        Session("Location") = ""
        Session("dvPhoto") = Nothing
        Session("Ppher") = ""
        Session("LocDescr") = ""
        Session("PhotoReviewed") = ""
        Session("ArgumentData") = ""    'data holder for custom arguments

        Session("TOCMode") = "Guides"   'Guides, Glossary, Taxa
        Session("GalleryMode") = ""     '3Thumb, 3ThumbDefault, HeaderThumb, Full, Text
        Session("DVGallery") = Nothing
        Session("PhotoArrayCurrRow") = -1    '0-based row pointer.  -1 for no DVGallery population
        Session("PhotoID1") = ""
        Session("PhotoID2") = ""
        Session("PhotoID3") = ""
        'Session("GalleryDefault") = False   'True if we're to apply gallery photos to default taxon in BuildGuide
        'Session("GalleryReturn") = ""   'The URL to return to from the Gallery

    End Sub

End Class