using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineConsulting.Constants;
using OnlineConsulting.Models.Entities;
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
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private const int PAGE_SIZE = 10;

        public UserController(
            IUserRepository userRepository,
            UserManager<User> userManager
            )
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        [Route("employer-list")]
        public async Task<IActionResult> EmployerList(int pageIndex = 1)
        {
            var employersQuery = _userRepository.GetAllUsersWithRoleQuery(UserRoleValue.EMPLOYER);
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

        [Route("employee-list/{employerId}")]
        public async Task<IActionResult> EmployeeList(string employerId, int pageIndex = 1)
        {
            var employer = _userRepository.GetUserById(employerId); 
            var employeesQuery = _userRepository.GetAllConsultantsForEmployerQuery(employerId);


            var employeesList = await PaginatedList<User>.CreateAsync(
                                 employeesQuery,
                                 pageIndex,
                                 PAGE_SIZE);

            return View(new EmployeeListViewModel()
            {
               Employees = employeesList,
               Employer  = employer
            });
        }

        [HttpGet("search")]
        public async Task<IActionResult> FindUser(string email)
        {
            if (email == null) return RedirectToAction("EmployerList");

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return View("EmployerList", new EmployerListViewModel());

            var isAdmin = await _userManager.IsInRoleAsync(user, UserRoleValue.ADMIN);

            if (isAdmin) return View("EmployerList", new EmployerListViewModel());

            var userByEmailQuery = _userRepository.GetUserByEmailQuery(email);

            if (user.EmployerId == null)
            {
                var userWithSubscriptionQuery = _userRepository
                                                    .GetUsersWithSubscriptionQuery(userByEmailQuery);

                var employers = await PaginatedList<UserWithSubscription>.CreateAsync(
                                                            userWithSubscriptionQuery, 1, PAGE_SIZE);

                return View("EmployerList", new EmployerListViewModel
                {
                    Employers = employers
                });
            }

            var employees = await PaginatedList<User>.CreateAsync(userByEmailQuery, 1, PAGE_SIZE);
            var employer = await _userManager.FindByIdAsync(user.EmployerId);

            return View("EmployeeList",new EmployeeListViewModel()
            {
                Employees = employees,
                Employer = employer
            });
        }

        [HttpPost("lock")]
        public async Task<IActionResult> ChangeAccountLockState(string employerId, bool isLocked)
        {
            await _userRepository.LockEmployerWithEmployees(employerId, isLocked);

            return RedirectToAction("EmployerList");
        }        
    }
}
