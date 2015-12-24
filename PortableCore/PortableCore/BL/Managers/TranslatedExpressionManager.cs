using System;
using System.Collections.Generic;
using System.Linq;
using PortableCore.BL.Contracts;
using PortableCore.DL;

namespace PortableCore.BL.Managers
{
	public class TranslatedExpressionManager : IDataManager<TranslatedExpression>
    {
		public TranslatedExpressionManager ()
		{
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
        public void AddNewWord(string originalExpression, List<TranslateResult> resultList)
		{
            //ToDo:запись сделать через менеджер
            SourceExpressionManager sourceManager = new SourceExpressionManager();
            IEnumerable<SourceExpression> localCacheDataList = sourceManager.GetItemsForText(originalExpression);

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
                    localCacheDataList = sourceManager.GetItemsForText(originalExpression);
                    sourceItemID = localCacheDataList.ToList()[0].ID;
                }
            }

            DefinitionTypesManager defTypesManager = new DefinitionTypesManager();
            List<DefinitionTypes> defTypesList = defTypesManager.GetItems();

            DAL.Repository<TranslatedExpression> reposTranslated = new PortableCore.DAL.Repository<TranslatedExpression>();
            foreach (TranslateResult item in resultList)
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
            }
        }

		public List<TranslatedExpression> GetItems()
		{
			DAL.Repository<TranslatedExpression> repos = new PortableCore.DAL.Repository<TranslatedExpression> ();
			return new List<TranslatedExpression> (repos.GetItems ());
		}

        //ToDo:Отказаться от передачи INumerator в пользу List
        public IEnumerable<TranslatedExpression> GetTranslateResultFromLocalCache(int sourceId)
        {
            throw new Exception("not realized");
            //return SqlLiteInstance.DB.Table<TranslatedExpression>().ToList().Where(item => item.SourceExpressionID == sourceId);
        }

	}
}

