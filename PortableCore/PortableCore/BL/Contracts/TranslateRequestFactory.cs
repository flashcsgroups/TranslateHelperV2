using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace PortableCore.BL.Contracts
{
    public abstract class TranslateRequestFactory
    {
        public abstract Task<string> GetResponse(string direction);//ToDo:direction перенести в свойства и устанавливать через отдельный метод
        //public abstract TranslateResultCollection ParseResponse(string responseText);//ToDo: old, delete it!
        public abstract TranslateResultView Parse(string responseText);//actual
        public abstract void SetSourceString(string sourceString);

        public async Task<string> GetJsonResponse(string url)
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
        }

    }
}