using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PortableCore.BL;
using PortableCore.DL;
using PortableCore.BL.Contracts;
using PortableCore.BL.Managers;
using PortableCore.BL.Views;
using PortableCore.BL.Models;
using PortableCore.BL.Presenters;
using PortableCore.Tests.Mocks;

namespace PortableCore.Tests
{
    [TestFixture]
    public class DirectionsPresenterTests
    {
        [Test]
        public void TestMust_GetListRecentDirections()
        {
            //arrange
            var db = new MockSQLite();
            var languageManager = new LanguageManager(db);
            var mockView = new MockDirectionsView();
            var chatHistoryManager = new ChatHistoryManager(db);
            var anecdoteManager = new AnecdoteManager(db, languageManager);
            var presenter = new DirectionsPresenter(mockView, new ChatManager(db, languageManager, chatHistoryManager), languageManager, anecdoteManager);

            //act
            presenter.SelectedRecentLanguagesEvent();

            //assert
            Assert.AreEqual(3, mockView.listDirectionsRecent.Count);
            Assert.AreEqual(1, mockView.listDirectionsRecent[0].ChatId);//порядок важен
            Assert.AreEqual(2, mockView.listDirectionsRecent[1].ChatId);//порядок важен
        }

        [Test]
        public void TestMust_GetListAllDirections()
        {
            //arrange
            var db = new MockSQLite();
            var languageManager = new LanguageManager(db);
            var mockView = new MockDirectionsView();
            var chatHistoryManager = new ChatHistoryManager(db);
            var anecdoteManager = new AnecdoteManager(db, languageManager);
            var presenter = new DirectionsPresenter(mockView, new ChatManager(db, languageManager, chatHistoryManager), languageManager, anecdoteManager);

            //act
            presenter.SelectedAllLanguagesEvent("en");

            //assert
            Assert.GreaterOrEqual(6, mockView.listLanguages.Count, "Количество элементов меньше ожидаемого");
            Assert.IsTrue(mockView.listLanguages.Exists(i => i.NameLocal == "Русский"), "В коллекции нет Русского языка, а должен быть");
            Assert.IsTrue(mockView.listLanguages.Exists(i => i.NameLocal == "English"), "В коллекции нет Английского языка, а должен быть");
        }

        [Test]
        public void TestMust_FoundCurrentLocaleLanguageAtBottomList()
        {
            //arrange
            var db = new MockSQLite();
            var languageManager = new LanguageManager(db);
            var mockView = new MockDirectionsView();
            var chatHistoryManager = new ChatHistoryManager(db);
            var anecdoteManager = new AnecdoteManager(db, languageManager);
            var presenter = new DirectionsPresenter(mockView, new ChatManager(db, languageManager, chatHistoryManager), languageManager, anecdoteManager);
            string localeLanguage = "ru";

            //act
            presenter.SelectedAllLanguagesEvent(localeLanguage);

            //assert
            Assert.GreaterOrEqual(6, mockView.listLanguages.Count, "Количество элементов меньше ожидаемого");
            Assert.IsTrue(mockView.listLanguages[mockView.listLanguages.Count - 1].NameShort == localeLanguage, "Русский язык должен быть в самом низу списка поскольку это язык локалии");
        }

        class MockDirectionsView : IDirectionsView
        {
            public List<Language> listLanguages { get; private set; }

            public List<Tuple<Language, Language>> listDirections { get; private set; }
            public List<DirectionsRecentItem> listDirectionsRecent { get; private set; }

            public void updateListAllLanguages(List<Language> listLanguages)
            {
                this.listLanguages = listLanguages;
            }

            public void updateListDirectionsOfStoryes(List<StoryWithTranslateItem> listDirectionsOfStories)
            {
                throw new NotImplementedException();
            }

            public void updateListRecentDirections(List<Tuple<Language, Language>> listDirections)
            {
                this.listDirections = listDirections;
            }

            public void updateListRecentDirections(List<DirectionsRecentItem> listDirectionsRecent)
            {
                this.listDirectionsRecent = listDirectionsRecent;
            }
        }


        /*class MockDirectionManager : IDirectionManager
        {
            //private 
            public MockDirectionManager()
            {

            }

            Direction IDirectionManager.GetItemForId(int Id)
            {
                throw new NotImplementedException();
            }

            Direction IDirectionManager.GetItemForName(string name)
            {
                DirectionManager directionManager = new DirectionManager(new MockSQLite());
                var arr = directionManager.GetDefaultData();
                return arr.Single(p=>p.Name==name);
            }
        }*/
    }
}
