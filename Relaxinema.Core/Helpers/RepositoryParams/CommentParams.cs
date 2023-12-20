using System.ComponentModel.DataAnnotations;

namespace Relaxinema.Core.Helpers.RepositoryParams
{
    public class CommentParams : PaginatedParams
    {
        [Required]
        public Guid? FilmId { get; set; }
    }
}
