using OnlineConsulting.Enums;
using System;

namespace OnlineConsulting.Models.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string EmployerId { get; set; }
        public int Price { get; set; }
        public PaymentStatus Status { get; set; }
        public string DotPayOperationNumber { get; set; }

        public User Employee { get; set; }
    }
}
