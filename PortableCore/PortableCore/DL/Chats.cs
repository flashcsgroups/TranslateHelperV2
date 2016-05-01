using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortableCore.BL.Contracts;
using SQLite;

namespace PortableCore.DL
{
    public class Chats : IBusinessEntity
    {
        public Chats()
        {

        }

        [PrimaryKey, AutoIncrement, Indexed]
        public int ID { get; set; }
        public int LanguageFrom { get; set; }
        public int LanguageTo { get; set; }
        public string LanguageCaptionFrom { get; set; }
        public string LanguageCaptionTo { get; set; }
        [Indexed]
        public DateTime UpdateDate { get; set; }
        public int DeleteMark { get; set; }
    }
}
