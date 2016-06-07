using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortableCore.BL.Managers;
using PortableCore.Tests.Mocks;

namespace PortableCore.Tests
{
    [TestFixture]
    public class ChatManagerTests
    {
        [Test]
        public void TestMust_Get2ChatsForLastDays()
        {
            var db = new MockSQLite();
            var languageManager = new LanguageManager(db);
            var chatHistoryManager = new ChatHistoryManager(db);
            var chatManager = new ChatManager(db, languageManager, chatHistoryManager);
            var list = chatManager.GetChatsForLastDays(5);
            Assert.IsTrue(list.Count == 2);
        }
    }
}
