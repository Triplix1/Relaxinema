using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.Domain.RepositoryContracts;
using Relaxinema.Core.Helpers.RepositoryParams;
using Relaxinema.Infrastructure.DatabaseContext;

namespace Relaxinema.Infrastructure.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDbContext _context;

    public CommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task CreateAsync(Comment comment)
    {
        await _context.Comments.AddAsync(comment);
    }

    public Task<Comment> GetByIdAsync(Guid id, CommentParams? commentParams = null)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Comment>> GetAllAsync(CommentParams commentParams)
    {
        throw new NotImplementedException();
    }

    public Task<Comment> UpdateAsync(Comment comment)
    {
        throw new NotImplementedException();
    }

    public Task<Comment> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
    
    private IQueryable<Comment> ApplyParams(CommentParams)
}