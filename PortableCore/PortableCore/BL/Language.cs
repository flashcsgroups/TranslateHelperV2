using System;
using PortableCore.BL.Contracts;
using SQLite;

namespace PortableCore
{
    //Todo: не используется, решить нужен или нет
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
