using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TournamentMaker.BO
{
    public class Rank
    {
        [Key, Column(Order = 1)]
        public string PlayerId { get; set; }
        [ForeignKey("PlayerId")]
        public Player Player { get; set; }

        [Key, Column(Order = 0)]
        public string SportKey { get; set; }
        [ForeignKey("SportKey")]
        public Sport Sport { get; set; }

        public int Level { get; set; }
    }
}