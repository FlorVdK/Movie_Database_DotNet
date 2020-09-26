﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MMDB_WebApp.Models
{
    public class MovieVM
    {

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        [Required(ErrorMessage = "title is required")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("releaseDate")]
        public DateTime ReleaseDate { get; set; }

        [JsonPropertyName("poster")]
        [DisplayName("PosterName")]
        public string Poster { get; set; }

        [JsonPropertyName("posterFile")]
        [DisplayName("UploadImage")]
        public IFormFile PosterFile { get; set; }

        [JsonPropertyName("directorId")]
        public int? DirectorId { get; set; }
        [JsonPropertyName("directorName")]
        public string DirectorName { get; set; }

        [JsonPropertyName("actors")]
        public virtual ICollection<ActorMovieVM> Actors { get; set; }
        [JsonPropertyName("comments")]
        public virtual ICollection<CommentVM> Comments { get; set; }
        [JsonPropertyName("votes")]
        public virtual ICollection<VotesVM> Votes { get; set; }
        [JsonPropertyName("genres")]
        public virtual ICollection<MovieGenresVM> Genres { get; set; }
    }
}
