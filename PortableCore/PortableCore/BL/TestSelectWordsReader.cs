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

        public List<TestWordItem> GetRandomFavorites(int maxCountOfWords, int chatId)
        {
            List<TestWordItem> resultList = new List<TestWordItem>();
            int languageFromId = getRandomDirection(chatId);
            Random rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            var listItems = from item in db.Table<ChatHistory>()
                       join parentItem in db.Table<ChatHistory>() on item.ParentRequestID equals parentItem.ID into favorites
                       from subFavorites in favorites
                       where item.ChatID == chatId && item.DeleteMark == 0 && item.LanguageFrom == languageFromId && item.InFavorites
                       select new Tuple<ChatHistory, ChatHistory>(item, subFavorites);
            int countOfItems = listItems.Count();
            for (int i = 0; i < (maxCountOfWords < countOfItems ? maxCountOfWords : countOfItems); i++)
            {
                int indexOfRecord = rnd.Next(0, countOfItems);
                var item = new TestWordItem() { TextFrom = separateAndGetRandom(listItems.ElementAt(indexOfRecord).Item2.TextFrom), TextTo = separateAndGetRandom(listItems.ElementAt(indexOfRecord).Item1.TextTo) };
                resultList.Add(item);
            }
            return resultList;
        }
        private int getRandomDirection(int chatId)
        {
            var listItems = from item in db.Table<Chat>() where item.ID == chatId && item.DeleteMark == 0 select item;
            Random rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            int index = rnd.Next(0, 2);
            int direction = index == 0 ? listItems.ElementAtOrDefault<Chat>(0).LanguageFrom : listItems.ElementAtOrDefault<Chat>(0).LanguageTo;
            return direction;
        }

        private string separateAndGetRandom(string textTo)
        {
            var arrayOfWords = textTo.Split(',');
            Random rnd = new Random(arrayOfWords.Count());
            int index = rnd.Next(arrayOfWords.Count() - 1);
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

        public List<string> GetIncorrectVariants(int excludeCorrectSourceId, int countOfIncorrectWords, TranslateDirection direction)
        {
            throw new NotImplementedException();
            /*var srcDefView = from item in db.Table<SourceExpression>()
                              join sourceDefItem in db.Table<SourceDefinition>() on item.ID equals sourceDefItem.SourceExpressionID into sources
                              from subSources in sources.DefaultIfEmpty()
                              where (item.ID != excludeCorrectSourceId)&&(item.DirectionID == direction.GetCurrentDirectionId()) 
                              select subSources.ID;

            var view = from item in db.Table<Favorites>()
                       join trExprItem in db.Table<TranslatedExpression>() on item.TranslatedExpressionID equals trExprItem.ID into expressions
                       from subExpressions in expressions.DefaultIfEmpty()
                       where srcDefView.Contains(subExpressions.SourceDefinitionID)
                       select subExpressions.TranslatedText;

            return view.Take(countOfIncorrectWords).ToList<string>();*/
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
}
