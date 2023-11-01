using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.Helpers.RepositoryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relaxinema.Core.Domain.RepositoryContracts
{
    public interface ICommentRepository
    {
        Task CreateAsync(Comment comment);
        Task<Comment> GetByIdAsync(Guid id, CommentParams? commentParams = null);
        Task<IEnumerable<Comment>> GetAllAsync(CommentParams commentParams);
        Task<Comment> UpdateAsync(Comment comment);
        Task<Comment> DeleteAsync(Guid id);
    }
}
