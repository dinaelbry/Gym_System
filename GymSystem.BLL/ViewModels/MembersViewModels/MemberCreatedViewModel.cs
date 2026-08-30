using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.BLL.ViewModels.MembersViewModels
{
    public class MemberCreatedViewModel
    {
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string TemporaryPassword { get; set; } = default!;
    }
}
