namespace Relaxinema.Core.Domain.Entities
{
    public class Genre
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<Film> Films { get; set; }
    }
}
