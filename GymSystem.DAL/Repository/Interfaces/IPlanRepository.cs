//using GymSystem.DAL.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace GymSystem.DAL.Repository.Interfaces
//{
//    public interface IGenericRepository
//    {
//        Task<IEnumerable<Plan>> GetAll(bool isTracked, CancellationToken ct = default);
//        Task<Plan?> GetById(int id, CancellationToken ct=default);
//        void Add(Plan plan);
//        void Update(Plan plan);
//        void Delete(Plan id);
//        Task<int> CompleteAsync();
//    }
//}
