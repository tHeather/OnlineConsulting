using Microsoft.EntityFrameworkCore;
using OnlineConsulting.Data;
using OnlineConsulting.Enums;
using OnlineConsulting.Models.Entities;
using OnlineConsulting.Services.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OnlineConsulting.Services.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public PaymentRepository(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<Payment> CreatePayment(
            decimal price, string employerId, Guid subscriptionTypeId
            )
        {
            var payment = new Payment
            {
                CreateDate = DateTime.UtcNow,
                EmployerId = employerId,
                Price = price,
                Status = PaymentStatus.NEW,
                SubscriptionTypeId = subscriptionTypeId
            };

            await _dbContext.Payments.AddAsync(payment);
            await _dbContext.SaveChangesAsync();

            return payment;
        }

        public async Task<Payment> GetPaymentByIdAsync(Guid id)
        {
            return await _dbContext.Payments.SingleOrDefaultAsync(p => p.Id == id);
        }

    }
}
