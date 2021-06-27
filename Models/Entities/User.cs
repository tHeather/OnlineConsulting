using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineConsulting.Models.Entities
{
    public class User : IdentityUser
    {
    
        public int EmployerId { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public int EmployerSettingsId { get; set; }

    }
}
