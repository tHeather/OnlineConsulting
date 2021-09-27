using Microsoft.EntityFrameworkCore;
using OnlineConsulting.Data;
using OnlineConsulting.Models.Entities;
using OnlineConsulting.Services.Repositories.Interfaces;
using System;
using System.Collections.Generic;
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

        public async Task<List<SubscriptionType>> GetAllSubscriptionTypesAsync()
        {
            return await _dbContext.SubscriptionTypes.ToListAsync();
        }

    }
}
