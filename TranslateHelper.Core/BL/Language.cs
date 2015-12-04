using System;
using TranslateHelper.Core.BL.Contracts;
using SQLite;

namespace TranslateHelper.Core
{
    //Todo: �� ������������, ������ ����� ��� ���
    public class Language : IBusinessEntity
    {
        public Language()
        {
        }
        [PrimaryKey, AutoIncrement, Indexed]
        public int ID { get; set; }
        public int DeleteMark { get; set; }
        [Indexed]
        public string Name { get; set; }
    }
}
