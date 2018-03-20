using Microsoft.VisualStudio.TestTools.UnitTesting;
using TTPRODB.DatabaseCommunication;

namespace TTPRODB.UnitTests
{
    [TestClass]
    public class DBUnitTests
    {
        [TestMethod]
        public void ValidateEmptyDbTest()
        {
            DbConnect.ValidateDatabase();
        }
    }
}
