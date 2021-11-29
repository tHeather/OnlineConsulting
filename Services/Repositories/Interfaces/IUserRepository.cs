using Microsoft.AspNetCore.Identity;
using OnlineConsulting.Models.Entities;
using OnlineConsulting.Models.ValueObjects.Users;
using OnlineConsulting.Models.ViewModels.Consultant;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineConsulting.Services.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public User GetEmployerForConsultant(string ConsultantId);
        public User GetUserById(string id);
        public IQueryable<User> GetUserByEmailQuery(string email);
        public IQueryable<User> GetAllConsultantsForEmployerQuery(string employerId);
        public IQueryable<User> GetAllUsersWithRoleQuery(string userRole);
        public Task<CreateConsultant> CreateConsultantAsync(AddConsultantViewModel addConsultantViewModel, string employerId);
        public Task LockEmployerWithEmployeesAsync(string employerId, bool isLocked);
        public Task<IdentityResult> DeleteConsultantAsync(User user);
        public Task<User> CreateEmployerAsync(string email, string firstName, string surname, string password);
        public Task<ResetPasswordResult> RestUserPasswordAsync(string userId, string password);
        public IQueryable<UserWithSubscription> GetUsersWithSubscriptionQuery(IQueryable<User> source);
        public string GetUserRole(string userId);
    }
}
