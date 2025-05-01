using Microsoft.AspNetCore.Mvc;

namespace Assignment3_Part1.Controllers
{
    [ApiController]
    [Route("api/match")]
    public class MatchController : Controller
    {
        private readonly TournamentService _service = new TournamentService();

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMatch(int id, [FromQuery] int result)
        {
            await _service.UpdateMatchPessimisticAsync(id, result);
            return Ok("Match updated successfully!");
        }
    }
}
