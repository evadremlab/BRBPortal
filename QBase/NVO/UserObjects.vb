Imports System.Runtime.InteropServices
Public Class UserObjects
    Public ucount As Integer = 0

    Sub New()
        ucount = GetGuiResourcesUserCount()
    End Sub

    <DllImport("User32")> _
    Public Shared Function GetGuiResources(ByVal hProcess As IntPtr, ByVal uiFlags As Integer) As Integer
    End Function

    Public Shared Function GetGuiResourcesUserCount() As Integer
        Return GetGuiResources(Process.GetCurrentProcess().Handle, 1)
    End Function
End Class
