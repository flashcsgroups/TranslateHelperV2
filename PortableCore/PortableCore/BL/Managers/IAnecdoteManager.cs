using System.Collections.Generic;
using PortableCore.BL.Models;
using PortableCore.DL;

namespace PortableCore.BL.Managers
{
    public interface IAnecdoteManager
    {
        List<StoryWithTranslateItem> GetListDirectionsForStories();
        int SaveItem(Anecdote item);
        Anecdote GetItemForId(int id);
    }
}