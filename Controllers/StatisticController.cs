using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineConsulting.Constants;
using OnlineConsulting.Services.Repositories.Interfaces;

namespace OnlineConsulting.Controllers
{
    [Authorize(Roles = UserRoleValue.EMPLOYER)]
    [Route("statistics")]
    public class StatisticController : Controller
    {
        private readonly IUserRepository _userRepository;

        public StatisticController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult GetStatistics()
        {
            return View();
        }

        [HttpGet("{id}")]
        public IActionResult GetStatisticsForConsultant(string id)
        {
            var consultant = _userRepository.GetUserById(id);
            return View("GetStatistics", consultant);
        }

    }
}
