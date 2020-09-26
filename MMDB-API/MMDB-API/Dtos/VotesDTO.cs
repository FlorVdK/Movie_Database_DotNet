using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMDB_API.Dtos
{
    public class VotesDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public int Score { get; set; }
    }
}
