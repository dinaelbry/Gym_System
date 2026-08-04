using GymSystem.BLL.Common;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.BookingViewModels;
using GymSystem.BLL.ViewModels.MembershipViewModels;
using GymSystem.DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Classes
{
    public class BookingService: IBookingService
    {
        private readonly IUnitOfWork unitOfWork;
        public BookingService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        //public async Task<Result> CancelBookingAsync(int memberId, int sessionId, CancellationToken ct = default)
        //{
        //    var session = await unitOfWork.SessionRepository.GetById(sessionId);
        //    if (session is null)
        //    {
        //        return Result.NotFound("Session not found.");
        //    }

        //    if (session.StartDate <= DateTime.Now)
        //    {
        //        return Result.Fail("Cannot cancel booking for a session that has already started.");
        //    }

        //    var booking = await unitOfWork.BookingRepository.FirstOrDefaultAsync(b => b.SessionId == sessionId && b.MemberId == memberId, tracking: true, ct: ct);
        //    if (booking is null)
        //    {
        //        return Result.NotFound("Booking not found.");
        //    }
        //    unitOfWork.BookingRepository.Delete(booking);
        //    var result = await unitOfWork.SaveChangesAsync(ct);
        //    return result > 0 ? Result.Ok() : Result.Fail("Failed to cancel booking.");

        //}

        public Task<Result> CreateNewBookingAsync(CreateBookingViewModel model, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MemberSelectListViewModel>> GetMembersForDropDownAsync(int sessionId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MemberForSessionViewModel>> GetMembersForOngoingBySessionIdAsync(int sessionId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MemberForSessionViewModel>> GetMembersForUpcomingBySessionIdAsync(int sessionId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result> MarkAttendedAsync(int memberId, int sessionId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
