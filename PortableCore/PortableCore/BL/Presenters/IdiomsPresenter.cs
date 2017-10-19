using PortableCore.BL.Managers;
using PortableCore.BL.Models;
using PortableCore.BL.Views;
using PortableCore.DL;
using PortableCore.WS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableCore.BL.Presenters
{
    public class IdiomsPresenter
    {
        private ISQLiteTesting db;
        private int languageFromId;
        private int languageToId;
        private IIdiomsView view;
        private IdiomManager idiomManager;
        private string hostUrl = "http://serverweb20171015090425.azurewebsites.net/api";

        public IdiomsPresenter(IIdiomsView view, ISQLiteTesting db, int languageFromId, int languageToId)
        {
            this.view = view;
            this.db = db;
            this.languageFromId = languageFromId;
            this.languageToId = languageToId;
            this.idiomManager = new IdiomManager(db, new LanguageManager(db));
        }


        public async void Init()
        {
            FindIdioms(string.Empty);
            ApiRequest apiClient = new ApiRequest(hostUrl);
            ClientSync syncTable = new ClientSync(db, idiomManager, apiClient, "idiom");
            DateTime timeStamp = syncTable.GetLocalMaxTimeStamp();
            List<int> iDs = await syncTable.GetChangedIDsFromServer(timeStamp);
            if(iDs.Count > 0)
                await syncTable.Sync(iDs);
        }

        public void FindIdioms(string searchString)
        {
            var indexedItems = this.idiomManager.GetIdiomsForDirections(languageFromId, languageToId, searchString);
            view.UpdateList(indexedItems);
        }
    }
}
