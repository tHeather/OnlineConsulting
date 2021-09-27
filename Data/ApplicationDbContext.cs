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

            builder.Entity<IdentityRole>().HasData(
                    new IdentityRole
                    {
                        Id = "c319ab1e-f914-4ebb-8ac9-d6da40d88419",
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

            base.OnModelCreating(builder);
        }
    }
}
