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
    [Authorize(Roles = UserRoleValue.ADMIN)]
    public class SubscriptionController : Controller
    {
        private readonly ISubscriptionTypeRepository _subscriptionTypeRepository;

        public SubscriptionController(ISubscriptionTypeRepository subscriptionTypeRepository)
        {
            _subscriptionTypeRepository = subscriptionTypeRepository;
        }

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

        [HttpPost("change-price")]
        public async Task<IActionResult> ChangeSubscriptionPrice(SubscriptionType subscription)
        {

            await _subscriptionTypeRepository.UpdatePriceAsync(
                    subscription.Id,
                    subscription.Price
                );

            return RedirectToAction("ChangeSubscriptionPrice", new { isSaved = true });
        }
    }
}
