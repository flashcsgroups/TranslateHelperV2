
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
using System.Json;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace TranslateHelper.Droid
{
	[Activity (Label = "Dictionary", Icon = "@drawable/icon", Theme = "@style/MyTheme")]
	public class DictionaryActivity : Activity
	{

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			//base.ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
			base.ActionBar.Hide ();
			SetContentView(Resource.Layout.Dictionary);



			EditText editSourceText = FindViewById<EditText>(Resource.Id.textSourceString);

			editSourceText.FocusChange += async (object sender, View.FocusChangeEventArgs e) => {
				{
					string resultString = string.Empty;
					try
					{
						JsonValue json = await TranslateWordAsync (editSourceText.Text);
						resultString = json ["text"].ToString ();
					}
					catch
					{
						//todo: добавить логгирование
					}

					UpdateListResults(resultString);
				}
			};

			Button buttonTranslate = FindViewById<Button>(Resource.Id.buttonTranslate);
			buttonTranslate.Click += async (object sender, EventArgs e) => {
				{
					string resultString = string.Empty;
					try
					{
						JsonValue json = await TranslateWordAsync (editSourceText.Text);
						resultString = json ["text"].ToString ();
					}
					catch
					{
						//todo: добавить логгирование
					}

					UpdateListResults(resultString);
				}
			};
			editSourceText.KeyPress  += async (object sender, View.KeyEventArgs e) => {
				e.Handled = false;
				if(e.Event.Action == KeyEventActions.Down)
				{
					if((e.KeyCode == Keycode.Space)||(e.KeyCode==Keycode.Enter))
					{
						e.Handled = true;
						JsonValue json = await TranslateWordAsync (editSourceText.Text.Trim());
						UpdateListResults(json ["text"].ToString ());
					}
				}
			};

			ListView resultListView = FindViewById<ListView>(Resource.Id.listResultListView);
			resultListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
				string item = (string)resultListView.GetItemAtPosition(e.Position);
				AddToFavorites(item);
			};
		}

		void UpdateListResults(string resultString)
		{
			if(resultString.Contains("["))
				{
					resultString = resultString.Substring (2, resultString.Length - 4);
				}

			var ListResultStrings = new List<string>();
			ListResultStrings.Add(resultString);
			ListView lv = FindViewById<ListView>(Resource.Id.listResultListView);
			lv.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, ListResultStrings.ToArray());

		}

		private async Task<JsonValue> TranslateWordAsync (string inputText)
		{
			string url = string.Format ("https://translate.yandex.net/api/v1.5/tr.json/translate?key=trnsl.1.1.20150918T114904Z.45ab265b9b9ac49d.d4de7a7a003321c5af46dc22110483b086b8125f&text={0}&lang=en-ru&format=plain", inputText);
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (new Uri (url));
			request.Method = "GET";
			using (WebResponse response = await request.GetResponseAsync ()) {
				using (Stream stream = response.GetResponseStream ()) {
					JsonValue jsonDoc = await Task.Run (() => JsonObject.Load (stream));
					return jsonDoc;
				}
			}

		}

		private void AddToFavorites(string originalText)
		{
			Core.ExpressionManager manager = new Core.ExpressionManager ();
			manager.SaveTranslatedWord (FindViewById<EditText> (Resource.Id.textSourceString).Text, originalText);
		}
	}
}

