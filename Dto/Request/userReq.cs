using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Dto.Request
{
    public class userReq
    {
         [Required]
        [MaxLength(80)]
        [MinLength(3)]
        public string FullName { get; set; }
        [Required]
        
        public long PhoneNumber { get; set; }
        [Required]
        public String UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}