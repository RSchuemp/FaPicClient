using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaPicClient.Models
{
    public class HomeModel
    {
        public IFormFile File { get; set; }

        public IList<byte[]> Images { get; set; }
    }
}
