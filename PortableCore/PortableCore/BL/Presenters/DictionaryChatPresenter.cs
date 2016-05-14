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
        string preparedTextForRequest = string.Empty;

        Chat currentChat;

        public DictionaryChatPresenter(IDictionaryChatView view, ISQLiteTesting db, int selectedChatID)
        {
            this.view = view;
            this.db = db;
            ChatManager chatManager = new ChatManager(db);
            this.currentChat = chatManager.GetItemForId(selectedChatID);
            LanguageManager languageManager = new LanguageManager(db);
            Language userLang = languageManager.GetItemForId(this.currentChat.LanguageFrom);
            Language robotLang = languageManager.GetItemForId(this.currentChat.LanguageTo);

            direction = new TranslateDirection(this.db, new DirectionManager(this.db), languageManager);
            direction.SetDirection(userLang, robotLang);

            RequestReference = new goTranslateRequest(translateRequest);
            view.UpdateChat(getListBubbles());
        }

        public void UserAddNewTextEvent(string userText)
        {
            preparedTextForRequest = prepareTextForRequest(userText);
            if (!string.IsNullOrEmpty(preparedTextForRequest))
            {
                invertDirectionOfNeed(preparedTextForRequest);
                addToDBUserRequest(preparedTextForRequest);
                addToDBDefaultRobotResponse();
                view.UpdateChat(getListBubbles());
                startRequestWithValidation(preparedTextForRequest);
            }
        }

        private void addToDBDefaultRobotResponse()
        {
            ChatHistory item = new ChatHistory();
            item.ChatID = currentChat.ID;
            item.UpdateDate = DateTime.Now;
            item.TextFrom = "Роюсь в словаре...";
            item.LanguageFrom = direction.LanguageFrom.ID;
            item.LanguageTo = direction.LanguageTo.ID;
            ChatHistoryManager manager = new ChatHistoryManager(this.db);
            manager.SaveItem(item);
        }

        private void addToDBUserRequest(string userText)
        {
            ChatHistory item = new ChatHistory();
            item.ChatID = currentChat.ID;
            item.UpdateDate = DateTime.Now;
            item.TextTo = userText;
            item.LanguageFrom = direction.LanguageFrom.ID;
            item.LanguageTo = direction.LanguageTo.ID;
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

        public void DeleteBubbleFromChat(BubbleItem bubbleItem)
        {
            ChatHistoryManager chatHistoryManager = new ChatHistoryManager(db);
            chatHistoryManager.DeleteItemById(bubbleItem.HistoryRowId);
            view.UpdateChat(getListBubbles());
        }

        private async void startRequestWithValidation(string preparedTextForRequest)
        {
            await RequestReference(preparedTextForRequest);
        }

        private void invertDirectionOfNeed(string originalText)
        {
            DetectInputLanguage detect = new DetectInputLanguage(originalText);
            DetectInputLanguage.Language result = detect.Detect();
            if ((result != DetectInputLanguage.Language.Unknown) && !direction.IsFrom(result))
            {
                direction.Invert();
            };
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
                    view.UpdateChat(getListBubbles());
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
            LanguageManager languageManager = new LanguageManager(db);
            var languagesList = languageManager.GetDefaultData();
            List<BubbleItem> resultBubbles = new List<BubbleItem>();
            ChatHistoryManager chatHistoryManager = new ChatHistoryManager(db);
            IEnumerable<ChatHistory> history = chatHistoryManager.ReadChatMessages(currentChat);
            foreach (var item in history)
            {
                BubbleItem bubble = new BubbleItem();
                bubble.HistoryRowId = item.ID;
                bubble.TextTo = item.TextTo;
                bubble.TextFrom = item.TextFrom;
                bubble.IsRobotResponse = string.IsNullOrEmpty(item.TextTo);
                bubble.Transcription = item.Transcription;
                bubble.Definition = item.Definition;
                bubble.LanguageTo = languagesList.FirstOrDefault(t => t.ID == item.LanguageTo);
                bubble.LanguageFrom = languagesList.FirstOrDefault(t => t.ID == item.LanguageFrom);
                resultBubbles.Add(bubble);
            }
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
