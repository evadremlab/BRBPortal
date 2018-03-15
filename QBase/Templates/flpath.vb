Imports QSILib
Imports System.IO

Public Class flpath

    'iPath is the full path to return to the calling program
    'txtPath.Text is the path as it stands now, before returning to the calling program
    Public iPath As String = ""
    Public iAccepted As Boolean = False
    Private iTitle As String = ""
    Private iCallingPath As String
    Private iDrive As String = ""
    Private iError As Boolean = False
    Private iIsFillingComboBox As Boolean = False

    Public Sub New(ByVal aDrive As String, ByVal aPath As String, ByVal aViewLocalNetwork As Boolean, Optional ByVal Title As String = "")
        Try
            InitializeComponent()
            iIsLoading = True
            iDrive = aDrive
            iTitle = Title
            txtPath.Text = aPath
            chViewLocalNetwork.Checked = aViewLocalNetwork
            iCallingPath = aPath
            gvFiles.ColumnHeadersDefaultCellStyle.BackColor = QColHeaderBackColor
        Catch ex As Exception
            ShowError("Trouble loading Directory Browse window", ex)
        End Try
    End Sub

    'Load Form
    Private Sub flPath_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If iTitle <> "" Then
                Me.Text = iTitle
            End If
            FillDrives()
            FillDir()
            If iError = True Then
                iAccepted = False
                Me.Close()
            End If
            iIsLoading = False
        Catch ex As Exception
            ShowError("Trouble loading Directory Browse", ex)
        End Try
    End Sub

    'Fill Drives dropdown
    Sub FillDrives()

        Dim Drives As String = ""
        Dim Possibles As String = "CDEFGHIJKLMNOPQRSTUVWXYZ"    'Exclude Floppies to improve performance and possibly improve security
        If chViewLocalNetwork.Checked = False Then Possibles = "EFGHIJKLMNOPQRSTUVWXYZ" 'Don't show C and D drives on Citrix
        For i As Integer = 1 To 24
            Dim Letter As String = Mid(Possibles, i, 1)
            Try
                If chViewLocalNetwork.Checked = True Then
                    If Directory.GetDirectories("//client/" & Letter & "$").Count > 0 Then
                        If Drives.Length > 0 Then Drives &= ","
                        Drives &= Letter & "=" & Letter
                    End If
                Else
                    'BHS 04/03/09  broadened the test - If Directory.GetDirectories(Letter & ":").Count > 0 Then   
                    Dim j As Integer = Directory.GetDirectories(Letter & ":").Count()   'If this doesn't fail, we want the drive.
                    If Drives.Length > 0 Then Drives &= ","
                    Drives &= Letter & "=" & Letter
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
        If txtPath.Text.Length > 0 Then
            If Mid(txtPath.Text, 1, 1) <> "\" Then iPath &= "\"
            iPath &= txtPath.Text
        End If

        Dim T As New DataTable
        T.Columns.Add("DName", GetType(String))

        Try

            For Each DirName As String In Directory.GetDirectories(iPath)
                Dim R As DataRow = T.NewRow
                Dim wstr As String = DirName
                R.Item("DName") = RightParseStr(wstr, "\")
                T.Rows.Add(R)
            Next

        Catch ex As Exception
            'BHS 1/7/10 allow user to abort if they have no rights to anything
            If MsgBoxQuestion("Path is not valid. Do you wish to select a new directory now?") = MsgBoxResult.No Then iError = True
            Return
        End Try

        gvFiles.DataSource = T
    End Sub

    'User clicked a cell in the directory/file name display
    Private Sub gvFiles_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gvFiles.CellClick
        Try
            If e.RowIndex < 0 Then Return

            If e.ColumnIndex = 0 Then
                txtPath.Text &= "\" & gvFiles.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString
                SubstituteAllStr(txtPath.Text, "\\", "\")
                FillDir()
            End If

        Catch ex As Exception
            ShowError("Trouble handling cell click in Directory/File List", ex)
        End Try
    End Sub

    'User finishes typing and leaves path textbox
    Private Sub txtDocPath_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtPath.Validating
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
                txtPath.Text = ""
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
            RightParseStr(txtPath.Text, "\")
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
                txtPath.Text = ""
                FillDir()
            End If
        Catch ex As Exception
            ShowError("Trouble changing drive", ex)
        End Try
    End Sub

    Private Sub btnAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccept.Click
        iAccepted = True
        Close()

    End Sub
End Class