using AutoMapper;
using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.Domain.RepositoryContracts;
using Relaxinema.Core.DTO.Subscribe;
using Relaxinema.Core.ServiceContracts;

namespace Relaxinema.Core.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IMapper _mapper;

    public SubscriptionService(ISubscriptionRepository subscriptionRepository, IMapper mapper)
    {
        _subscriptionRepository = subscriptionRepository;
        _mapper = mapper;
    }
    
    public async Task<SubscribeResponse?> GetSubscriptionAsync(Guid filmId, Guid userId)
    {
        var subscription = await _subscriptionRepository.GetSubscriptionAsync(filmId, userId);

        return subscription is null ? null : _mapper.Map<SubscribeResponse>(subscription);
    }

    public async Task<SubscribeResponse> CreateSubscriptionAsync(SubscribeAddRequest subscribeAddRequest, Guid userId)
    {
        var subscription = _mapper.Map<Subscription>(subscribeAddRequest);

        subscription.UserId = userId;

        await _subscriptionRepository.CreateSubscriptionAsync(subscription);

        return _mapper.Map<SubscribeResponse>(subscription);
    }

    public async Task<IEnumerable<SubscribeResponse>> GetAllSubscriptionsByFilm(Guid filmId)
    {
        return _mapper.Map<IEnumerable<SubscribeResponse>>(await _subscriptionRepository.GetAllSubscriptionsByFilm(filmId));
    }

    public async Task DeleteAsync(Guid filmId, Guid userId)
    {
        var result = await _subscriptionRepository.DeleteAsync(filmId, userId);

        if (!result)
            throw new KeyNotFoundException();
    }
}