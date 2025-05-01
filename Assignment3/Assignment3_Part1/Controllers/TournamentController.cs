using Microsoft.AspNetCore.Mvc;

namespace Assignment3_Part1.Controllers
{
    [ApiController]
    [Route("api/tournament")]
    public class TournamentController : Controller
    {
        private readonly TournamentService _service = new TournamentService();

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTournament(int id, [FromQuery] DateTime newStartDate)
        {
            var success = await _service.UpdateTournamentOptimisticAsync(id, newStartDate);
            if (!success)
                return Conflict("Conflict detected with optimistic locking.");
            return Ok("Tournament updated.");
        }
    }
}
