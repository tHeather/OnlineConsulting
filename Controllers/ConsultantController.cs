
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using OnlineConsulting.Attributes;
using OnlineConsulting.Constants;
using OnlineConsulting.Models.Constants;
using OnlineConsulting.Models.Entities;
using OnlineConsulting.Models.ViewModels.Consultant;
using OnlineConsulting.Models.ViewModels.Modals;
using OnlineConsulting.Services.Interfaces;
using OnlineConsulting.Services.Repositories.Interfaces;
using OnlineConsulting.Tools;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OnlineConsulting.Controllers
{
    [TypeFilter(typeof(ValidateSubscriptionAttribute))]
    [Authorize(Roles = UserRoleValue.EMPLOYER)]
    [Route("consultants")]

    public class ConsultantController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IOptions<IdentityOptions> _identityOptions;
        private readonly UserManager<User> _userManager;
        private readonly ISendgridService _sendgridService;

        public ConsultantController(
             IUserRepository userRepository,
             IOptions<IdentityOptions> identityOptions,
             UserManager<User> userManager,
             ISendgridService sendgridService
            )
        {
            _userRepository = userRepository;
            _identityOptions = identityOptions;
            _userManager = userManager;
            _sendgridService = sendgridService;
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
            if (ModelState.IsValid)
            {

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                var createConsultantValueObject = await _userRepository.CreateConsultantAsync(addConsultantViewModel, userIdClaim.Value);

                if (createConsultantValueObject.IdentityResult.Succeeded)
                {

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(createConsultantValueObject.User);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = createConsultantValueObject.User.Id, code = code, returnUrl = "/" },
                        protocol: Request.Scheme);

                    await _sendgridService.SendConfirmEmailAddressLink(createConsultantValueObject.User.Email, callbackUrl);

                    ModelState.Clear();
                    return View(new AddConsultantViewModel
                    {
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
                    new GetAllConsultantsViewModel()
                    {
                        ConsultantList = consultantList,
                        Modal = modalViewModel
                    }
                );
        }

        [HttpGet("delete")]
        public async Task<ActionResult> DeleteConsultant(string id)
        {

            var user = _userRepository.GetUserById(id);

            if (user == null) return RedirectToAction("GetAllConsultants", new ModalViewModel
            {
                ModalLabel = "Delete consultant",
                ModalText = new List<string>() { "Selected consultant does not exist." },
                IsVisible = true,
                ModalType = ModalStyles.ERROR
            });

            var result = await _userRepository.DeleteConsultantAsync(user);

            if (!result.Succeeded) RedirectToAction("GetAllConsultants", new ModalViewModel
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

        public async Task<IActionResult> ResetConsultantPassword(string id)
        {
            var passwordOptions = _identityOptions.Value.Password;
            using var randomPasswordGenerator = new RandomPasswordGenerator(passwordOptions);
            var generatedPassword = randomPasswordGenerator.Generate();

            var resetResult = await _userRepository.RestUserPasswordAsync(id, generatedPassword);

            return RedirectToAction("GetAllConsultants", new ModalViewModel
            {
                ModalLabel = "Password reset",
                ModalText = resetResult.IsSucceed ?
                                new List<string>() {
                                    $"Password has been reset successfully. The new password is: {generatedPassword}"
                                } : resetResult.Errors,
                IsVisible = true,
                ModalType = resetResult.IsSucceed ? ModalStyles.SUCCESS : ModalStyles.ERROR
            });
        }

    }
}
