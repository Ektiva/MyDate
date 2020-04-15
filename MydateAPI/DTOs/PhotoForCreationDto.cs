using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MydateAPI.DTOs
{
    public class PhotoForCreationDto
    {
        public string Url { get; set; }

        //this representate the file send with the http Request
        public IFormFile File { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }

        //From Cloudinary
        public string PublicId { get; set; }
        public PhotoForCreationDto()
        {
            DateAdded = DateTime.Now;
        }
    }
}
