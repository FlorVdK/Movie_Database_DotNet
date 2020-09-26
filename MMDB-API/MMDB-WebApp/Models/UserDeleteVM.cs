using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MMDB_WebApp.Models
{
    public class UserDeleteVM
    {
        public string Id { get; internal set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Length must be between 2 and 50")]
        [RegularExpression(@"^[a-zA-Z0-9ÀàáÂâçÉéÈèÊêëïîÔô'-\.\s]+$", ErrorMessage = "Invalid characters used")]
        public string FirstName { get; internal set; }
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Length must be between 2 and 50")]
        [RegularExpression(@"^[a-zA-Z0-9ÀàáÂâçÉéÈèÊêëïîÔô'-\.\s]+$", ErrorMessage = "Invalid characters used")]
        [Required]
        public string LastName { get; internal set; }
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Length must be between 2 and 50")]
        [RegularExpression(@"^[a-zA-Z0-9ÀàáÂâçÉéÈèÊêëïîÔô'-\.\s]+$", ErrorMessage = "Invalid characters used")]
        [Required]
        public string Username { get; internal set; }
        public string Email { get; internal set; }
    }
}
