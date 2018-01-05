using PortableCore.BL.Models;
using System.Collections.Generic;
using PortableCore.DL;
using System;
using PortableCore.BL.Managers;

namespace PortableCore.BL.Views
{
    public interface IDirectionsView
    {
        void UpdateListAllLanguages(List<Language> listLanguages);
        void UpdateListRecentDirections(List<DirectionsRecentItem> listDirectionsRecent);
        void UpdateListDirectionsOfStoryes(List<DirectionAnecdoteItem> listDirectionsOfStories);
        void UpdateListDirectionsOfIdioms(List<DirectionIdiomItem> listDirectionsOfIdioms);
        void StartChatActivityByChatId(int chatId);
        void SetViewToFullListLanguages();
    }
}