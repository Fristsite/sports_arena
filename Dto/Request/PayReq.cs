using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Dto.Request
{
    public class PayReq
    {
        [ForeignKey("Booking")]
        public long BookingId { get; set; }
        public string PaymentMethod { get; set; }
        public double Amount { get; set; }
        public string TransactionId{get;set;}

    }
}