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
        readonly int countOfWords;
        int positionWordInList;
        readonly int countOfVariants = 8;//ћаксимальное количество вариантов
        readonly ITestSelectWordsView view;
        readonly ISQLiteTesting db;
        readonly ITestSelectWordsReader wordsReader;
        readonly int currentChatId;
        TestWordItem currentWord = new TestWordItem();
        List<Tuple<string, bool>> results = new List<Tuple<string, bool>>();
        internal List<TestWordItem> wordsForTest = new List<TestWordItem>();// оллекци€ слов-заданий дл€ тестировани€
        internal int directionIdFrom;//направление перевода дл€ текущего набора тестовых слов-заданий

        public TestSelectWordsPresenter(ITestSelectWordsView view, ISQLiteTesting db, ITestSelectWordsReader wordsReader, int currentChatId, int countOfWords)
        {
            this.view = view;
            this.db = db;
            this.wordsReader = wordsReader;
            this.countOfWords = countOfWords;
            this.currentChatId = currentChatId;
        }

        public void OnSelectVariant(string selectedWord)
        {
            int diff = string.Compare(selectedWord, currentWord.TextTo, StringComparison.CurrentCultureIgnoreCase);
            bool resultIsRight = diff == 0;

            results.Add(new Tuple<string, bool>(currentWord.TextTo, resultIsRight));

            if (!resultIsRight)
            {
                view.SetButtonErrorState();
            }
            else
            {
                OnSubmit();
            }
        }

        private void OnSubmit()
        {
            if (positionWordInList < countOfWords)
            {
                newVariant();
                positionWordInList++;
            } else
            {
                finalizeTest();
            }
        }

        public void Init()
        {
            var favorites = wordsReader.GetRandomFavorites(countOfWords, currentChatId);
            wordsForTest = favorites.WordsList;
            directionIdFrom = favorites.languageFromId;
            newVariant();
            positionWordInList++;
        }

        private void newVariant()
        {
            if(positionWordInList <= wordsForTest.Count - 1)
            {
                currentWord = wordsForTest[positionWordInList];
                var incorrectWords = wordsReader.GetIncorrectVariants(countOfVariants - 1, currentChatId, directionIdFrom, currentWord.TextFrom);
                Random rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
                int index = rnd.Next(0, countOfVariants);
                incorrectWords.Insert(index, currentWord);
                view.DrawNewVariant(currentWord, incorrectWords);
            } else
            {
                finalizeTest();
            }
        }

        private void finalizeTest()
        {
            positionWordInList = 0;
            var incorrectWords = results.Where(x => x.Item2 == false).GroupBy(x => x.Item1);
            view.SetFinalTest(countOfWords, countOfWords - incorrectWords.Count());
        }
    }
}