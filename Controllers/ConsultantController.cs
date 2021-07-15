
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineConsulting.Constants;
using OnlineConsulting.Models.Constants;
using OnlineConsulting.Models.Entities;
using OnlineConsulting.Models.ViewModels.Consultant;
using OnlineConsulting.Models.ViewModels.Modals;
using OnlineConsulting.Services.Repositories.Interfaces;
using OnlineConsulting.Tools;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineConsulting.Controllers
{
    [Authorize(Roles = UserRoleValue.EMPLOYER)]
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
            return View(new AddConsultantViewModel());
        }

        [HttpPost("create")]
        [AutoValidateAntiforgeryToken]
        public async Task<ActionResult> AddConsultant(AddConsultantViewModel addConsultantViewModel)
        {
            if (ModelState.IsValid) {

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                var createConsultantValueObject = await _userRepository.CreateConsultantAsync(addConsultantViewModel, userIdClaim.Value);

                if (createConsultantValueObject.IdentityResult.Succeeded) 
                {
                    ModelState.Clear();
                    return View(new AddConsultantViewModel {
                        GeneratedPassword = createConsultantValueObject.GeneratedPassword,
                        Login = addConsultantViewModel.Email
                    });
                }

                foreach (var error in createConsultantValueObject.IdentityResult.Errors)
                {
                    ModelState.AddModelError("userManager", error.Description);
                }
            }

            return View(addConsultantViewModel);
        }

        [HttpGet("list")]
        public async Task<ActionResult> GetAllConsultants(int pageIndex = 1, ModalViewModel modalViewModel = null)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            var consultantList = await PaginatedList<User>.CreateAsync(
                    _userRepository.GetAllConsultantsForEmployerQuery(userIdClaim.Value),
                    pageIndex,
                    10);

            return View(
                    new GetAllConsultantsViewModel() { 
                        ConsultantList = consultantList,
                        Modal =  modalViewModel
                    }
                );
        }

        [HttpGet("delete")]
        public async Task<ActionResult> DeleteConsultant(string id) {

            var user = _userRepository.GetUserById(id);

            if(user == null) return RedirectToAction("GetAllConsultants", new ModalViewModel
            {
                ModalLabel = "Delete consultant",
                ModalText = new List<string>() { "Selected consultant does not exist." },
                IsVisible = true,
                ModalType = ModalStyles.ERROR
            });

            var result = await _userRepository.DeleteConsultant(user);

            if(!result.Succeeded) RedirectToAction("GetAllConsultants", new ModalViewModel
            {
                ModalLabel = "Delete consultant",
                ModalText = (List<string>)result.Errors,
                IsVisible = true,
                ModalType = ModalStyles.ERROR
            });

            return RedirectToAction("GetAllConsultants", new ModalViewModel
            {
                ModalLabel = "Delete consultant",
                ModalText = new List<string>() { "Selected consultant deleted successfully" },
                IsVisible = true,
                ModalType = ModalStyles.SUCCESS
            });

        }

    }
}
