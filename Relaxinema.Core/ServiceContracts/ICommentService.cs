using Relaxinema.Core.DTO.Comment;
using Relaxinema.Core.Helpers;
using Relaxinema.Core.Helpers.RepositoryParams;

namespace Relaxinema.Core.ServiceContracts;

public interface ICommentService
{
    Task<CommentResponse> GetByIdAsync(Guid id);
    Task<PagedList<CommentResponse>> GetAllForFilmAsync(CommentParams commentParams);
    Task<CommentResponse> CreateCommentAsync(CommentAddRequest commentAddRequest, Guid userId);
    Task<CommentResponse> UpdateCommentAsync(CommentUpdateRequest commentUpdateRequest);
    Task DeleteAsync(Guid id);
}