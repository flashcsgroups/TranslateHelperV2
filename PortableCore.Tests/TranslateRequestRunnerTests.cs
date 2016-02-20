﻿using NUnit.Framework;
using PortableCore.BL.Contracts;
using System.Collections.Generic;
using PortableCore.DL;
using PortableCore.BL;
using PortableCore.Helpers;
using System.Threading.Tasks;
using System.Linq;

namespace PortableCore.Tests
{
    [TestFixture]
    public class TranslateRequestRunnerTests
    {
        [Test]
        public void TestMust_CreateResultOfSuccessRequest_DictionaryService()
        {
            //arrange
            string testSourceText = "test";
            string testTranscription = "test";
            string testTranslatedText = "тест";
            DefinitionTypesEnum testDefinition = DefinitionTypesEnum.noun;
            List<ResultLineData> variants = new List<ResultLineData>();
            variants.Add(new ResultLineData(testTranslatedText, testDefinition));
            List<TranslateResultDefinition> defs = new List<TranslateResultDefinition>();
            defs.Add(new TranslateResultDefinition(testSourceText, testDefinition, testTranscription, variants));
            IRequestTranslateString translaterDictSrv = new testService(defs, string.Empty);
            IRequestTranslateString localCacheSrv = new testService(new List<TranslateResultDefinition>(), string.Empty);
            IRequestTranslateString translaterTranslateSrv = new testTranslateService();
            SQLiteTest sqliteTestInstance = new SQLiteTest();

            //act
            TranslateRequestRunner runner = new TranslateRequestRunner(sqliteTestInstance, localCacheSrv, translaterDictSrv, translaterTranslateSrv);
            var result = runner.GetDictionaryResult(testSourceText, new BL.TranslateDirection(sqliteTestInstance));

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
        public void TestMust_CreateResultOfSuccessRequest_ReadLocalCache()
        {
            //arrange
            string testSourceText = "test";
            string testTranscription = "test";
            string testTranslatedText = "тест";
            DefinitionTypesEnum testDefinition = DefinitionTypesEnum.noun;
            List<ResultLineData> variants = new List<ResultLineData>();
            variants.Add(new ResultLineData(testTranslatedText, testDefinition));
            List<TranslateResultDefinition> defs = new List<TranslateResultDefinition>();
            defs.Add(new TranslateResultDefinition(testSourceText, testDefinition, testTranscription, variants));
            IRequestTranslateString translaterDictSrv = new testService(new List<TranslateResultDefinition>(), string.Empty);
            IRequestTranslateString localCacheSrv = new testService(defs, string.Empty);
            IRequestTranslateString translaterTranslateSrv = new testTranslateService();
            SQLiteTest sqliteTestInstance = new SQLiteTest();

            //act
            TranslateRequestRunner runner = new TranslateRequestRunner(sqliteTestInstance, localCacheSrv, translaterDictSrv, translaterTranslateSrv);
            var result = runner.GetDictionaryResult(testSourceText, new BL.TranslateDirection(sqliteTestInstance));

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
            SQLiteTest sqliteTestInstance = new SQLiteTest();

            string testSourceText = "test";
            string testErrorText = "error";
            List<TranslateResultDefinition> defs = new List<TranslateResultDefinition>();
            IRequestTranslateString translaterDictSrv = new testService(defs, testErrorText);
            IRequestTranslateString translaterTranslateSrv = new testTranslateService();
            IRequestTranslateString localCacheSrv = new testService(defs, testErrorText);

            //act
            TranslateRequestRunner runner = new TranslateRequestRunner(sqliteTestInstance, localCacheSrv, translaterDictSrv, translaterTranslateSrv);

            string error = string.Empty;
            var result = runner.GetDictionaryResult(testSourceText, new BL.TranslateDirection(sqliteTestInstance));
            error = result.Exception.InnerException.Message;

            //assert
            Assert.IsTrue(error == "Ошибка подключения к интернет:error");
        }

        public class SQLiteTest : ISQLiteTesting
        {
            public IEnumerable<Direction> Table<Direction>() where Direction : IBusinessEntity, new()
            {
                List<Direction> listDirection = new List<Direction>();
                var item = new Direction();
                //item.Name = "en-ru";
                item.ID = 1;
                //item.ProviderID = 11;
                listDirection.Add(item);
                return listDirection;
            }
        }

        public class testService : IRequestTranslateString
        {
            private List<TranslateResultDefinition> definitions;
            private string errorText = string.Empty;

            public testService(List<TranslateResultDefinition> definitions, string errorText)
            {
                this.definitions = definitions;
                this.errorText = errorText;
            }

            public async Task<TranslateRequestResult> Translate(string sourceString)
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
            public async Task<TranslateRequestResult> Translate(string sourceString)
            {
                TranslateRequestResult result = new TranslateRequestResult(sourceString);
                return result;
            }
        }
    }
}
