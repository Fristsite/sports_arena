using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Models
{
    #nullable disable
    public class CourtDetails
    {
        [Key]
        public long ItemId{get;set;}
        [Required]
        [ForeignKey("Arena")]
        public long ArenaId {get;set;}
        [ForeignKey("Game")]
        public long GameId {get;set;}
        [Required]
        public string CourtName {get;set;}
        public string CourtDiscription{get;set;}
        public double Price{get;set;}

        
        public bool IsActive{get;set;}


        public virtual Arena Arena{get;set;}
        public virtual Game Game {get;set;}
        public virtual ICollection<BookingCourt>BookingCourts{get;set;}
        
    }
}