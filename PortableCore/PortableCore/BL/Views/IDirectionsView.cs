using PortableCore.BL.Models;
using System.Collections.Generic;
using PortableCore.DL;
using System;
using PortableCore.BL.Managers;

namespace PortableCore.BL.Views
{
    public interface IDirectionsView
    {
        void updateListAllLanguages(List<Language> listLanguages);
        void updateListRecentDirections(List<DirectionsRecentItem> listDirectionsRecent);
        void updateListDirectionsOfStoryes(List<StoryWithTranslateItem> listDirectionsOfStories);
    }
}