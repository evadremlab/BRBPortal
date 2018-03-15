Imports QSILib

Public Class fShowTable
    ''' <summary>
    ''' Modal form to show table information, either from a single Table reference or from a DataSet reference
    ''' BHS 4/4/08
    ''' </summary>
    ''' 
    Public iDS As DataSet = Nothing
    Public iTable As DataTable = Nothing
    Public iDSInternal As New DataSet

    ''' <summary> Load </summary>
    Private Sub fShowTable_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Show one table
        If iTable IsNot Nothing Then
            ShowTable(iTable)
            Return
        End If

        'Show many tables
        If iDS IsNot Nothing Then
            Dim Values As String = ""
            For Each T As DataTable In iDS.Tables
                Values += T.TableName & "=" & T.TableName & ","
            Next
            Values = Mid(Values, 1, Values.Length - 1)
            FillComboBox(Values, CType(cbTables, ComboBox))
        End If
    End Sub

    ''' <summary> Build Table ComboBox </summary>
    Private Sub cbTables_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbTables.SelectedValueChanged
        ShowTable(iDS.Tables(cbTables.SelectedValue.ToString))
    End Sub

    ''' <summary> Show Selected Table </summary>
    Sub ShowTable(ByVal aT As DataTable)
        If aT IsNot Nothing Then
            Dim T As New DataTable
            iDSInternal.Clear()
            iDSInternal.EnforceConstraints = False
            iDSInternal.Tables.Add(T)
            T.Columns.Add("RowState", GetType(System.String))
            T.Merge(aT, True)
            For Each R As DataRow In T.Rows
                'DJW 06/06/2013 - Added next line so showtable would would work properly
                If R.RowState = DataRowState.Deleted Then Continue For
                R.Item("RowState") = R.RowState.ToString()
            Next
            gvTable.DataSource = T
            Me.Text = aT.TableName
        End If
    End Sub

End Class