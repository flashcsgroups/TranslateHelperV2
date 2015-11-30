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
    public class YandexDictionaryJSON : TranslateRequestFactory
    {
        private string AuthKey = "dict.1.1.20151130T123842Z.c9e307b1d977e039.89e55f949ba679f1f024feba1f6033701acf58bd";
        public override async Task<string> GetResponse(string sourceString, string direction)
        {
            string url = string.Format("https://dictionary.yandex.net/api/v1/dicservice.json/lookup?key={0}&lang={2}&text={1}", AuthKey, sourceString, direction);
            string responseString = await GetJsonResponse(url);
            return responseString;
        }

        public override string ParseResponse(string responseText)
        {
            string translatedString = string.Empty;
            var jsonResponse = JsonValue.Parse(responseText);
            JsonValue def = jsonResponse["def"];
            if (def.Count > 0)
            {
                var trcollection = def[0]["tr"];
                //translatedString = trcollection[0]["text"].ToString();
                foreach (JsonValue item in trcollection)
                {
                    translatedString += item["text"].ToString() + ",";
                }
                if(trcollection[0].ContainsKey("syn"))
                {
                    foreach (JsonValue item in trcollection[0]["syn"])
                    {
                        translatedString += item["text"].ToString() + ",";
                    }
                }
                /*if (trcollection[0]["syn"].Count > 0)
                {
                    foreach (JsonValue item in trcollection[0]["syn"])
                    {
                        translatedString += item["text"].ToString() + ",";
                    }
                }*/

            }
            
            return translatedString;
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