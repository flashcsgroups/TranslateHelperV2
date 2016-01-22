using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortableCore.BL.Contracts
{
    public class TranslateResultView 
    {
        //устаревшие свойства
        //public string Pos;//часть речи
        //public string Ts;//транскрипция
        //public string TranslatedText;
        //public int TranslatedExpressionId;//заполнено Id если результат найден в локальном кеше
        //public int FavoritesId;//Заполнено если результат найден в избранном
        //public List<Synonym> SynonymsCollection;
        //public List<Mean> MeansCollection;
        //public List<ExampleText> ExamplesCollection;

        //новые свойства
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

        public void AddDefinition(string originalText, DefinitionTypesEnum pos, string transcription, List<ResultLineData> translateVariants)
        {
            Definitions.Add(new TranslateResultDefinition(originalText, pos, transcription, translateVariants));
        }
    }

    /// <summary>
    /// Свойства оригинального слова
    /// </summary>
    public class TranslateResultDefinition
    {
        public string OriginalText { get; private set; }
        public DefinitionTypesEnum Pos { get; private set; }
        public string Transcription { get; private set; }
        public List<ResultLineData> TranslateVariants { get; private set; }

        public TranslateResultDefinition(string originalText, DefinitionTypesEnum pos, string transcription, List<ResultLineData> translateVariants)
        {
            OriginalText = originalText;
            Pos = pos;
            Transcription = transcription;
            TranslateVariants = translateVariants;
        }
    }

    /// <summary>
    /// Свойства варианта перевода
    /// </summary>
    public class ResultLineData
    {
        public string Text { get; private set; }
        public DefinitionTypesEnum Pos { get; private set; }

        public ResultLineData(string text, DefinitionTypesEnum pos)
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
