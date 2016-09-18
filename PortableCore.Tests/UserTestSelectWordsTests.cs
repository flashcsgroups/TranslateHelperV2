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

namespace PortableCore.Tests
{
    [TestFixture]
    public class UserTestSelectWordsTests
    {
        /*[Test]
        public void TestMust_GetFavoritesDataForTest()
        {
            //arrange
            int countOfWords = 10;
            MockSQLite dbHelper = new MockSQLite();
            TranslateDirection direction = new TranslateDirection(dbHelper);
            //direction.SetDefaultDirection();

            //act
            TestSelectWordsReader data = new TestSelectWordsReader(dbHelper);
            List<FavoriteItem> favoritesList = data.GetRandomFavorites(countOfWords, direction);

            //assert
            Assert.IsTrue(favoritesList.Count == 10);
        }*/

        class MockSQLite : ISQLiteTesting
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
        }

        [Test]
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
            MockTestSelectWordsReader wordsReader = new MockTestSelectWordsReader();
            DirectionManager directionManager = new DirectionManager(dbHelper);
            LanguageManager languageManager = new LanguageManager(dbHelper);
            TranslateDirection direction = new TranslateDirection(dbHelper, directionManager, languageManager);
            presenter = new TestSelectWordsPresenter(testActivity, dbHelper, wordsReader, direction, countOfWords);
        }

        class MockTestSelectWordsActivity : ITestSelectWordsView
        {
            public string OriginalWord = string.Empty;
            public List<string> Variants = new List<string>();
            public bool CheckResult = false;
            public int CountOfTestedWords = 0;

            public void SetCheckError()
            {
                CheckResult = false;
            }

            public void SetOriginalWord(string originalWord)
            {
                OriginalWord = originalWord;
            }

            public void SetVariants(List<string> variants)
            {
                this.Variants = variants;
            }

            public void SetFinalTest(int countOfTestedWords)
            {
                CountOfTestedWords = countOfTestedWords;
            }
        }

        class MockTestSelectWordsReader : ITestSelectWordsReader
        {
            public int GetCountDifferenceSources(TranslateDirection direction)
            {
                return 10;
            }

            public List<string> GetIncorrectVariants(int excludeCorrectSourceId, int countOfIncorrectWords, TranslateDirection direction)
            {
                List<string> result = new List<string>();
                for(int i=0;i<countOfIncorrectWords;i++)
                {
                    result.Add(i.ToString());
                }
                return result;
            }

            public Tuple<string, string> GetNextWord(int translatedExpressionID)
            {
                return new Tuple<string, string>("test","тест");
            }

            public List<FavoriteItem> GetRandomFavorites(int countOfWords, TranslateDirection direction)
            {
                List<FavoriteItem> result = new List<FavoriteItem>();
                for (int i = 0; i < countOfWords; i++)
                {
                    result.Add(new FavoriteItem() { FavoriteId = i });
                }
                return result;
            }
        }
    }
}