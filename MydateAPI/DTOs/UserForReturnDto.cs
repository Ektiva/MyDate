using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MydateAPI.DTOs
{
    public class UserForReturnDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Lastname { get; set; }
        public string Avatar { get; set; }
        public string Company { get; set; }
        public string Jobtitle { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }
        public string Mood { get; set; }
        public int Size { get; set; }
        public int Pound { get; set; }
        public int Feet { get; set; }
        public string Smoke { get; set; }
        public string Drink { get; set; }
    }
}
