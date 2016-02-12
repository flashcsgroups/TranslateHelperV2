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
    /// <summary>
    /// ѕредназначен дл€ возврата ранее кешированного результата запроса
    /// </summary>
    public class CachedResultReader : IRequestTranslateString
    {
        private ISQLiteTesting db;

        public CachedResultReader(ISQLiteTesting dbHelper)
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
            }
            return RequestResult;
        }

        private TranslateResultView createTranslateResult(string sourceString, List<SourceExpression> sourceList, List<SourceDefinition> definitionsList, List<Tuple<TranslatedExpression, Favorites>> translatedList)
        {
            TranslateResultView result = new TranslateResultView();
            foreach (var definition in definitionsList)
            {
                List<ResultLineData> translateVariants = new List<ResultLineData>();
                var viewVariants = from item in translatedList where item.Item1.SourceDefinitionID == definition.ID select new { item.Item1, item.Item2 };
                foreach (var item in viewVariants)
                {
                    var dataLine = new ResultLineData(item.Item1.TranslatedText, (DefinitionTypesEnum)(item.Item1.DefinitionTypeID));
                    dataLine.TranslatedExpressionId = item.Item1.ID;
                    if(item.Item2!=null)
                    {
                        dataLine.FavoritesId = item.Item2.ID;
                    }
                    translateVariants.Add(dataLine);
                }
                result.AddDefinition(sourceString, (DefinitionTypesEnum)definition.DefinitionTypeID, definition.TranscriptionText, translateVariants);
            }
            return result;
        }
    }
}