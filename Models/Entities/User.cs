using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace OnlineConsulting.Models.Entities
{
    public class User : IdentityUser
    {
        public string EmployerId { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 1)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 1)]
        public string Surname { get; set; }

    }
}
