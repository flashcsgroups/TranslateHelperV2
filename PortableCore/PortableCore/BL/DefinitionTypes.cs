using System;
using PortableCore.BL.Contracts;
using SQLite;

namespace PortableCore
{
    /// <summary>
    /// Типы частей речи - существительные, глаголы, прилагательные и т.д.
    /// </summary>
	public class DefinitionTypes : IBusinessEntity
	{
		public DefinitionTypes()
		{
		}
		[PrimaryKey]
		public int ID { get; set; }
		[Indexed]
		public int DeleteMark { get; set; }
		[Indexed]
		public string Name { get; set; }
    }
}
