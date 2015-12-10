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
    public class TranslateResult: IHasLabel, IComparable<TranslateResult>
    {
        public string OriginalText;
        public string Pos;//����� ����
        public string Ts;//������������
        public string TranslatedText;
        public int TranslatedExpressionId;//��������� Id ���� ��������� ������ � ��������� ����
        public int FavoritesId;//��������� ���� ��������� ������ � ���������
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
            get { return OriginalText[0].ToString(); }
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