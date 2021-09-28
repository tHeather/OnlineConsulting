using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OnlineConsulting.Constants;
using OnlineConsulting.Models.ViewModels.Payment;
using OnlineConsulting.Services.Interfaces;
using OnlineConsulting.Services.Repositories.Interfaces;
using System;
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
        private readonly ISubscriptionTypeRepository _subscriptionTypeRepository;
        private readonly string _currency;


        public PaymentController(
            IDotPayService dotPayService,
            IPaymentRepository paymentRepository,
            ISubscriptionRepository subscriptionRepository,
            ISubscriptionTypeRepository subscriptionTypeRepository,
            IConfiguration configuration)
        {
            _dotPayService = dotPayService;
            _paymentRepository = paymentRepository;
            _subscriptionRepository = subscriptionRepository;
            _subscriptionTypeRepository = subscriptionTypeRepository;
            _currency = configuration[Parameters.DOTPAY_CURRENCY] ?? throw new ArgumentNullException(_currency);
        }

        [HttpGet("create")]
        public async Task<IActionResult> PayForSubscription()
        {
            var employerId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var subscription = await _subscriptionRepository.GetSubscriptionForUserAsync(employerId);

            var payForSubscriptionViewModel = new PayForSubscriptionViewModel
            {
                EndDate = subscription.EndDate,
                Currency = _currency
            };

            return View(payForSubscriptionViewModel);
        }

        [HttpPost("create")]
        public async Task<IActionResult> PayForSubscription(PayForSubscriptionViewModel payForSubscriptionViewModel)
        {

            var employerId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var employerEmail = User.FindFirst(ClaimTypes.Email).Value;

            var subscriptionType = await _subscriptionTypeRepository.GetSubscriptionTypeByIdAsync(
                                                               payForSubscriptionViewModel.SubscriptionTypeId
                                                               );

            var payment = await _paymentRepository.CreatePayment(
                                                    subscriptionType.Price, employerId, subscriptionType.Id
                                                    );

            var redirectUrl = _dotPayService.CreatePaymentUri(
                                    payment.Id, subscriptionType.Price, employerEmail, subscriptionType.Name
                                    );

            return Redirect(redirectUrl);
        }

    }
}
