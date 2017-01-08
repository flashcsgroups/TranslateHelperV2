﻿using System.Collections.Generic;
using PortableCore.DL;

namespace PortableCore.BL.Managers
{
    public interface IChatHistoryManager
    {
        int GetCountOfMessagesForChat(int chatId);
        int SaveItem(ChatHistory item);
        ChatHistory GetItemForId(int id);
        ChatHistory GetLastRobotMessage();
        void DeleteItemById(int historyRowId);
        List<ChatHistory> ReadChatMessages(Chat chatItem);
        List<ChatHistory> ReadSuspendedChatMessages(Chat chatItem);
        string GetSearchMessage(Language languageFrom);
    }
}