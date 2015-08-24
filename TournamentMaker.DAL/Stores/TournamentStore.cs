using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TournamentMaker.BO;
using TournamentMaker.BO.Tournaments;
using TournamentMaker.DAL.Interfaces;

namespace TournamentMaker.DAL.Stores
{
    public class TournamentStore : ITournamentStore
    {
        private readonly MatchContext _matchContext;

        public TournamentStore(MatchContext matchContext)
        {
            _matchContext = matchContext;
        }

        public async Task<Tournament> Get(int id)
        {
            return await _matchContext.Tournaments.Include("Teams").Include("Teams.Players").Include("Teams.Players").Include("Qualifications").Include("Qualifications.Matchs").Include("Qualifications.Matchs.Teams").Include("Qualifications.Matchs.Teams.Players").FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task<ICollection<Tournament>> Get()
        {
            return await _matchContext.Tournaments.Include("Qualifications").Include("Teams").Include("Teams.Players").OrderBy(e => e.Date).Take(10).ToListAsync();
        }

        public async Task<Tournament> Add(Tournament tournament)
        {
            _matchContext.Tournaments.Add(tournament);
            return await Task.FromResult(tournament);
        }

        public async Task<Tournament> SetState(Tournament tournament, EntityState entityState)
        {
            _matchContext.Entry(tournament).State = entityState;
            return await Task.FromResult(tournament);
        }

        public async Task SaveChangesAsync()
        {
            try
            {
            await _matchContext.SaveChangesAsync();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationError in dbEx.EntityValidationErrors.SelectMany(validationErrors => validationErrors.ValidationErrors))
                {
                    Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                }
            }
        }
    }
}
