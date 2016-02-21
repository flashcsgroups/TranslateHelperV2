using System.Collections.Generic;

namespace PortableCore.BL
{
    public interface ITestSelectWordsView
    {
        void SetOriginalWord(string originalWord);
        void SetCheckResult(bool success);
        void SetVariants(List<string> variants);
        void SetFinalTest(int countOfTestedWords);
    }
}