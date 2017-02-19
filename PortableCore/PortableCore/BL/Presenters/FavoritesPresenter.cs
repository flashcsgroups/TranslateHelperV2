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

        //ILanguageManager languageManager;
        //IChatHistoryManager chatHistoryManager;
        //TranslateDirection direction;
        /// <summary>
        /// Основной конструктор
        /// </summary>
        /// <param name="db"></param>
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
                    ChatHistoryId = item.ID,
                    OriginalText = item.TextTo,
                    TranslatedText = item.TextFrom,
                    Transcription = item.Transcription
                });

            }
            view.UpdateFavorites(indexedFavItems);
        }
    }
}
