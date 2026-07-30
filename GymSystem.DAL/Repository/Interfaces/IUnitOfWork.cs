using GymSystem.DAL.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        //UnitOfWork.GetRepos<Member>().GetAll();
        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity , new();
        public Task<int> CompleteAsync();
    }
}
