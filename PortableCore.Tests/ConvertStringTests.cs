using System;
using NUnit.Framework;
using PortableCore.WS;
using PortableCore.Helpers;

namespace PortableCore.Tests
{
	[TestFixture]
    public class ConvertStringTests
    {
        [Test]
        public void TestMust_ConvertStringToOneLowerLineWithTrim()
        {
            //arrange
            //именно с переводом строки
            string sourceText = @" Проверка ввода
текста ";

            //act
            string result = ConvertStrings.StringToOneLowerLineWithTrim(sourceText);

            //assert
            Assert.AreEqual(result, "проверка ввода текста");
        }
    }
}