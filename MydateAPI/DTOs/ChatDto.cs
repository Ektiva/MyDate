using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MydateAPI.DTOs
{
    public class ChatDto
    {
        public string Id { get; set; }
        public virtual IEnumerable<ChatToReturnDto> Dialog { get; set; }
    }
}
