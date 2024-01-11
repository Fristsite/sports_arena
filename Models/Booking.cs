using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Models
{
#nullable disable
    public class Booking
    {
        [Key]
        public long BookingId { get; set; }
        [ForeignKey("Arena")]
        public long ArenaId { get; set; }
        [ForeignKey("User")]
        public long UserId { get; set; }
        [ForeignKey("BookingCartType")]
        public long BookingCartTypeId { get; set; }

        public DateTime DateOfBooked { get; set; }
        public double SubTotal { get; set; }
        public double DiscountAmount { get; set; }
        public double TotalAmount { get; set; }

        public bool BookedStatus { get; set; }



        public virtual Arena Arena { get; set; }
        public virtual User User { get; set; }
        public virtual Payment Payment { get; set; }
        public virtual Ratings Ratings { get; set; }
        public virtual ICollection<BookingCourt> BookingCourts { get; set; }
        public virtual ICollection<BookingSportskit>BookingSportskits{get;set;}

        public virtual ICollection<PersonToPlay> PersonToPlays { get; set; }
        public virtual BookingCartType BookingCartType { get; set; }


    }
    public class BookingCourtData
{
    public BookingCourt BookingCourt { get; set; }
    public Booking Booking { get; set; }
}

}