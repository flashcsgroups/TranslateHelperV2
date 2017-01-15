using PortableCore.BL.Managers;
using PortableCore.DAL;
using PortableCore.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableCore.BL
{
    public class TranslateDirection
    {
        public Language LanguageFrom { get; private set; }
        public Language LanguageTo { get; private set; }

        ISQLiteTesting db;
        ILanguageManager languageManager;
        string currentDirectionFrom = string.Empty;
        string currentDirectionTo = string.Empty;

        public TranslateDirection(ISQLiteTesting dbHelper, ILanguageManager languageManager)
        {
            this.db = dbHelper;
            this.languageManager = languageManager;
            this.LanguageFrom = languageManager.GetItemForNameEng("English");
            this.LanguageTo = languageManager.GetItemForNameEng("Russian");
        }

        public void SetDirection(Language languageFrom, Language languageTo)
        {
            this.LanguageFrom = languageFrom;
            this.LanguageTo = languageTo;
        }

        public void Invert()
        {
            var currentDirectionFrom = this.LanguageFrom;
            var currentDirectionTo = this.LanguageTo;
            SetDirection(currentDirectionTo, currentDirectionFrom);
        }
    }
}
