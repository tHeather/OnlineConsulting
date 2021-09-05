using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineConsulting.Constants;

namespace OnlineConsulting.Controllers
{
    [Authorize(Roles = UserRoleValue.EMPLOYER)]
    [Route("statistics")]
    public class StatisticController : Controller
    {
        [HttpGet("")]
        public IActionResult GetStatistics()
        {
            return View();
        }

        [HttpGet("{id}")]
        public IActionResult GetStatisticsForConsultant(string id)
        {
            return View("GetStatistics", id);
        }

    }
}
