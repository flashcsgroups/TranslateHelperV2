
using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using PortableCore.BL.Contracts;
using PortableCore.WS;
using PortableCore.Helpers;
using PortableCore.BL.Managers;
using Droid.Core.Helpers;
using System.Threading.Tasks;

namespace TranslateHelper.Droid
{
    [Activity (Label = "Translate helper", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/MyTheme")]
    public class DictionaryActivity : Activity
	{
        List<TranslateResult> items;

        protected override void OnCreate (Bundle bundle)
		{
            base.OnCreate (bundle);
			base.ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
			SetContentView (Resource.Layout.Dictionary);
			

            EditText editSourceText = FindViewById<EditText> (Resource.Id.textSourceString);
			ImageButton buttonNew = FindViewById<ImageButton> (Resource.Id.buttonNew);
			ImageButton buttonTranslate = FindViewById<ImageButton> (Resource.Id.buttonTranslate);
			ListView resultListView = FindViewById<ListView> (Resource.Id.listResultListView);

            

			buttonNew.Click += (object sender, EventArgs e) => {
				{
					editSourceText.Text = string.Empty;
					clearTraslatedRegion();
				}
			};

            //ToDo:Поправить жесткий копипаст
            buttonTranslate.Click += (object sender, EventArgs e) =>
            {
                //getTranslateResult(editSourceText);
                TranslateResult result = getTranslateResult(editSourceText.Text);
            };

            //ToDo:Поправить жесткий копипаст
			/*editSourceText.TextChanged += async (object sender, Android.Text.TextChangedEventArgs e) => {
                
                if ((editSourceText.Text.Length > 0) && (iSSymbolForStartTranslate (editSourceText.Text.Last ()))) {
                    //ToDo:убрать перевод строки в контроле
                    //string sourceText = editSourceText.Text.Replace('\n', ' ').Trim().ToLower();
                    string sourceText = ConvertStrings.StringToOneLowerLineWithTrim(editSourceText.Text);
                    if (sourceText.Length > 0)
                    {
                        IRequestTranslateString translaterFromCache = new LocalDatabaseCache();
                        var resultFromCache = await translaterFromCache.Translate(sourceText, "en-ru");
                        if (resultFromCache.translateResult.Collection.Count > 0)
                        {
                            UpdateListResults(sourceText, resultFromCache, false);
                        }
                        else
                        {
                            IRequestTranslateString translaterDict = new TranslateRequest(TypeTranslateServices.YandexDictionary);
                            var resultDict = await translaterDict.Translate(sourceText, "en-ru");
                            if (resultDict.translateResult.Collection.Count > 0)
                            {
                                UpdateListResults(sourceText, resultDict, true);
                            }
                            else
                            {
                                IRequestTranslateString translaterTranslate = new TranslateRequest(TypeTranslateServices.YandexTranslate);
                                var resultTrans = await translaterTranslate.Translate(sourceText, "en-ru");
                                UpdateListResults(sourceText, resultTrans, true);
                            }
                        }
                    }
                }
            };*/

			resultListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
                var item = resultListView.GetItemAtPosition (e.Position).Cast<TranslateResult>();
                addToFavorites(ConvertStrings.StringToOneLowerLineWithTrim(editSourceText.Text), item);
			};

			clearTraslatedRegion ();
		}

        private TranslateResult getTranslateResult(string originalText, string direction)
        {
            string convertedSourceText = ConvertStrings.StringToOneLowerLineWithTrim(originalText);
            if (convertedSourceText.Length > 0)
            {
                IRequestTranslateString translaterFromCache = new LocalDatabaseCache();
                var resultFromCache = translaterFromCache.Translate(originalText, direction);
                if (resultFromCache..translateResult.Collection.Count > 0)
                {
                    updateListResults(sourceText, resultFromCache, false);
                }
                else
                {
                    /*IRequestTranslateString translaterDict = new TranslateRequest(TypeTranslateServices.YandexDictionary);
                    var resultDict = await translaterDict.Translate(sourceText, direction);
                    if (resultDict.translateResult.Collection.Count > 0)
                    {
                        updateListResults(sourceText, resultDict, true);
                    }
                    else
                    {
                        IRequestTranslateString translaterTranslate = new TranslateRequest(TypeTranslateServices.YandexTranslate);
                        var resultTrans = await translaterTranslate.Translate(sourceText, direction);
                        updateListResults(sourceText, resultTrans, true);
                    }*/
                }
            }
            throw new NotImplementedException();
        }

        private async Task getTranslateResultOld(EditText editSourceText)
        {
            string sourceText = ConvertStrings.StringToOneLowerLineWithTrim(editSourceText.Text);
            if (sourceText.Length > 0)
            {
                IRequestTranslateString translaterFromCache = new LocalDatabaseCache();
                var resultFromCache = await translaterFromCache.Translate(sourceText, "en-ru");
                if (resultFromCache.translateResult.Collection.Count > 0)
                {
                    updateListResults(sourceText, resultFromCache, false);
                }
                else
                {
                    IRequestTranslateString translaterDict = new TranslateRequest(TypeTranslateServices.YandexDictionary);
                    var resultDict = await translaterDict.Translate(sourceText, "en-ru");
                    if (resultDict.translateResult.Collection.Count > 0)
                    {
                        updateListResults(sourceText, resultDict, true);
                    }
                    else
                    {
                        IRequestTranslateString translaterTranslate = new TranslateRequest(TypeTranslateServices.YandexTranslate);
                        var resultTrans = await translaterTranslate.Translate(sourceText, "en-ru");
                        updateListResults(sourceText, resultTrans, true);
                    }
                }
            }
        }

        private void clearTraslatedRegion()
		{
            var ListResultStrings = new List<string>();
            ListView lv = FindViewById<ListView>(Resource.Id.listResultListView);
            lv.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, ListResultStrings.ToArray());
            TextView splash = FindViewById<TextView>(Resource.Id.splashTextView);
            splash.Visibility = ViewStates.Visible;
        }

		private bool iSSymbolForStartTranslate (char p)
		{
			return ((p == ' ') || (p == '\n'));
        }

        //ToDo: разнести по разным методам вывод и запись в кэш
        void updateListResults(string sourceText, TranslateRequestResult result, bool addToLocalCache)
        {
            if (string.IsNullOrEmpty(result.errorDescription))
            {
                if(result.translateResult.Collection.Count > 0)
                {
                    TextView splash = FindViewById<TextView>(Resource.Id.splashTextView);
                    splash.Visibility = ViewStates.Invisible;
                    var listView = FindViewById<ListView>(Resource.Id.listResultListView);
                    items = result.translateResult.Collection;
                    listView.Adapter = new TranslateResultAdapter(this, items);
                    if (addToLocalCache)
                    {
                        addResultToLocalCache(sourceText, result.translateResult.Collection);
                    }
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
        }

        private void addResultToLocalCache(string sourceText, List<TranslateResult> resultList)
        {
            if(resultList.Count > 0)
            {
                TranslatedExpressionManager manager = new TranslatedExpressionManager();
                //ToDo:передавать дерево результатов
                manager.AddNewWord(sourceText, resultList);
            }
        }

        private async void addToFavorites(string sourceText, TranslateResult result)
        {
            FavoritesManager favoritesManager = new FavoritesManager();
            favoritesManager.AddWordToFavorites(sourceText, result);
            Android.Widget.Toast.MakeText(this, "Элемент добавлен в избранное", Android.Widget.ToastLength.Short).Show();

            IRequestTranslateString translaterFromCache = new LocalDatabaseCache();
            EditText editSourceText = FindViewById<EditText>(Resource.Id.textSourceString);
            var resultFromCache = await translaterFromCache.Translate(sourceText, "en-ru");
            if (resultFromCache.translateResult.Collection.Count > 0)
            {
                updateListResults(sourceText, resultFromCache, false);
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_DictionaryScreen, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_favorites:
                    StartActivity(typeof(FavoritesActivity));
                    return true;
                case Resource.Id.menu_testing:
                    //StartActivity(typeof(FavoritesActivity));
                    return true;
                case Resource.Id.menu_settings:
                    StartActivity(typeof(SettingsActivity));
                    return true;
                case global::Android.Resource.Id.Home:
                    StartActivity(typeof(SettingsActivity));
                    return true;
                default:
                    break;
            }
            return true;
        }

        void menuItemClicked(string item)
        {
            Console.WriteLine(item + " option menuitem clicked");
            var t = Toast.MakeText(this, "Options Menu '" + item + "' clicked", ToastLength.Short);
            t.SetGravity(GravityFlags.Center, 0, 0);
            t.Show();
        }

    }
}

