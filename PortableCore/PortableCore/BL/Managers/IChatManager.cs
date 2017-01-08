using System.Collections.Generic;
using PortableCore.BL.Models;
using PortableCore.DL;

namespace PortableCore.BL.Managers
{
    public interface IChatManager
    {
        List<DirectionsRecentItem> GetChatsForLastDays(int countOfDays);
        Chat GetChatByCoupleOfLanguages(Language language1, Language language2);
        int SaveItem(Chat item);
        Chat GetItemForId(int id);
    }
}