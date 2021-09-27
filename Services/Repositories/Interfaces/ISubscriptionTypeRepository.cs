using OnlineConsulting.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineConsulting.Services.Repositories.Interfaces
{
    public interface ISubscriptionTypeRepository
    {
        public Task<SubscriptionType> GetSubscriptionTypeByIdAsync(Guid subscriptionId);
        public Task<List<SubscriptionType>> GetAllSubscriptionTypesAsync();
    }
}
