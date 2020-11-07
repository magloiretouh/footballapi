using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FootballApi.Models
{
    [DataContract]
    public class Championship
    {
        [DataMember]
        public int ChampionshipID { get; set; }
        [DataMember]
        public string Name { get; set; }
        public List<FootballTeam> FootballTeams { get; set; }
    }
}
