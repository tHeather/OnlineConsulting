using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineConsulting.Models.Entities;
using OnlineConsulting.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineConsulting.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {

        public DbSet<EmployerSetting> EmployerSettings { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<IdentityRole>().HasData(
                    new IdentityRole {
                        Id = Guid.NewGuid().ToString(),
                        Name = UserRoleEnum.Admin.ToString(), 
                        NormalizedName = UserRoleEnum.Admin.ToString().ToUpper() 
                    }
                );

            builder.Entity<IdentityRole>().HasData(
                    new IdentityRole
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = UserRoleEnum.Employer.ToString(),
                        NormalizedName = UserRoleEnum.Employer.ToString().ToUpper()
                    }
                );

            builder.Entity<IdentityRole>().HasData(
                    new IdentityRole
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = UserRoleEnum.Consultant.ToString(),
                        NormalizedName = UserRoleEnum.Consultant.ToString().ToUpper()
                    }
                );

            base.OnModelCreating(builder);
        }
    }
}
