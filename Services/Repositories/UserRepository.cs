using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using OnlineConsulting.Constants;
using OnlineConsulting.Data;
using OnlineConsulting.Models.Entities;
using OnlineConsulting.Models.ValueObjects.User;
using OnlineConsulting.Models.ViewModels.Consultant;
using OnlineConsulting.Services.Repositories.Interfaces;
using OnlineConsulting.Tools;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineConsulting.Services.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly IOptions<IdentityOptions> _identityOptions;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(
            UserManager<User> userManager,
            IOptions<IdentityOptions> identityOptions,
            ISubscriptionRepository subscriptionRepository,
            ApplicationDbContext applicationDbContext
            )
        {
            _userManager = userManager;
            _identityOptions = identityOptions;
            _subscriptionRepository = subscriptionRepository;
            _dbContext = applicationDbContext;
        }

        public User GetUserById(string id)
        {
            return _userManager.Users.SingleOrDefault(u => u.Id == id);
        }

        public IQueryable<User> GetAllConsultantsForEmployerQuery(string employerId)
        {
            return _userManager.Users.Where(u => u.EmployerId == employerId);
        }

        public async Task<CreateConsultant> CreateConsultantAsync(
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
                await _userManager.AddToRoleAsync(user, UserRoleValue.CONSULTANT);
                await _userManager.UpdateAsync(user);
            }


            return new CreateConsultant { IdentityResult = result, GeneratedPassword = generatedPassword };
        }

        public async Task<IdentityResult> DeleteConsultant(User user)
        {
            return await _userManager.DeleteAsync(user);
        }

        public async Task<User> CreateEmployerAsync(
            string email, string firstName, string surname, string password
            )
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var user = new User
                {
                    UserName = email,
                    Email = email,
                    FirstName = firstName,
                    Surname = surname,
                };

                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, UserRoleValue.EMPLOYER);

                    await _subscriptionRepository.CreateSubscriptionAsync(user.Id);

                    await _userManager.UpdateAsync(user);

                    transaction.Commit();
                    return user;
                }

            }
            catch (Exception)
            {
                transaction.Rollback();
            }

            return null;
        }

    }
}
