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

namespace PortableCore.Tests
{
    [TestFixture]
    public class TranslateDirectionTests
    {
        /*[TestCase("zz", "en-ru")]//раскладка ЛАТИНИЦА, приравниваю к английской
        [TestCase("en_EN", "en-ru")]
        [TestCase("en", "en-ru")]
        [TestCase("En", "en-ru")]
        [TestCase("EN", "en-ru")]
        [TestCase("ru_RU", "ru-en")]
        [TestCase("Ru", "ru-en")]*/
        /*[Test, TestCaseSource(nameof(objectsForCaseGetTrue))]
        public void TestMust_CompareCurrentLocale_With_KeyboardLocale_And_GetTrue(DetectInputLanguage.Language comparedLanguage, string strDirection)
        {
            //arrange
            MockDirectionManager dirManager = new MockDirectionManager();
            TranslateDirection direction = new TranslateDirection(new MockSQLite(), dirManager);
            //direction.SetDirection(strDirection);

            //act
            bool result = direction.IsFrom(comparedLanguage);

            //assert
            Assert.IsTrue(result);
        }

        static object[] objectsForCaseGetTrue =
        {
            new object[] { DetectInputLanguage.Language.English, "en-ru" },
            new object[] { DetectInputLanguage.Language.Russian, "ru-en" }
        };*/

        /*[Test, TestCaseSource(nameof(objectsForCaseGetFalse))]
        public void TestMust_CompareCurrentLocale_With_KeyboardLocale_And_GetFalse(DetectInputLanguage.Language comparedLanguage, string strDirection)
        {
            //arrange
            MockDirectionManager dirManager = new MockDirectionManager();
            TranslateDirection direction = new TranslateDirection(new MockSQLite(), dirManager);
            //direction.SetDirection(strDirection);

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
        };*/
        /*[TestCase("zz", "ru-en")]//раскладка ЛАТИНИЦА, приравниваю к английской
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
        }*/

        [TestCase("Russian", "English")]
        [TestCase("English", "Russian")]
        public void TestMust_SetNewDirection(string directionFrom, string directionTo)
        {
            //arrange
            MockSQLite mockSqlLite = new MockSQLite();
            LanguageManager languageManager = new LanguageManager(mockSqlLite);
            var defaultLanguages = languageManager.GetDefaultData();
            MockDirectionManager mockDirectionManager = new MockDirectionManager();
            MockLanguageManager mockLanguageManager = new MockLanguageManager(mockSqlLite);
            TranslateDirection direction = new TranslateDirection(mockSqlLite, mockDirectionManager, mockLanguageManager);

            //act
            direction.SetDirection(defaultLanguages.Single(x => x.NameEng == directionFrom), defaultLanguages.Single(x => x.NameEng == directionTo));

            //assert
            Assert.AreEqual(direction.LanguageFrom.NameEng, directionFrom);
            Assert.AreEqual(direction.LanguageTo.NameEng, directionTo);
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
    }
}
