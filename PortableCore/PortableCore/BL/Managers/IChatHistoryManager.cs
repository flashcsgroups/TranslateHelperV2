using System.Collections.Generic;
using PortableCore.DL;

namespace PortableCore.BL.Managers
{
    public interface IChatHistoryManager
    {
        int GetCountOfMessagesForChat(int chatId);
        int SaveItem(ChatHistory item);
        ChatHistory GetLastRobotMessage();
        void DeleteItemById(int historyRowId);
        List<ChatHistory> ReadChatMessages(Chat chatItem);
    }
}