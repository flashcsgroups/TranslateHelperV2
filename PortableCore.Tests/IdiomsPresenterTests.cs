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
using PortableCore.Tests.Mocks;
using PortableCore.BL.Presenters;
using PortableCore.DAL;
using PortableCore.BL.Views;
using PortableCore.BL.Models;

namespace PortableCore.Tests
{
    [TestFixture]
    public class IdiomsPresenterTests
    {
        [TestCase(1, 2, "life")]
        public void TestMust_FoundEngString(int languageFromId, int languageToId, string searchString)
        {
            //arrange
            var db = new MockSQLite();
            var mockView = new MockIdiomsView();

            var presenter = new IdiomsPresenter(mockView, db, languageFromId, languageToId);

            //act
            presenter.RefreshIdiomsList(searchString, false);

            //assert
            Assert.AreEqual(1, mockView.ListItems.Count());
        }

        [TestCase(1, 2, "хватит")]
        public void TestMust_FoundRusString(int languageFromId, int languageToId, string searchString)
        {
            //arrange
            var db = new MockSQLite();
            var mockView = new MockIdiomsView();

            var presenter = new IdiomsPresenter(mockView, db, languageFromId, languageToId);

            //act
            presenter.RefreshIdiomsList(searchString, false);

            //assert
            Assert.AreEqual(1, mockView.ListItems.Count());
        }

        class MockIdiomsView : IIdiomsView
        {
            public IndexedCollection<IdiomItem> ListItems = null;

            public void UpdateList(IndexedCollection<IdiomItem> list, bool updatedFromServer)
            {
                ListItems = list;
            }
        }

    }
}
