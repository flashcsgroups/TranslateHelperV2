using NUnit.Framework;
using PortableCore.BL.Contracts;
using System.Collections.Generic;
using PortableCore.DL;
using PortableCore.BL;
using System;
using PortableCore.BL.Managers;

namespace PortableCore.Tests
{
    [TestFixture]
    public class DetectInputLanguageTests
    {
        [TestCase("Test")]
        [TestCase("grow")]
        [TestCase("j")]
        public void TestMust_DetectEnglish(string inputString)
        {
            //arrange

            //act
            DetectInputLanguage detect = new DetectInputLanguage(inputString);
            DetectInputLanguage.Language result = detect.Detect();

            //assert
            Assert.IsTrue(result == DetectInputLanguage.Language.English);
        }

        [TestCase(" ")]
        [TestCase("")]
        public void TestMust_DetectUnknown(string inputString)
        {
            //arrange

            //act
            DetectInputLanguage detect = new DetectInputLanguage(inputString);
            DetectInputLanguage.Language result = detect.Detect();

            //assert
            Assert.IsTrue(result == DetectInputLanguage.Language.Unknown);
        }

        [TestCase("Тест")]
        public void TestMust_DetectRussian(string inputString)
        {
            //arrange

            //act
            DetectInputLanguage detect = new DetectInputLanguage(inputString);
            DetectInputLanguage.Language result = detect.Detect();

            //assert
            Assert.IsTrue(result == DetectInputLanguage.Language.Russian);
        }

    }
}