using OnlineConsulting.Enums;
using System;

namespace OnlineConsulting.Models.ValueObjects.Payment
{
    public class GetPaymentsFilters
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public PaymentStatus? Status { get; set; }
        public string DotPayOperationNumber { get; set; }
        public string EmployerEmail { get; set; }
    }
}
