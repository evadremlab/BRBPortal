Imports QSILib

'Ancestor of client-level authority functions
Public Class Authority

    Public Overridable Function IsReader(ByVal aLogName As String, ByVal aFunctName As String) As Boolean
        Return False  'Require client-level authority approval
    End Function

    ''' <summary> isCampusWriter(Campus from this record, Type of campus such as HomeLic, HomePros, HomeAcct) </summary>
    Public Overridable Function isCampusWriter(ByVal aCampus As String, ByVal aFieldName As String) As Boolean
        Return False
    End Function

    Public Overridable Function HasAuthority(ByVal aLogName As String, ByVal aFunctName As String, ByVal aMinAuth As Integer) As Boolean
        Return False
    End Function

    Public Overridable Function IsWriter(ByVal aFunctName As String) As Boolean
        Return False  'Require client-level authority approval
    End Function

    'Get LogName from Context.User.Identity.Name, stripping leading machine name
    Public Function GetLogName() As String
        Dim LogName As String = My.User.Name

        'Remove everything up to \
        While LogName.IndexOf("\") > 0
            LogName = Mid(LogName, LogName.IndexOf("\") + 2)
        End While

        Return LogName

    End Function

    'Get LogName returning no more than a given length
    Public Function GetLogName(ByVal aLength As Integer) As String
        Dim wstr As String = GetLogName()
        Return Mid(wstr, 1, aLength)
    End Function


    'Get User intials based on GetLogName
    Public Overridable Function GetInits() As String
        Return "XXX"  'Require client-level function
    End Function

    Public Overridable Function GetPermLevel(ByVal aResourceName As String) As Integer
        Return Nothing  'Require client-level function
    End Function

End Class
