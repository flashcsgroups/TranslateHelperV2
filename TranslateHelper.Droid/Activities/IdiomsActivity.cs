using System;
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
using TranslateHelper.App;

namespace TranslateHelper.Droid.Activities
{
    [Activity(Label = "@string/act_idioms_caption", Theme = "@style/MyTheme")]
    public class IdiomsActivity : Activity, IIdiomsView
    {
        IdiomsPresenter presenter;
        private int languageFromId;
        private int languageToId;
        private TranslateHelperApplication appContext;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            appContext = (TranslateHelperApplication)this.Application;
            base.OnCreate(savedInstanceState);

            HockeyAppMetricsHelper.Register(Application);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);
            SetContentView(Resource.Layout.Idioms);
            HockeyAppMetricsHelper.TrackEvent("Open idioms");
            ImageButton buttonSearch = FindViewById<ImageButton>(Resource.Id.ibSearchIdiom);
            EditText editTextSearch = FindViewById<EditText>(Resource.Id.etSearchIdiom);
            editTextSearch.AfterTextChanged += EditTextSearch_AfterTextChanged;

            buttonSearch.Click += (object sender, EventArgs e) =>
            {
                editTextSearch = FindViewById<EditText>(Resource.Id.etSearchIdiom);
                presenter.RefreshIdiomsList(editTextSearch.Text, false);
            };

            //http://polyidioms.narod.ru/index/0-128
            //http://catchenglish.ru/frazy-i-vyrazheniya.html
        }

        private void EditTextSearch_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
        {
            EditText editTextSearch = FindViewById<EditText>(Resource.Id.etSearchIdiom);
            string newText = editTextSearch.Text;
            if (newText.Length > 0 || newText.Length == 0)
                presenter.RefreshIdiomsList(newText, false);
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
                presenter.CheckServerTablesUpdate(appContext.LastServerCheckUpdateTables);
                appContext.LastServerCheckUpdateTables = DateTime.Now;
            }
            else
            {
                throw new Exception("Language not found");
            }
        }

        public void UpdateList(IndexedCollection<IdiomItem> list, bool updatedFromServer)
        {
            if(list.Count() > 0)
            {
                TextView tvIdiomsNothingFound = FindViewById<TextView>(Resource.Id.tvIdiomsNothingFound);
                if (tvIdiomsNothingFound.Visibility == ViewStates.Visible) tvIdiomsNothingFound.Visibility = ViewStates.Gone;
                ListView listIdiomsListView = FindViewById<ListView>(Resource.Id.listIdiomsListView);
                if (listIdiomsListView.Visibility == ViewStates.Gone) listIdiomsListView.Visibility = ViewStates.Visible;
                var adapter = CreateAdapter(list.GetSortedData());
                var listView = FindViewById<ListView>(Resource.Id.listIdiomsListView);
                listView.FastScrollEnabled = true;
                listView.Adapter = adapter;
                //listView.ItemClick += ListView_ItemClick;
                if(updatedFromServer)
                {
                    Toast.MakeText(this, "Updated!", ToastLength.Long).Show();
                }
            }
            else
            {
                TextView tvIdiomsNothingFound = FindViewById<TextView>(Resource.Id.tvIdiomsNothingFound);
                if (tvIdiomsNothingFound.Visibility == ViewStates.Gone) tvIdiomsNothingFound.Visibility = ViewStates.Visible;
                ListView listIdiomsListView = FindViewById<ListView>(Resource.Id.listIdiomsListView);
                if (listIdiomsListView.Visibility == ViewStates.Visible) listIdiomsListView.Visibility = ViewStates.Gone;
            }
        }

        IdiomsAdapter CreateAdapter<T>(Dictionary<string, List<T>> sortedObjects) where T : IHasLabel, IComparable<T>
        {
            var adapter = new IdiomsAdapter(this);
            foreach (var e in sortedObjects.OrderBy(de => de.Key))
            {
                var section = e.Value;
                var label = e.Key.Trim().Length > 0 ? e.Key : "Error:" + " (" + section.Count.ToString() + ")";
                adapter.AddSection(label, new ArrayAdapter<T>(this, Resource.Layout.IdiomsListItem, Resource.Id.IdiomsTextFromTextView, section));
            }
            return adapter;
        }
    }
}