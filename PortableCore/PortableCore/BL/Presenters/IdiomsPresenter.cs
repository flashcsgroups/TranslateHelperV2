using PortableCore.BL.Managers;
using PortableCore.BL.Models;
using PortableCore.BL.Views;
using PortableCore.DL;
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

            FindIdioms(string.Empty);
        }

        public void FindIdioms(string searchString)
        {
            var indexedItems = this.idiomManager.GetIdiomsForDirections(languageFromId, languageToId, searchString);
            view.UpdateList(indexedItems);
        }
    }
}
