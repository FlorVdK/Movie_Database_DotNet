using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Text.Json;

namespace MMDB_WebApp.Extensions
{
    public static class SessionExtensions
    {
        public static void SetStateData(this ISession session, string key, Dictionary<string, string> dictionary)
        {
            session.SetString(key, JsonSerializer.Serialize(dictionary));
        }

        public static Dictionary<string, string> GetStateData(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<Dictionary<string, string>>(value);
        }
    }
}
