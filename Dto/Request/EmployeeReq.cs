using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Dto.Request
{
    #nullable disable
    public class EmployeeReq
    {
        
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        
        public long PhoneNumber {get;set;}
        [ForeignKey("Arena")]
        public long ArenaId { get; set; }
    }
     public class EmployeeReqs
    {
        
        public long EmployeeId {get;set;}
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        
        public long PhoneNumber {get;set;}
        [ForeignKey("Arena")]
        public long ArenaId { get; set; }
    }
}