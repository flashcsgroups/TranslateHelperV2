namespace TranslateHelper.Droid
{
    public interface ITestSelectWordsView
    {
        void SetOriginalWord(string originalWord);
        void SetCheckResult(bool success);
        void SetVariants(string variantText);
    }
}