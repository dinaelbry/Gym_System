using GymSystem.DAL.Entities;
using System.Linq.Expressions;

namespace GymSystem.DAL.Repository.Interfaces
{
    public interface ISessionRepository: IGenericRepository<Session>
    {
        Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(Expression<Func<Session, bool>>? predicate = null, CancellationToken ct = default);

        Task<Session?> GetSessionWithTrainerAndCategoryAsync(int sessionId, CancellationToken ct = default);

        Task<int> GetCountOfBookedSlotsAsync(int sessionId, CancellationToken ct = default);
    }
}
