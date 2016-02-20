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
using PortableCore.BL;
using PortableCore.DL;
using PortableCore.DAL;

namespace TranslateHelper.Droid
{
    [Activity(Label = "@string/act_testselectwords_caption", Theme = "@style/MyTheme")]
    public class TestSelectWordsActivity : Activity, ITestSelectWordsView
    {
        TestSelectWordsPresenter presenter;
        List<Favorites> favoritesList = new List<Favorites>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);
            SetContentView(Resource.Layout.TestSelectWords);
            int countOfWords = Intent.GetIntExtra("countOfWords", 10);
            presenter = new TestSelectWordsPresenter(this, SqlLiteInstance.DB, countOfWords);
            presenter.StartTest();
            var btnSubmit = FindViewById<Button>(Resource.Id.buttonSubmitTest);
            btnSubmit.Click += (object sender, EventArgs e) => {
                {
                    PressSubmit();
                }
            };
            RadioGroup radioGroup = FindViewById<RadioGroup>(Resource.Id.radioGroup1);
            radioGroup.CheckedChange += RadioGroup_CheckedChange;
            /*TranslateDirection direction = new TranslateDirection(SqlLiteInstance.DB);
            direction.SetDefaultDirection();
            TestSelectWordsReader data = new TestSelectWordsReader(SqlLiteInstance.DB);
            List<Favorites> favoritesList = data.GetRandomFavorites(countOfWords, direction);*/
        }

        private void RadioGroup_CheckedChange(object sender, RadioGroup.CheckedChangeEventArgs e)
        {
            RadioButton radioButton = FindViewById<RadioButton>(e.CheckedId);
            presenter.OnSelectVariant(radioButton.Text);
        }

        public void SetVariants(string variantText)
        {
            RadioButton radioButton = FindViewById<RadioButton>(Resource.Id.radioButton2);
            radioButton.Text = variantText;
        }

        public void SetOriginalWord(string originalWord)
        {
            var textOriginalWord = FindViewById<TextView>(Resource.Id.textOriginalWord);
            textOriginalWord.Text = originalWord;
        }

        public void SetCheckResult(bool success)
        {
            var checkResultText = FindViewById<TextView>(Resource.Id.checkResultText);
            checkResultText.Visibility = success ? ViewStates.Invisible : ViewStates.Visible;
            var buttonSubmitTest = FindViewById<TextView>(Resource.Id.buttonSubmitTest);
            buttonSubmitTest.Visibility = !success ? ViewStates.Invisible : ViewStates.Visible;
        }

        public void PressSubmit()
        {
            /*RadioGroup radioGroup = FindViewById<RadioGroup>(Resource.Id.radioGroup1);
            RadioButton radioButton = FindViewById<RadioButton>(radioGroup.CheckedRadioButtonId);
            string selectedWord = radioButton.Text;*/
            presenter.OnSubmit();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_TestSelectWordsScreen, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case global::Android.Resource.Id.Home:
                    StartActivity(typeof(SelectTestLevelActivity));
                    return true;
                default:
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

    }
}