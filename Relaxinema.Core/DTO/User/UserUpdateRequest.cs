using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Relaxinema.Core.DTO.User;

public class UserUpdateRequest
{
    public Guid Id { get; set; }
    [MinLength(2)]
    [MaxLength(25)]
    [Required]
    public string Nickname { get; set; }
    public IFormFile? File { get; set; }
}