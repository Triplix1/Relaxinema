using Relaxinema.Core.DTO.Film;

namespace Relaxinema.Core.DTO.User;

public class AccountInfoResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Nickname { get; set; }
    public string? PhotoUrl { get; set; }
    public IEnumerable<FilmCardResponse> SubscribedTo { get; set; }
}