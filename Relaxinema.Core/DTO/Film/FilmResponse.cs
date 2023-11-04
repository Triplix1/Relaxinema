using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relaxinema.Core.DTO.Film
{
    public class FilmResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public short? Year { get; set; }
        public short? Limitation { get; set; }
        public string? Description { get; set; }
        public bool Publish { get; set; }
        public bool IsExpected { get; set; }
        public string? PhotoUrl { get; set; }
        public string[] Sources { get; set; }
        public string Trailer { get; set; } = null!;
        public string[] GenreNames { get; set; }
    }
}
