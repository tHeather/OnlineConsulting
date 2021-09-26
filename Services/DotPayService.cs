using Microsoft.Extensions.Configuration;
using OnlineConsulting.Constants;
using OnlineConsulting.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace OnlineConsulting.Services
{
    public class DotPayService : IDotPayService
    {
        private readonly string dotpayShopId;
        private readonly string dotpayShopPin;
        private readonly string dotpayCurrency;
        private readonly string dotpayShopEmail;
        private readonly string dotpayShopName;
        private readonly string dotpayUri;
        private readonly string dotpayCallbackPath;
        private readonly string applicationUrl;

        public DotPayService(IConfiguration configuration)
        {
            dotpayShopId = configuration[Parameters.DOTPAY_SHOP_ID] ?? throw new ArgumentNullException(dotpayShopId);
            dotpayShopPin = configuration[Parameters.DOTPAY_SHOP_PIN] ?? throw new ArgumentNullException(dotpayShopPin);
            dotpayCurrency = configuration[Parameters.DOTPAY_CURRENCY] ?? throw new ArgumentNullException(dotpayCurrency);
            dotpayShopEmail = configuration[Parameters.DOTPAY_SHOP_EMAIL] ?? throw new ArgumentNullException(dotpayShopEmail);
            dotpayShopName = configuration[Parameters.DOTPAY_SHOP_NAME] ?? throw new ArgumentNullException(dotpayShopName);
            dotpayUri = configuration[Parameters.DOTPAY_URI] ?? throw new ArgumentNullException(dotpayUri);
            dotpayCallbackPath = configuration[Parameters.DOTPAY_CALLBACK_PATH] ?? throw new ArgumentNullException(dotpayCallbackPath);
            applicationUrl = configuration[Parameters.APPLICATION_URL] ?? throw new ArgumentNullException(applicationUrl);
        }

        public string CreatePaymentUri(Guid paymentId, int amount, string userEmail, string subscriptionDuration)
        {
            var paymentParams = new Dictionary<string, string>()
            {
                { "id", dotpayShopId },
                { "amount", amount.ToString() },
                { "currency", dotpayCurrency },
                { "description", subscriptionDuration },
                { "control", paymentId.ToString() },
                { "urlc", $"{applicationUrl}{dotpayCallbackPath}" },
                { "email", userEmail },
                { "p_info", dotpayShopName },
                { "p_email", dotpayShopEmail }
            };

            var chk = GenerateChk(string.Join("", paymentParams.Select(x => x.Value).ToList()));
            var linkParams = string.Join("", paymentParams.Select(x => $"&{x.Key}={x.Value}")).Remove(0, 1);
            return $"{dotpayUri}?{linkParams}&chk={chk}";
        }

        public string GenerateChk(string parameters)
        {
            string concatString = dotpayShopPin + parameters;
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
