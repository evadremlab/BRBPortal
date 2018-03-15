Namespace Windows.Forms

    'Parameters form parent.  Used to start reports or updates...
    Public Class fpMain
        Inherits fBase
        Protected Event OnClearControl()

        'Is Resizable - default is false, but may be set to true for Master/Detail edit forms.
        Private _iIsResizable As Boolean
        Public Property iIsResizable() As Boolean
            Get
                Return _iIsResizable
            End Get
            Set(ByVal value As Boolean)
                _iIsResizable = value
                If _iIsResizable = True Then
                    FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
                    StatusStrip1.SizingGrip = True
                Else
                    FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
                    StatusStrip1.SizingGrip = False
                End If
            End Set
        End Property

#Region "---------------------------- Documentation ------------------------------"
        'Parameter Ancestor WinForm


#End Region

        Private Sub fpMain_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
            If Me.WindowState = FormWindowState.Maximized Then
                Me.WindowState = FormWindowState.Normal
            End If
        End Sub

        '''<summary> Set Color Scheme </summary>
        Private Sub fpMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                iIsResizable = False

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

        '''<summary> Load local array list into iParams </summary>
        Public Sub SetParams(ByRef aParams As ArrayList)
            iParams = aParams
        End Sub

        '''<summary> Load a Name Value pair into iParams </summary>
        Public Sub SetParams(ByVal aName As String, ByVal aValue As String)
            Dim P As strParam = New strParam

            P.Name = aName
            P.Value = aValue
            iParams.Add(P)

        End Sub

        '''<summary> Show status message </summary>
        Overrides Function ShowStatus(ByVal aMessage As String) As Boolean
            'BHS 8/17/09 Only change message if there is a change - this avoids unneeded screen paints
            If StringCompare(StatusMsg.Text, aMessage) = False Then
                StatusMsg.Text = aMessage
                Refresh()
            End If
            Return True
        End Function

        '''<summary> Show Progress message </summary>
        Overrides Function ShowProgress(ByVal aMessage As String) As Boolean
            'BHS 8/17/09 Only change message if there is a change - this avoids unneeded screen paints
            If StringCompare(ProgressMsg.Text, aMessage) = False Then
                ProgressMsg.Text = aMessage
                Refresh()
            End If

            Return True
        End Function

        '''<summary> Show Progress and control cursor image </summary>
        Overrides Function ShowProgress(ByVal aMessage As String, ByVal aShowHourGlass As Boolean) As Boolean
            'BHS 8/17/09 Only change message if there is a change - this avoids unneeded screen paints
            If StringCompare(ProgressMsg.Text, aMessage) = False Then
                ProgressMsg.Text = aMessage
                Refresh()
            End If

            Me.Cursor = Cursors.Arrow
            If aShowHourGlass Then Me.Cursor = Cursors.WaitCursor
        End Function

        '''<summary> Show Help message </summary>
        Overrides Function ShowHelp(ByVal aMessage As String) As Boolean
            'BHS 8/17/09 Only change message if there is a change - this avoids unneeded screen paints
            If StringCompare(HelpMsg.Text, aMessage) = False Then
                HelpMsg.Text = aMessage
                Refresh()
            End If

            Return True
        End Function

        Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click
            'Call fbase function to clear all fields 
            ClearQueryControls(Me, False)
            'Raise event to allow decendent to clear list and set defaults
            Try
                RaiseEvent OnClearControl()
            Catch ex As Exception
                ShowError("Unexpected error clearing controls (OnClearControl)", ex)
            End Try

            Post("EndOfClear")
        End Sub

        '''<summary> Help button clicked </summary>
        Private Sub btnSearchHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearchHelp.Click
            Try
                Dim frm As New fhMain("Search")
                frm.Show()
            Catch ex As Exception
                ShowError("Error showing help", ex)
                Return  'Not fatal
            End Try
        End Sub
    End Class

End Namespace