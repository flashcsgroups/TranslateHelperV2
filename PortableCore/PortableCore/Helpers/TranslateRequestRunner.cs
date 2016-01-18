﻿using PortableCore.BL.Contracts;
using PortableCore.DAL;
using PortableCore.DL;
using PortableCore.WS;
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

        public TranslateRequestRunner(ISQLiteTesting db, IRequestTranslateString translaterFromCache, IRequestTranslateString translaterDictSrv, IRequestTranslateString translaterTranslateSrv)
        {
            this.db = db;
            this.translaterFromCache    = translaterFromCache;
            this.translaterDictSrv      = translaterDictSrv;
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
                result = translaterFromCache.Translate(originalText, direction).Result;
                if (result.TranslatedData.Definitions.Count == 0)
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