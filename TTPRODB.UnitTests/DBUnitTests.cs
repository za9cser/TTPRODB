using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TTPRODB.DatabaseCommunication;
using System.Data.SqlClient;

namespace TTPRODB.UnitTests
{
    [TestClass]
    public class DBUnitTests
    {
        [TestMethod]
        public void ValidateEmptyDbTest()
        {
            DbConnect.ValidateDatabase();
            SqlDataReader res = DbConnect.ExecuteQuery("SELECT COUNT(*) FROM Blades")[0];            
            res.Read();
            Assert.AreEqual(0, res.GetInt32(0));
            res.Close();
        }
    }
}
