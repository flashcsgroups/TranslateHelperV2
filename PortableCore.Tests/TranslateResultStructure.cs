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
        public void TestMust_GetOriginalText()
        {
            //arrange
            TranslateResult testResult = new TranslateResult();

            //act


            //assert
            Assert.IsTrue(testResult.OriginalText == "explicit");
        }

        [Test]
        public void TestMust_GetTwoVariantsTranslate()
        {
            //arrange
            TranslateResult testResult = new TranslateResult();

            //act


            //assert
            Assert.AreEqual(testResult.Definitions.Count, 2);
        }

        [Test]
        public void TestMust_GetAdjectivePos()
        {
            //arrange
            TranslateResult testResult = new TranslateResult();

            //act


            //assert
            Assert.AreEqual(testResult.Definitions[0].Pos, DefinitionTypesEnum.adjective);
        }

        [Test]
        public void TestMust_GetTransription()
        {
            //arrange
            TranslateResult testResult = new TranslateResult();

            //act


            //assert
            Assert.IsTrue(testResult.Definitions[0].Transcription == "ɪksˈplɪsɪt");
        }

        [Test]
        public void TestMust_GetThreeVariantsOfTranslation()
        {
            //arrange
            TranslateResult testResult = new TranslateResult();

            //act


            //assert
            Assert.AreEqual(testResult.Definitions[0].TranslateVariants.Count, 3);
        }

        [Test]
        public void TestMust_GetTranslatedTextForTranslateVariant()
        {
            //arrange
            TranslateResult testResult = new TranslateResult();

            //act


            //assert
            Assert.IsTrue(testResult.Definitions[0].TranslateVariants[0].Text == "явный");
        }

        [Test]
        public void TestMust_GetPosForTranslateVariant()
        {
            //arrange
            TranslateResult testResult = new TranslateResult();

            //act


            //assert
            Assert.AreEqual(testResult.Definitions[0].TranslateVariants[0].Pos, DefinitionTypesEnum.adjective);
        }
    }
}