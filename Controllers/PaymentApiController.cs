using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineConsulting.Enums;
using OnlineConsulting.Models.ValueObjects;
using OnlineConsulting.Services.Interfaces;
using OnlineConsulting.Services.Repositories.Interfaces;
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
        private readonly IDotPayService _dotPayService;
        private readonly IPaymentRepository _paymentRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly string[] _DOTPAY_IPS = { "195.150.9.37", "91.216.191.181", "91.216.191.182",
                                                "91.216.191.183", "91.216.191.184", "91.216.191.185",
                                                "5.252.202.255" };

        public PaymentApiController(
            IDotPayService dotPayService,
            IPaymentRepository paymentRepository,
            ISubscriptionRepository subscriptionRepository)
        {
            _dotPayService = dotPayService;
            _paymentRepository = paymentRepository;
            _subscriptionRepository = subscriptionRepository;
        }


        [HttpPost("dotpay-callback")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> PaymentServiceCallback([FromBody] DotPayCallbackParameters dotPayCallback)
        {
            var ip = HttpContext.Connection.RemoteIpAddress.ToString();
            if (!_DOTPAY_IPS.Contains(ip)) return Ok("OK");

            if (dotPayCallback.operation_type != "payment") return Ok("OK");
            if (dotPayCallback.operation_status != "completed") return Ok("OK");
            if (dotPayCallback.operation_amount != dotPayCallback.operation_original_amount) return Ok("OK");
            if (dotPayCallback.operation_currency != dotPayCallback.operation_original_currency) return Ok("OK");

            var paramsString = MakeParamsString(dotPayCallback);
            var calcSignature = _dotPayService.GenerateChk(paramsString);
            if (calcSignature != dotPayCallback.signature) return Ok("OK");

            var payment = await _paymentRepository.GetPaymentByIdAsync(dotPayCallback.control);
            if (payment == null) return Ok("OK");

            payment.Status = Enum.Parse<PaymentStatus>(dotPayCallback.operation_status, true);
            payment.DotPayOperationNumber = dotPayCallback.operation_number;
            await _subscriptionRepository.ExtendUsersSubscriptionDuration(
                                            payment.EmployerId, payment.SubscriptionTypeId);

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


    }
}
