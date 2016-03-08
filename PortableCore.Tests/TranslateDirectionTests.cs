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

namespace PortableCore.Tests
{
    [TestFixture]
    public class TranslateDirectionTests
    {
        [TestCase("zz", "en-ru")]//раскладка ЛАТИНИЦА, приравниваю к английской
        [TestCase("en_EN", "en-ru")]
        [TestCase("en", "en-ru")]
        [TestCase("En", "en-ru")]
        [TestCase("EN", "en-ru")]
        [TestCase("ru_RU", "ru-en")]
        [TestCase("Ru", "ru-en")]
        public void TestMust_CompareCurrentLocale_With_KeyboardLocale_And_GetTrue(string comparedLocale, string strDirection)
        {
            //arrange
            MockDirectionManager dirManager = new MockDirectionManager();
            TranslateDirection direction = new TranslateDirection(new MockSQLite(), dirManager);
            direction.SetDirection(strDirection);

            //act
            bool result = direction.IsFrom(comparedLocale);

            //assert
            Assert.IsTrue(result);
        }

        [TestCase("zz", "ru-en")]//раскладка ЛАТИНИЦА, приравниваю к английской
        [TestCase("en_En", "ru-en")]
        [TestCase("Ru_ru", "en-ru")]
        [TestCase("en", "ru-en")]
        [TestCase("Ru", "en-ru")]
        public void TestMust_CompareCurrentLocale_With_KeyboardLocale_And_GetFalse(string comparedLocale, string strDirection)
        {
            //arrange
            MockDirectionManager dirManager = new MockDirectionManager();
            TranslateDirection direction = new TranslateDirection(new MockSQLite(), dirManager);
            direction.SetDirection(strDirection);

            //act
            bool result = direction.IsFrom(comparedLocale);

            //assert
            Assert.IsFalse(result);
        }

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
        }

        class MockSQLite : ISQLiteTesting
        {
            public IEnumerable<Direction> Table<Direction>() where Direction : IBusinessEntity, new()
            {
                List<Direction> listFav = new List<Direction>();
                listFav.Add(new Direction() { ID = 1 });
                return listFav;
            }
        }

        class MockDirectionManager : IDirectionManager
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
        }
    }
}
