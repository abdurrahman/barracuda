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
            try
            {
                // We're using command-type migration with dotnet ef tool.
                // If not needed migration history then reopen this.
                if (!await context.Database.EnsureCreatedAsync())
                {
                    await context.Database.MigrateAsync();
                }

                // await SeedRoles(roleManager);
                // await SeedUsers(userManager, context);
                //
                // if ((await context.Database.GetPendingMigrationsAsync()).Any())
                // {
                //     await context.Database.MigrateAsync();
                // }

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        private static async Task SeedUsers(UserManager<ApplicationUser> userManager, BarracudaDbContext context)
        {
            if (userManager.Users.Any()) return;
            var playerList = new List<ApplicationUser>
            {
                new ApplicationUser()
                {
                    UserName = "Quentin",
                    Email = "quentin@tarantino.com",
                    FirstName = "Quentin",
                    LastName = "Tarantino",
                    CreateDate = DateTime.UtcNow
                },
                new ApplicationUser()
                {
                    UserName = "Martin",
                    Email = "martin@scorsese.com",
                    FirstName = "Martin",
                    LastName = "Scorsese",
                    CreateDate = DateTime.UtcNow
                },
                new ApplicationUser()
                {
                    UserName = "David",
                    Email = "david@fincher.com",
                    FirstName = "David",
                    LastName = "Fincher",
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

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (roleManager.Roles.Any()) return;
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