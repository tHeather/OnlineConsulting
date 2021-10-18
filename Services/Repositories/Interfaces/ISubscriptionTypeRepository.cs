using OnlineConsulting.Models.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineConsulting.Services.Repositories.Interfaces
{
    public interface ISubscriptionTypeRepository
    {
        public Task<SubscriptionType> GetSubscriptionTypeByIdAsync(Guid subscriptionId);
        public IQueryable<SubscriptionType> GetAllSubscriptionTypesQuery();
        public Task<SubscriptionType> UpdatePriceAsync(Guid id, decimal price);
    }
}
