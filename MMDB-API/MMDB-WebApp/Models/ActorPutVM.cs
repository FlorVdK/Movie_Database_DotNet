using Microsoft.AspNetCore.Http;
using MMDB_WebApp.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MMDB_WebApp.Models
{
    public class ActorPutVM
    {

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        [Required(ErrorMessage = "name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Length must be between 2 and 50")]
        [RegularExpression(@"^[a-zA-Z0-9ÀàáÂâçÉéÈèÊêëïîÔô'-\.\s]+$", ErrorMessage = "Invalid characters used")]
        public string Name { get; set; }

        [JsonPropertyName("dateOfBirth")]
        [Required(ErrorMessage = "Date is required")]
        public DateTime DateOfBirth { get; set; }

        [JsonPropertyName("gender")]
        [Required(ErrorMessage = "Gender is required")]
        public Gender gender { get; set; }

        [JsonPropertyName("avatar")]
        [DisplayName("AvatarName")]
        public string Avatar { get; set; }
    }
}
