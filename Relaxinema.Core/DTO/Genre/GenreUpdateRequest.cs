using System.ComponentModel.DataAnnotations;

namespace Relaxinema.Core.DTO.Genre
{
    public class GenreUpdateRequest
    {
        [Required]
        public Guid? Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
    }
}
