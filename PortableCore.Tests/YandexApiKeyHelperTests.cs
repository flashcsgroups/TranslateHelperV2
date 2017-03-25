using NUnit.Framework;
using PortableCore.WS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableCore.Tests
{
    [TestFixture]
    public class YandexApiKeyHelperTests
    {
        [Test]
        public void TestMust_GetAllTranslateApiKey()
        {
            int numOfKeys = 3;
            YandexApiKeyHelper helper = new YandexApiKeyHelper();
            List<string> keys = new List<string>();
            for(int i=0;i<=10;i++)
            {
                string apiKey = helper.GetTranslateApiKey();
                //задержка чтобы рандомные значения были разными
                System.Threading.Thread.Sleep(500);
                if (!keys.Any(e=>e.Contains(apiKey))) keys.Add(apiKey);
            }
            Assert.IsTrue(keys.Count == numOfKeys);
        }

        [Test]
        public void TestMust_GetAllDictionaryApiKey()
        {
            int numOfKeys = 3;
            YandexApiKeyHelper helper = new YandexApiKeyHelper();
            List<string> keys = new List<string>();
            for (int i = 0; i <= 10; i++)
            {
                string apiKey = helper.GetDictionaryApiKey();
                //задержка чтобы рандомные значения были разными
                System.Threading.Thread.Sleep(500);
                if (!keys.Any(e => e.Contains(apiKey))) keys.Add(apiKey);
            }
            Assert.IsTrue(keys.Count == numOfKeys);
        }
    }
}
