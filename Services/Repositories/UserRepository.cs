using Microsoft.AspNetCore.Identity;
using OnlineConsulting.Models.Entities;
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
    }
}
