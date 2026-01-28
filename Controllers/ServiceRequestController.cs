using CivicCare.Application.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/requests")]
public class ServiceRequestsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ServiceRequestsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // ===================== CITIZEN =====================
    [HttpPost]
    [Authorize(Roles = "Citizen")]
    public async Task<IActionResult> Create(
        CreateServiceRequest command)
    {
        command.CreatedById = int.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        return Ok(await _mediator.Send(command));
    }

    //// ===================== ADMIN =====================
    //[HttpPut("{id}/assign")]
    //[Authorize(Roles = "Admin")]
    //public async Task<IActionResult> Assign(
    //    int id,
    //    AssignServiceRequestCommand command)
    //{
    //    command.ServiceRequestId = id;
    //    return Ok(await _mediator.Send(command));
    //}

    //// ===================== OFFICER =====================
    //[HttpPut("{id}/status")]
    //[Authorize(Roles = "Officer")]
    //public async Task<IActionResult> UpdateStatus(
    //    int id,
    //    UpdateServiceRequestStatusCommand command)
    //{
    //    command.ServiceRequestId = id;
    //    command.UpdatedByUserId = int.Parse(
    //        User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    //    return Ok(await _mediator.Send(command));
    //}
}