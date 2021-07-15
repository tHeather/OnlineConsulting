using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineConsulting.Models.ValueObjects.User
{
    public class CreateConsultant
    {
      public  IdentityResult IdentityResult { get; set; }
         
       public string GeneratedPassword { get; set; }
    }
}
