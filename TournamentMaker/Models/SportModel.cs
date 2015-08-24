using System;

namespace TournamentMaker.Models
{
    public class SportModel
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public int MaxPlayers { get; set; }
        public int MaxTeams { get; set; }
        public string Key { get; set; }
    }
}