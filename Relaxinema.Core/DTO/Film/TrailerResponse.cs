namespace Relaxinema.Core.DTO.Film;

public class TrailerResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Trailer { get; set; } = null!;
}