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
        Direction currentDirection;
        IDirectionManager directionManager;
        ILanguageManager languageManager;
        string currentDirectionFrom = string.Empty;
        string currentDirectionTo = string.Empty;

        public TranslateDirection(ISQLiteTesting dbHelper, IDirectionManager directionManager, ILanguageManager languageManager)
        {
            this.db = dbHelper;
            this.directionManager = directionManager;
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

        /// <summary>
        /// Истина если параметр локали соответствует языку-источнику в направлении перевода.
        /// </summary>
        /// <param name="comparedLocaleName"></param>
        /// <returns></returns>
        public bool IsFrom(DetectInputLanguage.Language lang)
        {
            bool result = (lang == DetectInputLanguage.Language.English && this.LanguageFrom.NameShort == "en");
            if (!result)
                result = (lang == DetectInputLanguage.Language.Russian && this.LanguageFrom.NameShort == "ru");
            return result;
        }

        #region Obsolete
        [Obsolete()]
        public TranslateDirection(ISQLiteTesting dbHelper, IDirectionManager directionManager)
        {
            this.db = dbHelper;
            this.directionManager = directionManager;
        }

        public void SetDefaultDirection()
        {
            throw new NotImplementedException();
            //SetDirection("en-ru");//default, Eng->Rus
        }

        public int GetCurrentDirectionId1()
        {
            throw new NotImplementedException();
            int id = 0;
            if (currentDirection != null) id = currentDirection.ID;
            return id;
        }

        public string GetCurrentDirectionName()
        {
            throw new NotImplementedException();
            string name = string.Empty;
            if (currentDirection != null) name = currentDirection.Name;
            return name;
        }

        public string GetCurrentDirectionNameFull()
        {
            throw new NotImplementedException();
            string fullName = string.Empty;
            if (currentDirection != null) fullName = currentDirection.FullName;
            return fullName;
        }

        /*[Obsolete()]
        public void SetDirection(string textDirection)
        {
            throw new NotImplementedException();
            currentDirection = directionManager.GetItemForName(textDirection);
            var arr = textDirection.Split('-');
            if (arr.Count() > 1)
            {
                currentDirectionFrom = arr[0];
                currentDirectionTo = arr[1];
            }
            else throw new Exception("Error parsing direction!");
        }*/


        #endregion
    }
}
