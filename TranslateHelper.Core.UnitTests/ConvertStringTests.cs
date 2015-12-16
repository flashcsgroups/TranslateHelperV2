using System;
using NUnit.Framework;
using TranslateHelper.Core.WS;
using TranslateHelper.Core.Helpers;

namespace TranslateHelper.Core.UnitTests
{
    [TestFixture]
    public class ConvertStringTests
    {
        [Test]
        public void TestMust_ConvertStringToOneLowerLineWithTrim()
        {
            //arrange
            string sourceText = @" Проверка ввода
текста ";

            //act
            string result = ConvertStrings.StringToOneLowerLineWithTrim(sourceText);

            //assert
            Assert.AreEqual(result, "проверка ввода текста");
        }
    }
}