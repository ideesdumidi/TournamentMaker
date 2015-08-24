using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using TournamentMaker.BO;
using TournamentMaker.BP;
using TournamentMaker.Models;

namespace TournamentMaker.Controllers
{
    public class MatchController : ApiController
    {
        private readonly MatchBP _matchBP;
        private readonly string _userMatricule;

        public MatchController(MatchBP matchBP)
        {
            if (matchBP == null) throw new ArgumentNullException("matchBP");
            _matchBP = matchBP;

            if(System.Web.HttpContext.Current.User != null)
            _userMatricule = System.Web.HttpContext.Current.User.Identity.Name;
        }

        public async Task<IHttpActionResult> Get()
        {
            return Ok((await _matchBP.Get()).Select(MatchModel.From).ToList());
        }

        public async Task<IHttpActionResult> Get(int id)
        {
            return Ok(MatchModel.From(await _matchBP.Get(id)));
        }

        public async Task<IHttpActionResult> GetBySport(string sport)
        {
            return Ok((await _matchBP.Get()).Where(s=>s.SportKey == sport).Select(MatchModel.From).ToList());
        }

        public async Task<IHttpActionResult> GetByCreator(string creator)
        {
            return Ok((await _matchBP.Get()).Where(s => s.CreatorId == creator).Select(MatchModel.From).ToList());
        }

        public async Task<IHttpActionResult> Put(int id, Match match)
        {
            var newMatch = await _matchBP.Update(match, _userMatricule);
            return Ok(MatchModel.From(newMatch));
        }

        public async Task<IHttpActionResult> Post(Match match)
        {
            var newMatch = await _matchBP.Add(match, _userMatricule);
            return Ok(MatchModel.From(newMatch));
        }
        public async Task<IHttpActionResult> Delete(int id)
        {
            return Ok(await _matchBP.Remove(id, _userMatricule));
        }

        [Route("api/match/close")]
        public async Task<IHttpActionResult> Close(Match match)
        {
            return Ok(MatchModel.From(await _matchBP.Close(match, _userMatricule)));
        }
        [Route("api/match/start")]
        public async Task<IHttpActionResult> Start(Match match)
        {
            return Ok(MatchModel.From(await _matchBP.Start(match, _userMatricule)));
        }
        [Route("api/match/randomize")]
        public async Task<IHttpActionResult> Randomize(Match match)
        {
            return Ok(MatchModel.From(await _matchBP.Randomize(match, _userMatricule)));
        }

        [Route("api/match/join")]
        public async Task<IHttpActionResult> Join(Match match, int teamId, string matricule)
        {
            return Ok(MatchModel.From(await _matchBP.Join(match, teamId, matricule, _userMatricule)));
        }
        [Route("api/match/join")]
        public async Task<IHttpActionResult> Join(Match match, int teamId )
        {
            return Ok(MatchModel.From(await _matchBP.Join(match, teamId, _userMatricule, _userMatricule)));
        }
        [Route("api/match/leave")]
        public async Task<IHttpActionResult> Leave(Match match, string matricule)
        {
            return Ok(MatchModel.From(await _matchBP.Leave(match, matricule, _userMatricule)));
        }
        [Route("api/match/leave")]
        public async Task<IHttpActionResult> Leave(Match match)
        {
            return Ok(MatchModel.From(await _matchBP.Leave(match, _userMatricule, _userMatricule)));
        }
    }
}