Imports QSILib
Imports System.IO

Public Class fSaveFile

    'iPath is the full path to return to the calling program
    'txtPath.Text is the path as it stands now, before returning to the calling program
    Public iPath As String = ""
    Public iSavePath As String = ""
    Private iDrive As String = ""
    Private iIsFillingComboBox As Boolean = False
    Private iExtension As String = ""

    ''' <summary>
    ''' fSaveFile is similar to fDirectory, in that is shows directories and files, but fSaveFile lets you specify a file to be
    ''' saved.  If you choose a file, the file name is put in the FileName textbox.  When you press Save, the program writes a short
    ''' string to the file to make sure this user may write, and then returns the full path to the calling program.
    ''' 
    ''' The File list is filtered based on the extension and the letters you've alredy typed into the File Name textbox.
    '''</summary>

    Public Sub New(ByVal aDrive As String, ByVal aDocPath As String, ByVal aViewLocalNetwork As Boolean, ByRef aSavePath As String, Optional ByVal aExtension As String = "")
        Try
            InitializeComponent()
            iIsLoading = True
            iDrive = aDrive
            txtDocPath.Text = aDocPath
            chViewLocalNetwork.Checked = aViewLocalNetwork
            iSavePath = aSavePath
            iExtension = aExtension.ToLower
            gvFiles.ColumnHeadersDefaultCellStyle.BackColor = QColHeaderBackColor
        Catch ex As Exception
            ShowError("Trouble loading File Save window", ex)
        End Try
    End Sub

    'Load Form
    Private Sub flDirectory_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If iExtension.Length > 0 Then
                Me.Text = "Save " & iExtension.ToUpper & " File"
            End If
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
                    'If Directory.GetDirectories(Letter & ":").Count > 0 Then
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
                'Filter for files with the named extension
                If iExtension.Length > 0 Then
                    If FileName.ToLower.IndexOf(iExtension) = -1 Then Continue For
                End If

                'Filter for files that start with FileName Text
                Dim wstr As String = FileName
                Dim FName As String = RightParseStr(wstr, "\")
                If txtFileName.Text.Length > 0 Then
                    If Mid(FName.ToLower, 1, txtFileName.Text.Length) <> txtFileName.Text.ToLower Then Continue For
                End If

                Dim R As DataRow = T.NewRow
                R.Item("FName") = FName
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

            If e.ColumnIndex = 0 Then   'Clicked in Directory colum
                txtDocPath.Text &= "\" & gvFiles.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString
                SubstituteAllStr(txtDocPath.Text, "\\", "\")
                FillDir()
            End If

            If e.ColumnIndex = 1 Then   'Clicked in File column
                txtFileName.Text = gvFiles.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString
                SubstituteAllStr(txtFileName.Text, "\\", "\")
            End If
        Catch ex As Exception
            ShowError("Trouble handling cell click in Directory/File List", ex)
        End Try
    End Sub

    'User finishes typing and leaves Path textbox
    Private Sub txtDocPath_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtDocPath.Validating
        Try
            FillDir()
            SetFocusControl("txtFileName")
        Catch ex As Exception
            ShowError("Trouble filling in directory names", ex)
        End Try
    End Sub

    'User finishes typing and leaves File Name textbox
    Private Sub txtFileName_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtFileName.Validating
        Try
            FillDir()
            SetFocusControl("btnSave")
        Catch ex As Exception
            ShowError("Trouble filling in file names", ex)
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

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            Dim FName As String = txtFileName.Text

            If FName.Length = 0 Then
                MsgBox("File Name is required", MsgBoxStyle.Exclamation, "Can't Save File")
                Return
            End If

            If FName.Length < iExtension.Length + 2 Then
                FName &= "." & iExtension
                txtFileName.Text = FName
            End If

            'BHS 4/7/09 Append .xls if file name doesn't end in .xls

            If FName.ToLower.IndexOf("." & iExtension, FName.Length - (iExtension.Length + 1)) < 0 Then
                txtFileName.Text = FName & "." & iExtension
            End If

            iSavePath = iPath & "\" & txtFileName.Text
            SubstituteAllStr(iSavePath, "\\", "\")

            If File.Exists(iSavePath) Then
                If MsgBoxQuestion("OK to overwrite this file?") = MsgBoxResult.No Then
                    iSavePath = ""  'Indicate path not chosen, so calling program doesn't try to use it
                Else
                    'Try writing to file, to create an error if user doesn't have rights
                    ' Make error not fatal and user friendly - GBV 4/11/2014 (Ticket 3066)
                    Try
                        File.WriteAllText(iSavePath, "test")
                    Catch ex As Exception
                        If ex.Message.ToLower.IndexOf("because it is being used by another process") > -1 Then
                            MsgBox("The process cannot overwrite " & iSavePath & " because it is being used by another process.", _
                                   MsgBoxStyle.Exclamation, "Can't Overwrite File")
                        ElseIf ex.Message.ToLower.IndexOf("access denied") > -1 Then
                            MsgBox("The process cannot overwrite " & iSavePath & " because you do not have sufficient file permissions.", _
                                   MsgBoxStyle.Exclamation, "Can't Overwrite File")
                        Else
                            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Can't Overwrite File")
                        End If
                        iSavePath = ""
                        Return ' Not fatal
                    End Try
                End If
            End If

            Close()
        Catch ex As Exception
            If ex.Message.ToLower.IndexOf("part of the path") > -1 Then
                MsgBox("Invalid Path - try again", MsgBoxStyle.Exclamation, "Can't Save File")
                iSavePath = ""
                Return
            ElseIf ex.Message.ToLower.IndexOf("access to the path") > -1 Then
                MsgBox("Cannot write to that file", MsgBoxStyle.Exclamation, "Can't Save File")
                iSavePath = ""
                Return
            Else
                ShowError("Trouble writing to specified file", ex)
            End If
        End Try
    End Sub

    
End Class
