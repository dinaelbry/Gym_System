using GymSystem.DAL.Data.Contexts;
using GymSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GymSystem.DAL.Data.DataSeeds
{
    public static class GymDbDataSeed
    {
        public static async Task SeedAsync(GymDbContext dbcontext, string SeedFilesPath, ILogger logger, CancellationToken ct = default)
        {
            try
            {
                if (!await dbcontext.Plans.AnyAsync(ct))
                {
                    var plans = LoadDataFromJsonFile<Plan>("Plans.json", SeedFilesPath);
                    if (plans.Count > 0)
                    {
                        dbcontext.Plans.AddRange(plans);
                        logger.LogInformation($"Seeding {plans.Count} Plans data...");
                    }
                }

                if (dbcontext.ChangeTracker.HasChanges())
                {
                    await dbcontext.SaveChangesAsync(ct);
                }
             }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
                throw;

            }
        }

        private static List<T> LoadDataFromJsonFile<T>(string fileName,string FolderPath)
        {
            string filePath = Path.Combine(FolderPath, fileName);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file '{filePath}' was not found.");
            }

            var Data = File.ReadAllText(filePath);
            var Options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            Options.Converters.Add(new JsonStringEnumConverter());

            return JsonSerializer.Deserialize<List<T>>(Data, Options) ?? [];
        }

    }
}
