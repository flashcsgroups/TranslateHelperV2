using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortableCore.BL.Contracts;
using SQLite;

namespace PortableCore.DL
{
    public class Idiom : IBusinessEntity
    {
        public Idiom()
        {

        }

        [PrimaryKey, AutoIncrement, Indexed]
        public int ID { get; set; }
        [Indexed]
        public int CategoryID { get; set; }
        [Indexed]
        public int LanguageFrom { get; set; }
        [Indexed]
        public int LanguageTo { get; set; }
        public string TextFrom { get; set; }//текст на языке From
        public string TextTo { get; set; }//перевод на языке To
        public string ExampleTextFrom { get; set; }//Пример перевода на языке From
        public string ExampleTextTo { get; set; }//Пример перевода на языке To
        [Indexed]
        public string DescriptionTextFrom { get; set; }//Описание категории фразы на исходном языке From
        [Indexed]
        public string DescriptionTextTo { get; set; }//Описание категории фразы на языке назначения To
        [Indexed]
        public int DeleteMark { get; set; }
        [Indexed]
        public bool InFavorites { get; set; }//добавлен в избранное
    }
}
