using System;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace TinyServiceBusExplorer.Core.Tests
{
    public class Settings
    {
        public void InitSettings()
        {
            var location = Assembly.GetExecutingAssembly().Location;

            var path = Path.Combine(Path.GetDirectoryName(location), "settings.json");

            var text = File.ReadAllText(path);

            var settings = JsonSerializer.Deserialize<Settings>(text, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            ConnectionString = settings.ConnectionString;
        }

        public string ConnectionString { get; set; }
    }
}
