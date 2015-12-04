
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
using TranslateHelper.Core;

namespace TranslateHelper.Droid
{
	[Activity (Label = "FavoritesActivity", Icon = "@drawable/icon", Theme = "@style/MyTheme")]			
	public class FavoritesActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			base.ActionBar.Hide ();
			SetContentView(Resource.Layout.Favorites);

            // Create your application here
            /*TranslatedExpressionManager expr = new TranslatedExpressionManager();
			var translatedItems = expr.GetTranslatedItems ();
			var ListFavoritesStrings = new List<string>();
			foreach (TranslatedExpression item in translatedItems) {
				ListFavoritesStrings.Add (item.TranslatedText);
			}
			ListView lv = FindViewById<ListView>(Resource.Id.listFavoritesListView);
			lv.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, ListFavoritesStrings.ToArray());*/
            var listView = FindViewById<ListView>(Resource.Id.listFavoritesListView);
            FavoritesManager favManager = new FavoritesManager();
            var items = favManager.GetItems();
            //items = result.translateResult.Collection;
            listView.Adapter = new FavoritesAdapter(this, items);

        }
    }
}

