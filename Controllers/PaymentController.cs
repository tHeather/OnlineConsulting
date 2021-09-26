using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineConsulting.Constants;
using OnlineConsulting.Models.ViewModels.Payment;
using OnlineConsulting.Services.Interfaces;
using OnlineConsulting.Services.Repositories.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineConsulting.Controllers
{
    [Authorize(Roles = UserRoleValue.EMPLOYER)]
    [Route("payment")]
    public class PaymentController : Controller
    {
        private readonly IDotPayService _dotPayService;
        private readonly IPaymentRepository _paymentRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;

        public PaymentController(
            IDotPayService dotPayService,
            IPaymentRepository paymentRepository,
            ISubscriptionRepository subscriptionRepository)
        {
            _dotPayService = dotPayService;
            _paymentRepository = paymentRepository;
            _subscriptionRepository = subscriptionRepository;
        }

        [HttpGet("create")]
        public async Task<IActionResult> PayForSubscription()
        {
            var employerId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var subscription = await _subscriptionRepository.GetSubscriptionForUserAsync(employerId);

            var payForSubscriptionViewModel = new PayForSubscriptionViewModel
            {
                EndDate = subscription.EndDate
            };

            return View(payForSubscriptionViewModel);
        }

        [HttpPost("create")]
        public async Task<IActionResult> PayForSubscription(PayForSubscriptionViewModel payForSubscriptionViewModel)
        {

            var employerId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var employerEmail = User.FindFirst(ClaimTypes.Email).Value;

            var price = _subscriptionRepository.GetPriceForSubscription(payForSubscriptionViewModel.SubscriptionDuration);

            var payment = await _paymentRepository.CreatePayment(price, employerId);

            var redirectUrl = _dotPayService.CreatePaymentUri(payment.Id, price, employerEmail, payForSubscriptionViewModel.SubscriptionDuration);

            return Redirect(redirectUrl);
        }

    }
}
