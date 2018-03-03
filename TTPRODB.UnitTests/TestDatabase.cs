using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TTPRODB.DatabaseCommunication;

namespace TTPRODB.UnitTests
{
    [TestClass]
    public class TestDatabase
    {
        [TestMethod]
        public void TestDbNotExists()
        {
            Assert.AreEqual(false, DbConnect.CheckDbData());
        }
    }
}
