﻿
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
using TranslateHelper.Core;
using TranslateHelper.Core.Helpers;

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
            buttonTranslate.Click += async (object sender, EventArgs e) =>
            {
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
            };

            //ToDo:Поправить жесткий копипаст
			editSourceText.TextChanged += async (object sender, Android.Text.TextChangedEventArgs e) => {
                
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
            };

			resultListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
                var item = resultListView.GetItemAtPosition (e.Position).Cast<TranslateResult>();
                AddToFavorites(ConvertStrings.StringToOneLowerLineWithTrim(editSourceText.Text), item);
			};

			clearTraslatedRegion ();
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
        void UpdateListResults(string sourceText, TranslateRequestResult result, bool addToLocalCache)
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
                        AddResultToLocalCache(sourceText, result.translateResult.Collection);
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

        private void AddResultToLocalCache(string sourceText, List<TranslateResult> resultList)
        {
            if(resultList.Count > 0)
            {
                Core.TranslatedExpressionManager manager = new Core.TranslatedExpressionManager();
                //ToDo:передавать дерево результатов
                manager.AddNewWord(sourceText, resultList);
            }
        }

        private async void AddToFavorites(string sourceText, TranslateResult result)
        {
            Core.SourceExpressionManager sourceExprManager = new SourceExpressionManager();
            IEnumerable<SourceExpression> sourceEnumerator = sourceExprManager.GetItemsForText(sourceText);
            List<SourceExpression> listSourceExpr = sourceEnumerator.ToList<SourceExpression>();
            if (listSourceExpr.Count > 0)
            {
                int sourceId = listSourceExpr[0].ID;
                Core.TranslatedExpressionManager transExprManager = new Core.TranslatedExpressionManager();
                IEnumerable<TranslatedExpression> transEnumerator = transExprManager.GetTranslateResultFromLocalCache(sourceId);
                var transExprItem = transEnumerator.Where(item => item.TranslatedText == result.TranslatedText).Single<TranslatedExpression>();
                Core.FavoritesManager favoritesManager = new FavoritesManager();
                favoritesManager.AddNewWord(transExprItem.ID);
                Android.Widget.Toast.MakeText(this, "Элемент добавлен в избранное", Android.Widget.ToastLength.Short).Show();

                IRequestTranslateString translaterFromCache = new LocalDatabaseCache();
                EditText editSourceText = FindViewById<EditText>(Resource.Id.textSourceString);
                var resultFromCache = await translaterFromCache.Translate(sourceText, "en-ru");
                if (resultFromCache.translateResult.Collection.Count > 0)
                {
                    UpdateListResults(sourceText, resultFromCache, false);
                }

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

        void MenuItemClicked(string item)
        {
            Console.WriteLine(item + " option menuitem clicked");
            var t = Toast.MakeText(this, "Options Menu '" + item + "' clicked", ToastLength.Short);
            t.SetGravity(GravityFlags.Center, 0, 0);
            t.Show();
        }

    }
}

