using System.Collections.Generic;
using PortableCore.BL.Models;
using PortableCore.DL;

namespace PortableCore.BL.Managers
{
    public interface IAnecdoteManager
    {
        //List<DirectionsRecentItem> GetChatsForLastDays(int countOfDays);
        //Chat GetChatByCoupleOfLanguages(Language language1, Language language2);
        int SaveItem(Anecdote item);
        Anecdote GetItemForId(int id);
    }
}