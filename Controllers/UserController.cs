using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineConsulting.Constants;
using OnlineConsulting.Models.ValueObjects.Users;
using OnlineConsulting.Models.ViewModels.Users;
using OnlineConsulting.Services.Repositories.Interfaces;
using OnlineConsulting.Tools;
using System.Threading.Tasks;

namespace OnlineConsulting.Controllers
{
    [Authorize(Roles = UserRoleValue.ADMIN)]
    [Route("users")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private const int PAGE_SIZE = 10;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [Route("list")]
        public async Task<IActionResult> EmployerList(int pageIndex = 1)
        {
            var employersQuery = _userRepository.GetAllEmployersQuery();
            var usersWithSubscriptionQuery = _userRepository.
                                                GetUsersWithSubscriptionQuery(employersQuery);

            var employers = await PaginatedList<UserWithSubscription>.CreateAsync(
                                usersWithSubscriptionQuery,
                                 pageIndex,
                                 PAGE_SIZE);

            return View(new EmployerListViewModel
            {
                Employers = employers
            });
        }
    }
}
