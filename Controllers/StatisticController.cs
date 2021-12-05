using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineConsulting.Attributes;
using OnlineConsulting.Constants;
using OnlineConsulting.Services.Repositories.Interfaces;

namespace OnlineConsulting.Controllers
{
    [ValidateAntiForgeryToken]
    [TypeFilter(typeof(ValidateSubscriptionAttribute))]
    [Authorize(Roles = UserRoleValue.EMPLOYER)]
    [Route("statistics")]
    public class StatisticController : Controller
    {
        private readonly IUserRepository _userRepository;

        public StatisticController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [IgnoreAntiforgeryToken]
        public IActionResult GetStatistics()
        {
            return View();
        }

        [IgnoreAntiforgeryToken]
        [HttpGet("{id}")]
        public IActionResult GetStatisticsForConsultant(string id)
        {
            var consultant = _userRepository.GetUserById(id);
            return View("GetStatistics", consultant);
        }

    }
}
