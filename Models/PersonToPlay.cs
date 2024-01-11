using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Play.Models
{
#nullable disable
    public class PersonToPlay
    {
        [Key]
        public long PersonToId { get; set; }
        [ForeignKey("Booking")]
        public long BookingId { get; set; }
        public string Person_Name { get; set; }

        public virtual Booking Booking {get;set;}

    }
     public class Pr
    {
       
        [ForeignKey("Booking")]
        public long BookingId { get; set; }
        public string Person_Name { get; set; }

    }
}