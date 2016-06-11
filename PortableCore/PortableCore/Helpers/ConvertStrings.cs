using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace PortableCore.Helpers
{
    public static class ConvertStrings
    {
        public static string StringToOneLowerLineWithTrim(string sourceString)
        {
            string tmp = sourceString.Trim().Replace("\r", "");
            return tmp.Replace("\n", " ").ToLower();
        }
    }
}