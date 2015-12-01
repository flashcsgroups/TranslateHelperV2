
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
using TranslateHelper.Core.WS;
using TranslateHelper.Core.BL.Contracts;

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
			SetContentView (Resource.Layout.Dictionary);



			EditText editSourceText = FindViewById<EditText> (Resource.Id.textSourceString);
			ImageButton buttonNew = FindViewById<ImageButton> (Resource.Id.buttonNew);
			ImageButton buttonNewBottom = FindViewById<ImageButton> (Resource.Id.buttonNewBottom);
			ImageButton buttonTranslate = FindViewById<ImageButton> (Resource.Id.buttonTranslate);
			ListView resultListView = FindViewById<ListView> (Resource.Id.listResultListView);

            

			buttonNew.Click += (object sender, EventArgs e) => {
				{
					editSourceText.Text = string.Empty;
					//UpdateListResults (string.Empty);
					clearTraslatedRegion();
				}
			};
			buttonNewBottom.Click += (object sender, EventArgs e) => {
				{
					editSourceText.Text = string.Empty;
					//UpdateListResults (string.Empty);
					clearTraslatedRegion();
				}
			};
			buttonTranslate.Click += async (object sender, EventArgs e) =>
            {
                IRequestTranslateString translaterDict = new TranslateRequest(TypeTranslateServices.YandexDictionary);
                var resultDict = await translaterDict.Translate(editSourceText.Text, "en-ru");
                if (resultDict.translateResult.Collection.Count > 0)
                {
                    UpdateListResults(resultDict);
                }
                else
                {
                    IRequestTranslateString translaterTranslate = new TranslateRequest(TypeTranslateServices.YandexTranslate);
                    var resultTrans = await translaterTranslate.Translate(editSourceText.Text, "en-ru");
                    UpdateListResults(resultTrans);
                }
            };

			editSourceText.TextChanged += async (object sender, Android.Text.TextChangedEventArgs e) => {
                
                string sourceText = editSourceText.Text;
                if ((sourceText.Length > 0) && (iSSymbolForStartTranslate (sourceText.Last ()))) {
                    IRequestTranslateString translaterDict = new TranslateRequest(TypeTranslateServices.YandexDictionary);
                    var resultDict = await translaterDict.Translate(editSourceText.Text, "en-ru");
                    if (resultDict.translateResult.Collection.Count > 0)
                    {
                        UpdateListResults(resultDict);
                    }
                    else
                    {
                        IRequestTranslateString translaterTranslate = new TranslateRequest(TypeTranslateServices.YandexTranslate);
                        var resultTrans = await translaterTranslate.Translate(editSourceText.Text, "en-ru");
                        UpdateListResults(resultTrans);
                    }
                }
            };

			resultListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
				string item = (string)resultListView.GetItemAtPosition (e.Position);
				AddToFavorites (item);
			};

			clearTraslatedRegion ();
		}

        private void clearTraslatedRegion()
		{
            var ListResultStrings = new List<string>();
            ListView lv = FindViewById<ListView>(Resource.Id.listResultListView);
            lv.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, ListResultStrings.ToArray());
        }

		private bool iSSymbolForStartTranslate (char p)
		{
			return ((p == ' ') || (p == '\n'));
		}

        void UpdateListResults(TranslateRequestResult result)
        {
            if(string.IsNullOrEmpty(result.errorDescription))
            {
                if(result.translateResult.Collection.Count > 0)
                {
                    var ListResultStrings = new List<string>();
                    foreach (var item in result.translateResult.Collection)
                    {
                        string pos = !string.IsNullOrEmpty(item.Pos) ? " ('" + item.Pos + "')": "";
                        ListResultStrings.Add(item.TranslatedText + pos);
                    }

                    ListView lv = FindViewById<ListView>(Resource.Id.listResultListView);
                    lv.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, ListResultStrings.ToArray());
                }
                else
                {
                    Android.Widget.Toast.MakeText(this, "Неизвестное выражение, проверьте текст на наличие ошибок.", Android.Widget.ToastLength.Long).Show();
                }

            }
            else
            {
                Android.Widget.Toast.MakeText(this, "Ошибка подключения к интернет", Android.Widget.ToastLength.Short).Show();
            }
            /*if (resultString.Contains("["))
            {
                resultString = resultString.Substring(2, resultString.Length - 4);
            }

            var ListResultStrings = new List<string>();
            ListResultStrings.Add(resultString);
            ListView lv = FindViewById<ListView>(Resource.Id.listResultListView);
            lv.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, ListResultStrings.ToArray());
            */
        }

        private void AddToFavorites (string originalText)
		{
			Core.ExpressionManager manager = new Core.ExpressionManager ();
			manager.SaveTranslatedWord (FindViewById<EditText> (Resource.Id.textSourceString).Text, originalText);
            Android.Widget.Toast.MakeText(this, "Элемент добавлен в избранное", Android.Widget.ToastLength.Short).Show();
        }
	}
}

