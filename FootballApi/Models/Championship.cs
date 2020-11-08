using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FootballApi.Models
{
    public class Championship
    {
        public int ChampionshipID { get; set; }
        public string Name { get; set; }
        public List<FootballTeam> FootballTeams { get; set; }
    }
}
