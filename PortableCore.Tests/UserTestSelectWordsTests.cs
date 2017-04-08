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
            throw new Exception("Тест вообще неверный, я тестирую мок, а надо реальный GetRandomFavorites! Хрень какая-то!");

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

            throw new Exception("Тест вообще неверный, я тестирую мок, а надо реальный GetRandomFavorites! Хрень какая-то!");

            //assert
            //Из-за рандомайзера неизвестно сколько будет вариантов, но не больше чем countOfWordsForTest
            Assert.IsTrue(favorites.WordsList.Count > 0 && favorites.WordsList.Count <= countOfWordsForTest, "Вариантов должно быть 1 или 2");
        }

        [Test]
        public void TestMust_GetIncorrectVariantsWithNoCorrectVariant()
        {
            //arrange
            int currentChatId = 3;
            int countOfIncorrectWords = 7;
            int languageFromId = 1;
            string correctWord = "test";
            ChatHistoryBuilder builder = new ConcreteChatHistoryBuilder();
            MockSQLite dbHelper = new MockSQLite();
            var list1 = builder.CreateChatHistory().SetChatId(currentChatId).SetLanguageFrom(languageFromId).SetLanguageTo(2).SetFavoriteState(true).SetCount(7).AddItems(0);
            var list2 = builder.SetChatId(currentChatId).SetLanguageFrom(languageFromId).SetLanguageTo(2).SetFavoriteState(true).SetCount(1).SetTextFrom(correctWord).SetTextTo("to").AddItems(list1[list1.Count - 1].ID + 1);
            dbHelper.ItemsChatHistory = list2;

            //act
            TestSelectWordsReader wordsReader = new TestSelectWordsReader(dbHelper);
            var wordsList = wordsReader.GetIncorrectVariants(countOfIncorrectWords, currentChatId, languageFromId, correctWord);

            //assert
            Assert.IsTrue(wordsList.Count > 0 && wordsList.Count == countOfIncorrectWords, "Вариантов должно быть " + countOfIncorrectWords.ToString());
            Assert.IsFalse(wordsList.Exists(x=>x.TextFrom == correctWord));
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