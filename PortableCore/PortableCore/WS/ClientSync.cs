using PortableCore.BL.Managers;
using PortableCore.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableCore.WS
{
    public class ClientSync
    {
        private ISQLiteTesting _db;
        private IIdiomManager _idiomManager;
        private string _tableName;
        private IApiClient _srvClient;

        public ClientSync(ISQLiteTesting db, IIdiomManager idiomManager, IApiClient srvClient, string tableName)
        {
            this._db = db;
            this._idiomManager = idiomManager;
            this._tableName = tableName.ToLower();
            this._srvClient = srvClient;
        }

        internal DateTime GetLocalMaxTimeStamp()
        {
            DateTime maxDate = new DateTime();

            switch (_tableName)
            {
                case "idiom":
                    {
                        maxDate = _db.Table<Idiom>().Max(item => item.UpdateDate);
                    }; break;
            }

            return maxDate;
        }

        internal async Task<List<int>> GetChangedIDsFromServer(DateTime localMaxTimeStampString)
        {
            var result = await _srvClient.GetChangedIDsFromServer(_tableName, localMaxTimeStampString);
            return result;
        }

        internal async Task<int> Sync(List<int> iDs)
        {
            int processed = 0;
            switch(_tableName)
            {
                case "idiom":
                    {
                        LanguageManager langManager = new LanguageManager(_db);
                        List<Idiom> idioms = await _srvClient.GetIdiomsFromServer(iDs);
                        foreach (Idiom idiomItem in idioms)
                        {
                            try
                            {
                                _idiomManager.SaveItem(idiomItem);
                                processed++;
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }; break;
                case "IdiomTest":
                    {
                        throw new NotImplementedException();
                    }; 
            }
            return processed;
        }
    }
}
