using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using TournamentMaker.BP;
using TournamentMaker.Models;

namespace TournamentMaker.Controllers
{
    public class PlayerController : ApiController
    {
        private readonly UserBP _userBP;

        public PlayerController(UserBP userBP)
        {
            if (userBP == null) throw new ArgumentNullException("userBP");
            _userBP = userBP;
        }

        public async Task<IHttpActionResult> Get()
        {
            try
            {
                return Ok((await _userBP.Get()).Select(PlayerModel.From).ToList());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        } 
    }
}
