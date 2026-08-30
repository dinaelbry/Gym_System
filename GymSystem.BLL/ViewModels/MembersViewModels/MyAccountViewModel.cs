using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.BLL.ViewModels.MembersViewModels
{
    public class MyAccountViewModel
    {
        // personal info
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public string? Photo { get; set; }
        public string Address { get; set; } = default!;


        public string? PlanName { get; set; }
        public string? MembershipStartDate { get; set; }
        public string? MembershipEndDate { get; set; }
        public int? DaysRemaining { get; set; } // calc
        public bool HasActiveMembership { get; set; } 

        // sessions
        public int AttendedSessionsCount { get; set; }
        public List<MyBookingViewModel> UpcomingSessions { get; set; } = new();

    }

    public class MyBookingViewModel
    {
        public string SessionCategory { get; set; } = default!;
        public string TrainerName { get; set; } = default!;
        public string StartDate { get; set; } = default!;
    }

}
