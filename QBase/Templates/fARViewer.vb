Imports DataDynamics.ActiveReports

Public Class fARViewer
    Public irpt As DataDynamics.ActiveReports.ActiveReport3

    Sub New(ByVal arpt As DataDynamics.ActiveReports.ActiveReport3)
        InitializeComponent()
        irpt = arpt
    End Sub

    Private Sub farMain_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Me.WindowState = FormWindowState.Maximized
    End Sub

    Private Sub farMain_Deactivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Deactivate
        Me.WindowState = FormWindowState.Normal
    End Sub

    Private Sub fARViewer_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Viewer1.Document = irpt.Document
        Viewer1.AutoSize = True
        irpt.Run()
    End Sub
End Class