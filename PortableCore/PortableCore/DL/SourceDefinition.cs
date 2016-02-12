using System;
using PortableCore.BL.Contracts;
using SQLite;

namespace PortableCore.DL
{
	public class SourceDefinition : IBusinessEntity
	{
        //ToDo: добавить геттер для свойств публичных
		public SourceDefinition()
		{
            TranscriptionText = string.Empty;
        }
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
		public int DeleteMark { get; set; }
        public string TranscriptionText { get; set; }
        public int DefinitionTypeID { get; set; }
		[Indexed]
		public int SourceExpressionID { get; set; }
	}
}
