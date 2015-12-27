using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableCore.WS.Contracts
{
    //класс автоматически построен с помощью утилиты JSON to C#
    public class YandexDictionaryScheme : IDictionaryService
    {
        //заголовок обычно пустой
        [JsonProperty("head")]
        private Dictionary<string, string> Head { get; set; }
        //варианты для разных частей речи
        [JsonProperty("def")]
        private List<Def> Def { get; set; }

        //public List<>
    }

    //данные вариантов по типам речи, элементов столько, сколько вариантов - сущ., предлог и т.д.
    public class Def
    {
        //исходное слово
        [JsonProperty("text")]
        public string Text { get; set; }
        //тип речи
        [JsonProperty("pos")]
        public string Pos { get; set; }
        //транскрипция
        [JsonProperty("ts")]
        public string Ts { get; set; }

        [JsonProperty("tr")]
        public List<Tr> Tr { get; set; }
    }

    //перевод
    public class Tr
    {
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("pos")]
        public string Pos { get; set; }
        [JsonProperty("syn")]
        public List<Syn> Syn { get; set; }
        [JsonProperty("mean")]
        public List<Mean> Mean { get; set; }
        [JsonProperty("ex")]
        public List<Ex> Ex { get; set; }
    }

    //синонимы
    public class Syn
    {
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("pos")]
        public string Pos { get; set; }
    }

    //варианты значений
    public class Mean
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }

    //примеры
    public class Ex
    {
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("tr")]
        public List<ExTr> ExTr { get; set; }
    }

    //перевод примера
    public class ExTr
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }

}
