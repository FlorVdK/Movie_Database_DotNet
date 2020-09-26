using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MMDB_WebApp.Areas.Comment.Models
{
    public class CommentVM
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("commentText")]
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
