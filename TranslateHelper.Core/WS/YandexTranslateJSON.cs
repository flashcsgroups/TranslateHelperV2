using System;
using System.IO;
using System.Json;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TranslateHelper.Core.BL.Contracts;

namespace TranslateHelper.Core.WS
{
    public class YandexTranslateJSON : TranslateRequestFactory
    {
        private string AuthKey = "trnsl.1.1.20150918T114904Z.45ab265b9b9ac49d.d4de7a7a003321c5af46dc22110483b086b8125f";
        public override async Task<string> GetResponse(string sourceString, string direction)
        {
            string url = string.Format("https://translate.yandex.net/api/v1.5/tr.json/translate?key={0}&text={1}&lang={2}&format=plain", AuthKey, sourceString, direction);
            string responseString = await GetJsonResponse(url);
            return responseString;
        }

        public override string ParseResponse(string responseText)
        {
            var jsonResponse = JsonValue.Parse(responseText);
            return jsonResponse["text"].ToString();
        }

        private static async Task<string> GetJsonResponse(string url)
        {
            string result = string.Empty;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.Method = "GET";
            request.Timeout = 20000;

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
        }
    }
}