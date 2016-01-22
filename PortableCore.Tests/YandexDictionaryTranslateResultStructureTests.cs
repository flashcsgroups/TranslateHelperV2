using System;
using NUnit.Framework;
using PortableCore.WS;
using PortableCore.BL.Contracts;

namespace PortableCore.Tests
{
    [TestFixture]
    public class YandexDictionaryTranslateResultStructureTests
    {
        [Test]
        public void TestMust_GetOriginalText()
        {
            //arrange
            string responseText = JSONTexts.YandexDictionaryResponseForWord_Explicit;

            //act
            TranslateResultView testResult = GetTestTranslateResult<YandexDictionaryJSON>(responseText);

            //assert
            Assert.IsTrue(testResult.Definitions[0].OriginalText == "explicit");
        }

        [Test]
        public void TestMust_GetTwoVariantsTranslate()
        {
            //arrange
            string responseText = JSONTexts.YandexDictionaryResponseForWord_Explicit;

            //act
            TranslateResultView testResult = GetTestTranslateResult<YandexDictionaryJSON>(responseText);

            //assert
            Assert.AreEqual(testResult.Definitions.Count, 2);
        }

        [Test]
        public void TestMust_GetAdjectivePos()
        {
            //arrange
            string responseText = JSONTexts.YandexDictionaryResponseForWord_Explicit;

            //act
            TranslateResultView testResult = GetTestTranslateResult<YandexDictionaryJSON>(responseText);

            //assert
            Assert.AreEqual(testResult.Definitions[0].Pos, DefinitionTypesEnum.adjective);
        }

        [Test]
        public void TestMust_GetTranscriptionForSource()
        {
            //arrange
            string responseText = JSONTexts.YandexDictionaryResponseForWord_Explicit;

            //act
            TranslateResultView testResult = GetTestTranslateResult<YandexDictionaryJSON>(responseText);

            //assert
            Assert.IsTrue(testResult.Definitions[0].Transcription == "ɪksˈplɪsɪt");
        }

        [Test]
        public void TestMust_GetThreeVariantsOfTranslation()
        {
            //arrange
            string responseText = JSONTexts.YandexDictionaryResponseForWord_Explicit;

            //act
            TranslateResultView testResult = GetTestTranslateResult<YandexDictionaryJSON>(responseText);

            //assert
            Assert.AreEqual(testResult.Definitions[0].TranslateVariants.Count, 3);
        }

        [Test]
        public void TestMust_GetTranslatedTextForTranslateDefinition0_Variant0()
        {
            //arrange
            string responseText = JSONTexts.YandexDictionaryResponseForWord_Explicit;

            //act
            TranslateResultView testResult = GetTestTranslateResult<YandexDictionaryJSON>(responseText);

            //assert
            Assert.IsTrue(testResult.Definitions[0].TranslateVariants[0].Text == "явный");
        }

        [Test]
        public void TestMust_GetTranslatedTextForTranslateDefinition1_Variant0()
        {
            //arrange
            string responseText = JSONTexts.YandexDictionaryResponseForWord_Explicit;

            //act
            TranslateResultView testResult = GetTestTranslateResult<YandexDictionaryJSON>(responseText);

            //assert
            Assert.IsTrue(testResult.Definitions[1].TranslateVariants[0].Text == "открытый");
        }

        [Test]
        public void TestMust_GetPosForTranslateDefinition0_Variant0()
        {
            //arrange
            string responseText = JSONTexts.YandexDictionaryResponseForWord_Explicit;

            //act
            TranslateResultView testResult = GetTestTranslateResult<YandexDictionaryJSON>(responseText);

            //assert
            Assert.IsTrue(testResult.Definitions[0].TranslateVariants[0].Pos == DefinitionTypesEnum.adjective);
        }

        [Test]
        public void TestMust_GetPosForTranslateDefinition1_Variant0()
        {
            //arrange
            string responseText = JSONTexts.YandexDictionaryResponseForWord_Explicit;

            //act
            TranslateResultView testResult = GetTestTranslateResult<YandexDictionaryJSON>(responseText);

            //assert
            Assert.IsTrue(testResult.Definitions[1].TranslateVariants[0].Pos == DefinitionTypesEnum.participle);
        }

        [Test]
        public void TestMust_GetPosForSourceDefinition0()
        {
            //arrange
            string responseText = JSONTexts.YandexDictionaryResponseForWord_Explicit;

            //act
            TranslateResultView testResult = GetTestTranslateResult<YandexDictionaryJSON>(responseText);

            //assert
            Assert.IsTrue(testResult.Definitions[0].Pos == DefinitionTypesEnum.adjective);
        }

        [Test]
        public void TestMust_GetPosForSourceDefinition1()
        {
            //arrange
            string responseText = JSONTexts.YandexDictionaryResponseForWord_Explicit;

            //act
            TranslateResultView testResult = GetTestTranslateResult<YandexDictionaryJSON>(responseText);

            //assert
            Assert.IsTrue(testResult.Definitions[1].Pos == DefinitionTypesEnum.participle);
        }

        [Test]
        public void TestMust_GetPosForTranslateVariant()
        {
            //arrange
            string responseText = JSONTexts.YandexDictionaryResponseForWord_Explicit;

            //act
            TranslateResultView testResult = GetTestTranslateResult<YandexDictionaryJSON>(responseText);

            //assert
            Assert.AreEqual(testResult.Definitions[0].TranslateVariants[0].Pos, DefinitionTypesEnum.adjective);
        }

        [Test]
        public void TestMust_GetTranslateResultForEmptyResponse()
        {
            //arrange
            string responseText = "";

            //act
            TranslateResultView testResult = GetTestTranslateResult<YandexDictionaryJSON>(responseText);

            //assert
            Assert.AreEqual(testResult.Definitions.Count, 0);
        }
        private TranslateResultView GetTestTranslateResult<T>(string StringForParse) where T : TranslateRequestFactory, new()
        {
            var translater = new T();
            return translater.Parse(StringForParse);
        }
    }
}