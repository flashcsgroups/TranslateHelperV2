using System;
using PortableCore.BL.Contracts;
using SQLite;

namespace PortableCore
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
