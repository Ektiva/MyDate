using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MydateAPI.Models
{
    public class User1
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Avatar { get; set; }
        public string Nickname { get; set; }
        public string Company { get; set; }
        public string Jobtitle { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime Birthday { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }
        public string Mood { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Gender { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        //Each User have a collection of photo
        //Create a class to store all photos
        public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<Like> Likers { get; set; }
        public virtual ICollection<Like> Likees { get; set; }
        public virtual ICollection<Message> MessagesSent { get; set; }
        public virtual ICollection<Message> MessagesReceived { get; set; }

    }
}
