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
            public List<Language> listLanguages;

            public void updateListAllLanguages(List<Language> listLanguages)
            {
                this.listLanguages = listLanguages;
            }

            public void updateListRecentDirections(List<Tuple<Language, Language>> listDirections)
            {
                throw new NotImplementedException();
            }
        }

        class MockSQLite : ISQLiteTesting
        {
            public IEnumerable<Direction> Table<Direction>() where Direction : IBusinessEntity, new()
            {
                List<Direction> listFav = new List<Direction>();
                //listFav.Add(new Direction() { ID = 1 });
                return listFav;
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
