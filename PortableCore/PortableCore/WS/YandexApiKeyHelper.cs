using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableCore.WS
{
    public class YandexApiKeyHelper
    {
        List<string> translateApiKeys = new List<string>()
        {
            //thelper1
            "trnsl.1.1.20170211T171044Z.6037854c97679088.7608d1002ba3f30fa85c5bc4573b7ae626c70762",
            //thelper2
            "trnsl.1.1.20170211T180956Z.24bae87f76dfef14.6f6ecf56fd1018d0d72ca3731a84283894c411e2",
            //gromozeka07b9
            "trnsl.1.1.20150918T114904Z.45ab265b9b9ac49d.d4de7a7a003321c5af46dc22110483b086b8125f"
        };
        List<string> dictionaryApiKeys = new List<string>()
        {
            //thelper1
            "dict.1.1.20170211T171138Z.8b25685dbb77017a.d7040a7c5184259e2916b022cde37738c5f1e380",
            //thelper2
            "dict.1.1.20170211T180753Z.5363531532307c28.c8cfa4d54f33b2d176369473451ff0b4f237f43c",
            //gromozeka07b9
            "dict.1.1.20151121T213256Z.ed41c64573f0a13d.96329576b18faa0c58e42b40bd81905b80f6d8a6"
        };
        public string GetTranslateApiKey()
        {
            int indexOfRecord = getRandomNum(translateApiKeys.Count);
            return translateApiKeys[indexOfRecord];
        }

        public string GetDictionaryApiKey()
        {
            int indexOfRecord = getRandomNum(dictionaryApiKeys.Count);
            return dictionaryApiKeys[indexOfRecord];
        }
        private int getRandomNum(int count)
        {
            Random rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            return rnd.Next(0, count);
        }
    }
}
