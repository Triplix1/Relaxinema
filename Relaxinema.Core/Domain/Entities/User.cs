using Microsoft.AspNetCore.Identity;

namespace Relaxinema.Core.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        [PersonalData]
        public string Nickname { get; set; } = null!;
        [PersonalData]
        public string? PhotoUrl { get; set; }
        [PersonalData]
        public string? PhotoPublicId { get; set; }
        public ICollection<Film> SubscribedTo { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Rating> Rates { get; set; }
        public ICollection<IdentityUserRole<Guid>> Roles { get; set; }
    }
}
