using System;
using System.Collections.Generic;
using System.Linq;
using PortableCore.DL;

namespace PortableCore.BL
{

    public class TestSelectWordsPresenter
    {
        int maxCountOfWords;
        int countOfWords;
        int positionWordInList;
        int countOfVariantsWithoutCorrect = 4;//значение по-умолчанию для количества возможных вариантов без учета правильного варианта
        ITestSelectWordsView view;
        ISQLiteTesting db;
        ITestSelectWordsReader wordsReader;
        TranslateDirection direction;
        List<Favorites> favoritesList;
        string rightWord;

        public TestSelectWordsPresenter(ITestSelectWordsView view, ISQLiteTesting db, ITestSelectWordsReader wordsReader, int maxCountOfWords)
        {
            this.view = view;
            this.db = db;
            this.wordsReader = wordsReader;
            this.maxCountOfWords = maxCountOfWords;
            this.direction = new TranslateDirection(db);
            direction.SetDefaultDirection();
        }

        public void OnSelectVariant(string selectedWord)
        {
            int diff = string.Compare(selectedWord, rightWord, StringComparison.CurrentCultureIgnoreCase);

            view.SetCheckResult(diff == 0);                
        }

        public void OnSubmit()
        {
            if(positionWordInList < countOfWords)
            {
                Tuple<string, string> nextPair = getNextPair();
                rightWord = nextPair.Item2;
                view.SetOriginalWord(nextPair.Item1);
                var variantsArray = getIncorrectWord(countOfVariantsWithoutCorrect);
                addToVariantsCorrectWord(variantsArray, rightWord);
                view.SetVariants(variantsArray);
                positionWordInList++;
            } else
            {
                positionWordInList = 0;
                view.SetFinalTest(countOfWords);
            }
        }

        private void addToVariantsCorrectWord(List<string> variantsArray, string rightWord)
        {
            int count = variantsArray.Count;
            Random rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            int indexOfRecord = rnd.Next(0, count - 1);
            variantsArray.Insert(indexOfRecord, rightWord);
        }

        private List<string> getIncorrectWord(int countOfIncorrectWords)
        {
            return wordsReader.GetIncorrectVariants(rightWord, countOfIncorrectWords, direction);
        }

        public void StartTest()
        {
            favoritesList = wordsReader.GetRandomFavorites(maxCountOfWords, direction);
            countOfWords = favoritesList.Count();
            OnSubmit();
        }

        private Tuple<string, string> getNextPair()
        {
            var item = favoritesList[positionWordInList];
            Tuple<string, string> nextPair = wordsReader.GetNextWord(item.TranslatedExpressionID);
            return nextPair;
        }
    }
}