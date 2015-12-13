using System;

using System.IO;
using System.Json;
using System.Net;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Org.Json;
using Core = TranslateHelper.Core;

namespace TranslateHelper.Droid
{
	//Theme = "@style/MyTheme"
	[Activity (Label = "TranslateHelper", MainLauncher = false, Icon = "@drawable/icon", Theme = "@style/MyTheme")]
	public class MainActivity : Activity
	{

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			base.ActionBar.NavigationMode = ActionBarNavigationMode.List;
			//base.ActionBar.Hide ();

			InitDbIfRequired ();

			SetContentView (Resource.Layout.Main);

			SetClickEvents ();
        }

		protected override void OnResume ()
		{
			base.OnResume ();
			InitDbIfRequired ();
		}

		void SetClickEvents ()
		{
			ImageButton editProjectButton = FindViewById<ImageButton> (Resource.Id.buttonDictionary);
			editProjectButton.Click += delegate {
				StartActivity (typeof(DictionaryActivity));
			};
			ImageButton favoritesButton = FindViewById<ImageButton> (Resource.Id.buttonFavorites);
			favoritesButton.Click += delegate {
				StartActivity (typeof(FavoritesActivity));
			};
			ImageButton settingsButton = FindViewById<ImageButton> (Resource.Id.buttonSettings);
			settingsButton.Click += delegate {
				StartActivity (typeof(SettingsActivity));
			};
		}

		void InitDbIfRequired ()
		{
			Core.TranslateProviderManager managerProvider = new Core.TranslateProviderManager ();
			managerProvider.InitDefaultData ();

            Core.DefinitionTypesManager managerTypes = new Core.DefinitionTypesManager();
			managerTypes.InitDefaultData ();
        }

        //http://petrnohejl.github.io/Android-Cheatsheet-For-Graphic-Designers/
        /*private async Task<JsonValue> TranslateWordAsync (string inputText)
		{
			//http://developer.xamarin.com/recipes/android/web_services/consuming_services/call_a_rest_web_service/
			//https://tech.yandex.ru/translate/doc/dg/reference/translate-docpage/
			string url = string.Format ("https://translate.yandex.net/api/v1.5/tr.json/translate?key=trnsl.1.1.20150918T114904Z.45ab265b9b9ac49d.d4de7a7a003321c5af46dc22110483b086b8125f&text={0}&lang=en-ru&format=plain", inputText);
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (new Uri (url));
			request.Method = "GET";
			using (WebResponse response = await request.GetResponseAsync ()) {
				using (Stream stream = response.GetResponseStream ()) {
					//JsonObject test = new JsonObject ();
					JsonValue jsonDoc = await Task.Run (() => JsonObject.Load (stream));
					//string result = await Task.Run(()=>string.Load
					return jsonDoc;
				}
			}
		}*/

    }
}


