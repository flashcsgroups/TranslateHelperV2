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
        IChatManager chatManager;
        ILanguageManager languageManager;
        IChatHistoryManager chatHistoryManager;
        TranslateDirection direction;
        delegate Task goTranslateRequest(string originalText);
        goTranslateRequest requestReference;
        string preparedTextForRequest = string.Empty;

        Chat selectedChat;
        int selectedChatID = 0;


        /// <summary>
        /// Основной конструктор
        /// </summary>
        /// <param name="view"></param>
        /// <param name="db"></param>
        /// <param name="selectedChatID"></param>
        public DictionaryChatPresenter(IDictionaryChatView view, ISQLiteTesting db, int selectedChatID)
        {
            this.view = view;
            this.db = db;
            this.selectedChatID = selectedChatID;
            LanguageManager languageManager = new LanguageManager(db);
            this.chatHistoryManager = new ChatHistoryManager(db);
            this.chatManager = new ChatManager(db, languageManager, chatHistoryManager);
            this.languageManager = new LanguageManager(db);
        }

        /// <summary>
        /// Отдельный конструктор для тестирования
        /// </summary>
        /// <param name="view"></param>
        /// <param name="db"></param>
        /// <param name="selectedChatID"></param>
        /// <param name="chatManager"></param>
        public DictionaryChatPresenter(IDictionaryChatView view, ISQLiteTesting db, int selectedChatID, IChatManager chatManager, ILanguageManager languageManager, IChatHistoryManager chatHistoryManager)
        {
            this.view = view;
            this.db = db;
            this.selectedChatID = selectedChatID;
            this.chatManager = chatManager;
            this.languageManager = languageManager;
            this.chatHistoryManager = chatHistoryManager;
        }

        public void InitChat()
        {
            requestReference = new goTranslateRequest(translateRequest);
            view.UpdateChat(getListBubbles());
        }

        public void InitDirection()
        {
            this.selectedChat = chatManager.GetItemForId(selectedChatID);
            Language userLang = languageManager.GetItemForId(this.selectedChat.LanguageFrom);
            Language robotLang = languageManager.GetItemForId(this.selectedChat.LanguageTo);
            direction = new TranslateDirection(this.db, new DirectionManager(this.db), languageManager);
            direction.SetDirection(userLang, robotLang);
        }

        public void UserAddNewTextEvent(string userText)
        {
            preparedTextForRequest = prepareTextForRequest(userText);
            if (!string.IsNullOrEmpty(preparedTextForRequest))
            {
                invertDirectionOfNeed(preparedTextForRequest);
                addToChatHistory(false, preparedTextForRequest);
                addToChatHistory(true);
                view.UpdateChat(getListBubbles());
                startRequestWithValidation(preparedTextForRequest);
            }
        }

        public void InvertFavoriteState(BubbleItem bubbleItem)
        {
            var item = chatHistoryManager.GetItemForId(bubbleItem.HistoryRowId);
            if(item != null)
            {
                item.InFavorites = !item.InFavorites;
                int result = chatHistoryManager.SaveItem(item);
            }
        }

        private void addToChatHistory(bool useDefaultWaitMessage, string userText = null)
        {
            ChatHistory item = new ChatHistory();
            item.ChatID = selectedChatID;
            item.UpdateDate = DateTime.Now;
            if(useDefaultWaitMessage)
                item.TextTo = useDefaultWaitMessage ? "Роюсь в словаре..." : string.Empty;
            else
            {
                item.TextFrom = !string.IsNullOrEmpty(userText) ? userText : string.Empty;
            }
            item.LanguageFrom = direction.LanguageFrom.ID;
            item.LanguageTo = direction.LanguageTo.ID;
            chatHistoryManager.SaveItem(item);
            increaseChatUpdateDate(item.ChatID);
        }

        private void increaseChatUpdateDate(int chatID)
        {
            var item = chatManager.GetItemForId(chatID);
            if(item != null)
            {
                item.UpdateDate = DateTime.Now;
                chatManager.SaveItem(item);
            }
        }

        private void addToDBRobotResponse(TranslateRequestResult reqResult)
        {
            ChatHistory defaultRobotItem = chatHistoryManager.GetLastRobotMessage();
            defaultRobotItem.UpdateDate = DateTime.Now;
            defaultRobotItem.TextFrom = string.Empty;
            string delimiter = ", ";
            createDBItemsFromResponse(reqResult, chatHistoryManager, defaultRobotItem, delimiter);
            increaseChatUpdateDate(defaultRobotItem.ChatID);
        }

        private void createDBItemsFromResponse(TranslateRequestResult reqResult, IChatHistoryManager chatHistoryManager, ChatHistory defaultRobotItem, string delimiter)
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
                robotItem.TextTo = builderTextFrom.ToString().Remove(builderTextFrom.Length - delimiter.Length, delimiter.Length);
                robotItem.ChatID = defaultRobotItem.ChatID;
                robotItem.LanguageFrom = defaultRobotItem.LanguageFrom;
                robotItem.LanguageTo = defaultRobotItem.LanguageTo;
                var result = chatHistoryManager.SaveItem(robotItem);
                robotItem = new ChatHistory();
            }
        }

        public void DeleteBubbleFromChat(BubbleItem bubbleItem)
        {
            chatHistoryManager.DeleteItemById(bubbleItem.HistoryRowId);
            view.UpdateChat(getListBubbles());
        }

        private async Task startRequestWithValidation(string preparedTextForRequest)
        {
            if(requestReference!=null)
                await requestReference(preparedTextForRequest);
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
            IEnumerable<ChatHistory> history = chatHistoryManager.ReadChatMessages(selectedChat);
            foreach (var item in history)
            {
                BubbleItem bubble = new BubbleItem();
                bubble.HistoryRowId = item.ID;
                bubble.TextTo = item.TextTo;
                bubble.TextFrom = item.TextFrom;
                bubble.IsRobotResponse = string.IsNullOrEmpty(item.TextFrom);
                bubble.Transcription = item.Transcription;
                bubble.Definition = item.Definition;
                bubble.LanguageTo = languagesList.FirstOrDefault(t => t.ID == item.LanguageTo);
                bubble.LanguageFrom = languagesList.FirstOrDefault(t => t.ID == item.LanguageFrom);
                bubble.InFavorites = item.InFavorites;
                resultBubbles.Add(bubble);
            }
            return resultBubbles;
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
