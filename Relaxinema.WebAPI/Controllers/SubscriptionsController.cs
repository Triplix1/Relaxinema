using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Relaxinema.Core.DTO.Subscribe;
using Relaxinema.Core.Extentions;
using Relaxinema.Core.ServiceContracts;
using Relaxinema.WebAPI.Controllers.Base;

namespace Relaxinema.WebAPI.Controllers;

public class SubscriptionsController : BaseController
{
    private readonly ISubscriptionService _subscriptionService;

    public SubscriptionsController(ISubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }
    
    [Authorize]
    [HttpPost("subscribe")]
    public async Task<ActionResult<SubscribeResponse>> Subscribe([FromBody]SubscribeAddRequest subscribeAddRequest)
    {
        var userId = User.GetUserId();

        return Ok(await _subscriptionService.CreateSubscriptionAsync(subscribeAddRequest, userId));
    }
    
    [Authorize]
    [HttpDelete("unsubscribe/{filmId}")]
    public async Task<ActionResult> Unsubscribe([FromRoute]Guid filmId)
    {
        var userId = User.GetUserId();

        await _subscriptionService.DeleteAsync(filmId, userId);

        return Ok();
    }

    [HttpGet("{filmId}")]
    public async Task<ActionResult<bool>> IsSubscribed([FromRoute]Guid filmId)
    {
        Guid userId;
        try
        {
            userId = User.GetUserId();
        }
        catch (ApplicationException)
        {
            return false;
        }
        
        return Ok(await _subscriptionService.GetSubscriptionAsync(filmId, userId) is not null);
    }
    
}