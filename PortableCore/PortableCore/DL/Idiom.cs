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

        public int IdiomID { get { return ID; } set { ID = value; } } //Не знаю что с ним делать - в серверной модели именно IdiomID

        public int IdiomCategoryID { get { return CategoryID; } set { CategoryID = value; } } //Не знаю что с ним делать - в серверной модели именно IdiomCategoryID

        [PrimaryKey, Indexed]
        public int ID { get; set; }
        [Indexed]
        public int CategoryID { get; set; }
        [Indexed]
        public int LanguageFrom { get; set; }
        [Indexed]
        public int LanguageTo { get; set; }
        [Indexed]
        public string TextFrom { get; set; }//текст на языке From
        [Indexed]
        public string TextTo { get; set; }//перевод на языке To
        [Indexed]
        public string ExampleTextFrom { get; set; }//Пример перевода на языке From
        [Indexed]
        public string ExampleTextTo { get; set; }//Пример перевода на языке To
        [Indexed]
        public string DescriptionTextFrom { get; set; }//Описание категории фразы на исходном языке From
        [Indexed]
        public string DescriptionTextTo { get; set; }//Описание категории фразы на языке назначения To
        [Indexed]
        public int DeleteMark { get; set; }
        [Indexed]
        public bool InFavorites { get; set; }//добавлен в избранное
        [Indexed]
        public DateTime UpdateDate { get; set; } //Дата обновления
    }
}
