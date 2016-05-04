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
            view.UpdateChat(getListBubbles());
        }

        public void UserAddNewTextEvent(string userText)
        {
            preparedTextForRequest = prepareTextForRequest(userText);
            if (!string.IsNullOrEmpty(preparedTextForRequest))
            {
                addToDBUserRequest(preparedTextForRequest);
                addToDBDefaultRobotResponse();
                view.UpdateChat(getListBubbles());
                startRequestWithValidation(preparedTextForRequest);
            }
        }

        private void addToDBDefaultRobotResponse()
        {
            ChatHistory item = new ChatHistory();
            item.UpdateDate = DateTime.Now;
            item.TextFrom = "Роюсь в словаре...";
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
            defaultRobotItem.TextFrom = string.Empty;
            string delimiter = ", ";
            createDBItemsFromResponse(reqResult, chatHistoryManager, defaultRobotItem, delimiter);
        }

        private void createDBItemsFromResponse(TranslateRequestResult reqResult, ChatHistoryManager chatHistoryManager, ChatHistory defaultRobotItem, string delimiter)
        {
            var robotItem = defaultRobotItem;
            foreach (var definition in reqResult.TranslatedData.Definitions)
            {
                if (!string.IsNullOrEmpty(definition.Transcription)) robotItem.Transcription = string.Format("[{0}]", definition.Transcription);
                if (definition.Pos != DefinitionTypesEnum.translater) robotItem.Definition = definition.Pos.ToString();
                StringBuilder builderTextFrom = new StringBuilder();
                foreach (var textVariant in definition.TranslateVariants)
                {
                    builderTextFrom.Append(textVariant.Text);
                    builderTextFrom.Append(delimiter);
                }
                robotItem.TextFrom = builderTextFrom.ToString().Remove(builderTextFrom.Length - delimiter.Length, delimiter.Length);
                chatHistoryManager.SaveItem(robotItem);
                robotItem = new ChatHistory();
            }
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
                    //Task.Delay(1000).Wait();
                    addToDBRobotResponse(reqResult);
                    view.UpdateChat(getListBubbles());
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
                bubble.Transcription = item.Transcription;
                bubble.Definition = item.Definition;
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
