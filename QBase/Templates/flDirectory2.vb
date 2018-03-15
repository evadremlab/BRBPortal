Imports QSILib
Imports System.IO

Public Class flDirectory

    'iPath is the full path to return to the calling program
    'txtPath.Text is the path as it stands now, before returning to the calling program
    Public iPath As String = ""         
    Private iCallingDocPath As qTextBox
    Private iDrive As String = ""
    Private iIsFillingComboBox As Boolean = False

    'BHS 4/3/09 Added to allow flSaveFile to inherit from this
    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub New(ByVal aDrive As String, ByVal aDocPath As String, ByVal aViewLocalNetwork As Boolean, ByVal aCallingDocPath As qTextBox)
        Try
            InitializeComponent()
            iIsLoading = True
            iDrive = aDrive
            txtDocPath.Text = aDocPath
            chViewLocalNetwork.Checked = aViewLocalNetwork
            iCallingDocPath = aCallingDocPath
            gvFiles.ColumnHeadersDefaultCellStyle.BackColor = QColHeaderBackColor
        Catch ex As Exception
            ShowError("Trouble loading Directory Browse window", ex)
        End Try
    End Sub

    'Load Form
    Private Sub flDirectory_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            FillDrives()
            FillDir()
            iIsLoading = False
        Catch ex As Exception
            ShowError("Trouble loading Directory Browse", ex)
        End Try
    End Sub

    'Fill Drives dropdown
    Sub FillDrives()

        Dim Drives As String = ""
        Dim Possibles As String = "CDEFGHIJKLMNOPQRSTUVWXYZ"    'Exclude Floppies to improve performance and possibly improve security
        For i As Integer = 1 To 24
            Dim Letter As String = Mid(Possibles, i, 1)
            Try
                If chViewLocalNetwork.Checked = True Then
                    If Directory.GetDirectories("//client/" & Letter & "$").Count > 0 Then
                        If Drives.Length > 0 Then Drives &= ","
                        Drives &= Letter & "=" & Letter
                    End If
                Else
                    If Directory.GetDirectories(Letter & ":").Count > 0 Then
                        If Drives.Length > 0 Then Drives &= ","
                        Drives &= Letter & "=" & Letter
                    End If
                End If

            Catch ex As Exception
                Continue For
            End Try
        Next
        iIsFillingComboBox = True
        FillComboBox(Drives, CType(cbDrive, ComboBox))
        iIsFillingComboBox = False
        cbDrive.Text = iDrive

    End Sub

    'Fill Directories dropdown
    Sub FillDir()
        If chViewLocalNetwork.Checked = True Then
            iPath = "//client/" & cbDrive.Text & "$"
        Else
            iPath = cbDrive.Text & ":\"
        End If
        If txtDocPath.Text.Length > 0 Then
            If Mid(txtDocPath.Text, 1, 1) <> "\" Then iPath &= "\"
            iPath &= txtDocPath.Text
        End If

        Dim T As New DataTable
        T.Columns.Add("DName", GetType(String))
        T.Columns.Add("FName", GetType(String))

        Try

            For Each DirName As String In Directory.GetDirectories(iPath)
                Dim R As DataRow = T.NewRow
                Dim wstr As String = DirName
                R.Item("DName") = RightParseStr(wstr, "\")
                T.Rows.Add(R)
            Next

        

            For Each FileName As String In Directory.GetFiles(iPath)
                Dim R As DataRow = T.NewRow
                Dim wstr As String = FileName
                R.Item("FName") = RightParseStr(wstr, "\")
                T.Rows.Add(R)
            Next

        Catch ex As Exception
            MsgBoxErr("Path is not valid, or you don't have access rights to it")
            RightParseStr(txtDocPath.Text, "\")
            FillDir()
            Return
        End Try

        gvFiles.DataSource = T
    End Sub

    'User clicked a cell in the directory/file name display
    Private Sub gvFiles_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gvFiles.CellClick
        Try
            If e.RowIndex < 0 Then Return

            If e.ColumnIndex = 0 Then
                txtDocPath.Text &= "\" & gvFiles.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString
                SubstituteAllStr(txtDocPath.Text, "\\", "\")
                FillDir()
            End If

            If e.ColumnIndex = 1 Then
                iCallingDocPath.Text = iPath & "\" & gvFiles.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString
                SubstituteAllStr(iCallingDocPath.Text, "\\", "\")
                Close()
            End If
        Catch ex As Exception
            ShowError("Trouble handling cell click in Directory/File List", ex)
        End Try
    End Sub

    'User finishes typing and leaves path textbox
    Private Sub txtDocPath_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtDocPath.Validating
        Try
            FillDir()
        Catch ex As Exception
            ShowError("Trouble filling in directory names", ex)
        End Try
    End Sub

    'User changes View Local Network checkbox
    Private Sub chViewLocalNetwork_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chViewLocalNetwork.CheckedChanged
        Try
            If iIsLoading = False Then
                txtDocPath.Text = ""
                FillDrives()
                FillDir()
            End If
        Catch ex As Exception
            ShowError("Trouble switching between local and server directories", ex)
        End Try
    End Sub

    'User clicks on Parent Directory label
    Private Sub lblParentDir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblParentDir.Click
        Try
            RightParseStr(txtDocPath.Text, "\")
            FillDir()
        Catch ex As Exception
            ShowError("Trouble linking to parent directory", ex)
        End Try
    End Sub

    'User changes Drive in dropdown control
    Private Sub cbDrive_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbDrive.SelectedIndexChanged
        Try
            If iIsLoading = False And iIsFillingComboBox = False Then
                iDrive = cbDrive.Text
                txtDocPath.Text = ""
                FillDir()
            End If
        Catch ex As Exception
            ShowError("Trouble changing drive", ex)
        End Try
    End Sub

End Class