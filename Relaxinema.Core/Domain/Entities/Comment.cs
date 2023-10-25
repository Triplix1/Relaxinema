using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relaxinema.Core.Domain.Entities
{
    public class Comment
    {
        public Guid Id { get; set; }
        public Guid FilmId { get; set; }
        public Guid UserId { get; set; }
        public DateTime Created { get; set; }
        public string Text { get; set; }
        public Film Film { get; set; }
        public User User { get; set; }
    }
}
