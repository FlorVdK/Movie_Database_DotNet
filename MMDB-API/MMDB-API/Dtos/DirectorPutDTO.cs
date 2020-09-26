﻿using MMDB_API.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MMDB_API.Dtos
{
    public class DirectorPutDTO
    {
        public int Id { get; set; }
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Length must be between 2 and 50")]
        [RegularExpression(@"^[a-zA-Z0-9ÀàáÂâçÉéÈèÊêëïîÔô'-\.\s]+$", ErrorMessage = "Invalid characters used")]
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender gender { get; set; }
        public string Poster { get; set; }
    }
}
