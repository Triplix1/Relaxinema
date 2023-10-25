using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relaxinema.Core.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string Nickname { get; set; } = null!;
        public byte[] PasswordHash { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;
        public ICollection<Role> Roles { get; set; }
        public ICollection<Film> SubscribedTo { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
