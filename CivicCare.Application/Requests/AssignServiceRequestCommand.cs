using CivicCare.Application.Contracts;
using CivicCare.Application.Dtos;
using MediatR;

namespace CivicCare.Application.Requests
{
    public class AssignServiceRequestCommand
        : IRequest<CommonResponseDto<string>>
    {
        public int ServiceRequestId { get; set; }
        public int AssignedToUserId { get; set; }
    }

    public class AssignServiceRequestHandler
        : IRequestHandler<AssignServiceRequestCommand, CommonResponseDto<string>>
    {
        private readonly IApplicationDbContext _context;

        public AssignServiceRequestHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CommonResponseDto<string>> Handle(
            AssignServiceRequestCommand request,
            CancellationToken cancellationToken)
        {
            var serviceRequest = await _context.ServiceRequests.FindAsync(
                request.ServiceRequestId);

            serviceRequest!.AssignedToUserId = request.AssignedToUserId;
            serviceRequest.Status = "Assigned";
            serviceRequest.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return new CommonResponseDto<string>
            {
                Status = "Assigned",
                Message = "Request assigned to officer",
                Data = "Assigned",
                Error = null
            };
        }
    }
}