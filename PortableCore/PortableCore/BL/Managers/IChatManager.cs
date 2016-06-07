using System.Collections.Generic;
using PortableCore.BL.Models;
using PortableCore.DL;

namespace PortableCore.BL.Managers
{
    public interface IChatManager
    {
        List<DirectionsRecentItem> GetChatsForLastDays(int countOfDays);
        Chat GetChatByLanguage(Language userLanguage, Language robotLanguage);
        int SaveItem(Chat item);
        Chat GetItemForId(int id);
    }
}