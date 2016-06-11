using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortableCore.BL.Managers;
using PortableCore.DL;

namespace PortableCore.Tests.Mocks
{
    public class MockLanguageManager : ILanguageManager
    {
        ISQLiteTesting db;

        public MockLanguageManager(ISQLiteTesting dbHelper)
        {
            db = dbHelper;
        }

        public Language[] GetDefaultData()
        {
            throw new NotImplementedException();
        }

        public Language GetItemForId(int Id)
        {
            return new Language() { ID = 1 };
        }

        public Language GetItemForNameEng(string name)
        {
            return new Language() { ID = 1 };
        }
    }
}
