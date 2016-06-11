using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortableCore.BL.Managers;
using PortableCore.DL;

namespace PortableCore.Tests.Mocks
{
    public class MockDirectionManager : IDirectionManager
    {
        //private 
        public MockDirectionManager()
        {

        }

        Direction IDirectionManager.GetItemForId(int Id)
        {
            throw new NotImplementedException();
        }

        Direction IDirectionManager.GetItemForName(string name)
        {
            DirectionManager directionManager = new DirectionManager(new MockSQLite());
            var arr = directionManager.GetDefaultData();
            return arr.Single(p => p.Name == name);
        }
    }
}
