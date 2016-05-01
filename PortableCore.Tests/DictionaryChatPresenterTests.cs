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
        public void TestMethod()
        {
            //arrange
            var mockView = new MockDictionaryChatView();
            DictionaryChatPresenter presenter = new DictionaryChatPresenter(mockView, new MockSQLite());

            //act
            //presenter.SelectedRecentLanguagesEvent();

            //assert
            /*Assert.AreEqual(3, mockView.listDirections.Count);
            Assert.AreEqual(1, mockView.listDirections[0].Item1.ID);//порядок важен
            Assert.AreEqual(2, mockView.listDirections[1].Item1.ID);
            Assert.AreEqual(3, mockView.listDirections[2].Item1.ID);*/
        }

        class MockDictionaryChatView : IDictionaryChatView
        {
            public void UpdateChat(List<BubbleItem> listBubbles)
            {
                throw new NotImplementedException();
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
