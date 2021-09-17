using Microsoft.Extensions.Configuration;
using OnlineConsulting.Constants;
using StudyOnlineServer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace StudyOnlineServer.Services
{
    public class DotPayService : IDotPayService
    {
        private readonly string dotpayShopId;
        private readonly string dotpayShopPin;
        private readonly string dotpayCurrency;
        private readonly string dotpayShopEmail;
        private readonly string dotpayShopName;
        private readonly string dotpayPaymentDescription;
        private readonly string dotpayUri;

        public DotPayService(IConfiguration configuration)
        {
            dotpayShopId = configuration[Parameters.DOTPAY_SHOP_ID] ?? throw new ArgumentNullException(dotpayShopId);
            dotpayShopPin = configuration[Parameters.DOTPAY_SHOP_PIN] ?? throw new ArgumentNullException(dotpayShopPin);
            dotpayCurrency = configuration[Parameters.DOTPAY_CURRENCY] ?? throw new ArgumentNullException(dotpayCurrency);
            dotpayShopEmail = configuration[Parameters.DOTPAY_SHOP_EMAIL] ?? throw new ArgumentNullException(dotpayShopEmail);
            dotpayShopName = configuration[Parameters.DOTPAY_SHOP_NAME] ?? throw new ArgumentNullException(dotpayShopName);
            dotpayPaymentDescription = configuration[Parameters.DOTPAY_PAYMENT_DESCRIPTION] ?? throw new ArgumentNullException(dotpayPaymentDescription);
            dotpayUri = configuration[Parameters.DOTPAY_URI] ?? throw new ArgumentNullException(dotpayUri);
        }

        public string CreatePaymentUri(Guid transactionId, int amount, string userEmail, string callbackUrl)
        {
            var paymentParams = new Dictionary<string, string>()
            {
                { "id", dotpayShopId },
                { "amount", amount.ToString() },
                { "currency", dotpayCurrency },
                { "description", dotpayPaymentDescription },
                { "control", transactionId.ToString() },
                //{ "urlc", callbackUrl },
                { "email", userEmail },
                { "p_info", dotpayShopName },
                { "p_email", dotpayShopEmail }
            };

            var chk = GenerateChk(string.Join("", paymentParams.Select(x => x.Value).ToList()), false);
            var linkParams = string.Join("", paymentParams.Select(x => $"&{x.Key}={x.Value}")).Remove(0, 1);
            return $"{dotpayUri}?{linkParams}&chk={chk}";
        }

        public string GenerateChk(string parameters, bool includeShopId)
        {
            string concatString = dotpayShopPin + (includeShopId ? dotpayShopId : "") + parameters;
            StringBuilder hash = new StringBuilder();
            using (SHA256 sha256 = SHA256.Create())
            {
                var result = sha256.ComputeHash(Encoding.UTF8.GetBytes(concatString));
                foreach (var b in result)
                    hash.Append(b.ToString("x2"));
            }

            return hash.ToString();
        }
    }
}
