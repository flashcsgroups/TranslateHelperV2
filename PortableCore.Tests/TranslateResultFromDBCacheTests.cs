using System;
using NUnit.Framework;
using PortableCore.WS;
using PortableCore.BL.Contracts;
using PortableCore.DL;
using System.Collections.Generic;
using PortableCore.DAL;

namespace PortableCore.Tests
{
    [TestFixture]
    public class TranslateResultFromDBCacheTests
    {
        [Test]
        public void TestMust_GetOneTranslatedStringFormDB()
        {
            //arrange
            string sourceString = "explicit";
            //act
            //LocalDBCacheReader dbReader = new LocalDBCacheReader();
            //List<TranslateResult> list = dbReader.GetSourceExprCollection(sourceString).getDefinitions().getTranslatedExpressions().GetTranslateResults();
            //assert
        }

        /*[Test]
        public void TestMust_GetOneDefinitionForSourceText()
        {
            //arrange
            string originalWord = "explicit";
            LocalDBCacheReader cache = new LocalDBCacheReader();
            List<Tuple<SourceExpression, SourceDefinition, TranslatedExpression, Favorites>> listOfCoupleTranslateAndFavorites;
            SourceExpression sourceItem1;
            listOfCoupleTranslateAndFavorites = new List<Tuple<SourceExpression, SourceDefinition, TranslatedExpression, Favorites>>();
            InitItem1(originalWord, listOfCoupleTranslateAndFavorites, out sourceItem1);
        
            //act
            TranslateResult result = cache.ConvertDataLocalCacheToTranslateResult(originalWord, listOfCoupleTranslateAndFavorites);

            //assert
            Assert.AreEqual(result.Definitions.Count, 1);
        }

        [Test]
        public void TestMust_GetTwoDefinitionsForSourceText()
        {
            //arrange
            string originalWord = "explicit";
            LocalDBCacheReader cache = new LocalDBCacheReader();
            List<Tuple<SourceExpression, SourceDefinition, TranslatedExpression, Favorites>> listOfCoupleTranslateAndFavorites;
            SourceExpression sourceItem1;
            listOfCoupleTranslateAndFavorites = new List<Tuple<SourceExpression, SourceDefinition, TranslatedExpression, Favorites>>();
            InitItem1(originalWord, listOfCoupleTranslateAndFavorites, out sourceItem1);
            InitItem2(listOfCoupleTranslateAndFavorites, sourceItem1);

            //act
            TranslateResult result = cache.ConvertDataLocalCacheToTranslateResult(originalWord, listOfCoupleTranslateAndFavorites);

            //assert
            Assert.AreEqual(result.Definitions.Count, 2);
        }

        [Test]
        public void TestMust_GetSourceText()
        {
            //arrange
            string originalWord = "explicit";
            LocalDBCacheReader cache = new LocalDBCacheReader();
            List<Tuple<SourceExpression, SourceDefinition, TranslatedExpression, Favorites>> listOfCoupleTranslateAndFavorites;
            SourceExpression sourceItem1;
            listOfCoupleTranslateAndFavorites = new List<Tuple<SourceExpression, SourceDefinition, TranslatedExpression, Favorites>>();
            InitItem1(originalWord, listOfCoupleTranslateAndFavorites, out sourceItem1);
            InitItem2(listOfCoupleTranslateAndFavorites, sourceItem1);

            //act
            TranslateResult result = cache.ConvertDataLocalCacheToTranslateResult(originalWord, listOfCoupleTranslateAndFavorites);

            //assert
            Assert.IsTrue(result.OriginalText == originalWord);
        }

        private static void InitItem1(string originalWord, List<Tuple<SourceExpression, SourceDefinition, TranslatedExpression, Favorites>> listOfCoupleTranslateAndFavorites, out SourceExpression sourceItem1)
        {
        
            sourceItem1 = new SourceExpression() { ID = 11, DirectionID = 0, Text = originalWord };
            var defItem1 = new SourceDefinition() { DefinitionTypeID = (int)DefinitionTypesEnum.adjective, ID = 22, SourceExpressionID = sourceItem1.ID, TranscriptionText = "ɪksˈplɪsɪt" };
            var trItem1 = new TranslatedExpression() { DefinitionTypeID = (int)DefinitionTypesEnum.adjective, ID = 3, DefinitionID = defItem1.ID, TranslatedText = "явный" };
            var favItem1 = new Favorites() { ID = 0, TranslatedExpressionID = trItem1.ID };
            listOfCoupleTranslateAndFavorites.Add(new Tuple<SourceExpression, SourceDefinition, TranslatedExpression, Favorites>(sourceItem1, defItem1, trItem1, favItem1));
        }

        private static void InitItem2(List<Tuple<SourceExpression, SourceDefinition, TranslatedExpression, Favorites>> listOfCoupleTranslateAndFavorites, SourceExpression sourceItem1)
        {
            var defItem2 = new SourceDefinition() { DefinitionTypeID = (int)DefinitionTypesEnum.adjective, ID = 23, SourceExpressionID = sourceItem1.ID, TranscriptionText = "ɪksˈplɪsɪt" };
            var trItem2 = new TranslatedExpression() { DefinitionTypeID = (int)DefinitionTypesEnum.adjective, ID = 4, DefinitionID = defItem2.ID, TranslatedText = "открытый" };
            var favItem2 = new Favorites() { ID = 1, TranslatedExpressionID = trItem2.ID };
            listOfCoupleTranslateAndFavorites.Add(new Tuple<SourceExpression, SourceDefinition, TranslatedExpression, Favorites>(sourceItem1, defItem2, trItem2, favItem2));
        }*/
    }
}