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
        ISQLiteTesting db;
        Direction currentDirection;
        IDirectionManager directionManager;
        string currentDirectionFrom = string.Empty;
        string currentDirectionTo = string.Empty;

        public TranslateDirection(ISQLiteTesting dbHelper, IDirectionManager directionManager)
        {
            db = dbHelper;
            this.directionManager = directionManager;
        }

        public void SetDefaultDirection()
        {
            SetDirection("en-ru");//default, Eng->Rus
        }

        public int GetCurrentDirectionId()
        {
            int id = 0;
            if (currentDirection != null) id = currentDirection.ID;
            return id;
        }

        public string GetCurrentDirectionName()
        {
            string name = string.Empty;
            if (currentDirection != null) name = currentDirection.Name;
            return name;
        }

        public string GetCurrentDirectionNameFull()
        {
            string fullName = string.Empty;
            if (currentDirection != null) fullName = currentDirection.FullName;
            return fullName;
        }

        public void SetDirection(string textDirection)
        {
            currentDirection = directionManager.GetItemForName(textDirection);
            var arr = textDirection.Split('-');
            if (arr.Count() > 1)
            {
                currentDirectionFrom = arr[0];
                currentDirectionTo = arr[1];
            }
            else throw new Exception("Error parsing direction!");
        }

        public void Invert()
        {
            string tmp = currentDirectionFrom;
            currentDirectionFrom = currentDirectionTo;
            currentDirectionTo = tmp;
            SetDirection(currentDirectionFrom + "-" + currentDirectionTo);
        }

        /// <summary>
        /// Истина если параметр локали соответствует языку-источнику в направлении перевода.
        /// </summary>
        /// <param name="comparedLocaleName"></param>
        /// <returns></returns>
        public bool IsFrom(string comparedLocaleName)
        {
            string defaultLatLocale = "zz";//локаль для типа Латиница
            string localeName = string.Empty;
            var arr = comparedLocaleName.Split('_');
            if (arr.Count() > 0)
                localeName = arr[0].ToLower();
            else
                localeName = comparedLocaleName.ToLower();
            if (localeName == defaultLatLocale) localeName = "en";
            return currentDirectionFrom == localeName;
        }
    }
}
