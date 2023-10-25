using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relaxinema.Core.Domain.Entities
{
    public class Subscription
    {
        public Guid FilmId { get; set; }
        public Guid UserId { get; set; }
    }
}
