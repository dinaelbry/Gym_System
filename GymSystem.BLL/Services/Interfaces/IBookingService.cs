using GymSystem.BLL.Common;
using GymSystem.BLL.ViewModels.BookingViewModels;
using GymSystem.BLL.ViewModels.MembershipViewModels;
//using GymSystem.BLL.ViewModels.SessionViewModels;

namespace GymSystem.BLL.Services.Interfaces
{
    public interface IBookingService
    {
        //Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct);
        Task<IEnumerable<MemberForSessionViewModel>> GetMembersForUpcomingBySessionIdAsync(int sessionId, CancellationToken ct = default);
        Task<IEnumerable<MemberForSessionViewModel>> GetMembersForOngoingBySessionIdAsync(int sessionId, CancellationToken ct = default);
        Task<IEnumerable<MemberSelectListViewModel>> GetMembersForDropDownAsync(int sessionId, CancellationToken ct = default);

        Task<Result> CreateNewBookingAsync(CreateBookingViewModel model, CancellationToken ct = default);
        //Task<Result> CancelBookingAsync(int memberId, int sessionId, CancellationToken ct = default);
        Task<Result> MarkAttendedAsync(int memberId, int sessionId, CancellationToken ct = default);
    }
}
