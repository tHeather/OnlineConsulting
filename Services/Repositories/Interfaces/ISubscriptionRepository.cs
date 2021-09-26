using OnlineConsulting.Models.Entities;
using System.Threading.Tasks;

namespace OnlineConsulting.Services.Repositories.Interfaces
{
    public interface ISubscriptionRepository
    {
        public int GetPriceForSubscription(string subscriptionDuration);
        public Task<Subscription> GetSubscriptionForUserAsync(string userId);
        public Task CreateSubscriptionAsync(string userId);
        public Task ExtendUsersSubscriptionDuration(string userId, string subscriptionDuration);
    }
}
