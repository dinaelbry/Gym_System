namespace MVC_Sessions.Models
{
    public class Plan
    {
        public int Id { set; get; }
        public DateTime CreatedAt { set; get; }
        public DateTime UpdatedAt { set; get; }
        public string Name { set; get; } = default!;
        public string Description { set; get; } = default!;
        public decimal Price { set; get; }
        public int Duration { set; get; }
        public bool IsActive { set; get; }

    }
}
