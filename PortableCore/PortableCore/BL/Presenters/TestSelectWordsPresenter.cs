using System;
using System.Collections.Generic;
using System.Linq;
using PortableCore.DL;
using PortableCore.BL.Views;
using PortableCore.BL.Models;

namespace PortableCore.BL.Presenters
{
    public class TestSelectWordsPresenter
    {
        int maxCountOfWords;
        int countOfWords;
        int positionWordInList;
        int countOfVariantsWithoutCorrect = 7;//значение по-умолчанию для количества возможных вариантов без учета правильного варианта
        ITestSelectWordsView view;
        ISQLiteTesting db;
        ITestSelectWordsReader wordsReader;
        int currentChatId;
        //List<FavoriteItem> favoritesList;
        string rightWord;

        public TestSelectWordsPresenter(ITestSelectWordsView view, ISQLiteTesting db, int currentChatId, int countOfWords)
        {
            this.view = view;
            this.db = db;
            //this.wordsReader = wordsReader;
            this.countOfWords = countOfWords;
            this.currentChatId = currentChatId;
        }

        public void OnSelectVariant(string selectedWord)
        {
            int diff = string.Compare(selectedWord, rightWord, StringComparison.CurrentCultureIgnoreCase);
            bool checkResult = diff == 0;
            if (!checkResult)
            {
                view.SetCheckError();
            }
            else
            {
                OnSubmit();
            }
        }

        private void OnSubmit()
        {
            throw new NotImplementedException();
            /*if (positionWordInList < countOfWords)
            {
                Tuple<string, string> nextPair = getNextPair();
                rightWord = nextPair.Item2;
                view.SetOriginalWord(nextPair.Item1);
                var variantsArray = getIncorrectWord(favoritesList[positionWordInList].SourceExprId, countOfVariantsWithoutCorrect);
                addToVariantsCorrectWord(variantsArray, rightWord);
                view.SetVariants(variantsArray);
                positionWordInList++;
            } else
            {
                positionWordInList = 0;
                view.SetFinalTest(countOfWords);
            }*/
        }

        public void Init()
        {
            //List<string> variantsArray = new List<string>{ "one", "two", "three", "four", "sixth" ,"sevens", "eith" , "nine"};
            //view.SetVariants(variantsArray);
            var portionOfWordsForTestList = wordsReader.GetRandomFavorites(maxCountOfWords, currentChatId);
            view.SetVariants(portionOfWordsForTestList);
            //countOfWords = favoritesList.Count();
            //OnSubmit();
        }

        private void addToVariantsCorrectWord(List<string> variantsArray, string rightWord)
        {
            int count = variantsArray.Count;
            if (count > 0)
            {
                Random rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
                int indexOfRecord = rnd.Next(0, count - 1);
                variantsArray.Insert(indexOfRecord, rightWord);
            }
            //else throw new Exception("Error adding correct word");
        }

        private List<string> getIncorrectWord(int rightWordSourceExpr, int countOfIncorrectWords)
        {
            throw new NotImplementedException();
            //return wordsReader.GetIncorrectVariants(rightWordSourceExpr, countOfIncorrectWords, currentChatId);
        }

        public void StartTest()
        {
            //favoritesList = wordsReader.GetRandomFavorites(maxCountOfWords, currentChatId);
            //countOfWords = favoritesList.Count();
            //OnSubmit();
        }

        private Tuple<string, string> getNextPair()
        {
            throw new NotImplementedException();
            /*var item = favoritesList[positionWordInList];
            Tuple<string, string> nextPair = wordsReader.GetNextWord(item.TranslatedExpressionId);
            return nextPair;*/
        }
    }
}