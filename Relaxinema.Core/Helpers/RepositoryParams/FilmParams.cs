using Relaxinema.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Relaxinema.Core.Helpers.RepositoryParams
{
    public class FilmParams : PaginatedParams
    {
        public bool? Expected { get; set; }
        public short? Year { get; set; }
        public string? Genre { get; set; }
        public OrderByParams? OrderByParams { get; set; }
    }
}
