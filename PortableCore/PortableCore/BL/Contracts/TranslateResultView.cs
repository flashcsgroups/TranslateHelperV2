using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortableCore.BL.Contracts
{
    public class TranslateResultView 
    {
        public List<TranslateResultDefinition> Definitions { get; private set; }

        public TranslateResultView()
        {
            Definitions = new List<TranslateResultDefinition>();
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
        private int translatedExpressionId;//Используется для связи с БД, заполняется только после записи в кэш
        //private int favoritesId;//Используется для связи с БД, заполняется только после записи в кэш

        public int TranslatedExpressionId
        {
            set
            {
                this.translatedExpressionId = value;
            }
            get
            {
                return this.translatedExpressionId;
            }
        }

        /*public int FavoritesId
        {
            set
            {
                this.favoritesId = value;
            }
            get
            {
                return this.favoritesId;
            }
        }*/

        public ResultLineData(string text, DefinitionTypesEnum pos)
        {
            Text = text;
            Pos = pos;
        }
    }
}
