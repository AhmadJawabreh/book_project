using BusinessLogic.Manngers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{


    public class HarvesterController : Controller
    {
        internal HarvesterManager _harvesterManager;

        public HarvesterController(HarvesterManager harvesterManager)
        {
            this._harvesterManager = harvesterManager;
        }

        [HttpGet]
        public IActionResult ClearViewCaches()
        {
            this._harvesterManager.ClearViewCaches();
            return Ok();
        }
    }
}
