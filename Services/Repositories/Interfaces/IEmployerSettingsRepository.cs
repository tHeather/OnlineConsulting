using OnlineConsulting.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineConsulting.Services.Repositories.Interfaces
{
    public interface IEmployerSettingsRepository
    {
        public Task<EmployerSetting> CreateSettingsAsync(string userId);
    }
}
