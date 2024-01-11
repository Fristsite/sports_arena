using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Dto.Request
{
#nullable disable
    public class BookingReq
    {
        public long ArenaId { get; set; }
        public long BookingCartTypeId { get; set; }


    }

    public class BookingCourtreq
    {
        public long BookingId { get; set; }

        public long ItemId { get; set; }
        public DateOnly DateToplay { get; set; }
        public TimeOnly FromTime { get; set; }
        public int DurationInHours { get; set; }
    }

    // public class BookReq
    // {
    //     public BookingReq BookingReq { get; set; }
    //     public List<BookingCartreq> BookingCartreqs { get; set; }
    // }

    public class Bokreq
    {
        public long BookingId { get; set; }
        [ForeignKey("Arena")]
        public long ArenaId { get; set; }
        public long BookingCartTypeId { get; set; }
        [ForeignKey("User")]
        public long UserId { get; set; }

        public DateTime DateOfBooked { get; set; }
        public double SubTotal { get; set; }
        public double DiscountAmount
        {
            get; set;
        }
        public double TotalAmount { get; set; }

        public bool BookedStatus { get; set; }

    }
    public class Bc
    {

        public long ItemId { get; set; }
        public DateOnly DateToplay { get; set; }
        public TimeOnly FromTime { get; set; }
        public TimeOnly Totime { get; set; }
        public double TotalAmount { get; set; }

    }

    public class Br
    {

        public long BookingId { get; set; }
        public DateTime DateOfBooked { get; set; }
        public long BookingCartTypeId { get; set; }
        [ForeignKey("User")]
        public long UserId { get; set; }
        [ForeignKey("Arena")]
        public long ArenaId { get; set; }

        public List<Bc> Cart_Items { get; set; }



        public double SubTotal { get; set; }
        public double DiscountAmount
        {
            get; set;
        }
        public double TotalAmount { get; set; }

        public bool BookedStatus { get; set; }

    }


    public class Bs
    {

        public long ItemId { get; set; }
        public DateOnly DateToplay { get; set; }
        public TimeOnly FromTime { get; set; }
        public TimeOnly Totime { get; set; }
        public double TotalAmount { get; set; }

    }

    public class Brr
    {

        public long BookingId { get; set; }
        public DateTime DateOfBooked { get; set; }
        public long BookingCartTypeId { get; set; }
        [ForeignKey("User")]
        public long UserId { get; set; }
        [ForeignKey("Arena")]
        public long ArenaId { get; set; }

        public List<Bs> Cart_Items { get; set; }



        public double SubTotal { get; set; }
        public double DiscountAmount
        {
            get; set;
        }
        public double TotalAmount { get; set; }

        public bool BookedStatus { get; set; }

    }
    public class date1
    {
        public DateOnly Date { get; set; }
    }














}