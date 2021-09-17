using System;
using System.Collections.Generic;

namespace StudyOnlineServer.Services.Interfaces
{
    public interface IDotPayService
    {
        string CreatePaymentUri(Guid transactionId, int amount, string userEmail, string callbackUrl);
        string GenerateChk(string parameters, bool includeShopId);
    }
}
