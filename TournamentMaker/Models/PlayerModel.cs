using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TournamentMaker.BO;

namespace TournamentMaker.Models
{
    public class PlayerModel
    {
        [Key]
        public string Matricule { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Email { get; set; }
        public string Picture { get; set; }

        public static PlayerModel From(Player player)
        {
            if (player == null)
                return null;
            var playerModel = new PlayerModel
            {
                Matricule = player.Matricule,
                Lastname = player.Lastname,
                Firstname = player.Firstname,
                Email = player.Email,
                Picture = player.Picture
            };

            return playerModel;
        }

    }
}