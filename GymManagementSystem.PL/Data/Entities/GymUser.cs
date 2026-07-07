using GymManagementSystem.DAL.Data.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.DAL.Data.Entities
{
    public abstract class GymUser : BaseEntity
    {
        [Required,MaxLength(50)]
        public string Name { get; set; } = default!;
        [Required,EmailAddress,MaxLength(100)]
        public string Email { get; set; } = default!;
        [Required,Phone,MaxLength(11)]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Invalid phone number format.")]
        public string Phone { get; set; } = default!;
        public DateOnly DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public Address Address { get; set; } = default!;
    }

    [Owned]
    public class Address
    {
        public int BuildingNumber { get; set; }
        [Required,MaxLength(30)]
        public string City { get; set; } = default!;
        [Required,MaxLength(50)]
        public string Street { get; set; } = default!;

    }
}
