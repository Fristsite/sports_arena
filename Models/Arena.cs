using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Play.Models
{
#nullable disable
    public class Arena
    {

        [Key]
        public long ArenaId { get; set; }
        [Required]
        public string ArenaName { get; set; }

        [Required]
        [ForeignKey("Owner")]
        public long OwnerId { get; set; }

        [Required]
        [ForeignKey("Address")]
        public long AddressId { get; set; }

        public DateOnly DateofAdded{get;set;}

        public bool IsActive{get;set;}



        public virtual Address Address { get; set; }
        public virtual Owner Owner { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual ICollection<SportsKit> SportsKits { get; set; }
        public virtual ICollection<CourtDetails> CourtDetails { get; set; }
        public virtual ICollection<Booking>Bookings{get;set;}
    }
}