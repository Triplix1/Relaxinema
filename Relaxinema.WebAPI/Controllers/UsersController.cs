using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Relaxinema.Core.Domain.RepositoryContracts;
using Relaxinema.Core.DTO.User;
using Relaxinema.Core.Helpers;
using Relaxinema.Core.Helpers.RepositoryParams;
using Relaxinema.Core.ServiceContracts;
using Relaxinema.WebAPI.Controllers.Base;

namespace Relaxinema.WebAPI.Controllers;

public class UsersController : BaseController
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("admins")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<PagedList<UserResponse>>> GetAdmins([FromQuery]PaginatedParams pagination)
    {
        return Ok(await _userService.GetAllAsync(pagination, true));
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult> DeleteAdmin([FromRoute]Guid id)
    {
        await _userService.DeleteAsync(id);
        return Ok();
    }
}