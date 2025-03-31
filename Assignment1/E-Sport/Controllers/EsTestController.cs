using Microsoft.AspNetCore.Mvc;

namespace E_Sport.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EsTestController : ControllerBase
    {
        private readonly EsFunctions _functions;

        public EsTestController(EsFunctions functions)
        {
            _functions = functions;
        }

        [HttpPost("join")]
        public IActionResult JoinTournament(int playerId, int tournamentId)
        {
            try
            {
                _functions.CallJoinTournament(playerId, tournamentId);
                return Ok("Player joined tournament via stored procedure");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("join-direct")]
        public IActionResult JoinTournamentDirect(int playerId, int tournamentId)
        {
            try
            {
                _functions.JoinTournamentDirect(playerId, tournamentId);
                return Ok("Player joined tournament via direct SQL");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("submit")]
        public IActionResult SubmitMatch(int matchId, int winnerId)
        {
            try
            {
                _functions.CallSubmitMatchResult(matchId, winnerId);
                return Ok("Match result submitted via stored procedure");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("submit-direct")]
        public IActionResult SubmitMatchDirect(int matchId, int winnerId)
        {
            try
            {
                _functions.SubmitMatchResultDirect(matchId, winnerId);
                return Ok("Match result submitted via direct SQL");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
