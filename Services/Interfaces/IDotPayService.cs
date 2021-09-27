using System;

namespace OnlineConsulting.Services.Interfaces
{
    public interface IDotPayService
    {
        string CreatePaymentUri(Guid paymentId, decimal amount, string userEmail, string subscriptionName);
        string GenerateChk(string parameters);
    }
}
