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

        public TranslateDirection(ISQLiteTesting dbHelper)
        {
            db = dbHelper;
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
            DirectionManager dirManager = new DirectionManager(db);
            currentDirection = dirManager.GetItemForName(textDirection);
        }

        public void Invert()
        {
            //ToDo:Сделать универсально для разных языков
            switch (GetCurrentDirectionName())
            {
                case "en-ru":
                    {
                        SetDirection("ru-en");
                    }; break;
                case "ru-en":
                    {
                        SetDirection("en-ru");
                    }; break;
            }
        }
    }
}
