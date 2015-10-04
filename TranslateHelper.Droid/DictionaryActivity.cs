
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
	[Activity (Label = "Dictionary")]			
	public class DictionaryActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			base.ActionBar.Hide ();
			SetContentView(Resource.Layout.Dictionary);

			// Create your application here
			/*ListView listEmployeeListView = FindViewById<ListView>(Resource.Id.listEmployeesListView);
			listEmployeeListView.ItemClick += delegate
			{
				//SetContentView (Resource.Layout.EditProject);
				StartActivity(typeof(EditEmployeeActivity));
			};*/
			//UpdateListResults ();
			EditText editSourceText = FindViewById<EditText>(Resource.Id.textSourceString);
			editSourceText.KeyPress += async (object sender, View.KeyEventArgs e) => {
				e.Handled = false;
				if(e.Event.Action == KeyEventActions.Up && e.KeyCode == Keycode.Enter)
				{
					e.Handled = true;
					JsonValue json = await TranslateWordAsync (editSourceText.Text);
					UpdateListResults(json ["text"].ToString ());
					//button.Text = json ["text"].ToString ();
				}
				//UpdateListResults ();
			};
		}

		void UpdateListResults(string resultString)
		{
			/*var ExistsEmployees = ScrumHelper.BL.Managers.EmployeeManager.GetItems();
		if (ExistsEmployees.Count < 1)
		{
			CreateDefaultData();
			ExistsEmployees = ScrumHelper.BL.Managers.EmployeeManager.GetItems();
		}*/
			var ListResultStrings = new List<string>();
			ListResultStrings.Add(resultString);
			//ListResultStrings.Add("Транзакция");
			//ListResultStrings.Add("Урегулирование спора");
			//ListResultStrings.Add("Ведение");
			ListView lv = FindViewById<ListView>(Resource.Id.listResultListView);
			lv.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, ListResultStrings.ToArray());

		}

		private async Task<JsonValue> TranslateWordAsync (string inputText)
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
		}

	}
}

