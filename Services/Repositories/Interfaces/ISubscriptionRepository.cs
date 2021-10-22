using OnlineConsulting.Models.Entities;
using System;
using System.Threading.Tasks;

namespace OnlineConsulting.Services.Repositories.Interfaces
{
    public interface ISubscriptionRepository
    {
        public Task<Subscription> GetSubscriptionForUserAsync(string userId);
        public Task CreateSubscriptionAsync(string userId);
        public Task ExtendUsersSubscriptionDuration(string userId, Guid subscriptionTypeId, Guid lastPaymentId);
    }
}
