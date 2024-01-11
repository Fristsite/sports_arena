using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Play.Dto.Request
{
    #nullable disable
    public class ArenaReq
    {
        [Required]
        public string ArenaName { get; set; }
        [Required]
        [ForeignKey("Address")]
        public long AddressId { get; set; }

        // [JsonConverter(typeof(DateTimeConverter))]

        // public DateOnly DateofAdded { get; set; }


    }
}