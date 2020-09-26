using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System;
using System.Threading.Tasks;

namespace MMDB_WebApp.Services
{
    public class CultureProviderResolverService : RequestCultureProvider
    {
        private static readonly char[] _cookieSeparator = new[] { '|' };
        private const string _culturePrefix = "c=";
        private const string _uiCulturePrefix = "uic=";

        public override async Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            if (GetCultureFromQueryString(httpContext, out string culture))
                return new ProviderCultureResult(culture, culture);
            else if (GetCultureFromCookie(httpContext, out culture))
                return new ProviderCultureResult(culture, culture);
            else if (GetCultureFromSession(httpContext, out culture))
                return new ProviderCultureResult(culture, culture);

            return await NullProviderCultureResult.ConfigureAwait(false);
        }

        private bool GetCultureFromQueryString(HttpContext httpContext, out string culture)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            var request = httpContext.Request;

            if (!request.QueryString.HasValue)
            {
                culture = null;
                return false;
            }

            culture = request.Query["culture"];

            return true;
        }

        private bool GetCultureFromCookie(HttpContext httpContext, out string culture)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            var cookie = httpContext.Request.Cookies["culture"];

            if (string.IsNullOrEmpty(cookie))
            {
                culture = null;
                return false;
            }

            culture = ParseCookieValue(cookie);

            return !string.IsNullOrEmpty(culture);
        }

        public static string ParseCookieValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            var parts = value.Split(_cookieSeparator, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
            {
                return null;
            }

            var potentialCultureName = parts[0];
            var potentialUICultureName = parts[1];

            if (!potentialCultureName.StartsWith(_culturePrefix, StringComparison.OrdinalIgnoreCase) || !potentialUICultureName.StartsWith(_uiCulturePrefix, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            var cultureName = potentialCultureName.Substring(_culturePrefix.Length);
            var uiCultureName = potentialUICultureName.Substring(_uiCulturePrefix.Length);

            if (cultureName == null && uiCultureName == null)
            {
                return null;
            }

            if (cultureName != null && uiCultureName == null)
            {
                uiCultureName = cultureName;
            }

            if (cultureName == null && uiCultureName != null)
            {
                cultureName = uiCultureName;
            }

            return cultureName;
        }

        private bool GetCultureFromSession(HttpContext httpContext, out string culture)
        {
            culture = httpContext.Session.GetString("culture");
            return !string.IsNullOrEmpty(culture);
        }
    }
}
