using NUnit.Framework;
using PortableCore.BL.Contracts;
using PortableCore.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableCore.Tests
{
    [TestFixture]
    public class TranslateRequestRunnerTests
    {
        [Test]
        public void TestMust_CreateResultOfSuccessRequest_DictionaryService()
        {
            //arrange
            IRequestTranslateString translaterTranslateSrv = new testTranslateService();

            string testSourceText = "test";
            string testTranscription = "test";
            string testTranslatedText = "тест";
            DefinitionTypesEnum testDefinition = DefinitionTypesEnum.noun;
            List<TranslateResultVariant> variants = new List<TranslateResultVariant>();
            variants.Add(new TranslateResultVariant(testTranslatedText, testDefinition));

            List<TranslateResultDefinition> defs = new List<TranslateResultDefinition>();
            defs.Add(new TranslateResultDefinition(testSourceText, testDefinition, testTranscription, variants));

            //act
            IRequestTranslateString translaterDictSrv = new testDictService(defs, string.Empty);
            TranslateRequestRunner runner = new TranslateRequestRunner(translaterDictSrv, translaterTranslateSrv);
            var result = runner.GetDictionaryResult(testSourceText, "en-ru");

            //assert
            Assert.AreEqual(result.Result.TranslatedData.Definitions.Count, 1);
            Assert.IsTrue(result.Result.TranslatedData.Definitions[0].OriginalText == testSourceText);
            Assert.IsTrue(result.Result.TranslatedData.Definitions[0].Pos == testDefinition);
            Assert.IsTrue(result.Result.TranslatedData.Definitions[0].Transcription == testTranscription);
            Assert.AreEqual(result.Result.TranslatedData.Definitions[0].TranslateVariants.Count, 1);
            Assert.IsTrue(result.Result.TranslatedData.Definitions[0].TranslateVariants[0].Text == testTranslatedText);
            Assert.IsTrue(result.Result.TranslatedData.Definitions[0].TranslateVariants[0].Pos == testDefinition);
        }

        [Test]
        public void TestMust_CreateResultOfErrorRequest_DictionaryService()
        {
            //arrange
            IRequestTranslateString translaterTranslateSrv = new testTranslateService();

            string testSourceText = "test";
            string testErrorText = "error";
            List<TranslateResultDefinition> defs = new List<TranslateResultDefinition>();

            //act
            IRequestTranslateString translaterDictSrv = new testDictService(defs, testErrorText);
            TranslateRequestRunner runner = new TranslateRequestRunner(translaterDictSrv, translaterTranslateSrv);

            string error = string.Empty;
            var result = runner.GetDictionaryResult(testSourceText, "en-ru");
            error = result.Exception.InnerException.Message;

            //assert
            Assert.IsTrue(error == "Ошибка подключения к интернет:error");
        }
        public class testDictService : IRequestTranslateString
        {
            private List<TranslateResultDefinition> definitions;
            private string errorText = string.Empty;

            public testDictService(List<TranslateResultDefinition> definitions, string errorText)
            {
                this.definitions = definitions;
                this.errorText = errorText;
            }

            public async Task<TranslateRequestResult> Translate(string sourceString, string direction)
            {
                TranslateRequestResult result = new TranslateRequestResult(sourceString);
                result.errorDescription = errorText;
                TranslateResultView view = new TranslateResultView();
                foreach(var item in definitions)
                {
                    view.AddDefinition(item.OriginalText, item.Pos, item.Transcription, item.TranslateVariants);
                }
                result.SetTranslateResult(view);
                return result;
            }
        }

        private class testTranslateService : IRequestTranslateString
        {
            public async Task<TranslateRequestResult> Translate(string sourceString, string direction)
            {
                TranslateRequestResult result = new TranslateRequestResult(sourceString);
                return result;
            }
        }
    }
}
