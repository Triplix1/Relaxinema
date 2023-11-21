using Microsoft.EntityFrameworkCore;
using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.Domain.RepositoryContracts;
using Relaxinema.Infrastructure.DatabaseContext;

namespace Relaxinema.Infrastructure.Repositories;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly ApplicationDbContext _context;

    public SubscriptionRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Subscription?> GetSubscriptionAsync(Guid filmId, Guid userId)
    {
        return await _context.Subscriptions.FirstOrDefaultAsync(s => s.FilmId == filmId && s.UserId == userId);
    }

    public async Task CreateSubscriptionAsync(Subscription subscription)
    {
        await _context.Subscriptions.AddAsync(subscription);

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Subscription>> GetAllSubscriptionsByFilm(Guid filmId)
    {
        return await _context.Subscriptions.Include(s => s.User).Where(s => s.FilmId == filmId).ToListAsync();
    }

    public async Task<bool> DeleteAsync(Guid filmId, Guid userId)
    {
        var subscription =
            await _context.Subscriptions.FirstOrDefaultAsync(s => s.FilmId == filmId && s.UserId == userId);

        if (subscription is null)
            return false;

        _context.Subscriptions.Remove(subscription);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task DeleteByFilm(Guid filmId)
    {
        var subscriptions = _context.Subscriptions.Where(s => s.FilmId == filmId);
        
        _context.Subscriptions.RemoveRange(subscriptions);

        await _context.SaveChangesAsync();
    }
}