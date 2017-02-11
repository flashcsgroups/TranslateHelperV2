using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PortableCore.BL.Contracts;
using Newtonsoft.Json;
using PortableCore.WS.Contracts;

namespace PortableCore.WS
{
    public class YandexTranslateJSON : TranslateRequestFactory
    {
        //ToDo:Перенести в настройки для хранения в базе
        private string sourceString;

        public override void SetSourceString(string sourceString)
        {
            this.sourceString = sourceString;
        }

        public override async Task<string> GetResponse(string direction)
        {
            YandexApiKeyHelper yandexKeyHelper = new YandexApiKeyHelper();
            string url = string.Format("https://translate.yandex.net/api/v1.5/tr.json/translate?key={0}&text={1}&lang={2}&format=plain", yandexKeyHelper.GetTranslateApiKey(), sourceString, direction);
            return await GetJsonResponse(url);
        }

        public override TranslateResultView Parse(string responseText)
        {
            TranslateResultView result;
            YandexTranslateScheme deserializedResponse = JsonConvert.DeserializeObject<YandexTranslateScheme>(responseText);
            if (null != deserializedResponse)
                result = convertResponseToTranslateResult(deserializedResponse);
            else
                result = new TranslateResultView();
            return result;
        }

        private TranslateResultView convertResponseToTranslateResult(YandexTranslateScheme deserializedObject)
        {
            TranslateResultView result = new TranslateResultView();
            var translateVariants = new List<ResultLineData>();
            translateVariants.Add(new ResultLineData(deserializedObject.Text[0], DefinitionTypesEnum.translater));
            result.AddDefinition(sourceString, DefinitionTypesEnum.translater, string.Empty, translateVariants);
            return result;
        }
    }
}