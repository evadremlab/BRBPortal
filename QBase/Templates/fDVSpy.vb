'CAN I SCAN A FORM FOR IT'S DVs?  These are not controls...

Public Class fDVSpy

    'Instance Variables
    Public iFrm As System.Windows.Forms.Form

    'Call fDVSpy with reference back to calling form
    Public Sub New(ByVal aFrm As System.Windows.Forms.Form)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        'Load instance variable
        iFrm = aFrm

    End Sub

    

    'Load event
    Private Sub fDVSpy_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Fill dropdown with all Dataviews in the calling form
            Dim DV As DataView = BuildDV("Select '' Name From case_base Where 1 = 2", False)

            LoadDVs(DV, iFrm)

            DV.Sort = "Name"

            cbDVList.DisplayMember = "Name"
            cbDVList.DataSource = DV
            cbDVList.ValueMember = "Name"
            cbDVList.DisplayMember = "Name"

            If DV.Table.Rows.Count > 0 Then
                Dim FrmDV As DataView = GetDV(
                gvList.DataSource = iFrm.cbDVList.SelectedValue
                gvList.Refresh()
            End If

        Catch ex As Exception
            ShowError("Error Loading DVSpy Dropdown", ex)
        End Try

    End Sub

    'Find DVs in the control and add them as rows to aDV
    Sub LoadGVs(ByVal aGV As DataGridView, ByVal aC As Control)  'Recursive
        Dim R As DataRowView
        Dim DV As DataView

        For Each C As Control In aC.Controls
            If TypeOf (C) Is DataGridView Then
                DV = CType(C, DataView)

                R = aDV.AddNew
                R.Item("Name") = C.Name
                aDV.Table.Rows.Add(R)
            End If
            LoadDVs(aDV, C)
        Next

    End Sub

    'Show contents of selected DV
    Private Sub fDVSpy_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.TextChanged
        gvList.DataSource = cbDVList.SelectedValue
        gvList.Refresh()
    End Sub

End Class