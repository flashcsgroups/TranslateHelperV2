using PortableCore.BL.Models;
using PortableCore.BL.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableCore.Tests.Mocks
{
    class MockTestSelectWordsActivity : ITestSelectWordsView
    {
        public TestWordItem OriginalWord = new TestWordItem();
        public List<TestWordItem> Variants = new List<TestWordItem>();
        public bool CheckResult = true;
        public int CountOfTestedWords = 0;

        public void SetButtonErrorState()
        {
            CheckResult = false;
        }

        public void SetOriginalWord(TestWordItem originalWord)
        {
            OriginalWord = originalWord;
        }

        public void SetFinalTest(int countOfTotalWords, int countOfRightWords)
        {
            CountOfTestedWords = countOfTotalWords;
        }

        public void SetButtonNormalState()
        {
            throw new NotImplementedException();
        }

        public void DrawNewVariant(TestWordItem originalWord, List<TestWordItem> variants)
        {
            this.OriginalWord = originalWord;
            this.Variants = variants;
        }
    }
}
