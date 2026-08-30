using Microsoft.AspNetCore.Identity;



namespace GymSystem.DAL.Entities
{
    public class ApplicationUser: IdentityUser
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;

        public int? MemberId { get; set; }
        public Member? Member { get; set; }
    }
}
