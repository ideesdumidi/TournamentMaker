using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using TournamentMaker.BO;
using TournamentMaker.BO.Tournaments;

namespace TournamentMaker.Models
{
    public class EliminationTournamentModel : TournamentModel
    {
        public int Sleeves { get; set; }
        public int TeamsByMatch { get; set; }
    }

    public class PoolEliminationTournamentModel : TournamentModel
    {
        public bool PoolFreeForAll { get; set; }
        public int QualificationSleeves { get; set; }
        public int PoolSleeves { get; set; }
        public int TeamsByMatch { get; set; }
    }

    public class PoolTournamentModel : TournamentModel
    {
        public bool FreeForAll { get; set; }
        public int Sleeves { get; set; }
    }

    public class RoundTournamentModel : TournamentModel
    {
        public int Sleeves { get; set; }
        public int TeamsByMatch { get; set; }
    }

    public class TournamentModel
    {
        public string Type { get; set; }
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string CreatorId { get; set; }
        public PlayerModel Creator { get; set; }

        public int? WinnerId { get; set; }
        public TeamModel Winner { get; set; }

        public virtual ICollection<TeamModel> Teams { get; set; }
        public string Name { get; set; }
        public bool Private { get; set; }
        public bool Ranked { get; set; }
        public bool Randomize { get; set; }
        public string SportKey { get; set; }
        public SportModel Sport { get; set; }
        public StateEnum State { get; set; }
        public ICollection<QualificationModel> Qualifications { get; set; }

        private static void From(TournamentModel tournamentModel, Tournament tournament)
        {
            if (tournament == null)
                return;

                tournamentModel.Type = tournament.Type;
                tournamentModel.CreatorId = tournament.CreatorId;
                tournamentModel.Creator = PlayerModel.From(tournament.Creator);
                tournamentModel.Date = tournament.Date;
                tournamentModel.Id = tournament.Id;
                tournamentModel.Name = tournament.Name;
                tournamentModel.Private = tournament.Private;
                tournamentModel.Ranked = tournament.Ranked;
                tournamentModel.Randomize = tournament.Randomize;
                tournamentModel.SportKey = tournament.SportKey;
                tournamentModel.WinnerId = tournament.WinnerId;
                tournamentModel.Teams = tournament.Teams.Select(TeamModel.From).ToList();
                tournamentModel.State = tournament.State;
            tournamentModel.Qualifications = tournament.Qualifications.Select(QualificationModel.From).ToList();

        }

        public static TournamentModel From(Tournament tournament)
        {
            if (tournament == null)
                return null;

            if (tournament is EliminationTournament)
                return From((EliminationTournament)tournament);
            if (tournament is PoolTournament)
                return From((PoolTournament)tournament);
            if (tournament is PoolEliminationTournament)
                return From((PoolEliminationTournament)tournament);
            if (tournament is RoundTournament)
                return From((RoundTournament)tournament);

            return null;
        }

        public static EliminationTournamentModel From(EliminationTournament tournament)
        {
            if (tournament == null)
                return null;

            var model = new EliminationTournamentModel();
            From(model, tournament);

            model.Sleeves = tournament.Sleeves;
            model.TeamsByMatch = tournament.TeamsByMatch;

            return model;
        }
        public static PoolTournamentModel From(PoolTournament tournament)
        {
            if (tournament == null)
                return null;

            var model = new PoolTournamentModel();
            From(model, tournament);

            model.Sleeves = tournament.Sleeves;
            model.FreeForAll = tournament.FreeForAll;

            return model;
        }
        public static PoolEliminationTournamentModel From(PoolEliminationTournament tournament)
        {
            if (tournament == null)
                return null;

            var model = new PoolEliminationTournamentModel();
            From(model, tournament);

            model.PoolFreeForAll = tournament.PoolFreeForAll;
            model.QualificationSleeves = tournament.QualificationSleeves;
            model.PoolSleeves = tournament.PoolSleeves;
            model.TeamsByMatch = tournament.TeamsByMatch;

            return model;
        }
        public static RoundTournamentModel From(RoundTournament tournament)
        {
            if (tournament == null)
                return null;

            var model = new RoundTournamentModel();
            From(model, tournament);

            model.Sleeves = tournament.Sleeves;
            model.TeamsByMatch = tournament.TeamsByMatch;

            return model;
        }
    }
}