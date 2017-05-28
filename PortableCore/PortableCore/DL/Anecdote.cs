using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortableCore.BL.Contracts;
using SQLite;

namespace PortableCore.DL
{
    public class Anecdote : IBusinessEntity
    {
        public Anecdote()
        {

        }

        [PrimaryKey, AutoIncrement, Indexed]
        public int ID { get; set; }
        public int LanguageFrom { get; set; }
        public int LanguageTo { get; set; }
        public string TextFrom { get; set; }//текст на языке From
        public string TextTo { get; set; }//перевод на языке To
        [Indexed]
        public int ReadMark { get; set; }//прочитан
        public int DeleteMark { get; set; }
    }
}
