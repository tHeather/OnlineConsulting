using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineConsulting.Constants;
using OnlineConsulting.Models.Entities;
using OnlineConsulting.Models.ViewModels.Subscription;
using OnlineConsulting.Services.Repositories.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineConsulting.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("subscription")]
    public class SubscriptionController : Controller
    {
        private readonly ISubscriptionTypeRepository _subscriptionTypeRepository;

        public SubscriptionController(ISubscriptionTypeRepository subscriptionTypeRepository)
        {
            _subscriptionTypeRepository = subscriptionTypeRepository;
        }

        [IgnoreAntiforgeryToken]
        [Authorize(Roles = UserRoleValue.ADMIN)]
        [HttpGet("change-price")]
        public IActionResult ChangeSubscriptionPrice(bool isSaved)
        {
            var subscriptionTypeList = _subscriptionTypeRepository
                        .GetAllSubscriptionTypesQuery().ToList();

            var changeSubscriptionPriceViewModel = new ChangeSubscriptionPriceViewModel()
            {
                SubscriptionTypes = subscriptionTypeList,
                IsSaved = isSaved
            };

            return View("ChangeSubscriptionPrice", changeSubscriptionPriceViewModel);
        }

        [Authorize(Roles = UserRoleValue.ADMIN)]
        [HttpPost("change-price")]
        public async Task<IActionResult> ChangeSubscriptionPrice(SubscriptionType subscription)
        {

            await _subscriptionTypeRepository.UpdatePriceAsync(
                    subscription.Id,
                    subscription.Price
                );

            return RedirectToAction("ChangeSubscriptionPrice", new { isSaved = true });
        }

        [IgnoreAntiforgeryToken]
        [Authorize(Roles = UserRoleValue.EMPLOYER)]
        [HttpGet("employer/subscription-expired")]
        public IActionResult EmployerSubscriptionExpierd()
        {
            return View("EmployerSubscriptionExpierd");
        }

        [IgnoreAntiforgeryToken]
        [Authorize(Roles = UserRoleValue.CONSULTANT)]
        [HttpGet("consultant/subscription-expired")]
        public IActionResult ConsultantSubscriptionExpierd()
        {
            return View("ConsultantSubscriptionExpierd");
        }

    }
}
