using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Management.Instrumentation;
using System.Threading.Tasks;
using Microsoft.Practices.ObjectBuilder2;
using TournamentMaker.BO;
using TournamentMaker.BO.Tournaments;
using TournamentMaker.BP.TournamentSystems;
using TournamentMaker.DAL.Interfaces;

namespace TournamentMaker.BP
{
    public class TournamentBP
    {
        private readonly ITournamentStore _tournamentStore;
        private readonly IPlayerStore _playerStore;
        private readonly ITeamStore _teamStore;
        private readonly ISportStore _sportStore;
        private readonly ITournamentSystemFactory _tournamentSystemFactory;

        public TournamentBP(IPlayerStore playerStore, ITeamStore teamStore, ISportStore sportStore, ITournamentStore tournamentStore, ITournamentSystemFactory tournamentSystemFactory)
        {
            if (playerStore == null) throw new ArgumentNullException("playerStore");
            if (teamStore == null) throw new ArgumentNullException("teamStore");
            if (sportStore == null) throw new ArgumentNullException("sportStore");
            if (tournamentSystemFactory == null) throw new ArgumentNullException("tournamentSystemFactory");

            _playerStore = playerStore;
            _teamStore = teamStore;
            _sportStore = sportStore;
            _tournamentStore = tournamentStore;
            _tournamentSystemFactory = tournamentSystemFactory;
        }


        public async Task<ICollection<Tournament>> Get()
        {
            ICollection<Tournament> matches = await _tournamentStore.Get();
            return matches;
        }

        public async Task<Tournament> Get(int id)
        {
            Tournament tournament = await _tournamentStore.Get(id);
            return tournament;
        }

        public async Task<Tournament> Update(Tournament tournament, string userMatricule)
        {
            //récupération du tournoi existant
            Tournament currentTournament = await Get(tournament.Id);
            if (currentTournament == null)
            {
                throw new NullReferenceException(string.Format("le tournoi avec l'id {0} est introuvable", tournament.Id));
            }

            if (currentTournament.CreatorId != userMatricule)
                throw new InvalidOperationException("Seul le créateur du tournoi peut le modifier");

            //suppression des équipes
            currentTournament.Teams.ToList().ForEach(t => _teamStore.SetState(t, EntityState.Deleted));
            currentTournament.Teams.Clear();


            //recréation des équipes
            List<string> matricules = tournament.Teams.SelectMany(p => p.Players).Where(p => p != null).Select(p => p.Matricule).ToList();
            var players = await _playerStore.GetFromMatricules(matricules);

            foreach (Team team in tournament.Teams.Where(team => team != null && team.Players != null))
            {
                team.Id = 0;
                team.Players = team.Players.Where(p => p != null).Select(m => players.FirstOrDefault(j => m.Matricule == j.Matricule)).Where(p => p != null).ToList();
                currentTournament.Teams.Add(team);
            }
            await _teamStore.Add(tournament.Teams);

            //maj du match
            currentTournament.Date = tournament.Date;
            currentTournament.Name = tournament.Name;
            currentTournament.Private = tournament.Private;
            currentTournament.Ranked = tournament.Ranked;
            currentTournament.SportKey = tournament.SportKey;
            
            var tournamentSystem = _tournamentSystemFactory.Get(currentTournament);
            tournamentSystem.Create();

            await _tournamentStore.SaveChangesAsync();

            return tournament;
        }

        public async Task<Tournament> Start(Tournament tournament, string userMatricule)
        {
            //récupération du tournoi existant
            Tournament currentTournament = await Get(tournament.Id);
            if (currentTournament == null)
            {
                throw new NullReferenceException(string.Format("le tournoi avec l'id {0} est introuvable", tournament.Id));
            }


            if (currentTournament.State != StateEnum.Opened)
                throw new InvalidOperationException("Le tournoi est déjà en cours ou clos");

            if (currentTournament.CreatorId != userMatricule)
                throw new InvalidOperationException("Seul le créateur du tournoi peut le démarrer");

            if (currentTournament.Randomize)
                RandomizeTeams(tournament);

            currentTournament.State = StateEnum.InProgress;

            ///TODO: création des pools et des qualifs
            var tournamentSystem = _tournamentSystemFactory.Get(currentTournament);
            tournamentSystem.Create();

            await _tournamentStore.SaveChangesAsync();

            return currentTournament;
        }

        public async Task<Tournament> Randomize(Tournament tournament, string userMatricule)
        {
            //récupération du match existant
            Tournament currentTournament = await Get(tournament.Id);
            if (currentTournament == null)
            {
                throw new NullReferenceException(string.Format("le tournoi avec l'id {0} est introuvable", tournament.Id));
            }


            if (currentTournament.State != StateEnum.Opened)
                throw new InvalidOperationException("Le tournoi est déjà en cours ou clos");

            if (currentTournament.CreatorId != userMatricule)
                throw new InvalidOperationException("Seul le créateur du tournoi peut le modifier");

            RandomizeTeams(currentTournament);

            await _tournamentStore.SaveChangesAsync();

            return currentTournament;
        }

        public async Task<Tournament> Add(Tournament tournament, string userMatricule)
        {
            tournament.CreatorId = userMatricule;
            foreach (Team team in tournament.Teams.Where(team => team != null && team.Players != null))
            {
                List<string> players = team.Players.Where(p => p != null).Select(p => p.Matricule).ToList();
                team.Players = await _playerStore.GetFromMatricules(players);
            }

            tournament.Sport = await _sportStore.Get(tournament.SportKey);
             

            var tournamentSystem = _tournamentSystemFactory.Get(tournament);
            tournamentSystem.Tournament = tournament;
            tournamentSystem.Create();

            await _teamStore.Add(tournament.Teams);
            await _tournamentStore.Add(tournament);
            await _tournamentStore.SaveChangesAsync();

            return tournament;
        }

        private void RandomizeTeams(Tournament tournament)
        {
            //récupération des joueurs
            var rnd = new Random();
            List<Player> players = tournament.Teams.SelectMany(p => p.Players).Where(p => p != null).OrderBy(item => rnd.Next()).ToList();

            tournament.Teams.ForEach(t => t.Players.Clear());
            while (players.Count > 0)
            {
                foreach (var team in tournament.Teams)
                {
                    if (players.Count <= 0)
                        break;
                    var player = players.First();
                    players.RemoveAt(0);
                    team.Players.Add(player);
                }
            }

            //suppression des équipes vides
            tournament.Teams.Where(t => t.Players.Count == 0).ToList().ForEach(t => _teamStore.SetState(t, EntityState.Deleted));

            tournament.Teams = tournament.Teams.Where(t => t.Players.Count > 0).ToList();
        }

        public async Task<Tournament> Join(Tournament tournament, int teamId, string playerMatricule, string userMatricule)
        {
            var currentTournament = await _tournamentStore.Get(tournament.Id);

            if (currentTournament == null)
                throw new InstanceNotFoundException("Le tournoi est introuvable");


            if (currentTournament.State == StateEnum.Draw || currentTournament.State == StateEnum.Won)
                throw new InvalidOperationException("Le tournoi est déjà clos");

            if (playerMatricule != userMatricule && currentTournament.CreatorId != userMatricule)
                throw new InvalidOperationException("Seul le créateur du tournoi peut modifier sa composition");

            var player = await _playerStore.GetFromMatricule(playerMatricule);
            if (player == null)
                throw new InvalidOperationException("Le joueur est introuvable");

            if (currentTournament.Teams.SelectMany(t => t.Players).Select(p => p.Matricule.ToLowerInvariant()).Contains(playerMatricule.ToLowerInvariant()))
                throw new InvalidOperationException("Le joueur fait déjà parti d'une équipe");

            var team = currentTournament.Teams.SingleOrDefault(t => t.Id == teamId);
            if (team == null)
                throw new InvalidOperationException("L'équipe n'est pas liée au match");

            var currentSport = await _sportStore.Get(currentTournament.SportKey);
            if (team.Players.Count >= currentSport.MaxPlayers)
                throw new InvalidOperationException("L'équipe est déjà complète");

            team.Players.Add(player);
            await _playerStore.SaveChangesAsync();

            return await Task.FromResult(currentTournament);
        }

        public async Task<Tournament> Leave(Tournament tournament, string playerMatricule, string userMatricule)
        {
            var currentMatch = await _tournamentStore.Get(tournament.Id);

            if (currentMatch == null)
                throw new InstanceNotFoundException("Le tournoi est introuvable");

            if (currentMatch.State == StateEnum.Draw || currentMatch.State == StateEnum.Won)
                throw new InvalidOperationException("Le tournoi est déjà clos");

            if (playerMatricule != userMatricule && currentMatch.CreatorId != userMatricule)
                throw new InvalidOperationException("Seul le créateur du tournoi peut modifier sa composition");

            var player = await _playerStore.GetFromMatricule(playerMatricule);
            if (player == null)
                throw new InvalidOperationException("Le joueur est introuvable");

            var team = currentMatch.Teams.SingleOrDefault(t => t.Players.Select(p => p.Matricule.ToLowerInvariant()).Contains(playerMatricule.ToLowerInvariant()));
            if (team == null)
                throw new InvalidOperationException("Le joueur ne fait parti d'aucune équipe");


            team.Players.Remove(player);
            await _playerStore.SaveChangesAsync();


            return await Task.FromResult(currentMatch);
        }

        public async Task<bool> Remove(int tournamentId, string userMatricule)
        {
            var currentTournament = await _tournamentStore.Get(tournamentId);

            if (currentTournament == null)
                throw new InstanceNotFoundException("Le tournoi est introuvable");

            if (currentTournament.State == StateEnum.Draw || currentTournament.State == StateEnum.Won)
                throw new InvalidOperationException("Le tournoi est déjà clos");

            if (currentTournament.CreatorId != userMatricule)
                throw new InvalidOperationException("Seul le créateur du match peut le supprimer");


            //suppression des équipes
            currentTournament.Teams.ToList().ForEach(t => _teamStore.SetState(t, EntityState.Deleted));
            currentTournament.Teams.Clear();

            //suppression du match
            await _tournamentStore.SetState(currentTournament, EntityState.Deleted);

            await _playerStore.SaveChangesAsync();

            return await Task.FromResult(true);
        }

        public async Task<Tournament> CreateMatchs(Tournament tournament, string userMatricule)
        {
            //récupération du match existant
            Tournament currentTournament = await Get(tournament.Id);
            if (currentTournament == null)
            {
                throw new NullReferenceException(string.Format("le tournoi avec l'id {0} est introuvable", tournament.Id));
            }

            if (currentTournament.State != StateEnum.Opened)
                throw new InvalidOperationException("Le tournoi est déjà en cours ou clos");

            if (currentTournament.CreatorId != userMatricule)
                throw new InvalidOperationException("Seul le créateur du tournoi peut le modifier");

            var tournamentSystem = _tournamentSystemFactory.Get(currentTournament);
            tournamentSystem.Tournament = currentTournament;
            tournamentSystem.Create();

            await _tournamentStore.SaveChangesAsync();

            return currentTournament;
        }
    }
}