﻿using PortableCore.BL.Managers;
using PortableCore.BL.Views;
using PortableCore.DAL;
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
            //ToDo:Убрать зависимость от БД, тогда получится тест написать
            List<Tuple<Language, Language>> listDirections = new List<Tuple<Language, Language>>();
            Repository<Language> repos = new Repository<Language>();
            var viewItems = from item in db.Table<Language>() select item;
            listDirections.Add(new Tuple<Language, Language>(viewItems.ElementAt(0), viewItems.ElementAt(1)));
            listDirections.Add(new Tuple<Language, Language>(viewItems.ElementAt(1), viewItems.ElementAt(0)));
            listDirections.Add(new Tuple<Language, Language>(viewItems.ElementAt(2), viewItems.ElementAt(0)));
            listDirections.Add(new Tuple<Language, Language>(viewItems.ElementAt(0), viewItems.ElementAt(3)));

            view.updateListRecentDirections(listDirections);
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
