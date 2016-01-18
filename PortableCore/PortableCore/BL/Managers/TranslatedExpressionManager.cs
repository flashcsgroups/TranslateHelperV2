using System;
using System.Collections.Generic;
using System.Linq;
using PortableCore.BL.Contracts;
using PortableCore.DL;
using PortableCore.DAL;

namespace PortableCore.BL.Managers
{
	public class TranslatedExpressionManager : IDataManager<TranslatedExpression>
    {
        ISQLiteTesting db;

        public TranslatedExpressionManager(ISQLiteTesting dbHelper)
        {
            db = dbHelper;
        }

        public void InitDefaultData()
        {
        }

		public bool NeedUpdateDefaultData()
		{
			return false;
		}

        public TranslatedExpression GetItemForId(int id)
        {
            Repository<TranslatedExpression> repos = new Repository<TranslatedExpression>();
            TranslatedExpression result = repos.GetItem(id);
            return result;
        }

        //ToDo:Отрефакторить и тест
        public void AddNewWord(string originalExpression, List<TranslateResultView> resultList)
		{
            //ToDo:запись сделать через менеджер
            SourceExpressionManager sourceManager = new SourceExpressionManager(db);
            IEnumerable<SourceExpression> localCacheDataList = sourceManager.GetSourceExpressionCollection(originalExpression);

            int sourceItemID = 0;

            if (localCacheDataList.Count() > 0)
            {
                sourceItemID = localCacheDataList.ToList()[0].ID;
            }
            else
            {
                Repository<SourceExpression> sourceExpr = new Repository<SourceExpression>();
                SourceExpression itemSource = new SourceExpression();
                itemSource.Text = originalExpression;
                if(sourceExpr.Save(itemSource) == 1)
                {
                    localCacheDataList = sourceManager.GetSourceExpressionCollection(originalExpression);
                    sourceItemID = localCacheDataList.ToList()[0].ID;
                }
            }

            DefinitionTypesManager defTypesManager = new DefinitionTypesManager(db);
            List<DefinitionTypes> defTypesList = defTypesManager.GetItems();

            Repository<TranslatedExpression> reposTranslated = new Repository<TranslatedExpression>();
        }

		public List<TranslatedExpression> GetItems()
		{
			Repository<TranslatedExpression> repos = new Repository<TranslatedExpression> ();
			return new List<TranslatedExpression> (repos.GetItems ());
		}

        public List<Tuple<TranslatedExpression, Favorites>> GetListOfCoupleTranslatedExpressionAndFavorite(List<SourceDefinition> listOfDefinitions)
        {
            var arrayDefinitionIDs = from item in listOfDefinitions select item.ID;
            var view = from trItem in SqlLiteInstance.DB.Table<TranslatedExpression>()
                       join favItem in SqlLiteInstance.DB.Table<Favorites>() on trItem.ID equals favItem.TranslatedExpressionID into favorites
                       from subFavorite in favorites.DefaultIfEmpty()
                       where arrayDefinitionIDs.Contains(trItem.SourceDefinitionID) 
                       select new Tuple<TranslatedExpression, Favorites>(trItem, subFavorite);
            return view.ToList();
        }

	}
}

