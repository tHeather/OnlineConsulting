﻿using Microsoft.EntityFrameworkCore;
using OnlineConsulting.Data;
using OnlineConsulting.Models.Entities;
using OnlineConsulting.Services.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OnlineConsulting.Services.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly ISubscriptionTypeRepository _subscriptionTypeRepository;

        public SubscriptionRepository(ApplicationDbContext dbContext,
            ISubscriptionTypeRepository subscriptionTypeRepository)
        {
            _dbContext = dbContext;
            _subscriptionTypeRepository = subscriptionTypeRepository;
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

        public async Task ExtendUsersSubscriptionDuration(string userId, Guid subscriptionTypeId)
        {
            var subscription = await GetSubscriptionForUserAsync(userId);

            var subscriptionType = await _subscriptionTypeRepository.GetSubscriptionTypeByIdAsync(subscriptionTypeId);

            if (subscription.EndDate.Date < DateTime.UtcNow.Date)
            {
                subscription.EndDate = DateTime.UtcNow.Date.AddDays(subscriptionType.Days);
            }
            else
            {
                subscription.EndDate = subscription.EndDate.AddDays(subscriptionType.Days);
            }
            await _dbContext.SaveChangesAsync();
        }

    }
}