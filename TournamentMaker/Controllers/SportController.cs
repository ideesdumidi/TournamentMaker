using System;
using System.Threading.Tasks;
using System.Web.Http;
using TournamentMaker.BP;

namespace TournamentMaker.Controllers
{
    public class SportController : ApiController
    {
        private readonly SportBP _sportBP;
        
        public SportController(SportBP sportBP)
        {
            if (sportBP == null) throw new ArgumentNullException("sportBP");
            _sportBP = sportBP;
        }

        public async Task<IHttpActionResult> Get()
        {
            try
            {
                return Ok(await _sportBP.Get());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
