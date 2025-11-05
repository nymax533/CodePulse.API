using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Data
{
    public class AuthDbContext : IdentityDbContext
    {

        public AuthDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "91ec0940-e930-4a83-9a64-6f41ec52909a";
            var writerRoleId = "3c378921-5af9-41cd-be08-47e85ae61e53";
            var roles = new List<IdentityRole> {
                new IdentityRole()
                {
                    Id = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                    ConcurrencyStamp = readerRoleId
                },
                new IdentityRole()
                {
                   Id = writerRoleId,
                   Name = "Writer",
                   NormalizedName = "Writer".ToUpper(),
                   ConcurrencyStamp = writerRoleId
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);

            
            var adminUserId = "76961d6c-7445-4a8b-a930-cae4eee4af11";
            var admin = new IdentityUser()
            {
                Id = adminUserId,
                UserName = "admin@codepulse.com",
                NormalizedUserName = "ADMIN@CODEPULSE.COM",
                NormalizedEmail = "ADMIN@CODEPULSE.COM",
                EmailConfirmed = false,
                LockoutEnabled = false,
                AccessFailedCount = 0
            };

            
            admin.PasswordHash = "AQAAAAIAAYagAAAAEH0nWpktVp25A5Jg2N+3kR//TJFnlomXyDztr/Dsf7krJ51RilD7GudAu4YQOHG8qg==";//Admin@123
            admin.SecurityStamp = "c08b5723-9a95-4241-8cdf-12a9f9b89e2d";
            admin.ConcurrencyStamp = "ba947107-00b8-42d4-b50d-a7d61055f5ed";

            builder.Entity<IdentityUser>().HasData(admin);


            var adminUserRole = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    UserId = adminUserId,
                    RoleId = writerRoleId
                },
                new()
                {
                    UserId = adminUserId,
                    RoleId = readerRoleId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(adminUserRole);
        }
    }
}