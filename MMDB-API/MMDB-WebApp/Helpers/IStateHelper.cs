using MMDB_WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMDB_WebApp.Helpers
{
    public interface IStateHelper
    {
        void SetState(UserVM authenticatedUser, bool RememberMe);
        void SetState(Dictionary<string, string> stateDataFromCookie);
        void ClearState();
    }
}
