using GymSystem.DAL.Contexts;
using GymSystem.DAL.Entities;
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
    public class PlanRepository : IPlanRepository
    {
        private readonly GymDbContext dbcontext;
        public PlanRepository (GymDbContext _dbContext)
        {
            dbcontext = _dbContext;
        }

        public async Task<IEnumerable<Plan>> GetAllPlans(bool isTracked, CancellationToken ct = default)
        { 
            var palns = isTracked ? dbcontext.Plans : dbcontext.Plans.AsNoTracking();
            return await palns.ToListAsync();
        }
       public async Task<Plan?> GetPlanById(int id, CancellationToken ct = default)
            {
                var plan = await dbcontext.Plans.FirstOrDefaultAsync(p => p.Id == id, ct);
                return plan;

        }

        public void AddPlan(Plan plan)
        {
            dbcontext.Plans.Add(plan);
        }

        public void DeletePlan(Plan id)
        {
            var plan = dbcontext.Plans.FirstOrDefault(p => p.Id == id.Id);
            if (plan is not null) 
            {
                dbcontext.Plans.Remove(plan);
            }
        }
        public void UpdatePlan(Plan plan)
        {
            dbcontext.Plans.Update(plan);
        }  
        public async Task<int> CompleteAsync()
        {
            return await dbcontext.SaveChangesAsync();
        }

    }
  
}
