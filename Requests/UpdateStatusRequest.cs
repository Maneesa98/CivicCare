using CivicCare.Api.Data;
using CivicCare.Api.Dtos;
using CivicCare.Api.Models;
using MediatR;

namespace CivicCare.Api.Requests
{
    public class UpdateStatusRequest : IRequest<CommonResponseDto<int>>
    {
        public int ServiceRequestId { get; set; }
        public string Status { get; set; }
    }

    public class UpdateStatusRequestHandler : IRequestHandler<UpdateStatusRequest, CommonResponseDto<int>>
    {
        private readonly CivicCareDbContext _db;

        public UpdateStatusRequestHandler(CivicCareDbContext db)
        {
            _db = db;
        }

        public async Task<CommonResponseDto<int>> Handle(UpdateStatusRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var serviceRequest = await _db.ServiceRequests.FindAsync(new object[] { request.ServiceRequestId }, cancellationToken);
                if (serviceRequest == null)
                {
                    return new CommonResponseDto<int>
                    {
                        Message = "Service request not found",
                        Status = "error",
                        Error = "The specified service request does not exist"
                    };
                }

                serviceRequest.Status = request.Status;

                _db.StatusHistory.Add(new RequestStatusHistory
                {
                    ServiceRequestId = request.ServiceRequestId,
                    Status = request.Status
                });

                await _db.SaveChangesAsync(cancellationToken);

                return new CommonResponseDto<int>
                {
                    Message = "Status updated successfully",
                    Status = "success",
                    Data = serviceRequest.Id
                };
            }
            catch (Exception ex)
            {
                return new CommonResponseDto<int>
                {
                    Message = "Failed to update status",
                    Status = "error",
                    Error = ex.Message
                };
            }
        }
    }
}