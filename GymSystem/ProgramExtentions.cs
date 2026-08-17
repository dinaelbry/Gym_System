using GymSystem.DAL.Data.Contexts;
using GymSystem.DAL.Data.DataSeeds;
using GymSystem.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymSystem
{
    public static class ProgramExtentions
    {
        public static async Task MigrationAndSeedAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbcontext = scope.ServiceProvider.GetRequiredService<GymDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            //Identity Database Migration
            var RoleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // pending Migration
            var Pending = await dbcontext.Database.GetPendingMigrationsAsync();
            if (Pending.Any())
            {
                logger.LogInformation($"Apply {Pending.Count()} Pending Migration ...");
                await dbcontext.Database.MigrateAsync();    
            }

            var SeedPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "Files");


            await GymDbDataSeed.SeedAsync(dbcontext, SeedPath, logger);
            await IdentityDataSeed.SeedAsync(RoleManager, UserManager, logger);
        }
    }
}
