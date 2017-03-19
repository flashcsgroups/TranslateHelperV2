using PortableCore.BL.Managers;
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
            RandomWordsList result = new RandomWordsList() { WordsList = new List<TestWordItem>()};
            List<TestWordItem> resultList = new List<TestWordItem>();
            result.languageFromId = getRandomDirection(chatId);
            Random rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            var listItems = from item in db.Table<ChatHistory>()
                       join parentItem in db.Table<ChatHistory>() on item.ParentRequestID equals parentItem.ID into favorites
                       from subFavorites in favorites
                       where item.ChatID == chatId && item.DeleteMark == 0 && item.LanguageFrom == result.languageFromId && item.InFavorites
                       select new Tuple<ChatHistory, ChatHistory>(item, subFavorites);
            var hashSet = new HashSet<string>();
            hashSet.Add(string.Empty);
            int countOfItems = listItems.Count();
            for (int i = 0; i < (maxCountOfWords < countOfItems ? maxCountOfWords : countOfItems); i++)
            {
                string textFrom = string.Empty;
                string textTo = string.Empty;
                while (!hashSet.Add(textFrom))
                {
                    int indexOfRecord = rnd.Next(0, countOfItems);
                    textFrom = listItems.ElementAt(indexOfRecord).Item2.TextFrom;
                    textTo = separateAndGetRandom(listItems.ElementAt(indexOfRecord).Item1.TextTo);
                }
                var item = new TestWordItem() { TextFrom = textFrom, TextTo = textTo };
                result.WordsList.Add(item);
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
            //int direction = index == 0 ? listItems.ElementAtOrDefault<Chat>(0).LanguageFrom : listItems.ElementAtOrDefault<Chat>(0).LanguageTo;
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

        private IEnumerable<FavoriteItem> getFavoritesDistinct(IEnumerable<FavoriteItem> view)
        {
            throw new NotImplementedException();
            /*return from item in view
                   join sourceDefItem in db.Table<SourceDefinition>() on item.SourceDefinitionId equals sourceDefItem.ID into sources
                   from subSources in sources.DefaultIfEmpty(new SourceDefinition())
                   select new FavoriteItem() { FavoriteId = item.FavoriteId, TranslatedExpressionId = item.TranslatedExpressionId, SourceDefinitionId = item.SourceDefinitionId, SourceExprId = subSources.SourceExpressionID };*/
        }

        private IEnumerable<FavoriteItem> getFavoritesBySourceDefinition(IEnumerable<int> srcDefView)
        {
            throw new NotImplementedException();
            /*return from item in db.Table<Favorites>()
                   join trExprItem in db.Table<TranslatedExpression>() on item.TranslatedExpressionID equals trExprItem.ID into expressions
                   from subExpressions in expressions.DefaultIfEmpty(new TranslatedExpression())
                   where srcDefView.Contains(subExpressions.SourceDefinitionID)
                   select new FavoriteItem() { FavoriteId = item.ID, TranslatedExpressionId = item.TranslatedExpressionID, SourceDefinitionId = subExpressions.SourceDefinitionID };*/
        }

        private IEnumerable<int> getSourceDefinitionByTranslateDirection(TranslateDirection direction)
        {
            throw new NotImplementedException();
            /*return from item in db.Table<SourceExpression>()
                   join sourceDefItem in db.Table<SourceDefinition>() on item.ID equals sourceDefItem.SourceExpressionID into sources
                   from subSources in sources.DefaultIfEmpty(new SourceDefinition())
                   where item.DirectionID == direction.GetCurrentDirectionId()
                   select subSources != null ? subSources.ID : 0;*/
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
