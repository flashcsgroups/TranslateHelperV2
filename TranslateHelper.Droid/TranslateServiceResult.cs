using System.Net;

namespace TranslateHelper.Droid
{
    public struct TranslateServiceResult
	{
		public string errorDescription;
		public HttpStatusCode statuscode;
		public string response;
	}
}

