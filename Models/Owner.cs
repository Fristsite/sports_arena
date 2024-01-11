using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Models
{
#nullable disable
    public class Owner
    {
        [Key]
        public long OwnerId { get; set; }
        [Required]
        [MaxLength(80)]
        [MinLength(3)]
        public string OwnerName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password {get;set;}
       
        public long PhoneNumber { get; set; }
        public DateTime CreatedAt{get;set;}
        public bool IsActive { get; set; }


        public virtual ICollection<Arena>Arenas{get;set;}
    }
}