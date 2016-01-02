using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableCore.BL.Contracts
{
    public abstract class TranslateRequestFactory
    {
        public abstract Task<string> GetResponse(string sourceString, string direction);
        public abstract TranslateResultCollection ParseResponse(string responseText);//ToDo: old, delete it!
        public abstract TranslateResult Parse(string responseText);//actual
    }
}