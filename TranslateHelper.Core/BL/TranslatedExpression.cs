using System;
using TranslateHelper.Core.BL.Contracts;
using SQLite;

namespace TranslateHelper.Core
{
	public class TranslatedExpression : IBusinessEntity
	{
		public TranslatedExpression ()
		{
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
