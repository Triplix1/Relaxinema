using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.Helpers.RepositoryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Relaxinema.Core.Helpers;

namespace Relaxinema.Core.Domain.RepositoryContracts
{
    public interface ICommentRepository
    {
        Task CreateAsync(Comment comment);
        Task<Comment?> GetByIdAsync(Guid id, string[]? includeStrings = null);
        Task<PagedList<Comment>> GetAllForFilmAsync(CommentParams commentParams, string[]? includeStrings = null);
        Task<Comment?> UpdateAsync(Comment comment);
        Task<bool> DeleteAsync(Guid id);
    }
}
