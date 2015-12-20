using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortableCore.BL.Contracts
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

namespace PortableCore.BL.Contracts
{
    public class TranslateResult: IHasLabel, IComparable<TranslateResult>
    {
        public string OriginalText;
        public string Pos;//часть речи
        public string Ts;//транскрипция
        public string TranslatedText;
        public int TranslatedExpressionId;//заполнено Id если результат найден в локальном кеше
        public int FavoritesId;//Заполнено если результат найден в избранном
        public List<Synonym> SynonymsCollection;
        public List<Mean> MeansCollection;
        public List<ExampleText> ExamplesCollection;

        public TranslateResult()
        {
            SynonymsCollection = new List<Synonym>();
            MeansCollection = new List<Mean>();
            ExamplesCollection = new List<ExampleText>();
        }

        int IComparable<TranslateResult>.CompareTo(TranslateResult value)
        {
            return OriginalText.CompareTo(value.OriginalText);
        }

        public string Label
        {
            get
            {
                string label = string.Empty;
                if (!string.IsNullOrEmpty(OriginalText))
                    label = OriginalText[0].ToString();
                return label;
            }
        }

    }
}

namespace PortableCore.BL.Contracts
{
    public struct Synonym
    {
        public string TranslatedText;
    }
}

namespace PortableCore.BL.Contracts
{
    public struct Mean
    {
        public string Text;
    }
}

namespace PortableCore.BL.Contracts
{
    public struct ExampleText
    {
    }
}