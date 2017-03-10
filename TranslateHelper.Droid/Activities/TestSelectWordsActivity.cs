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
using PortableCore.BL.Managers;
using PortableCore.BL.Presenters;
using PortableCore.BL.Views;
using PortableCore.BL.Models;

namespace TranslateHelper.Droid.Activities
{
    [Activity(Label = "@string/act_testselectwords_caption", Theme = "@style/MyTheme")]
    public class TestSelectWordsActivity : Activity, ITestSelectWordsView
    {
        //TranslateDirection direction = new TranslateDirection(SqlLiteInstance.DB, new DirectionManager(SqlLiteInstance.DB));
        int countOfSubmitButtons = 8;//количество кнопок с ответами на форме
        TestSelectWordsPresenter presenter;
        private int currentChatId;

        private int lastSubmittedButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);
            SetContentView(Resource.Layout.TestSelectWords);
        }

        protected override void OnStart()
        {
            base.OnStart();
            currentChatId = Intent.GetIntExtra("currentChatId", -1);
            if (currentChatId >= 0)
            {
                int countOfWords = Intent.GetIntExtra("countOfWords", -1);
                presenter = new TestSelectWordsPresenter(this, SqlLiteInstance.DB, new TestSelectWordsReader(SqlLiteInstance.DB), currentChatId, countOfWords);
                presenter.Init();
            }
            else
            {
                throw new Exception("Chat not found");
            }
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            //base.OnSaveInstanceState(outState);
        }

        protected override void OnRestoreInstanceState(Bundle savedState)
        {
            base.OnRestoreInstanceState(savedState);
        }

        private void initSubmitButtons()
        {
            SetButtonNormalState();
            lastSubmittedButton = 0;
            for (int index = 1; index <= countOfSubmitButtons; index++)
            {
                Button submit = getSubmitButtonByName("buttonSubmitTest" + index.ToString());
                submit.Click += Submit_Click;
                submit.Visibility = ViewStates.Invisible;
                submit.SetBackgroundResource(Resource.Drawable.TestScreenButtonSelector);
            }
        }

        private void Submit_Click(object sender, EventArgs e)
        {
            lastSubmittedButton = ((Button)sender).Id;
            presenter.OnSelectVariant(((Button)sender).Text);
        }

        private Button getSubmitButtonByName(string buttonResourceName)
        {
            int res = (int)typeof(Resource.Id).GetField(buttonResourceName).GetValue(null);
            return FindViewById<Button>(res);
        }

        public void SetVariants(List<TestWordItem> variants)
        {
            initSubmitButtons();
            int buttonIndex = 1;
            foreach (var variantItem in variants)
            {
                Button submit = getSubmitButtonByName("buttonSubmitTest" + (buttonIndex).ToString());
                submit.Text = variantItem.TextTo;
                submit.Visibility = ViewStates.Visible;
                //submit.SetBackgroundResource(Resource.Drawable.TestScreenButtonSelector);
                buttonIndex++;
            }
        }

        public void SetOriginalWord(TestWordItem originalWord)
        {
            var textOriginalWord = FindViewById<TextView>(Resource.Id.textOriginalWord);
            textOriginalWord.Text = originalWord.TextFrom;
        }

        public void SetButtonErrorState()
        {
            Button button = FindViewById<Button>(lastSubmittedButton);
            button.SetBackgroundResource(Resource.Drawable.TestScreenButtonSelectorError);
        }
        public void SetButtonNormalState()
        {
            Button button = FindViewById<Button>(lastSubmittedButton);
            if(button!=null)
            {
                button.SetBackgroundResource(Resource.Drawable.TestScreenButtonSelector);
                button.Invalidate();
            }
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
                    backToDictionaryChat();
                    return true;
                default:
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        private void backToDictionaryChat()
        {
            var intent = new Intent(this, typeof(DictionaryChatActivity));
            intent.PutExtra("currentChatId", currentChatId);
            StartActivity(intent);
        }

        public void SetFinalTest(int countOfTestedWords)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle(Resource.String.msg_tests_repeatOrClose);
            alert.SetMessage(Resource.String.msg_final_test);
            alert.SetPositiveButton(Resource.String.msg_repeat, (senderAlert, args) => {
                presenter.Init();
            });
            alert.SetNegativeButton(Resource.String.msg_cancel, (senderAlert, args) => {
                backToDictionaryChat();
            });
            Dialog dialog = alert.Create();
            dialog.Show();
        }
    }
}