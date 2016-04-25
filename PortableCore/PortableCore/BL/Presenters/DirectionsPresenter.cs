using PortableCore.BL.Managers;
using PortableCore.BL.Views;
using PortableCore.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableCore.BL.Presenters
{
    public class DirectionsPresenter
    {
        IDirectionsView view;
        ISQLiteTesting db;
        List<Language> listLanguages = new List<Language>();

        public DirectionsPresenter(IDirectionsView view, ISQLiteTesting db)
        {
            this.view = view;
            this.db = db;
        }

        public void SelectedRecentLanguagesEvent()
        {
            //throw new NotImplementedException();
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
    }
}
