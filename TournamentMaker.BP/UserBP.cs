using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using TournamentMaker.BO;
using TournamentMaker.DAL.Interfaces;
using TournamentMaker.SAL;

namespace TournamentMaker.BP
{
    public class UserBP
    {
        private readonly IPlayerStore _playerStore;
        private readonly ISportStore _sportStore;

        public UserBP(IPlayerStore playerStore, ISportStore sportStore)
        {
            if (playerStore == null) throw new ArgumentNullException("playerStore");
            if (sportStore == null) throw new ArgumentNullException("sportStore");
            _playerStore = playerStore;
            _sportStore = sportStore;
        }

        public async Task<Player> Get(string matricule)
        {

            Player joueur = await _playerStore.GetFromMatricule(matricule);
                if (joueur != null) return joueur;

                using (var activeDirectoryRepository = new ActiveDirectoryService())
                {
                    using (var user = activeDirectoryRepository.GetUser(matricule))
                    {
                        if (user == null)
                            //throw new IdentityNotMappedException();
                            joueur = new Player
                            {
                                Email = "a@a.com",
                                Firstname = "Prénom",
                                Lastname = "Nom",
                                Matricule = matricule,
                                Picture = null
                            };
                        else
                        {
                            var picture = activeDirectoryRepository.GetUserPicture(matricule);
                            joueur = new Player
                            {
                                Lastname = user.Surname,
                                Firstname = user.GivenName,
                                Email = user.EmailAddress,
                                Matricule = matricule,
                                Picture = picture
                            };
                        }
                        var sports = await _sportStore.Get();
                        joueur.Ranks = sports.Select(s => new Rank {Level = 1000, SportKey = s.Key}).ToList();
                        await _playerStore.Add(joueur);
                        await _playerStore.SaveChangesAsync();
                    }
                }
                return joueur;
        }

        public async Task<ICollection<Player>> Get()
        {
            return await _playerStore.Get();
        }
    }
}