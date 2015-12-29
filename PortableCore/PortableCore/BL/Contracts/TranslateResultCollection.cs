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

    public class TranslateResult : IHasLabel, IComparable<TranslateResult>
    {
        //���������� ��������
        public string Pos;//����� ����
        public string Ts;//������������
        public string TranslatedText;
        public int TranslatedExpressionId;//��������� Id ���� ��������� ������ � ��������� ����
        public int FavoritesId;//��������� ���� ��������� ������ � ���������
        public List<Synonym> SynonymsCollection;
        public List<Mean> MeansCollection;
        public List<ExampleText> ExamplesCollection;

        //����� ��������
        public string OriginalText { get; private set; }
        public List<Definition> Definitions { get; private set; }

        public TranslateResult()
        {
            Definitions = new List<Definition>();
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

    /// <summary>
    /// �������� ������������� �����
    /// </summary>
    public class Definition
    {
        public DefinitionTypesEnum Pos { get; private set; }
        public string Transcription { get; private set; }
        public List<TranslateVariant> TranslateVariants { get; private set; }
    }

    /// <summary>
    /// �������� �������� ��������
    /// </summary>
    public class TranslateVariant
    {
        public string Text { get; private set; }
        public DefinitionTypesEnum Pos { get; private set; }
    }

    public struct Synonym
    {
        public string TranslatedText;
    }



    public struct Mean
    {
        public string Text;
    }



    public struct ExampleText
    {
    }
}
