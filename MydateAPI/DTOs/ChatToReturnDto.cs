using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MydateAPI.DTOs
{
    public class ChatToReturnDto
    {
        public int Who { get; set; } //Id
        public string Message { get; set; } //content
        public DateTime Time { get; set; } //MessageSent
        public int Whose { get; set; } //RecipientId
    }
}
