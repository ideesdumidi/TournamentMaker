using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using TournamentMaker.BP;
using TournamentMaker.Models;

namespace TournamentMaker.Controllers
{
    public class RankController : ApiController
    {
        private readonly RankBP _rankBP;

        public RankController(RankBP rankBP)
        {
            if (rankBP == null) throw new ArgumentNullException("rankBP");
            _rankBP = rankBP;
        }

        public async Task<IHttpActionResult> Get()
        {
            try
            {
                var ranks = await _rankBP.Get();
                return Ok(ranks.Select(RankModel.From));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        public async Task<IHttpActionResult> Get(string sport)
        {
            try
            {
                var ranks = await _rankBP.Get(sport);
                return Ok(ranks.Select(RankModel.From));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}