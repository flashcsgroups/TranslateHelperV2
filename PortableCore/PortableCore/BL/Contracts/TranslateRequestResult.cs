using System.Net;
using PortableCore.BL.Contracts;

namespace PortableCore.BL.Contracts
{
	public class TranslateRequestResult
	{
        public string errorDescription;//ToDo:переделать на приватное
        public TranslateResultView TranslatedData { get; private set; }

        public string OriginalText { get; private set; }

        public TranslateRequestResult(string originalText)
        {
            this.OriginalText = originalText;
        }

        public void SetTranslateResult(TranslateResultView translatedData)
        {
            TranslatedData = translatedData;
        }
    }
}

