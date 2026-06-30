using GymSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repository.Interfaces
{
    public interface IPlanRepository
    {
        Task<IEnumerable<Plan>> GetAllPlans(bool isTracked, CancellationToken ct = default);
        Task<Plan?> GetPlanById(int id, CancellationToken ct=default);
        void AddPlan(Plan plan);
        void UpdatePlan(Plan plan);
        void DeletePlan(Plan id);
        Task<int> CompleteAsync();
    }
}
