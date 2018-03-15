'Array of Entry fields and their original values, for checking Dirty status
'   GV filled in for Grid View entry fields, with C empty
'   C filled in for non-Grid View entry fields, with GV empty

Imports QSILib


Public Class EntryField
    Public OrigValue As String = ""

    'Gridview entry values
    Public GV As DataGridView
    Public ColName As String = ""
    Public Row As Integer = 0

    'Other control entry values
    Public C As Control

    Function GetCurrentValue() As String
        Try
            If GV IsNot Nothing Then
                If GV.Rows.Count > 0 And Row > -1 And Row < GV.Rows.Count Then
                    Return GV.Rows(Row).Cells(ColName).FormattedValue.ToString
                End If
            End If

        Catch ex As Exception
            TryError("Problem in EField.GetCurrentValue", ex)
        End Try

        If C IsNot Nothing Then
            If TypeOf C Is TextBox Or TypeOf C Is qTextBox Or TypeOf C Is qMaskedTextBox Or TypeOf C Is ComboBox Or TypeOf C Is qComboBox Then
                Return C.Text
            End If
        End If

        Return ""
    End Function

    Function GetDataType() As String
        Dim DataType As String = "str"
        Dim S As String

        If C IsNot Nothing Then
            If TypeOf C Is TextBox Then
                If C.Tag IsNot Nothing Then
                    S = C.Tag.ToString
                    qFunctions.ParseStr(S, "|")
                    If Left(S, 3).ToLower = "num" Then Return "num"
                    If Left(S, 3).ToLower = "dat" Then Return "dat"
                End If
            End If
        End If
        Return DataType
    End Function

    Function GetControlType() As String
        Return C.GetType.ToString.ToLower
    End Function

    Function ResetOrigValue() As Boolean
        Me.OrigValue = Me.GetCurrentValue
    End Function

    'Sub Clear()
    '    OrigValue = ""
    '    GV = Nothing
    '    C = Nothing
    '    ColName = ""
    '    Row = 0

    'End Sub

End Class

