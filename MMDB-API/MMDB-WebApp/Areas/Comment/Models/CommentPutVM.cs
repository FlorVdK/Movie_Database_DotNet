using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MMDB_WebApp.Areas.Comment.Models
{
    public class CommentPutVM
    {
        [JsonPropertyName("commentText")]
        [Required(ErrorMessage = "CommentText missing")]
        public string CommentText { get; set; }
        [JsonPropertyName("date")]
        [Required(ErrorMessage = "Date missing")]
        public DateTime Date { get; set; }

        //Relationships
        [JsonPropertyName("userId")]
        [Required(ErrorMessage = "UserId missing")]
        public string UserId { get; set; }
        [JsonPropertyName("movieId")]
        [Required(ErrorMessage = "MovieId missing")]
        public int? MovieId { get; set; }
    }
}
