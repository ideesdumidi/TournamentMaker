using TournamentMaker.BO;

namespace TournamentMaker.Models
{
    public class RankModel
    {
        public string PlayerId { get; set; }
        public PlayerModel Player { get; set; }
        public string SportKey { get; set; }
        public int Level { get; set; }
        public static RankModel From(Rank rank)
        {
            if (rank == null)
                return null;

            return new RankModel
            {
                PlayerId = rank.PlayerId,
                Player = new PlayerModel
                {
                    Firstname = rank.Player.Firstname,
                    Lastname = rank.Player.Lastname,
                    Matricule = rank.Player.Matricule,
                    Picture = rank.Player.Picture,
                },
                SportKey = rank.SportKey,
                Level = rank.Level
            };
        }
    }
}