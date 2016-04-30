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

namespace PortableCore.Tests
{
    [TestFixture]
    public class DirectionsPresenterTests
    {
        [Test]
        public void TestMust_GetListRecentDirections()
        {
            //arrange
            var mockView = new MockDirectionsView();
            var presenter = new DirectionsPresenter(mockView, new MockSQLite());

            //act
            presenter.SelectedRecentLanguagesEvent();

            //assert
            Assert.AreEqual(3, mockView.listDirections.Count);
            Assert.AreEqual(1, mockView.listDirections[0].Item1.ID);//порядок важен
            Assert.AreEqual(2, mockView.listDirections[1].Item1.ID);
            Assert.AreEqual(3, mockView.listDirections[2].Item1.ID);
        }

        [Test]
        public void TestMust_GetListAllDirections()
        {
            //arrange
            var mockView = new MockDirectionsView();
            var presenter = new DirectionsPresenter(mockView, new MockSQLite());

            //act
            presenter.SelectedAllLanguagesEvent();

            //assert
            Assert.GreaterOrEqual(6, mockView.listLanguages.Count, "Количество элементов меньше ожидаемого");
            Assert.IsTrue(mockView.listLanguages.Exists(i => i.NameLocal == "Русский"), "В коллекции нет Русского языка, а должен быть");
            Assert.IsTrue(mockView.listLanguages.Exists(i => i.NameLocal == "English"), "В коллекции нет Английского языка, а должен быть");
        }

        class MockDirectionsView : IDirectionsView
        {
            public List<Language> listLanguages { get; private set; }

            public List<Tuple<Language, Language>> listDirections { get; private set; }

            public void updateListAllLanguages(List<Language> listLanguages)
            {
                this.listLanguages = listLanguages;
            }

            public void updateListRecentDirections(List<Tuple<Language, Language>> listDirections)
            {
                this.listDirections = listDirections;
            }
        }

        public class MockSQLite : ISQLiteTesting
        {
            public IEnumerable<T> Table<T>() where T : IBusinessEntity, new()
            {
                List<T> listItems = new List<T>();
                var type = typeof(T);
                if(type == typeof(Language))
                {
                    listItems = getMockedDataForLanguage() as List<T>;
                }
                if (type == typeof(LastChats))
                {
                    listItems = getMockedDataForLastChats() as List<T>;
                }

                return listItems;
            }

            private List<LastChats> getMockedDataForLastChats()
            {
                List<LastChats> listObj = new List<LastChats>();
                listObj.Add(new LastChats() { ID = 1, LangFrom = 1, LangTo = 2, LastChanges = DateTime.Parse("2016.01.01") });
                listObj.Add(new LastChats() { ID = 2, LangFrom = 2, LangTo = 1, LastChanges = DateTime.Parse("2016.01.02") });
                listObj.Add(new LastChats() { ID = 3, LangFrom = 3, LangTo = 1, LastChanges = DateTime.Parse("2016.01.03") });
                return listObj;
            }

            private List<Language> getMockedDataForLanguage()
            {
                LanguageManager langMan = new LanguageManager(this);
                List<Language> listObj  = langMan.GetDefaultData().ToList();
                return listObj;
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
