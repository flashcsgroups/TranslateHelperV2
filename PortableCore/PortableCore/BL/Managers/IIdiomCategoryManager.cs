using System.Collections.Generic;
using PortableCore.BL.Models;
using PortableCore.DL;

namespace PortableCore.BL.Managers
{
    public interface IIdiomCategoryManager
    {
        int SaveItem(IdiomCategory item);
        IdiomCategory GetItemForId(int id);
    }
}