using OnlineConsulting.Models.Entities;
using System;
using System.Threading.Tasks;

namespace OnlineConsulting.Services.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        public Task<Payment> CreatePayment(decimal price, string employerId, Guid subscriptionTypeId);
        public Task<Payment> GetPaymentByIdAsync(Guid id);
    }
}
