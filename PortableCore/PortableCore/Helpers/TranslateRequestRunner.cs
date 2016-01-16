using PortableCore.BL.Contracts;
using PortableCore.WS;
using System;
using System.Threading.Tasks;

namespace PortableCore.Helpers
{
    public class TranslateRequestRunner
    {
        IRequestTranslateString translaterDictSrv;
        IRequestTranslateString translaterTranslateSrv;

        public TranslateRequestRunner(IRequestTranslateString translaterDictSrv, IRequestTranslateString translaterTranslateSrv)
        {
            this.translaterDictSrv = translaterDictSrv;
            this.translaterTranslateSrv = translaterTranslateSrv;
        }

        public async Task<TranslateRequestResult> GetDictionaryResult(string originalText, string direction)
        {
            return await request(translaterDictSrv, originalText, direction);
        }

        public async Task<TranslateRequestResult> GetTranslationResult(string originalText, string direction)
        {
            return await request(translaterTranslateSrv, originalText, direction);
        }

        private async Task<TranslateRequestResult> request(IRequestTranslateString service, string originalText, string direction)
        {
            string convertedSourceText = ConvertStrings.StringToOneLowerLineWithTrim(originalText);
            TranslateRequestResult result = new TranslateRequestResult(convertedSourceText);
            if (convertedSourceText.Length > 0)
            {
                /*IRequestTranslateString translaterFromCache = new LocalDBCacheReader(SqlLiteInstance.DB);
                var resultFromCache = translaterFromCache.Translate(originalText, direction);
                if (resultFromCache.translateResult.Collection.Count > 0)
                {
                    updateListResults(sourceText, resultFromCache, false);
                }
                else*/
                {
                    result = await service.Translate(originalText, direction);
                    if (!string.IsNullOrEmpty(result.errorDescription))
                    {
                        //ToDo: сделать общий обработчик ошибок
                        throw new Exception("Ошибка подключения к интернет:" + result.errorDescription);
                    }
                }
            }

            return result;
        }
    }
}