using System;

namespace OnlineConsulting.Models.ViewModels.Payment
{
    public class PayForSubscriptionViewModel
    {
        public Guid SubscriptionTypeId { get; set; }
        public DateTime EndDate { get; set; }
    }
}
