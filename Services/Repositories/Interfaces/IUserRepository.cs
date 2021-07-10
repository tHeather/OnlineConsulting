using Microsoft.AspNetCore.Identity;
using OnlineConsulting.Models.Entities;
using OnlineConsulting.Models.ValueObjects.User;
using OnlineConsulting.Models.ViewModels.Consultant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineConsulting.Services.Repositories.Interfaces
{
   public interface IUserRepository
    {
        public User GetUserById(string id);
        public IQueryable<User> GetAllConsultantsForEmployer(string employerId);

        public Task<CreateConsultantValueObject> CreateConsultantAsync(AddConsultantViewModel addConsultantViewModel, string employerId);

        public Task<IdentityResult> DeleteConsultant(User user);
    }
}
