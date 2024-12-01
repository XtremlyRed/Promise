using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Promise;

namespace Promise.Tests
{
    [TestClass()]
    public class TypeConverterExtensionsTests
    {
        [TestMethod()]
        public void TrueConvert()
        {
            Assert.IsTrue("True".ConvertTo<bool>());
        }

        [TestMethod()]
        public void FalseConvert()
        {
            Assert.IsTrue("False".ConvertTo<bool>());
        }

        [TestMethod()]
        public void NullConvert()
        {
            object obj = null!;
            Assert.ThrowsException<ArgumentNullException>(() => obj.ConvertTo<object>());
        }

        [TestMethod()]
        public void CustomConvert()
        {
            object obj = "abc";
            Assert.ThrowsException<InvalidOperationException>(() => obj.ConvertTo<int>());
        }

        [TestMethod()]
        public void Int32Convert()
        {
            object obj = "12"!;
            Assert.IsTrue(12 == obj.ConvertTo<int>());
        }
    }
}
