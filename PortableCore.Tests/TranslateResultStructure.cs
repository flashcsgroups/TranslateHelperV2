using System;
using NUnit.Framework;
using PortableCore.WS;
using PortableCore.BL.Contracts;

namespace PortableCore.Tests
{
    [TestFixture]
    public class TranslateResultStructure
    {
        [Test]
        public void TestMust_GetTwoVariantsTranslate()
        {
            //arrange
            //string responseText = JSONTexts.YandexDictionaryResponseForWord_Grow;

            //act
            TranslateResult testResult = new TranslateResult();

            //YandexDictionaryJSON dict = new YandexDictionaryJSON();
            //var result = dict.ParseResponse(responseText);

            //assert
            //Assert.AreEqual(result.Collection.Count, 9);
        }
    }
}