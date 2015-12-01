using System.Net;
using TranslateHelper.Core.BL.Contracts;

namespace TranslateHelper.Core.WS
{
	public class TranslateRequestResult
	{
		public string errorDescription;
		//public string translatedText;
        public TranslateResultCollection translateResult;

        public TranslateRequestResult()
        {
            translateResult = new TranslateResultCollection();
        }
    }
}

