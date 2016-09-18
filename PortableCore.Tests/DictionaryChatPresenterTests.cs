using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortableCore.BL.Presenters;
using PortableCore.DL;
using PortableCore.BL.Contracts;
using PortableCore.BL.Views;
using PortableCore.BL.Models;
using PortableCore.BL.Managers;
using PortableCore.Tests.Mocks;

namespace PortableCore.Tests
{
    [TestFixture]
    public class DictionaryChatPresenterTests
    {
        [Test]
        public void TestMust_AddUserTextToChat()
        {
            //arrange
            var db = new MockSQLite();
            var mockView = new MockDictionaryChatView();
            var mocklanguageManager = new MockLanguageManager(db);
            var chatHistoryManager = new MockChatHistoryManager(db);
            var mockChatManager = new MockChatManager(db);
            DictionaryChatPresenter presenter = new DictionaryChatPresenter(mockView, db, 1, mockChatManager, mocklanguageManager, chatHistoryManager);
            presenter.InitDirection();

            //act
            string userText = "test";
            string answerText = "тест";
            presenter.UserAddNewTextEvent(userText);

            //assert
            Assert.AreEqual(2, mockView.ListBubbles.Count);
            //Направление с английского на русский
            Assert.AreEqual(userText, mockView.ListBubbles[0].TextTo);
            Assert.AreEqual(answerText, mockView.ListBubbles[0].TextFrom);
            //Направление с русского на английский
            Assert.AreEqual(answerText, mockView.ListBubbles[1].TextTo);
            Assert.AreEqual(userText, mockView.ListBubbles[1].TextFrom);
        }

        class MockDictionaryChatView : IDictionaryChatView
        {
            public List<BubbleItem> ListBubbles = new List<BubbleItem>();
            public void UpdateChat(List<BubbleItem> listBubbles)
            {
                this.ListBubbles = listBubbles;
            }
        }

        private class MockChatManager : IChatManager
        {
            //ISQLiteTesting db;

            public MockChatManager(ISQLiteTesting dbHelper)
            {
            }

            public Chat GetChatByLanguage(Language userLanguage, Language robotLanguage)
            {
                throw new NotImplementedException();
            }

            public List<DirectionsRecentItem> GetChatsForLastDays(int countOfDays)
            {
                throw new NotImplementedException();
            }

            public Chat GetItemForId(int id)
            {
                return new Chat() { ID = 1, LanguageFrom = 1, LanguageTo = 2};
            }

            public int SaveItem(Chat item)
            {
                return 1;
            }
        }

        public class MockChatHistoryManager : IChatHistoryManager
        {
            ISQLiteTesting db;

            public MockChatHistoryManager(ISQLiteTesting dbHelper)
            {
                db = dbHelper;
            }

            public void DeleteItemById(int historyRowId)
            {
                throw new NotImplementedException();
            }

            public int GetCountOfMessagesForChat(int chatId)
            {
                throw new NotImplementedException();
            }

            public ChatHistory GetItemForId(int id)
            {
                throw new NotImplementedException();
            }

            public ChatHistory GetLastRobotMessage()
            {
                return new ChatHistory() { ID = 1, ChatID = 1};
            }

            public List<ChatHistory> ReadChatMessages(Chat chatItem)
            {
                return new List<ChatHistory>() {
                    new ChatHistory() { ID=1, ChatID = 1, TextFrom="тест", TextTo="test", Transcription = "", InFavorites = false},
                    new ChatHistory() { ID=2, ChatID = 2, TextFrom="test", TextTo="тест", Transcription = "", InFavorites = false}
                };
            }

            public int SaveItem(ChatHistory item)
            {
                return 1;
            }
        }

    }
}
