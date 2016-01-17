using System;
using PortableCore.BL.Contracts;
using System.Linq;
using PortableCore.DL;
using PortableCore.BL.Managers;

using System.Collections.Generic;
using PortableCore.DAL;

namespace TranslateHelper.Droid
{
    public class CachedResultWriter
    {
        private ISQLiteTesting db;
        private ISourceExpressionManager sourceExpressionManager;

        public CachedResultWriter(ISQLiteTesting db, ISourceExpressionManager sourceExpressionManager)
        {
            this.db = db;
            this.sourceExpressionManager = sourceExpressionManager;
        }

        public void SaveResultToLocalCache(TranslateRequestResult result)
        {
            if (string.IsNullOrEmpty(result.errorDescription))
            {
                int sourceItemID = 0;
                string originalText = result.OriginalText;
                TranslateResultView resultView = result.TranslatedData;
                IEnumerable<SourceExpression> localCacheDataList = sourceExpressionManager.GetSourceExpressionCollection(originalText);
                if (localCacheDataList.Count() > 0)
                {
                    sourceItemID = localCacheDataList.ToList()[0].ID;
                }
                else
                {
                    sourceItemID = writeSourceExpression(originalText, ref localCacheDataList);
                }
                writeTranslatedExpression(sourceItemID, resultView);
            }
            else throw new Exception(result.errorDescription);//ToDo:Нужен общий обработчик ошибок
        }

        private void writeTranslatedExpression(int sourceItemID, TranslateResultView resultView)
        {
            DefinitionTypesManager defTypesManager = new DefinitionTypesManager(db);
            List<DefinitionTypes> defTypesList = defTypesManager.GetItems();
            Repository<SourceDefinition> reposSourceDefinition = new Repository<SourceDefinition>();
            Repository<TranslatedExpression> reposTranslatedExpression = new Repository<TranslatedExpression>();
            foreach (var curDefinition in resultView.Definitions)
            {
                SourceDefinition sourceDefinitionItem = new SourceDefinition();
                sourceDefinitionItem.SourceExpressionID = sourceItemID;
                sourceDefinitionItem.TranscriptionText = curDefinition.Transcription;
                sourceDefinitionItem.DefinitionTypeID = (int)curDefinition.Pos;
                reposSourceDefinition.Save(sourceDefinitionItem);
                int sourceDefId = getSourceDefinitionItemId(sourceItemID, curDefinition);
                foreach (var curVariant in curDefinition.TranslateVariants)
                {
                    TranslatedExpression translatedItem = new TranslatedExpression();
                    translatedItem.TranslatedText = curVariant.Text;
                    translatedItem.SourceDefinitionID = sourceDefId;
                    translatedItem.DefinitionTypeID = (int)curVariant.Pos;
                    reposTranslatedExpression.Save(translatedItem);
                }
            }
        }

        private int getSourceDefinitionItemId(int sourceItemID, TranslateResultDefinition curDefinition)
        {
            var savedSourceDefinitionsItems = from item in SqlLiteInstance.DB.Table<SourceDefinition>()
                                              where (item.SourceExpressionID == sourceItemID) && (item.DefinitionTypeID == (int)curDefinition.Pos) && (item.DeleteMark == 0)
                                              select new SourceDefinition() { ID = item.ID, DefinitionTypeID = item.DefinitionTypeID, DeleteMark = item.DeleteMark, SourceExpressionID = item.SourceExpressionID, TranscriptionText = item.TranscriptionText };
            //ToDo:Просто жесть. Я в тупике, почему ни метод Save ни Insert не возвращают ИД записанного элемента, не понимаю.
            return savedSourceDefinitionsItems.FirstOrDefault().ID;
        }

        private int writeSourceExpression(string originalText, ref IEnumerable<SourceExpression> localCacheDataList)
        {
            int sourceItemID = 0;
            Repository<SourceExpression> sourceExpr = new Repository<SourceExpression>();
            SourceExpression itemSource = new SourceExpression();
            itemSource.Text = originalText;
            if (sourceExpr.Save(itemSource) == 1)
            {
                localCacheDataList = sourceExpressionManager.GetSourceExpressionCollection(originalText);
                sourceItemID = localCacheDataList.ToList()[0].ID;
            }
            return sourceItemID;
        }
    }
}