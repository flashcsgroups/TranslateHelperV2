using System;
using System.IO;
using System.Json;
using System.Net;
using System.Threading.Tasks;
using TranslateHelper.Core.BL.Contracts;

namespace TranslateHelper.Core.WS
{
    public class TranslateRequest : IRequestTranslateString 
    {
        private TranslateRequestFactory translater;

        public TranslateRequest(TypeTranslateServices typeSrv)
        {
            //ToDo:���������� �� ������. ���� ��������� ���.
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

        public async Task<TranslateRequestResult> Translate(string sourceString, string direction)
        {
            TranslateRequestResult RequestResult = new TranslateRequestResult();

            string responseText = string.Empty;
            try
            {
                responseText = await translater.GetResponse(sourceString, direction);
            }
            catch(Exception e)
            {
                RequestResult.errorDescription = e.Message;
            }

            if(string.IsNullOrEmpty(RequestResult.errorDescription))
                RequestResult.translateResult = translater.ParseResponse(responseText);

            return RequestResult;
        }

    }
}