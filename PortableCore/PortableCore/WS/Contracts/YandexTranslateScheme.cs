using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableCore.WS.Contracts
{
    //класс автоматически построен с помощью утилиты JSON to C#
    public class YandexTranslateScheme
    {
        //тип результата запроса в соответствии с кодами http
        [JsonProperty("code")]
        public int Code { get; set; }
        //текстовое описание направления перевода
        [JsonProperty("lang")]
        public string Lang { get; set; }
        //переведенный текст
        [JsonProperty("text")]
        public List<string> Text { get; set; }
    }
}
