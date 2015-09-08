using System;

namespace TranslateHelper.BL.Contracts {
	public interface IBusinessEntity {
		int ID { get; set; }
		int DeleteMark {get; set;}
	}
}

