
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
using PortableCore.DAL;

namespace TranslateHelper.Droid
{
    [Activity (Label = "Translate helper", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/MyTheme")]
    public class DictionaryActivity : Activity
	{
        List<TranslateResultView> items;

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
                //TranslateResult result = 
                getTranslateResult(editSourceText.Text, "en-ru");
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
                var item = resultListView.GetItemAtPosition (e.Position).Cast<TranslateResultView>();
                addToFavorites(ConvertStrings.StringToOneLowerLineWithTrim(editSourceText.Text), item);
			};

			clearTraslatedRegion ();
		}

        private async void getTranslateResult(string originalText, string direction)
        {
            string convertedSourceText = ConvertStrings.StringToOneLowerLineWithTrim(originalText);
            if (convertedSourceText.Length > 0)
            {
                /*IRequestTranslateString translaterFromCache = new LocalDBCacheReader(SqlLiteInstance.DB);
                var resultFromCache = translaterFromCache.Translate(originalText, direction);
                if (resultFromCache.translateResult.Collection.Count > 0)
                {
                    updateListResults(sourceText, resultFromCache, false);
                }
                else*/
                {
                    IRequestTranslateString translaterDict = new TranslateRequest(TypeTranslateServices.YandexDictionary);
                    var resultDict = await translaterDict.Translate(originalText, direction);
                    if (string.IsNullOrEmpty(resultDict.errorDescription))
                    {
                        updateListResults(originalText, resultDict.TranslatedData, true);
                    }
                    else
                    {
                        Android.Widget.Toast.MakeText(this, "Ошибка подключения к интернет", Android.Widget.ToastLength.Short).Show();
                    }
                    /*else
                    {
                        IRequestTranslateString translaterTranslate = new TranslateRequest(TypeTranslateServices.YandexTranslate);
                        var resultTrans = await translaterTranslate.Translate(sourceText, direction);
                        updateListResults(sourceText, resultTrans, true);
                    }*/
                }
            }
            //throw new NotImplementedException();
        }

        /*private async Task getTranslateResultOld(EditText editSourceText)
        {
            string sourceText = ConvertStrings.StringToOneLowerLineWithTrim(editSourceText.Text);
            if (sourceText.Length > 0)
            {
                IRequestTranslateString translaterFromCache = new LocalDBCacheReader(SqlLiteInstance.DB);
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
        }*/

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
        void updateListResults(string sourceText, TranslateResultView resultView, bool addToLocalCache)
        {
            if(resultView.Definitions.Count > 0)
            {
                TextView splash = FindViewById<TextView>(Resource.Id.splashTextView);
                splash.Visibility = ViewStates.Invisible;
                var listView = FindViewById<ListView>(Resource.Id.listResultListView);
                listView.FastScrollEnabled = true;

                //IndexedCollection<TranslateResultView> translateResultCollection = new IndexedCollection<TranslateResultView>();

                //listView.Adapter = CreateAdapter(translateResultCollection.GetSortedData());

                //var items = result.translateResult.Collection;
                listView.Adapter = new TranslateResultViewHeaderAdapter(this, resultView.Definitions);
                /*if (addToLocalCache)
                {
                    addResultToLocalCache(sourceText, result.translateResult.Collection);
                }*/
            }
            else
            {
                Android.Widget.Toast.MakeText(this, "Неизвестное выражение, проверьте текст на наличие ошибок.", Android.Widget.ToastLength.Long).Show();
            }
        }

        /*TranslateResultAdapter CreateAdapter<T>(Dictionary<string, List<T>> sortedObjects) where T : IHasLabel, IComparable<T>
        {
            var adapter = new TranslateResultAdapter(this);
            foreach (var e in sortedObjects.OrderBy(de => de.Key))
            {
                var section = e.Value;
                var label = e.Key.Trim().Length > 0 ? e.Key.ToUpper() : "Ошибки" + " (" + section.Count.ToString() + ")";
                adapter.AddSection(label, new ArrayAdapter<T>(this, Resource.Layout.FavoritesSectionListItem, Resource.Id.SourceTextView, section));
            }
            return adapter;
        }*/

        private void addResultToLocalCache(string sourceText, List<TranslateResultView> resultList)
        {
            if(resultList.Count > 0)
            {
                TranslatedExpressionManager manager = new TranslatedExpressionManager(SqlLiteInstance.DB);
                //ToDo:передавать дерево результатов
                manager.AddNewWord(sourceText, resultList);
            }
        }

        private async void addToFavorites(string sourceText, TranslateResultView result)
        {
            /*FavoritesManager favoritesManager = new FavoritesManager(SqlLiteInstance.DB);
            favoritesManager.AddWordToFavorites(sourceText, result);
            Android.Widget.Toast.MakeText(this, "Элемент добавлен в избранное", Android.Widget.ToastLength.Short).Show();

            IRequestTranslateString translaterFromCache = new LocalDBCacheReader(SqlLiteInstance.DB);
            EditText editSourceText = FindViewById<EditText>(Resource.Id.textSourceString);
            var resultFromCache = await translaterFromCache.Translate(sourceText, "en-ru");
            if (resultFromCache.translateResult.Collection.Count > 0)
            {
                updateListResults(sourceText, resultFromCache, false);
            }*/
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

