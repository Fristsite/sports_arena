using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Models
{
#nullable disable

    public class BookingCartType
    {

        [Key]
        public long BookingCartTypeId { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3)]

        public string BookingCartTypeName { get; set; }
        public virtual ICollection<Booking>Bookings{get;set;}

    }
    public class TypeReq
    {


        [Required]
        [StringLength(100, MinimumLength = 3)]

        public string BookingCartTypeName { get; set; }
    }
    public class typeRes
    {

        [Key]
        public long BookingCartTypeId { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3)]

        public string BookingCartTypeName { get; set; }

    }
}