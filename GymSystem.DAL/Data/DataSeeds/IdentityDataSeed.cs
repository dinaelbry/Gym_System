using GymSystem.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Data.DataSeeds
{
    public static class IdentityDataSeed
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ILogger logger, CancellationToken ct = default)
        {
            try
            {
                bool HasUsers = userManager.Users.Any();
                bool HasRoles = roleManager.Roles.Any();

                if (HasUsers && HasRoles) return;

                if (!HasRoles) 
                {
                     var roles = new List<IdentityRole>()
                    {
                        new IdentityRole { Name = "SuperAdmin" },
                        new IdentityRole { Name = "Admin" },
                        new IdentityRole {Name = "Receptionist" },
                        new IdentityRole {Name = "Member"}
                    };

                    foreach (var roleName in roles.Select(r => r.Name))
                    {
                        if (!await roleManager.RoleExistsAsync(roleName!))
                        {
                            var roleResult = await roleManager.CreateAsync(new IdentityRole { Name = roleName });
                            if (!roleResult.Succeeded)
                            {
                                logger.LogError($"Error creating role '{roleName}': {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                            }
                        }

                    }
                }

                if (!HasUsers)
                {
                    var Mainusers = new ApplicationUser()
                    {
                        FirstName = "Dina",
                        LastName = "Elbry",
                        UserName = "DinaElbry",
                        Email = "dinaelbry@gmail.com",
                        PhoneNumber = "01556663526",
                    };
                    var UserResult = await userManager.CreateAsync(Mainusers, "P@ssw0rd");

                    if (!UserResult.Succeeded)
                    {
                        logger.LogError($"Error creating user '{Mainusers.UserName}': {string.Join(", ", UserResult.Errors.Select(e => e.Description))}");
                        return;
                    }

                   var RoleResult = await userManager.AddToRoleAsync(Mainusers, "SuperAdmin");
                    if (!RoleResult.Succeeded)
                    {
                        logger.LogError($"Error assigning role: {string.Join(", ", RoleResult.Errors.Select(e => e.Description))}");
                    }

                }
                return;
            }
            catch (Exception )
            {
                logger.LogError("Failed to seed identity data");
                throw;
            }
        }
    }
}
