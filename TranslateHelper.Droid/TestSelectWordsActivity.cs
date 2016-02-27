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
        TranslateDirection direction = new TranslateDirection(SqlLiteInstance.DB);
        int countOfSubmitButtons = 8;//количество кнопок с ответами на форме
        TestSelectWordsPresenter presenter;
        List<Favorites> favoritesList = new List<Favorites>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);
            SetContentView(Resource.Layout.TestSelectWords);
            direction.SetDirection(Intent.GetStringExtra("directionName"));
            int countOfWords = Intent.GetIntExtra("countOfWords", 10);
            initSubmitButtons();
            presenter = new TestSelectWordsPresenter(this, SqlLiteInstance.DB, new TestSelectWordsReader(SqlLiteInstance.DB), direction, countOfWords);
            presenter.StartTest();
        }

        private void initSubmitButtons()
        {
            for (int index = 1; index <= countOfSubmitButtons; index++)
            {
                Button submit = getSubmitButtonByName("buttonSubmitTest" + index.ToString());
                submit.Click += Submit_Click;
                submit.Visibility = ViewStates.Invisible;
            }
        }

        private void Submit_Click(object sender, EventArgs e)
        {
            PressSubmit(((Button)sender).Text);
        }

        private Button getSubmitButtonByName(string buttonResourceName)
        {
            int res = (int)typeof(Resource.Id).GetField(buttonResourceName).GetValue(null);
            return FindViewById<Button>(res);
        }

        public void SetVariants(List<string> variants)
        {
            int buttonIndex = 1;
            foreach (var variantText in variants)
            {
                Button submit = getSubmitButtonByName("buttonSubmitTest" + buttonIndex.ToString());
                submit.Text = variantText;
                submit.Visibility = ViewStates.Visible;
                buttonIndex++;
            }
        }

        public void SetOriginalWord(string originalWord)
        {
            var textOriginalWord = FindViewById<TextView>(Resource.Id.textOriginalWord);
            textOriginalWord.Text = originalWord;
        }

        public void SetCheckError()
        {
            Toast.MakeText(this, Resource.String.msg_error_try_again, ToastLength.Short).Show();
        }

        public void PressSubmit(string answerText)
        {
            presenter.OnSelectVariant(answerText);
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

        public void SetFinalTest(int countOfTestedWords)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle(Resource.String.msg_tests_repeatOrClose);
            alert.SetMessage(Resource.String.msg_final_test);
            alert.SetPositiveButton(Resource.String.msg_repeat, (senderAlert, args) => { presenter.StartTest(); });
            alert.SetNegativeButton(Resource.String.msg_cancel, (senderAlert, args) => {
                StartActivity(typeof(DictionaryActivity));
            });
            Dialog dialog = alert.Create();
            dialog.Show();
        }
    }
}