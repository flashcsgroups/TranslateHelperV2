
namespace Server.Web.Models
{
    public class Idiom
    {
        public Idiom()
        {

        }

        public int IdiomID { get; set; }
        public int IdiomCategoryID { get; set; }
        public int LanguageFrom { get; set; }
        public int LanguageTo { get; set; }
        public string TextFrom { get; set; }//текст на языке From
        public string TextTo { get; set; }//перевод на языке To
        public string ExampleTextFrom { get; set; }//Пример перевода на языке From
        public string ExampleTextTo { get; set; }//Пример перевода на языке To
        public string DescriptionTextFrom { get; set; }//Описание категории фразы на исходном языке From
        public string DescriptionTextTo { get; set; }//Описание категории фразы на языке назначения To
        public int DeleteMark { get; set; }
        public bool InFavorites { get; set; }//добавлен в избранное
        public IdiomCategory IdiomCategory { get; set; }

    }
}
