
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
	[Activity (Label = "FavoritesActivity")]			
	public class FavoritesActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			base.ActionBar.Hide ();
			SetContentView(Resource.Layout.Favorites);

			// Create your application here
			ExpressionManager expr = new ExpressionManager();
			var translatedItems = expr.GetTranslatedItems ();
			var ListFavoritesStrings = new List<string>();
			foreach (TranslatedExpression item in translatedItems) {
				ListFavoritesStrings.Add (item.Value);
			}
			/*ListFavoritesStrings.Add("Транзакция");
			ListFavoritesStrings.Add("Урегулирование спора");
			ListFavoritesStrings.Add("Ведение");*/
			ListView lv = FindViewById<ListView>(Resource.Id.listFavoritesListView);
			lv.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, ListFavoritesStrings.ToArray());
		}
	}
}

