using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Management.Instrumentation;
using System.Threading.Tasks;
using Microsoft.Practices.ObjectBuilder2;
using TournamentMaker.BO;
using TournamentMaker.DAL;
using TournamentMaker.DAL.Interfaces;

namespace TournamentMaker.BP
{
    public class MatchBP
    {
        private readonly IMatchStore _matchStore;
        private readonly IPlayerStore _playerStore;
        private readonly ITeamStore _teamStore;
        private readonly ISportStore _sportStore;

        public MatchBP(IMatchStore matchStore, IPlayerStore playerStore, ITeamStore teamStore, ISportStore sportStore)
        {
            if (matchStore == null) throw new ArgumentNullException("matchStore");
            if (playerStore == null) throw new ArgumentNullException("playerStore");
            if (teamStore == null) throw new ArgumentNullException("teamStore");
            if (sportStore == null) throw new ArgumentNullException("sportStore");

            _matchStore = matchStore;
            _playerStore = playerStore;
            _teamStore = teamStore;
            _sportStore = sportStore;
        }


        public async Task<ICollection<Match>> Get()
        {
            ICollection<Match> matches = await _matchStore.Get();
            return matches;
        }

        public async Task<Match> Get(int id)
        {
            Match match = await _matchStore.Get(id);
            return match;
        }

        public async Task<Match> Update(Match match, string userMatricule)
        {
            //récupération du match existant
            Match currentMatch = await Get(match.Id);
            if (currentMatch == null)
            {
                throw new NullReferenceException(string.Format("le match avec l'id {0} est introuvable", match.Id));
            }

            if (currentMatch.CreatorId != userMatricule)
                throw new InvalidOperationException("Seul le créateur du match peut le modifier");

            //suppression des équipes
            currentMatch.Teams.ToList().ForEach(t => _teamStore.SetState(t, EntityState.Deleted));
            currentMatch.Teams.Clear();


            //recréation des équipes
            List<string> matricules = match.Teams.SelectMany(p => p.Players).Where(p => p != null).Select(p => p.Matricule).ToList();
            var players = await _playerStore.GetFromMatricules(matricules);

            foreach (Team team in match.Teams.Where(team => team != null && team.Players != null))
            {
                team.Id = 0;
                team.Players = team.Players.Where(p => p != null).Select(m => players.FirstOrDefault(j => m.Matricule == j.Matricule)).Where(p => p != null).ToList();
                currentMatch.Teams.Add(team);
            }
            await _teamStore.Add(match.Teams);

            //maj du match
            currentMatch.Date = match.Date;
            currentMatch.Name = match.Name;
            currentMatch.Points = match.Points;
            currentMatch.Private = match.Private;
            currentMatch.Ranked = match.Ranked;
            currentMatch.Sleeves = match.Sleeves;
            currentMatch.SportKey = match.SportKey;

            await _matchStore.SaveChangesAsync();

            return match;
        }
        public async Task<Match> Start(Match match, string userMatricule)
        {
            //récupération du match existant
            Match currentMatch = await Get(match.Id);
            if (currentMatch == null)
            {
                throw new NullReferenceException(string.Format("le match avec l'id {0} est introuvable", match.Id));
            }


            if (currentMatch.State != StateEnum.Opened)
                throw new InvalidOperationException("Le match est déjà en cours ou clos");

            if (currentMatch.CreatorId != userMatricule)
                throw new InvalidOperationException("Seul le créateur du match peut le démarrer");
            
            if(currentMatch.Randomize)
            RandomizeTeams(match);

            currentMatch.State = StateEnum.InProgress;

            await _matchStore.SaveChangesAsync();

            return currentMatch;
        }
        public async Task<Match> Randomize(Match match, string userMatricule)
        {
            //récupération du match existant
            Match currentMatch = await Get(match.Id);
            if (currentMatch == null)
            {
                throw new NullReferenceException(string.Format("le match avec l'id {0} est introuvable", match.Id));
            }


            if (currentMatch.State != StateEnum.Opened)
                throw new InvalidOperationException("Le match est déjà en cours ou clos");

            if (currentMatch.CreatorId != userMatricule)
                throw new InvalidOperationException("Seul le créateur du match peut le modifier");

                RandomizeTeams(currentMatch, false);

            await _matchStore.SaveChangesAsync();

            return currentMatch;
        }

        public async Task<Match> Add(Match match, string userMatricule)
        {
            match.CreatorId = userMatricule;
            foreach (Team team in match.Teams.Where(team => team != null && team.Players != null))
            {
                List<string> players = team.Players.Where(p => p != null).Select(p => p.Matricule).ToList();
                team.Players = await _playerStore.GetFromMatricules(players);
            }
            await _teamStore.Add(match.Teams);
            await _matchStore.Add(match);
            await _matchStore.SaveChangesAsync();

            return match;
        }

        public async Task<Match> Close(Match match, string userMatricule)
        {
            var currentMatch = await _matchStore.GetWithPlayersAndRanks(match.Id);

            if (currentMatch.State == StateEnum.Draw || currentMatch.State == StateEnum.Won)
                throw new InvalidOperationException("Le match est déjà clos");

            if (string.IsNullOrWhiteSpace(match.Scores))
                throw new InvalidOperationException("Aucun résultat n'a été renseigné");


            if (currentMatch.CreatorId != userMatricule && !currentMatch.Teams.SelectMany(t => t.Players).Select(p => p.Matricule).Contains(userMatricule))
                throw new InvalidOperationException("Seuls le créateur et les joueurs d'un match peuvent le clore");

            //suppression des équipes vides
            currentMatch.Teams.Where(t => t.Players.Count == 0).ToList().ForEach(t => _teamStore.SetState(t, EntityState.Deleted));

            currentMatch.Teams = currentMatch.Teams.Where(t => t.Players.Count > 0).ToList();
            if (currentMatch.Teams.Count < 2)
                throw new InvalidOperationException("Il faut au minimum 2 équipes pour clore un match");


            //calcul des scores
            var manches = new List<Dictionary<Team, int>>();
            foreach (string mancheString in match.Scores.Split(';').Where(s => !string.IsNullOrWhiteSpace(s)).ToList())
            {
                var score = new Dictionary<Team, int>();
                string[] scoreString = mancheString.Split('-');
                Team[] teams = currentMatch.Teams.ToArray();
                for (int i = 0; i < scoreString.Length; i++)
                {
                    if (currentMatch.Teams.Count < i)
                        continue;
                    var scores = scoreString[i].Split(':').Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                    var teamId = 0;
                    int.TryParse(scores[0], out teamId);
                    var result = 0;

                    var team = teams.FirstOrDefault(t => t.Id == teamId);
                    if (team != null && int.TryParse(scores[1], out result)) score.Add(team, result);
                }
                manches.Add(score);
            }

            if (manches.Count == 0)
                throw new InvalidOperationException("Il faut au minimum une manche de jouée pour clore un match");
            
            //selection du vainqueur, si vainqueur il y a:
            //L'équipe qui à le plus de manches gagnés

            //Si plusieurs vaiqueurs exaeco par manche: manche nulle
            var manchesGagnes = currentMatch.Teams.ToDictionary(t => t, t => 0);
            foreach (var equipes in manches.Select(m => m.Where(t => t.Value == m.Max(s => s.Value)).Select(t => t.Key).ToList()).Where(e => e.Count() == 1))
            {
                manchesGagnes[equipes.First()] += 1;
            }

            //Si plusieurs vainqueurs: égalité
            var victoiresMax = manchesGagnes.Max(m => m.Value);
            var manches2 = manchesGagnes.Where(t => t.Value.Equals(victoiresMax));
            var winners = manches2.Select(t => t.Key).ToList();
            if (winners.Count() == 1)
            {
                currentMatch.WinnerId = winners.First().Id;
                currentMatch.State = StateEnum.Won;
            }
            else
            {
                currentMatch.State = StateEnum.Draw;
            }

            currentMatch.CloseDate = DateTime.Now;
            currentMatch.Scores = match.Scores;

            //calculs des classements si le match est ranké
            if (currentMatch.Ranked)
            {
                Dictionary<Rank, double> eloResults =
                    currentMatch.Teams.SelectMany(t => t.Players).SelectMany(p => p.Ranks).Where(r => r.SportKey == currentMatch.SportKey).ToDictionary(t => t, t => 0.0d);

                int playersCount = eloResults.Count();
                if (playersCount == 0)
                    throw new InvalidOperationException("Aucun joueur ne participe à ce match");

                foreach (Team team in currentMatch.Teams)
                {
                    int teamId = team.Id;
                    List<Team> otherTeams = currentMatch.Teams.Where(t => t.Id != teamId).ToList();

                    int level1 = team.Players.Sum(p => p.Ranks.Where(r => r.SportKey == currentMatch.SportKey).Select(r => r.Level).FirstOrDefault())/team.Players.Count;

                    int level2 = otherTeams.Sum(t => t.Players.Sum(p => p.Ranks.Where(r => r.SportKey == currentMatch.SportKey).Select(r => r.Level).FirstOrDefault()))/
                                 otherTeams.Count();

                    int teamScore = (from manche in manches from score in manche where score.Key.Id == teamId select score.Value).Sum()/manches.Count();

                    foreach (Team otherTeam in otherTeams)
                    {
                        int otherTeamScore = (from manche in manches from score in manche where score.Key.Id == otherTeam.Id select score.Value).Sum()/manches.Count();

                        foreach (Player player in team.Players)
                        {
                            Rank playerRank = player.Ranks.FirstOrDefault(r => r.SportKey == currentMatch.SportKey);

                            if (playerRank == null) continue;

                            ResultatEnum matchState = currentMatch.State == StateEnum.Draw
                                ? ResultatEnum.Nul
                                : (currentMatch.WinnerId == team.Id) ? ResultatEnum.Victoire : ResultatEnum.Defaite;
                            eloResults[playerRank] += CalculateElo(player, level1, level2, teamScore, otherTeamScore, 1.0/otherTeams.Count, matchState);
                        }
                    }
                }

                //Maj du classement
                foreach (var eloResult in eloResults)
                {
                    eloResult.Key.Level += (int) Math.Round(eloResult.Value);
                }
            }

            await _matchStore.SaveChangesAsync();

            return await Task.FromResult(currentMatch);
        }

        private void RandomizeTeams(Match match, bool removeEmptyTeams = true)
        {
            //récupération des joueurs
            var rnd = new Random();
            List<Player> players = match.Teams.SelectMany(p => p.Players).Where(p => p != null).OrderBy(item => rnd.Next()).ToList();

            match.Teams.ForEach(t => t.Players.Clear());
            while (players.Count > 0)
            {
                foreach (var team in match.Teams)
                {
                    if (players.Count <= 0)
                        break;
                    var player = players.First();
                    players.RemoveAt(0);
                    team.Players.Add(player);
                }
            }

            if (!removeEmptyTeams)
                return;

            //suppression des équipes vides
            match.Teams.Where(t => t.Players.Count == 0).ToList().ForEach(t => _teamStore.SetState(t, EntityState.Deleted));

            match.Teams = match.Teams.Where(t => t.Players.Count > 0).ToList();
        }

        private double CalculateElo(Player player, int level1, int level2, int score1, int score2, double poids, ResultatEnum result)
        {
            //résultat théorique
            double probability = VictoryProbability(level1 - level2);

            //résultat du match
            double w = 0;

            //poids selon le niveau du joueur et et le type de compétition
            double k = 10;

            //coeff selon la différence de but
            double g;


            //calcul du résultat
            switch (result)
            {
                case ResultatEnum.Defaite:
                    w = 0;
                    break;
                case ResultatEnum.Nul:
                    w = 0.5;
                    break;
                case ResultatEnum.Victoire:
                    w = 1;
                    break;
            }

            //calcul du poids
            if (player.Parties < 15)
                k = 25;
            else if (level1 < 2400)
                k = 15;
            k *= poids;

            //calcul du coeff
            int diff = Math.Abs(score1 - score2);
            switch (diff)
            {
                case 0:
                    g = 1;
                    break;
                case 1:
                    g = 1;
                    break;
                case 2:
                    g = 1.5;
                    break;
                case 3:
                    g = 1.75;
                    break;
                default:
                    g = 1.75 + (diff - 3)*0.125;
                    break;
            }

            return (k*g*(w - probability));
        }

        private static double VictoryProbability(int difference)
        {
            return 1/(1 + Math.Pow(10, -difference*0.0025));
        }

        public async Task<Match> Join(Match match, int teamId, string playerMatricule, string userMatricule)
        {
            var currentMatch = await _matchStore.Get(match.Id);

            if (currentMatch == null)
                throw new InstanceNotFoundException("Le match est introuvable");


            if (currentMatch.State == StateEnum.Draw || currentMatch.State == StateEnum.Won)
                throw new InvalidOperationException("Le match est déjà clos");

            if (playerMatricule != userMatricule && currentMatch.CreatorId != userMatricule)
                throw new InvalidOperationException("Seul le créateur du match peut modifier sa composition");

            var player = await _playerStore.GetFromMatricule(playerMatricule);
            if (player == null)
                throw new InvalidOperationException("Le joueur est introuvable");

            if (currentMatch.Teams.SelectMany(t => t.Players).Select(p => p.Matricule.ToLowerInvariant()).Contains(playerMatricule.ToLowerInvariant()))
                throw new InvalidOperationException("Le joueur fait déjà parti d'une équipe");

            var team = currentMatch.Teams.SingleOrDefault(t => t.Id == teamId);
            if (team == null)
                throw new InvalidOperationException("L'équipe n'est pas liée au match");

            var currentSport = await _sportStore.Get(currentMatch.SportKey);
            if (team.Players.Count >= currentSport.MaxPlayers)
                throw new InvalidOperationException("L'équipe est déjà complète");

            team.Players.Add(player);
            await _playerStore.SaveChangesAsync();

            return await Task.FromResult(currentMatch);
        }

        public async Task<Match> Leave(Match match, string playerMatricule, string userMatricule)
        {
            var currentMatch = await _matchStore.Get(match.Id);

            if (currentMatch == null)
                throw new InstanceNotFoundException("Le match est introuvable");

            if (currentMatch.State == StateEnum.Draw || currentMatch.State == StateEnum.Won)
                throw new InvalidOperationException("Le match est déjà clos");

            if (playerMatricule != userMatricule && currentMatch.CreatorId != userMatricule)
                throw new InvalidOperationException("Seul le créateur du match peut modifier sa composition");

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

        public async Task<bool> Remove(int matchId, string userMatricule)
        {
            var currentMatch = await _matchStore.Get(matchId);

            if (currentMatch == null)
                throw new InstanceNotFoundException("Le match est introuvable");

            if (currentMatch.State == StateEnum.Draw || currentMatch.State == StateEnum.Won)
                throw new InvalidOperationException("Le match est déjà clos");

            if (currentMatch.CreatorId != userMatricule)
                throw new InvalidOperationException("Seul le créateur du match peut le supprimer");


            //suppression des équipes
            currentMatch.Teams.ToList().ForEach(t => _teamStore.SetState(t, EntityState.Deleted));
            currentMatch.Teams.Clear();

            //suppression du match
            await _matchStore.SetState(currentMatch, EntityState.Deleted);

            await _playerStore.SaveChangesAsync();

            return await Task.FromResult(true);
        }
    }
}

public enum ResultatEnum
{
    Defaite = -1,
    Nul = 0,
    Victoire = 1,
}