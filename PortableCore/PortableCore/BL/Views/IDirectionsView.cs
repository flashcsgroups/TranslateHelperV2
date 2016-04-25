using PortableCore.BL.Models;
using System.Collections.Generic;
using PortableCore.DL;

namespace PortableCore.BL.Views
{
    public interface IDirectionsView
    {
        void updateListAllLanguages(List<Language> listLanguages);
    }
}