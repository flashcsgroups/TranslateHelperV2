using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using PortableCore.BL.Contracts;
using PortableCore.BL;

namespace PortableCore.WS
{
    public class TranslateRequest : IRequestTranslateString 
    {
        private TranslateRequestFactory translater;
        private TranslateDirection direction;

        public TranslateRequest(TypeTranslateServices typeSrv, TranslateDirection direction)
        {
            this.direction = direction;
            //ToDo:Отказаться от свитча. Пока непонятно как.
            switch (typeSrv)
            {
                case TypeTranslateServices.YandexTranslate:
                    {
                        translater = new YandexTranslateJSON();
                    };
                    break;
                case TypeTranslateServices.YandexDictionary:
                    {
                        translater = new YandexDictionaryJSON();
                    };
                    break;
                default:
                    {
                    };
                    break;
            }
        }

        public async Task<TranslateRequestResult> Translate(string sourceString)
        {
            TranslateRequestResult RequestResult = new TranslateRequestResult(sourceString);

            string responseText = string.Empty;
            try
            {
                translater.SetSourceString(sourceString);
                responseText = await translater.GetResponse(string.Format("{0}-{1}", direction.LanguageFrom.NameShort, direction.LanguageTo.NameShort));
            }
            catch (Exception e)
            {
                RequestResult.errorDescription = e.Message;
            }

            //if (string.IsNullOrEmpty(RequestResult.errorDescription))
            //ToDo:логировать ошибку запроса
            //if (RequestResult.TranslatedData.Definitions.Count > 0)
            RequestResult.SetTranslateResult(translater.Parse(responseText));

            return RequestResult;
        }

    }
}