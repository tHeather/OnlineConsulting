using OnlineConsulting.Enums;
using System;

namespace OnlineConsulting.Models.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string EmployerId { get; set; }
        public decimal Price { get; set; }
        public PaymentStatus Status { get; set; }
        public string DotPayOperationNumber { get; set; }
        public Guid SubscriptionTypeId { get; set; }

        public User Employer { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
    }
}
