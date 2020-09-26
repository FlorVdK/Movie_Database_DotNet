using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MMDB_API.Dtos
{
    public class MoviePutDTO
    {
        public int Id { get; set; }
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Length must be between 2 and 50")]
        [RegularExpression(@"^[a-zA-Z0-9ÀàáÂâçÉéÈèÊêëïîÔô'-\.\s]+$", ErrorMessage = "Invalid characters used")]
        public string Title { get; set; }
        [StringLength(250, MinimumLength = 2, ErrorMessage = "Length must be between 2 and 250")]
        [RegularExpression(@"^[a-zA-Z0-9ÀàáÂâçÉéÈèÊêëïîÔô'-\.\s]+$", ErrorMessage = "Invalid characters used")]
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int DirectorId { get; set; }
        public virtual ICollection<MovieGenresDTO> Genres { get; set; }
        public string Poster { get; set; }
    }
}
