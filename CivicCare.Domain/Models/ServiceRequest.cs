namespace CivicCare.Domain.Models
{
    public class ServiceRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? CategoryId { get; set; } 
        public string Status { get; set; } 
        public int CreatedById { get; set; } 
        public int? AssignedDepartmentId { get; set; } 
        public string Location { get; set; } 
        public int? AssignedToUserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string? ResolutionComment { get; set; }
    }
}
