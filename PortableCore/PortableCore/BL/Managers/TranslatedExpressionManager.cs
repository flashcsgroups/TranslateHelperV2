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
            DAL.Repository<TranslatedExpression> repos = new PortableCore.DAL.Repository<TranslatedExpression>();
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
                DAL.Repository<SourceExpression> sourceExpr = new PortableCore.DAL.Repository<SourceExpression>();
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

            DAL.Repository<TranslatedExpression> reposTranslated = new PortableCore.DAL.Repository<TranslatedExpression>();
            //запись через Definition
            /*foreach (TranslateResult item in resultList)
            {
                TranslatedExpression translatedItem = new TranslatedExpression();
                translatedItem.TranslatedText = item.TranslatedText;
                translatedItem.TranscriptionText = item.Ts;
                translatedItem.SourceExpressionID = sourceItemID;
                if(item.Pos != null)
                {
                    DefinitionTypes defTypeWithConcreteName = defTypesList.Find(typeItem => typeItem.Name == item.Pos.ToLower());
                    if (defTypeWithConcreteName != null)
                        translatedItem.DefinitionTypeID = defTypeWithConcreteName.ID;
                }
                reposTranslated.Save(translatedItem);
            }*/
        }

		public List<TranslatedExpression> GetItems()
		{
			DAL.Repository<TranslatedExpression> repos = new PortableCore.DAL.Repository<TranslatedExpression> ();
			return new List<TranslatedExpression> (repos.GetItems ());
		}

        public List<Tuple<TranslatedExpression, Favorites>> GetListOfCoupleTranslatedExpressionAndFavorite(List<SourceDefinition> listOfDefinitions)
        {
            var arrayDefinitionIDs = from item in listOfDefinitions select item.ID;
            var view = from trItem in SqlLiteInstance.DB.Table<TranslatedExpression>()
                       join favItem in SqlLiteInstance.DB.Table<Favorites>() on trItem.ID equals favItem.TranslatedExpressionID 
                       where arrayDefinitionIDs.Contains(trItem.SourceDefinitionID) 
                       select new Tuple<TranslatedExpression, Favorites>(trItem, favItem);
            return view.ToList();
        }

	}
}

