using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TinyServiceBusExplorer.Core.Models;
using TinyServiceBusExplorer.Core.Services;
using Xamarin.Essentials;

namespace TinyServiceBusExplorer.Clients.Services
{
    public class SecureConnectionService : IConnectionService
    {
        public SecureConnectionService()
        {
        }

        public async Task<List<ConnectionInfo>> Get()
        {
            var json = await SecureStorage.GetAsync("connections");

            if(string.IsNullOrWhiteSpace(json))
            {
                return new List<ConnectionInfo>();
            }

            var connections = JsonSerializer.Deserialize<List<ConnectionInfo>>(json, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            return connections;
        }

        public async Task Remove(ConnectionInfo connection)
        {
            var connections = await Get();

            var current = connections.Single(x => x.Value == connection.Value);

            connections.Remove(current);

            var json = JsonSerializer.Serialize(connections);

            await SecureStorage.SetAsync("connections", json);
        }

        public async Task Save(ConnectionInfo connection)
        {
            var connections = await Get();

            connections.Add(connection);

            var json = JsonSerializer.Serialize(connections);

            await SecureStorage.SetAsync("connections", json);
        }
    }
}
