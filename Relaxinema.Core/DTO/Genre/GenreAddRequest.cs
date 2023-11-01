using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relaxinema.Core.DTO.Genre
{
    public class GenreAddRequest
    {
        [Required]
        public string Name { get; set; } = null!;
    }
}
