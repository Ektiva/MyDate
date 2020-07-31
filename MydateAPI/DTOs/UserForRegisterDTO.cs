using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MydateAPI.DTOs
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(11, MinimumLength = 7, ErrorMessage = "You must specify password between 7 and 11 characters")]
        public string Password { get; set; }
        [Required]
        public string Gender { get; set; }

        [Required]
        public string KnownAs { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }

        public UserForRegisterDto()
        {
            Created = DateTime.Now;
            LastActive = DateTime.Now;
        }

        public string Company { get; set; }
        [Required]
        public string Jobtitle { get; set; }
        [Required]
        public string Email { get; set; }
        public string Phone { get; set; }
        [Required]
        public string Address { get; set; }
        public string Lastname { get; set; }
        public string Mood { get; set; }
        [Required]
        public int Size { get; set; }
        [Required]
        public int Pound { get; set; }
        [Required]
        public int Feet { get; set; }
        [Required]
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string Smoke { get; set; }
        public string Drink { get; set; }
        public string Avatar { get; set; }
    }
}
