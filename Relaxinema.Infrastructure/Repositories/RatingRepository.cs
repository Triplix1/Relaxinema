using Microsoft.EntityFrameworkCore;
using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.Domain.RepositoryContracts;
using Relaxinema.Infrastructure.DatabaseContext;

namespace Relaxinema.Infrastructure.Repositories;

public class RatingRepository : IRatingRepository
{
    private readonly ApplicationDbContext _context;

    public RatingRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task RateAsync(Rating rating)
    {
        var userRating = await _context.Ratings.FirstOrDefaultAsync(r => r.FilmId == rating.FilmId && r.UserId == rating.UserId);

        if (userRating is null)
            await _context.Ratings.AddAsync(rating);
        else
            userRating.Rate = rating.Rate;

        await _context.SaveChangesAsync();
    }

    public async Task<float> GetFilmMarkAsync(Film film)
    {
        var userRatings = _context.Ratings.Where(r => r.FilmId == film.Id);

        var count = await userRatings.CountAsync();
        
        if (count == 0)
            return 0;

        return await userRatings.SumAsync(r => r.Rate) / (float)count;
    }

    public async Task<Rating?> GetRateAsync(User user, Film film)
    {
        return await _context.Ratings.FirstOrDefaultAsync(r => r.FilmId == film.Id && r.UserId == user.Id);
    }
}