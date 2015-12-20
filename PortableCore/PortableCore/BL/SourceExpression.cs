﻿using System;
using PortableCore.BL.Contracts;
using SQLite;

namespace PortableCore
{
	public class SourceExpression : IBusinessEntity
	{
		public SourceExpression ()
		{
		}
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
		public int DeleteMark { get; set; }        
		[Indexed]
		public string Text { get; set; }
		[Indexed]
		public int DirectionID { get; set; }
	}
}
