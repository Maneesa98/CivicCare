namespace CivicCare.Domain.Models
{
    public class RequestStatusHistory
    {
        public int Id { get; set; }
        public int ServiceRequestId { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
