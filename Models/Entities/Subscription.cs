using System;

namespace OnlineConsulting.Models.Entities
{
    public class Subscription
    {
        public Guid Id;
        public DateTime EndDate;
        public string EmployeeId;

        public User Employee { get; set; }
    }
}
