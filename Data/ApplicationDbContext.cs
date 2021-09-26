using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineConsulting.Constants;
using OnlineConsulting.Models.Entities;

namespace OnlineConsulting.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
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

            base.OnModelCreating(builder);
        }
    }
}
