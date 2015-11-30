using System;
using System.Net;
using System.Json;

namespace TranslateHelper.Droid
{
	public struct TranslateServiceResult
	{
		public string errorDescription;
		public HttpStatusCode statuscode;
		public string response;
	}
}

