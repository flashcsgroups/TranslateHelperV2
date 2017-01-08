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
            return new Language() { ID = Id };
        }

        public Language GetItemForNameEng(string name)
        {
            LanguageManager langManager = new LanguageManager(db);
            return langManager.GetItemForNameEng(name);
            //return new Language() { ID = 1 };
        }

        public Language DefaultLanguage()
        {
            return new Language() { ID = 0 };
        }

        public Language GetItemForShortName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
