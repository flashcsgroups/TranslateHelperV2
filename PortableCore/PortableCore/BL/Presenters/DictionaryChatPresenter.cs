using PortableCore.BL.Contracts;
using PortableCore.BL.Managers;
using PortableCore.BL.Models;
using PortableCore.BL.Views;
using PortableCore.DAL;
using PortableCore.DL;
using PortableCore.Helpers;
using PortableCore.WS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableCore.BL.Presenters
{
    public class DictionaryChatPresenter
    {
        IDictionaryChatView view;
        ISQLiteTesting db;
        TranslateDirection direction;
        delegate Task goTranslateRequest(string originalText);
        goTranslateRequest RequestReference;
        //List<BubbleItem> bubbles = new List<BubbleItem>();
        string preparedTextForRequest = string.Empty;


        public DictionaryChatPresenter(IDictionaryChatView view, ISQLiteTesting db)
        {
            this.view = view;
            this.db = db;
            direction = new TranslateDirection(this.db, new DirectionManager(this.db));
            direction.SetDefaultDirection();
            RequestReference = new goTranslateRequest(translateRequest);
        }

        public void UserAddNewTextEvent(string userText)
        {
            preparedTextForRequest = prepareTextForRequest(userText);
            if (!string.IsNullOrEmpty(preparedTextForRequest))
            {
                addToDBUserRequest(preparedTextForRequest);
                addToDBDefaultRobotResponse();
                startRequestWithValidation(preparedTextForRequest);
            }
        }

        private void addToDBDefaultRobotResponse()
        {
            ChatHistory item = new ChatHistory();
            item.UpdateDate = DateTime.Now;
            item.TextFrom = "wait...";
            ChatHistoryManager manager = new ChatHistoryManager(this.db);
            manager.SaveItem(item);
        }

        private void addToDBUserRequest(string userText)
        {
            ChatHistory item = new ChatHistory();
            item.UpdateDate = DateTime.Now;
            item.TextTo = userText;
            ChatHistoryManager manager = new ChatHistoryManager(this.db);
            manager.SaveItem(item);
        }

        private void addToDBRobotResponse(TranslateRequestResult reqResult)
        {
            ChatHistoryManager chatHistoryManager = new ChatHistoryManager(db);
            ChatHistory defaultRobotItem = chatHistoryManager.GetLastRobotMessage();
            defaultRobotItem.UpdateDate = DateTime.Now;
            defaultRobotItem.TextFrom = reqResult.TranslatedData.Definitions[0].TranslateVariants[0].Text;
            chatHistoryManager.SaveItem(defaultRobotItem);
        }

        private async void startRequestWithValidation(string originalText)
        {
            await RequestReference(originalText);
            /*preparedTextForRequest = prepareTextForRequest(originalText);
            if (!string.IsNullOrEmpty(preparedTextForRequest))
            {
                DetectInputLanguage detect = new DetectInputLanguage(originalText);
                DetectInputLanguage.Language result = detect.Detect();
                if ((result != DetectInputLanguage.Language.Unknown) && !direction.IsFrom(result))
                {
                    direction.Invert();
                };
                await RequestReference(preparedTextForRequest);
            }*/
        }

        private string prepareTextForRequest(string originalText)
        {
            return ConvertStrings.StringToOneLowerLineWithTrim(originalText);
        }

        private async Task translateRequest(string originalText)
        {
            if (!string.IsNullOrEmpty(originalText))
            {
                TranslateRequestRunner reqRunner = getRequestRunner(direction);
                TranslateRequestResult reqResult = await reqRunner.GetDictionaryResult(originalText, direction);
                if (string.IsNullOrEmpty(reqResult.errorDescription) && (reqResult.TranslatedData.Definitions.Count == 0))
                {
                    reqResult = await reqRunner.GetTranslationResult(originalText, direction);
                }

                if (string.IsNullOrEmpty(reqResult.errorDescription))
                {
                    addToDBRobotResponse(reqResult);
                    var bubbles = getListBubbles();
                    view.UpdateChat(bubbles);
                    /*bubbles.Add(new BubbleItem() { IsTheDeviceUser = false, Text = reqResult.OriginalText, UserNameText="MyName" });
                    string res = string.Empty;
                    foreach(var item in reqResult.TranslatedData.Definitions)
                    {
                        foreach(var itemvar in item.TranslateVariants)
                        {
                            res += itemvar.Text + ";";
                        }
                    }
                    bubbles.Add(new BubbleItem() { IsTheDeviceUser = true, Text = res, UserNameText = "TH" });
                    view.UpdateChat(bubbles);*/
                    //updateListResults(reqResult);
                    //TogglesSoftKeyboard.Hide(this);
                }
                else
                {
                    //Toast.MakeText(this, reqResult.errorDescription, ToastLength.Long).Show();
                }
            }
        }

        private List<BubbleItem> getListBubbles()
        {
            List<BubbleItem> resultBubbles = new List<BubbleItem>();
            ChatHistoryManager chatHistoryManager = new ChatHistoryManager(db);
            IEnumerable<ChatHistory> history = chatHistoryManager.ReadChatMessages();
            foreach (var item in history)
            {
                BubbleItem bubble = new BubbleItem();
                bubble.TextTo = item.TextTo;
                bubble.TextFrom = item.TextFrom;
                bubble.IsRobotResponse = string.IsNullOrEmpty(item.TextTo);
                bubble.UserNameText = "username";
                resultBubbles.Add(bubble);
            }
            /*bubbles.Add(new BubbleItem() { IsTheDeviceUser = false, Text = reqResult.OriginalText, UserNameText="MyName" });
            string res = string.Empty;
            foreach(var item in reqResult.TranslatedData.Definitions)
            {
                foreach(var itemvar in item.TranslateVariants)
                {
                    res += itemvar.Text + ";";
                }
            }
            bubbles.Add(new BubbleItem() { IsTheDeviceUser = true, Text = res, UserNameText = "TH" });*/
            return resultBubbles;
        }

        public string GetCurrentDirectionName()
        {
            return direction.GetCurrentDirectionName();
        }

        private TranslateRequestRunner getRequestRunner(TranslateDirection translateDirection)
        {
            TranslateRequestRunner reqRunner = new TranslateRequestRunner(
                db,
                new CachedResultReader(translateDirection, db),
                new TranslateRequest(TypeTranslateServices.YandexDictionary, translateDirection),
                new TranslateRequest(TypeTranslateServices.YandexTranslate, translateDirection));
            return reqRunner;
        }
    }

}
