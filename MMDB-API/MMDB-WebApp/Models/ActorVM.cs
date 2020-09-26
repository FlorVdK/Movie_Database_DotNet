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
    public class ActorVM
    {

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        [Required(ErrorMessage = "name is required")]
        public string Name { get; set; }

        [JsonPropertyName("dateOfBirth")]
        [Required(ErrorMessage = "Street is required")]
        public DateTime DateOfBirth { get; set; }

        [JsonPropertyName("gender")]
        [Required(ErrorMessage = "Street is required")]
        public Gender gender { get; set; }

        [JsonPropertyName("avatar")]
        [DisplayName("AvatarName")]
        public string Avatar { get; set; }

        [JsonPropertyName("avatarFile")]
        [DisplayName("UploadImage")]
        public IFormFile AvatarFile { get; set; }

        [JsonPropertyName("movies")]
        public virtual ICollection<ActorMovieVM> Movies { get; set; }
    }
}
