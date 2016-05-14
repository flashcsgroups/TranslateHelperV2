using PortableCore.BL.Managers;
using PortableCore.BL.Views;
using PortableCore.DAL;
using PortableCore.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortableCore.BL.Models;

namespace PortableCore.BL.Presenters
{
    public class DirectionsPresenter
    {
        IDirectionsView view;
        ISQLiteTesting db;
        List<Language> listLanguages = new List<Language>();
        List<DirectionsRecentItem> listDirectionsRecent;

        public DirectionsPresenter(IDirectionsView view, ISQLiteTesting db)
        {
            this.view = view;
            this.db = db;
        }

        public void SelectedRecentLanguagesEvent()
        {
            if(listDirectionsRecent == null)
                listDirectionsRecent = getLastChats();

            view.updateListRecentDirections(listDirectionsRecent);
        }

        public void ShowRecentOrFullListLanguages()
        {
            if (listDirectionsRecent == null)
                listDirectionsRecent = getLastChats();

            if (listDirectionsRecent.Count > 0)
            {
                SelectedRecentLanguagesEvent();
            }
        }

        private List<DirectionsRecentItem> getLastChats()
        {
            List<DirectionsRecentItem> listDirectionsRecent;
            ChatManager chatManager = new ChatManager(db);
            listDirectionsRecent = chatManager.GetChatsForLastDays(10);
            return listDirectionsRecent;
        }

        public void SelectedAllLanguagesEvent()
        {
            if(listLanguages.Count() == 0)
            {
                LanguageManager langManager = new LanguageManager(db);
                listLanguages = langManager.GetDefaultData().ToList();
            }
            view.updateListAllLanguages(listLanguages);
        }

        public Chat FoundExistingOrCreateChat(Language robotLanguage)
        {
            LanguageManager languageManager = new LanguageManager(db);
            Language userLanguage = languageManager.GetItemForNameEng("Russian");
            ChatManager chatManager = new ChatManager(db);
            Chat chat = chatManager.GetChatByLanguage(userLanguage, robotLanguage);
            if(chat == null)
            {
                chat = new Chat();
                chat.LanguageFrom = userLanguage.ID;
                chat.LanguageCaptionFrom = userLanguage.NameLocal;
                chat.LanguageTo = robotLanguage.ID;
                chat.LanguageCaptionTo = robotLanguage.NameLocal;
                chat.UpdateDate = DateTime.Now;
                chatManager.SaveItem(chat);
            }
            return chat;
        }
    }
}
