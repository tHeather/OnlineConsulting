using OnlineConsulting.Enums;
using System;

namespace OnlineConsulting.Models.Entities
{
    public class Payment
    {
        public Guid Id;
        public DateTime CreateDate;
        public string EmployeeId;
        public PaymentStatus Status;

        public User Employee { get; set; }
    }
}
