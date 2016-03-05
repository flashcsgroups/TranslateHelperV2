using System;
using PortableCore.BL.Contracts;
using SQLite;

namespace PortableCore.DL
{
	public class TranslatedExpression : IBusinessEntity
	{
        //ToDo: добавить геттер для свойств публичных
		public TranslatedExpression ()
		{
            TranslatedText = string.Empty;
        }
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
		public int DeleteMark { get; set; }

		public string TranslatedText { get; set; }
        public int DefinitionTypeID { get; set; }

		[Indexed]
		public int SourceDefinitionID { get; set; }
        [Indexed]
        public int DirectionID { get; set; }
    }
}
