using Microsoft.AspNetCore.Identity;
using OnlineConsulting.Models.Entities;
using OnlineConsulting.Models.Enums;
using OnlineConsulting.Models.ViewModels.Consultant;
using OnlineConsulting.Services.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineConsulting.Services.Repositories
{
    public class UserRepository:IUserRepository
    {    
        private readonly UserManager<User> _userManager;

        public UserRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public IQueryable<User> GetAllConsultantsForEmployer(string employerId) {
            return _userManager.Users.Where(u => u.EmployerId == employerId);
        }

        public async Task<IdentityResult> CreateConsultantAsync(
            AddConsultantViewModel addConsultantViewModel,
            string employerId)
        {

            var user = new User
            {
                UserName = addConsultantViewModel.Email,
                Email = addConsultantViewModel.Email,
                FirstName = addConsultantViewModel.FirstName,
                Surname = addConsultantViewModel.Surname,
                EmployerId = employerId
            };

            var result = await _userManager.CreateAsync(user, addConsultantViewModel.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, UserRoleEnum.Consultant.ToString());
                await _userManager.UpdateAsync(user);
            }

            return result;
        }
    }
}
