using System.ComponentModel.DataAnnotations;


namespace GymSystem.BLL.ViewModels.MembershipViewModels
{
    public class CreateMembershipViewModel
    {
        [Required(ErrorMessage = "Please choose a member")]
        public int MemberId { get; set; }

        [Required(ErrorMessage = "Please choose a plan")]
        public int PlanId { get; set; }

        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }
    }
}
