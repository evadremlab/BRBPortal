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

    Public Class fBrowse2
        Inherits System.Windows.Forms.Form

        Protected WithEvents iGV As qGVList
        Protected iDV As DataView
        Protected iBS As New BindingSource
        Protected iClientTextBox As qTextBox      'Client textbox we write codes back into
        Protected iAccept As Boolean = False      'Flags that we're leaving with user clicking Accept button  
        Private iFilter As String = ""
        Private iInMultiEventProcess As Boolean = False
        Public iDescrStr As String = ""

        Protected Event OnLoadBrowse()         'Define iGV in derived class

#Region "--------------------------- Setup and Load -------------------------------"

        'Setup, called from client program, before Load
        'Copies in DV and client textbox reference
        Public Function Setup(ByRef aDV As DataView, ByRef aTxtCode As qTextBox) As Boolean

            iDV = aDV
            iClientTextBox = aTxtCode
            txtCode.Text = aTxtCode.Text

            Return True

        End Function

        'Load Browse Form
        Private Sub fBrowse_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            RaiseEvent OnLoadBrowse()
            SetGVProperties()
            Me.BringToFront()
            If fBrowse.ActiveForm IsNot Nothing Then
                iGV.DataSource = iBS
                iBS.DataSource = iDV
                iBS.Filter = ""
                iGV.Sort(iGV.Columns("code"), System.ComponentModel.ListSortDirection.Ascending)
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
                TryError("Name Key Entry error", ex)

            Finally
                iInMultiEventProcess = False

            End Try

        End Sub

        'User enters Find Description
        Private Sub txtDescr_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDescr.TextChanged
            If iInMultiEventProcess Then Return 'Only process if this routine caused by user typing

            Try
                iInMultiEventProcess = True
                iGV.Sort(iGV.Columns("descr"), System.ComponentModel.ListSortDirection.Ascending)

                txtCode.Text = ""
                FindText(txtDescr.Text, "descr", False)

            Catch ex As Exception
                TryError("Organization Entry error", ex)

            Finally
                iInMultiEventProcess = False

            End Try
        End Sub

        'Use Enters Filter
        Private Sub txtFilter_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFilter.TextChanged

            Try
                Dim gvFilter As String = BuildGVFilter(iGV, txtFilter.Text)
                iBS.Filter = gvFilter

                If txtCode.Text.Length > 0 Then FindText(txtCode.Text, "code", False)
                If txtDescr.Text.Length > 0 Then FindText(txtDescr.Text, "descr", False)
                ShowRowCount()

            Catch ex As Exception
                TryError("Filter error", ex)
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
                TryError("Dropdown click error", ex)
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

        'As window closes, update iClientTextBox
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
                        Else
                            'Descr = DescrStr
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

        'This should probably be moved to qGV, to guarantee a good untrapped GV error report
        Private Sub gvDD_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles iGV.DataError
            MsgBox(e.Exception)
            e.Cancel = True
        End Sub

#End Region

#Region "--------------------------- Subroutines and Functions -------------------------------"
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

        'Toggle check box on this row
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

        'Set iGV Properties
        Sub SetGVProperties()
            If fBrowse.ActiveForm IsNot Nothing Then
                iGV.ColumnHeadersDefaultCellStyle.BackColor = QColHeaderBackColor
            End If
        End Sub

        'Display current GV rowcount
        Sub ShowRowCount()
            lblRows.Text = Format(iGV.Rows.Count, "N0") & " rows"
        End Sub

        Sub ClearSelectedRows()
            iBS.Filter = ""
            If fBrowse.ActiveForm IsNot Nothing Then
                For Each R As DataGridViewRow In iGV.Rows
                    R.Cells("Sel").Value = 0
                    R.Cells("SortSel").Value = 0
                    iBS.EndEdit()
                Next
            End If

            ApplyFilter()

        End Sub

        'Apply Filter
        Sub ApplyFilter()
            iFilter = BuildGVFilter(iGV, txtFilter.Text)
            FilterTimer.Start()
        End Sub

        'Filter work done after any entry process is complete
        Private Sub FilterTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FilterTimer.Tick
            FilterTimer.Stop()

            iBS.Filter = iFilter
            If txtCode.Text.Length > 0 Then FindText(txtCode.Text, "code", False)
            If txtDescr.Text.Length > 0 Then FindText(txtDescr.Text, "descr", False)
            ShowRowCount()
        End Sub

        'Look for a string in GV and scroll to that first matching row
        Function FindText(ByVal aText As String, ByVal aColName As String, ByVal aSelectOnFullMatch As Boolean) As Boolean
            Dim i As Integer = aText.Length
            Dim LastR As DataGridViewRow

            For Each R As DataGridViewRow In iGV.Rows
                LastR = R
                If Mid(GetItemString(R, aColName).ToUpper, 1, i) = aText.ToUpper Then
                    iGV.FirstDisplayedCell = R.Cells("code")
                    If GetItemString(R, aColName).ToUpper = aText.ToUpper And aSelectOnFullMatch Then
                        R.Cells("Sel").Value = 1
                        iBS.EndEdit()
                        Return True
                    End If
                    Return False
                    'If no match, but we've passed alphabetically, position to last row below aText
                ElseIf Mid(GetItemString(R, aColName).ToUpper, 1, i) > aText.ToUpper Then
                    iGV.FirstDisplayedCell = LastR.Cells("code")
                    Return False
                End If
            Next
            Return False

        End Function
#End Region


    End Class

End Namespace
