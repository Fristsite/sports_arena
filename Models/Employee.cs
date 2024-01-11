using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Models
{
#nullable disable
    public class Employee
    {
        [Key]
        public long EmployeeId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        
        public long PhoneNumber {get;set;}
        [ForeignKey("Arena")]
        public long ArenaId { get; set; }
        public DateOnly DateofJoined { get; set; }
        public bool IsActive { get; set; }

        public virtual Arena Arena {get;set;}
    }
}