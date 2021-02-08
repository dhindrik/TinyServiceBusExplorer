using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TinyServiceBusExplorer.Core.Models;

namespace TinyServiceBusExplorer.Core.Services
{
    public interface IConnectionService
    {
        Task<List<ConnectionInfo>> Get();
        Task Save(ConnectionInfo connection);
        Task Remove(ConnectionInfo connection);
    }
}
