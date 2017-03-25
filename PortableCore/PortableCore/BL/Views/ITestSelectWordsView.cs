using System.Collections.Generic;
using PortableCore.BL.Models;

namespace PortableCore.BL.Views
{
    public interface ITestSelectWordsView
    {
        void DrawNewVariant(TestWordItem originalWord, List<TestWordItem> variants);
        void SetButtonErrorState();
        void SetFinalTest(int countOfTotalWords, int countOfRightWords);
    }
}