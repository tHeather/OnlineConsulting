using Microsoft.AspNetCore.Identity;
using OnlineConsulting.Models.Entities;
using OnlineConsulting.Models.ViewModels.Consultant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineConsulting.Services.Repositories.Interfaces
{
   public interface IUserRepository
    {
        public IQueryable<User> GetAllConsultantsForEmployer(string employerId);

        public Task<IdentityResult> CreateConsultantAsync(AddConsultantViewModel addConsultantViewModel, string employerId);
    }
}
