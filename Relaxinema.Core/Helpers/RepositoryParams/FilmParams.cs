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
        public FilterParams? FilterParams { get; set; }
        public OrderByParams? OrderByParams { get; set; }
        public bool ShowHiddens { get; set; } = false;
    }
}
