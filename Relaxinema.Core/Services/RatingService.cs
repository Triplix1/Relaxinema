using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.Domain.RepositoryContracts;
using Relaxinema.Core.DTO.Rating;
using Relaxinema.Core.ServiceContracts;

namespace Relaxinema.Core.Services;

public class RatingService : IRatingService
{
    private readonly IRatingRepository _ratingRepository;
    private readonly IFilmRepository _filmRepository;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;

    public RatingService(IRatingRepository ratingRepository, IFilmRepository filmRepository, UserManager<User> userManager, IMapper mapper)
    {
        _ratingRepository = ratingRepository;
        _filmRepository = filmRepository;
        _userManager = userManager;
        _mapper = mapper;
    }
    
    public async Task<float> GetRatingAsync(Guid filmId)
    {
        var film = await GetFilm(filmId);

        return await _ratingRepository.GetFilmMarkAsync(film);
    }

    public async Task<RatingResponse> GetUserRateAsync(Guid filmId, Guid userId)
    {
        var user = await GetUser(userId);

        var film = await GetFilm(filmId);

        var rate =  await _ratingRepository.GetRateAsync(user, film);

        if (rate is null)
            return new RatingResponse { Rate = 0 };

        return _mapper.Map<RatingResponse>(rate);
    }

    // public async Task<RatingResponse> RateFilmAsync(RatingRequest ratingRequest, Guid userId)
    // {
    //     var user = await GetUser(userId);
    //     var film = await GetFilm(ratingRequest.FilmId.GetValueOrDefault());
    //
    //     var rate = _mapper.Map<Rating>(ratingRequest);
    //     rate.UserId = userId;
    //
    //     await _ratingRepository.RateAsync(rate);
    //
    //     return _mapper.Map<RatingResponse>(rate);
    // }
    
    public async Task<RatingResponse> RateFilmAsync(RatingRequest ratingRequest, Guid userId)
    {
        var user = await GetUser(userId);
        
        if (user is null)
            throw new KeyNotFoundException("Cannot find such user");
        
        var film = await GetFilm(ratingRequest.FilmId.GetValueOrDefault());

        var rate = _mapper.Map<Rating>(ratingRequest);
        rate.UserId = userId;

        await _ratingRepository.RateAsync(rate);

        return new RatingResponse()
        {
            Rate = await _ratingRepository.GetFilmMarkAsync(film)
        };
    }
    
    private async Task<User> GetUser(Guid userId)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        
        if (user is null)
            throw new KeyNotFoundException("Cannot find user with such id");
        
        return user;
    }

    private async Task<Film> GetFilm(Guid filmId)
    {
        var film = await _filmRepository.GetByIdAsync(filmId);
        
        if (film is null)
            throw new KeyNotFoundException("Cannot find film with such id");

        return film;
    }
}