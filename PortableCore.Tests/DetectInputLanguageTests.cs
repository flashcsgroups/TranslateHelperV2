using NUnit.Framework;
using PortableCore.BL.Contracts;
using System.Collections.Generic;
using PortableCore.DL;
using PortableCore.BL;
using System;
using PortableCore.BL.Managers;
using PortableCore.Tests.Mocks;

namespace PortableCore.Tests
{
    [TestFixture]
    public class DetectInputLanguageTests
    {
        [TestCase("Test")]
        [TestCase("grow")]
        [TestCase("j")]
        public void TestMust_DetectEnglish(string inputString)
        {
            //arrange
            MockSQLite mockSqlLite = new MockSQLite();
            LanguageManager languageManager = new LanguageManager(mockSqlLite);

            //act
            DetectInputLanguage detect = new DetectInputLanguage(inputString, languageManager);
            Language result = detect.Detect();

            //assert
            Assert.IsTrue(result.NameEng == "English");
        }

        [TestCase(" ")]
        [TestCase("")]
        public void TestMust_DetectUnknown(string inputString)
        {
            //arrange
            MockSQLite mockSqlLite = new MockSQLite();
            LanguageManager languageManager = new LanguageManager(mockSqlLite);

            //act
            DetectInputLanguage detect = new DetectInputLanguage(inputString, languageManager);
            Language result = detect.Detect();

            //assert
            Assert.IsTrue(result.ID == 0);
        }

        [TestCase("Тест")]
        public void TestMust_DetectRussian(string inputString)
        {
            //arrange
            MockSQLite mockSqlLite = new MockSQLite();
            LanguageManager languageManager = new LanguageManager(mockSqlLite);

            //act
            DetectInputLanguage detect = new DetectInputLanguage(inputString, languageManager);
            Language result = detect.Detect();

            //assert
            Assert.IsTrue(result.NameEng == "Russian");
        }

        /*[TestCase("Тест")]
        public void TestMust_NeedInvertIsTrue_EnglishToRussian(string inputString)
        {
            //arrange
            MockSQLite mockSqlLite = new MockSQLite();
            LanguageManager languageManager = new LanguageManager(mockSqlLite);
            var defaultLanguages = languageManager.GetDefaultData();
            MockDirectionManager mockDirectionManager = new MockDirectionManager();
            MockLanguageManager mockLanguageManager = new MockLanguageManager(mockSqlLite);
            TranslateDirection direction = new TranslateDirection(mockSqlLite, mockDirectionManager, mockLanguageManager);

            //act
            DetectInputLanguage detect = new DetectInputLanguage(inputString, languageManager);
            bool result = detect.NeedInvertDirection(direction);

            //assert
            Assert.IsTrue(result);
        }*/

        /*[TestCase("Test")]
        public void TestMust_NeedInvertIsFalse_EnglishToRussian(string inputString)
        {
            //arrange
            MockSQLite mockSqlLite = new MockSQLite();
            LanguageManager languageManager = new LanguageManager(mockSqlLite);
            var defaultLanguages = languageManager.GetDefaultData();
            MockDirectionManager mockDirectionManager = new MockDirectionManager();
            MockLanguageManager mockLanguageManager = new MockLanguageManager(mockSqlLite);
            TranslateDirection direction = new TranslateDirection(mockSqlLite, mockDirectionManager, mockLanguageManager);

            //act
            DetectInputLanguage detect = new DetectInputLanguage(inputString);
            bool result = detect.NeedInvertDirection(direction);

            //assert
            Assert.IsFalse(result);
        }*/

        /*[TestCase("Тест")]
        public void TestMust_NeedInvertIsTrue_SpainToRussian(string inputString)
        {
            //arrange
            MockSQLite mockSqlLite = new MockSQLite();
            LanguageManager languageManager = new LanguageManager(mockSqlLite);
            var defaultLanguages = languageManager.GetDefaultData();
            MockDirectionManager mockDirectionManager = new MockDirectionManager();
            MockLanguageManager mockLanguageManager = new MockLanguageManager(mockSqlLite);
            TranslateDirection direction = new TranslateDirection(mockSqlLite, mockDirectionManager, mockLanguageManager);
            direction.SetDirection(mockLanguageManager.GetItemForNameEng("Spain"), mockLanguageManager.GetItemForNameEng("Russian"));

            //act
            DetectInputLanguage detect = new DetectInputLanguage(inputString);
            bool result = detect.NeedInvertDirection(direction);

            //assert
            Assert.IsTrue(result);
        }*/

        /*[TestCase("Test")]
        public void TestMust_NeedInvertIsFalse_SpainToRussian(string inputString)
        {
            //arrange
            MockSQLite mockSqlLite = new MockSQLite();
            LanguageManager languageManager = new LanguageManager(mockSqlLite);
            var defaultLanguages = languageManager.GetDefaultData();
            MockDirectionManager mockDirectionManager = new MockDirectionManager();
            MockLanguageManager mockLanguageManager = new MockLanguageManager(mockSqlLite);
            TranslateDirection direction = new TranslateDirection(mockSqlLite, mockDirectionManager, mockLanguageManager);
            direction.SetDirection(mockLanguageManager.GetItemForNameEng("Spain"), mockLanguageManager.GetItemForNameEng("Russian"));

            //act
            DetectInputLanguage detect = new DetectInputLanguage(inputString);
            bool result = detect.NeedInvertDirection(direction);

            //assert
            Assert.IsFalse(result);
        }*/

    }
}