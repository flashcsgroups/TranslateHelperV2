﻿using PortableCore.BL.Managers;
using PortableCore.BL.Models;
using PortableCore.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PortableCore.BL
{
    public class TestSelectWordsReader : ITestSelectWordsReader
    {
        ISQLiteTesting db;
        public TestSelectWordsReader(ISQLiteTesting dbHelper)
        {
            db = dbHelper;
        }

        public RandomWordsList GetRandomFavorites(int maxCountOfWords, int chatId)
        {
            RandomWordsList result = new RandomWordsList() { WordsList = new List<TestWordItem>() };
            List<TestWordItem> resultList = new List<TestWordItem>();
            result.languageFromId = getRandomDirection(chatId);
            var listItems = from item in db.Table<ChatHistory>()
                            join parentItem in db.Table<ChatHistory>() on item.ParentRequestID equals parentItem.ID into favorites
                            from subFavorites in favorites
                            where item.ChatID == chatId && item.DeleteMark == 0 && item.LanguageFrom == result.languageFromId && item.InFavorites
                            select new Tuple<ChatHistory, ChatHistory>(item, subFavorites);
            result.WordsList = getRandomWordsFromList(maxCountOfWords, listItems);
            return result;
        }

        private List<TestWordItem> getRandomWordsFromList(int maxCountOfWords, IEnumerable<Tuple<ChatHistory, ChatHistory>> listItems)
        {
            List<TestWordItem> result = new List<TestWordItem>();
            Random rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            var indexHashSet = new HashSet<int>();
            int countOfItems = listItems.Count();
            int indexOfRecord = 0;
            for (int i = 0; i < (maxCountOfWords < countOfItems ? maxCountOfWords : countOfItems); i++)
            {
                while (!indexHashSet.Add(indexOfRecord))
                {
                    indexOfRecord = rnd.Next(0, countOfItems);
                }
            }
            foreach(int index in indexHashSet)
            {
                string textFrom = listItems.ElementAt(index).Item2.TextFrom;
                string textTo = separateAndGetRandom(listItems.ElementAt(index).Item1.TextTo);
                var item = new TestWordItem() { TextFrom = textFrom, TextTo = textTo };
                result.Add(item);
            }
            return result;
        }

        private int getRandomDirection(int chatId)
        {
            int minimumCountMessages = 10;
            var listItems = from item in db.Table<ChatHistory>()
                            where item.ChatID == chatId && item.DeleteMark == 0
                            group item by item.LanguageFrom into g
                            select new { LanguageFrom = g.Key, MsgCount = g.Count() };
            var listItemsWithSuitableCount = from item in listItems where item.MsgCount > minimumCountMessages select item.LanguageFrom;
            Random rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            int index = rnd.Next(0, listItemsWithSuitableCount.Count());
            int direction = listItemsWithSuitableCount.ElementAtOrDefault<int>(index);
            return direction;
        }

        private string separateAndGetRandom(string textTo)
        {
            var arrayOfWords = textTo.Split(',');
            Random rnd = new Random(arrayOfWords.Count());
            int index = rnd.Next(arrayOfWords.Count());
            return arrayOfWords[index].Trim();
        }

        public List<TestWordItem> GetIncorrectVariants(int countOfIncorrectWords, int chatId, int languageFromId, string correctWord)
        {
            List<TestWordItem> resultList = new List<TestWordItem>();
            //int languageFromId = getRandomDirection(chatId);
            Random rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            var listItems = from item in db.Table<ChatHistory>()
                            join parentItem in db.Table<ChatHistory>() on item.ParentRequestID equals parentItem.ID into favorites
                            from subFavorites in favorites
                            where item.ChatID == chatId && item.DeleteMark == 0 && item.LanguageFrom == languageFromId && item.InFavorites
                            select new Tuple<ChatHistory, ChatHistory>(item, subFavorites);
            var hashSet = new HashSet<string>();
            hashSet.Add(string.Empty);
            hashSet.Add(correctWord);
            int countOfItems = listItems.Count();
            for (int i = 0; i < (countOfIncorrectWords < countOfItems ? countOfIncorrectWords : countOfItems); i++)
            {
                //string textFrom = "testFrom";
                //string textTo = "testTo";
                string textFrom = string.Empty;
                string textTo = string.Empty;
                while (!hashSet.Add(textFrom))
                {
                    int indexOfRecord = rnd.Next(0, countOfItems);
                    textFrom = listItems.ElementAt(indexOfRecord).Item2.TextFrom;
                    textTo = separateAndGetRandom(listItems.ElementAt(indexOfRecord).Item1.TextTo);
                }
                var item = new TestWordItem() { TextFrom = textFrom, TextTo = textTo };
                resultList.Add(item);
            }
            return resultList;
        }

        public int GetCountDifferenceSources(TranslateDirection direction)
        {
            throw new NotImplementedException();
            /*return (from item in db.Table<SourceExpression>()
                   where item.DeleteMark == 0 && item.DirectionID == direction.GetCurrentDirectionId()
                   select item.ID).Count();*/
            
        }

        public Tuple<string, string> GetNextWord(int translatedExpressionID)
        {
            TranslatedExpressionManager teManager = new TranslatedExpressionManager(db);
            TranslatedExpression trexpressionItem = teManager.GetItemForId(translatedExpressionID);
            string translatedText = trexpressionItem.TranslatedText;
            SourceDefinitionManager sdManager = new SourceDefinitionManager(db);
            SourceDefinition sdItem = sdManager.GetItemForId(trexpressionItem.SourceDefinitionID);
            SourceExpressionManager seManager = new SourceExpressionManager(db);
            SourceExpression seItem = seManager.GetItemForId(sdItem.SourceExpressionID);
            Tuple<string, string> pair = new Tuple<string, string>(seItem.Text, translatedText);
            return pair;
        }
    }
    public struct RandomWordsList
    {
        public List<TestWordItem> WordsList;
        public int languageFromId;
    }
}
