using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PortableCore.BL.Contracts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PortableCore.WS.Contracts;
using PortableCore.BL.Managers;

namespace PortableCore.WS
{
    public class YandexDictionaryJSON : TranslateRequestFactory
    {
        private string AuthKey = "dict.1.1.20151130T123842Z.c9e307b1d977e039.89e55f949ba679f1f024feba1f6033701acf58bd";
        private string sourceString;

        public override void SetSourceString(string sourceString)
        {
            this.sourceString = sourceString;
        }

        public override async Task<string> GetResponse(string direction)
        {
            string url = string.Format("https://dictionary.yandex.net/api/v1/dicservice.json/lookup?key={0}&lang={2}&text={1}", AuthKey, sourceString, direction);
            return await GetJsonResponse(url);
        }

        public override TranslateResultView Parse(string responseText)
        {
            TranslateResultView result;
            YandexDictionaryScheme deserializedResponse = JsonConvert.DeserializeObject<YandexDictionaryScheme>(responseText);
            if (null != deserializedResponse)
                result = convertResponseToTranslateResult(deserializedResponse);
            else
                result = new TranslateResultView();
            return result;
        }

        private TranslateResultView convertResponseToTranslateResult(YandexDictionaryScheme deserializedObject)
        {
            string originalString = deserializedObject.Def.Count > 0 ? deserializedObject.Def[0].Text : string.Empty;
            TranslateResultView result = new TranslateResultView();
            foreach (var def in deserializedObject.Def)
            {
                var translateVariantsSource = def.Tr;
                var translateVariants = new List<ResultLineData>();
                foreach (var tr in translateVariantsSource)
                {
                    translateVariants.Add(new ResultLineData(tr.Text, DefinitionTypesManager.GetEnumDefinitionTypeFromName(tr.Pos)));
                }
                result.AddDefinition(def.Text, DefinitionTypesManager.GetEnumDefinitionTypeFromName(def.Pos), def.Ts, translateVariants);
            }
            return result;
        }

        /*private static async Task<string> GetJsonResponse(string url)
        {
            string result = string.Empty;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.Method = "GET";
            //request.Timeout = 20000;

            using (WebResponse response = await request.GetResponseAsync())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        result = reader.ReadToEnd();
                    }
                }
            }

            return result;
        }*/
    }
}