using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Barracuda.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Barracuda.Application
{
    public class DbContextSeed
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, BarracudaDbContext context)
        {
            // TODO: Only run this if using a real database
            if (!context.Database.EnsureCreated())
            {
                context.Database.Migrate();
            }

            await SeedRoles(roleManager);
            await SeedUsers(userManager, context);
        }

        private static async Task SeedUsers(UserManager<ApplicationUser> userManager, BarracudaDbContext context)
        {
            if (!userManager.Users.Any())
            {
                var playerList = new List<ApplicationUser>
                {
                    new ApplicationUser()
                    {
                        UserName = "Abdurrahman",
                        Email = "isik.abdurrahmann@gmail.com",
                        FirstName = "Abdurrahman",
                        LastName = "Işık",
                        CreateDate = DateTime.UtcNow
                    }
                };

                playerList.ForEach(player =>
                {
                    player.PasswordHash = CreatePasswordHash(player, "admin");
                    
                    var identityResult = userManager.CreateAsync(player).Result;
                    if (identityResult.Succeeded)
                    {
                        userManager.AddToRoleAsync(player, "Admin");
                    }
                });

                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                var roleList = new List<IdentityRole>
                {
                    new IdentityRole {Name = "Admin"},
                    new IdentityRole {Name = "User"}
                };

                foreach (var role in roleList)
                {
                    await roleManager.CreateAsync(role);
                }
            }
        }

        #region Utilities

        private static string CreatePasswordHash(ApplicationUser user, string password)
        {
            var passwordHasher = new PasswordHasher<ApplicationUser>();
            var hashedPassword = passwordHasher.HashPassword(user, password);
            return hashedPassword;
        }

        #endregion Utilities
    }
}