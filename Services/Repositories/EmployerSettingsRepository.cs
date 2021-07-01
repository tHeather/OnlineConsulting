using OnlineConsulting.Data;
using OnlineConsulting.Models.Entities;
using OnlineConsulting.Services.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineConsulting.Services.Repositories
{
    public class EmployerSettingsRepository: IEmployerSettingsRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public EmployerSettingsRepository(ApplicationDbContext applicationDbContext)
        {
           _applicationDbContext = applicationDbContext;
        }

        public async Task<EmployerSetting> CreateSettingsAsync(string userId)
        {
            var settings = new EmployerSetting
            {
                UserId = userId
            };

            await _applicationDbContext.EmployerSettings.AddAsync(settings);

            await _applicationDbContext.SaveChangesAsync();

            return settings;

        }
    }
}
