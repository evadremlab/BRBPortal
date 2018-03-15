Public Class strParam
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
