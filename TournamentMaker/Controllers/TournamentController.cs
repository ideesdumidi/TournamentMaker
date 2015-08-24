using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using TournamentMaker.BO;
using TournamentMaker.BO.Tournaments;
using TournamentMaker.BP;
using TournamentMaker.Models;

namespace TournamentMaker.Controllers
{
    public class TournamentController : ApiController
    {
        private readonly TournamentBP _tournamentBP;
        private readonly string _userMatricule;

        public TournamentController(TournamentBP tournamentBP)
        {
            if (tournamentBP == null) throw new ArgumentNullException("tournamentBP");
            _tournamentBP = tournamentBP;

            if(System.Web.HttpContext.Current.User != null)
            _userMatricule = System.Web.HttpContext.Current.User.Identity.Name;
        }

        public async Task<IHttpActionResult> Get()
        {
            return Ok((await _tournamentBP.Get()).Select(TournamentModel.From).ToList());
        }

        public async Task<IHttpActionResult> Get(int id)
        {
            var tournament = await _tournamentBP.Get(id);
            if(tournament != null)
            return Ok(TournamentModel.From(tournament));
            return NotFound();
        }

        public async Task<IHttpActionResult> GetBySport(string sport)
        {
            return Ok((await _tournamentBP.Get()).Where(s=>s.SportKey == sport).Select(TournamentModel.From).ToList());
        }

        public async Task<IHttpActionResult> GetByCreator(string creator)
        {
            return Ok((await _tournamentBP.Get()).Where(s => s.CreatorId == creator).Select(TournamentModel.From).ToList());
        }

        public async Task<IHttpActionResult> Put(int id, Tournament tournament)
        {
            var newMatch = await _tournamentBP.Update(tournament, _userMatricule);
            return Ok(TournamentModel.From(newMatch));
        }

        public async Task<IHttpActionResult> Post(Tournament tournament)
        {
            var newMatch = await _tournamentBP.Add(tournament, _userMatricule);
            return Ok(TournamentModel.From(newMatch));
        }
        public async Task<IHttpActionResult> Delete(int id)
        {
            return Ok(await _tournamentBP.Remove(id, _userMatricule));
        }

        [Route("api/tournament/start")]
        public async Task<IHttpActionResult> Start(Tournament tournament)
        {
            return Ok(TournamentModel.From(await _tournamentBP.Start(tournament, _userMatricule)));
        }
        [Route("api/tournament/randomize")]
        public async Task<IHttpActionResult> Randomize(Tournament tournament)
        {
            return Ok(TournamentModel.From(await _tournamentBP.Randomize(tournament, _userMatricule)));
        }

        [Route("api/tournament/join")]
        public async Task<IHttpActionResult> Join(Tournament tournament, int teamId, string matricule)
        {
            return Ok(TournamentModel.From(await _tournamentBP.Join(tournament, teamId, matricule, _userMatricule)));
        }
        [Route("api/tournament/join")]
        public async Task<IHttpActionResult> Join(Tournament tournament, int teamId )
        {
            return Ok(TournamentModel.From(await _tournamentBP.Join(tournament, teamId, _userMatricule, _userMatricule)));
        }
        [Route("api/tournament/leave")]
        public async Task<IHttpActionResult> Leave(Tournament tournament, string matricule)
        {
            return Ok(TournamentModel.From(await _tournamentBP.Leave(tournament, matricule, _userMatricule)));
        }
        [Route("api/tournament/leave")]
        public async Task<IHttpActionResult> Leave(Tournament tournament)
        {
            return Ok(TournamentModel.From(await _tournamentBP.Leave(tournament, _userMatricule, _userMatricule)));
        }
    }
}