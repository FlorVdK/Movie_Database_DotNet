using MMDB_API.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MMDB_API.Dtos
{
    public class UserDTO
    {
        public string Id { get; set; }
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Length must be between 2 and 50")]
        [RegularExpression(@"^[a-zA-Z0-9ÀàáÂâçÉéÈèÊêëïîÔô'-\.\s]+$", ErrorMessage = "Invalid characters used")]
        public string FirstName { get; set; }
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Length must be between 2 and 50")]
        [RegularExpression(@"^[a-zA-Z0-9ÀàáÂâçÉéÈèÊêëïîÔô'-\.\s]+$", ErrorMessage = "Invalid characters used")]
        public string LastName { get; set; }
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Length must be between 2 and 50")]
        [RegularExpression(@"^[a-zA-Z0-9ÀàáÂâçÉéÈèÊêëïîÔô'-\.\s]+$", ErrorMessage = "Invalid characters used")]
        public string Username { get; set; }
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Length must be between 2 and 50")]
        [RegularExpression(@"^[a-zA-Z0-9ÀàáÂâçÉéÈèÊêëïîÔô'-\.\s]+$", ErrorMessage = "Invalid characters used")]
        public string Email { get; set; }
        public string Password { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "<Pending>")]
        public byte[] Avatar { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender gender { get; set; }

        public string Token { get; set; }

        //Relationships
        public int? RoleId { get; set; }
        public ICollection<UserRoleAssignmentDTO> Roles { get; set; }
        public virtual ICollection<CommentDTO> CommentDTOs { get; set; }
        public virtual ICollection<VotesDTO> VotesDTOs { get; set; }
    }
}
