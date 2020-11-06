using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FootballApi.Models
{
    public class Transaction
    {
        public int TransactionID { get; set; }
        [Display(Name = "Player")]
        public int PlayerID { get; set; }
        public Player Player { get; set; }
        [Display(Name = "Leaving Team")]
        public int LeavingTeamID { get; set; }
        [ForeignKey("LeavingTeamID")]
        public virtual FootballTeam LeavingTeam { get; set; }
        [Display(Name = "Coming Team")]
        public int ComingTeamID { get; set; }
        [ForeignKey("ComingTeamID")]
        public virtual FootballTeam ComingTeam { get; set; }
    }
}
