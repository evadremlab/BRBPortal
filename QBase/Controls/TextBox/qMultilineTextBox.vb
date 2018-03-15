Imports System.Windows.Forms

Namespace Windows.Forms

    ''' <summary>
    ''' Represents a custom Windows multi-line text box control.
    ''' </summary>
    ''' <remarks></remarks>
    <System.ComponentModel.DefaultBindingProperty("Text")>
    Public Class qMultilineTextBox
        Inherits QSILib.Windows.Forms.qTextBox

        Public Sub New()
            ' Set Muliline Properties
            Me.Multiline = True
            Me.Height = 100
            Me.ScrollBars = System.Windows.Forms.ScrollBars.Both
        End Sub

    End Class

End Namespace
