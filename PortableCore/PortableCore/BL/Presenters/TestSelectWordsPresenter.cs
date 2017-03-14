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
        int countOfWords;
        int positionWordInList;
        //int countOfVariantsWithoutCorrect = 7;//значение по-умолчанию для количества возможных вариантов без учета правильного варианта
        int countOfVariants = 8;//Максимальное количество вариантов
        ITestSelectWordsView view;
        ISQLiteTesting db;
        ITestSelectWordsReader wordsReader;
        int currentChatId;
        TestWordItem rightWord = new TestWordItem();

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
            int diff = string.Compare(selectedWord, rightWord.TextTo, StringComparison.CurrentCultureIgnoreCase);
            bool checkResult = diff == 0;
            if (!checkResult)
            {
                view.SetButtonErrorState();
            }
            else
            {
                //view.SetButtonNormalState();
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
                positionWordInList = 0;
                view.SetFinalTest(countOfWords);
            }
        }

        public void Init()
        {
            newVariant();
        }

        private void newVariant()
        {
            var portionOfWordsForTestList = wordsReader.GetRandomFavorites(countOfVariants, currentChatId);
            rightWord = chooseCorrectWord(portionOfWordsForTestList);
            view.DrawNewVariant(rightWord, portionOfWordsForTestList);
        }

        private TestWordItem chooseCorrectWord(List<TestWordItem> portionOfWordsForTestList)
        {
            Random rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            int index = rnd.Next(0, portionOfWordsForTestList.Count);
            return portionOfWordsForTestList[index];

        }
    }
}