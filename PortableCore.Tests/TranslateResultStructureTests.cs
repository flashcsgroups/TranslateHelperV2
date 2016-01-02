using System;
using NUnit.Framework;
using PortableCore.WS;
using PortableCore.BL.Contracts;

namespace PortableCore.Tests
{
    [TestFixture]
    public class TranslateResultStructureTests
    {
        [Test]
        public void TestMust_GetOriginalText()
        {
            //arrange
            string responseText = JSONTexts.YandexDictionaryResponseForWord_Explicit;

            //act
            TranslateResult testResult = GetTestTranslateResult<YandexDictionaryJSON>(responseText);

            //assert
            Assert.IsTrue(testResult.OriginalText == "explicit");
        }

        [Test]
        public void TestMust_GetTwoVariantsTranslate()
        {
            //arrange
            string responseText = JSONTexts.YandexDictionaryResponseForWord_Explicit;

            //act
            TranslateResult testResult = GetTestTranslateResult<YandexDictionaryJSON>(responseText);

            //assert
            Assert.AreEqual(testResult.Definitions.Count, 2);
        }

        [Test]
        public void TestMust_GetAdjectivePos()
        {
            //arrange
            string responseText = JSONTexts.YandexDictionaryResponseForWord_Explicit;

            //act
            TranslateResult testResult = GetTestTranslateResult<YandexDictionaryJSON>(responseText);

            //assert
            Assert.AreEqual(testResult.Definitions[0].Pos, DefinitionTypesEnum.adjective);
        }

        [Test]
        public void TestMust_GetTranscription()
        {
            //arrange
            string responseText = JSONTexts.YandexDictionaryResponseForWord_Explicit;

            //act
            TranslateResult testResult = GetTestTranslateResult<YandexDictionaryJSON>(responseText);

            //assert
            Assert.IsTrue(testResult.Definitions[0].Transcription == "ɪksˈplɪsɪt");
        }

        [Test]
        public void TestMust_GetThreeVariantsOfTranslation()
        {
            //arrange
            string responseText = JSONTexts.YandexDictionaryResponseForWord_Explicit;

            //act
            TranslateResult testResult = GetTestTranslateResult<YandexDictionaryJSON>(responseText);

            //assert
            Assert.AreEqual(testResult.Definitions[0].TranslateVariants.Count, 3);
        }

        [Test]
        public void TestMust_GetTranslatedTextForTranslateVariant()
        {
            //arrange
            string responseText = JSONTexts.YandexDictionaryResponseForWord_Explicit;

            //act
            TranslateResult testResult = GetTestTranslateResult<YandexDictionaryJSON>(responseText);

            //assert
            Assert.IsTrue(testResult.Definitions[0].TranslateVariants[0].Text == "явный");
        }

        [Test]
        public void TestMust_GetPosForTranslateVariant()
        {
            //arrange
            string responseText = JSONTexts.YandexDictionaryResponseForWord_Explicit;

            //act
            TranslateResult testResult = GetTestTranslateResult<YandexDictionaryJSON>(responseText);

            //assert
            Assert.AreEqual(testResult.Definitions[0].TranslateVariants[0].Pos, DefinitionTypesEnum.adjective);
        }

        [Test]
        public void TestMust_GetResultWithIncorrectResponse()
        {
            //arrange
            string responseText = "";

            //act
            TranslateResult testResult = GetTestTranslateResult<YandexDictionaryJSON>(responseText);

            //assert
            Assert.AreEqual(testResult.OriginalText, "");
        }
        private TranslateResult GetTestTranslateResult<T>(string StringForParse) where T : TranslateRequestFactory, new()
        {
            var translater = new T();
            return translater.Parse(StringForParse);
        }
    }
}