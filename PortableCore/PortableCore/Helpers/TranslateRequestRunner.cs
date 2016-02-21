using PortableCore.BL;
using PortableCore.BL.Contracts;
using PortableCore.BL.Managers;
using PortableCore.DAL;
using PortableCore.DL;
using System;
using System.Threading.Tasks;

namespace PortableCore.Helpers
{
    public class TranslateRequestRunner
    {
        ISQLiteTesting db;
        IRequestTranslateString translaterDictSrv;
        IRequestTranslateString translaterTranslateSrv;
        IRequestTranslateString translaterFromCache;

        //ToDo:Direction - В интерфейс
        public TranslateRequestRunner(ISQLiteTesting db, IRequestTranslateString translaterFromCache, IRequestTranslateString translaterDictSrv, IRequestTranslateString translaterTranslateSrv)
        {
            this.db = db;
            this.translaterFromCache    = translaterFromCache;
            this.translaterDictSrv      = translaterDictSrv;
            this.translaterTranslateSrv = translaterTranslateSrv;
        }

        public async Task<TranslateRequestResult> GetDictionaryResult(string originalText, TranslateDirection direction)
        {
            var result = await request(translaterDictSrv, originalText);
            if(result.TranslatedData.Definitions.Count > 0)
                saveResultToLocalCache(result, direction);
            return result;
        }

        private void saveResultToLocalCache(TranslateRequestResult result, TranslateDirection direction)
        {
            CachedResultWriter localDBWriter = new CachedResultWriter(direction, db, new SourceExpressionManager(db));
            localDBWriter.SaveResultToLocalCacheIfNotExist(result);
        }

        public async Task<TranslateRequestResult> GetTranslationResult(string originalText, TranslateDirection direction)
        {
            var result = await request(translaterTranslateSrv, originalText);
            if (result.TranslatedData.Definitions.Count > 0)
                saveResultToLocalCache(result, direction);
            return result;
        }

        private async Task<TranslateRequestResult> request(IRequestTranslateString service, string originalText)
        {
            string convertedSourceText = ConvertStrings.StringToOneLowerLineWithTrim(originalText);
            TranslateRequestResult result = new TranslateRequestResult(convertedSourceText);
            result = translaterFromCache.Translate(originalText).Result;
            if (result.TranslatedData.Definitions.Count == 0)
            {
                result = await service.Translate(originalText);
                /*if (!string.IsNullOrEmpty(result.errorDescription))
                {
                    //ToDo: сделать общий обработчик ошибок
                    throw new Exception("Ошибка подключения к интернет:" + result.errorDescription);
                }*/
            }
            return result;
        }
    }
}