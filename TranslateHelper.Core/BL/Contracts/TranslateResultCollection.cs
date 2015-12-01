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

namespace TranslateHelper.Core.BL.Contracts
{
    public class TranslateResultCollection
    {
        public List<TranslateResult> Collection;
        public TranslateResultCollection()
        {
            Collection = new List<TranslateResult>();
        }
    }
}

namespace TranslateHelper.Core.BL.Contracts
{
    public class TranslateResult
    {
        public string OriginalText;
        public string Pos;//часть речи
        public string Ts;//транскрипция
        public string TranslatedText;
        public List<Synonym> SynonymsCollection;
        public List<Mean> MeansCollection;
        public List<ExampleText> ExamplesCollection;

        public TranslateResult()
        {
            SynonymsCollection = new List<Synonym>();
            MeansCollection = new List<Mean>();
            ExamplesCollection = new List<ExampleText>();
        }
    }
}

namespace TranslateHelper.Core.BL.Contracts
{
    public struct Synonym
    {
        public string TranslatedText;
    }
}

namespace TranslateHelper.Core.BL.Contracts
{
    public struct Mean
    {
        public string Text;
    }
}

namespace TranslateHelper.Core.BL.Contracts
{
    public struct ExampleText
    {
    }
}