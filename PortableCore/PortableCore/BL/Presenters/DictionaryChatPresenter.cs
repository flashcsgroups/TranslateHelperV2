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
        delegate Task goTranslateRequest(string originalText, int requestId);
        goTranslateRequest requestReference;
        string preparedTextForRequest = string.Empty;

        Chat selectedChat;
        int selectedChatID = 0;

        public TranslateDirection Direction
        {
            get
            {
                return direction;
            }
        }

        public int currentChatId
        {
            get
            {
                return selectedChatID;
            }
        }


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

        public void InitChat(string currentLocaleShort)
        {
            requestReference = new goTranslateRequest(translateRequest);
            var listBubbles = getListBubbles();
            view.UpdateChat(listBubbles);
            if(listBubbles.Count == 0)
            {
                DictionaryWelcomeMsg welcome = new DictionaryWelcomeMsg(currentLocaleShort);
                addUserMsgToChatHistory(welcome.GetWelcomeMessage());
                UserAddNewTextEvent(welcome.GetExampleMessage());
            }
        }

        public void UpdateOldSuspendedRequests()
        {
            var viewSuspendedMessages = chatHistoryManager.ReadSuspendedChatMessages(selectedChat);
            foreach(var robotMsg in viewSuspendedMessages)
            {
                var userMsg = chatHistoryManager.GetItemForId(robotMsg.ParentRequestID);
                robotMsg.DeleteMark = 1;
                chatHistoryManager.SaveItem(robotMsg);
                userMsg.DeleteMark = 1;
                chatHistoryManager.SaveItem(userMsg);
                UserAddNewTextEvent(userMsg.TextFrom);
            }
        }

        public void InitDirection()
        {
            this.selectedChat = chatManager.GetItemForId(selectedChatID);
            Language userLang = languageManager.GetItemForId(this.selectedChat.LanguageFrom);
            Language robotLang = languageManager.GetItemForId(this.selectedChat.LanguageTo);
            direction = new TranslateDirection(this.db, languageManager);
            direction.SetDirection(userLang, robotLang);
            view.UpdateBackground("back" + robotLang.NameEng);
            if((userLang.NameShort == "ru")|| (robotLang.NameShort == "ru"))
            {
                view.HideButtonForSwapLanguage();
            }
        }

        public void UserAddNewTextEvent(string userText)
        {
            preparedTextForRequest = prepareTextForRequest(userText);
            if (!string.IsNullOrEmpty(preparedTextForRequest))
            {
                invertDirectionIfNeedForRussianLocaleOnly(preparedTextForRequest);
                int requestId = addUserMsgToChatHistory(preparedTextForRequest);
                addRobotMsgToChatHistory(true, string.Empty, requestId);
                view.UpdateChat(getListBubbles());
                startRequestWithValidation(preparedTextForRequest, requestId);
            }
        }

        public void UserSwapDirection()
        {
            Direction.Invert();
        }

        public void InvertFavoriteState(BubbleItem bubbleItem)
        {
            var item = chatHistoryManager.GetItemForId(bubbleItem.HistoryRowId);
            if(item != null)
            {
                item.InFavorites = !item.InFavorites;
                chatHistoryManager.SaveItem(item);
            }
        }

        private int addUserMsgToChatHistory(string userText = null)
        {
            ChatHistory item = new ChatHistory();
            item.ChatID = selectedChatID;
            item.UpdateDate = DateTime.Now;
            item.TextFrom = !string.IsNullOrEmpty(userText) ? userText : string.Empty;
            item.LanguageFrom = Direction.LanguageFrom.ID;
            item.LanguageTo = Direction.LanguageTo.ID;
            chatHistoryManager.SaveItem(item);
            int id = chatHistoryManager.GetMaxItemId();
            increaseChatUpdateDate(item.ChatID);
            return id;
        }
        private void addRobotMsgToChatHistory(bool useDefaultWaitMessage, string robotText, int requestId)
        {
            ChatHistory item = new ChatHistory();
            item.ChatID = selectedChatID;
            item.UpdateDate = DateTime.Now;
            item.TextTo = useDefaultWaitMessage ? chatHistoryManager.GetSearchMessage(Direction.LanguageFrom) : robotText;
            item.LanguageFrom = Direction.LanguageFrom.ID;
            item.LanguageTo = Direction.LanguageTo.ID;
            item.ParentRequestID = requestId;
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

        private void addToDBRobotResponse(TranslateRequestResult reqResult, int requestId)
        {
            ChatHistory defaultRobotItem = chatHistoryManager.GetLastRobotMessage();
            defaultRobotItem.UpdateDate = DateTime.Now;
            defaultRobotItem.TextFrom = string.Empty;
            string delimiter = ", ";
            createDBItemsFromResponse(reqResult, chatHistoryManager, defaultRobotItem, requestId, delimiter);
            increaseChatUpdateDate(defaultRobotItem.ChatID);
        }

        private void createDBItemsFromResponse(TranslateRequestResult reqResult, IChatHistoryManager chatHistoryManager, ChatHistory defaultRobotItem, int requestId, string delimiter)
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
                robotItem.ParentRequestID = requestId;
                robotItem.UpdateDate = DateTime.Now;
                chatHistoryManager.SaveItem(robotItem);
                robotItem = new ChatHistory();
            }
        }

        public void DeleteBubbleFromChat(BubbleItem bubbleItem)
        {
            chatHistoryManager.DeleteItemById(bubbleItem.HistoryRowId);
            view.UpdateChat(getListBubbles());
        }

        private async Task startRequestWithValidation(string preparedTextForRequest, int requestId)
        {
            if(requestReference!=null)
                await requestReference(preparedTextForRequest, requestId);
        }

        private void invertDirectionIfNeedForRussianLocaleOnly(string originalText)
        {
            //NeedInvertDirection(originalText);
            Language rusLang = languageManager.GetItemForNameEng("Russian");
            if ((rusLang.Equals(Direction.LanguageFrom)) || (rusLang.Equals(Direction.LanguageTo)))
            {
                DetectInputLanguage detect = new DetectInputLanguage(originalText, languageManager);
                if (Direction.LanguageFrom.NameShort == "ru")
                {
                    if(detect.Detect().NameShort == "en")
                    {
                        Direction.Invert();
                    }
                } else
                {
                    if (detect.Detect().NameShort == "ru")
                    {
                        Direction.Invert();
                    }
                }
            }
        }

        private string prepareTextForRequest(string originalText)
        {
            return ConvertStrings.StringToOneLowerLineWithTrim(originalText);
        }

        private async Task translateRequest(string originalText, int requestId)
        {
            if (!string.IsNullOrEmpty(originalText))
            {
                TranslateRequestRunner reqRunner = getRequestRunner(Direction, requestId);
                TranslateRequestResult reqResult = await getTranslateResult(originalText, reqRunner);

                if (string.IsNullOrEmpty(reqResult.errorDescription))
                {
                    addToDBRobotResponse(reqResult, requestId);
                    view.UpdateChat(getListBubbles());
                }
                else
                {
                    view.ShowToast(reqResult.errorDescription);
                }
            }
        }

        private async Task<TranslateRequestResult> getTranslateResult(string originalText, TranslateRequestRunner reqRunner)
        {
            TranslateRequestResult reqResult;

            //предполагаю, если уж встретили пробел, то вероятость того, что это не слово, а предложение, больше
            //попытаемся сначала в сервисе перевода перевести и только потом уже пробуем сервис словаря
            if (originalText.Contains(" "))
            {
                reqResult = await reqRunner.GetTranslationResult(originalText, Direction);
                if (string.IsNullOrEmpty(reqResult.errorDescription) && (reqResult.TranslatedData.Definitions.Count == 0))
                {
                    reqResult = await reqRunner.GetDictionaryResult(originalText, Direction);
                }
            }
            else
            {
                reqResult = await reqRunner.GetDictionaryResult(originalText, Direction);
                if (string.IsNullOrEmpty(reqResult.errorDescription) && (reqResult.TranslatedData.Definitions.Count == 0))
                {
                    reqResult = await reqRunner.GetTranslationResult(originalText, Direction);
                }
            }
            return reqResult;
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

        private TranslateRequestRunner getRequestRunner(TranslateDirection translateDirection, int requestId)
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
