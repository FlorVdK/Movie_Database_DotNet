using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MMDB_WebApp.Extensions;
using MMDB_WebApp.Helpers;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace MMDB_WebApp.Handlers
{
    public class ValidateHeaderHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStateHelper _stateHelper;
        private string token = null;

        public ValidateHeaderHandler
        (
            IHttpContextAccessor httpContextAccessor,
            IStateHelper stateHelper
        )
        {
            _httpContextAccessor = httpContextAccessor;
            _stateHelper = stateHelper;
        }

        protected override async Task<HttpResponseMessage> SendAsync
        (
            HttpRequestMessage request,
            CancellationToken cancellationToken
        )
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }

            // If StateData such as username and JWT token is available in the session, use those values
            if (_httpContextAccessor.HttpContext.Session.GetStateData("StateData") != null)
            {
                token = _httpContextAccessor.HttpContext.Session.GetStateData("StateData")["_Token"];

                if (token != null)
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }
            else // If StateData such as username and JWT token is available in the cookie, use those values
            {
                if (_httpContextAccessor.HttpContext.Request.GetStateData("StateData") != null)
                {
                    // Set StateData in session, cookie and TempData
                    _stateHelper.SetState(_httpContextAccessor.HttpContext.Request.GetStateData("StateData"));
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
