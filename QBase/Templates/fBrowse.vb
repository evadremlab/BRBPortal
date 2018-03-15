Imports System.Data
Imports System.Data.SqlClient
Imports System.Transactions
Imports System.Windows.Forms
Imports QSILib

Namespace Windows.Forms
    'FBrowse is an ancestor for a Browse Window.  The Browse Window descendant provides the gridview and prompts, but all logic is managed here.
    'The Browse Window descendant is called from a client program, which provides a DV and a Textbox reference.
    'Then the user manipulates browser rows and marks different rows
    'On Accept, write selected values back to client text box
    '11/21/07 BHS Make Code column the secondary sort on any other sort

    Public Class fBrowse
        Inherits System.Windows.Forms.Form

        Protected WithEvents iGV As qGVList
        Protected iDV As DataView
        Protected iBS As New BindingSource
        'BHS 10/05/09 Change from qTextBox to Control so masked Textbox can be used as well
        Protected iClientTextBox As Control      'Client textbox we write codes back into
        Protected iAccept As Boolean = False      'Flags that we're leaving with user clicking Accept button  
        Private iFilter As String = ""
        Private iInMultiEventProcess As Boolean = False
        Public iDescrStr As String = ""

        'BHS 8/19/10 make sure complex clear completes before filtertimer completes
        Private iClearingSelectedRows As Boolean = False
        Private iErrorShown As Boolean = False

        Protected Event OnLoadBrowse()         'Define iGV in derived class

#Region "--------------------------- Setup and Load -------------------------------"

        '''<summary> Setup, called from client program, before Load
        ''' Copies in DV and client textbox reference </summary>
        Public Function Setup(ByRef aDV As DataView, ByRef aTxtCode As Control) As Boolean

            iDV = aDV
            iClientTextBox = aTxtCode
            txtCode.Text = aTxtCode.Text

            Return True

        End Function

        '''<summary> Load Browse Form </summary>
        Private Sub fBrowse_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                RaiseEvent OnLoadBrowse()
            Catch ex As Exception
                ShowError("Unexpected error preparing browse form (fBrowse OnLoadBrowse)", ex)
            End Try

            SetGVProperties()
            Me.BringToFront()
            'If ActiveForm IsNot Nothing Then
            If Not InDevEnv() Then
                iBS.DataSource = iDV
                iBS.Filter = ""
                iGV.DataSource = iBS
                'iGV.Sort(iGV.Columns("code"), System.ComponentModel.ListSortDirection.Ascending)

                'Look to see if IDV.Sort is already populated before overwritting it - DJW 08/21/12
                If iDV.Sort Is Nothing OrElse iDV.Sort = "" Then iDV.Sort = iGV.Columns("code").DataPropertyName
                MarkRowsFromClientTextBox()
                ShowRowCount()
            End If
            txtCode.Focus()
        End Sub
#End Region

#Region "--------------------------- Events -------------------------------"
        'User enters Find Code
        Private Sub txtCode_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCode.TextChanged
            If iInMultiEventProcess Then Return 'Only process if this routine caused by user typing

            Try
                iInMultiEventProcess = True
                If iGV IsNot Nothing Then
                    iGV.Sort(iGV.Columns("code"), System.ComponentModel.ListSortDirection.Ascending)
                    txtDescr.Text = ""
                    FindText(txtCode.Text, "code", False)
                End If

            Catch ex As Exception
                If ex.Message.IndexOf("iBindingList") > -1 Then Exit Try
                ShowError("Name Key Entry error", ex)

            Finally
                iInMultiEventProcess = False

            End Try

        End Sub

        'User enters Find Description
        Private Sub txtDescr_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDescr.TextChanged
            If iInMultiEventProcess Then Return 'Only process if this routine caused by user typing

            Try
                iInMultiEventProcess = True
                If iGV IsNot Nothing Then
                    iGV.Sort(iGV.Columns("code"), System.ComponentModel.ListSortDirection.Ascending)
                    iGV.Sort(iGV.Columns("descr"), System.ComponentModel.ListSortDirection.Ascending)
                    'iDV.Sort = iGV.Columns("descr").DataPropertyName & ", " & iGV.Columns("code").DataPropertyName
                End If

                txtCode.Text = ""
                FindText(txtDescr.Text, "descr", False)

            Catch ex As Exception
                ShowError("Organization Entry error", ex)

            Finally
                iInMultiEventProcess = False

            End Try
        End Sub

        'User Enters Filter
        Private Sub txtFilter_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFilter.TextChanged

            Try
                Dim gvFilter As String = BuildGVFilter(iGV, txtFilter.Text)
                iBS.Filter = gvFilter

                If txtCode.Text.Length > 0 Then FindText(txtCode.Text, "code", False)
                If txtDescr.Text.Length > 0 Then FindText(txtDescr.Text, "descr", False)
                ShowRowCount()

            Catch ex As Exception
                ShowError("Filter error", ex)
            End Try
        End Sub

        'User clicks row to highlight it
        Private Sub gvDD_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles iGV.CellContentClick
            Try
                If e.RowIndex >= 0 Then 'Didn't click the header, or some spot that can't be linked to a row
                    If Control.ModifierKeys = Keys.Shift Then   'Shift Click
                        For i As Integer = e.RowIndex To 0 Step -1
                            If CInt(iGV.Rows(i).Cells("Sel").Value) = 1 Then Return
                            MarkRow(iGV.Rows(i))
                        Next
                    Else
                        MarkRow(iGV.Rows(e.RowIndex))
                    End If
                End If
            Catch ex As Exception
                ShowError("Dropdown click error", ex)
            End Try
        End Sub

        'Clear Button
        Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click
            ClearSelectedRows()
        End Sub

        'Accept Button
        Private Sub btnAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccept.Click
            iAccept = True
            Me.Close()
        End Sub

        'Cancel Button
        Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            iAccept = False
            Me.Close()
        End Sub

        '''<summary> As window closes, update iClientTextBox </summary>
        Private Sub ddFrm_Deactivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Deactivate
            Dim SelectedRows As Integer = 0

            If iAccept = True Then
                iBS.Filter = ""  'So we can find all the accepted rows
                Dim SaveCode As String = iClientTextBox.Text
                iClientTextBox.Text = ""


                For Each R As DataGridViewRow In iGV.Rows

                    If CInt(R.Cells("Sel").Value) = 1 Then
                        If SelectedRows = 0 Then
                            iClientTextBox.Text = CStr(R.Cells("code").FormattedValue)
                            'Must set Descr after Code, since change to code in 611 erases description
                            iDescrStr = CStr(R.Cells("descr").FormattedValue) & "=" & SelectedRows.ToString
                            SelectedRows += 1
                            'ElseIf SelectedRows > 1000 Then
                            '    'SRM 03/23/2015 if more than 1000 rows are selected, stop filling combobox
                            '    If Not iErrorShown Then
                            '        iErrorShown = True
                            '        MsgBox("Only 1000 records are allowed to be selected", , "Too many rows selected")
                            '    End If
                            '    Exit For
                        Else

                            iClientTextBox.Text &= "|" & CStr(R.Cells("code").FormattedValue)
                            iDescrStr &= "|" & CStr(R.Cells("descr").FormattedValue) & "=" & SelectedRows.ToString
                            SelectedRows += 1
                        End If
                    End If
                Next

                If iDescrStr.Length = 0 Then iDescrStr = "="
            Else
                iDescrStr = "CancelButton"
            End If
        End Sub


        'ToDo This should probably be moved to qGV, to guarantee a good untrapped GV error report
        Private Sub gvDD_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles iGV.DataError
            MsgBox(e.Exception)
            e.Cancel = True
        End Sub

#End Region

#Region "--------------------------- Subroutines and Functions -------------------------------"

        '''<summary> Upon loading, mark rows referenced in Client Text Box, and put the first row's key in txtCode.Text </summary>
        Overridable Function MarkRowsFromClientTextBox() As Boolean
            ClearSelectedRows() 'Turn all Sel values to 0
            Dim S As String = iClientTextBox.Text
            Dim Selected As Boolean = False
            While S.Length > 0
                Dim NameKey As String = Trim(ParseStr(S, gOrSymbol))
                If FindText(NameKey, "code", True) Then Selected = True 'True parameter marks row selected
            End While
            If Selected Then    'Show first row
                For Each R As DataGridViewRow In iGV.Rows
                    If CInt(R.Cells("Sel").Value) = 1 Then
                        txtCode.Text = CStr(R.Cells("code").Value)
                        Exit For
                    End If
                Next
            Else
                txtCode.Text = iClientTextBox.Text 'Incomplete code - point to first record
            End If
        End Function

        '''<summary> Toggle check box on this row </summary>
        Sub MarkRow(ByVal aR As DataGridViewRow)

            With aR.Cells("Sel")
                If CInt(.Value) = 0 Then
                    .Value = 1
                Else
                    .Value = 0
                End If
            End With
            iBS.EndEdit()

        End Sub

        '''<summary> Set iGV Properties </summary>
        Sub SetGVProperties()
            'If ActiveForm IsNot Nothing Then
            If Not InDevEnv() Then
                iGV.ColumnHeadersDefaultCellStyle.BackColor = QColHeaderBackColor
            End If
        End Sub

        '''<summary> Display current GV rowcount </summary>
        Sub ShowRowCount()
            lblRows.Text = Format(iGV.Rows.Count, "N0") & " rows"
        End Sub

        '''<summary> Clear selected rows </summary>
        Sub ClearSelectedRows()
            iClearingSelectedRows = True    'BHS 8/19/10

            iBS.Filter = ""
            'If ActiveForm IsNot Nothing Then
            If Not InDevEnv() Then
                For Each R As DataGridViewRow In iGV.Rows
                    R.Cells("Sel").Value = 0
                    R.Cells("SortSel").Value = 0
                    iBS.EndEdit()
                Next
            End If

            iClearingSelectedRows = False
            ApplyFilter()

        End Sub

        '''<summary> Apply Filter </summary>
        Sub ApplyFilter()
            iFilter = BuildGVFilter(iGV, txtFilter.Text)
            FilterTimer.Start()
        End Sub

        '''<summary> Filter work done after any entry process is complete </summary>
        Private Sub FilterTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FilterTimer.Tick
            'BHS 8/19/10 don't stop timer if we're still clearing rows
            If iClearingSelectedRows = False Then
                FilterTimer.Stop()

                iBS.Filter = iFilter
                If txtCode.Text.Length > 0 Then FindText(txtCode.Text, "code", False)
                If txtDescr.Text.Length > 0 Then FindText(txtDescr.Text, "descr", False)
                ShowRowCount()
            End If
            
        End Sub

        '''<summary> Look for a string in GV and scroll to that first matching row </summary>
        Function FindText(ByVal aText As String, ByVal aColName As String, ByVal aSelectOnFullMatch As Boolean) As Boolean
            Dim i As Integer = aText.Length
            Dim LastR As DataGridViewRow
            'BHS add numeric logic 9/1/10
            If aColName = "code" And txtCode._DataType = DataTypeEnum.Num Then

                For Each R As DataGridViewRow In iGV.Rows
                    LastR = R
                    Dim Num1, Num2 As Decimal
                    If Not IsNumeric(aText) Then  'SRM - 02/10/11 - changed txtCode.Text to aText
                        Num1 = 0
                    Else
                        Num1 = CDec(aText) 'SRM - 02/10/11 - changed txtCode.Text to aText
                    End If
                    If Not IsNumeric(R.Cells("code").Value.ToString) Then
                        Num2 = 0
                    Else
                        Num2 = CDec(R.Cells("code").Value.ToString)
                    End If

                    If Num1 = Num2 Then
                        If R.Cells("code").Visible Then iGV.FirstDisplayedCell = R.Cells("code")
                        If aSelectOnFullMatch Then
                            R.Cells("Sel").Value = 1
                            iBS.EndEdit()
                            Return True
                        End If
                        Return False

                    ElseIf Num2 > Num1 Then
                        iGV.FirstDisplayedCell = LastR.Cells("code")
                        Return False
                    End If
                Next

            Else

                For Each R As DataGridViewRow In iGV.Rows
                    LastR = R
                    If Mid(GetItemString(R, aColName).ToUpper, 1, i) = aText.ToUpper Then
                        If R.Cells("code").Visible Then iGV.FirstDisplayedCell = R.Cells("code")
                        If GetItemString(R, aColName).ToUpper = aText.ToUpper And aSelectOnFullMatch Then
                            R.Cells("Sel").Value = 1
                            iBS.EndEdit()
                            Return True
                        End If
                        Return False
                        'If no match, but we've passed alphabetically, position to last row below aText
                    ElseIf Mid(GetItemString(R, aColName).ToUpper, 1, i) > aText.ToUpper Then
                        If LastR.Cells("code").Visible Then iGV.FirstDisplayedCell = LastR.Cells("code")
                        Return False
                    End If
                Next
            End If
            Return False
        End Function

        '''<summary> Multi-column sort: always sort on code after primary sort </summary>  'BHS 11/21/07
        Private Sub iGV_Sorted(ByVal sender As Object, ByVal e As System.EventArgs) Handles iGV.Sorted
            Dim Col As DataGridViewColumn = iGV.SortedColumn
            'Leave GlyphDirection alone, and re-sort including Code column
            If Col.HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending Then
                iDV.Sort = Col.DataPropertyName & ", " & iGV.Columns("code").DataPropertyName
                Col.HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending
            Else
                iDV.Sort = Col.DataPropertyName & " DESC, " & iGV.Columns("code").DataPropertyName
                Col.HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Descending
            End If
        End Sub

#End Region

    End Class

End Namespace
