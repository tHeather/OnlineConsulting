
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineConsulting.Models.Entities;
using OnlineConsulting.Models.Enums;
using OnlineConsulting.Models.ViewModels.Consultant;
using OnlineConsulting.Services.Repositories.Interfaces;
using OnlineConsulting.Tools;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineConsulting.Controllers
{
    [Route("consultants")]

    public class ConsultantController : Controller
    {
        private readonly IUserRepository _userRepository;

        public ConsultantController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("create")]
        public ActionResult AddConsultant()
        {
            return View(new AddConsultantViewModel { IsAdded = false });
        }

        [HttpPost("create")]
        [AutoValidateAntiforgeryToken]
        public async Task<ActionResult> AddConsultant(AddConsultantViewModel addConsultantViewModel)
        {
            if (ModelState.IsValid) {

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                var result = await _userRepository.CreateConsultantAsync(addConsultantViewModel, userIdClaim.Value);

                if (result.Succeeded) 
                {
                    ModelState.Clear();
                    return View(new AddConsultantViewModel { IsAdded = true });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("userManager", error.Description);
                }
            }

            return View(addConsultantViewModel);
        }

        [HttpGet("list")]
        public async Task<ActionResult> GetAllConsultants(int pageIndex = 1)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            return View(
                await PaginatedList<User>.CreateAsync(
                    _userRepository.GetAllConsultantsForEmployer(userIdClaim.Value),
                    pageIndex, 
                    10)
                );
        }

    }
}
