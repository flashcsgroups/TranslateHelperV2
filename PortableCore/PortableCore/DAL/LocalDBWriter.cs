using System;
using PortableCore.BL.Contracts;
using PortableCore.DL;

namespace TranslateHelper.Droid
{
    public class LocalDBWriter
    {
        private ISQLiteTesting db;

        public LocalDBWriter(ISQLiteTesting dbHelper)
        {
            db = dbHelper;
        }

        public void saveResultToLocalCache(TranslateRequestResult result)
        {
            //throw new NotImplementedException();
        }
    }
}