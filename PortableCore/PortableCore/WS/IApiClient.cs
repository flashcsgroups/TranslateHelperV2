using System;
using System.Collections.Generic;
using PortableCore.DL;
using System.Threading.Tasks;

namespace PortableCore.WS
{
    public interface IApiClient
    {
        Task<List<int>> GetChangedIDsFromServer(string tableName, DateTime localMaxTimeStampString);
        Task<List<Idiom>> GetIdiomsFromServer(List<int> iDs);
    }
}