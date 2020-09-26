using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMDB_WebApp.Models
{
    public class UserPatchVM
    {
        public string CurrentPassword { get; internal set; }
        public string NewPassword { get; internal set; }
        public string Id { get; internal set; }
    }
}
