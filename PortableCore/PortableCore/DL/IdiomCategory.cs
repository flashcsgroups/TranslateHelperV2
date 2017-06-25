using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortableCore.BL.Contracts;
using SQLite;

namespace PortableCore.DL
{
    public class IdiomCategory : IBusinessEntity
    {
        public IdiomCategory()
        {

        }

        [PrimaryKey, AutoIncrement, Indexed]
        public int ID { get; set; }
        [Indexed]
        public int LanguageFrom { get; set; }
        [Indexed]
        public int LanguageTo { get; set; }
        public string TextFrom { get; set; }//текст на языке From
        public string TextTo { get; set; }//перевод на языке To
        [Indexed]
        public int DeleteMark { get; set; }
    }
}
