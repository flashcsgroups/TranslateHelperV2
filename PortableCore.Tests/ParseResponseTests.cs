using System;
using NUnit.Framework;
using PortableCore.WS;

namespace PortableCore.Tests
{
    [TestFixture]
    public class ParseResponseTests
    {
        /*[Test]
        public void TestMust_CheckCountResultLinesForWord_Grow()
        {
            //arrange
            string responseText = JSONTexts.YandexDictionaryResponseForWord_Grow;

            //act
            YandexDictionaryJSON dict = new YandexDictionaryJSON();
            var result = dict.ParseResponse(responseText);

            //assert
            Assert.AreEqual(result.Collection.Count, 9);
        }

        [Test]
        public void TestMust_CheckSomeResultLinesForWord_Grow()
        {
            //arrange
            string responseText = JSONTexts.YandexDictionaryResponseForWord_Grow;

            //act
            YandexDictionaryJSON dict = new YandexDictionaryJSON();
            var result = dict.ParseResponse(responseText);

            //assert
            Assert.AreEqual(result.Collection[0].TranslatedText, "расти");
            Assert.AreEqual(result.Collection[0].Pos, "verb");
            Assert.AreEqual(result.Collection[0].Ts, "grəʊ");
            Assert.AreEqual(result.Collection[0].SynonymsCollection.Count, 7);

            Assert.AreEqual(result.Collection[5].TranslatedText, "разрастаться");
            Assert.AreEqual(result.Collection[5].Pos, "verb");
            Assert.AreEqual(result.Collection[5].Ts, "grəʊ");
            Assert.AreEqual(result.Collection[5].SynonymsCollection.Count, 0);
        }*/

        [Test]
        public void TestMust_CheckCountResultLinesForIncorrectWord()
        {
            //arrange
            string responseText = JSONTexts.YandexDictionaryResponseForIncorrectWord;

            //act
            YandexDictionaryJSON dict = new YandexDictionaryJSON();
            var result = dict.ParseResponse(responseText);

            //assert
            Assert.AreEqual(result.Collection.Count, 0);
        }

        [Test]
        public void TestMust_testjson()
        {
            //arrange
            string responseText = "{'head':{},'def':[]}";

            //act
            YandexDictionaryJSON dict = new YandexDictionaryJSON();
            var result = dict.ParseResponse(responseText);

            //assert
            Assert.AreEqual(result.Collection.Count, 0);
        }
    }
}