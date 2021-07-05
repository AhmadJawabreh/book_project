using BusinessLogic.Manngers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("Harvester")]
    [ApiController]
    public class HarvesterController : Controller
    {
        private readonly IHarvesterManager _harvesterManager;

        public HarvesterController(IHarvesterManager harvesterManager)
        {
            _harvesterManager = harvesterManager;
        }

        [HttpGet]
        public IActionResult RestViewCaches()
        {
            _harvesterManager.RestViewCaches();
            return Ok();
        }
    }
}
