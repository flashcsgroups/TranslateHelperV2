﻿using NUnit.Framework;
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
            List<string> favoritesList = wordsReader.GetRandomFavorites(countOfWordsForTest, currentChatId);

            //assert
            Assert.IsTrue(favoritesList.Count == 10);

            //check distinct
            var hashSet = new HashSet<string>();
            Assert.AreEqual(countOfWordsForTest, favoritesList.Where(x => !hashSet.Add(x)).Count(), "Присутствуют одинаковые значения");
        }

        public void TestMust_GetDistinctWordsForTestWithoutComma(int countOfWordsForTest)
        {

        }
        public void TestMust_GetWordsForTestWithoutRightWord(int countOfWordsForTest)
        {

        }
        /*class MockSQLite : ISQLiteTesting
        {
            public IEnumerable<Favorites> Table<Favorites>() where Favorites : IBusinessEntity, new()
            {
                List<Favorites> listFav = new List<Favorites>();
                listFav.Add(new Favorites() { ID = 1});
                listFav.Add(new Favorites() { ID = 2 });
                listFav.Add(new Favorites() { ID = 3 });
                listFav.Add(new Favorites() { ID = 4 });
                listFav.Add(new Favorites() { ID = 5 });
                listFav.Add(new Favorites() { ID = 6 });
                listFav.Add(new Favorites() { ID = 7 });
                listFav.Add(new Favorites() { ID = 8 });
                listFav.Add(new Favorites() { ID = 9 });
                listFav.Add(new Favorites() { ID = 10 });
                return listFav;
            }
        }*/

        [Test]
        [Ignore("старая структура")]
        public void TestMust_StartTestAndGetOriginalWord()
        {
            //arrange
            int countOfVariants;
            MockTestSelectWordsActivity testActivity;
            TestSelectWordsPresenter presenter;
            InitTestData(out countOfVariants, out testActivity, out presenter);

            //act
            presenter.StartTest();

            //assert
            Assert.IsTrue(testActivity.OriginalWord == "test");
            Assert.IsTrue(testActivity.Variants[0] == "0");
        }

        [Test]
        [Ignore("старая структура")]
        public void TestMust_CheckCountVariants()
        {
            //arrange
            int countOfVariants;
            MockTestSelectWordsActivity testActivity;
            TestSelectWordsPresenter presenter;
            InitTestData(out countOfVariants, out testActivity, out presenter);

            //act
            presenter.StartTest();

            //assert
            Assert.IsTrue(testActivity.Variants.Count == countOfVariants);
        }

        /*[Test]
        public void TestMust_SuccessfulCheckCorrectVariant()
        {
            //arrange
            int countOfVariants;
            MockTestSelectWordsActivity testActivity;
            TestSelectWordsPresenter presenter;
            InitTestData(out countOfVariants, out testActivity, out presenter);

            //act
            presenter.StartTest();
            presenter.OnSelectVariant("тест");

            //assert
            Assert.IsTrue(testActivity.CheckResult);
        }*/

        [Test]
        [Ignore("старая структура")]
        public void TestMust_GetErrorForMistakeVariant()
        {
            //arrange
            int countOfVariants;
            MockTestSelectWordsActivity testActivity;
            TestSelectWordsPresenter presenter;
            InitTestData(out countOfVariants, out testActivity, out presenter);

            //act
            presenter.StartTest();
            presenter.OnSelectVariant("вапв");

            //assert
            Assert.IsFalse(testActivity.CheckResult);
        }

        [Test]
        [Ignore("старая структура")]
        public void TestMust_GetFinalMessageText()
        {
            //arrange
            int countOfVariants;
            MockTestSelectWordsActivity testActivity;
            TestSelectWordsPresenter presenter;
            InitTestData(out countOfVariants, out testActivity, out presenter);

            //act
            presenter.StartTest();
            presenter.OnSelectVariant("тест");
            //presenter.OnSubmit();
            presenter.OnSelectVariant("тест");
            //presenter.OnSubmit();
            presenter.OnSelectVariant("тест");
            //presenter.OnSubmit();
            presenter.OnSelectVariant("тест");
            //presenter.OnSubmit();
            presenter.OnSelectVariant("тест");
            //presenter.OnSubmit();
            presenter.OnSelectVariant("тест");
            //presenter.OnSubmit();
            presenter.OnSelectVariant("тест");
            //presenter.OnSubmit();
            presenter.OnSelectVariant("тест");
            //presenter.OnSubmit();
            presenter.OnSelectVariant("тест");
            //presenter.OnSubmit();
            presenter.OnSelectVariant("тест");
            //presenter.OnSubmit();

            //assert
            Assert.IsTrue(testActivity.CountOfTestedWords == 10);
        }

        private void InitTestData(out int countOfVariants, out MockTestSelectWordsActivity testActivity, out TestSelectWordsPresenter presenter)
        {
            int countOfWords = 10;
            countOfVariants = 8;
            MockSQLite dbHelper = new MockSQLite();
            testActivity = new MockTestSelectWordsActivity();
            int currentChatId = 1;
            //MockTestSelectWordsReader wordsReader = new MockTestSelectWordsReader();
            //LanguageManager languageManager = new LanguageManager(dbHelper);
            //TranslateDirection direction = new TranslateDirection(dbHelper, languageManager);
            presenter = new TestSelectWordsPresenter(testActivity, dbHelper, currentChatId, countOfWords);
            //presenter = new TestSelectWordsPresenter(testActivity, dbHelper, wordsReader, direction, countOfWords);
        }
    }
}