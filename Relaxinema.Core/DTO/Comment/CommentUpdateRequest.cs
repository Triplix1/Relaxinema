using System.ComponentModel.DataAnnotations;

namespace Relaxinema.Core.DTO.Comment;

public class CommentUpdateRequest
{
    [Required]
    public Guid? Id { get; set; }
    
    [Required]
    [MinLength(1)]
    [MaxLength(150)]
    public string Text { get; set; }
}