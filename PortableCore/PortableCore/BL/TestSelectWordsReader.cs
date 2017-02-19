using PortableCore.BL.Managers;
using PortableCore.BL.Models;
using PortableCore.DL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PortableCore.BL
{
    public class TestSelectWordsReader : ITestSelectWordsReader
    {
        ISQLiteTesting db;
        public TestSelectWordsReader(ISQLiteTesting dbHelper)
        {
            db = dbHelper;
        }

        public List<FavoriteItem> GetRandomFavorites(int countOfWords, TranslateDirection direction)
        {
            throw new NotImplementedException();
            /*IEnumerable<int> srcDefView = getSourceDefinitionByTranslateDirection(direction);
            IEnumerable<FavoriteItem> favView = getFavoritesBySourceDefinition(srcDefView);
            IEnumerable<FavoriteItem> favDistinctView = getFavoritesDistinct(favView);
            var favElements = favDistinctView.Distinct();

            int countOfRecords = favElements.Count();
            FavoritesManager favManager = new FavoritesManager(db);
            Random rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            int maxCountOfWords = countOfWords <= countOfRecords ? countOfWords : countOfRecords;
            List<FavoriteItem> items = new List<FavoriteItem>();
            List<int> usedIdList = new List<int>();
            for(int i=0;i< maxCountOfWords;i++)
            {
                int indexOfRecord = rnd.Next(0, maxCountOfWords - 1);
                if (usedIdList.Contains(indexOfRecord))
                    indexOfRecord = rnd.Next(0, maxCountOfWords - 1);
                //ToDo:нужна проверка в цикле, может быть ситуация с повторными ид
                items.Add(favElements.ElementAt(indexOfRecord));
                usedIdList.Add(indexOfRecord);
            }
            return items;*/
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
