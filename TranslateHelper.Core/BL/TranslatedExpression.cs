﻿using System;
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
		public string Value { get; set; }
		[Indexed]
		public int SourceExpressionID { get; set; }
	}
}
