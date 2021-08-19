using System.Text.Json;
using System.Text.Json.Serialization;

namespace Test.Bernd.App.Config
{
    public static class JsonConfig
    {
        public static JsonSerializerOptions GetJsonSettings()
        {
            var settings = new JsonSerializerOptions
            {
                IgnoreNullValues = true,
                Converters = { new JsonStringEnumConverter()},
                PropertyNameCaseInsensitive = true
            };
            return settings;
        }
    }
}