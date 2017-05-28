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
        private int selectedChatID;
        private IAnecdotesView view;
        private AnecdoteManager anecdoteManager;

        /// <summary>
        /// Основной конструктор
        /// </summary>
        /// <param name="view"></param>
        /// <param name="db"></param>
        /// <param name="selectedChatID"></param>
        public AnecdotesPresenter(IAnecdotesView view, ISQLiteTesting db, int selectedChatID)
        {
            this.view = view;
            this.db = db;
            this.selectedChatID = selectedChatID;
            this.anecdoteManager = new AnecdoteManager(db);
        }


        public void Init()
        {
            var indexedItems = this.anecdoteManager.GetAllAnecdotesForChat(selectedChatID);
            view.UpdateList(indexedItems);
        }
    }
}
