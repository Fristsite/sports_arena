using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Dto.Request
{
    #nullable disable
    public class CourtReq
    {
        [Required]
        [ForeignKey("Arena")]
        public long ArenaId {get;set;}
        [ForeignKey("Game")]
        public long GameId {get;set;}
        [Required]
        public string CourtName {get;set;}
        public string CourtDiscription{get;set;}
        public double Price{get;set;}
    }
}