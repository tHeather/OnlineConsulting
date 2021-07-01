using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineConsulting.Models.Entities
{
    public class User : IdentityUser
    {

        public string EmployerId { get; set; }

        [ForeignKey("EmployerSetting")]
        public int? EmployerSettingId { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 1)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 1)]
        public string Surname { get; set; }




        public EmployerSetting EmployerSetting { get; set; }

    }
}
