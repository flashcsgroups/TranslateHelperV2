using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslateHelper.Core.BL.Contracts
{
    public abstract class TranslateRequestFactory
    {
        public abstract Task<string> GetResponse(string sourceString, string direction);
        public abstract string ParseResponse(string responseText);
    }
}