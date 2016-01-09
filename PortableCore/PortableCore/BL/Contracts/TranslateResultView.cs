using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortableCore.BL.Contracts
{
    public class TranslateResultView 
    {
        //���������� ��������
        //public string Pos;//����� ����
        //public string Ts;//������������
        //public string TranslatedText;
        //public int TranslatedExpressionId;//��������� Id ���� ��������� ������ � ��������� ����
        //public int FavoritesId;//��������� ���� ��������� ������ � ���������
        //public List<Synonym> SynonymsCollection;
        //public List<Mean> MeansCollection;
        //public List<ExampleText> ExamplesCollection;

        //����� ��������
        public List<TranslateResultDefinition> Definitions { get; private set; }

        public TranslateResultView()
        {
            //new
            Definitions = new List<TranslateResultDefinition>();

            //old
            //SynonymsCollection = new List<Synonym>();
            //MeansCollection = new List<Mean>();
            //ExamplesCollection = new List<ExampleText>();
        }

        public void AddDefinition(string originalText, DefinitionTypesEnum pos, string transcription, List<TranslateResultVariant> translateVariants)
        {
            Definitions.Add(new TranslateResultDefinition(originalText, pos, transcription, translateVariants));
        }
    }

    /// <summary>
    /// �������� ������������� �����
    /// </summary>
    public class TranslateResultDefinition
    {
        public string OriginalText { get; private set; }
        public DefinitionTypesEnum Pos { get; private set; }
        public string Transcription { get; private set; }
        public List<TranslateResultVariant> TranslateVariants { get; private set; }

        public TranslateResultDefinition(string originalText, DefinitionTypesEnum pos, string transcription, List<TranslateResultVariant> translateVariants)
        {
            OriginalText = originalText;
            Pos = pos;
            Transcription = transcription;
            TranslateVariants = translateVariants;
        }
    }

    /// <summary>
    /// �������� �������� ��������
    /// </summary>
    public class TranslateResultVariant
    {
        public string Text { get; private set; }
        public DefinitionTypesEnum Pos { get; private set; }

        public TranslateResultVariant(string text, DefinitionTypesEnum pos)
        {
            Text = text;
            Pos = pos;
        }
    }

    /*public struct Synonym
    {
        public string TranslatedText;
    }



    public struct Mean
    {
        public string Text;
    }



    public struct ExampleText
    {
    }*/
}
