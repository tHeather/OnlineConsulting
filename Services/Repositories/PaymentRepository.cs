using Microsoft.EntityFrameworkCore;
using OnlineConsulting.Data;
using OnlineConsulting.Enums;
using OnlineConsulting.Models.Entities;
using OnlineConsulting.Models.ValueObjects.Payment;
using OnlineConsulting.Services.Repositories.Interfaces;
using System;
using System.Linq;
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

        public IQueryable<Payment> GetPaymentsQuery(GetPaymentsFilters getPaymentsFilters)
        {
            var query = _dbContext.Payments.Where(
                p => p.CreateDate >= getPaymentsFilters.StartDate &&
                     p.CreateDate <= getPaymentsFilters.EndDate);

            if (getPaymentsFilters.Status != null)
            {
                query = query.Where(p => p.Status == getPaymentsFilters.Status);
            }

            if (getPaymentsFilters.DotPayOperationNumber != null)
            {
                query = query.Where(
                    p => p.DotPayOperationNumber == getPaymentsFilters.DotPayOperationNumber
                    );
            }

            if (getPaymentsFilters.EmployerEmail != null)
            {
                query = query.Where(p => p.Employer.Email == getPaymentsFilters.EmployerEmail);
            }

            query = query.Include(p => p.Employer);
            query = query.Include(p => p.SubscriptionType);

            return query;
        }

    }
}
