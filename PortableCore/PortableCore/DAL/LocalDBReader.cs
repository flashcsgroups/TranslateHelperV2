using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using PortableCore.BL.Contracts;
using PortableCore.BL.Managers;
using PortableCore.DL;
using System;
using PortableCore.DAL;

namespace PortableCore.DAL
{
    public class LocalDBReader : IRequestTranslateString
    {
        private ISQLiteTesting db;

        public LocalDBReader(ISQLiteTesting dbHelper)
        {
            db = dbHelper;
        }

        public async Task<TranslateRequestResult> Translate(string sourceString, string direction)
        {
            TranslateRequestResult RequestResult = new TranslateRequestResult(sourceString);

            SourceExpressionManager sourceManager = new SourceExpressionManager(db);
            List<SourceExpression> sourceList = sourceManager.GetSourceExpressionCollection(sourceString).ToList<SourceExpression>();
            if (sourceList.Count > 0)
            {
                SourceDefinitionManager defManager = new SourceDefinitionManager(db);
                List<SourceDefinition> definitionsList = defManager.GetDefinitionCollection(sourceList[0].ID);

                TranslatedExpressionManager translatedManager = new TranslatedExpressionManager(db);
                var translatedList = translatedManager.GetListOfCoupleTranslatedExpressionAndFavorite(definitionsList);

                RequestResult.SetTranslateResult(createTranslateResult(sourceString, sourceList, definitionsList, translatedList));
                //getTranslatedResults(sourceId, listOfDefinitions);

                /*List<TranslatedExpression> listTranslatedExpression = translatedManager.GetTranslateResultFromLocalCache(defItem.ID);

                FavoritesManager favManager = new FavoritesManager();
                foreach (TranslatedExpression item in translateResultCollection)
                {
                    var favoritesItem = favManager.GetItemForTranslatedExpressionId(item.ID);

                }*/
                /*TranslatedExpressionManager translatedManager = new TranslatedExpressionManager();
                IEnumerable<TranslatedExpression> translateResultCollection = translatedManager.GetTranslateResultFromLocalCache(sourceId);
                FavoritesManager favManager = new FavoritesManager();
                List<Tuple<SourceExpression, SourceDefinition, TranslatedExpression, Favorites>> listOfCoupleTranslateAndFavorites = new List<Tuple<SourceExpression, SourceDefinition, TranslatedExpression, Favorites>>();
                foreach(TranslatedExpression item in translateResultCollection)
                {
                    var favoritesItem = favManager.GetItemForTranslatedExpressionId(item.ID);
                    listOfCoupleTranslateAndFavorites.Add(Tuple.Create(new SourceExpression(), new SourceDefinition(), item, favoritesItem));
                    RequestResult.translateResult.Collection.Add(new TranslateResult("")
                        {
                           // OriginalText = sourceString,
                            TranslatedText = item.TranslatedText,
                            Pos = "*",
                            Ts = item.TranscriptionText,
                            TranslatedExpressionId = item.ID,
                            FavoritesId = favItemId
                        }
                    );
                }*/
                //var translateResult = ConvertDataLocalCacheToTranslateResult(sourceString, listOfCoupleTranslateAndFavorites);
                //RequestResult.SetTranslateResult(translateResult);
            }
            return RequestResult;
        }

        private TranslateResultView createTranslateResult(string sourceString, List<SourceExpression> sourceList, List<SourceDefinition> definitionsList, List<Tuple<TranslatedExpression, Favorites>> translatedList)
        {
            TranslateResultView result = new TranslateResultView();
            foreach (var definition in definitionsList)
            {
                List<TranslateResultVariant> translateVariants = new List<TranslateResultVariant>();
                var viewVariants = from item in translatedList where item.Item1.DefinitionID == definition.ID select new { item.Item1, item.Item2 };
                foreach (var item in viewVariants)
                {
                    translateVariants.Add(new TranslateResultVariant(item.Item1.TranslatedText, (DefinitionTypesEnum)(item.Item1.DefinitionTypeID)));
                }
                result.AddDefinition(sourceString, (DefinitionTypesEnum)definition.DefinitionTypeID, definition.TranscriptionText, translateVariants);
            }
            return result;
        }

        /*public void GetTranslatedResults(int sourceId, List<SourceDefinition> listDefinitions)
        {
            IEnumerable<TranslateResult> view;

            foreach (var defItem in listDefinitions)
            {
                TranslatedExpressionManager translatedManager = new TranslatedExpressionManager();
                view = from trItem in SqlLiteInstance.DB.Table<TranslatedExpression>()
                       join favItem in SqlLiteInstance.DB.Table<Favorites>() on trItem.ID equals favItem.TranslatedExpressionID
                       where trItem.DefinitionID == sourceId
                       select new { trItem.ID, trItem.DefinitionID };


            }
        }*/

        /*public List<SourceDefinition> GetDefinitions(int sourceId)
        {
            SourceDefinitionManager defManager = new SourceDefinitionManager();
            List<SourceDefinition> listDefinitions = defManager.GetDefinitionCollection(sourceId);
            return listDefinitions;
        }*/

        /*public List<SourceExpression> GetSourceExprCollection(string sourceString)
        {
            SourceExpressionManager sourceManager = new SourceExpressionManager();
            IEnumerable<SourceExpression> sourceListIterator = sourceManager.GetItemsForText(sourceString);
            return sourceListIterator.ToList<SourceExpression>();
        }*/
    }
}