namespace Relaxinema.Core.Domain.Entities
{
    public class Subscription
    {
        public Guid FilmId { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Film Film { get; set; }
    }
}
