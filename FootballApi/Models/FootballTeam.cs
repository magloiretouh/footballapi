﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FootballApi.Models
{
    public class FootballTeam
    {
        public int FootballTeamID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        [Display(Name = "Football Championship")]
        public int ChampionshipID { get; set; }
        public List<Player> Players { get; set; }
        public List<Stadium> Stadiums { get; set; }
        public List<Transaction> LeavingTransactions { get; set; }
        public List<Transaction> ComingTransactions { get; set; }
    }
}
