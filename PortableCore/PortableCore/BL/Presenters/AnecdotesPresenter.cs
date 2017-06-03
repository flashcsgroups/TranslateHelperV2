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
    public class AnecdotesPresenter
    {
        private ISQLiteTesting db;
        private int languageFromId;
        private int languageToId;
        private IAnecdotesView view;
        private AnecdoteManager anecdoteManager;


        public AnecdotesPresenter(IAnecdotesView view, ISQLiteTesting db, int languageFromId, int languageToId)
        {
            this.view = view;
            this.db = db;
            this.languageFromId = languageFromId;
            this.languageToId = languageToId;
            this.anecdoteManager = new AnecdoteManager(db, new LanguageManager(db));
        }


        public void Init()
        {
            var indexedItems = this.anecdoteManager.GetAllAnecdotesForDirections(languageFromId, languageToId);
            view.UpdateList(indexedItems);
        }
    }
}
