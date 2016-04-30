using System;
using PortableCore.BL.Contracts;
using SQLite;

namespace PortableCore.BL.Presenters
{
    public class LastChats : IBusinessEntity
    {
        public LastChats()
        {

        }

        [PrimaryKey, AutoIncrement, Indexed]
        public int ID { get; set; }
        public int LangFrom { get; set; }
        public int LangTo { get; set; }
        [Indexed]
        public DateTime LastChanges { get; set; }
        public int DeleteMark { get; set; }
    }
}