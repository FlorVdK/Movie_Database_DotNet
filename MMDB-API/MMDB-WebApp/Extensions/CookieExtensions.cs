using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace MMDB_WebApp.Extensions
{
    public static class CookieExtensions
    {
        public static void SetStateData(this IResponseCookies responseCookies, string key, Dictionary<string, string> dictionary, CookieOptions cookieOptions)
        {
            if (responseCookies == null) { throw new ArgumentNullException(nameof(responseCookies)); }

            responseCookies.Append(key, JsonSerializer.Serialize(dictionary), cookieOptions);
        }

        public static Dictionary<string, string> GetStateData(this HttpRequest request, string key)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }

            var value = request.Cookies[key];
            return value == null ? default : JsonSerializer.Deserialize<Dictionary<string, string>>(value);
        }
    }
}
