﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Droid.Core.Helpers;
using PortableCore.BL;
using PortableCore.BL.Models;
using PortableCore.BL.Presenters;
using PortableCore.BL.Views;
using PortableCore.DAL;
using TranslateHelper.Droid.Adapters;
using PortableCore.BL.Contracts;
using System.Globalization;

namespace TranslateHelper.Droid.Activities
{
    [Activity(Label = "@string/act_idioms_caption", Theme = "@style/MyTheme")]
    public class IdiomsActivity : Activity, IIdiomsView
    {
        IdiomsPresenter presenter;
        private int languageFromId;
        private int languageToId;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            HockeyAppMetricsHelper.Register(Application);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);
            SetContentView(Resource.Layout.Idioms);
            HockeyAppMetricsHelper.TrackEvent("Open idioms");
            ImageButton buttonSearch = FindViewById<ImageButton>(Resource.Id.ibSearchIdiom);
            buttonSearch.Click += (object sender, EventArgs e) =>
            {
                EditText editTextSearch = FindViewById<EditText>(Resource.Id.etSearchIdiom);
                var founded = presenter.Search(editTextSearch.Text);
            };

            //http://polyidioms.narod.ru/index/0-128
            //http://catchenglish.ru/frazy-i-vyrazheniya.html
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case global::Android.Resource.Id.Home:
                    var intent = new Intent(this, typeof(DirectionsActivity));
                    intent.AddFlags(ActivityFlags.ClearTop);
                    StartActivity(intent);
                    return true;
                default:
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }
        protected override void OnStart()
        {
            base.OnStart();
            languageFromId = Intent.GetIntExtra("languageFromId", -1);
            languageToId = Intent.GetIntExtra("languageToId", -1);
            if ((languageFromId >= 0) && (languageToId >= 0))
            {
                presenter = new IdiomsPresenter(this, SqlLiteInstance.DB, languageFromId, languageToId);
                presenter.Init();
            }
            else
            {
                throw new Exception("Language not found");
            }
        }

        public void UpdateList(IndexedCollection<IdiomItem> list)
        {
            var adapter = CreateAdapter(list.GetSortedData());
            var listView = FindViewById<ListView>(Resource.Id.listIdiomsListView);
            listView.FastScrollEnabled = true;
            listView.Adapter = adapter;
            listView.ItemClick += ListView_ItemClick;
        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var viewParent = e.View;
            var translatedTextView = ((View)viewParent).FindViewById<TextView>(Resource.Id.IdiomsTextToTextView);
            if (translatedTextView.Visibility == ViewStates.Gone)
            {
                translatedTextView.Visibility = ViewStates.Visible;
            }
            else
            {
                translatedTextView.Visibility = ViewStates.Gone;
            }
        }

        IdiomsAdapter CreateAdapter<T>(Dictionary<string, List<T>> sortedObjects) where T : IHasLabel, IComparable<T>
        {
            var adapter = new IdiomsAdapter(this);
            foreach (var e in sortedObjects.OrderBy(de => de.Key))
            {
                var section = e.Value;
                var label = e.Key.Trim().Length > 0 ? e.Key.ToUpper(CultureInfo.CurrentCulture) : "Error:" + " (" + section.Count.ToString() + ")";
                adapter.AddSection(label, new ArrayAdapter<T>(this, Resource.Layout.IdiomsListItem, Resource.Id.IdiomsTextFromTextView, section));
            }
            return adapter;
        }
    }
}