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
        private IChatManager chatManager;
        private ILanguageManager languageManager;
        private IAnecdoteManager anecdotesManager;

        public DirectionsPresenter(IDirectionsView view, IChatManager chatManager, ILanguageManager languageManager, IAnecdoteManager anecdotesManager)
        {
            this.view = view;
            this.chatManager = chatManager;
            this.languageManager = languageManager;
            this.anecdotesManager = anecdotesManager;
        }

        public void SelectedRecentLanguagesEvent()
        {
            listDirectionsRecent = getLastChats();

            view.updateListRecentDirections(listDirectionsRecent);
        }

        /*public void ShowRecentOrFullListLanguages(string currentLocaleShort)
        {
            if (listDirectionsRecent == null)
                listDirectionsRecent = getLastChats();

            if (listDirectionsRecent.Count > 0)
            {
                SelectedRecentLanguagesEvent();
            }
            else
                SelectedAllLanguagesEvent(currentLocaleShort);
        }*/
        public void ShowRecentListLanguages(string currentLocaleShort)
        {
            if (listDirectionsRecent == null)
                listDirectionsRecent = getLastChats();
            if (listDirectionsRecent.Count > 0)
            {
                SelectedRecentLanguagesEvent();
            }
            else
            {
                ShowFullListLanguages(currentLocaleShort);
            }
        }

        public void ShowFullListLanguages(string currentLocaleShort)
        {
            SelectedAllLanguagesEvent(currentLocaleShort);
        }

        private List<DirectionsRecentItem> getLastChats()
        {
            List<DirectionsRecentItem> listDirectionsRecent;
            listDirectionsRecent = chatManager.GetChatsForLastDays(10);
            return listDirectionsRecent;
        }

        public void SelectedAllLanguagesEvent(string currentLocaleShort)
        {
            if(listLanguages.Count() == 0)
            {
                var defaultData = languageManager.GetDefaultData();
                listLanguages = defaultData.Where(e=>e.NameShort != currentLocaleShort).ToList();
                listLanguages.Add(defaultData.Where(e => e.NameShort == currentLocaleShort).Single());
                listLanguages.Add(defaultData.Where(e => e.NameShort == currentLocaleShort).Single());
                listLanguages.Add(defaultData.Where(e => e.NameShort == currentLocaleShort).Single());
                listLanguages.Add(defaultData.Where(e => e.NameShort == currentLocaleShort).Single());
                listLanguages.Add(defaultData.Where(e => e.NameShort == currentLocaleShort).Single());
                listLanguages.Add(defaultData.Where(e => e.NameShort == currentLocaleShort).Single());
                listLanguages.Add(defaultData.Where(e => e.NameShort == currentLocaleShort).Single());
                listLanguages.Add(defaultData.Where(e => e.NameShort == currentLocaleShort).Single());
            }
            view.updateListAllLanguages(listLanguages);
        }

        public void SelectedListFunStoriesEvent()
        {
            var listDirectionsOfStories = anecdotesManager.GetListDirectionsForStories();
            view.updateListDirectionsOfStoryes(listDirectionsOfStories);
        }

        public int GetIdForExistOrCreatedChat(string systemLocale, Language robotLanguage)
        {
            var userLanguage = languageManager.GetItemForShortName(systemLocale);
            Chat chat = chatManager.GetChatByCoupleOfLanguages(userLanguage, robotLanguage);
            if (chat.ID == 0)
            {
                chat = new Chat();
                chat.LanguageFrom = userLanguage.ID;
                chat.LanguageCaptionFrom = userLanguage.NameLocal;
                chat.LanguageTo = robotLanguage.ID;
                chat.LanguageCaptionTo = robotLanguage.NameLocal;
                chat.UpdateDate = DateTime.Now;
                chatManager.SaveItem(chat);
            }
            return chat.ID;
        }
    }
}
