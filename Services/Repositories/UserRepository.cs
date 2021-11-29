using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using OnlineConsulting.Constants;
using OnlineConsulting.Data;
using OnlineConsulting.Models.Entities;
using OnlineConsulting.Models.ValueObjects.Users;
using OnlineConsulting.Models.ViewModels.Consultant;
using OnlineConsulting.Services.Repositories.Interfaces;
using OnlineConsulting.Tools;
using System;
using System.Collections.Generic;
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

        public User GetEmployerForConsultant(string ConsultantId)
        {
            var employerId = _dbContext.Users.Where(u => u.Id == ConsultantId)
                                             .Select(u => u.EmployerId)
                                             .SingleOrDefault();

            return GetUserById(employerId);
        }

        public string GetUserRole(string userId)
        {
            return _dbContext.UserRoles
                    .Where(r => r.UserId == userId)
                    .Join(
                            _dbContext.Roles,
                            userRole => userRole.RoleId,
                            role => role.Id,
                            (userRole, role) => role.Name
                          )
                    .FirstOrDefault();
        }

        public User GetUserById(string id)
        {
            return _dbContext.Users.SingleOrDefault(u => u.Id == id);
        }

        public IQueryable<User> GetUserByEmailQuery(string email)
        {
            return _dbContext.Users.Where(u => u.Email == email);
        }

        public IQueryable<User> GetAllConsultantsForEmployerQuery(string employerId)
        {
            return _dbContext.Users.Where(u => u.EmployerId == employerId);
        }

        public IQueryable<User> GetAllUsersWithRoleQuery(string userRole)
        {

            return _dbContext.UserRoles
                                .Join(
                                        _dbContext.Users,
                                        role => role.UserId,
                                        user => user.Id,
                                        (role, user) =>
                                        new { User = user,  role.RoleId }
                                    )
                                .Join(
                                        _dbContext.Roles,
                                        user => user.RoleId,
                                        role => role.Id,
                                        (user, role) =>
                                        new {  user.User, Role = role.Name }
                                      )
                                .Where(u => u.Role ==userRole)
                                .Select(u => u.User);
        }

        public IQueryable<UserWithSubscription> GetUsersWithSubscriptionQuery(IQueryable<User> source)
        {
            return _dbContext.Subscriptions.Join(
                                            source,
                                            sub => sub.EmployerId,
                                            user => user.Id,
                                            (sub, user) => new UserWithSubscription
                                            {
                                                User = user,
                                                Subscription = sub
                                            }
                                            );
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

        public async Task<IdentityResult> DeleteConsultantAsync(User user)
        {
            return await _userManager.DeleteAsync(user);
        }

        public async Task LockEmployerWithEmployeesAsync(string employerId, bool isLocked)
        {
          var employer = GetUserById(employerId);
          if (employer == null) return;
          employer.IsAccountLocked = isLocked;

         var employees = GetAllConsultantsForEmployerQuery(employerId).ToList();
         employees.ForEach(e => e.IsAccountLocked = isLocked);
    
         await _dbContext.SaveChangesAsync();
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

        public async Task<ResetPasswordResult> RestUserPasswordAsync(string userId, string password)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ResetPasswordResult()
                {
                    IsSucceed = false,
                    Errors = new List<string> { "Can't find user." }
                };
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, code, password);

            return new ResetPasswordResult()
            {
                IsSucceed = result.Succeeded,
                Errors = result.Errors.Select(e => e.Description).ToList()

            };
        }
    }
}
