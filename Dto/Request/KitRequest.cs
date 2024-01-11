using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Dto.Request
{
    public class KitRequest
    {
        [Required]
        [ForeignKey("Arena")]
        public long ArenaId { get; set; }
        [ForeignKey("Game")]
        public long GameId { get; set; }
        public string SportsKitDescription { get; set; }
        public double PricePerHour { get; set; }

        public int Count { get; set; }
    }
}