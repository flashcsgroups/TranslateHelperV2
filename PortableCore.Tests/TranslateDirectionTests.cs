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
        [TestCase("Russian", "English")]
        [TestCase("English", "Russian")]
        public void TestMust_SetNewDirection(string directionFrom, string directionTo)
        {
            //arrange
            MockSQLite mockSqlLite = new MockSQLite();
            LanguageManager languageManager = new LanguageManager(mockSqlLite);
            var defaultLanguages = languageManager.GetDefaultData();
            MockLanguageManager mockLanguageManager = new MockLanguageManager(mockSqlLite);
            TranslateDirection direction = new TranslateDirection(mockSqlLite, mockLanguageManager);

            //act
            direction.SetDirection(defaultLanguages.Single(x => x.NameEng == directionFrom), defaultLanguages.Single(x => x.NameEng == directionTo));

            //assert
            Assert.AreEqual(direction.LanguageFrom.NameEng, directionFrom);
            Assert.AreEqual(direction.LanguageTo.NameEng, directionTo);
        }
    }
}
