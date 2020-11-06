using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FootballApi.Models
{
    public class Player
    {
        public int PlayerID { get; set; }
        public string FullName { get; set; }
        public string Surname { get; set; }
        [Display(Name = "Height (1-100)")]
        public int Height { get; set; }
        [Display(Name = "Weight (1-100)")]
        public int Weight { get; set; }
        [Display(Name = "Speed (1-100)")]
        public int Speed { get; set; }
        [Display(Name = "Finishing (1-100)")]
        public int Finishing { get; set; }
        [Display(Name = "Freekick (1-100)")]
        public int FreeKick { get; set; }
        [Display(Name = "Dribbling (1-100)")]
        public int Dribbling { get; set; }
        [Display(Name = "Team")]
        public int FootballTeamID { get; set; }
        public virtual FootballTeam FootballTeam { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
