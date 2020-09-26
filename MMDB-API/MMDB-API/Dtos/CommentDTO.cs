using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MMDB_API.Dtos
{
    public class CommentDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Commenttext is required")]
        public string CommentText { get; set; }
        public DateTime Date { get; set; }

        //Relationships
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int? MovieId { get; set; }
    }
}
