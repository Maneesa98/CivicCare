using CivicCare.Application.Contracts;
using CivicCare.Application.Dtos;
using CivicCare.Domain.Models;
using MediatR;

namespace CivicCare.Application.Requests
{
    public class CreateServiceRequest
        : IRequest<CommonResponseDto<int>>
    {
        public int CategoryId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int AssignedDepartmentId { get; set; }

        public int CreatedById { get; set; } // from JWT
    }

    public class CreateServiceRequestHandler
        : IRequestHandler<CreateServiceRequest, CommonResponseDto<int>>
    {
        private readonly IApplicationDbContext _context;

        public CreateServiceRequestHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CommonResponseDto<int>> Handle(
            CreateServiceRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                var entity = new ServiceRequest
                {
                    CategoryId = request.CategoryId,
                    Title = request.Title,
                    Description = request.Description,
                    Location = request.Location,
                    AssignedDepartmentId = request.AssignedDepartmentId,
                    CreatedById = request.CreatedById,
                    Status = "Submitted",
                    CreatedAt = DateTime.UtcNow
                };

                _context.ServiceRequests.Add(entity);
                await _context.SaveChangesAsync(cancellationToken);

                return new CommonResponseDto<int>
                {
                    Status = "success",
                    Message = "Request created successfully",
                    Data = entity.Id
                };
            }
            catch (Exception ex)
            {
                return new CommonResponseDto<int>
                {
                    Status = "error",
                    Message = "Failed to create Request.",
                    Error = ex.InnerException?.Message ?? ex.Message
                };
            }
            }
            
        }
    }