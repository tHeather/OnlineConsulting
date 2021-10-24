using System;

namespace OnlineConsulting.Models.Entities
{
    public class Subscription
    {
        public Guid Id { get; set; }
        public DateTime EndDate { get; set; }
        public string EmployerId { get; set; }
        public Guid LastPaymentId { get; set; }

        public User Employer { get; set; }
    }
}
