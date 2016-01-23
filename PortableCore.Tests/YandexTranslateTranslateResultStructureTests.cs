using System;
using NUnit.Framework;
using PortableCore.WS;
using PortableCore.BL.Contracts;

namespace PortableCore.Tests
{
    [TestFixture]
    public class YandexTranslateTranslateResultStructureTests
    {
        [Test]
        public void TestMust_GetTranslatedText()
        {
            //arrange
            string responseText = JSONTexts.YandexTranslateResponse;

            //act
            TranslateResultView testResult = GetTestTranslateResult<YandexTranslateJSON>("this positive test", responseText);

            //assert
            Assert.IsTrue(testResult.Definitions[0].OriginalText == "this positive test");
            Assert.IsTrue(testResult.Definitions[0].TranslateVariants[0].Text == "это положительный тест");
        }

        [Test]
        public void TestMust_GetDefinitionTypeSourceString_Translater()
        {
            //arrange
            string responseText = JSONTexts.YandexTranslateResponse;

            //act
            TranslateResultView testResult = GetTestTranslateResult<YandexTranslateJSON>("this positive test", responseText);

            //assert
            Assert.IsTrue(testResult.Definitions[0].Pos == DefinitionTypesEnum.translater);
        }

        [Test]
        public void TestMust_GetDefinitionTypeTranslatedString_Translater()
        {
            //arrange
            string responseText = JSONTexts.YandexTranslateResponse;

            //act
            TranslateResultView testResult = GetTestTranslateResult<YandexTranslateJSON>("this positive test", responseText);

            //assert
            Assert.IsTrue(testResult.Definitions[0].TranslateVariants[0].Pos == DefinitionTypesEnum.translater);
        }

        private TranslateResultView GetTestTranslateResult<T>(string OriginalText, string StringForParse) where T : TranslateRequestFactory, new()
        {
            var translater = new T();
            translater.SetSourceString(OriginalText);
            return translater.Parse(StringForParse);
        }

    }
}