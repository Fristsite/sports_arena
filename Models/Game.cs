using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Models
{
#nullable disable
    public class Game
    {
        [Key]
        public long GameId { get; set; }
        [Required]
        public string GameName { get; set; }

        public virtual SportsKit SportsKit{ get; set; }

        public virtual ICollection<CourtDetails> CourtDetails { get; set; }
    }
     public class GameReq
    {
          
          [Required]
        public string GameName { get; set; }

  }
   public class GameReqs
    {
        
        public long GameId { get; set; }
        [Required]
        public string GameName { get; set; }

  }
}