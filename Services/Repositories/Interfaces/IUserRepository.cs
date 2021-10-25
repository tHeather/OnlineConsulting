using Microsoft.AspNetCore.Identity;
using OnlineConsulting.Models.Entities;
using OnlineConsulting.Models.ValueObjects.User;
using OnlineConsulting.Models.ViewModels.Consultant;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineConsulting.Services.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public User GetUserById(string id);
        public IQueryable<User> GetAllConsultantsForEmployerQuery(string employerId);

        public Task<CreateConsultant> CreateConsultantAsync(AddConsultantViewModel addConsultantViewModel, string employerId);

        public Task<IdentityResult> DeleteConsultant(User user);
        public Task<User> CreateEmployerAsync(string email, string firstName, string surname, string password);
        public Task<ResetPasswordResult> RestUserPasswordAsync(string userId, string password);
    }
}
