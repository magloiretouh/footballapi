using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FootballApi.Models
{
    public class Stadium
    {
        public int StadiumID { get; set; }
        public string Name { get; set; }
        [Display(Name = "Number of stadium places")]
        public int NumberOfPlaces { get; set; }
        [Display(Name = "Team")]
        public int FootballTeamID { get; set; }
    }
}
