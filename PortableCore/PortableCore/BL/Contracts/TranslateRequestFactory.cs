using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace PortableCore.BL.Contracts
{
    public abstract class TranslateRequestFactory
    {
        public abstract Task<string> GetResponse(string direction);//ToDo:direction перенести в свойства и устанавливать через отдельный метод
        public abstract TranslateResultView Parse(string responseText);//actual
        public abstract void SetSourceString(string sourceString);

        public async Task<string> GetJsonResponse(string url)
        {
            string result = string.Empty;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.Method = "GET";

            using (WebResponse response = await request.GetResponseAsync())
            {
                var webresponse = (HttpWebResponse)response;
                //if (webresponse.StatusCode != HttpStatusCode.OK) throw new Exception(webresponse.StatusCode.ToString());
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