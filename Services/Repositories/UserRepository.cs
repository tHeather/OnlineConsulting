﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using OnlineConsulting.Models.Entities;
using OnlineConsulting.Models.Enums;
using OnlineConsulting.Models.ViewModels.Consultant;
using OnlineConsulting.Services.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineConsulting.Tools;
using OnlineConsulting.Models.ValueObjects.User;

namespace OnlineConsulting.Services.Repositories
{
    public class UserRepository:IUserRepository
    {    
        private readonly UserManager<User> _userManager;
        private readonly IOptions<IdentityOptions> _identityOptions;

        public UserRepository(UserManager<User> userManager,
            IOptions<IdentityOptions> identityOptions)
        {
            _userManager = userManager;
            _identityOptions = identityOptions;
        }

        public IQueryable<User> GetAllConsultantsForEmployer(string employerId) {
            return _userManager.Users.Where(u => u.EmployerId == employerId);
        }

        public async Task<CreateConsultantValueObject> CreateConsultantAsync(
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

            var passwordOptions = _identityOptions.Value.Password;
            using var randomPasswordGenerator = new RandomPasswordGenerator(passwordOptions);
            var generatedPassword = randomPasswordGenerator.Generate();

            var result = await _userManager.CreateAsync(user, generatedPassword);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, UserRoleEnum.Consultant.ToString());
                await _userManager.UpdateAsync(user);
            }


            return new CreateConsultantValueObject { IdentityResult = result , GeneratedPassword = generatedPassword };
        }
    }
}
