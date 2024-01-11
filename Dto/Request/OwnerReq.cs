using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Dto.Request
{
    #nullable disable
    public class OwnerReq
    {
        [Required]
        [MaxLength(80)]
        [MinLength(3)]
        public string OwnerName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Range(6000000000,9999999999)]
        public long PhoneNumber { get; set; }

    }
}