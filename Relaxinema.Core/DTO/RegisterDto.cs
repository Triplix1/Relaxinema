using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relaxinema.Core.DTO
{
    public class RegisterDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Nickname {  get; set; } 
        [Required]
        public string Password { get; set; }
    }
}
