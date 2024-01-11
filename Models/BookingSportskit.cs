using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Models
{
    public class BookingSportskit
    {
        [Key]
        public long BookingSportskitId { get; set; }
        [ForeignKey("Booking")]
        public long BookingId { get; set; }
        [ForeignKey("SportsKit")]
        public long ItemId { get; set; }
        public DateOnly DateToplay { get; set; }
        public TimeOnly FromTime { get; set; }
        public TimeOnly ToTime { get; set; }
        public double TotalAmount { get; set; }
        public bool Status { get; set; }

        public virtual Booking Booking { get; set; }


        public virtual SportsKit SportsKit { get; set; }
    }
    public class BookingSportskitreq
    {
        [ForeignKey("Booking")]
        public long BookingId { get; set; }
        [ForeignKey("SportsKit")]
        public long ItemId { get; set; }
        public DateOnly DateToplay { get; set; }
        public TimeOnly FromTime { get; set; }
        public int DurationInHours { get; set; }
    }

}