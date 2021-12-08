using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineConsulting.Constants;
using OnlineConsulting.Enums;
using OnlineConsulting.Models.ValueObjects;
using OnlineConsulting.Models.ValueObjects.Payment;
using OnlineConsulting.Services.Interfaces;
using OnlineConsulting.Services.Repositories.Interfaces;
using OnlineConsulting.Tools;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineConsulting.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentApiController : ControllerBase
    {
        private const int PAGINATION_PAGE_SIZE = 10;
        private readonly IDotPayService _dotPayService;
        private readonly IPaymentRepository _paymentRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ILogger<PaymentApiController> _logger;
        private readonly string[] _DOTPAY_IPS = { "195.150.9.37", "91.216.191.181", "91.216.191.182",
                                                "91.216.191.183", "91.216.191.184", "91.216.191.185",
                                                "5.252.202.255" };

        public PaymentApiController(
            IDotPayService dotPayService,
            IPaymentRepository paymentRepository,
            ISubscriptionRepository subscriptionRepository,
            ILogger<PaymentApiController> logger)
        {
            _dotPayService = dotPayService;
            _paymentRepository = paymentRepository;
            _subscriptionRepository = subscriptionRepository;
            _logger = logger;
        }


        [HttpPost("dotpay-callback")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> PaymentServiceCallback([FromBody] DotPayCallbackParameters dotPayCallback)
        {
            var ip = HttpContext.Connection.RemoteIpAddress.ToString();
            if (!_DOTPAY_IPS.Contains(ip)) {
                _logger.LogInformation("Dotpay callback failed, paymentId: {paymentId}. Wrong IP: {ip}",
                                        dotPayCallback.control, ip);
                return Ok("OK");
            };

            if (dotPayCallback.operation_type != "payment")
            {
                _logger.LogInformation("Dotpay callback failed, paymentId: {paymentId}. Wrong operation_type: {operation_type}",
                                        dotPayCallback.control, dotPayCallback.operation_type);
                return Ok("OK");
            };

            if (dotPayCallback.operation_status != "completed")
            {
                _logger.LogInformation("Dotpay callback failed, paymentId: {paymentId}. Wrong operation_status: {operation_status}",
                                        dotPayCallback.control, dotPayCallback.operation_status);
                return Ok("OK");
            };

            if (dotPayCallback.operation_amount != dotPayCallback.operation_original_amount)
            {
                _logger.LogInformation("Dotpay callback failed, paymentId: {paymentId}. Wrong operation_amount: {operation_amount}",
                                        dotPayCallback.control, dotPayCallback.operation_amount);
                return Ok("OK");
            };

            if (dotPayCallback.operation_currency != dotPayCallback.operation_original_currency)
            {
                _logger.LogInformation("Dotpay callback failed, paymentId: {paymentId}. Wrong operation_currency: {operation_currency}",
                                        dotPayCallback.control, dotPayCallback.operation_currency);
                return Ok("OK");
            };

            var paramsString = MakeParamsString(dotPayCallback);
            var calcSignature = _dotPayService.GenerateChk(paramsString);
            if (calcSignature != dotPayCallback.signature)
            {
                _logger.LogInformation("Dotpay callback failed, paymentId: {paymentId}. Wrong signature: {signature}",
                                        dotPayCallback.control, dotPayCallback.signature);
                return Ok("OK");
            };

            var payment = await _paymentRepository.GetPaymentByIdAsync(dotPayCallback.control);
            if (payment == null)
            {
                _logger.LogInformation("Dotpay callback failed, paymentId: {paymentId}. Payment not found.",
                                        dotPayCallback.control);
                return Ok("OK");
            };

            payment.Status = Enum.Parse<PaymentStatus>(dotPayCallback.operation_status, true);
            payment.DotPayOperationNumber = dotPayCallback.operation_number;
            await _subscriptionRepository.ExtendUsersSubscriptionDuration(
                                            payment.EmployerId, payment.SubscriptionTypeId, payment.Id);

            _logger.LogInformation("Subscription extended. Payment: {paymentId}.", payment.Id);

            return Ok("OK");
        }


        private string MakeParamsString(DotPayCallbackParameters parameters)
        {
            var stringBuilder = new StringBuilder();
            foreach (var property in parameters.GetType().GetProperties())
            {
                if (property.Name == "signature") continue;
                stringBuilder.Append(property.GetValue(parameters));
            }
            return stringBuilder.ToString();
        }

        [Authorize(Roles = UserRoleValue.ADMIN)]
        [HttpGet("list")]
        public async Task<AdminPaymentList> GetPaymentList(
            [FromQuery] GetPaymentsFilters getPaymentsFilters, int pageIndex = 1)
        {

            var paymentQuery = _paymentRepository
                .GetPaymentsQuery(getPaymentsFilters)
                .Select(p => new AdminPaymentListItem()
                {
                    Id = p.Id,
                    CreateDate = p.CreateDate,
                    Price = p.Price,
                    DotPayOperationNumber = p.DotPayOperationNumber,
                    Email = p.Employer.Email,
                    Status = Enum.GetName(p.Status.GetType(), p.Status),
                    SubscriptionType = p.SubscriptionType.Name,

                });

            var paymentList = await PaginatedList<AdminPaymentListItem>
                                            .CreateAsync(paymentQuery, pageIndex, PAGINATION_PAGE_SIZE);


            return new AdminPaymentList()
            {
                List = paymentList,
                PageIndex = paymentList.PageIndex,
                HasPreviousPage = paymentList.HasPreviousPage,
                HasNextPage = paymentList.HasNextPage,
                SurroundingIndexes = paymentList.GetSurroundingIndexes(2)
            };


        }

    }
}
