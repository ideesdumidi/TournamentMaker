using System;
using System.Threading.Tasks;
using System.Web.Http;
using TournamentMaker.BP;
using TournamentMaker.Models;

namespace TournamentMaker.Controllers
{
    public class IdentityController : ApiController
    {
        private readonly UserBP _userBP;

        public IdentityController(UserBP userBP)
        {
            if (userBP == null) throw new ArgumentNullException("userBP");
            _userBP = userBP;
        }

        public async Task<IHttpActionResult> Get()
        {
            var matricule = System.Web.HttpContext.Current.User.Identity.Name;

            try
            {
                return Ok( PlayerModel.From(await _userBP.Get(matricule)));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        } 
    }
}
