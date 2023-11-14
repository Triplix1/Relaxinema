using System.ComponentModel.DataAnnotations;

namespace Relaxinema.Core.DTO.Comment;

public class CommentAddRequest
{
    [Required]
    public Guid? FilmId { get; set; }
    
    [Required]
    [MinLength(1)]
    [MaxLength(150)]
    public string Text { get; set; }
}