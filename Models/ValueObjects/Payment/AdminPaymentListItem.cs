using System;

namespace OnlineConsulting.Models.ValueObjects.Payment
{
    public class AdminPaymentListItem
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public string DotPayOperationNumber { get; set; }
        public string SubscriptionType { get; set; }
        public string Email { get; set; }
    }
}
