using System;
using PortableCore.BL.Contracts;
using PortableCore.DL.SQLite;

namespace PortableCore.DL
{
    //Todo: �� ������������, ������ ����� ��� ���
    public class Language : IBusinessEntity
    {
        public Language()
        {
        }
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int DeleteMark { get; set; }
    }
}