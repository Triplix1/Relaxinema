﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Relaxinema.Core.DTO.Film
{
    public class FilmAddRequest
    {
        [Required]
        public string Name { get; set; } = null!;

        public short? Year { get; set; } = null;
        public short? Limitation { get; set; } = null;
        public string? Description { get; set; } = null;
        [Required]
        public bool? Publish { get; set; }
        [Required]
        public bool? IsExpected { get; set; }
        public IFormFile? File { get; set; }
        public string? SourceNames { get; set; }
        [Required]
        public string Trailer { get; set; } = null!;
        [Required]
        public string GenreNames { get; set; } = null!;
    }
}
