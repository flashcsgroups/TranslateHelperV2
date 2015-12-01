using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TranslateHelper.Core.WS;
using System.Resources;
using System.Reflection;

namespace TranslateHelper.Core.Tests
{
    [TestClass]
    public class ParseResponseTests
    {
        [TestMethod]
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

        [TestMethod]
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
        }

        [TestMethod]
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
    }
}
