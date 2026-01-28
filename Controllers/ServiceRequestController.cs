using CivicCare.Api.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CivicCare.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/requests")]
    public class ServiceRequestController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ServiceRequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Citizen")]
        public async Task<IActionResult> Create(CreateServiceRequest command)
        {
            var result = await _mediator.Send(command);

            if (result.Status == "success")
            {
                return Ok(new
                {
                    message = result.Message,
                    status = result.Status,
                    id = result.Data
                });
            }

            return BadRequest(new
            {
                message = result.Message,
                status = result.Status,
                error = result.Error
            });
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Officer,Admin")]
        public async Task<IActionResult> UpdateStatus(int id, UpdateStatusRequest command)
        {
            command.ServiceRequestId = id;
            var result = await _mediator.Send(command);

            if (result.Status == "success")
            {
                return Ok(new
                {
                    message = result.Message,
                    status = result.Status,
                    id = result.Data
                });
            }

            return BadRequest(new
            {
                message = result.Message,
                status = result.Status,
                error = result.Error
            });
        }
    }
}