using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Models
{
#nullable disable
    public class Ratings
    {
        [Key]
        public long RatingsId { get; set; }
        [ForeignKey("Booking")]
        public long BookingId { get; set; }
        [Required]
        [Range(0, 5)]
        public int RatingStar { get; set; }
        public string FeedBacK { get; set; }


        public virtual Booking Booking { get; set; }
    }
}