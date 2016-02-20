using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Globalization;
using PortableCore.BL;
using PortableCore.DL;
using PortableCore.BL.Managers;

namespace TranslateHelper.Droid
{

    public class TestSelectWordsPresenter
    {
        int maxCountOfWords;
        int countOfWords;
        int positionWordInList;
        ITestSelectWordsView view;
        ISQLiteTesting db;
        TranslateDirection direction;
        List<Favorites> favoritesList;
        string rightWord;

        public TestSelectWordsPresenter(ITestSelectWordsView view, ISQLiteTesting db, int maxCountOfWords)
        {
            this.view = view;
            this.db = db;
            this.maxCountOfWords = maxCountOfWords;
            this.direction = new TranslateDirection(db);
            direction.SetDefaultDirection();
        }

        public void OnSelectVariant(string selectedWord)
        {
            int diff = string.Compare(selectedWord, rightWord, true);

            view.SetCheckResult(diff == 0);                
        }

        internal void OnSubmit()
        {
            if(positionWordInList < countOfWords)
            {
                string nextWord = getNextWordText();
                view.SetOriginalWord(nextWord);
                view.SetVariants(rightWord);
                positionWordInList++;
            } else
            {

            }
        }

        internal void StartTest()
        {
            TestSelectWordsReader data = new TestSelectWordsReader(db);
            favoritesList = data.GetRandomFavorites(maxCountOfWords, direction);
            countOfWords = favoritesList.Count();
            OnSubmit();
        }

        private string getNextWordText()
        {
            var item = favoritesList[positionWordInList];
            TranslatedExpressionManager teManager = new TranslatedExpressionManager(db);
            TranslatedExpression trexpressionItem = teManager.GetItemForId(item.TranslatedExpressionID);
            rightWord = trexpressionItem.TranslatedText;
            SourceDefinitionManager sdManager = new SourceDefinitionManager(db);
            SourceDefinition sdItem = sdManager.GetItemForId(trexpressionItem.SourceDefinitionID);
            SourceExpressionManager seManager = new SourceExpressionManager(db);
            SourceExpression seItem = seManager.GetItemForId(sdItem.SourceExpressionID);
            return seItem.Text;
        }
    }
}