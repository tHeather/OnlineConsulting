using Microsoft.AspNetCore.Identity;

namespace OnlineConsulting.Models.ValueObjects.Users
{
    public class CreateConsultant
    {
        public IdentityResult IdentityResult { get; set; }

        public string GeneratedPassword { get; set; }
    }
}
