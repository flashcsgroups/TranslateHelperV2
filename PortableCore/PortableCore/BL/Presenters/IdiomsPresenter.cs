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
        
        public void Init()
        {
            RefreshIdiomsList(string.Empty, false);
        }

        public async void CheckServerTablesUpdate(DateTime lastCheckDate)
        {
            //TimeSpan diff = DateTime.Now - lastCheckDate;
            //Куда ж чаще раза в час проверять?
            //if(diff.Hours > 0)
            //Пока что совсем проверку отключу, надо рефреш делать по свайпу
            //Пока просто год проверяю - он должен быть пустой если первый раз после запуска приложения активити создан
            if(lastCheckDate.Year == 1)
            {
                ApiRequest apiClient = new ApiRequest(hostUrl);
                ClientSync syncTable = new ClientSync(db, idiomManager, apiClient, "idiom");
                DateTime timeStamp = syncTable.GetLocalMaxTimeStamp();
                List<int> iDs = await syncTable.GetChangedIDsFromServer(timeStamp);
                if (iDs.Count > 0)
                {
                    int updatedCount = await syncTable.Sync(iDs);
                    if(updatedCount > 0)
                    {
                        RefreshIdiomsList(string.Empty, true);
                    }
                }
            }
        }

        public void RefreshIdiomsList(string searchString, bool updatedFromServer)
        {
            var indexedItems = this.idiomManager.GetIdiomsForDirections(languageFromId, languageToId, searchString);
            view.UpdateList(indexedItems, updatedFromServer);
        }
    }
}
