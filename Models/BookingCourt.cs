using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Models
{
#nullable disable
    public class BookingCourt
    {

        [Key]
        public long BookingCartId { get; set; }
        [ForeignKey("Booking")]
        public long BookingId { get; set; }

        [ForeignKey("CourtDetails")]
        public long ItemId { get; set; }
        public DateOnly DateToplay { get; set; }
        public TimeOnly FromTime { get; set; }
        public TimeOnly ToTime { get; set; }
        public double TotalAmount { get; set; }
        public bool Status { get; set; }

        public virtual Booking Booking { get; set; }

        public virtual CourtDetails CourtDetails { get; set; }




    }
}