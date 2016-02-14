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
        private  Direction currentDirection;

        public TranslateDirection()
        {
            SetDirection("en-ru");//default, Eng->Rus
        }

        public int GetCurrentDirectionId()
        {
            return currentDirection.ID;
        }

        public string GetCurrentDirectionName()
        {
            return currentDirection.Name;
        }

        public string GetCurrentDirectionNameFull()
        {
            return currentDirection.FullName;
        }

        public void SetDirection(string textDirection)
        {
            DirectionManager dirManager = new DirectionManager(SqlLiteInstance.DB);
            currentDirection = dirManager.GetItemForName(textDirection);
        }
    }
}
