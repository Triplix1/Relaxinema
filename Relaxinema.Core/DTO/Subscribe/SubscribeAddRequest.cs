using System.ComponentModel.DataAnnotations;

namespace Relaxinema.Core.DTO.Subscribe;

public class SubscribeAddRequest
{
    [Required]
    public Guid? FilmId { get; set; }
}