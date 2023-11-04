using System.ComponentModel.DataAnnotations;

namespace Relaxinema.Core.DTO.Rating;

public class RatingRequest
{
    [Required] 
    public Guid? FilmId { get; set; }
    [Required]
    public Guid? UserId { get; set; }
    [Required] 
    public short Rate { get; set; }
}