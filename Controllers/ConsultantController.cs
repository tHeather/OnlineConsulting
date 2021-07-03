
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineConsulting.Models.Entities;
using OnlineConsulting.Models.Enums;
using OnlineConsulting.Models.ViewModels.Consultant;
using System.Threading.Tasks;

namespace OnlineConsulting.Controllers
{
    public class ConsultantController : Controller
    {
        private readonly UserManager<User> _userManager;

        public ConsultantController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }


        public ActionResult AddConsultant()
        {
            return View(new AddConsultantViewModel { IsAdded = false });
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<ActionResult> AddConsultant(AddConsultantViewModel addConsultantViewModel)
        {
            if (ModelState.IsValid) {

                var user = new User
                {
                    UserName = addConsultantViewModel.Email,
                    Email = addConsultantViewModel.Email,
                    FirstName = addConsultantViewModel.FirstName,
                    Surname = addConsultantViewModel.Surname,
                    EmployerId = _userManager.GetUserId(User)
                };

                var result = await _userManager.CreateAsync(user, addConsultantViewModel.Password);

                if (result.Succeeded) 
                {
                    await _userManager.AddToRoleAsync(user, UserRoleEnum.Consultant.ToString());
                    await _userManager.UpdateAsync(user);
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

    }
}
