using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BRBPortal_CSharp;

namespace CSharp_Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CSharp_Left()
        {
            var str = "testing";
            str = BRBFunctions_CSharp.Left(str, 3);
            Assert.AreEqual("tes", str);
        }

        [TestMethod]
        public void CSharp_Right()
        {
            var str = "testing";
            str = BRBFunctions_CSharp.Right(str, 4);
            Assert.AreEqual("ting", str);
        }

        [TestMethod]
        public void CSharp_Mid()
        {
            var str = "testing";
            str = BRBFunctions_CSharp.Mid(str, 5, 3);
            Assert.AreEqual("ing", str);
        }
    }
}
