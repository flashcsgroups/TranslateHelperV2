using System;
using PortableCore.BL.Contracts;
using SQLite;

namespace PortableCore.DL
{
    public class Language : IBusinessEntity
    {
        public Language()
        {
        }
        [PrimaryKey, AutoIncrement, Indexed]
        public int ID { get; set; }
        public string NameEng { get; set; }
        public string NameLocal { get; set; }
        public int DeleteMark { get; set; }
    }
}
