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
using PortableCore.BL.Presenters;
using PortableCore.DAL;
using PortableCore.BL.Views;
using PortableCore.BL;
using PortableCore.BL.Models;
using PortableCore.BL.Contracts;
using System.Globalization;
using TranslateHelper.Droid.Adapters;
using Droid.Core.Helpers;

namespace TranslateHelper.Droid.Activities
{
    [Activity(Label = "@string/act_anecdotes_caption", Theme = "@style/MyTheme")]
    public class AnecdotesActivity : Activity, IAnecdotesView
    {
        AnecdotesPresenter presenter;
        private int languageFromId;
        private int languageToId;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            HockeyAppMetricsHelper.Register(Application);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);
            SetContentView(Resource.Layout.Anecdotes);
            HockeyAppMetricsHelper.TrackEvent("Open anecdotes");
        }
        protected override void OnStart()
        {
            base.OnStart();
            languageFromId = Intent.GetIntExtra("languageFromId", -1);
            languageToId = Intent.GetIntExtra("languageToId", -1);
            if ((languageFromId >= 0)&&(languageToId >= 0))
            {
                presenter = new AnecdotesPresenter(this, SqlLiteInstance.DB, languageFromId, languageToId);
                presenter.Init();
            }
            else
            {
                throw new Exception("Chat not found");
            }
        }
        public void UpdateList(IndexedCollection<AnecdoteItem> list)
        {
            var adapter = CreateAdapter(list.GetSortedData());
            var listView = FindViewById<ListView>(Resource.Id.listAnecdotesListView);
            listView.FastScrollEnabled = true;
            listView.Adapter = adapter;
            listView.ItemClick += ListView_ItemClick;
        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var viewParent = e.View;
            var translatedTextView = ((View)viewParent).FindViewById<TextView>(Resource.Id.AnecdoteTranslatedTextView);
            if (translatedTextView.Visibility == ViewStates.Gone)
            {
                translatedTextView.Visibility = ViewStates.Visible;
            }
            else
            {
                translatedTextView.Visibility = ViewStates.Gone;
            }
        }

        AnecdotesAdapter CreateAdapter<T>(Dictionary<string, List<T>> sortedObjects) where T : IHasLabel, IComparable<T>
        {
            var adapter = new AnecdotesAdapter(this);
            foreach (var e in sortedObjects.OrderBy(de => de.Key))
            {
                var section = e.Value;
                var label = e.Key.Trim().Length > 0 ? e.Key.ToUpper(CultureInfo.CurrentCulture) : "Error:" + " (" + section.Count.ToString() + ")";
                adapter.AddSection(label, new ArrayAdapter<T>(this, Resource.Layout.AnecdotesSectionListItem, Resource.Id.AnecdoteSourceTextView, section));
            }
            return adapter;
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

    }
}