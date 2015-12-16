using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TranslateHelper.Core.Helpers
{
    public static class ConvertStrings
    {
        public static string StringToOneLowerLineWithTrim(string sourceString)
        {
            return sourceString.Trim().Replace("\r\n", " ").ToLower();
        }
    }
}