using System;
using NUnit.Framework;
using PortableCore.WS;

namespace PortableCore.Tests
{
    [TestFixture]
    public class LocalDBCache
    {
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
    }
}