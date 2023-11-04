namespace Relaxinema.Core.DTO.Film;

public class FilmCardResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public short? Year { get; set; }
    public string? PhotoUrl { get; set; }
    public string[] GenreNames { get; set; }
}