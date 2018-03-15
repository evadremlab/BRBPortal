Namespace Windows.Forms

    'Parameters form parent.  Used to start reports or updates...
    Public Class fpMain
        Inherits fBase


#Region "---------------------------- Documentation ------------------------------"
        'Parameter Ancestor WinForm


#End Region

        'Set Color Scheme
        Private Sub fpMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                Dim toolTip1 As New ToolTip()
                SetToolTipProperties(toolTip1)
                toolTip1.SetToolTip(Me.btnRun, "Run the report, based on what you've entered")

                For Each C As Control In Me.Controls
                    AssignToolTips(toolTip1, C)    'Call recursive routine
                Next

                SetHandles()

                SetColorScheme()    'Default fore and backcolors
                StatusStrip1.BackColor = QBackColor
            Catch ex As Exception
                ShowError("Error setting up form", ex)
                Close() 'Close form
            End Try
        End Sub

        Public Sub SetParams(ByRef aParams As ArrayList)
            iParams = aParams
        End Sub

        Public Sub SetParams(ByVal aName As String, ByVal aValue As String)
            Dim P As strParam = New strParam

            P.Name = aName
            P.Value = aValue
            iParams.Add(P)

        End Sub

        'Show status message
        Overrides Function ShowStatus(ByVal aMessage As String) As Boolean
            StatusMsg.Text = aMessage
            Refresh()

            Return True
        End Function

        'Show Progress message
        Overrides Function ShowProgress(ByVal aMessage As String) As Boolean
            ProgressMsg.Text = aMessage
            Refresh()
            Return True
        End Function

        'Show Progress and control cursor image
        Overrides Function ShowProgress(ByVal aMessage As String, ByVal aShowHourGlass As Boolean) As Boolean
            ProgressMsg.Text = aMessage
            Refresh()
            Me.Cursor = Cursors.Arrow
            If aShowHourGlass Then Me.Cursor = Cursors.WaitCursor
        End Function

        'Show Help message
        Overrides Function ShowHelp(ByVal aMessage As String) As Boolean
            HelpMsg.Text = aMessage
            Refresh()

            Return True
        End Function

    End Class

End Namespace