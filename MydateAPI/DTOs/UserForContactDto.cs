using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MydateAPI.DTOs
{
    public class UserForContactDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Status { get; set; }
        public string Mood { get; set; }
    }
}
