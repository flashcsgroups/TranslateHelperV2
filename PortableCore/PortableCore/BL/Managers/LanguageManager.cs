﻿using PortableCore.DL;
using PortableCore.BL.Contracts;
using PortableCore.DAL;
using System.Linq;
using System;

namespace PortableCore.BL.Managers
{
    public class LanguageManager : IInitDataTable<Language>, ILanguageManager
    {
        ISQLiteTesting db;

        public LanguageManager(ISQLiteTesting dbHelper)
        {
            db = dbHelper;
        }

		public void InitDefaultData ()
		{
            Repository<Language> repos = new Repository<Language>();
            Language[] data = GetDefaultData ();
            if(repos.Count() != data.Length)
            {
                repos.DeleteAllDataInTable();
                repos.AddItemsInTransaction(data);
            }			
		}

        public Language GetItemForId(int Id)
        {
            Repository<Language> repos = new Repository<Language>();
            Language result = repos.GetItem(Id);
            return result;
        }

        public Language GetItemForNameEng(string name)
        {
            Language result = new Language();
            Repository<Language> repos = new Repository<Language>();
            var view = from item in db.Table<Language>() where item.NameEng == name && item.DeleteMark == 0 select item;
            if (view.Count() == 1) result = view.First(); 
            return result;
        }

        public Language[] GetDefaultData()
		{
            Language[] directionList = new Language[] {
                new Language (){ ID=1, NameEng = "Russian", NameShort="ru", NameLocal = "Русский", NameImageResource="FlagRussia"},
                new Language (){ ID=2, NameEng = "English", NameShort="en", NameLocal = "English", NameImageResource="FlagEnglish"},
                new Language (){ ID=3, NameEng = "France", NameShort="fr", NameLocal = "Français", NameImageResource="FlagFrance"},
                new Language (){ ID=4, NameEng = "Spain", NameShort="es", NameLocal = "Español", NameImageResource="FlagSpain"},
                new Language (){ ID=5, NameEng = "Germany", NameShort="de", NameLocal = "Deutsch", NameImageResource="FlagGermany"},
                new Language (){ ID=6, NameEng = "Italian", NameShort="it", NameLocal = "Italiano", NameImageResource="FlagItaly"},
            };

			return directionList;
		}
    }
}

