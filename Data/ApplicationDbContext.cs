using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineConsulting.Constants;
using OnlineConsulting.Models.Entities;
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
                        Name = UserRoleValue.ADMIN, 
                        NormalizedName = UserRoleValue.ADMIN.ToUpper() 
                    }
                );

            builder.Entity<IdentityRole>().HasData(
                    new IdentityRole
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = UserRoleValue.EMPLOYER,
                        NormalizedName = UserRoleValue.EMPLOYER.ToUpper()
                    }
                );

            builder.Entity<IdentityRole>().HasData(
                    new IdentityRole
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = UserRoleValue.CONSULTANT,
                        NormalizedName = UserRoleValue.CONSULTANT.ToUpper()
                    }
                );

            base.OnModelCreating(builder);
        }
    }
}
