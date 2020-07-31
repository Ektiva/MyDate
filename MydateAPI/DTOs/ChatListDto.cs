using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MydateAPI.DTOs
{
    public class ChatListDto
    {
        public string Id { get; set; } //to create
        public int contactId { get; set; } //Sender or Recipient Id
        public string Name { get; set; } //Sender or Recipient Name
        public int Unread { get; set; } 
        public string LastMessage { get; set; } 
        public DateTime LastMessageTime { get; set; }
    }
}
