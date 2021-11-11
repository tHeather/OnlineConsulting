using Microsoft.AspNetCore.Authorization;
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
        private readonly IUserRepository _userRepository;
        private const int PAGE_SIZE = 10;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
    }
}
