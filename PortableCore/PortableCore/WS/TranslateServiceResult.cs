using System.Net;
using PortableCore.BL.Contracts;

namespace PortableCore.WS
{
	public class TranslateRequestResult
	{
        public string errorDescription;//ToDo:переделать на приватное
        public TranslateResultCollection translateResult;//Todo:Устарело, удалить!
        public TranslateResult TranslatedData { get; private set; }

        public TranslateRequestResult()
        {
            translateResult = new TranslateResultCollection();
        }

        /*public TranslateRequestResult(ITranslatedData translatedData)
        {
            TranslatedData = translatedData;
        }*/
    }
}

