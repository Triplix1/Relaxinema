using System.ComponentModel.DataAnnotations;

namespace Relaxinema.Core.DTO.Genre;

public class GenreAddRequest
{
    [Required]
    public string Name { get; set; } = null!;
}