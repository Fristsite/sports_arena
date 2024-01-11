using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Models
{
    public class SportsKit
    {
#nullable disable
        [Key]
        public long ItemId { get; set; }
        [Required]
        [ForeignKey("Arena")]
        public long ArenaId { get; set; }
        [ForeignKey("Game")]
        public long GameId { get; set; }
        public string SportsKitDescription { get; set; }
        public double PricePerHour { get; set; }

        public int Count { get; set; }

        public bool IsActive { get; set; }


        public virtual Arena Arena { get; set; }
        public virtual Game Game { get; set; }
        public virtual ICollection<BookingSportskit> BookingSportskits { get; set; }


    }
}