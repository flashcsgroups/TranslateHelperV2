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
    public class FavoritesPresenter
    {
        private ChatHistoryManager chatHistoryManager;
        private ISQLiteTesting db;
        private int selectedChatID;
        private IFavoritesView view;

        /// <summary>
        /// Основной конструктор
        /// </summary>
        /// <param name="view"></param>
        /// <param name="db"></param>
        /// <param name="selectedChatID"></param>
        public FavoritesPresenter(IFavoritesView view, ISQLiteTesting db, int selectedChatID)
        {
            this.view = view;
            this.db = db;
            this.selectedChatID = selectedChatID;
            this.chatHistoryManager = new ChatHistoryManager(db);
        }

        public void Init()
        {
            IndexedCollection<FavoriteItem> indexedFavItems = new IndexedCollection<FavoriteItem>();
            var listMessages = this.chatHistoryManager.GetFavoriteMessages(selectedChatID);
            foreach(var item in listMessages)
            {
                indexedFavItems.Add(new FavoriteItem()
                {
                    ChatHistoryId = item.Item1.ID,
                    OriginalText = item.Item2.TextFrom,
                    TranslatedText = item.Item1.TextTo,
                    Transcription = item.Item1.Transcription,
                    OriginalTextDefinition = item.Item1.Definition
                });

            }
            view.UpdateFavorites(indexedFavItems);
        }
    }
}
