using PortableCore.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortableCore.BL.Views
{
    public interface IAnecdotesView
    {
        void UpdateList(IndexedCollection<AnecdoteItem> list);
    }
}
