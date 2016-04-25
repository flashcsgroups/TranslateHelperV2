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
        /*[Test, TestCaseSource(nameof(objectsForCaseGetTrue))]
        public void TestMust_CompareCurrentLocale_With_KeyboardLocale_And_GetTrue(DetectInputLanguage.Language comparedLanguage, string strDirection)
        {
            //arrange
            MockDirectionManager dirManager = new MockDirectionManager();
            TranslateDirection direction = new TranslateDirection(new MockSQLite(), dirManager);
            direction.SetDirection(strDirection);

            //act
            bool result = direction.IsFrom(comparedLanguage);

            //assert
            Assert.IsTrue(result);
        }

        static object[] objectsForCaseGetTrue =
        {
            new object[] { DetectInputLanguage.Language.English, "en-ru" },
            new object[] { DetectInputLanguage.Language.Russian, "ru-en" }
        };

        [Test, TestCaseSource(nameof(objectsForCaseGetFalse))]
        public void TestMust_CompareCurrentLocale_With_KeyboardLocale_And_GetFalse(DetectInputLanguage.Language comparedLanguage, string strDirection)
        {
            //arrange
            MockDirectionManager dirManager = new MockDirectionManager();
            TranslateDirection direction = new TranslateDirection(new MockSQLite(), dirManager);
            direction.SetDirection(strDirection);

            //act
            bool result = direction.IsFrom(comparedLanguage);

            //assert
            Assert.IsFalse(result);
        }
        static object[] objectsForCaseGetFalse =
        {
            new object[] { DetectInputLanguage.Language.Unknown, "en-ru" },
            new object[] { DetectInputLanguage.Language.Russian, "en-ru" },
            new object[] { DetectInputLanguage.Language.English, "ru-en" }
        };

        [TestCase("ru-en")]
        [TestCase("en-ru")]
        public void TestMust_SetNewDirection(string newDirection)
        {
            //arrange
            MockDirectionManager dirManager = new MockDirectionManager();
            TranslateDirection direction = new TranslateDirection(new MockSQLite(), dirManager);

            //act
            direction.SetDirection(newDirection);
            string result = direction.GetCurrentDirectionName();

            //assert
            Assert.AreEqual(result, newDirection);
        }*/

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
