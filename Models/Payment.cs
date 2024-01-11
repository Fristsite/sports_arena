using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Models
{
#nullable disable
    public class Payment
    {
        [Key]
        public long PaymentId { get; set; }
        [ForeignKey("Booking")]
        public long BookingId { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public double Amount { get; set; }
        public string TransactionId{get;set;}
        public bool PaymentStatus { get; set; }


        public virtual Booking Booking { get; set; }
    }
}