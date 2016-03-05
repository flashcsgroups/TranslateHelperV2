using System;
using PortableCore.BL.Contracts;
using System.Linq;
using PortableCore.DL;
using PortableCore.BL.Managers;

using System.Collections.Generic;
using PortableCore.DAL;
using PortableCore.BL;

namespace PortableCore.DAL
{
    public class CachedResultWriter
    {
        private ISQLiteTesting db;
        private ISourceExpressionManager sourceExpressionManager;
        private TranslateDirection direction;

        public CachedResultWriter(TranslateDirection direction, ISQLiteTesting db, ISourceExpressionManager sourceExpressionManager)
        {
            this.direction = direction;
            this.db = db;
            this.sourceExpressionManager = sourceExpressionManager;
        }

        //ToDo:Нужен общий обработчик ошибок
        //ToDo:Тест!
        public void SaveResultToLocalCacheIfNotExist(TranslateRequestResult result)
        {
            if (string.IsNullOrEmpty(result.errorDescription))
            {
                int sourceItemID = 0;
                string originalText = result.OriginalText;
                TranslateResultView resultView = result.TranslatedData;
                IEnumerable<SourceExpression> localCacheDataList = sourceExpressionManager.GetSourceExpressionCollection(originalText, direction);
                if (localCacheDataList.Count() == 0)
                {
                    sourceItemID = writeSourceExpression(originalText, ref localCacheDataList);
                    writeTranslatedExpression(sourceItemID, resultView);
                }
            }
            else throw new Exception(result.errorDescription);
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
                    translatedItem.DirectionID = direction.GetCurrentDirectionId();
                    reposTranslatedExpression.Save(translatedItem);
                    fillTranslateResultIdsForNewItem(translatedItem.SourceDefinitionID, translatedItem.DefinitionTypeID, curVariant);
                }
            }
        }

        private void fillTranslateResultIdsForNewItem(int sourceDefinitionID, int definitionTypeID, ResultLineData curVariant)
        {
            var savedTranslatedExpressionItems = from item in SqlLiteInstance.DB.Table<TranslatedExpression>()
                                              where (item.SourceDefinitionID == sourceDefinitionID) && (item.DeleteMark == 0) && (item.TranslatedText == curVariant.Text)
                                              select new TranslatedExpression {ID = item.ID};
            curVariant.TranslatedExpressionId = savedTranslatedExpressionItems.ToList()[0].ID;
        }

        private int getSourceDefinitionItemId(int sourceItemID, TranslateResultDefinition curDefinition)
        {
            var savedSourceDefinitionsItems = from item in SqlLiteInstance.DB.Table<SourceDefinition>()
                                              where (item.SourceExpressionID == sourceItemID) && (item.DefinitionTypeID == (int)curDefinition.Pos) && (item.DeleteMark == 0)
                                              select new SourceDefinition() { ID = item.ID, DefinitionTypeID = item.DefinitionTypeID, DeleteMark = item.DeleteMark, SourceExpressionID = item.SourceExpressionID, TranscriptionText = item.TranscriptionText };
            //ToDo:Просто жесть. Я в тупике, почему ни метод Save ни Insert не возвращают ИД записанного элемента, не понимаю.
            int id = 0;
            var firstItem = savedSourceDefinitionsItems.FirstOrDefault();
            if (firstItem != null)
                id = firstItem.ID;
            return id;
        }

        private int writeSourceExpression(string originalText, ref IEnumerable<SourceExpression> localCacheDataList)
        {
            int sourceItemID = 0;
            Repository<SourceExpression> sourceExpr = new Repository<SourceExpression>();
            SourceExpression itemSource = new SourceExpression();
            itemSource.Text = originalText;
            itemSource.DirectionID = direction.GetCurrentDirectionId();
            if (sourceExpr.Save(itemSource) == 1)
            {
                localCacheDataList = sourceExpressionManager.GetSourceExpressionCollection(originalText, direction);
                sourceItemID = localCacheDataList.ToList()[0].ID;
            }
            return sourceItemID;
        }
    }
}