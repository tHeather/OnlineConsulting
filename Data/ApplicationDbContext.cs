using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineConsulting.Constants;
using OnlineConsulting.Models.Entities;
using System;

namespace OnlineConsulting.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<SubscriptionType> SubscriptionTypes { get; set; }
        public DbSet<Payment> Payments { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            const string ADMIN_EMAIL = "admin@admin.pl";
            const string ADMIN_GUID = "7f10381c-7a7c-4e65-b468-a5b32c789720";
            const string ADMIN_ROLE_GUID = "c319ab1e-f914-4ebb-8ac9-d6da40d88419";

            builder.Entity<IdentityRole>().HasData(
                    new IdentityRole
                    {
                        Id = ADMIN_ROLE_GUID,
                        Name = UserRoleValue.ADMIN,
                        NormalizedName = UserRoleValue.ADMIN.ToUpper()
                    }
                );

            builder.Entity<IdentityRole>().HasData(
                    new IdentityRole
                    {
                        Id = "51802d91-7fa7-436c-9873-a201c8a35bfb",
                        Name = UserRoleValue.EMPLOYER,
                        NormalizedName = UserRoleValue.EMPLOYER.ToUpper()
                    }
                );

            builder.Entity<IdentityRole>().HasData(
                    new IdentityRole
                    {
                        Id = "e1dbd6ec-4d0e-4f0a-bd9f-125cb168ff42",
                        Name = UserRoleValue.CONSULTANT,
                        NormalizedName = UserRoleValue.CONSULTANT.ToUpper()
                    }
                );

            builder.Entity<SubscriptionType>().HasData(
                  new SubscriptionType
                  {
                      Id = new Guid("c258fa62-93ce-4eec-a609-10ffbea8b92b"),
                      Name = "Month",
                      Days = 30,
                      Price = 100
                  }
              );

            builder.Entity<SubscriptionType>().HasData(
                  new SubscriptionType
                  {
                      Id = new Guid("ebc9c455-fc60-441d-8c3d-6178912cc4c1"),
                      Name = "Quarter",
                      Days = 91,
                      Price = 250
                  }
              );

            builder.Entity<SubscriptionType>().HasData(
                  new SubscriptionType
                  {
                      Id = new Guid("d57c126c-085c-4541-8ce5-1eb3e4b9b04f"),
                      Name = "Half year",
                      Days = 182,
                      Price = 450
                  }
              );

            builder.Entity<SubscriptionType>().HasData(
                  new SubscriptionType
                  {
                      Id = new Guid("815d30cf-1b74-47d5-9829-4afd4363979a"),
                      Name = "Year",
                      Days = 365,
                      Price = 850
                  }
              );


            builder.Entity<User>().HasData(
              new User
              {
                  Id = ADMIN_GUID,
                  Email = ADMIN_EMAIL,
                  NormalizedEmail = ADMIN_EMAIL.ToUpper(),
                  UserName = ADMIN_EMAIL,
                  NormalizedUserName = ADMIN_EMAIL.ToUpper(),
                  PasswordHash = "AQAAAAEAACcQAAAAEOYBeJPoRPDerQ65Eyj6pmLGeMTpwjMPKvtmAKI8bbn0eykfamwp5dlh+h2mlcTyBw==", //Qwerty!2345
                  EmailConfirmed = true,
                  FirstName = "Admin",
                  Surname = "Admin"
              }
          );

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = ADMIN_ROLE_GUID,
                UserId = ADMIN_GUID
            });

            base.OnModelCreating(builder);
        }
    }
}
