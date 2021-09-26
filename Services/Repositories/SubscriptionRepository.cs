using Microsoft.EntityFrameworkCore;
using OnlineConsulting.Constants;
using OnlineConsulting.Data;
using OnlineConsulting.Models.Entities;
using OnlineConsulting.Services.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineConsulting.Services.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {

        private readonly ApplicationDbContext _dbContext;

        private readonly Dictionary<string, int> SUBSCRIPTION_PRICE_DICTIONARY = new Dictionary<string, int>()
        {
            { SubscriptionDuration.MONTH, 100 },
            { SubscriptionDuration.QUARTER, 250 },
            { SubscriptionDuration.HALF_YEAR, 450 },
            { SubscriptionDuration.YEAR, 850 },
        };

        private readonly Dictionary<string, int> SUBSCRIPTION_DURATION_DICTIONARY = new Dictionary<string, int>()
        {
            { SubscriptionDuration.MONTH, 30 },
            { SubscriptionDuration.QUARTER, 91 },
            { SubscriptionDuration.HALF_YEAR, 182 },
            { SubscriptionDuration.YEAR, 365 },
        };
        public SubscriptionRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int GetPriceForSubscription(string subscriptionDuration)
        {
            return SUBSCRIPTION_PRICE_DICTIONARY.GetValueOrDefault(subscriptionDuration);
        }

        public async Task<Subscription> GetSubscriptionForUserAsync(string userId)
        {
            return await _dbContext.Subscriptions.SingleOrDefaultAsync(s => s.EmployerId == userId);
        }

        public async Task CreateSubscriptionAsync(string userId)
        {
            var subscription = new Subscription()
            {
                EmployerId = userId,

            };

            await _dbContext.Subscriptions.AddAsync(subscription);
            await _dbContext.SaveChangesAsync();
        }

        public async Task ExtendUsersSubscriptionDuration(string userId, string subscriptionDuration)
        {
            var subscription = await GetSubscriptionForUserAsync(userId);
            var days = SUBSCRIPTION_DURATION_DICTIONARY.GetValueOrDefault(subscriptionDuration);
            if (subscription.EndDate.Date < DateTime.UtcNow.Date)
            {
                subscription.EndDate = DateTime.UtcNow.Date.AddDays(days);
            }
            else
            {
                subscription.EndDate = subscription.EndDate.AddDays(days);
            }
            await _dbContext.SaveChangesAsync();
        }

    }
}
