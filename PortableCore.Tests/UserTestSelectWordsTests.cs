using NUnit.Framework;
using PortableCore.BL.Contracts;
using System.Collections.Generic;
using PortableCore.DL;
using PortableCore.BL;
using System;
using PortableCore.BL.Managers;
using PortableCore.BL.Presenters;
using PortableCore.BL.Views;
using PortableCore.BL.Models;
using PortableCore.Tests.Mocks;
using System.Linq;

namespace PortableCore.Tests
{
    [TestFixture]
    public class UserTestSelectWordsTests
    {
        [TestCase(10)]
        public void TestMust_GetDistinctWordsForTest(int countOfWordsForTest)
        {
            //arrange
            MockSQLite dbHelper = new MockSQLite();
            int currentChatId = 1;

            //act
            TestSelectWordsReader wordsReader = new TestSelectWordsReader(dbHelper);
            var favorites = wordsReader.GetRandomFavorites(countOfWordsForTest, currentChatId);

            //assert
            Assert.IsTrue(favorites.WordsList.Count == 10);

            //check distinct
            var hashSet = new HashSet<string>();
            int factCount = favorites.WordsList.Where(x => hashSet.Add(x.TextFrom)).Count();
            Assert.AreEqual(countOfWordsForTest, factCount , "Присутствуют одинаковые значения");
        }

        [TestCase(5)]
        public void TestMust_GetWordsFromSameDirections(int countOfWordsForTest)
        {
            //arrange
            MockSQLite dbHelper = new MockSQLite();
            int currentChatId = 3;

            //act
            TestSelectWordsReader wordsReader = new TestSelectWordsReader(dbHelper);
            var favorites = wordsReader.GetRandomFavorites(countOfWordsForTest, currentChatId);

            //assert
            //Из-за рандомайзера неизвестно сколько будет вариантов, но не больше чем countOfWordsForTest
            Assert.IsTrue(favorites.WordsList.Count > 0 && favorites.WordsList.Count <= countOfWordsForTest, "Вариантов должно быть 1 или 2");
        }

        [Test]
        public void TestMust_InitializeWordsForTestInPresenter()
        {
            //arrange
            int countOfWords = 10;//общее количество слов для тестирования
            int maxVariants = 8;//количество вариантов в соответствии с количеством кнопок
            MockSQLite dbHelper = new MockSQLite();
            var testActivity = new MockTestSelectWordsActivity();
            int currentChatId = 1;
            MockTestSelectWordsReader wordsReader = new MockTestSelectWordsReader();
            var presenter = new TestSelectWordsPresenter(testActivity, dbHelper, wordsReader, currentChatId, countOfWords);

            //act
            presenter.Init();

            //assert
            Assert.IsTrue(testActivity.Variants.Count == maxVariants);
            Assert.AreEqual(presenter.wordsForTest.Count, countOfWords);
            Assert.AreEqual(presenter.directionIdFrom, 1);
        }

        [Test]
        public void TestMust_GetErrorForMistake()
        {
            //arrange
            int countOfWords = 10;//общее количество слов для тестирования
            MockSQLite dbHelper = new MockSQLite();
            var testActivity = new MockTestSelectWordsActivity();
            int currentChatId = 1;
            MockTestSelectWordsReader wordsReader = new MockTestSelectWordsReader();
            var presenter = new TestSelectWordsPresenter(testActivity, dbHelper, wordsReader, currentChatId, countOfWords);
            presenter.Init();

            //act
            presenter.OnSelectVariant("hello!");

            //assert
            Assert.IsTrue(!testActivity.CheckResult);
        }
    }
}