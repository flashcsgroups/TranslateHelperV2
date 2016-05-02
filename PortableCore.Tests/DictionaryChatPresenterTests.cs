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

namespace PortableCore.Tests
{
    [TestFixture]
    public class DictionaryChatPresenterTests
    {
        [Test]
        public void TestMust_AddUserTextToChat()
        {
            //arrange
            var mockView = new MockDictionaryChatView();
            DictionaryChatPresenter presenter = new DictionaryChatPresenter(mockView, new MockSQLite());

            //act
            string userText = "test";
            presenter.UserAddNewTextEvent(userText);

            //assert
            Assert.AreEqual(2, mockView.ListBubbles.Count);
            Assert.AreEqual(userText, mockView.ListBubbles[0].TextTo);
            Assert.AreEqual("тест", mockView.ListBubbles[1].TextFrom);
        }

        class MockDictionaryChatView : IDictionaryChatView
        {
            public List<BubbleItem> ListBubbles = new List<BubbleItem>();
            public void UpdateChat(List<BubbleItem> listBubbles)
            {
                this.ListBubbles = listBubbles;
            }
        }

        public class MockSQLite : ISQLiteTesting
        {
            public IEnumerable<T> Table<T>() where T : IBusinessEntity, new()
            {
                List<T> listItems = new List<T>();
                /*var type = typeof(T);
                if (type == typeof(Language))
                {
                    listItems = getMockedDataForLanguage() as List<T>;
                }
                if (type == typeof(ChatHistory))
                {
                    listItems = getMockedDataForLastChats() as List<T>;
                }*/

                return listItems;
            }
        }
    }
}
