using System.Collections.Generic;
using PortableCore.BL.Models;

namespace PortableCore.BL.Views
{
    public interface ITestSelectWordsView
    {
        void SetOriginalWord(TestWordItem originalWord);
        void SetButtonErrorState();
        void SetVariants(List<TestWordItem> variants);
        void SetFinalTest(int countOfTestedWords);
        void SetButtonNormalState();
    }
}