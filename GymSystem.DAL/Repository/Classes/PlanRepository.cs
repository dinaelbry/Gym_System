using GymSystem.DAL.Data.Contexts;
using GymSystem.DAL.Data.Entities;
using GymSystem.DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repository.Classes
{
    public class PlanRepository : IGenericRepository
    {
        private readonly GymDbContext dbcontext;
        public PlanRepository (GymDbContext _dbContext)
        {
            dbcontext = _dbContext;
        }

        public async Task<IEnumerable<Plan>> GetAll(bool isTracked, CancellationToken ct = default)
        { 
            var palns = isTracked ? dbcontext.Plans : dbcontext.Plans.AsNoTracking();
            return await palns.ToListAsync();
        }
       public async Task<Plan?> GetById(int id, CancellationToken ct = default)
            {
                var plan = await dbcontext.Plans.FirstOrDefaultAsync(p => p.Id == id, ct);
                return plan;

        }

        public void Add(Plan plan)
        {
            dbcontext.Plans.Add(plan);
        }

        public void Delete(Plan id)
        {
            var plan = dbcontext.Plans.FirstOrDefault(p => p.Id == id.Id);
            if (plan is not null) 
            {
                dbcontext.Plans.Remove(plan);
            }
        }
        public void Update(Plan plan)
        {
            dbcontext.Plans.Update(plan);
        }  
        public async Task<int> CompleteAsync()
        {
            return await dbcontext.SaveChangesAsync();
        }

    }
  
}
