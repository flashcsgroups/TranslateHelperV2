using System;
using TranslateHelper.Core.BL.Contracts;
using SQLite;

namespace TranslateHelper.Core
{
    /// <summary>
    /// Типы частей речи - существительные, глаголы, прилагательные и т.д.
    /// </summary>
	public class DefinitionTypes : IBusinessEntity
	{
		public DefinitionTypes()
		{
		}
		[PrimaryKey, Indexed]
		public int ID { get; set; }
		[Indexed]
		public int DeleteMark { get; set; }
		[Indexed]
		public string Name { get; set; }
    }
}
