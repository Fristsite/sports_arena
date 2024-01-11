using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Models
{
#nullable disable
    public class User
    {
        [Key]
        public long UserId { get; set; }
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
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }



        public virtual ICollection<Booking> Bookings { get; set; }







    }
}