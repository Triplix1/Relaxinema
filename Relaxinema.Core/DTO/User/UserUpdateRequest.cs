using Microsoft.AspNetCore.Http;

namespace Relaxinema.Core.DTO.User;

public class UserUpdateRequest
{
    public string Nickname { get; set; }
    public IFormFile File { get; set; }
}