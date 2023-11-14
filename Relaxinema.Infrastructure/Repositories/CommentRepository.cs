using System.Runtime.CompilerServices;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.Domain.RepositoryContracts;
using Relaxinema.Core.Helpers;
using Relaxinema.Core.Helpers.RepositoryParams;
using Relaxinema.Infrastructure.DatabaseContext;
using Relaxinema.Infrastructure.RepositoryHelpers;

namespace Relaxinema.Infrastructure.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CommentRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task CreateAsync(Comment comment)
    {
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
    }

    public async Task<Comment?> GetByIdAsync(Guid id, string[]? includeStrings = null)
    {
        var query = IncludeParamsHelper<Comment>.IncludeStrings(includeStrings, _context.Comments.AsQueryable());

        return await query.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<PagedList<Comment>> GetAllForFilmAsync(CommentParams commentParams, string[]? includeStrings = null)
    {
        return await ApplyParams(commentParams, includeStrings);
    }

    public async Task<Comment?> UpdateAsync(Comment comment)
    {
        var origin = await _context.Comments.FirstOrDefaultAsync(c => c.Id == comment.Id);

        if (origin is null)
            return null;

        origin.Text = comment.Text;

        await _context.SaveChangesAsync();

        return origin;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var origin = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
        
        if (origin is null)
            return false;

        _context.Comments.Remove(origin);
        
        return await _context.SaveChangesAsync() > 0;
    }

    private async Task<PagedList<Comment>> ApplyParams(CommentParams commentParams, string[]? includeStrings)
    {
        var query = _context.Comments.AsQueryable();

        query = IncludeParamsHelper<Comment>.IncludeStrings(includeStrings, query);
        
        

        return await PagedList<Comment>.CreateAsync(query.OrderByDescending(c => c.Created), commentParams.PageNumber, commentParams.PageSize);
    }
}