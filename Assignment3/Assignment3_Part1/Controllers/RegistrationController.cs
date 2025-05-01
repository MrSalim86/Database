using Microsoft.AspNetCore.Mvc;

namespace Assignment3_Part1.Controllers
{
    [ApiController]
    [Route("api/registration")]
    public class RegistrationController : Controller
    {

        private readonly TournamentService _service = new TournamentService();

        [HttpPost]
        public async Task<IActionResult> RegisterPlayer([FromQuery] int tournamentId, [FromQuery] int playerId)
        {
            var success = await _service.RegisterPlayerAsync(tournamentId, playerId);
            if (!success)
                return Conflict("Tournament full. Registration failed.");
            return Ok("Player registered.");
        }
    }
}
