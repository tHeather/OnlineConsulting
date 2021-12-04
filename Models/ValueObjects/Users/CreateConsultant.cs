using Microsoft.AspNetCore.Identity;
using OnlineConsulting.Models.Entities;

namespace OnlineConsulting.Models.ValueObjects.Users
{
    public class CreateConsultant
    {
        public IdentityResult IdentityResult { get; set; }

        public string GeneratedPassword { get; set; }
        public User User { get; set; }
    }
}
