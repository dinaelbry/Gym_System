using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading; 
using System.Threading.Tasks;
using GymSystem.DAL.Entities;

namespace GymSystem.DAL.Repository.Interfaces
{
    public interface IBookingRepository: IGenericRepository<Booking>
    {
        public Task<List<Booking>> GetBySessionIdAsync(int sessionId,CancellationToken ct = default);
    }
}
