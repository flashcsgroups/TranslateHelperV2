using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortableCore.BL.Managers;
using PortableCore.DL;

namespace PortableCore.Tests.Mocks
{
    public class MockIdiomManager : IIdiomManager
    {
        ISQLiteTesting db;

        public MockIdiomManager(ISQLiteTesting dbHelper)
        {
            db = dbHelper;
        }

        public Idiom GetItemForId(int id)
        {
            throw new NotImplementedException();
        }

        public List<DirectionIdiomItem> GetListDirections()
        {
            throw new NotImplementedException();
        }

        public int SaveItem(Idiom item)
        {
            return 1;
        }
    }
}
