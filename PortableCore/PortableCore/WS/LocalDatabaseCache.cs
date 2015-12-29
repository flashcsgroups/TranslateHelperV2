using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using PortableCore.BL.Contracts;
using PortableCore.BL.Managers;
using PortableCore.DL;

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
                foreach(var item in translateResultCollection)
                {
                    var favoritesItem = favManager.GetItemForTranslatedExpressionId(item.ID);
                    int favItemId = 0;
                    if (favoritesItem != null)
                    {
                        favItemId = favoritesItem.ID;
                    }

                    RequestResult.translateResult.Collection.Add(new TranslateResult()
                        {
                           // OriginalText = sourceString,
                            TranslatedText = item.TranslatedText,
                            Pos = "*",
                            Ts = item.TranscriptionText,
                            TranslatedExpressionId = item.ID,
                            FavoritesId = favItemId
                        }
                    );
                }
            }
            return RequestResult;
        }

    }
}