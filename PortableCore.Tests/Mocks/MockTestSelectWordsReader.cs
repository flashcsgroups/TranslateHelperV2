using Ploeh.AutoFixture;
using PortableCore.BL;
using PortableCore.BL.Models;
using PortableCore.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableCore.Tests.Mocks
{
    class MockTestSelectWordsReader : ITestSelectWordsReader
    {
        Fixture fixture = new Fixture();
        public int GetCountDifferenceSources(TranslateDirection direction)
        {
            return 10;
        }

        public List<TestWordItem> GetIncorrectVariants(int countOfIncorrectWords, int chatId, int languageFromId, string correctWord)
        {
            List<TestWordItem> result = new List<TestWordItem>();
            for (int i = 0; i < countOfIncorrectWords; i++)
            {
                result.Add(fixture.Create<TestWordItem>());
            }
            return result;
        }

        public Tuple<string, string> GetNextWord(int translatedExpressionID)
        {
            return fixture.Create<Tuple<string, string>>();
        }

        public RandomWordsList GetRandomFavorites(int countOfWords, int chatId)
        {
            int languageFromId = 1;

            List <TestWordItem> listObj = new List<TestWordItem>();
            for(int x=0;x< countOfWords; x++)
            {
                listObj.Add(fixture.Create<TestWordItem>());
            }
            return new RandomWordsList() { languageFromId = languageFromId, WordsList = listObj };
        }
    }
}
