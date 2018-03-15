Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()> Public Class VB_Tests

    <TestMethod()> Public Sub VB_Left()
        Dim str As String
        str = "testing"
        str = Right(str, 3)
        Assert.AreEqual("ing", str)
    End Sub

    <TestMethod()> Public Sub VB_Right()
        Dim str As String
        str = "testing"
        str = Left(str, 3)
        Assert.AreEqual("tes", str)
    End Sub

    <TestMethod()> Public Sub VB_Mid1()
        Dim str As String
        str = "testing"
        str = Mid(str, 4)
        Assert.AreEqual("ting", str)
    End Sub

    <TestMethod()> Public Sub VB_Mid2()
        Dim str As String
        str = "testing"
        str = Mid(str, 5, 3) ' start, length
        Assert.AreEqual("ing", str)
    End Sub

End Class