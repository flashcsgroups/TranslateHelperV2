using System;
using TranslateHelper.Core.BL.Contracts;
using SQLite;

namespace TranslateHelper.Core
{
	public class TranslatedExpression : IBusinessEntity
	{
        //ToDo: добавить геттер для свойств публичных
		public TranslatedExpression ()
		{
            TranslatedText = string.Empty;
            TranscriptionText = string.Empty;
        }
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
		public int DeleteMark { get; set; }
		public string TranslatedText { get; set; }
        public string TranscriptionText { get; set; }
        public int DefinitionTypeID { get; set; }
		[Indexed]
		public int SourceExpressionID { get; set; }
	}
}
