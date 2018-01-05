using System;
using NUnit.Framework;
using PortableCore.WS;
using PortableCore.BL.Contracts;
using System.Collections.Generic;
using PortableCore.Tests.Mocks;
using System.Threading.Tasks;

namespace PortableCore.Tests
{
    [TestFixture]
    public class ClientSyncTests
    {
        [TestCase("Idiom")]
        public void TestMust_GetMaxTimeStampForLocalTable(string tableName)
        {
            //arrange
            var db = new MockSQLite();
            IApiClient srvClient = new MockClientApi();
            MockIdiomManager mockIdiomManager = new MockIdiomManager(db);
            ClientSync syncTable = new ClientSync(db, mockIdiomManager, srvClient, tableName);

            //act
            DateTime maxTimeStamp = syncTable.GetLocalMaxTimeStamp();

            //assert
            Assert.IsTrue(maxTimeStamp.Day == 13);
            Assert.IsTrue(maxTimeStamp.Month == 10);
            Assert.IsTrue(maxTimeStamp.Year == 2017);
        }

        [TestCase("Idiom", "2017-10-14")]
        public void TestMust_GetChangesIDs(string tableName, string maxTimeStampString)
        {
            //arrange
            DateTime localMaxTimeStampString = DateTime.Parse(maxTimeStampString);
            var db = new MockSQLite();
            IApiClient srvClient = new MockClientApi();
            MockIdiomManager mockIdiomManager = new MockIdiomManager(db);
            ClientSync syncTable = new ClientSync(db, mockIdiomManager, srvClient, tableName);

            //act
            Task<List<int>> iDs = syncTable.GetChangedIDsFromServer(localMaxTimeStampString);

            //assert
            Assert.IsTrue(iDs.Result.Count == 3);
        }

        [TestCase("Idiom", "2017-10-14")]
        public void TestMust_SuccessSyncTable(string tableName, string maxTimeStampString)
        {
            //arrange
            DateTime localMaxTimeStampString = DateTime.Parse(maxTimeStampString);
            var db = new MockSQLite();
            IApiClient srvClient = new MockClientApi();
            MockIdiomManager mockIdiomManager = new MockIdiomManager(db);
            ClientSync syncTable = new ClientSync(db, mockIdiomManager, srvClient, tableName);
            Task<List<int>> iDs = syncTable.GetChangedIDsFromServer(localMaxTimeStampString);

            //act
            Task<int> processed = syncTable.Sync(iDs.Result);

            //assert
            Assert.IsTrue(processed.Result == 3);
        }
    }
}