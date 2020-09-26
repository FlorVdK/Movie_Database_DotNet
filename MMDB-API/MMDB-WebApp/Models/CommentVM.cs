using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MMDB_WebApp.Models
{
    public class CommentVM
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("commentText")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Length must be between 2 and 50")]
        [RegularExpression(@"^[a-zA-Z0-9ÀàáÂâçÉéÈèÊêëïîÔô'-\.\s]+$", ErrorMessage = "Invalid characters used")]
        public string CommentText { get; set; }
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        //Relationships
        [JsonPropertyName("userId")]
        public string UserId { get; set; }
        [JsonPropertyName("userName")]
        public string UserName { get; set; }
        [JsonPropertyName("userAvatar")]
        public string UserAvatar { get; set; }
        [JsonPropertyName("movieId")]
        public int? MovieId { get; set; }
    }
}
