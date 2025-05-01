using Microsoft.AspNetCore.Mvc;

namespace Assignment3_Part1.Controllers
{
    [ApiController]
    [Route("api/performance")]
    public class PerformanceController : Controller
    {
        [HttpGet("optimistic")]
        public IActionResult RunOptimistic()
        {
            var result = PerformanceTester.RunOptimisticTest();
            return Ok(result);
        }

        [HttpGet("pessimistic")]
        public IActionResult RunPessimistic()
        {
            var result = PerformanceTester.RunPessimisticTest();
            return Ok(result);
        }
    }
}
