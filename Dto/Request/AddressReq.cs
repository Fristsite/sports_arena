using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Dto.Request
{
    public class AddressReq
    {
        [Required]
        public string ArenaAddress { get; set; }
        [Required]
        public string ArenaName { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
    }

     public class Addresss
    {
        [Key]
        public long AddressId { get; set; }
        [Required]
        public string ArenaAddress { get; set; }
        [Required]
        public string ArenaName { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }



    }
}