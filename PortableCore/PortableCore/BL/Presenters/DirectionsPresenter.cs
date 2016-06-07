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
        //ISQLiteTesting db;
        List<Language> listLanguages = new List<Language>();
        List<DirectionsRecentItem> listDirectionsRecent;
        IChatManager chatManager;
        ILanguageManager languageManager;

        public DirectionsPresenter(IDirectionsView view, IChatManager chatManager, ILanguageManager languageManager)
        {
            this.view = view;
            this.chatManager = chatManager;
            this.languageManager = languageManager;
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
            else
                SelectedAllLanguagesEvent();
        }

        private List<DirectionsRecentItem> getLastChats()
        {
            List<DirectionsRecentItem> listDirectionsRecent;
            listDirectionsRecent = chatManager.GetChatsForLastDays(10);
            return listDirectionsRecent;
        }

        public void SelectedAllLanguagesEvent()
        {
            if(listLanguages.Count() == 0)
            {
                listLanguages = languageManager.GetDefaultData().ToList();
            }
            view.updateListAllLanguages(listLanguages);
        }

        public Chat FoundExistingOrCreateChat(Language robotLanguage)
        {
            //LanguageManager languageManager = new LanguageManager(db);
            Language userLanguage = languageManager.GetItemForNameEng("Russian");
            //ChatManager chatManager = new ChatManager(db);
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
