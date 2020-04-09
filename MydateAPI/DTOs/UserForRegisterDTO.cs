using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MydateAPI.DTOs
{
    public class UserForRegisterDTO
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(11, MinimumLength = 7, ErrorMessage = "You must specify password between 7 and 11 characters")]
        public string Password { get; set; }
    }
}
