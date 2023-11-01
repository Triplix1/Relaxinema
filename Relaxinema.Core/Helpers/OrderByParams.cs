using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relaxinema.Core.Helpers
{
    public class OrderByParams
    {
        public OrderByParams(string orderBy)
        {
            OrderBy = orderBy;
        }

        public OrderByParams(string orderBy, bool asc) : this(orderBy)
        {
            Asc = asc;
        }

        public string OrderBy { get; set; }
        public bool Asc {  get; set; }
    }
}
