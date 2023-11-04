namespace Relaxinema.Core.Domain.Entities;

public class Trailer
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Frame { get; set; } = null!;
}