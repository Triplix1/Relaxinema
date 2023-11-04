using Relaxinema.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Relaxinema.Core.Helpers.RepositoryParams
{
    public class CommentParams : PaginatedParams
    {
        [Required]
        public Guid? FilmId { get; set; }
    }
}
