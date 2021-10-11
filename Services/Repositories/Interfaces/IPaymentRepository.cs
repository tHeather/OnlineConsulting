using OnlineConsulting.Models.Entities;
using OnlineConsulting.Models.ValueObjects.Payment;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineConsulting.Services.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        public Task<Payment> CreatePayment(decimal price, string employerId, Guid subscriptionTypeId);
        public Task<Payment> GetPaymentByIdAsync(Guid id);
        public IQueryable<Payment> GetPaymentsQuery(GetPaymentsFilters getPaymentsFilters);
    }
}
