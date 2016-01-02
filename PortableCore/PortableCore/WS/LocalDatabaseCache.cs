using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using PortableCore.BL.Contracts;
using PortableCore.BL.Managers;
using PortableCore.DL;
using System;

namespace PortableCore.WS
{
    public class LocalDatabaseCache : IRequestTranslateString
    {
        public LocalDatabaseCache()
        {

        }

        public async Task<TranslateRequestResult> Translate(string sourceString, string direction)
        {
            TranslateRequestResult RequestResult = new TranslateRequestResult();

            SourceExpressionManager sourceManager = new SourceExpressionManager();
            IEnumerable<SourceExpression> sourceListIterator = sourceManager.GetItemsForText(sourceString);
            var sourceList = sourceListIterator.ToList<SourceExpression>();
            if (sourceList.Count > 0)
            {
                int sourceId = sourceList[0].ID;
                TranslatedExpressionManager translatedManager = new TranslatedExpressionManager();
                IEnumerable<TranslatedExpression> translateResultCollection = translatedManager.GetTranslateResultFromLocalCache(sourceId);
                FavoritesManager favManager = new FavoritesManager();
                List<Tuple<TranslatedExpression, Favorites>> listOfCoupleTranslateAndFavorites = new List<Tuple<TranslatedExpression, Favorites>>();
                foreach(TranslatedExpression item in translateResultCollection)
                {
                    var favoritesItem = favManager.GetItemForTranslatedExpressionId(item.ID);
                    /*int favItemId = 0;
                    if (favoritesItem != null)
                    {
                        favItemId = favoritesItem.ID;

                    }*/
                    listOfCoupleTranslateAndFavorites.Add(Tuple.Create(item, favoritesItem));
                    /*RequestResult.translateResult.Collection.Add(new TranslateResult("")
                        {
                           // OriginalText = sourceString,
                            TranslatedText = item.TranslatedText,
                            Pos = "*",
                            Ts = item.TranscriptionText,
                            TranslatedExpressionId = item.ID,
                            FavoritesId = favItemId
                        }
                    );*/
                }
                var translateResult = ConvertCacheToTranslateResult(sourceString, listOfCoupleTranslateAndFavorites);
                RequestResult.SetTranslateResult(translateResult);
            }
            return RequestResult;
        }

        public TranslateResult ConvertCacheToTranslateResult(string sourceString, List<Tuple<TranslatedExpression, Favorites>> listOfCoupleTranslateAndFavorites)
        {
            /*string originalString = string.Empty;
            if (deserializedObject.Def.Count > 0)
            {
                originalString = deserializedObject.Def[0].Text;
            }
            else
            {
                originalString = "";
            }*/
            TranslateResult result = new TranslateResult(sourceString);
            /*foreach (var def in deserializedObject.Def)
            {
                var translateVariantsSource = def.Tr;
                var translateVariants = new List<TranslateVariant>();
                foreach (var tr in translateVariantsSource)
                {
                    translateVariants.Add(new TranslateVariant(tr.Text, DefinitionTypesManager.GetEnumDefinitionType(tr.Pos)));
                }
                result.AddDefinition(DefinitionTypesManager.GetEnumDefinitionType(def.Pos), def.Ts, translateVariants);
            }*/
            return result;
        }

    }
}