using Microsoft.EntityFrameworkCore;
using OnlineConsulting.Data;
using OnlineConsulting.Models.Entities;
using OnlineConsulting.Services.Repositories.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineConsulting.Services.Repositories
{
    public class SubscriptionTypeRepository : ISubscriptionTypeRepository
    {

        private readonly ApplicationDbContext _dbContext;

        public SubscriptionTypeRepository(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<SubscriptionType> GetSubscriptionTypeByIdAsync(Guid subscriptionId)
        {
            return await _dbContext.SubscriptionTypes.SingleOrDefaultAsync(s => s.Id == subscriptionId);
        }

        public IQueryable<SubscriptionType> GetAllSubscriptionTypesQuery()
        {
            return _dbContext.SubscriptionTypes;
        }

        public async Task<SubscriptionType> UpdatePriceAsync(Guid id, decimal price)
        {
            var subscriptionType = await _dbContext.SubscriptionTypes
                                                .SingleOrDefaultAsync(s => s.Id == id);

            subscriptionType.Price = price;

            await _dbContext.SaveChangesAsync();

            return subscriptionType;
        }

    }
}
