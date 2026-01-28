namespace CivicCare.Api.Models
{
    public class ServiceRequest
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }

        public string Status { get; set; } 

        public int CreatedById { get; set; }
        public User CreatedBy { get; set; }

        public int? AssignedDepartmentId { get; set; }
        public Department AssignedDepartment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
