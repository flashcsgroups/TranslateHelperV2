using PortableCore.BL.Managers;
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
            var srcDefView = from item in db.Table<SourceExpression>()
                             join sourceDefItem in db.Table<SourceDefinition>() on item.ID equals sourceDefItem.SourceExpressionID into sources
                             from subSources in sources.DefaultIfEmpty(new SourceDefinition())
                             where item.DirectionID == direction.GetCurrentDirectionId()
                             select subSources.ID;
            var view = from item in db.Table<Favorites>()
                       join trExprItem in db.Table<TranslatedExpression>() on item.TranslatedExpressionID equals trExprItem.ID into expressions
                       from subExpressions in expressions.DefaultIfEmpty(new TranslatedExpression())
                       where srcDefView.Contains(subExpressions.SourceDefinitionID)
                       select new FavoriteItem() {FavoriteId = item.ID, TranslatedExpressionId = item.TranslatedExpressionID, SourceDefinitionId = subExpressions.SourceDefinitionID };
            var favDistinctView = from item in view
                                  join sourceDefItem in db.Table<SourceDefinition>() on item.SourceDefinitionId equals sourceDefItem.ID into sources
                                  from subSources in sources.DefaultIfEmpty(new SourceDefinition())
                                  select new FavoriteItem() { FavoriteId = item.FavoriteId, TranslatedExpressionId = item.TranslatedExpressionId, SourceDefinitionId = item.SourceDefinitionId, SourceExprId = subSources.SourceExpressionID  };
            var favElements = favDistinctView.Distinct();

            int countOfRecords = favElements.Count();
            FavoritesManager favManager = new FavoritesManager(db);
            Random rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            int maxCountOfWords = countOfWords <= countOfRecords ? countOfWords : countOfRecords;
            return favElements.Take(maxCountOfWords).ToList();
        }

        public List<string> GetIncorrectVariants(int excludeCorrectSourceId, int countOfIncorrectWords, TranslateDirection direction)
        {
            var srcDefView = from item in db.Table<SourceExpression>()
                              join sourceDefItem in db.Table<SourceDefinition>() on item.ID equals sourceDefItem.SourceExpressionID into sources
                              from subSources in sources.DefaultIfEmpty()
                              where (item.ID != excludeCorrectSourceId)&&(item.DirectionID == direction.GetCurrentDirectionId()) 
                              select subSources.ID;

            var view = from item in db.Table<Favorites>()
                       join trExprItem in db.Table<TranslatedExpression>() on item.TranslatedExpressionID equals trExprItem.ID into expressions
                       from subExpressions in expressions.DefaultIfEmpty()
                       where srcDefView.Contains(subExpressions.SourceDefinitionID)
                       select subExpressions.TranslatedText;

            return view.Take(countOfIncorrectWords).ToList<string>();
        }

        public int GetCountDifferenceSources(TranslateDirection direction)
        {
            return (from item in db.Table<SourceExpression>()
                   where item.DeleteMark == 0 && item.DirectionID == direction.GetCurrentDirectionId()
                   select item.ID).Count();
            
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
