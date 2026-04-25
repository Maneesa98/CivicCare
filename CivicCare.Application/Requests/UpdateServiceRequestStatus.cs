using CivicCare.Application.Contracts;
using CivicCare.Application.Dtos;
using MediatR;

namespace CivicCare.Application.Requests
{
    public class UpdateServiceRequestStatusCommand
        : IRequest<CommonResponseDto<string>>
    {
        public int ServiceRequestId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string ResolutionComment { get; set; } = string.Empty;

        public int UpdatedByUserId { get; set; }
    }

    public class UpdateServiceRequestStatusHandler
        : IRequestHandler<UpdateServiceRequestStatusCommand, CommonResponseDto<string>>
    {
        private readonly IApplicationDbContext _context;

        public UpdateServiceRequestStatusHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CommonResponseDto<string>> Handle(
            UpdateServiceRequestStatusCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await _context.ServiceRequests.FindAsync(
                request.ServiceRequestId);

            entity!.Status = request.Status;
            entity.ResolutionComment = request.ResolutionComment;
            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return new CommonResponseDto<string>
            {
                Status = "StatusUpdated",
                Message = "Status updated successfully",
                Data = "StatusUpdated",
                Error = null
            };
        }
    }
}