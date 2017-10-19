﻿using System;

namespace Server.Web.Models
{
    public class IdiomCategory
    {
        public IdiomCategory()
        {

        }

        public int IdiomCategoryID { get; set; }
        public int LanguageFrom { get; set; }
        public int LanguageTo { get; set; }
        public string TextFrom { get; set; }//текст на языке From
        public string TextTo { get; set; }//перевод на языке To
        public int DeleteMark { get; set; }
        public DateTime UpdateDate { get; set; } //Дата обновления
    }
}
