using System;

namespace OnlineConsulting.Services.Interfaces
{
    public interface IDotPayService
    {
        string CreatePaymentUri(Guid paymentId, int amount, string userEmail, string subscriptionDuration);
        string GenerateChk(string parameters);
    }
}
