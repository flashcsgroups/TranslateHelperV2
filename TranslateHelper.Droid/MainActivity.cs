using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Org.Json;

namespace TranslateHelper.Droid
{
	[Activity (Label = "TranslateHelper.Droid", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		int count = 1;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button> (Resource.Id.myButton);
			button.Click += delegate {
				//button.Text = string.Format ("{0} clicks!", count++);
				button.Text = await TranslateWordAsync("test");
			};
		}

		private async Task<string> TranslateWordAsync(string inputText)
		{
			//http://developer.xamarin.com/recipes/android/web_services/consuming_services/call_a_rest_web_service/
			//https://tech.yandex.ru/translate/doc/dg/reference/translate-docpage/
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (new Uri(string.Format("https://translate.yandex.net/api/v1.5/tr.json/translate?key=trnsl.1.1.20150918T114904Z.45ab265b9b9ac49d.d4de7a7a003321c5af46dc22110483b086b8125f&text=%1&lang=en-ru&format=plain", inputText)));
			request.Method = "GET";
			using (WebResponse response = await request.GetResponseAsync ()) 
			{
				using (Stream stream = response.GetResponseStream ()) 
				{
					JsonValue jsonDoc =
					//string result = await Task.Run(()=>string.Load
				}
			}
		}

	}
}


