using System.Net;
using PortableCore.BL.Contracts;

namespace PortableCore.WS
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

