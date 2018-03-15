Imports QSILib

Public Class ctxParams
    Public iParams As ArrayList = New ArrayList

    Function AddParam(ByVal aName As String, ByVal aValue As String) As Integer
        Dim P As New strParam

        P.Name = aName
        P.Value = aValue
        Return iParams.Add(P)   'Returns the index of the new line

    End Function

    Function ParamsToDV() As DataView
        'Set up an empty DV with Name and Value fields
        Dim DV As DataView

        'If Appl.ConnType = "ODBC" Then     BHS 7/10/08
        'DV = BuildDV("Select 'x' Name, 'y' Value From system Where 1 = 2", False)
        'Else
        DV = BuildDV("Select 'x' Name, 'y' Value Where 1 = 2", False)
        'End If

        Dim P As New strParam

        For Each P In iParams
            Dim DR As DataRow = DV.Table().Rows.Add
            DR.Item(0) = P.Name
            DR.Item(1) = P.Value
        Next

        Return DV

    End Function

    Function Clear() As Boolean
        iParams.Clear()
        Return True
    End Function



    Public Name As String = ""
    Public Value As String = ""
    Public Type As String = ""

    Function GetValue() As Object
        Select Case Type.ToLower
            Case "", "string", "str"
                Return Value
            Case "integer"
                Return CInt(Value)
            Case "datetime"
                Return CType(Value, DateTime)
            Case "date"
                Return CType(Value, Date)
            Case "decimal", "dec", "num", "number"
                Return CType(Value, Decimal)
            Case Else
                Return Value
        End Select

    End Function

End Class
