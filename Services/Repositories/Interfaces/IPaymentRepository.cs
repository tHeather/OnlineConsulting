using OnlineConsulting.Models.Entities;
using System;
using System.Threading.Tasks;

namespace OnlineConsulting.Services.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        public Task<Payment> CreatePayment(int price, string employerId);
        public Task<Payment> GetPaymentByIdAsync(Guid id);
    }
}
