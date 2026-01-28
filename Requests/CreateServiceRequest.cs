using CivicCare.Api.Data;
using CivicCare.Api.Dtos;
using MediatR;

namespace CivicCare.Api.Requests
{
    // Input fields for creating a service request
    public class CreateServiceRequest : IRequest<CommonResponseDto<int>>
    {
        public string Category { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public int CreatedById { get; set; }
        public int? AssignedDepartmentId { get; set; }
    }

    // Handler for processing the creation of a service request
    public class CreateServiceRequestHandler : IRequestHandler<CreateServiceRequest, CommonResponseDto<int>>
    {
        private readonly CivicCareDbContext _db;
        private readonly ILogger<CreateServiceRequestHandler> _logger;

        public CreateServiceRequestHandler(CivicCareDbContext db, ILogger<CreateServiceRequestHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<CommonResponseDto<int>> Handle(CreateServiceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                // Create a new service request entity
                var serviceRequest = new ServiceRequest
                {
                    Category = request.Category,
                    Description = request.Description,
                    Location = request.Location,
                    CreatedById = request.CreatedById,
                    AssignedDepartmentId = request.AssignedDepartmentId,
                    Status = "Open", // Default status
                    CreatedAt = DateTime.UtcNow
                };

                // Add the service request to the database
                _db.ServiceRequests.Add(serviceRequest);
                await _db.SaveChangesAsync(cancellationToken);

                // Log the creation of the service request
                _logger.LogInformation("Service request created with ID {Id}", serviceRequest.Id);

                // Return a success response
                return new CommonResponseDto<int>
                {
                    Message = "Request created successfully",
                    Status = "success",
                    Data = serviceRequest.Id
                };
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, "Error creating service request");

                // Return an error response
                return new CommonResponseDto<int>
                {
                    Message = "Failed to create request",
                    Status = "error",
                    Error = ex.Message
                };
            }
        }
    }
}