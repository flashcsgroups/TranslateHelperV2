using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortableCore.DL;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace PortableCore.WS
{
    public class ApiRequest : IApiClient
    {
        private string _hostUrl = string.Empty;
        public ApiRequest(string hostUrl)
        {
            _hostUrl = hostUrl;
        }
        public async Task<List<int>> GetChangedIDsFromServer(string tableName, DateTime localMaxTimeStamp)
        {
            List<int> deserializedValue = new List<int>();
            try
            {
                var response = await GetJsonResponse($"{this._hostUrl}/idiom/changes?clientMaxDate={localMaxTimeStamp.ToString("yyyy-MM-ddTHH:mm:ss")}");
                deserializedValue = JsonConvert.DeserializeObject<List<int>>(response);
            }
            catch (Exception)
            {

            }

            return deserializedValue;
        }

        public async Task<List<Idiom>> GetIdiomsFromServer(List<int> iDs)
        {
            StringBuilder idFilter = new StringBuilder();
            foreach(int id in iDs)
            {
                idFilter.Append($"Id={id}&");
            }
            List<Idiom> deserializedValue = new List<Idiom>();
            try
            {
                var response = await GetJsonResponse($"{this._hostUrl}/idiom?{idFilter.ToString()}");
                deserializedValue = JsonConvert.DeserializeObject<List<Idiom>>(response);
            }
            catch (Exception)
            {

            }
            return deserializedValue;
        }

        private async Task<string> GetJsonResponse(string url)
        {
            string result = string.Empty;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.Method = "GET";

            using (WebResponse response = await request.GetResponseAsync())
            {
                var webresponse = (HttpWebResponse)response;
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
